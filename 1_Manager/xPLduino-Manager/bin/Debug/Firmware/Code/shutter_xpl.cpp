#include "Arduino.h"
#include "shutter_xpl.h"
#include "shutter_core.h"
#include "utility.h"
#include <avr/pgmspace.h>

#define SHUTTER_XPL_VERSION 2

//v2: ajout envoi command: goto, open, close et stop

//############################### GESTION SHUTTER ##################################

extern char vendor_id[];        // vendor id
extern char device_id[];        // device id
extern char instance_id[];      // instance id

extern xPL xpl;
extern Shutter *SHUTTER;

extern int NumberOfShutter; // number of instances declared

#define GOTO        0
#define DEVICE      1
#define LEVEL       2
#define LAST        5
#define COMMAND     6

int shutter_basic(xPL_Message * message){

    int8_t cmnd=0; //donnees temporaire pour la recherche des commandes
    char temp[4+1]="\0";    // level ou fade-rate au format char pour conversion en int
    int16_t i=0;
    int16_t nbre= message -> command_count;


    // memorise le numero de la commande
    byte id_command=0;
    byte id_device=0;
    byte id_level=0;

    #ifdef DEBUG_SHUTTER_XPL
    Serial.println(F("----------  xPL Shutter basic  ----------"));
    Serial.print(F("- analyse de "));
    Serial.print(nbre);
    Serial.println(F(" commandes -"));
    #endif

    for(i=0; i < nbre; i++){

        #ifdef DEBUG_SHUTTER_XPL
        Serial.print(F("  cherche "));
        Serial.println(message -> command[i].name);
        #endif

        if(!strcmp(message -> command[i].name,"command")){
            if(!bitRead(cmnd,COMMAND)){
                bitSet(cmnd,COMMAND);
                id_command=i;
                #ifdef DEBUG_SHUTTER_XPL
                Serial.println(F("    => ok"));
                #endif
            }else{  // doublon
                return 1;
            }
        }else
        if(!(strcmp(message -> command[i].name,"device"))){
            if(!bitRead(cmnd,DEVICE)){
                bitSet(cmnd,DEVICE);
                id_device=i;
                #ifdef DEBUG_SHUTTER_XPL
                Serial.println(F("    => ok"));
                #endif
            }else{  // doublon
                return 1;
            }
        }else
        if(!(strcmp(message -> command[i].name,"level"))){
            if(!bitRead(cmnd,LEVEL)){
                bitSet(cmnd,LEVEL);
                id_level=i;
                #ifdef DEBUG_SHUTTER_XPL
                Serial.println(F("    => ok"));
                #endif
            }else{  // doublon
                return 1;
            }
        }

    }


    //verification que 'command' et 'device' sont presents.
    if(bitRead(cmnd,COMMAND) && bitRead(cmnd,DEVICE)){

        ///ici, on dispose du nom du device, du type de commande. Il faut donc executer l'action voulu: trouver le device dans la liste et executer la commande
        //recherche du device shutter a modifier
        int position=shutter_find(message -> command[id_device].value, 0);

        if(position>=0)
        {
            // si command=goto et level present
            if(!strcmp(message -> command[id_command].value,"goto") && bitRead(cmnd,LEVEL)){

                int setpoint=0;

                if(sscanf(message -> command[id_level].value, "%d", &setpoint)){ // verification que la consigne est valable

                    Serial.print(F("   level: "));
                    Serial.println( message -> command[id_level].value);

                    SHUTTER[position].new_setpoint(setpoint); // application nouvelle consigne seule

                }else
                if(!strcmp(message -> command[id_level].value,"last")){
                    // not supported yet: have to add a method in shutter_core
                    Serial.println(F("   last level is not supported yet"));
                }else
                if(!strcmp(message -> command[id_level].value,"default")){
                    // not supported yet: have to add a method in shutter_core
                    Serial.println(F("   default level is not supported yet"));
                }else{
                    Serial.print(F("   level unknown: "));
                    Serial.println(message -> command[id_level].value);
                }

            } // si command=stop
            else if(!strcmp(message -> command[id_command].value,"toggle")){
                #ifdef DEBUG_SHUTTER_XPL
                Serial.print(message -> command[id_device].value);
                Serial.println(F(" toggled"));
                #endif
                SHUTTER[position].toggle(); // demande basculement consigne volet
            }
            else if(!strcmp(message -> command[id_command].value,"open")){
                #ifdef DEBUG_SHUTTER_XPL
                Serial.print(message -> command[id_device].value);
                Serial.println(F(" opened"));
                #endif
                SHUTTER[position].opening(); // demande arret du volet
            }
            else if(!strcmp(message -> command[id_command].value,"close")){
                #ifdef DEBUG_SHUTTER_XPL
                Serial.print(message -> command[id_device].value);
                Serial.println(F(" closed"));
                #endif
                SHUTTER[position].closing(); // demande arret du volet
            }else if(!strcmp(message -> command[id_command].value,"close")){
                #ifdef DEBUG_SHUTTER_XPL
                Serial.print(message -> command[id_device].value);
                Serial.println(F(" stopped"));
                #endif
                SHUTTER[position].stop(); // demande arret du volet
            }else{ // si pas de correspondance trouvee
                #ifdef DEBUG_SHUTTER_XPL
                Serial.print(message -> command[id_device].value);
                Serial.println(F("   -> NOT FOUND !!!"));
                #endif
            }

            /// nota: le nouveau status est renvoye automatiquement dans une autre routine
        }
        else
        {
            #ifdef DEBUG_SHUTTER_XPL
            Serial.println(F("commandes incompletes"));
            #endif
        }

    #ifdef DEBUG_SHUTTER_XPL
    Serial.println(F("- fin de l'analyse -"));
    #endif
    return 0;


    }

}


