#include "Arduino.h"

#include "BoardIn4Dimmer4.h"
#include "BoardR8.h"
#include "BoardIn16.h"
#include "BoardIn8R8.h"

#include "switch_core.h"
#include "switch_xpl.h"
#include "lighting_core.h"

/*       Part of the xplduino project - http://code.google.com/p/xplduino/
 *       Write by Romain TISSOT CHARLOD
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
 *
 */

extern Switch *SWITCH;
extern In4Dimmer4 *IN4DIM4;
extern Relay8 *R8;
extern Lighting *LIGHTING;
extern Input16 *IN16;
extern In8R8 *IN8R8;

extern int NumberOfPre; // number of instances declared

#define PRE_VERSION 0


void dimmer_to_switch(uint8_t switch_id, uint8_t card_id, uint8_t card_channel){

    if (SWITCH[switch_id].update(IN4DIM4[card_id].inputIsSet(card_channel))) // si l'update renvoi une valeur >0 alors c'est qu'il y a eu un chgt
        switch_status(&SWITCH[switch_id], T_TRIG); //TRIG

}

void dimmer_to_lighting(uint8_t lighting_id, uint8_t card_id, uint8_t card_channel){

    // on doit mettre à jour une instance de lighting
    //~ if(LIGHTING[lighting_id].current != LIGHTING[lighting_id].setpoint)
    if(LIGHTING[lighting_id].IsChanging())
        LIGHTING[lighting_id].postCurrent(IN4DIM4[card_id].ReadCurrent(card_channel));
        //~ LIGHTING[lighting_id].postCurrent(LIGHTING[lighting_id].getSetpoint());

    /// il faudra probablement ne pas lire si changement ok

}

void in16_to_switch(uint8_t switch_id, uint8_t card_id, uint8_t card_channel){

    if (SWITCH[switch_id].update(IN16[card_id].inputIsSet(card_channel))) // si l'update renvoi une valeur >0 alors c'est qu'il y a eu un chgt
        switch_status(&SWITCH[switch_id], T_TRIG); //TRIG

}

void R8_to_lighting(uint8_t lighting_id, uint8_t card_id, uint8_t card_channel){

    LIGHTING[lighting_id].postCurrent(LIGHTING[lighting_id].getSetpoint());
    /// il faudra recuperer le vrai etat

}

void in8R8_to_lighting(uint8_t lighting_id, uint8_t card_id, uint8_t card_channel){

    // on doit mettre à jour une instance de switch
    LIGHTING[lighting_id].postCurrent(LIGHTING[lighting_id].getSetpoint());
    /// il faudra recuperer le vrai etat
}

void in8R8_to_switch(uint8_t switch_id, uint8_t card_id, uint8_t card_channel){

    if(IN8R8[card_id].IsValid()){ // on verifie d'abord que la com avec la carte est ok
        if (SWITCH[switch_id].update(IN8R8[card_id].inputIsSet(card_channel))) // si l'update renvoi une valeur >0 alors c'est qu'il y a eu un chgt
            switch_status(&SWITCH[switch_id], T_TRIG); //TRIG
    }

}


byte aboutPreVersion(){
    return PRE_VERSION;
}


