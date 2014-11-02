#ifndef BoardIn4Dimmer4_h
#define BoardIn4Dimmer4_h

#include <avr/io.h>
#include "Arduino.h"
#include "config.h"
#include <stdio.h>


extern int NumberOfBoardIn4Dimmer4; // number of instances declared

class In4Dimmer4
{

    private:

    uint8_t   card_id;      // id
    uint8_t   loss_counter; // number of loss communication
    uint8_t   hbeat;        // valeur du hbeat


    uint8_t   setpoint[4];  // Setpoint channel i
    uint8_t   fade[4];      // Setpoint channel i
    uint8_t   input;        // Input status
    uint8_t   status;       // status
    uint8_t   status2;      // status extension


    In4Dimmer4(void); // constructor

    public:
    uint8_t IsValid();
    uint8_t IsNotifiedOK();
    uint8_t IsNotifiedNOK();
    void    postNotifyOK();
    void    postNotifyNOK();
    void    SetComIsOK();
    void    ClearComIsOK();


    uint8_t init(byte _card_id);
    uint8_t inputIsSet(byte _id);  // lecture de l'état d'une entrée
    uint8_t newSP(byte _channel, byte _SP, byte _fade); // demande nouvelle consigne
    uint8_t manageHbeat(byte _hbeat_value); // echange des hbeat
    uint8_t aboutVersion();
    uint8_t aboutZC();
    uint8_t ReadCurrent(uint8_t _card_channel);
    uint8_t readInput();  // mise à jour des E/S
    uint8_t IsChannelChanged(uint8_t _channel);
    uint8_t writeSP(byte _channel); // demande nouvelle consigne
    uint8_t AddLossCounter();
    uint8_t getLossCounter();
    uint8_t getCardId();
    uint8_t getHBeat();
    uint8_t getInput(); // renvoi l'etat des entrees

    private:
    uint8_t readSP(byte _channel); //  recupere valeur actuelle
    uint8_t getSetpoint(byte _channel); //  recupere valeur actuelle


    uint8_t readCommand(byte _command);
    uint8_t writeCommand(byte _command, byte _value);

    uint8_t compute_lrc(uint8_t _data);





};

extern byte hbeat; // compteur hbeat

void BoardIn4Dimmer4_pre();
void BoardIn4Dimmer4_post();
void BoardIn4Dimmer4_hbeat();


#endif
