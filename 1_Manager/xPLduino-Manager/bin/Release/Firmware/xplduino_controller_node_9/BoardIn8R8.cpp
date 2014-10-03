#include "BoardIn8R8.h"
#include "I2C16.h"
//~ #include "utility.h"

extern void printDateTime();

#define IN8R8_VERSION 7
//version 2: correction concatenation adresse
//version 3: remplacement lib Wire par lib I2C
//version 4: utilisation de methode pour manipuler les données
//version 5: ajout recuperation valeur compteur rapide
//version 6: ajout gestion etat com sur read command
//version 7: ajout force renvoi des commandes output sur recup communication

#define IN8R8_MASK 0x53

#define OFF              0x00 // usage ?

#define R_INPUT          0x50 // read the input status

#define R_OUTPUT         0x41 // write the new output level
#define W_OUTPUT         0x42 // write the new output level

#define W_HBEAT          0x60 // raz the hbeat counter
#define R_HBEAT          0x61 // read the hbeat counter

#define R_VERSION        0x90 // read the version of software
#define R_STATUS         0x91 // read the status of the board

#define R_CHANNEL0_COUNTER    0x0C // read the input counter
#define R_CHANNEL1_COUNTER    0x1C // read the input counter
#define R_CHANNEL2_COUNTER    0x2C // read the input counter
#define R_CHANNEL3_COUNTER    0x3C // read the input counter
#define R_CHANNEL4_COUNTER    0x4C // read the input counter
#define R_CHANNEL5_COUNTER    0x5C // read the input counter
#define R_CHANNEL6_COUNTER    0x6C // read the input counter
#define R_CHANNEL7_COUNTER    0x7C // read the input counter

#define ADDR_OUTPUTS_CHANGED 3
#define ADDR_INPUTS_CHANGED  4
#define ADDR_INIT_OK         5
#define ADDR_COM_OK          0
#define ADDR_HBEAT_OK        6
#define ADDR_HBEAT_NOTIF_OK  1
#define ADDR_HBEAT_NOTIF_KO  2

In8R8::In8R8(void)
{
    card_id=0;     // id
    hbeat=0;       // valeur du hbeat
    input=0;       // Input status
    output=0;      // output status
    status=0;      // status
}

uint8_t In8R8::init(uint8_t _card_id){

    card_id=_card_id;
    //reply_input=_reply_input;

    hbeat=0;       // valeur du hbeat
    input=0;       // Input status
    output=0;      // output status
    status=0;      // status
    loss_counter=0;

    Serial.print(F("Init IN8R8 output with id="));
    Serial.print(card_id);
    Serial.print(F(", software revision is:"));

    uint8_t version=aboutVersion();

    if(bitRead(status, ADDR_COM_OK)){
        Serial.println(aboutVersion());
    }else{
        Serial.println(F(" unknown !"));
    }

    if(card_id>15)
        Serial.println(F("  -> Bad id, must be between 0 to 15 !"));

    manageHbeat(0); // controle de la com lors de l'initialisation

    return 0;

}

uint8_t In8R8::getSetpoint(uint8_t _channel){

    return bitRead(output,_channel);

}

uint8_t In8R8::inputIsSet(uint8_t _id){

    return bitRead(input,_id);

}

uint8_t In8R8::newSP(uint8_t _channel, uint8_t _SP){ // demande nouvelle consigne

    bitWrite(output, _channel, _SP); // on positionne le flag changement
    bitSet(status, ADDR_OUTPUTS_CHANGED); // on positionne le flag changement

    return 0;

}

uint8_t In8R8::readInput(){


    input=readCommand(R_INPUT);

    #ifdef DEBUG_I2C_IN8R8
    if(IsValid()){
        Serial.print("read input=");
        Serial.print(input, BIN);
        Serial.print(" from card in8r8 id=");
        Serial.println(card_id);
    }else{
        Serial.print("read input is not valid in8r8 id=");
        Serial.println(card_id);
    }
    #endif

    return input; // read input

}

uint8_t In8R8::write(){

    writeCommand(W_OUTPUT, output); // update outputs

    #ifdef DEBUG_I2C_IN8R8
    Serial.print("Output, sent=");
    Serial.print(output, BIN);
    Serial.print(", read=");
    Serial.println(readCommand(R_OUTPUT), BIN);
    #endif

    bitClear(status, ADDR_OUTPUTS_CHANGED); // on raz le flag changement

    return 0;

}

uint8_t In8R8::readCounter(uint8_t _channel){  // lecture compteur impulsion du channel

    return readCommand(((_channel << 4) | 0x0C));

}

uint8_t In8R8::readCommand(uint8_t _command){


    digitalWrite(STATUS_LED, HIGH);

    int _cardAddress= (card_id + IN8R8_MASK); // concatenation du type de carte (page d'adresse 0x20-0x2F) et de l'adresse en parametre

    uint8_t dataBuffer[2]={0,0};
    // 0=value
    // 1=lrc
    // 2=spare
    // 3=spare
    uint8_t com_status=I2c16.read(_cardAddress, _command, 2, dataBuffer);

    #ifdef DEBUG_I2C_IN
        Serial.print(F("Read from address 0x"));
        Serial.print(_cardAddress, HEX);
        Serial.print(F(" command=0x"));
        Serial.print(_command, HEX);
        Serial.print(F(", value="));
        Serial.print(dataBuffer[0]);
        Serial.print(" com_status=");
        Serial.println(com_status, HEX);
    #endif

    //~ bitWrite(status, ADDR_COM_OK, dataBuffer[1]==compute_lrc(dataBuffer[0]));

    if (!com_status && dataBuffer[1]==compute_lrc(dataBuffer[0]))
        SetComIsOK();
    else{
        ClearComIsOK();
    }

    return dataBuffer[0];

}