void shutter_status( Shutter * _shutter, byte _type){

    /* preparation de la valeur actuelle */
    char _current[3+1]="\0";

    //~ sprintf(_current, "%s%d", _current, _shutter -> current); /// TODO convertir en %

    sprintf(_current, "%s%d", _current, _shutter -> getPosition());


    #ifdef DEBUG_SHUTTER_XPL
    Serial.print(F("status current "));
    Serial.println(_current);
    #endif

    xPL_Message msg;

    msg.hop = 1;

    if(_type == XPL_TRIG){
        msg.type = XPL_TRIG;
    }else{
        msg.type = XPL_STAT;
    }

    msg.SetSource("xplduino","shutter",instance_id);
    msg.SetTarget_P(PSTR("*"));

    msg.SetSchema_P(PSTR("shutter"), PSTR("device"));

    msg.AddCommand("device", _shutter -> getName());
    msg.AddCommand("level",_current);

    xpl.SendMessage(msg.toString());

    #ifdef DEBUG_SHUTTER_XPL
    Serial.println(F("------------  Shutter xPL   ------------"));
    Serial.println(msg.toString());
    #endif

}

// recherche le device dans la liste et renvoie son etat si trouvee
int shutter_request(xPL_Message * message){

#define REQUEST 0

    int8_t cmnd=0; //donnees temporaire pour la recherche des commandes
    char temp[4+1]="\0";    // level ou fade-rate au format char pour conversion en int
    int16_t i=0;
    int16_t nbre= message -> command_count;

    // memorise le numero de la commande
    byte id_request=0;
    byte id_device=0;

    #ifdef DEBUG_SHUTTER_XPL
    Serial.print(F("- analyse de "));
    Serial.print(nbre);
    Serial.println(F(" commandes -"));
    #endif


    for(i=0; i < nbre; i++){


        #ifdef DEBUG_SHUTTER_XPL
        Serial.print(F(" cherche "));
        Serial.println(message -> command[i].name);
        #endif

        if(!strcmp(message -> command[i].name,"request")){
            if(!bitRead(cmnd,REQUEST)){
                bitSet(cmnd,REQUEST);
                id_request=i;
                #ifdef DEBUG_SHUTTER_XPL
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
                #ifdef DEBUG_SHUTTER_XPL
                Serial.println(F("  => ok"));
                #endif
            }else{  // doublon
                return 1;
            }
        }
    }

    //verification que commande goto, device et level sont present. Fade-rate est en option.
    if(bitRead(cmnd,REQUEST) && bitRead(cmnd,DEVICE)){

        //recherche du device shutter
        int position=shutter_find(message -> command[id_device].value, 0);
        if(position>=0)
        {

            int setpoint=0;

            if(!strcmp(message -> command[id_request].value,"devstate")){
                shutter_status(&SHUTTER[i], T_STAT); //STAT
            }
            }else{
                Serial.print(F("  => command unknown: "));
                Serial.print(message -> command[id_request].value);
            }
    }
}


