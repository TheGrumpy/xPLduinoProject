#include "Arduino.h"
#include "post.h"

#include "BoardIn4Dimmer4.h"
#include "BoardR8.h"
#include "BoardIn16.h"
#include "BoardIn8R8.h"

#include "switch_core.h"
#include "switch_xpl.h"
#include "lighting_core.h"
#include "shutter_core.h"

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
extern Shutter *SHUTTER;
extern In4Dimmer4 *IN4DIM4;
extern Relay8 *R8;
extern Lighting *LIGHTING;
extern Input16 *IN16;
extern In8R8 *IN8R8;

#define POST_VERSION 2

void lighting_to_dimmer(uint8_t lighting_id, uint8_t card_id, uint8_t card_channel){

    if(LIGHTING[lighting_id].IsFlagSPChanged()){
        IN4DIM4[card_id].newSP(card_channel, LIGHTING[lighting_id].getSetpoint(),LIGHTING[lighting_id].getFade());
        LIGHTING[lighting_id].ResetFlagSPChanged();
    }

}

void lighting_to_R8(uint8_t lighting_id, uint8_t card_id, uint8_t card_channel){

    if(LIGHTING[lighting_id].IsFlagSPChanged()){
        R8[card_id].newSP(card_channel, LIGHTING[lighting_id].getSetpoint());
        LIGHTING[lighting_id].ResetFlagSPChanged();
    }

}

void lighting_to_in8R8(uint8_t lighting_id, uint8_t card_id, uint8_t card_channel){

    if(LIGHTING[lighting_id].IsFlagSPChanged()){
        IN8R8[card_id].newSP(card_channel, LIGHTING[lighting_id].getSetpoint());
        LIGHTING[lighting_id].ResetFlagSPChanged();
    }

}

void shutter_to_R8(uint8_t shutter_id, uint8_t card_id, uint8_t DO1_card_channel, uint8_t DO2_card_channel){

    if(SHUTTER[shutter_id].IsToWrite()){
        R8[card_id].newSP(DO1_card_channel, SHUTTER[shutter_id].getDO1());
        R8[card_id].newSP(DO2_card_channel, SHUTTER[shutter_id].getDO2());
        SHUTTER[shutter_id].postIsWrited();
    }

}

void shutter_to_in8R8(uint8_t shutter_id, uint8_t card_id, uint8_t DO1_card_channel, uint8_t DO2_card_channel){

    if(SHUTTER[shutter_id].IsToWrite()){
        IN8R8[card_id].newSP(DO1_card_channel, SHUTTER[shutter_id].getDO1());
        IN8R8[card_id].newSP(DO2_card_channel, SHUTTER[shutter_id].getDO2());
        SHUTTER[shutter_id].postIsWrited();
    }

}

void write_in8R8(uint8_t value, uint8_t card_id, uint8_t card_channel){

        IN8R8[card_id].newSP(card_channel, value);

}

void set_in8R8(uint8_t card_id, uint8_t card_channel){

        IN8R8[card_id].newSP(card_channel, true);

}

void clear_in8R8(uint8_t card_id, uint8_t card_channel){

        IN8R8[card_id].newSP(card_channel, false);

}


byte aboutPostVersion(){
    return POST_VERSION;
}


