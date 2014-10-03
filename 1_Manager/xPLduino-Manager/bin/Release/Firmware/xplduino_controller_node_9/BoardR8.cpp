#include "BoardR8.h"
#include "I2C16.h"
//~ #include "utility.h"

extern void printDateTime();

#define BOARD_R8_VERSION 3

// version 2: utilisation de methode pour manipuler les données
// version 3: raz loss_counter lors de l'init

#define ADDR_COM_OK           0
#define ADDR_OUTPUT_CHANGED   3
#define ADDR_INIT_OK          5
#define ADDR_HBEAT_OK         6
#define ADDR_HBEAT_NOTIF_OK   1
#define ADDR_HBEAT_NOTIF_KO   2

Relay8::Relay8(void)
{
    card_id=0;      // id
    output=0;       // input status
    status=0;       // status
    loss_counter=0; // number of loss communication
}

/* initialize the board. Parameter is Card_id (between 0 to 7). Return if init is ok */

uint8_t Relay8::init(uint8_t _card_id){

    card_id=_card_id;
    output=0;      // input status
    status=0;      // status
    loss_counter=0; // number of loss communication

    Serial.print(F("Init R8 output with id="));
    Serial.println(card_id);
    if(card_id>7)
        Serial.println(F("  -> Bad id, must be between 0 to 7 !"));
    else
        bitSet(status, ADDR_INIT_OK);

    return bitRead(status, ADDR_INIT_OK);;

}

/* write to the card output register and reply if the output register has changed */

uint8_t Relay8::newSP(uint8_t _channel, uint8_t _value){

    uint8_t _output_old = output; // save the actual value

    bitWrite(output, _channel, _value); // save the actual value

    if(output != _output_old)
        bitSet(status, ADDR_OUTPUT_CHANGED); // the output modification is notified (used for card register update)

    #ifdef DEBUG_BOARD_R8
    Serial.print(F("Change R8 output "));
    Serial.print(_channel);
    Serial.print(" outputs from ");
    Serial.print(_output_old, BIN);
    Serial.print(" to ");
    Serial.println(output, BIN);
    #endif

    return bitRead(status, ADDR_OUTPUT_CHANGED);

}

/* write to the card output register and reply the communication status */
uint8_t Relay8::write(){

    if bitRead(status, ADDR_OUTPUT_CHANGED){ // update the register of the PCF only if an output changed

        uint8_t _expander = card_id | B0111000;  // Address with three address pins grounded.

        uint8_t output_inv=output ^ 0xFF;   // inversion of the result

        uint8_t com_status=I2c16.write(_expander, output_inv);    // write the new output register

        if (!com_status){   // check if the transaction is as success.
            bitClear(status, ADDR_OUTPUT_CHANGED);
            bitSet(status, ADDR_COM_OK);
        }else
            bitClear(status, ADDR_COM_OK); /// peut etre devrait on qd meme raz le flag ADDR_OUTPUT_CHANGED

        #ifdef DEBUG_BOARD_R8
        Serial.print(" -> @");
        Serial.print(_expander, HEX);
        Serial.print(" output: ");
        Serial.print(output, BIN);
        Serial.print(" com_status=");
        Serial.println(com_status, HEX);
        #endif

    }

return bitRead(status, ADDR_COM_OK);

}

/* read from the card output register  and reply the communication status */

uint8_t Relay8::read(){

    uint8_t _expander = (card_id | B0111000);   // PCF8574A base address + card_id

    uint8_t dataBuffer[4]={0,0,0,0};

    uint8_t com_status=I2c16.read(_expander, 1, dataBuffer);

    if (!com_status)
        bitSet(status, ADDR_COM_OK);
    else
        bitClear(status, ADDR_COM_OK); /// peut etre devrait on qd meme raz le flag ADDR_OUTPUT_CHANGED


    #ifdef DEBUG_BOARD_R8
    Serial.print(" -> @");
    Serial.print(_expander, HEX);
    Serial.print(" output: ");
    Serial.print(output, BIN);
    Serial.print(" com_status=");
    Serial.println(com_status, HEX);
    #endif

    //~ return dataBuffer[0]; /// devrait renvoyer le status de la transaction
    return bitRead(status, ADDR_COM_OK);

}


/* Return the status of the communication with the board */
uint8_t Relay8::IsValid(){

    return bitRead(status, ADDR_COM_OK);

}

/* Memorise that the "com ok" status has been notified */
void Relay8::postNotifyOK(){

    bitSet(status, ADDR_HBEAT_NOTIF_OK);
    bitClear(status, ADDR_HBEAT_NOTIF_KO);

}

/* Memorise that the "com not ok" status has been notified */
void Relay8::postNotifyNOK(){

    bitSet(status, ADDR_HBEAT_NOTIF_KO);
    bitClear(status, ADDR_HBEAT_NOTIF_OK);

}

/* Return if the "com ok" is already notified */
uint8_t Relay8::IsNotifiedOK(){

    return bitRead(status, ADDR_HBEAT_NOTIF_OK);

}

/* Return if the "com ok" is already notified */
uint8_t Relay8::IsNotifiedNOK(){

    return bitRead(status, ADDR_HBEAT_NOTIF_KO);

}

/* Increase the loss counter and return the new value */
uint8_t Relay8::AddLossCounter(){ // renvoi le compteur d'erreur incrémenté de 1

    return loss_counter++;

}

/* Return the value of loss counter  */
uint8_t Relay8::getLossCounter(){ // renvoi le compteur d'erreur

    return loss_counter;

}


uint8_t Relay8::getCardId(){ // renvoi le compteur d'erreur

    return card_id;

}


extern Relay8 *R8;


/* write the new output to all the board */
void BoardR8_post(){

    for(int i=0; i<NumberOfBoardR8; i++)
        R8[i].write();

}

/* check periodically the good communication with the board (each 1s) */
void BoardR8_hbeat(){

    for(int i=0; i<NumberOfBoardR8; i++){

        R8[i].read(); // make a read of the card to update the communication status

        if(!R8[i].IsValid() &! R8[i].IsNotifiedNOK()){ // communication is KO

            R8[i].AddLossCounter();

            printDateTime();
            Serial.print(F(" ! Warning : com KO with R8 id="));
            Serial.println(i);
            R8[i].postNotifyNOK();

        }else if(R8[i].IsValid() &! R8[i].IsNotifiedOK()){ // communication is OK

            R8[i].postNotifyOK();

            printDateTime();
            Serial.print(F(" ! Warning : com OK with R8 id="));
            Serial.println(i);

        };

    }

}


