// works only with tinyfat library
// with SD library packets lost will occurs
// don't know why !
// tested with arduino mega 1280 and uno
// send 240 Megabyte file without packet loss
// at 100 kbyte/s
// tinyfat read a block of 512 bytes on SD card ,
// so the buffer must be 512 + 60 bytes
// on the arduino mega with bigger buffer you can adjust
// if (cur>=512) { // 512 to 1024 with 1100 bytes buffer

// -------------------- Version ---------------------------------------
//ajout Teleinfo
//ajout flag DEBUG_WEB_SERVER
 

#include "WebServerSD.h"

#include <EtherCard.h>

#define TCP_FLAGS_FIN_V 1 //as declared in net.h
#define TCP_FLAGS_ACK_V 16 //as declared in net.h

int buffer_offset=0;
// used as cursor while filling the buffer
static BufferFiller bfill;

unsigned long cur;
byte res=0;

void setup_WebServerSD() {

  // Initialize tinyFAT
  // You might need to select a lower speed than the default SPISPEED_HIGH

  Serial.print("start SD...");
  file.setSSpin(7);
  res=file.initFAT(0);

  if (res==NO_ERROR)
    Serial.println("ok");
    else
    Serial.println("nok");

}

int loop_WebServerSD(int _len)
{
  // word len = ether.packetReceive();
    word pos = ether.packetLoop(_len);
    // Serial.print("LEN=");
    // Serial.println(_len);
    // Serial.print("POS=");
    // Serial.println(pos);
    if (pos) {

        bfill = ether.tcpOffset();
        char* data = (char *) Ethernet::buffer + pos;
        cur=0;

        // Serial.println("data:");
        // Serial.println(data);


        if (checkUrl(F("GET / "), data)) {// nothing specified
          sendfiles("index.htm");
          Serial.println("index.htm");
          }
        // else if (strncmp("GET /l_", data, 7) == 0) { // serve list json 
        else if (checkUrl(F("GET /l_"), data)) { // serve list json 
			// Serial.println("l_");
			if(checkUrl(F("GET /l_light"), data)){ // serve list board
				#ifdef  DEBUG_WEB_SERVER 
					Serial.println("l_light");
				#endif
				listLightJson(bfill);
				ether.httpServerReply(bfill.position()); // send web page data
			}else
			if(checkUrl(F("GET /l_switch"), data)){ // serve list board
				#ifdef  DEBUG_WEB_SERVER 
					Serial.println("l_switch");
				#endif
				listSwitchJson(bfill);
				ether.httpServerReply(bfill.position()); // send web page data
			}
			else
			if(checkUrl(F("GET /l_shutter"), data)){ // serve list board
				#ifdef  DEBUG_WEB_SERVER
					Serial.println("l_shutter");
				#endif
				listShutterJson(bfill);
				ether.httpServerReply(bfill.position()); // send web page data
			}
			else
			if(checkUrl(F("GET /l_teleinfo.jso"), data)){ // serve list board
				#ifdef  DEBUG_WEB_SERVER
					Serial.println("l_teleinfo.jso");
				#endif
				listTeleinfoJson(bfill);
				ether.httpServerReply(bfill.position()); // send web page data
			}
			else
			if(checkUrl(F("GET /l_temp"), data)){ // serve list board
				#ifdef  DEBUG_WEB_SERVER
					Serial.println("l_temp");
				#endif
				listTempJson(bfill);
				ether.httpServerReply(bfill.position()); // send web page data
			}			else
			if(checkUrl(F("GET /l_in8r8.jso"), data)){ // serve list board
				#ifdef  DEBUG_WEB_SERVER	
					Serial.println("l_in8r8");
				#endif
				listIn8R8Json(bfill);
				ether.httpServerReply(bfill.position()); // send web page data
			}			else
			if(checkUrl(F("GET /l_in4dim4.jso"), data)){ // serve list board
				#ifdef  DEBUG_WEB_SERVER
					Serial.println("l_in4dim4");
				#endif
				listIn4dim4Json(bfill);
				ether.httpServerReply(bfill.position()); // send web page data
			}
			else
				not_found(); 
          }
        else if (strncmp("GET /", data, 5) == 0) { // serve anything on sd card
            Serial.println("sd files");
            int i =0;
            char temp[15]=""; // here will be the name of requested file
            while (data[i+5]!=32) {temp[i]=data[i+5];i++;}//search the end
            sendfiles((char*) temp);

          }
        else
            not_found();

        /*  uncomment this if you want to have printed the ip of the target browser*/
        origin();
  }

}


