#include "Arduino.h"
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
 *       version 0.2
 *
 */

#define SHUTTER_CORE_VERSION 6


// version 4: ajout config()
// version 5: ajout uint8_t IsPositionSaved(); void    postPositionSaved(); void    clearPositionSaved(); uint8_t postCurrent(uint16_t _new_current); et change int en uint16_t
// version 6: postCurrent(): ajout configuraton du flag last_direction en fonction de la valeur forcée

// Constructor
Shutter::Shutter(void)
{

    parameter=0;             // parameter data
    status=0;                // status data
    setpoint=0;              // setpoint (1=100ms)
    cycle_reference_time=0;  // reference time for a complete cycle (1=100ms)
    current=0;               // current position(1=100ms)
    impulsion=0;             // time counter for the pulse function (1=100ms)

}

// this routin, called one time at the start-up, initialize the shutter

uint8_t Shutter::init(char *_name, uint8_t _parameter, uint16_t _cycle_reference_time)
{

   if(strlen( _name)>16){
        Serial.print(F("name '"));
        Serial.print(_name);
        Serial.println(F("' exceed 16 characters !"));
    }


    parameter=_parameter;   // parameters (type of shutter...)

    setpoint=0;     // setpoint

    status=0x00;    // reset all status

    cycle_reference_time=_cycle_reference_time;    // init the time of a complete cycle (0 to 100%)

    impulsion=0;    // reset the time counter

    current=0;      // reset the position counter

    sprintf(name,"%s",_name);

    printConfig();
    Serial.println();


    return 1;
}


void Shutter::printConfig(){

    Serial.print(F("Shutter '"));
    Serial.print(name);

    if(R_TYPE_PULSE)
        Serial.print(F("', type pulse"));
    else if(R_TYPE_PERMANENT)
        Serial.print(F(", type permanent"));
    else if(R_TYPE_PERMANENT)
        Serial.print(F("', type permanent"));
    else
        Serial.print(F("', type inconnu"));
    Serial.print(", cycle in ms=");
    Serial.print(cycle_reference_time*100);

}

uint8_t Shutter::toggle()
{

    if(R_LAST_DIRECTION)
    {
        closing();
    }
    else
    {
        opening();
    }

    return 0;

}

uint8_t Shutter::opening()
{

    // compare the actual position to the maximum position

    // if it's ok, we continue
    if(current<cycle_reference_time){

        if(!R_OPEN && !R_CLOSE){    // if there is no closing and opening then we request a full opening (100%)
            new_setpoint(100);
        }
        else    // else we reset the actual command by a stop command
        {
            stop();
        }

    }
    else    // else, the shutter is already fully opened, we reset all the command
    {
        W_OPEN(false);
        W_CLOSE(false);
        return 0;
    }

return 1;

}

uint8_t Shutter::closing()
{

    // compare the actual position to the minimum position

    // if it's ok, we continue
    if(current> 0){

        if(!R_CLOSE && !R_OPEN){    // if there is no closing and opening then we request a full closing (0%)
            new_setpoint(0);
        }
        else    // else we reset the actual command by a stop command
        {
            stop();
        }

    }
    else{    // else, the shutter is already fully closed, we reset all the command
        W_OPEN(false);
        W_CLOSE(false);
        return 0;
    }

    return 1;

}


uint8_t Shutter::new_setpoint(uint8_t _setpoint)
{

    setpoint=pourcent_to_time(_setpoint,cycle_reference_time); // convert the setpoint in % to a setpoint in time (1=100ms)
    setpoint=constrain(setpoint, 0, cycle_reference_time); // limit the setpoint to the maximum value possible (i.e. the reference time of a cycle

#ifdef DEBUG_SHUTTER_CORE
    Serial.print("Move of ");
    Serial.print(name);
    Serial.print(" from ");
    Serial.print(time_to_pourcent(current,cycle_reference_time));
    Serial.print(" % to ");
    Serial.print(time_to_pourcent(_setpoint,cycle_reference_time));
    Serial.println(" %");
#endif

    // compare the actual position to the new setpoint

    if(current>setpoint){   // case where the new setpoint is lower than actual position

        if(!R_CLOSE) // if there is no closing command, then we request a closing command (pulse)
        {
        #ifdef DEBUG_SHUTTER_CORE
            Serial.print("demande descente de ");
            Serial.print(name);
            Serial.print(" de ");
            Serial.print(current);
            Serial.print(" a ");
            Serial.println(setpoint);
        #endif
            W_CLOSE_PULSE(true);
        }

        W_OPEN(false);              // we reset any opening command
        W_CLOSE(true);              // we set the closing command
        W_STATUS(false);            // we reset the flag "status"
        W_LAST_DIRECTION(false);    // we reset the last direction flag
        postIsToWrite();
        clearPositionSaved(); // we reset the flag to save in rom
    }
    else if(current<setpoint){  // case where the new setpoint is superior than actual position

        if(!R_OPEN){ // if there is no opening command, then we request a opening command (pulse)
        #ifdef DEBUG_SHUTTER_CORE
            Serial.print("demande montee de ");
            Serial.print(name);
            Serial.print(" de ");
            Serial.print(current);
            Serial.print(" a ");
            Serial.println(setpoint);
        #endif
            W_OPEN_PULSE(true);
        }

        W_OPEN(true);               // we set the opening command
        W_CLOSE(false);             // we reset any closing command
        W_STATUS(false);            // we reset the flag "status"
        W_LAST_DIRECTION(true); // we set the last direction flag
        postIsToWrite();
        clearPositionSaved(); // we reset the flag to save in rom
    }
    else    // case where the setpoint doesn't change
    {
        W_OPEN(false);
        W_CLOSE(false);
        return 0;
    }

    return 1;
}