// recherche le device dans la liste et modifie ses parametres suite a un message CONFIG.RESPONSE
int shutter_find(char *_name, int first){

    extern Shutter *SHUTTER;

    // on parcourt la liste des device shutter en commencant par la ligne "first"
    for(int i=first; i < NumberOfShutter; i++){

    #ifdef DEBUG_SHUTTER_XPL
        Serial.print(F("   id "));
        Serial.print(i);
        Serial.print(F(" device "));
        Serial.print(_name);
        Serial.print(F(" compare to '"));
        Serial.println(SHUTTER[i].getName());
    #endif

        if(!strcmp(_name, SHUTTER[i].getName()))    // si device trouve alors on retourne le numero
        {
    #ifdef DEBUG_SHUTTER_XPL
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



void shutter_sendGoto(char *_instance_id, char *_device, byte _new_setpoint){


    char _current[3+1]="\0";

    sprintf(_current, "%s%d", _current, _new_setpoint);


    /* construction du message type commande */

    extern xPL xpl;

    xPL_Message msg;

    msg.hop = 1;
    msg.type = XPL_CMND;

    msg.SetTarget_P(PSTR("*"));
    msg.SetSchema_P(PSTR("shutter"), PSTR("basic"));

    msg.AddCommand("device", _device);
    msg.AddCommand("command", "goto");
    msg.AddCommand("level", _current);

    xpl.SendMessage(&msg);


#ifdef DEBUG_SHUTTER_XPL
    Serial.println(F("---------- xPL Shutter command ----------"));
    Serial.print(_device);
    Serial.print(" current= ");
    Serial.print(_new_setpoint);
    Serial.println("%");
#endif

}

#define CMND_OPEN   1
#define CMND_CLOSE  2
#define CMND_STOP   3
#define CMND_TOGGLE 4

uint8_t shutter_sendOpen(char *_instance_id, char *_device){

    shutter_sendCmd(_instance_id, _device, CMND_OPEN);

}

uint8_t shutter_sendClose(char *_instance_id, char *_device){

    shutter_sendCmd(_instance_id, _device, CMND_CLOSE);

}

uint8_t shutter_sendStop(char *_instance_id, char *_device){

    shutter_sendCmd(_instance_id, _device, CMND_STOP);

}

uint8_t shutter_sendToggle(char *_instance_id, char *_device){

    shutter_sendCmd(_instance_id, _device, CMND_TOGGLE);

}

uint8_t shutter_sendCmd(char *_instance_id, char *_device, byte _command){

    /* construction du message type commande */

    extern xPL xpl;

    xPL_Message msg;

    msg.hop = 1;
    msg.type = XPL_CMND;

    msg.SetTarget_P(PSTR("*"));
    msg.SetSchema_P(PSTR("shutter"), PSTR("basic"));

    msg.AddCommand("device", _device);

    switch(_command){

        case CMND_TOGGLE:
        msg.AddCommand("command", "toggle");
        break;

        case CMND_OPEN:
        msg.AddCommand("command", "open");
        break;

        case CMND_CLOSE:
        msg.AddCommand("command", "close");
        break;

        case CMND_STOP:
        msg.AddCommand("command", "stop");
        break;

        default:
        printDateTime();
        Serial.print(F(" xPL Shutter command unknown: 0x"));
        Serial.println(_command);

        return 0;
    }

    xpl.SendMessage(&msg);


#ifdef DEBUG_SHUTTER_XPL
    Serial.println(F("---------- xPL Shutter command ----------"));
    printDateTime();
    Serial.print(F("-"));
    Serial.print(_device);
    Serial.print(" command=");

    switch(_command){

    case CMND_TOGGLE:
    Serial.println(F("toggle"));
    break;

    case CMND_OPEN:
    Serial.println(F("open"));
    break;

    case CMND_CLOSE:
    Serial.println(F("close"));
    break;

    case CMND_STOP:
    Serial.println(F("stop"));
    break;

    default:
    Serial.println(F("unknown"));

    return 0;
    }

#endif

    return _command;

}

byte aboutShutterXplVersion(){
    return SHUTTER_XPL_VERSION;
}

/*
void shutter_xpl_stopshutter(char *_vendor_id, char *_device_id, char *_instance_id, char *_device){

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
    sprintf(buffer, "%s%s", buffer,"\n}\nshutter.basic\n{\nrequest=stop\ndevice=");
    sprintf(buffer, "%s%s", buffer,_device);
    sprintf(buffer, "%s%s", buffer,"\n}\n");

    ///Udp1.sendPacket(buffer, Config.targetIp, Config.targetPort);

    Serial.print(millis());
    Serial.print(" - SEND shutter_xpl stop command ----> ");
    Serial.println(_device);

}
*/

/*
// recherche le device dans la liste et modifie ses parametres suite a un message CONFIG.RESPONSE
void shutter_xpl_response(struct_command *_command){

    if(!(strcmp(_command[0].name,"device")) && (!strcmp(_command[1].name,"new")))
    {

        //on recherche le device a modifier
        int position=shutter_find(_command[0].value, 0);

        if(position>=0)
        {
            // on recherche si le nouveau nom n existe pas deja
            if(shutter_find(_command[1].value, position+1)<0)
            {
            // TODO: ajouter une condition si champs vide (fin de la liste)
            // TODO: permettre de reaffecter le numero de pin (port+channel) depuis un message xpl

                //~ Serial.print("changed name of device '");
                //~ Serial.print(SHUTTER[position].name);

                sprintf(SHUTTER[position].name,"%s",_command[1].value);

                //~ Serial.print("' in position ");
                //~ Serial.print(position);
                //~ Serial.print(" to ");
                //~ Serial.println(SHUTTER[position].name);

                shutter_xpl_status(TRIG, _command[1].value, "ok");

                // return;
            }
            else
            {
                //~ Serial.print("device name'");
                //~ Serial.print(_command[1].value);
                //~ Serial.println("' already affect to a device !");

                shutter_xpl_status(TRIG, _command[0].value, "Er02");   //  the device is not founded or new name already assigned
            }

        }
        else
        {
            //~ Serial.print("device '");
            //~ Serial.print(_command[0].value);
            //~ Serial.println("' is not founded and can't be changed !");

            shutter_xpl_status(TRIG, _command[0].value, "Er01");   //  the device is not founded or new name already assigned
        }

#endif
    }
}
*/

