/*       Part of the xplduino project - http://code.google.com/p/xplduino/
 *       Write by Steeve VERMEERSCH
 *       Contact: xplduino@gmail.com
 *
 *       This program is free software; you can redistribute it and/or modify
 *       it under the terms of the GNU General Public License as published by
 *       the Free Software Foundation; either version 2 of the License, or
 *       (at your option) any later version.
 *
 *       This program is distributed in the hope that it will be useful,
 *       but WITHOUT ANY WARRANTY; without even the implied warranty of
 *       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *       GNU General Public License for more details.
 *
 *       You should have received a copy of the GNU General Public License
 *       along with this program; if not, write to the Free Software
 *       Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
 *       MA 02110-1301, USA
 *
 *       version 0.0: creation
 *       version 1.0: add the management in periodic time (by seconds or by minutes)
 *
 */

#define TEMPERATURE_CORE_VERSION 4.0
//------ Version 2.0 ------------
// Ajout du CRC avant la lecture de temperature
//------ Version 3.0 ------------
// Passage en fonctions privées
// Normalisation des flag de debug

#include <DS2482.h>
#include "temperature_core.h"
#include <DS1307new.h>

/*---------------------Config bus Temperature-------------------------*/
// Setup a oneWire instance to communicate with any OneWire devices (not just Maxim/Dallas temperature ICs)
extern Temperature *TEMPERATURE;
extern int NumberOfTemp; // number of instances declared
extern char vendor_id[];        // vendor id
extern char instance_id[];      // instance id
extern byte hbeat; // life counter sent to others devices

DS2482 ds;
/*--------------------------------------------------------------------*/
//constructor
Temperature::Temperature(){
}
/*-----------------Specification d'une sonde---------------------------*/
uint8_t Temperature::init(char *_name,byte _adress0,byte _adress1,byte _adress2,byte _adress3,byte _adress4,byte _adress5,byte _adress6,byte _adress7,boolean _arrond)
{
  sprintf(name,"%s",_name);
  addr[0]=_adress0;
  addr[1]=_adress1;
  addr[2]=_adress2;
  addr[3]=_adress3;
  addr[4]=_adress4;
  addr[5]=_adress5;
  addr[6]=_adress6;
  addr[7]=_adress7;
  tempC=-127;
  value=-127;
  Temp_status=0;
  if (_arrond==true){postArrondi();} else {postNotArrondi();}
// Affichage de la configuration dans la console
  Serial.print("Init sensor : ");
  Serial.print(name);
  Serial.print(" @:");
  for (uint8_t i = 0; i < 8; i++){
      if (i>0) {
            Serial.print("/");
          }
          Serial.print(addr[i], HEX);
        }
        if (_arrond==false) {Serial.println(F(" - arrondi a 0,1"));} else {Serial.println(F(" - arrondi a 0,5"));}

        return 1;
}