// this routin must be called every 100 ms (for each shutter).
// it's permit to update the status of the shutter.
// if the setpoint is reached, then the shutter is automaticaly stopped

uint8_t Shutter::controle(void)
{
    // increment or decrement the position counter in function of the closing or opening command
    if(R_CLOSE){
        current=current-1;
    }
    if(R_OPEN){
        current=current+1;
    }

    // increment the pulse counter
    if(R_OPEN_PULSE ||  R_CLOSE_PULSE){
        impulsion++;
    }

    // automatic stop command if the setpoint is reached
    if((current>=setpoint) && R_OPEN){  // automatic stop of the opening cycle
        stop();
    }

    if((current<=setpoint) && R_CLOSE){ // automatic stop of the closing cycle
        stop();
    }

    return 1;
}


// this routin execute a stop command

uint8_t Shutter::stop()
{

    // we check if a closing or opening cycle is actually running

    if(R_OPEN || R_CLOSE){
    #ifdef DEBUG_SHUTTER_CORE
        Serial.print("Stop of ");
        Serial.print(name);
        Serial.print(" at ");
        Serial.print(time_to_pourcent(current,cycle_reference_time));
        Serial.println(" %");
    #endif
        setpoint = current; // the current position is forced to setpoint. TODO: check if it's always necessary ?

        if(R_OPEN){ // case of opening cycle
            W_OPEN(false);          // reset the opening command
            W_OPEN_PULSE(true);     // set a pulse on the open output to stop the shutter
        }
        if(R_CLOSE){ // case of closing cycle
            W_CLOSE(false);         // reset the closing command
            W_CLOSE_PULSE(true);    // set a pulse on the open output to stop the shutter
        }
        postIsToWrite();
    }

    return 1;
}


// this routin must be called every 100 ms (for each shutter).
// it manage the output 1 and output 2 in function of the type of shutter.

uint8_t Shutter::post()
{


    // #### case of management by pulse ####
    // the output up/down is set to true for 500ms:
    // - at the beginning of the cycle of up/down
    // - when the open/close command disappears

    // #### case of management by direct control of the motor ####
    // the output open/close is set to true until the open/close command disappears

    // #### case of management by direct control of the motor with electromechanical locked ####
    // there is one output for the on/off (DO1) command and one output for the direction opening/closing (DO2).
    // - the output DO1 is set to true until the open/close command disappears
    // - the output DO2 is set to true if it's a close command and until the close command disappears


    if(R_TYPE_PULSE){   // case of management by pulse

        if(R_OPEN_PULSE && impulsion<5 ){
            W_CMD_DO1(HIGH);
        }
        else
        {
            W_OPEN_PULSE(false);
            W_CMD_DO1(LOW);
        }

        if(R_CLOSE_PULSE && impulsion<5 ){
            W_CMD_DO2(HIGH);
        }
        else
        {
            W_CLOSE_PULSE(false);
            W_CMD_DO2(LOW);
        }

        if(!R_OPEN_PULSE && !R_CLOSE_PULSE){
            impulsion=0;
        }

    } else if(R_TYPE_PERMANENT){    // case of management by direct control of the motor

        if(R_OPEN){
            W_CMD_DO1(HIGH);
        }
        else
        {
            W_OPEN_PULSE(false);
            W_CMD_DO1(LOW);
        }

        if(R_CLOSE){
            W_CMD_DO2(HIGH);
        }
        else
        {
            W_CLOSE_PULSE(false);
            W_CMD_DO2(LOW);
        }
    } else if(R_TYPE_PERMANENT_COMB){   // case of management by direct control of the motor with electromechanical locked

        if(R_OPEN || R_CLOSE) {
            W_CMD_DO1(HIGH);
        }
        else
        {
            W_CMD_DO1(LOW);
            W_CMD_DO2(LOW);
        }

        if(R_OPEN){
            W_CMD_DO2(LOW);
        }
        else
        {
            W_OPEN_PULSE(false);
        }

        if(R_CLOSE){
            W_CMD_DO2(HIGH);
        }
        else
        {
            W_CLOSE_PULSE(false);
        }

    }

    return 1;
}

