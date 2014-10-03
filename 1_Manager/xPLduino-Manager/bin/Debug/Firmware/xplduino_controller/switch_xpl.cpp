#include "switch_xpl.h"
#include "switch_core.h"
#include <avr/pgmspace.h>

#define SWITCH_XPL_VERSION 3

// version 1:   passage au methode
// version 2:   ajout type type=input dans emission message et traitement reception.
//              Note: il va y avoir conflit sur le traitement de message sensor.basic de different type.
// version 3:   sensor.request, request=devstate devient request=current

extern char vendor_id[];        // vendor id
extern char device_id[];        // device id
extern char instance_id[];      // instance id

extern xPL xpl;
extern Switch *SWITCH;

extern int NumberOfSwitch; // number of instances declared

#define DEVICE      1
#define COMMAND     6
#define TYPE        2

int switch_status(Switch *_switch, byte _type){

#ifdef DEBUG_SWITCH_XPL
        if(_type==T_TRIG){
            Serial.print(F("trigger of "));
        }

        Serial.print(_switch -> getName());
        Serial.print(F(": "));

        if(_switch -> isPulse()){
            Serial.println(F("Pulse"));
        }
        else if(_switch -> isDoublePulse()){
            Serial.println(F("Dpulse"));
        }
        else if(_switch -> isOnOSR()){
            Serial.println(F("High"));
        }
        else if(_switch -> isOnOSF()){
            Serial.println(F("Low"));
        }
#endif


    xPL_Message msg;

    msg.hop = 1;

    /* construction du message */

    if(_type == XPL_TRIG){
        msg.type = XPL_TRIG;
    }else{
        msg.type = XPL_STAT;
    }

    msg.SetSource("xplduino","switch",instance_id);
    msg.SetTarget_P(PSTR("*"));

    msg.SetSchema_P(PSTR("sensor"), PSTR("device"));

    msg.AddCommand("type", "input");
    msg.AddCommand("device", _switch -> getName());


    if(_switch -> isPulse()){
        msg.AddCommand("current","pulse");
    }
    else if(_switch -> isDoublePulse()){
        msg.AddCommand("current","dpulse");
    }
    else if(_switch -> isOnOSR()){
        msg.AddCommand("current","high");
    }
    else if(_switch -> isOnOSF()){
        msg.AddCommand("current","low");
    }

    /* envoi du message */
    xpl.SendMessage(msg.toString());

    #ifdef DEBUG_SWITCH_XPL
    Serial.println(F("------------   Switch xPL   ------------"));
    Serial.println(msg.toString());
    #endif

    return 1;

}


// recherche le device dans la liste et renvoie son etat si trouvee
int switch_request(xPL_Message * message){

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


        #ifdef DEBUG_SWITCH_XPL
        Serial.print(F(" cherche "));
        Serial.println(message -> command[i].name);
        #endif

        if(!strcmp(message -> command[i].name,"request")){
            if(!bitRead(cmnd,REQUEST)){
                bitSet(cmnd,REQUEST);
                id_request=i;
                #ifdef DEBUG_SWITCH_XPL
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
                #ifdef DEBUG_SWITCH_XPL
                Serial.println(F("  => ok"));
                #endif
            }else{  // doublon
                return 1;
            }
        }else
        if(!(strcmp(message -> command[i].name,"type"))){

            if((strcmp(message -> command[i].name,"input"))) // ce n'est pas un type input
                return 1;

            if(!bitRead(cmnd,TYPE)){
                bitSet(cmnd,TYPE);
                id_device=i;
                #ifdef DEBUG_SWITCH_XPL
                Serial.println(F("  => ok"));
                #endif
            }else{  // doublon
                return 1;
            }
        }
    }

    //verification que commande request, device et type sont present.
    if(bitRead(cmnd,REQUEST) && bitRead(cmnd,DEVICE) && bitRead(cmnd,TYPE)){

        //recherche du device switch
        int position=switch_find(message -> command[id_device].value, 0);
        if(position>=0)
        {

            int setpoint=0;

            if(!strcmp(message -> command[id_request].value,"current")){
                switch_status(&SWITCH[i], T_STAT); //TRIG
            }
            }else{
                Serial.print(F("  => command unknown: "));
                Serial.print(message -> command[id_request].value);
            }
    }
}


// recherche le device dans la liste et modifie ses parametres suite a un message CONFIG.RESPONSE
int switch_find(char *_name, int first){

    extern Switch *SWITCH;

    // on parcourt la liste des device switch en commencant par la ligne "first"
    for(int i=first; i < NumberOfSwitch; i++){

    #ifdef DEBUG_SWITCH_XPL
        Serial.print(F("   id "));
        Serial.print(i);
        Serial.print(F(" device "));
        Serial.print(_name);
        Serial.print(F(" compare to '"));
        Serial.println(SWITCH[i].getName());
    #endif

        if(!strcmp(_name, SWITCH[i].getName()))    // si device trouve alors on retourne le numero
        {
    #ifdef DEBUG_SWITCH_XPL
        Serial.print(F("    => device '"));
        Serial.print(_name);
        Serial.print(F(" ' has been found at position "));
        Serial.println(i);
    #endif
            return i;
        }
    }
    return -1; // si device pas trouve alors on retourne -1
}

byte aboutSwitchXplVersion(){
    return SWITCH_XPL_VERSION;
}



