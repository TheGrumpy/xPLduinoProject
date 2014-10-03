#include "BoardIn16.h"
#include "I2C16.h"
//~ #include "utility.h"

extern void printDateTime();

#define BOARD_IN16_VERSION 3

// version 2: utilisation de la lib I2C au lieu de Wire, ajout gestion des défauts de com
// version 3: utilisation de methode pour manipuler les données:


#define ADDR_COM_OK          0
#define ADDR_OUTPUTS_CHANGED 3
#define ADDR_INPUTS_CHANGED  4
#define ADDR_INIT_OK         5
#define ADDR_HBEAT_OK        6
#define ADDR_HBEAT_NOTIF_OK  1
#define ADDR_HBEAT_NOTIF_KO  2


// Constructor
Input16::Input16(void)
{
    card_id_0=0;     // id
    card_id_1=0;     // id
    input_0=0;      // input status
    input_1=0;      // input status
    status=0;      // status
    loss_counter=0;
}


byte Input16::init(byte _card_id_0, byte _card_id_1){

    card_id_0 =_card_id_0;
    card_id_1 = _card_id_1;

    input_0=0;      // input status
    input_1=0;      // input status
    status=0;      // status
    loss_counter=0;

    Serial.print(F("Init IN16 input with id0 ="));
    Serial.print(card_id_0);
    Serial.print(F(" and id1="));
    Serial.println(card_id_1);


    if(card_id_0>7 || card_id_1>7)
        Serial.println(F("  -> Bad id, must be between 0 to 7 !"));

    read();

    Serial.print(F("  -> Read..."));
    if(IsValid())
        Serial.println(F("OK"));
    else
        Serial.println(F("NOK"));

    return 0;

}

byte Input16::inputIsSet(byte _channel){

    if(_channel<8){
        return bitRead(input_0, _channel);
    } else if(_channel<16){
        return bitRead(input_1, _channel-8);
    }
    return 0;

}

// retreive the state of inputs from the two PCF8574
byte Input16::read(){

    bitSet(status, ADDR_COM_OK); // on met à vrai le flag de com ok, raz si nok plus bas

    int _expander = card_id_0 | B0100000;   // PCF8574 base address + card_id

    uint8_t dataBuffer[4]={0,0,0,0};

    uint8_t com_status_0=I2c16.read(_expander, 1, dataBuffer);

    if (!com_status_0)
        input_0=dataBuffer[0] ^ 0xFF;
    else{
        input_0=0; // defaut dans com -> valeur de repli
        bitClear(status, ADDR_COM_OK);
    }
    #ifdef DEBUG_BOARD_IN16
    Serial.print(" -> @");
    Serial.print(_expander, HEX);
    Serial.print(" input_0: ");
    Serial.print(input_0, BIN);
    Serial.print(" com_status=");
    Serial.println(com_status_0, HEX);
    #endif

    _expander = card_id_1 | B0100000;   // PCF8574 base address + card_id

    uint8_t com_status_1=I2c16.read(_expander, 1, dataBuffer);

    if (!com_status_1)
        input_1=dataBuffer[0] ^ 0xFF;
    else{
        input_1=0; // defaut dans com -> valeur de repli
        bitClear(status, ADDR_COM_OK);
    }

    #ifdef DEBUG_BOARD_IN16
    Serial.print(" -> @");
    Serial.print(_expander, HEX);
    Serial.print(" input_1: ");
    Serial.print(input_1, BIN);
    Serial.print(" com_status=");
    Serial.println(com_status_1, HEX);
    #endif

  return 0;

}

uint8_t Input16::IsValid(){

    return bitRead(status, ADDR_COM_OK);

}

void Input16::postNotifyOK(){

    bitSet(status, ADDR_HBEAT_NOTIF_OK);
    bitClear(status, ADDR_HBEAT_NOTIF_KO);

}

void Input16::postNotifyNOK(){

    bitSet(status, ADDR_HBEAT_NOTIF_KO);
    bitClear(status, ADDR_HBEAT_NOTIF_OK);

}

uint8_t Input16::IsNotifiedOK(){

    return bitRead(status, ADDR_HBEAT_NOTIF_OK);

}

uint8_t Input16::IsNotifiedNOK(){

    return bitRead(status, ADDR_HBEAT_NOTIF_KO);

}


extern Input16 *IN16;

void BoardIn16_pre(){

    for(int i=0; i<NumberOfBoardIn16; i++){

        IN16[i].read();

    }

}

void BoardIn16_hbeat(){

    for(int i=0; i<NumberOfBoardIn16; i++){

        IN16[i].read();

        //~ if( !IN16[i].IsValid() &! bitRead(IN16[i].status, ADDR_HBEAT_NOTIF_KO)){
        if(!IN16[i].IsValid() &! IN16[i].IsNotifiedNOK()){

            IN16[i].loss_counter++;

            printDateTime();
            Serial.print(F(" ! Warning : com KO with IN16 id="));
            Serial.println(i);
            IN16[i].postNotifyNOK();
            //~ bitSet(IN16[i].status, ADDR_HBEAT_NOTIF_KO);
            //~ bitClear(IN16[i].status, ADDR_HBEAT_NOTIF_OK);


        //~ }else if( IN16[i].IsValid() &! bitRead(IN16[i].status, ADDR_HBEAT_NOTIF_OK)){
        }else if(IN16[i].IsValid() &! IN16[i].IsNotifiedOK()){

            IN16[i].postNotifyOK();
            //~ bitSet(IN16[i].status, ADDR_HBEAT_NOTIF_OK);
            //~ bitClear(IN16[i].status, ADDR_HBEAT_NOTIF_KO);

            printDateTime();
            Serial.print(F(" ! Warning : com OK with In16 id="));
            Serial.println(i);

        };

    }

}


