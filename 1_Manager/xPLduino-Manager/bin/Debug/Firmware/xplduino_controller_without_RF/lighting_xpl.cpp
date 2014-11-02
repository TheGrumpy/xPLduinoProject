#include "Arduino.h"
#include "lighting_xpl.h"
#include "lighting_core.h"
#include <avr/pgmspace.h>

/*       Part of the xplduino project - http://code.google.com/p/xplduino/
 *       Write by Romain TISSOT CHARLOD
 *       Contact: xplduino@gmail.com
 *
 *       This code supplies the xPL layer of lighting_core library:
 *       - analyses the contain of the lighting.basic schema
 *       - analyses the contain of the lighting.request schema
 *       - send lighting command to a another xplduino lighting node
 *       - send the status of a lighting device
 *       - find a lighting device by its name
 *
 *       version 0.0: creation
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
 */

#define LIGHTING_XPL_VERSION 3

// version 2: ajout support commande toggle (sendToggle) et stop (sendStop). Sendcmd devient SendGoto.
// v3: ajout support comande gotoLastSetpoint() et gotoDefaultSetpoint()


//############################### GESTION LIGHTING ##################################

extern char vendor_id[];        // vendor id
extern char device_id[];        // device id
extern char instance_id[];      // instance id

extern xPL xpl;
extern Lighting *LIGHTING;

extern int NumberOfLighting; // number of instances declared

// analyse the command part of the schema lighting.basic:

/*  XPL-LIGHTING.BASIC Structure
 *  lighting.basic
 *  {
 *  command=[goto|activate|deactivate]
 *  [network=ID]
 *  [[device=ID]|[scene=ID]]
 *  [channel=#]
 *  [level=0-100]
 *  [fade-rate=[default|<time>]
 *  }
 */