uint8_t In8R8::writeCommand(uint8_t _command, uint8_t _value){

    digitalWrite(STATUS_LED, HIGH);

    uint8_t _cardAddress= (card_id + IN8R8_MASK); // concatenation du type de carte (plage d'adresse 0x50-0x5F) et de l'adresse en parametre

    #ifdef DEBUG_I2C_IN8R8
    Serial.print(F("Write to address 0x"));
    Serial.print(_cardAddress, HEX);
    Serial.print(F(" command=0x"));
    Serial.print(_command, HEX);
    Serial.print(F(", value="));
    Serial.println(_value);
    #endif

    I2c16.write(_cardAddress, _command, _value);

  return 0;

}

uint8_t In8R8::manageHbeat(uint8_t _hbeat_value){

    writeCommand(W_HBEAT, _hbeat_value);

    uint8_t new_hbeat=readCommand(R_HBEAT);

    //~ bitWrite(status, ADDR_HBEAT_OK, new_hbeat != hbeat); // comparaison nouveau et ancien hbeat
    if(new_hbeat != hbeat){
        bitSet(status, ADDR_HBEAT_OK); // comparaison nouveau et ancien hbeat
    }else{
        bitClear(status, ADDR_HBEAT_OK); // comparaison nouveau et ancien hbeat
        input=reply_input;
    }

    //~ if(bitRead(status, ADDR_HBEAT_OK)){
        //~ Serial.print("hbeat=");
        //~ Serial.println(new_hbeat, HEX);
    //~ }else{
        //~ Serial.println("pas de com");
    //~ }


    hbeat=new_hbeat;

    return bitRead(status, ADDR_HBEAT_OK);

}


void In8R8::ClearComIsOK(){

    bitClear(status, ADDR_COM_OK);

}

void In8R8::SetComIsOK(){

    bitSet(status, ADDR_COM_OK);

}



uint8_t In8R8::getCardId(){ // renvoi le compteur d'erreur

    return card_id;

}

uint8_t In8R8::aboutVersion(){

    return readCommand(R_VERSION);

}


uint8_t In8R8::compute_lrc(uint8_t _data){

    uint8_t LRC=(LRC+_data) & 0xFF;

    LRC = ((LRC ^ 0xFF) + 1) & 0xFF;

    return LRC;
}



uint8_t In8R8::IsValid(){

    return bitRead(status, ADDR_COM_OK);

}

void In8R8::postNotifyOK(){

    bitSet(status, ADDR_HBEAT_NOTIF_OK);
    bitClear(status, ADDR_HBEAT_NOTIF_KO);

}

void In8R8::postNotifyNOK(){

    bitSet(status, ADDR_HBEAT_NOTIF_KO);
    bitClear(status, ADDR_HBEAT_NOTIF_OK);

}

uint8_t In8R8::IsNotifiedOK(){

    return bitRead(status, ADDR_HBEAT_NOTIF_OK);

}

uint8_t In8R8::IsNotifiedNOK(){

    return bitRead(status, ADDR_HBEAT_NOTIF_KO);

}

uint8_t In8R8::IsOutputsChanged(){

    return bitRead(status, ADDR_OUTPUTS_CHANGED);

}


uint8_t In8R8::getHBeat(){ // renvoi le compteur d'erreur

    return hbeat;

}


uint8_t In8R8::AddLossCounter(){ // renvoi le compteur d'erreur incrémenté de 1

    return loss_counter++;

}


uint8_t In8R8::getLossCounter(){ // renvoi le compteur d'erreur

    return loss_counter;

}

uint8_t In8R8::getInput(){ // renvoi l'etat des entrees

    return input;

}

uint8_t In8R8::getOutput(){ // renvoi l'etat des sorties

    return output;

}

extern In8R8 *IN8R8;


void BoardIn8R8_pre(){

    for(int i=0; i<NumberOfBoardIn8R8; i++){

       IN8R8[i].readInput();

    }

}



void BoardIn8R8_post(){

    for(int i=0; i<NumberOfBoardIn8R8; i++){

        if(IN8R8[i].IsOutputsChanged()){
            #ifdef DEBUG_I2C_IN8R8
            Serial.print(F("post in8R8="));
            Serial.println(i);
            #endif
            IN8R8[i].write();
        }
    }

}

void BoardIn8R8_hbeat(){

    for(int i=0; i<NumberOfBoardIn8R8; i++){

        IN8R8[i].manageHbeat(hbeat);

        if(!IN8R8[i].IsValid() &! IN8R8[i].IsNotifiedNOK()){

            IN8R8[i].AddLossCounter();

            printDateTime();
            Serial.print(F(" ! Warning : com KO with In8R8 id="));
            Serial.print(i);
            Serial.print(", hbeat=");
            Serial.println(IN8R8[i].getHBeat());

            IN8R8[i].postNotifyNOK();


        }else if(IN8R8[i].IsValid() &! IN8R8[i].IsNotifiedOK()){

            IN8R8[i].postNotifyOK();
            IN8R8[i].write(); // force renvoi des commandes output

            printDateTime();
            Serial.print(F(" ! Warning : com OK with In8R8 id="));
            Serial.print(i);
            Serial.print(", hbeat=");
            Serial.println(IN8R8[i].getHBeat());

        };

        #ifdef DEBUG_I2C_IN8R8
            Serial.print("in8R8 id=");
            Serial.print(i);
            Serial.print(", hbeat=");
            Serial.println(IN8R8[i].getHBeat());
        #endif

    }

}




