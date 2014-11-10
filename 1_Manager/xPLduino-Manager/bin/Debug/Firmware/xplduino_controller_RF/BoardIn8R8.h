#ifndef BoardIn8R8_h
#define BoardIn8R8_h

#include <avr/io.h>
#include "Arduino.h"
#include "config.h"
#include <stdio.h>



extern int NumberOfBoardIn8R8; // number of instances declared

class In8R8
{

    private:


    uint8_t   card_id;     // id
    uint8_t   hbeat;       // valeur du hbeat
    uint8_t   input;       // Inputs status
    uint8_t   reply_input; // Inputs reply value
    uint8_t   output;      // output status
    uint8_t   status;      // status
    uint8_t   loss_counter; // number of loss communication
    uint8_t   bad_lrc;      // number of bad lrc

    In8R8(void); // constructor

    public:

    uint8_t init(uint8_t _card_id, uint8_t _reply_input);
    uint8_t inputIsSet(uint8_t _id);  // lecture de l'état d'une entrée

    uint8_t IsOutputsChanged();
    uint8_t IsValid();
    uint8_t IsNotifiedOK();
    uint8_t IsNotifiedNOK();
    void    postNotifyOK();
    void    postNotifyNOK();
    void    SetComIsOK();
    void    ClearComIsOK();

    uint8_t newSP(uint8_t _channel, uint8_t _SP); // demande nouvelle consigne

    uint8_t getSetpoint(uint8_t _channel); //  recupere valeur actuelle

    uint8_t write(); //  écrit nouvel etat des sorties

    uint8_t readInput();   // mise à jour des E/S
    uint8_t readOutput();  // lecture etat des sorties
    uint8_t readCounter(uint8_t _channel);  // lecture compteur impulsion du channel

    uint8_t getInput(); // renvoi l'etat des entrees
    uint8_t getOutput(); // renvoi l'etat des sorties
    uint8_t getHBeat();       // renvoi le compteur de vie
    uint8_t AddLossCounter(); // renvoi le compteur d'erreur incrémenté de 1
    uint8_t getLossCounter(); // renvoi le compteur d'erreur


    uint8_t manageHbeat(uint8_t _hbeat_value); // echange des hbeat

    uint8_t readCommand(uint8_t _command);
    uint8_t writeCommand(uint8_t _command, uint8_t _value);

    uint8_t getCardId();
    uint8_t aboutVersion();
    uint8_t compute_lrc(uint8_t _value);

};

extern uint8_t hbeat; // compteur hbeat

void BoardIn8R8_pre();
void BoardIn8R8_post();
void BoardIn8R8_hbeat();


#endif