// TeleInfo JSON
#include "TeleInfo_core.h"
extern TeleInfo TeleInfo;

void listTeleinfoJson(BufferFiller& buf) {
  writeHeaders(buf);
  buf.println(F("Content-Type: application/json\r\n"));
  buf.print(F("{\"list\":["));
  if (TeleInfo.pres=true){
        buf.emit_p(PSTR("{\"name\":\"Reference ADCO\",\"val\":\"$S\"}"),TeleInfo.adco);
    buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Option tarifaire\",\"val\":\"$S\"}"),TeleInfo.optarif);
    buf.write(',');
        int temp;

        buf.emit_p(PSTR("{\"name\":\"Intensite souscrite\",\"val\":\"$D A\"}"),TeleInfo.isousc);
    buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Puissance apparente\",\"val\":\"$D VA\"}"),TeleInfo.papp);
     if (TeleInfo.abo<=4) {
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Intensite instantanee\",\"val\":\"$D A\"}"),TeleInfo.iinst);
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Intensite maximale appelee\",\"val\":\"$D A\"}"),TeleInfo.imax);
      }else if (TeleInfo.abo>=5) {
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Intensite instantanee phase 1\",\"val\":\"$D A\"}"),TeleInfo.IINST1);
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Intensite instantanee phase 2\",\"val\":\"$D A\"}"),TeleInfo.IINST2);
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Intensite instantanee phase 3\",\"val\":\"$D A\"}"),TeleInfo.IINST3);
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Intensite maximale phase 1\",\"val\":\"$D A\"}"),TeleInfo.IMAX1);
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Intensite maximale phase 2\",\"val\":\"$D A\"}"),TeleInfo.IMAX2);
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Intensite maximale phase 3\",\"val\":\"$D A\"}"),TeleInfo.IMAX3);
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Presence des potentiels\",\"val\":\"$S\"}"),TeleInfo.PPOT);
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Puissance maximale triphasee\",\"val\":\"$D W\"}"),TeleInfo.PMAX);
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Avertissement de depassement d'intensite phase 1\",\"val\":\"$S\"}"),TeleInfo.ADIR1);
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Avertissement de depassement d'intensite phase 2\",\"val\":\"$S\"}"),TeleInfo.ADIR2);
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Avertissement de depassement d'intensite phase 3\",\"val\":\"$S\"}"),TeleInfo.ADIR3);
      }
      if (TeleInfo.abo==1) {
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Index option base\",\"val\":\"$L Wh\"}"),TeleInfo.base);
      }else if (TeleInfo.abo==2) {      // Abonnement heures creuses
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Heures creuses\",\"val\":\"$L Wh\"}"),TeleInfo.hchc);
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Heures pleines\",\"val\":\"$L Wh\"}"),TeleInfo.hchp);
        /*buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Groupe horaire\",\"val\":\"$S\"}"),TeleInfo.hhphc); */
      }else if (TeleInfo.abo==3) { // abonnement Effacement des jours de pointes (EJP)
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Heures normales\",\"val\":\"$L Wh\"}"),TeleInfo.EJPHN);
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Heures de pointe\",\"val\":\"$L Wh\"}"),TeleInfo.EJPHPM);
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Preavis debut EJP (30min)\",\"val\":\"$S\"}"),TeleInfo.PEJP);
      }else if (TeleInfo.abo==4) { // abonnement Tempo (Bleu,blanc,rouge)
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Heures creuses jours bleus\",\"val\":\"$L Wh\"}"),TeleInfo.BBRHCJB);
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Heures pleines jours bleus\",\"val\":\"$L Wh\"}"),TeleInfo.BBRHPJB);
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Heures creuses jours blancs\",\"val\":\"$L Wh\"}"),TeleInfo.BBRHCJW);
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"HHeures pleines jours blancs\",\"val\":\"$L Wh\"}"),TeleInfo.BBRHPJW);
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Heures creuses jours rouges\",\"val\":\"$L Wh\"}"),TeleInfo.BBRHCJR);
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Heures pleines jours rouges\",\"val\":\"$L Wh\"}"),TeleInfo.BBRHPJR);
        buf.write(',');
        buf.emit_p(PSTR("{\"name\":\"Couleur du lendemain\",\"val\":\"$S\"}"),TeleInfo.DEMAIN);
      }
  }
    buf.emit_p(PSTR("],\"uptime\":$L,\"free\":$D}"), millis(), 0); /// supprimer la fin
}


