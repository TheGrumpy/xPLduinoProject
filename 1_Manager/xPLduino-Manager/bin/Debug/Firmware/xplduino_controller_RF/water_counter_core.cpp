

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
 *       version 0: creation
 *       version 1: ajout support TP et TOF
 *
 */

#include "water_counter_core.h"

#define WATER_COUNTER_CORE_VERSION 0


// Constructor
water_counter::water_counter(void){}

// this routin, called one time at the start-up, initialize the command
uint8_t water_counter::init(char *_name, uint8_t _ratio, uint8_t _storage_address)
{

    sprintf(name,"%s",_name);

    status=0;                // status register
    storage_address=_storage_address;       // where is store the water_counter value
    current=0;               // current value

    ratio=_ratio;

    if(ratio==0)
        ratio=1;

    //~ current=readEeprom(); // au démarrage on relit la valeur stockée

    return 1;
}

// return the water_counter value
uint16_t water_counter::getValue()
{
    return current;

}

// return the water_counter value
uint8_t water_counter::getTempValue()
{
    return temp_current;

}

char* water_counter::getName(){

    return name;

}

// this routin reset the water_counter
void water_counter::reset()
{
    current=0;

}

// this routin increase the water_counter
void water_counter::ctu()
{
    current++;

}

// this routin increase the water_counter with the value
void water_counter::ctu(uint8_t _value)
{

    temp_current=temp_current+_value; // on incremente le compteur intermediaire

    if(temp_current >= ratio){ // si le compteur depasse
        //~ Serial.println("temp_current >= ratio");
        current=current+(temp_current/ratio);
        //~ Serial.print("current=");
        //~ Serial.println(current);
        temp_current=temp_current % ratio;
        //~ Serial.print("reliquat=");
        //~ Serial.println(temp_current);

    }


}

uint8_t aboutwaterCounterVersion(){
    return WATER_COUNTER_CORE_VERSION;
}
