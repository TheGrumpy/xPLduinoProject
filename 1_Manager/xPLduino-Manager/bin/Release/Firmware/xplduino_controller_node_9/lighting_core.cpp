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
 *       version 0.2
 *
 */

#define LIGHTING_CORE_VERSION 6

// v4: ajout default_fade pour revenir à ce fade apres un tune(), lors du stop()
// v5: ajout gotoLastSetpoint() et gotoDefaultSetpoint()
// v6: toggle() -> utilisation default_fade au lieu de dernier fade
// v7: ajout IsOn() et IsOff()

/********************************************************************************************
 * switch_lib
 * bibliotheque gerant les éclairages (bibliotheque lighting_lib)
 * version 0.0 beta
 *
 * init() => initialise les parametres de l'objet boutton
 * update() => sur appel de la fonction, reactualise l'etat du switch
 *
 ********************************************************************************************/

#include "Arduino.h"
#include "lighting_core.h"

//constructor
Lighting::Lighting(){

    last_setpoint=0;         // dernière consigne demandée
    //~ parameter=0;             // octet contenant un ensemble de paramètres (cf détails)
    status=0;                // status (bit0: 1=changement de valeur  en cours)
    fade=0;                  // fade rate par défaut (0=fade interdit)
    setpoint=0;              // consigne demandee
    default_setpoint=0;              // consigne demandee
    current=0;               // consigne en cours

}

// name: init
// @param:
// uint8_t _name       -> name of the device
// uint8_t _parameter  -> 4 MSB is type of card, 4 LSB is not used yet
// uint8_t _DO_address -> address of the physical output, usage depends of the _parameter value (see documentation for more information)
// uint8_t _fade       -> fade rate configuration (0=fade-rate is not allowed)
// @return

uint8_t Lighting::init(char *_name, uint8_t _start_level, uint8_t _default_fade)
{

   if(strlen( _name)>16){
        Serial.print(F("name '"));
        Serial.print(_name);
        Serial.println(F("' exceed 16 characters !"));
    }

    //~ parameter =_parameter;
    //~ DO_address =_DO_address;    // affectation du n° de DO_address
    setpoint=_start_level;          // consigne demandee
    default_setpoint=_start_level;  // consigne demandee
    current=0;               // consigne en cours
    W_LAST_DIRECTION(setpoint); // flag direction si consigne non nulle
    //~ W_LAST_DIRECTION(false); // flag direction si consigne non nulle
    W_SP_CHANGED(true); // flag changement de consigne pour envoi

    W_STATUS(false); // raz flag pour envoi nouvel etat0

    sprintf(name,"%s",_name);   // nom de la variable

    if(_default_fade>0) // si fade autorisée
    {
        default_fade=_default_fade; // mode dimmer: temps de fade lors d'un changement niveau de canal (par pas de 10 ms) ex: 2 x 50 cycles de 10ms= 1s de transition
        fade=_default_fade; // mode dimmer: temps de fade lors d'un changement niveau de canal (par pas de 10 ms) ex: 2 x 50 cycles de 10ms= 1s de transition
        W_FADE(true); // le fading est autorisee
    }
    else
    {
        default_fade=0; // mode dans dimmer
        fade=0; // mode dans dimmer
        W_FADE(false); // le fading est interdit
    }

    //~ setpoint=OFF;       // consigne demandee
    current=OFF;        // consigne actuelle
    last_setpoint=DEFAULT;      // derniere consigne demandee

    printConfig();
    Serial.println();

    return 0;
}

void Lighting::printConfig(){

    Serial.print(F("Light '"));
    Serial.print(name);
    Serial.print(F("', startup= "));
    Serial.print(setpoint, DEC);
    Serial.print(F("%, default fade="));
    Serial.print(default_fade, DEC);
    Serial.print(F(", direction="));
    Serial.print(R_LAST_DIRECTION());

}

// name: toggle
// @param:
// uint8_t _mini -> minimum level to applied
// uint8_t _maxi -> maximum level to applied
// uint8_t _fade -> fade rate to applied
// @return

// la fonction toggle est une fonction télérupteur évolué. Elle permet de basculer l'état de la lumiere entre une consigne mini et une consigne maxi.

uint8_t Lighting::toggle()
{

    toggle(0, 255, default_fade, 0);

}

uint8_t Lighting::toggle(uint8_t _mini, uint8_t _maxi, uint8_t _fade, uint8_t _timer)
{

#ifdef DEBUG_LIGHTING_CORE_
    Serial.println("toggle debut ---");
    Serial.println(setpoint);
    Serial.println(_mini, DEC);
    Serial.println(_maxi, DEC);
    Serial.println(R_LAST_DIRECTION());
    Serial.println("--- toggle fin");
#endif

    // si maxi = 255 alors on utilise la derniere valeur enregistre
    if(_maxi == 255){

        //~ _maxi= constrain(last_setpoint,5,100);
        _maxi= constrain(last_setpoint,0,100);

        if(setpoint>0)
        {
            new_setpoint(_mini, _fade);
        }
        else
        {
            new_setpoint(_maxi, _fade);
        }

    }
    else{
        if(R_LAST_DIRECTION())
        {
            new_setpoint(_mini, _fade);
        }
        else
        {
            new_setpoint(_maxi, _fade);
        }
    }

    return 0;

}

// name: new_setpoint
// @param:
// uint8_t _new_setpoint -> new level to applied
// uint8_t _fade         -> fade rate to applied
// @return

// la fonction new_setpoint permet d'appliquer une nouvelle consigne à la lumière. Dans cette fonction, on met en forme les paramètres pour être compatible.

