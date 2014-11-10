#ifndef shutter_core_h
#define shutter_core_h

#include "Arduino.h"
#include "config.h"

// address of the bits for parameter byte:
#define TYPE_PULSE              1   // par impulsion
#define TYPE_PERMANENT          2   // par maintien relais up et down
#define TYPE_PERMANENT_COMB     4   // par maintien relais: variante à 1 relais marche et 1 relais sélection up ou down

// address of the bits for parameter byte:
#define ADDR_TYPE_PULSE              0   // par impulsion
#define ADDR_TYPE_PERMANENT          1   // par maintien relais up et down
#define ADDR_TYPE_PERMANENT_COMB     2   // par maintien relais: variante à 1 relais marche et 1 relais sélection up ou down
#define SHUTTER_ADDR_STATUS          3   // status flag
#define SHUTTER_POSITION_SAVED       4   // status flag

// address of the bits for status byte:
#define SHUTTER_ADDR_OPEN               0   // open the shutter
#define SHUTTER_ADDR_CLOSE              1   // close the shutter
#define SHUTTER_ADDR_OPEN_PULSE         2   // pulse on the opening output
#define SHUTTER_ADDR_CLOSE_PULSE        3   // pulse on the closing output
#define SHUTTER_ADDR_WRITE_TO_BOARD     4   // write to board flag
#define SHUTTER_ADDR_CMD_DO1            5   // open the shutter or start the motor
#define SHUTTER_ADDR_CMD_DO2            6   // close the shutter or select the direction
#define SHUTTER_ADDR_LAST_DIRECTION     7   // last direction known of the shutter

// macro to read bits inside parameter byte:
#define R_TYPE_PULSE                 bitRead(parameter, ADDR_TYPE_PULSE)
#define R_TYPE_PERMANENT             bitRead(parameter, ADDR_TYPE_PERMANENT)
#define R_TYPE_PERMANENT_COMB        bitRead(parameter, ADDR_TYPE_PERMANENT_COMB)
#define R_STATUS                     bitRead(parameter, SHUTTER_ADDR_STATUS)

// macro to read bits inside status byte:
#define R_OPEN                  bitRead(status, SHUTTER_ADDR_OPEN)
#define R_CLOSE                 bitRead(status, SHUTTER_ADDR_CLOSE)
#define R_OPEN_PULSE            bitRead(status, SHUTTER_ADDR_OPEN_PULSE)
#define R_CLOSE_PULSE           bitRead(status, SHUTTER_ADDR_CLOSE_PULSE)
#define R_CMD_DO1               bitRead(status, SHUTTER_ADDR_CMD_DO1)
#define R_CMD_DO2               bitRead(status, SHUTTER_ADDR_CMD_DO2)
#define R_LAST_DIRECTION        bitRead(status, SHUTTER_ADDR_LAST_DIRECTION)

#define W_STATUS(value)         bitWrite(parameter, SHUTTER_ADDR_STATUS, value)

// macro to write bits inside status byte:
#define W_OPEN(value)           bitWrite(status, SHUTTER_ADDR_OPEN, value)
#define W_CLOSE(value)          bitWrite(status, SHUTTER_ADDR_CLOSE, value)
#define W_OPEN_PULSE(value)     bitWrite(status, SHUTTER_ADDR_OPEN_PULSE, value)
#define W_CLOSE_PULSE(value)    bitWrite(status, SHUTTER_ADDR_CLOSE_PULSE, value)
#define W_CMD_DO1(value)        bitWrite(status, SHUTTER_ADDR_CMD_DO1, value)
#define W_CMD_DO2(value)        bitWrite(status, SHUTTER_ADDR_CMD_DO2, value)
#define W_LAST_DIRECTION(value) bitWrite(status, SHUTTER_ADDR_LAST_DIRECTION, value)


class Shutter
{
    private:

    char    name[16+1];            // name of the shutter
    uint8_t parameter;             // parameter data
    uint8_t status;                // status data
    uint16_t     setpoint;              // setpoint (1=100ms)
    uint16_t     cycle_reference_time;  // reference time for a complete cycle (1=100ms)
    uint16_t     current;               // current position(1=100ms)
    uint8_t impulsion;             // time counter for the pulse function (1=100ms)

    public:

    Shutter(void); // constructor

    uint8_t init(char *name, uint8_t _parameter, uint16_t _cycle_reference_time);
    uint8_t opening();
    uint8_t closing();
    uint8_t new_setpoint(uint8_t _setpoint);
    uint8_t controle();
    uint8_t stop();
    uint8_t post();
    uint8_t reset();
    uint8_t toggle();
    uint8_t postCurrent(uint16_t _new_current);
    uint16_t getCurrent();
    uint8_t getPosition();
    char*   getName();
    uint8_t getSetpoint();
    uint8_t IsSetpointReached();
    uint8_t IsToWrite();
    void postIsToWrite();
    void postIsWrited();
    uint8_t getDO1();
    uint8_t getDO2();

    uint8_t IspostStatusSended();
    void    postStatusSended();

    uint8_t IsPositionSaved();
    void    postPositionSaved();
    void    clearPositionSaved();

    void printConfig(); // renvoi la config du switch

    uint8_t time_to_pourcent(uint16_t _value_to_convert, uint16_t _cycle_reference_time);
    uint16_t pourcent_to_time(uint8_t _value_to_convert, uint16_t _cycle_reference_time);

};

int shutter_post(); // update status of each shutter
int shutter_hard_status();
byte aboutShutterVersion();


#endif

