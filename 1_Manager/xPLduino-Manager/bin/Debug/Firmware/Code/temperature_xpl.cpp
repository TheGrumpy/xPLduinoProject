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

#define TEMPERATURE_XPL_VERSION 2
// Normalisation des flag de debug

// #include <DS2482.h>
#include "temperature_xpl.h"

/*---------------------Config bus Temperature-------------------------*/
// Setup a oneWire instance to communicate with any OneWire devices (not just Maxim/Dallas temperature ICs)
extern Temperature *TEMPERATURE;
extern int NumberOfTemp; // number of instances declared
extern char vendor_id[];        // vendor id
extern char instance_id[];      // instance id

// extern DS2482 ds;
//~ byte byte_sec;
//~ byte byte_min;
//~ byte time;
//~ boolean timeSec;

void temperature_status( Temperature * _temperature, byte _type){
  extern xPL xpl;

  if (_temperature -> IsPres()) {
    // creation du message XPL
    xPL_Message msg;

    msg.hop = 1;
    msg.type = XPL_STAT;
    msg.SetSource("xplduino","temp",instance_id);
    msg.SetTarget_P(PSTR("*"));
    msg.SetSchema_P(PSTR("sensor"), PSTR("basic"));
    msg.AddCommand_P(PSTR("type"),PSTR("temp"));
    msg.AddCommand(("device"),(_temperature -> getName()));

    char tmp[60]="\0";
    //~ sprintf(tmp, "%s%d", tmp, value);
    dtostrf(_temperature -> getValue(), 3, 2, tmp);

    msg.AddCommand(("current"),(tmp));
    //* envoi du message */
    xpl.SendMessage(msg.toString());

#ifdef DEBUG_Temp_XPL
    Serial.println(F("------------  Temperature xPL  ------------"));
    Serial.println(msg.toString());
#endif
  }
}

// fonction pour l'envois XPL si le flag est à 1
byte Device_I;
void temperature_sendXPL() {

extern Temperature *TEMPERATURE;

if (Device_I>=NumberOfTemp) {
    Device_I=0;
  }
 if (TEMPERATURE[Device_I].IsSendToXPL()) {
     temperature_status(&TEMPERATURE[Device_I], T_STAT);
     TEMPERATURE[Device_I].postNotSendToXPL();
 }
  Device_I=Device_I+1;
}

// recherche le device dans la list et renvoie sont état si trouvee
int sensor_request(xPL_Message * message){

#define GOTO        0
#define DEVICE      1
#define LEVEL       2
#define FADE        3
#define LAST        5
#define COMMAND     6
#define REQUEST 0


    int8_t cmnd=0; //donnees temporaire pour la recherche des commandes
    char temp[4+1]="\0";    // level ou fade-rate au format char pour conversion en int
    int16_t i=0;
    int16_t nbre= message -> command_count;
    // memorise le numero de la commande
    byte id_request=0;
    byte id_device=0;



    #ifdef DEBUG_SWITCH_XPL
    Serial.print(F("- analyse de "));
    Serial.print(nbre);
    Serial.println(F(" commandes -"));
    #endif


    for(i=0; i < nbre; i++){


        #ifdef DEBUG_Temp_XPL
        Serial.print(F(" cherche "));
        Serial.println(message -> command[i].name);
        #endif

        if(!strcmp(message -> command[i].name,"request")){
            if(!bitRead(cmnd,REQUEST)){
                bitSet(cmnd,REQUEST);
                id_request=i;
                #ifdef DEBUG_Temp_XPL
                Serial.println(F("  => ok"));
                #endif
            }else{  // doublon
                return 1;
            }
        }else
        if(!(strcmp(message -> command[i].name,"device"))){
            if(!bitRead(cmnd,DEVICE)){
                bitSet(cmnd,DEVICE);
                id_device=i;
                #ifdef DEBUG_Temp_XPL
                Serial.println(F("  => ok"));
                #endif
            }else{  // doublon
                return 1;
            }
        }
    }

    // Step 2: if we have found at least the commmand 'request' and 'device' we continue.
    if(bitRead(cmnd,REQUEST) && bitRead(cmnd,DEVICE)){


        // Step 2.0: now we check if the device is really part of this instance of xplduino.
        int position=temperature_find(message -> command[id_device].value, 0);

        // Step 2.1: if the position is not negative, the device is in this node, we can determinate the command
        if(position>=0)
        {
            // we check the type of reset
            if(!strcmp(message -> command[id_request].value,"devstate")){ // "request=devstate"
                temperature_status(&TEMPERATURE[position], T_STAT); // send the status of the lighting device
            }
            }else{ // not supported request
                Serial.print(F("  => command unknown: "));
                Serial.print(message -> command[id_request].value);
            }
    }
}
// this routin search the device in the list and replies the position
int temperature_find(char *_name, int first){

    extern Temperature *TEMPERATURE;

    // we read the name of each lighting instance to find its position
    for(int i=first; i < NumberOfTemp; i++){

    #ifdef DEBUG_Temp_XPL
        Serial.print(F("   id "));
        Serial.print(i);
        Serial.print(F(" device "));
        Serial.print(_name);
        Serial.print(F(" compare to '"));
        Serial.println(TEMPERATURE[i].getName());
    #endif

        if(!strcmp(_name, TEMPERATURE[i].getName())) // if the device name is the right the routin replies the position in the list
        {
    #ifdef DEBUG_Temp_XPL
        Serial.print(F("    => device '"));
        Serial.print(_name);
        Serial.print(F(" ' has been found at position "));
        Serial.println(i);
    #endif
            return i;
        }
    }
    return -1; // but if the device is not found, the routin replies a negative value
}


byte aboutTemperatureXplVersion(){
  return TEMPERATURE_XPL_VERSION;
}


