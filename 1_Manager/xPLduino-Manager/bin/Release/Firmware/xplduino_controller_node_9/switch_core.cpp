//      switch_core.cpp
//
//      Copyright 2012 Romain TISSOT CHARLOD <romain@romain-laptop>
//
//      This program is free software; you can redistribute it and/or modify
//      it under the terms of the GNU General Public License as published by
//      the Free Software Foundation; either version 2 of the License, or
//      (at your option) any later version.
//
//      This program is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU General Public License for more details.
//
//      You should have received a copy of the GNU General Public License
//      along with this program; if not, write to the Free Software
//      Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
//      MA 02110-1301, USA.

/********************************************************************************************
 * switch_lib
 * bibliotheque gerant les boutons poussoirs (bibliotheque switch_lib)
 * version 0.0
 *
 * init() => initialise les parametres de l'objet boutton
 * update() => sur appel de la fonction, reactualise l'etat du switch
 *
 ********************************************************************************************/


#include "Arduino.h"
#include "switch_core.h"
#include <stdio.h>

#define SWITCH_CORE_VERSION 0.0

//constructor
Switch::Switch(){

    maintained_delay=0;      // délai en ms button pour considérer comme mauint8_tenu
    timer_maintained=0;      // compteur de temps incrémenté tant que l’entrée est à 1 (x100ms)
    timer_doublepulse=0;     // compteur de temps décrémenté dès qu'une impulsion est détecté
    status=0;                // octet contenant les états calculés (cf détails)
    parameter=0;             // octet contenant un ensemble de paramètres (cf détails)

}

uint8_t Switch::init(char *_name, uint8_t _parameter, uint8_t _maintained_delay)
{

   if(strlen( _name)>16){
        Serial.print(F("Error: name '"));
        Serial.print(_name);
        Serial.println(F("' exceed 16 characters !"));
        W_INIT_OK(false);
        return 0;
    }

    parameter=_parameter;

    //lecture du type d'entrée (niveau haut ou niveau bas) pour ajuster le flag de changement d'état.
    //evitant ainsi de detecter un faux changement d'état au premier update du switch
    W_LEVEL(not R_HIGH);

    //raz des bits d'etat
    W_PULSE(false);
    W_DPULSE(false);
    W_ON(false);
    W_ON_OSR(false);
    W_ON_OSF(false);
    W_TEMP(false);

    maintained_delay=_maintained_delay; // valeur pour declarer une entree en appui long
    timer_maintained=0;  // init timer detection mauint8_tien
    timer_doublepulse=0; // init timer double pulse

    sprintf(name,"%s",_name);

    printConfig();

    W_INIT_OK(true);

    return 1;
}


uint8_t Switch::update(uint8_t _new_level)
{

    uint8_t return_value=0;

    W_PULSE(false);   // RAZ de la memoire appuie pulse
    W_DPULSE(false);  // RAZ de la memoire appuie double pulse
    W_ON_OSF(false);  // RAZ Front descendant 'on'
    W_ON_OSR(false);  // RAZ Front montant 'on'

    W_TEMP(_new_level);    // on stocke le niveau de l'entree temporairement

    if (R_TEMP != R_LEVEL){  // Traitement du changement d'etat de l'entree

        if (R_TEMP!=R_HIGH && !R_ON){   // cas haut => bas, relachement du switch avant d'atteindre l'etat "on"
            if(timer_doublepulse>0){
                W_DPULSE(true);  // info "appuie double pulse" pour un cycle
                return_value=ADDR_DPULSE;
                timer_doublepulse=0;
            #ifdef DEBUG_SWITCH_CORE
                Serial.print(F("Sw#dpulse-"));
                Serial.println(name);
            #endif

            }else{
                W_PULSE(true);  // info "appuie pulse" pour un cycle
                timer_doublepulse=maintained_delay; // sur detection d'un pulse, on lance le timer doublepulse: si un nouveau pulse est detecté avant la fin du décompteur, ce sera un double pulse
                return_value=ADDR_PULSE;
            #ifdef DEBUG_SWITCH_CORE
                Serial.print(F("Sw#pulse-"));
                Serial.println(name);
            #endif

            }
        }
        else if (R_TEMP!=R_HIGH && R_ON){ // cas haut => bas, relachement du switch suite etat "on"
            W_ON_OSF(true);  // Front montant switch released
            return_value=ADDR_ON_OSF;
        #ifdef DEBUG_SWITCH_CORE
            Serial.print(F("Sw#off-"));
            Serial.println(name);
        #endif

        }

        W_LEVEL(R_TEMP); // a la fin du traitement on copie le nouvel etat de l'entree

    }

    if( R_LEVEL==R_HIGH ){   //  Traitement selon etat de l'entree

        if(R_ON==0){

            timer_maintained=timer_maintained++; // incremente le compteur a chaque appel de routine tous les 100 ms

            if (timer_maintained > maintained_delay){ // si appuie sur le switch depuis plus de x ms et pas encore detecte mauint8_tenu
                W_ON(true);     // info "appuie mauint8_tenu" jusqu'au relachement du switch
                W_ON_OSR(true); // info "mode mauint8_tenu" un seul cycle

                return_value=ADDR_ON_OSR;
            #ifdef DEBUG_SWITCH_CORE
                Serial.print(F("Sw#on-"));
                Serial.println(name);
            #endif

            }
        }

    }
    else{  // si switch released
        W_ON(false);         // RAZ de la memoire "appuie mauint8_tenu"
        timer_maintained=0;  // RAZ du compteur de duree d'appui
    }

    // gestion du timer de détection du double impulsion
    if(timer_doublepulse>0){
        timer_doublepulse--;
    }

    return return_value;
}


void Switch::printConfig(){

    Serial.print(F("Switch #'"));
    Serial.print(name);
    Serial.print(F("', high if "));
    Serial.print(R_HIGH, BIN);
    Serial.print(F(" during "));
    Serial.print(maintained_delay, DEC);
    Serial.println(F(" ms"));

}

uint8_t Switch::isPulse(){

    return R_PULSE;

}

uint8_t Switch::isDoublePulse(){

    return R_DPULSE;

}

uint8_t Switch::isOn(){

    return R_ON;

}

uint8_t Switch::isOnOSR(){

    return R_ON_OSR;

}

uint8_t Switch::isOff(){

    return not R_ON;

}

uint8_t Switch::isOnOSF(){

    return R_ON_OSF;

}

uint8_t Switch::getStatus(){

    return status;

}

char* Switch::getName(){

    return name;

}

uint8_t aboutSwitchVersion(){
    return SWITCH_CORE_VERSION;
}