// TEMPERATURE JSON
#include "temperature_core.h"
extern int NumberOfTemp; // number of instances declared
extern Temperature *TEMPERATURE;

void listTempJson(BufferFiller& buf) {
  writeHeaders(buf);
  buf.println(F("Content-Type: application/json\r\n"));
  buf.print(F("{\"list\":["));

  uint8_t index = 0;

  //~ DeviceAddress addr;
  //~ sensors.requestTemperatures();
  //~ oneWire.reset_search();
  while (index < NumberOfTemp) {
    if (index != 0) buf.write(',');
    //~ float tempC = sensors.getTempC(addr);

    buf.emit_p(PSTR("{\"id\":\"$H$H$H$H$H$H$H$H\",\"name\":\"$S\",\"val\":$T}")
      ,TEMPERATURE[index].getAddr(0), TEMPERATURE[index].getAddr(1), TEMPERATURE[index].getAddr(2), TEMPERATURE[index].getAddr(3), TEMPERATURE[index].getAddr(4), TEMPERATURE[index].getAddr(5), TEMPERATURE[index].getAddr(6), TEMPERATURE[index].getAddr(7), TEMPERATURE[index].getName(), TEMPERATURE[index].getValue());
    index++;
  }

  buf.emit_p(PSTR("],\"uptime\":$L,\"free\":$D}"), millis(), 0); /// supprimer la fin
}


boolean checkUrl(const __FlashStringHelper *val, const char* data) {
  const char PROGMEM *p = (const char PROGMEM *)val;
  while (1) {
    char c = pgm_read_byte(p++);
    if (c == 0) break;
    if (*data != c) return false;
    data++;
  }
  return true;
}

void writeHeaders(BufferFiller& buf) {
  buf.print(F("HTTP/1.0 200 OK\r\nPragma: no-cache\r\n"));
}

// LIGHTING JSON
#include "lighting_core.h"
extern int NumberOfLighting; // number of instances declared
extern Lighting *LIGHTING;

void listLightJson(BufferFiller& buf) {

  writeHeaders(buf);
  buf.println(F("Content-Type: application/json\r\n"));
  buf.print(F("{\"list\":["));

  int index = 0;

  while (index < NumberOfLighting) {
    if (index != 0) buf.write(',');

        buf.emit_p(PSTR("{\"id\":\"$S\",\"val\":$D}"), LIGHTING[index].getName(), LIGHTING[index].getCurrent());
        index++;

    }

  buf.emit_p(PSTR("]}"));

}

// SHUTTER JSON
#include "shutter_core.h"
extern int NumberOfShutter; // number of instances declared
extern Shutter *SHUTTER;

void listShutterJson(BufferFiller& buf) {

  writeHeaders(buf);
  buf.println(F("Content-Type: application/json\r\n"));
  buf.print(F("{\"list\":["));

  int index = 0;

  while (index < NumberOfShutter) {
    if (index != 0) buf.write(',');

        buf.emit_p(PSTR("{\"id\":\"$S\",\"val\":$D}"), SHUTTER[index].getName(), SHUTTER[index].getCurrent());
        index++;

    }

  buf.emit_p(PSTR("]}"));

}

// SWITCH JSON
#include "switch_core.h"
extern int NumberOfSwitch; // number of instances declared
extern Switch *SWITCH;

void listSwitchJson(BufferFiller& buf) {

  writeHeaders(buf);
  buf.println(F("Content-Type: application/json\r\n"));
  buf.print(F("{\"list\":["));

  int index = 0;

  while (index < NumberOfSwitch) {
    if (index != 0) buf.write(',');

        buf.emit_p(PSTR("{\"id\":\"$S\",\"val\":$D}"), SWITCH[index].getName(), SWITCH[index].getStatus());
        index++;

    }

  buf.emit_p(PSTR("]}"));

}