void Temperature::request_temp()
{
#ifdef DEBUG_TEMP_OW
        Serial.print("Request temp of ");Serial.println(name);
#endif
    ds.wireReset();
    ds.romMatch(addr);
    ds.wireWrite(ConvertT);
}
// Lecture de la temperature
void Temperature::read_temp()
{
#ifdef DEBUG_TEMP_OW
        Serial.print("Read temp of ");Serial.println(name);
#endif
    byte ScratchPad[8]={0,0,0,0,0,0,0,0};

    ds.wireReset();
    ds.romMatch(addr);
    ds.wireWrite(ReadScratchPad);

    ScratchPad[0]=ds.wireRead();
    ScratchPad[1]=ds.wireRead();
    ScratchPad[2]=ds.wireRead();
    ScratchPad[3]=ds.wireRead();
    ScratchPad[4]=ds.wireRead();
    ScratchPad[5]=ds.wireRead();
    ScratchPad[6]=ds.wireRead();
    ScratchPad[7]=ds.wireRead();
    ScratchPad[8]=ds.wireRead();

if ((uint8_t)ScratchPad[0]==0 && (uint8_t)ScratchPad[1]==0 && (uint8_t)ScratchPad[2]==0 && (uint8_t)ScratchPad[3]==0 && (uint8_t)ScratchPad[4]==0 && (uint8_t)ScratchPad[5]==0 && (uint8_t)ScratchPad[6]==0 && (uint8_t)ScratchPad[7]==0 && (uint8_t)ScratchPad[8]==0) {DS2482_Init();}

if ((uint8_t)ScratchPad[8]==crc8(ScratchPad,8) && (uint8_t)ScratchPad[0]!=0 && (uint8_t)ScratchPad[8]!=0){

    int16_t rawTemperature = (((int16_t)ScratchPad[TEMP_MSB]) << 8) | ScratchPad[TEMP_LSB];

    switch (addr[0])
    {
    case DS18B20MODEL:
    case DS1822MODEL:
      switch (ScratchPad[CONFIGURATION])
      {
      case TEMP_12_BIT:
        tempC= (float)rawTemperature * 0.0625;
        break;
      case TEMP_11_BIT:
        tempC= (float)(rawTemperature >> 1) * 0.125;
        break;
      case TEMP_10_BIT:
        tempC= (float)(rawTemperature >> 2) * 0.25;
        break;
      case TEMP_9_BIT:
        tempC= (float)(rawTemperature >> 3) * 0.5;
        break;
      }
      break;
    case DS18S20MODEL:

      /*
      Resolutions greater than 9 bits can be calculated using the data from
       the temperature, COUNT REMAIN and COUNT PER �C registers in the
       scratchpad. Note that the COUNT PER �C register is hard-wired to 16
       (10h). After reading the scratchpad, the TEMP_READ value is obtained
       by truncating the 0.5�C bit (bit 0) from the temperature data. The
       extended resolution temperature can then be calculated using the
       following equation:

       COUNT_PER_C - COUNT_REMAIN
       TEMPERATURE = TEMP_READ - 0.25 + --------------------------
       COUNT_PER_C
       */
      // Good spot. Thanks Nic Johns for your contribution



      tempC= (float)(rawTemperature >> 1) - 0.25 +((float)(ScratchPad[COUNT_PER_C] - ScratchPad[COUNT_REMAIN]) / (float)ScratchPad[COUNT_PER_C] );
      break;
    }

    if (IsArrondi()){
      tempC = Arrondi(tempC);            // arrondi  à 0,5 près
    }
    if (IsArrondi()==false){
      tempC = float(int(tempC*10))/10;  // arrondi 1 chiffre après la virgule
    }

    if (tempC==DEVICE_DISCONNECTED || tempC==85)    // Patch pour la première lecture à 85
    {
      postPresNok();  // Sonde non connectée
    }
    else
    {
      postPresOk();  // Sonde  detectée
    }
        if (IsPres() && (value!=tempC)) {
          postSendToXPL();   // envois si la temperature a changé

          // mise en memoire de la valeur de temperature
          value=tempC;

#ifdef DEBUG_TEMP_OW
        Serial.print("Sensor : ");
        Serial.print(name);
        Serial.print(" @:");
        for (uint8_t i = 0; i < 8; i++){
          if (i>0) {
            Serial.print("/");
          }
          Serial.print(addr[i], HEX);
        }
        Serial.print(F(" "));
        Serial.print(tempC);
        Serial.print(F("C - at time : "));
        RTC.getTime();
        Serial.print(RTC.hour);Serial.print(":");Serial.print(RTC.minute);Serial.print(":");Serial.println(RTC.second);
#endif
        }
      }
    else{
#ifdef DEBUG_TEMP_OW
        Serial.println("CRC KO ");
#endif
        postPresNok(); // Sonde non connectée
        }
}

// function to read the address
byte Temperature::getAddr(uint8_t Addr_ID)
{
  return addr[Addr_ID];
}