int lighting_basic(xPL_Message * message){

#define GOTO        0
#define DEVICE      1
#define LEVEL       2
#define FADE        3
#define LAST        5
#define COMMAND     6


    int8_t cmnd=0;          // contains the flags to mark a command as found
    char temp[4+1]="\0";    // temporary string to convert a level or fade-rate from char to int

    // store the position of each command in the schema
    byte id_command=0;
    byte id_device=0;
    byte id_level=0;
    byte id_fade=0;

    #ifdef DEBUG_LIGHTING_XPL
    Serial.print(F("- analyse of "));
    Serial.print(message -> command_count);
    Serial.println(F(" commands -"));
    #endif

    // Step 1: in this loop, we scan each command line to determinate their position
    //~ for(int i= message -> command_count-1; i; i--){
    for(int i=0; i < message -> command_count; i++){

        #ifdef DEBUG_LIGHTING_XP
        Serial.print(F("  search "));
        Serial.println(message -> command[i].name);
        #endif

        if(!strcmp(message -> command[i].name,"command")){ // "command=..."
            if(!bitRead(cmnd,COMMAND)){
                bitSet(cmnd,COMMAND);
                id_command=i;
                #ifdef DEBUG_LIGHTING_XPL
                Serial.println(F("    => command ok"));
                #endif
            }else{  // doublon
                return 1;
            }
        }else
        if(!(strcmp(message -> command[i].name,"device"))){ // "device=..."
            if(!bitRead(cmnd,DEVICE)){
                bitSet(cmnd,DEVICE);
                id_device=i;
                #ifdef DEBUG_LIGHTING_XPL
                Serial.println(F("    => device ok"));
                #endif
            }else{  // doublon
                return 1;
            }
        }else
        if(!(strcmp(message -> command[i].name,"level"))){ // "level=..."
            if(!bitRead(cmnd,LEVEL)){
                bitSet(cmnd,LEVEL);
                id_level=i;
                #ifdef DEBUG_LIGHTING_XPL
                Serial.println(F("    => level ok"));
                #endif
            }else{  // doublon
                return 1;
            }
        }else
        if(!(strcmp(message -> command[i].name,"fade-rate"))){ // "fade-rate=..."
            if(!bitRead(cmnd,FADE)){
                bitSet(cmnd,FADE);
                id_fade=i;
                #ifdef DEBUG_LIGHTING_XPL
                Serial.println(F("    => ok"));
                #endif
            }else{  // doublon
                return 1;
            }
        }
    }


    // Step 2: if we have found at least the commmand 'goto' and 'device' we continue. Level and Fade-rate are not mandatory.
    if(bitRead(cmnd,COMMAND) && bitRead(cmnd,DEVICE)){

        // Step 2.0: now we check if the device is really part of this instance of xplduino.
        int position=lighting_find(message -> command[id_device].value, 0);

        // Step 2.1: if the position is not negative, the device is in this node, we can determinate the command
        if(position>=0)
        {

            // Previously we have found a level command and the command is 'goto'
            if(bitRead(cmnd,LEVEL) && !strcmp(message -> command[id_command].value, "goto")){

                int setpoint=0; // store the result of the setpoint converted from char to integer

                //  we convert the level (setpoint) from char to int. If sscanf replies zero, then level is not valid (not a number ?)
                if(sscanf(message -> command[id_level].value, "%d", &setpoint)){

                    #ifdef DEBUG_LIGHTING_XPL
                    Serial.print(F("   level: "));
                    Serial.println( message -> command[id_level].value);
                    #endif

                    if(bitRead(cmnd,FADE)){ // Previously we have found a fade-rate command in the message ?

                        int fade=0;

                        // what kind of fade-rate is requested ?
                        if(!strcmp(message -> command[id_fade].value,"default")){ // default fade-rate
                            LIGHTING[position].new_setpoint(setpoint); // we change only the setpoint
                        }else
                        if(sscanf(message -> command[id_fade].value, "%d", &fade)){ // the fade-rate is valid (number)
                            LIGHTING[position].new_setpoint(setpoint, fade); // we change the setpoint and the fade-rate
                        }

                    }else{ // no fade-rate is request
                        LIGHTING[position].new_setpoint(setpoint); // we change only the setpoint
                    }

                }else
                // Else, we check if the command is 'last'
                if(!strcmp(message -> command[id_level].value,"last")){
                    LIGHTING[position].gotoLastSetpoint();
                    //~ Serial.println(F("   last level is not supported yet"));
                }else
                // Else, we check if the command is 'default'
                if(!strcmp(message -> command[id_level].value,"default")){
                    LIGHTING[position].gotoDefaultSetpoint();
                    //~ Serial.println(F("   default level is not supported yet"));
                }else
                // Else, no supported command was found
                {
                    Serial.print(F("   level unknown: "));
                    Serial.println(message -> command[id_level].value);
                }

            }
            // Else, we check if the command is 'stop'
            else if(!strcmp(message -> command[id_command].value,"stop")){
                #ifdef DEBUG_LIGHTING_XPL
                Serial.print(message -> command[id_device].value);
                Serial.println(F(" stopped"));
                #endif
                LIGHTING[position].stop(); // demande arret de la variation
            }
            // Else, we check if the command is 'stop'
            else if(!strcmp(message -> command[id_command].value,"toggle")){
                #ifdef DEBUG_LIGHTING_XPL
                Serial.print(message -> command[id_device].value);
                Serial.println(F(" toggled"));
                #endif
                LIGHTING[position].toggle(); // demande arret de la variation
            }else
            // Else, no supported command was found :(
            {
                #ifdef DEBUG_LIGHTING_XPL
                Serial.print(message -> command[id_device].value);
                Serial.println(F("   -> NOT FOUND :("));
                #endif
            }

        }
        else
        {
            #ifdef DEBUG_LIGHTING_XPL
            Serial.println(F("unkwnown device"));
            #endif
        }

    #ifdef DEBUG_LIGHTING_XPL
    Serial.println(F("- end of analyse -"));
    #endif
    return 0;


    }

}

  /*   Send the lighting status
  *
  *   lighting.device
  *   {
  *   device=<lighting name>
  *   level=<level value>
  *   }
  */

void lighting_status( Lighting * _lighting, byte _type){


    /* preparation de la valeur actuelle */
    char _setpoint[3+1]="\0";

    //~ sprintf(_setpoint, "%s%d", _setpoint, _lighting -> setpoint); /// TODO mettre Current
    sprintf(_setpoint, "%s%d", _setpoint, _lighting -> getSetpoint()); /// TODO mettre Current

    #ifdef DEBUG_LIGHTING_XPL
    Serial.print(F("status current "));
    Serial.println(_setpoint);
    #endif

    xPL_Message msg;

    msg.hop = 1;

    if(_type == XPL_TRIG){
        msg.type = XPL_TRIG;
    }else{
        msg.type = XPL_STAT;
    }

    msg.SetSource("xplduino","lighting",instance_id);
    msg.SetTarget_P(PSTR("*"));

    msg.SetSchema_P(PSTR("lighting"), PSTR("device"));

    msg.AddCommand("device", _lighting -> getName());
    msg.AddCommand("level",_setpoint);


    if(_lighting -> getFade()){
        char _fade[3+1]="\0";
        sprintf(_fade, "%s%d", _fade, _lighting -> getFade());
        msg.AddCommand("fade-rate",_fade);
    }


    xpl.SendMessage(msg.toString());

    #ifdef DEBUG_LIGHTING_XPL
    Serial.println(F("------------  Lighting xPL  ------------"));
    Serial.println(msg.toString());
    #endif


}


