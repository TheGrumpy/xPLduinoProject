#include "water_counter_xpl.h"
#include "water_counter_core.h"

#include <avr/pgmspace.h>

#define WATER_COUNTER_XPL_VERSION 0


extern char vendor_id[];        // vendor id
extern char device_id[];        // device id
extern char instance_id[];      // instance id

extern xPL xpl;
extern water_counter *WATER_COUNTER;

extern int NumberOfWaterCounter; // number of instances declared

#define DEVICE      1
#define COMMAND     6

int water_counter_status(water_counter *_water_counter, byte _type){


    char _current[10+1]="\0";

    sprintf(_current, "%s%d", _current, _water_counter -> getValue());

#ifdef DEBUG_WATER_COUNTER_XPL

        if(_type==T_TRIG){
            Serial.print(F("trigger of "));
        }
        else if(_type==T_STAT){
            Serial.print(F("stat of "));
        }
        else{
            Serial.print(F("unknown type of "));
        }

        Serial.print(_water_counter -> getName());
        Serial.print(F(": "));

        Serial.print(_water_counter -> getValue());
        Serial.println(F(" L"));

#endif


    xPL_Message msg;

    msg.hop = 1;

    /* construction du message */

    if(_type == XPL_TRIG){
        msg.type = XPL_TRIG;
    }else{
        msg.type = XPL_STAT;
    }

    msg.SetSource("xplduino","water",instance_id);
    msg.SetTarget_P(PSTR("*"));

    msg.SetSchema_P(PSTR("sensor"), PSTR("basic"));

    msg.AddCommand("type","water");
    msg.AddCommand("device", _water_counter -> getName());

    msg.AddCommand("current", _current);


    /* envoi du message */
    xpl.SendMessage(msg.toString());

    #ifdef DEBUG_WATER_COUNTER_XPL
    Serial.println(F("------------   Water Counter xPL   ------------"));
    Serial.println(msg.toString());
    #endif

    return 1;


}


// recherche le device dans la liste et renvoie son etat si trouvee
int water_counter_request(xPL_Message * message){

#define REQUEST 0

    int8_t cmnd=0; //donnees temporaire pour la recherche des commandes
    char temp[4+1]="\0";    // level ou fade-rate au format char pour conversion en int
    int16_t i=0;
    int16_t nbre= message -> command_count;

    // memorise le numero de la commande
    byte id_request=0;
    byte id_device=0;

    #ifdef DEBUG_WATER_COUNTER_XPL
    Serial.print(F("- analyse de "));
    Serial.print(nbre);
    Serial.println(F(" commandes -"));
    #endif


    for(i=0; i < nbre; i++){


        #ifdef DEBUG_WATER_COUNTER_XPL
        Serial.print(F(" cherche "));
        Serial.println(message -> command[i].name);
        #endif

        if(!strcmp(message -> command[i].name,"request")){
            if(!bitRead(cmnd,REQUEST)){
                bitSet(cmnd,REQUEST);
                id_request=i;
                #ifdef DEBUG_WATER_COUNTER_XPL
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
                #ifdef DEBUG_WATER_COUNTER_XPL
                Serial.println(F("  => ok"));
                #endif
            }else{  // doublon
                return 1;
            }
        }
    }

    //verification que commande goto, device et level sont present. Fade-rate est en option.
    if(bitRead(cmnd,REQUEST) && bitRead(cmnd,DEVICE)){

        //recherche du device water_counter
        int position=water_counter_find(message -> command[id_device].value, 0);
        if(position>=0)
        {

            int setpoint=0;

            if(!strcmp(message -> command[id_request].value,"devstate")){
                water_counter_status(&WATER_COUNTER[i], T_STAT); //TRIG
            }
            }else{
                Serial.print(F("  => command unknown: "));
                Serial.print(message -> command[id_request].value);
            }
    }
}


// recherche le device dans la liste et modifie ses parametres suite a un message CONFIG.RESPONSE
int water_counter_find(char *_name, int first){

    extern water_counter *water_counter;

    // on parcourt la liste des device water_counter en commencant par la ligne "first"
    for(int i=first; i < NumberOfWaterCounter; i++){

    #ifdef DEBUG_WATER_COUNTER_XPL
        Serial.print(F("   id "));
        Serial.print(i);
        Serial.print(F(" device "));
        Serial.print(_name);
        Serial.print(F(" compare to '"));
        Serial.println(water_counter[i].getName());
    #endif

        if(!strcmp(_name, water_counter[i].getName()))    // si device trouve alors on retourne le numero
        {
    #ifdef DEBUG_WATER_COUNTER_XPL
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

byte aboutWaterCounterXplVersion(){
    return WATER_COUNTER_XPL_VERSION;
}