// function to read the address
char* Temperature::getName()
{
    return name;
}
// function to read the address
float Temperature::getValue()
{
    return value;
}
// function to check is the device is present
uint8_t  Temperature::IsPres(){
  return bitRead(Temp_status, Addr_pres);
}
// function to check is the value is send by XPL
uint8_t  Temperature::IsSendToXPL(){
  return bitRead(Temp_status, Addr_sendXPL);
}
// function to check is the value arrondi
uint8_t  Temperature::IsArrondi(){
  return bitRead(Temp_status, Addr_arrondi);
}
// function to make the device is present
void  Temperature::postPresOk(){
  bitSet(Temp_status, Addr_pres);
}
// function to make the device is present
void  Temperature::postPresNok(){
  bitClear(Temp_status, Addr_pres);
}
// function to indicate that the value is send by XPL
void  Temperature::postSendToXPL(){
   bitSet(Temp_status, Addr_sendXPL);
}
// function to indicate that the value is send by XPL
void  Temperature::postNotSendToXPL(){
   bitClear(Temp_status, Addr_sendXPL);
}
// function to activate the arrondi
void  Temperature::postArrondi(){
  bitSet(Temp_status, Addr_arrondi);
}
// function to desactivate the arrondi
void  Temperature::postNotArrondi(){
  bitClear(Temp_status, Addr_arrondi);
}

float Arrondi(float var)
{
  float val_dec = (var - (int)var);
  if(val_dec >= 0.25 && val_dec <= 0.74){
    return  (int)var + 0.5;
  }
  else return ((int)var + (val_dec > 0.5));
}


// Initialisation de la lecture des sondes et de la voie serie pour le debug
void DS2482_Init()
{
  ds.init(0);    // initialisation des capteurs
}

// Lecture de toutes les sondes sur le reseau OneWire
void Read_DS2482_DeviceAdr()
{
    Serial.println("fonction");
  byte Addr[8]={1,1,1,1,1,1,1,1};
  byte FirstAddr[8]={0,0,0,0,0,0,0,0};
  uint8_t k=0;

  Serial.println(F("------------Read OneWire sensor(s) address---------------"));
  ds.romSearch(Addr,0);     // lecture de l'adresse de la premiere sonde

    while (FirstAddr[7]!=Addr[7])
    {
       if(k==0) {for (uint8_t j = 0; j < 8; j++){FirstAddr[j]=Addr[j];}}
       Serial.print(F("Sensor - "));Serial.print(k); Serial.print(F(" @:"));
       for (uint8_t i = 0; i < 8; i++){
          if (i>0) {
            Serial.print(F("/"));
          }
          Serial.print(Addr[i], HEX);
        }
        Serial.println("");
        k++;                     // nombre de sondes
        ds.romSearch(Addr,0);   // lecture de l'adresse de la sonde suivante
  }
  Serial.println(F("------------No more address---------------"));
}

byte Device_ID_read;
byte Device_ID_req;
byte start;
void Read_DS2482_Temp(boolean timeSec)
// function to read temperature of device
{
#ifdef DEBUG_TEMP_OW
        Serial.println("Cycle of Read Temp");
#endif
  if (Device_ID_read>=NumberOfTemp) {
    Device_ID_read=0;
  }
    if (Device_ID_req>=NumberOfTemp) {
    Device_ID_req=0;
  }
  if (start==1){TEMPERATURE[Device_ID_read].read_temp();Device_ID_read=Device_ID_read+1;}
  TEMPERATURE[Device_ID_req].request_temp();
  Device_ID_req=Device_ID_req+1;

  start=1;
#ifdef DEBUG_TEMP_OW
        Serial.println("--------------");
#endif
}




uint8_t crc8(const uint8_t *addr, uint8_t len)
// function to calcuate the CRC
{
    uint8_t crc = 0;

    while (len--) {
        uint8_t inbyte = *addr++;
        for (uint8_t i = 8; i; i--) {
            uint8_t mix = (crc ^ inbyte) & 0x01;
            crc >>= 1;
            if (mix) crc ^= 0x8C;
            inbyte >>= 1;
        }
    }
    return crc;
}


byte aboutTemperatureVersion(){
  return TEMPERATURE_CORE_VERSION;
}