// BOARD IN8R8 JSON
#include "BoardIn8R8.h"
extern int NumberOfBoardIn8R8; // number of instances declared
extern In8R8 *IN8R8;

void listIn8R8Json(BufferFiller& buf) {

  writeHeaders(buf);
  buf.println(F("Content-Type: application/json\r\n"));
  buf.print(F("{\"in8r8\":["));

  int index = 0;

  while (index < NumberOfBoardIn8R8) {
    if (index != 0) buf.write(',');

        buf.emit_p(PSTR("{\"id\":\"$D\",\"addr\":\"$D\",\"input\":\"$D\",\"output\":\"$D\",\"status\":\"$D\"}"), index, IN8R8[index].getCardId(), IN8R8[index].getInput(), IN8R8[index].getOutput(), IN8R8[index].IsValid());
        index++;

    }

  buf.emit_p(PSTR("]}"));

}


// BOARD IN4DIM4 JSON
#include "BoardIn4Dimmer4.h"
extern int NumberOfBoardIn4Dimmer4; // number of instances declared
extern In4Dimmer4 *IN4DIM4;

void listIn4dim4Json(BufferFiller& buf) {

  writeHeaders(buf);
  buf.println(F("Content-Type: application/json\r\n"));
  buf.print(F("{\"in4dim4\":["));

  int index = 0;

  while (index < NumberOfBoardIn4Dimmer4) {
    if (index != 0) buf.write(',');

        buf.emit_p(PSTR("{\"id\":\"$D\",\"addr\":\"$D\",\"input\":\"$D\",\"output\":\"$D\",\"status\":\"$D\"}"), index, IN4DIM4[index].getCardId(), IN4DIM4[index].getInput(), 0, IN4DIM4[index].IsValid());
        index++;

    }

  buf.emit_p(PSTR("]}"));

}


void not_found() { //content not found
  cur=0;
  streamfile ("404.hea",TCP_FLAGS_FIN_V);
  // Serial.println("not found");
}

byte streamfile (char* name , byte lastflag) { //send a file to the buffer
  if (!file.exists(name)) {return 0;}
  res=file.openFile(name, FILEMODE_BINARY);
  int  car=512;
  while (car==512) {
    car=file.readBinary();
    for(int i=0;i<car;i++) {
    cur++;
    Ethernet::buffer[cur+53]=file.buffer[i];
    }
if (cur>=512) {
      ether.httpServerReply_with_flags(cur,TCP_FLAGS_ACK_V);
      cur=0;
    }  else {

  if (lastflag==TCP_FLAGS_FIN_V) {
    ether.httpServerReply_with_flags(cur,TCP_FLAGS_ACK_V+TCP_FLAGS_FIN_V);
  }
    }
}
  file.closeFile();
  return 1;
}

byte sendfiles(char* name) { // function to find the correct header and send a file
  ether.httpServerReplyAck();
  int i =0;
  char dtype[13]="";
  while (name[i]!=0) {
    i++;
  }//search the end
  int b=i-1;
  while ((name[b]!=46)&&(b>0)) {
    b--;
  }//search the point
  int a=b+1;
  while (a<i) {
    dtype[a-b-1]=name[a];
    a++;
  }
  dtype[a-b-1]='.';
  dtype[a-b]='h';
  dtype[a-b+1]='e';
  dtype[a-b+2]='a';
  //Serial.println(dtype); // print the requested header file
  if (streamfile ((char *)dtype,0)==0) {
    streamfile ("txt.hea",0);
  }
    //Serial.println(name); // print the requested file
  if (streamfile ((char *)name,TCP_FLAGS_FIN_V)==0) {
    cur=0;
    not_found();
  }
  /*  uncomment this if you want to have printed the ip of the target browser*/
 origin();

}

void origin(){
#ifdef  DEBUG_WEB_SERVER
  Serial.print("from ");
  for(int i=30; i<34; i++) {
    Serial.print(Ethernet::buffer[i]);
    if (i<33) Serial.print(".");
    }
  Serial.println(" ");
#endif  
}