uint8_t Lighting::new_setpoint(uint8_t _new_setpoint, uint8_t _fade)  // force une nouvelle current sans tenir compte de la current actuelle
{

    // mise en forme bornage des paramètres
    _fade=constrain( _fade, 0, 255);
    _new_setpoint=constrain(_new_setpoint, 0, 100);

    #ifdef DEBUG_LIGHTING_CORE
        Serial.print(name);
        Serial.print(F(" actual setpoint="));
        Serial.print(setpoint);
        Serial.print(F(", new setpoint requested="));
        Serial.print(_new_setpoint);
        Serial.print(F(", actual fade="));
        Serial.print(fade);
        Serial.print(F(", new fade="));
        Serial.print(_fade);
    #endif

    // la nouvelle consigne est-elle différente de l'actuelle ?
    if(setpoint != _new_setpoint){

        W_LAST_DIRECTION(_new_setpoint > setpoint);

        #ifdef DEBUG_LIGHTING_CORE
            Serial.print(F("=> now applied "));
            Serial.print(_new_setpoint);
            Serial.print(F(", direction="));
            Serial.println(R_LAST_DIRECTION());
        #endif

        setpoint=_new_setpoint; // nouvelle consigne

        W_STATUS(false); // raz flag pour envoi nouvel etat
        W_SP_CHANGED(true); // la consigne a changé

        fade=_fade;

    return 0;

    }

    return 1;

}

uint8_t Lighting::new_setpoint(uint8_t _new_setpoint)  // force une nouvelle current sans tenir compte de la current actuelle
{

    // mise en forme bornage des paramètres
    _new_setpoint=constrain(_new_setpoint, 0, 100);

    #ifdef DEBUG_LIGHTING_CORE
        Serial.print(name);
        Serial.print(F(" actual setpoint="));
        Serial.print(setpoint);
        Serial.print(F(", actual fade="));
        Serial.print(fade);
        Serial.print(F(", new setpoint requested="));
        Serial.println(_new_setpoint);
    #endif

    // la nouvelle consigne est-elle différente de l'actuelle ?
    if(setpoint != _new_setpoint){

        W_LAST_DIRECTION(_new_setpoint > setpoint);

    #ifdef DEBUG_LIGHTING_CORE
        Serial.print(F("new setpoint="));
        Serial.print(_new_setpoint);
        Serial.print(F(", sens="));
        Serial.println(R_LAST_DIRECTION());
    #endif

        setpoint=_new_setpoint; // nouvelle consigne

        W_STATUS(false); // raz flag pour envoi nouvel etat
        W_SP_CHANGED(true); // la consigne a changé

        return 0;

    }

    return 1;

}


uint8_t Lighting::tune(void)        // demande variation lente
{

    toggle(0, 100, 8, 0);

    return 0;
}

uint8_t Lighting::gotoLastSetpoint(void)        // demande dernier consigne
{

    new_setpoint(last_setpoint);

    return 0;
}

uint8_t Lighting::gotoDefaultSetpoint(void)        // demande dernier consigne
{

    new_setpoint(default_setpoint);

    return 0;
}

uint8_t Lighting::stop(void)        // force de l'arret de la variation
{

    if(current != setpoint){
        uint8_t direction=(current < setpoint); // memorisation de la direction pour mise à jour apres newsetpoint, sinon on ne sait pas changer de sens. A revoir pour faire plus propre.
        last_setpoint=constrain(current, 5, 100); // minimum de 5%
        new_setpoint(current, default_fade);
        W_LAST_DIRECTION(direction);
    }

    return 0;
}


uint8_t Lighting::IsOn(){

    return current>0;

}

uint8_t Lighting::IsOff(){

    return current==0;

}

uint8_t Lighting::IsChanging(){

    return current != setpoint;

}

uint8_t Lighting::IsFlagSPChanged(){

    return bitRead(status, LIGHTING_ADDR_SP_CHANGED);

}

void Lighting::ResetFlagSPChanged(){

    bitClear(status, LIGHTING_ADDR_SP_CHANGED);

}

uint8_t Lighting::IspostStatusSended(){

    return bitRead(status, LIGHTING_ADDR_STATUS);

}

void Lighting::postStatusSended(){

    bitSet(status, LIGHTING_ADDR_STATUS);

}

uint8_t Lighting::getSetpoint(){

    return setpoint;

}

uint8_t Lighting::getFade(){

    return fade;

}

uint8_t Lighting::getCurrent(){

    return current;

}

void Lighting::postCurrent(uint8_t _current){

    current=_current;

}

char* Lighting::getName(){

    return name;

}

extern Lighting *LIGHTING;
extern void lighting_status(Lighting * _lighting, uint8_t _type);

extern uint8_t NumberOfLighting; // number of instances declared

uint8_t aboutLightingVersion(){
    return LIGHTING_CORE_VERSION;
}

uint8_t lighting_hard_status(){

    for(uint8_t i=0; i < NumberOfLighting; i++){

        if(!LIGHTING[i].IspostStatusSended()){
            lighting_status(&LIGHTING[i], T_TRIG); // TRIG
                Serial.print("Light #");
                Serial.print(i);
                Serial.print(",'");
                Serial.print(LIGHTING[i].getName());
                Serial.print("', sp=");
                Serial.print(LIGHTING[i].getSetpoint());
                Serial.print("%, current=");
                Serial.print(LIGHTING[i].getCurrent());
                Serial.println("%");
            if(LIGHTING[i].getCurrent() == LIGHTING[i].getSetpoint())
                LIGHTING[i].postStatusSended();
        }

    }

    return 0;

}