// this routin manage the reset command
// basicaly, if a reset command is requested,
// we force the current value to the maximum
// and we request a closing command

uint8_t Shutter::reset()
{
#ifdef DEBUG_SHUTTER_CORE
    Serial.print("Init of ");
    Serial.print(name);
#endif

    current=cycle_reference_time;
    closing();

    return 1;
}

uint16_t Shutter::getCurrent()
{

    return current;

}

uint8_t Shutter::postCurrent(uint16_t _new_current)
{

    current=constrain(_new_current, 0, cycle_reference_time); // limit the setpoint to the maximum value possible (i.e. the reference time of a cycle

    W_LAST_DIRECTION(current);    // we reset the last direction flag

    return current;

}

uint8_t Shutter::IsSetpointReached()
{

    return (current==setpoint);

}

void Shutter::postIsToWrite()
{

    bitSet(status, SHUTTER_ADDR_WRITE_TO_BOARD);

}

uint8_t Shutter::IsToWrite()
{

    return bitRead(status, SHUTTER_ADDR_WRITE_TO_BOARD);

}

void Shutter::postIsWrited()
{

    bitClear(status, SHUTTER_ADDR_WRITE_TO_BOARD);

}



// this routin convert a time (current value for example) to a value in pourcent (0-100%)

uint8_t Shutter::time_to_pourcent(uint16_t _value_to_convert, uint16_t _cycle_reference_time){

    float _pourcent=_value_to_convert*100;
    _pourcent=_pourcent/_cycle_reference_time;

return _pourcent;

}

// this routin convert a pourcent (0-100%) to a value in millisecond (1=100 ms)

uint16_t Shutter::pourcent_to_time(uint8_t _value_to_convert, uint16_t _cycle_reference_time){

    float _time=_value_to_convert*_cycle_reference_time;
    _time=_time/100;

return _time;

}

// this routin return the current position in pourcent (0-100%)

uint8_t Shutter::getPosition(){

    float _pourcent=current*100/cycle_reference_time;

return _pourcent;

}

char* Shutter::getName(){

    return name;

}


uint8_t Shutter::IspostStatusSended(){

    return bitRead(parameter, SHUTTER_ADDR_STATUS);

}

void Shutter::postStatusSended(){

    bitSet(parameter, SHUTTER_ADDR_STATUS);

}


uint8_t Shutter::IsPositionSaved(){

    return bitRead(parameter, SHUTTER_POSITION_SAVED);

}

void Shutter::postPositionSaved(){

    bitSet(parameter, SHUTTER_POSITION_SAVED);

}

void Shutter::clearPositionSaved(){

    bitClear(parameter, SHUTTER_POSITION_SAVED);

}

uint8_t Shutter::getDO1(){

    return bitRead(status, SHUTTER_ADDR_CMD_DO1);

}

uint8_t Shutter::getDO2(){

    return bitRead(status, SHUTTER_ADDR_CMD_DO2);

}



extern Shutter* SHUTTER;
extern void shutter_status( Shutter * _shutter, byte _type);

extern int NumberOfShutter; // number of instances declared

byte aboutShutterVersion(){
    return SHUTTER_CORE_VERSION;
}

int shutter_post(){

    for(int i=0; i < NumberOfShutter; i++){
        SHUTTER[i].controle();
        SHUTTER[i].post();
    }

    return 0;

}

int shutter_hard_status(){

    for(int i=0; i< NumberOfShutter; i++){


        if(!SHUTTER[i].IspostStatusSended()) // if is set equal to the processed (no change in the loop)
        {
            shutter_status(&SHUTTER[i], T_TRIG); // TRIG

            if(SHUTTER[i].IsSetpointReached())
                SHUTTER[i].postStatusSended();

        }

    }

    return 0;

}
