#ifndef BoardIn16_h
#define BoardIn16_h

#include <avr/io.h>
#include "Arduino.h"
#include "config.h"
#include <stdio.h>


extern int NumberOfBoardIn16; // number of instances declared

class Input16
{
    public:
    uint8_t card_id_0;     // id
    uint8_t card_id_1;     // id
    uint8_t loss_counter; // number of loss communication

    private:

    uint8_t input_0;      // input status
    uint8_t input_1;      // input status
    uint8_t status;      // status

    Input16(void); // constructor

    public:

    uint8_t init(uint8_t _card_id_0, uint8_t _card_id_1);
    uint8_t inputIsSet(uint8_t _channel); //  recupere valeur actuelle
    uint8_t read();
    uint8_t IsValid();
    uint8_t IsNotifiedOK();
    uint8_t IsNotifiedNOK();
    void    postNotifyOK();
    void    postNotifyNOK();

};

void BoardIn16_pre();
void BoardIn16_hbeat();

#endif