// analyse the command part of the schema lighting.request:

/*  XPL-LIGHTING.REQUEST Structure
 *  lighting.request
 *  {
 *  request=[devstate]
 *  device=[name]
 *  }
 */

int lighting_request(xPL_Message * message){

#define REQUEST 0

    int8_t cmnd=0;          // contains the flags to mark a command as found
    char temp[4+1]="\0";    // temporary string to convert a level or fade-rate from char to int

    // store the position of each command in the schema
    byte id_request=0;
    byte id_device=0;

    #ifdef DEBUG_LIGHTING_XPL
    Serial.print(F("- analyse of "));
    Serial.print(message -> command_count);
    Serial.println(F(" commands -"));
    #endif

    // store the position of each command in the schema
    for(int i=0; i < message -> command_count; i++){

        #ifdef DEBUG_LIGHTING_XPL
        Serial.print(F(" search "));
        Serial.println(message -> command[i].name);
        #endif

        if(!strcmp(message -> command[i].name,"request")){ // "request=..."
            if(!bitRead(cmnd,REQUEST)){
                bitSet(cmnd,REQUEST);
                id_request=i;
                #ifdef DEBUG_LIGHTING_XPL
                Serial.println(F("  => ok"));
                #endif
            }else{  // doublon
                return 1;
            }
        }else
        if(!(strcmp(message -> command[i].name,"device"))){ // "device=..."
            if(!bitRead(cmnd,DEVICE)){
                bitSet(cmnd,DEVICE);
                id_device=i;
                #ifdef DEBUG_LIGHTING_XPL
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
        int position=lighting_find(message -> command[id_device].value, 0);

        // Step 2.1: if the position is not negative, the device is in this node, we can determinate the command
        if(position>=0)
        {
            // we check the type of reset
            if(!strcmp(message -> command[id_request].value,"devstate")){ // "request=devstate"
                lighting_status(&LIGHTING[position], T_STAT); // send the status of the lighting device
            }
            }else{ // not supported request
                Serial.print(F("  => command unknown: "));
                Serial.print(message -> command[id_request].value);
            }
    }
}


// this routin search the device in the list and replies the position
int lighting_find(char *_name, int first){

    extern Lighting *LIGHTING;

    // we read the name of each lighting instance to find its position
    for(int i=first; i < NumberOfLighting; i++){

    #ifdef DEBUG_LIGHTING_XPL
        Serial.print(F("   id "));
        Serial.print(i);
        Serial.print(F(" device "));
        Serial.print(_name);
        Serial.print(F(" compare to '"));
        Serial.println(LIGHTING[i].getName());
    #endif
        /// attention au changement name -> getName()
        if(!strcmp(_name, LIGHTING[i].getName())) // if the device name is the right the routin replies the position in the list
        {
    #ifdef DEBUG_LIGHTING_XPL
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


// build and send a lighting.basic message:

/*  XPL-LIGHTING.BASIC Structure
 *  lighting.basic
 *  {
 *  command=[goto|activate|deactivate]
 *  device=[name]
 *  level=[0-100]
 *  [fade-rate=[default|<time>]
 *  }
 */

void lighting_sendGoto(char *_instance_id, char *_device, byte _new_setpoint, byte _new_fade){

    char _setpoint[3+1]="\0"; // temporary variable to store the new setpoint converted from int to char

    sprintf(_setpoint, "%s%d", _setpoint, _new_setpoint); // convert the new setpoint from int to char


    /* build the message type commande */

    extern xPL xpl;

    xPL_Message msg;

    msg.hop = 1;
    msg.type = XPL_CMND;

    msg.SetTarget_P(PSTR("*"));
    msg.SetSchema_P(PSTR("lighting"), PSTR("basic"));

    msg.AddCommand("device", _device);
    msg.AddCommand("command", "goto");
    msg.AddCommand("level", _setpoint);

    if(_new_fade !=255){
        char _fade[3+1]="\0";
        sprintf(_fade, "%s%d", _fade, _new_fade);

        msg.AddCommand("fade-rate", _fade);
    }
     xpl.SendMessage(&msg);


#ifdef DEBUG_LIGHTING_XPL
    Serial.print(millis());
    Serial.print(" - SEND lighting_xpl goto command ----> ");
    Serial.print(_device);
    Serial.print(" current= ");
    Serial.print(_new_setpoint);
    Serial.println("%");
#endif

}

void lighting_sendStop(char *_instance_id, char *_device){

    /* build the message type commande */

    extern xPL xpl;

    xPL_Message msg;

    msg.hop = 1;
    msg.type = XPL_CMND;

    msg.SetTarget_P(PSTR("*"));
    msg.SetSchema_P(PSTR("lighting"), PSTR("basic"));

    msg.AddCommand("device", _device);
    msg.AddCommand("command", "stop");

     xpl.SendMessage(&msg);


#ifdef DEBUG_LIGHTING_XPL
    Serial.print(millis());
    Serial.print(" - SEND lighting_xpl stop command ----> ");
    Serial.println(_device);
#endif

}

void lighting_sendToggle(char *_instance_id, char *_device){

    /* build the message type commande */

    extern xPL xpl;

    xPL_Message msg;

    msg.hop = 1;
    msg.type = XPL_CMND;

    msg.SetTarget_P(PSTR("*"));
    msg.SetSchema_P(PSTR("lighting"), PSTR("basic"));

    msg.AddCommand("device", _device);
    msg.AddCommand("command", "toggle");

     xpl.SendMessage(&msg);


#ifdef DEBUG_LIGHTING_XPL
    Serial.print(millis());
    Serial.print(" - SEND lighting_xpl toggle command ----> ");
    Serial.println(_device);
#endif

}

byte aboutLightingXplVersion(){
    return LIGHTING_XPL_VERSION;
}

/*
void lighting_xpl_stoplighting(char *_vendor_id, char *_device_id, char *_instance_id, char *_device){

    char buffer[240]="\0"; // 213

    sprintf(buffer, "%s%s", buffer,"xpl-cmnd\n{\nhop=1\nsource=");
    sprintf(buffer, "%s%s", buffer,Config.vendor_id);
    sprintf(buffer, "%s%s", buffer,"-scenario.");
    sprintf(buffer, "%s%s", buffer,Config.instance_id);
    sprintf(buffer, "%s%s", buffer,"\ntarget=");
    sprintf(buffer, "%s%s", buffer,_vendor_id);
    sprintf(buffer, "%s%s", buffer,"-");
    sprintf(buffer, "%s%s", buffer,_device_id);
    sprintf(buffer, "%s%s", buffer,".");
    sprintf(buffer, "%s%s", buffer,_instance_id);
    sprintf(buffer, "%s%s", buffer,"\n}\nlighting.basic\n{\nrequest=stop\ndevice=");
    sprintf(buffer, "%s%s", buffer,_device);
    sprintf(buffer, "%s%s", buffer,"\n}\n");

    ///Udp1.sendPacket(buffer, Config.targetIp, Config.targetPort);

    Serial.print(millis());
    Serial.print(" - SEND lighting_xpl stop command ----> ");
    Serial.println(_device);

}
*/

/*
// recherche le device dans la liste et modifie ses parametres suite a un message CONFIG.RESPONSE
void lighting_xpl_response(struct_command *_command){

    if(!(strcmp(_command[0].name,"device")) && (!strcmp(_command[1].name,"new")))
    {

        //on recherche le device a modifier
        int position=lighting_find(_command[0].value, 0);

        if(position>=0)
        {
            // on recherche si le nouveau nom n existe pas deja
            if(lighting_find(_command[1].value, position+1)<0)
            {
            // TODO: ajouter une condition si champs vide (fin de la liste)
            // TODO: permettre de reaffecter le numero de pin (port+channel) depuis un message xpl

                //~ Serial.print("changed name of device '");
                //~ Serial.print(LIGHTING[position].name);

                sprintf(LIGHTING[position].name,"%s",_command[1].value);

                //~ Serial.print("' in position ");
                //~ Serial.print(position);
                //~ Serial.print(" to ");
                //~ Serial.println(LIGHTING[position].name);

                lighting_xpl_status(TRIG, _command[1].value, "ok");

                // return;
            }
            else
            {
                //~ Serial.print("device name'");
                //~ Serial.print(_command[1].value);
                //~ Serial.println("' already affect to a device !");

                lighting_xpl_status(TRIG, _command[0].value, "Er02");   //  the device is not founded or new name already assigned
            }

        }
        else
        {
            //~ Serial.print("device '");
            //~ Serial.print(_command[0].value);
            //~ Serial.println("' is not founded and can't be changed !");

            lighting_xpl_status(TRIG, _command[0].value, "Er01");   //  the device is not founded or new name already assigned
        }

#endif
    }
}
*/

