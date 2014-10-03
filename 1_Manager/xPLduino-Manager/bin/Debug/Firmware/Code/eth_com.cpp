#include <Eth_com.h>
#include <xPL.h>
#include <EtherCard.h>
#include <Arduino.h>
#include <xPL_Message.h>
#include "config.h"

#include "WebServerSD.h"

#ifdef WITH_LIGHTING
#include "lighting_xpl.h"
#endif

#include "shutter_xpl.h"
#include "switch_xpl.h"
#include "temperature_xpl.h"

#define ETH_COM_VERSION 2

// v1: ajout dhcp/static
// v2: ajout temp

// byte Ethernet::buffer[XPL_MESSAGE_BUFFER_MAX];
xPL xpl;

extern char vendor_id[];    // vendor id
extern char instance_id[];    // instance id


/// déplacé dans customer.ino
//~ #define STATIC 1  // set to 1 to disable DHCP (adjust myip/gwip values below)


// Init du serveur HTTP
//~ #if STATIC
//~ static byte myip[] = { 192,168,1,55 };
//~ static byte gwip[] = { 192,168,1,1 };
//~ #endif


extern byte myip[];
extern byte gwip[];


//~ static byte mymac[] = { 0x62,0x68,0x68,0x1D,0x31,0x35 };
extern byte mymac[];
byte Ethernet::buffer[1200]; // tcp/ip send and receive buffer


// Ethercard seems to only send broadcast on this addr
// If using xpl-perl, don't forget to add "--define broadcast=0.0.0.0" to the xpl-hub
uint8_t broadcast[4] = { 255,255,255,255};

#define SS_PIN 10

void AfterParseAction(xPL_Message * message)
{
    //~ if (xpl.TargetIsMe(message))
    //~ {
        #ifdef WITH_LIGHTING
        if (message->IsSchema_P(PSTR("lighting"), PSTR("basic"))){
            Serial.println(F("new lighting.basic"));
            lighting_basic(message);
        }else
        if (message->IsSchema_P(PSTR("lighting"), PSTR("request"))){
            Serial.println(F("new lighting.request"));
            lighting_request(message);
        }else
        #endif
        if (message->IsSchema_P(PSTR("shutter"), PSTR("basic"))){
            Serial.println(F("new shutter.basic"));
            shutter_basic(message);
        }else
        if (message->IsSchema_P(PSTR("shutter"), PSTR("request"))){
            Serial.println(F("new lighting.request"));
            shutter_request(message);
        }else
        if (message->IsSchema_P(PSTR("switch"), PSTR("request"))){
            Serial.println(F("new switch.request"));
            switch_request(message);
        }else
        if (message->IsSchema_P(PSTR("sensor"), PSTR("request"))){
            Serial.println(F("new sensor.request"));
            sensor_request(message);
        }
    #ifdef DEBUG_INCOMING
    // show message
    Serial.println(F("---------- xPL mess reception -----------"));
    Serial.println(message->toString());
    #endif
}

void SendUdPMessage(char *buffer)
{
    ether.sendUdp (buffer, strlen(buffer), xpl.udp_port, broadcast, xpl.udp_port);
}

boolean isXpl( uint8_t *buffer)
{
  return ( buffer[IP_PROTO_P] == IP_PROTO_UDP_V
            && buffer[UDP_DST_PORT_L_P] == XPL_PORT_L
            && buffer[UDP_DST_PORT_H_P] == XPL_PORT_H);
}

// convert byte* to char*
void ByteToChar(byte* bytes, char* chars, unsigned int count)
{
    for(unsigned int j = 0, i = UDP_DATA_P; i < count; i++)
    {
        chars[j++] = (char)bytes[i];
    }
}

void setup_Udp()
{

    #if WEBSERVER
    Serial.println(F("------------    setup SD   ------------"));
    setup_WebServerSD();
    #endif

    Serial.println(F("------------    setup UDP   ------------"));

    // Serial.print(F("IP="));
    // Serial.print(myip[0]);
    // Serial.print(F("."));
    // Serial.print(myip[1]);
    // Serial.print(F("."));
    // Serial.print(myip[2]);
    // Serial.print(F("."));
    // Serial.println(myip[3]);

    // Serial.print(F("GW="));
    // Serial.print(gwip[0]);
    // Serial.print(F("."));
    // Serial.print(gwip[1]);
    // Serial.print(F("."));
    // Serial.print(gwip[2]);
    // Serial.print(F("."));
    // Serial.println(gwip[3]);

    // Serial.print(F("MAC="));
    // Serial.print(mymac[0], HEX);
    // Serial.print(F("."));
    // Serial.print(mymac[1], HEX);
    // Serial.print(F("."));
    // Serial.print(mymac[2], HEX);
    // Serial.print(F("."));
    // Serial.print(mymac[3], HEX);
    // Serial.print(F("."));
    // Serial.print(mymac[4], HEX);
    // Serial.print(F("."));
    // Serial.println(mymac[5], HEX);

    Serial.print( "Access Ethernet controller...");
    if (ether.begin(sizeof Ethernet::buffer, mymac, SS_PIN) == 0){
        Serial.println( "Failed");}
    else
        Serial.println( "ok");

    #if STATIC
    Serial.print( "static setup...");
    ether.staticSetup(myip, gwip);
    Serial.println( "ok");
    #else
        Serial.print("DHCP request...");
    if (!ether.dhcpSetup())
        Serial.println( "Failed");
    else
        Serial.println( "ok");
    #endif

    ether.printIp("IP:  ", ether.myip);
    ether.printIp("GW:  ", ether.gwip);
    ether.printIp("DNS: ", ether.dnsip);

    Serial.println(F("------------ setup XPL layer ------------"));

    Serial.print(F("vendor_id= '"));
    Serial.print(vendor_id);
    Serial.println(F("'"));

    Serial.print(F("instance_id= '"));
    Serial.print(instance_id);
    Serial.println(F("'"));

  xpl.SendExternal = &SendUdPMessage;  // pointer to the send callback

  xpl.AfterParseAction = &AfterParseAction;  // pointer to a post parsing action callback

  //~ xpl.SetSource_P(PSTR("xplduino"), PSTR("config"), PSTR("test")); // parameters for hearbeat message
  xpl.SetSource_P(PSTR("xplduino"), PSTR("config"), instance_id); // parameters for hearbeat message

    Serial.println(F("----------- setup XPL layer done -----------"));

}

void loop_Udp()
{

   xpl.Process();  // heartbeat management

   word len = ether.packetReceive();
   if(len)
   {

    // Serial.print("receive bytes:");
    // Serial.print(len);

     // Check if Xpl UDP packet
     if( isXpl( Ethernet::buffer ) )
     {
        // Serial.println(" of xpl");
       char xPLMessageBuff[XPL_MESSAGE_BUFFER_MAX];
       ByteToChar(Ethernet::buffer, xPLMessageBuff, len);

       // parse message
       xpl.ParseInputMessage(xPLMessageBuff);
     }
    #if WEBSERVER
     else{ // it's probably a tcp packet
        // Serial.println(" of tcp");
        loop_WebServerSD(len);
     }
    #endif
   }

}

