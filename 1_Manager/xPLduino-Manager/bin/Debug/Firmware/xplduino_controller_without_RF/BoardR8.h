#ifndef BoardR8_h
#define BoardR8_h

#include <avr/io.h>
#include "Arduino.h"
#include "config.h"
#include <stdio.h>



extern int NumberOfBoardR8; // number of instances declared

class Relay8
{
    private:

    uint8_t card_id;      // id of the board
    uint8_t output;       // output status
    uint8_t status;       // status
    uint8_t loss_counter; // number of loss communication

    Relay8(void); // constructor

    public:

    uint8_t init(byte _card_id);
    uint8_t AddLossCounter();
    uint8_t getLossCounter();
    uint8_t read();
    uint8_t write();
    uint8_t newSP(uint8_t _channel, uint8_t _value);   // demande nouvelle consigne
    uint8_t IsValid();
    uint8_t IsNotifiedOK();
    uint8_t IsNotifiedNOK();
    void    postNotifyOK();
    void    postNotifyNOK();

    uint8_t getCardId();

};

void BoardR8_post();
void BoardR8_hbeat();

#endif
