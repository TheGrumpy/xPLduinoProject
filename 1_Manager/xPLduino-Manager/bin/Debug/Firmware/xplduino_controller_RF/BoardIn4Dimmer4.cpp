#include "BoardIn4Dimmer4.h"
#include "I2C16.h"
#include <DS1307new.h>

//~ version 3: ajout init des variables dans init()
//~ version 4: remplacement lib Wire par lib I2C + ajout notif perte com + slave address 0x16 -> 0x20 (conflit ds2482)

#define I2C_DIMMER_VERSION 4

#define DIMMER_MASK 0x20

#define OFF              0x00 // usage ?

#define R_INPUT          0x50 // read the input status
#define W_HBEAT          0x60 // raz the hbeat counter
#define R_HBEAT          0x61 // read the hbeat counter

#define W_CHANNEL0_DEC   0x02 // command Decrease lighting
#define W_CHANNEL0_INC   0x03 // command Increase lighting
#define W_CHANNEL0_STOP  0x04 // command Stop
#define W_CHANNEL0_MIN   0x05 // set the Min Setpoint
#define W_CHANNEL0_MAX   0x06 // set the Max Setpoint
#define W_CHANNEL0_SP    0x07 // new Setpoint
#define W_CHANNEL0_FD    0x08 // new Fade Rate
#define R_CHANNEL0_SP    0x0A // read the setpoint
#define R_CHANNEL0_FD    0x0B // read the actual fade rate

#define W_CHANNEL1_DEC   0x12 // command Decrease lighting
#define W_CHANNEL1_INC   0x13 // command Increase lighting
#define W_CHANNEL1_STOP  0x14 // command Stop
#define W_CHANNEL1_MIN   0x15 // set the Min Setpoint
#define W_CHANNEL1_MAX   0x16 // set the Max Setpoint
#define W_CHANNEL1_SP    0x17 // new Setpoint
#define W_CHANNEL1_FD    0x18 // new Fade Rate
#define R_CHANNEL1_SP    0x1A // read the setpoint
#define R_CHANNEL1_FD    0x1B // read the actual fade rate

#define W_CHANNEL2_DEC   0x22 // command Decrease lighting
#define W_CHANNEL2_INC   0x23 // command Increase lighting
#define W_CHANNEL2_STOP  0x24 // command Stop
#define W_CHANNEL2_MIN   0x25 // set the Min Setpoint
#define W_CHANNEL2_MAX   0x26 // set the Max Setpoint
#define W_CHANNEL2_SP    0x27 // new Setpoint
#define W_CHANNEL2_FD    0x28 // new Fade Rate
#define R_CHANNEL2_SP    0x2A // read the setpoint
#define R_CHANNEL2_FD    0x2B // read the actual fade rate

#define W_CHANNEL3_DEC   0x32 // command Decrease lighting
#define W_CHANNEL3_INC   0x33 // command Increase lighting
#define W_CHANNEL3_STOP  0x34 // command Stop
#define W_CHANNEL3_MIN   0x35 // set the Min Setpoint
#define W_CHANNEL3_MAX   0x36 // set the Max Setpoint
#define W_CHANNEL3_SP    0x37 // new Setpoint
#define W_CHANNEL3_FD    0x38 // new Fade Rate
#define R_CHANNEL3_SP    0x3A // read the setpoint
#define R_CHANNEL3_FD    0x3B // read the actual fade rate

#define R_VERSION        0x90 // read the version of software
#define R_STATUS         0x91 // read the status of the board

#define ADDR_CHAN0_CHANGED   0
#define ADDR_CHAN1_CHANGED   1
#define ADDR_CHAN2_CHANGED   2
#define ADDR_CHAN3_CHANGED   3
#define ADDR_INPUTS_CHANGED  4
#define ADDR_INIT_OK         5
#define ADDR_HBEAT_OK        6
#define ADDR_COM_OK          7

#define ADDR_HBEAT_NOTIF_OK  1
#define ADDR_HBEAT_NOTIF_KO  2


In4Dimmer4::In4Dimmer4(void)
{
    card_id=0;     // id
    hbeat=0;       // valeur du hbeat
    setpoint[0]=0; // Setpoint channel i
    setpoint[1]=0; // Setpoint channel i
    setpoint[2]=0; // Setpoint channel i
    setpoint[3]=0; // Setpoint channel i
    fade[0]=0;     // Setpoint channel i
    fade[1]=0;     // Setpoint channel i
    fade[2]=0;     // Setpoint channel i
    fade[3]=0;     // Setpoint channel i
    input=0;       // Input status
    status=0;      // status
    status2=0;      // status
}

uint8_t In4Dimmer4::init(uint8_t _card_id){

    card_id=0;     // id
    hbeat=0;       // valeur du hbeat
    setpoint[0]=0; // Setpoint channel i
    setpoint[1]=0; // Setpoint channel i
    setpoint[2]=0; // Setpoint channel i
    setpoint[3]=0; // Setpoint channel i
    fade[0]=0;     // Setpoint channel i
    fade[1]=0;     // Setpoint channel i
    fade[2]=0;     // Setpoint channel i
    fade[3]=0;     // Setpoint channel i
    input=0;       // Input status
    status=0;      // status
    status2=0;     // status
    loss_counter=0;

    card_id=_card_id;

    Serial.print(F("Init IN4DIM4 output with id="));
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

uint8_t In4Dimmer4::getSetpoint(uint8_t _channel){

    return setpoint[_channel];

}

uint8_t In4Dimmer4::inputIsSet(uint8_t _id){

    return bitRead(input,_id);

}

uint8_t In4Dimmer4::newSP(uint8_t _channel, uint8_t _SP, uint8_t _fade){ // demande nouvelle consigne

    setpoint[_channel]=_SP;
    fade[_channel]=_fade;
    bitSet(status, _channel); // on positionne le flag changement

    return 0;

}


uint8_t In4Dimmer4::readInput(){

    input=readCommand(R_INPUT);

    #ifdef DEBUG_I2C_DIMMER
    Serial.print("read input=");
    Serial.println(input, BIN);
    #endif

    return input; // read input

}

uint8_t In4Dimmer4::writeSP(uint8_t _channel){

    writeCommand(((_channel << 4) | 0x08), fade[_channel]); // set fade
    writeCommand(((_channel << 4) | 0x07), setpoint[_channel]); // set SP

    #ifdef DEBUG_I2C_DIMMER
    Serial.print("relu SP=");
    Serial.println(readCommand((_channel << 4) | 0x0A));
    Serial.print("relu Fade=");
    Serial.println(readCommand((_channel << 4) | 0x0B));
    #endif

    bitClear(status, _channel); // on raz le flag changement

    return 0;

}

uint8_t In4Dimmer4::readSP(uint8_t _channel){

    return readCommand(((_channel << 4) | 0x0A));

}

uint8_t In4Dimmer4::readCommand(uint8_t _command){

    digitalWrite(STATUS_LED, HIGH);

    int _cardAddress= (card_id + DIMMER_MASK); // concatenation du type de carte (page d'adresse 0x20-0x2F) et de l'adresse en parametre

    uint8_t dataBuffer[4]={0,0,0,0};
    // 0=value
    // 1=lrc
    // 2=spare
    // 3=spare

    uint8_t com_status=I2c16.read(_cardAddress, _command, 2, dataBuffer);

    #ifdef DEBUG_I2C_DIMMER
        Serial.print(F("Read from address 0x"));
        Serial.print(_cardAddress, HEX);
        Serial.print(F(" command=0x"));
        Serial.print(_command, HEX);
        Serial.print(F(", value="));
        Serial.print(dataBuffer[0]);
        Serial.print(" com_status=");
        Serial.println(com_status, HEX);
    #endif

    if (!com_status)
        SetComIsOK();
    else{
        ClearComIsOK();
    }

    ///~ bitWrite(status, ADDR_COM_OK, dataBuffer[1]==compute_lrc(dataBuffer[0]));

    return dataBuffer[0];

}

uint8_t In4Dimmer4::writeCommand(uint8_t _command, uint8_t _value){

    digitalWrite(STATUS_LED, HIGH);

    uint8_t _cardAddress= (card_id + DIMMER_MASK); // concatenation du type de carte (page d'adresse 0x20-0x2F) et de l'adresse en parametre

    uint8_t com_status=I2c16.write(_cardAddress, _command, _value);

    #ifdef DEBUG_I2C_DIMMER
    Serial.print(F("Write to address 0x"));
    Serial.print(_cardAddress, HEX);
    Serial.print(F(" command=0x"));
    Serial.print(_command, HEX);
    Serial.print(F(", value="));
    Serial.println(_value);
    Serial.print(" com_status=");
    Serial.println(com_status, HEX);
    #endif

    if (!com_status)
        SetComIsOK();
    else{
        ClearComIsOK();
    }



  return 0;

}

uint8_t In4Dimmer4::manageHbeat(uint8_t _hbeat_value){

    writeCommand(W_HBEAT, _hbeat_value);
    uint8_t new_hbeat=readCommand(R_HBEAT);

    if(IsValid()){
        bitWrite(status, ADDR_HBEAT_OK, new_hbeat != hbeat); // comparaison nouveau et ancien hbeat
        hbeat=new_hbeat;
    }

    return 0;

}

uint8_t In4Dimmer4::aboutVersion(){

    return readCommand(R_VERSION);

}

uint8_t In4Dimmer4::aboutZC(){

    return readCommand(R_STATUS);

}


uint8_t In4Dimmer4::ReadCurrent(uint8_t _card_channel){

    return readSP(_card_channel);

}


uint8_t In4Dimmer4::compute_lrc(uint8_t _data){

    uint8_t LRC=(LRC+_data) & 0xFF;

    LRC = ((LRC ^ 0xFF) + 1) & 0xFF;

    return LRC;
}


uint8_t In4Dimmer4::IsValid(){

    return bitRead(status, ADDR_COM_OK);

}

void In4Dimmer4::postNotifyOK(){

    bitSet(status2, ADDR_HBEAT_NOTIF_OK);
    bitClear(status2, ADDR_HBEAT_NOTIF_KO);

}

void In4Dimmer4::postNotifyNOK(){

    bitSet(status2, ADDR_HBEAT_NOTIF_KO);
    bitClear(status2, ADDR_HBEAT_NOTIF_OK);

}

uint8_t In4Dimmer4::IsNotifiedOK(){

    return bitRead(status2, ADDR_HBEAT_NOTIF_OK);

}

uint8_t In4Dimmer4::IsNotifiedNOK(){

    return bitRead(status2, ADDR_HBEAT_NOTIF_KO);

}

void In4Dimmer4::ClearComIsOK(){

    bitClear(status, ADDR_COM_OK);

}

void In4Dimmer4::SetComIsOK(){

    bitSet(status, ADDR_COM_OK);

}

uint8_t In4Dimmer4::IsChannelChanged(uint8_t _channel){

    return bitRead(status, _channel);

}

uint8_t In4Dimmer4::getCardId(){ // renvoi le compteur d'erreur

    return card_id;

}

uint8_t In4Dimmer4::getHBeat(){ // renvoi le compteur d'erreur

    return hbeat;

}

uint8_t In4Dimmer4::AddLossCounter(){ // renvoi le compteur d'erreur incrémenté de 1

    return loss_counter++;

}


uint8_t In4Dimmer4::getLossCounter(){ // renvoi le compteur d'erreur

    return loss_counter;

}

uint8_t In4Dimmer4::getInput(){ // renvoi l'etat des entrees

    return input;

}


extern In4Dimmer4 *IN4DIM4;


void BoardIn4Dimmer4_pre(){

    for(int i=0; i<NumberOfBoardIn4Dimmer4; i++){
        IN4DIM4[i].readInput();


    #ifdef DEBUG_I2C_DIMMER
    if(IN4DIM4[i].IsValid()){
        Serial.print("read input=");
        //~ Serial.print(IN4DIM4[i].input, BIN);
        Serial.print(" from card in4dim4 id=");
        Serial.println(i);
    }else{
        Serial.print("cancel of read input of card in4dim4 id=");
        Serial.println(i);
    }
    #endif


    }

}



void BoardIn4Dimmer4_post(){

    for(int i=0; i<NumberOfBoardIn4Dimmer4; i++){

        if(IN4DIM4[i].IsChannelChanged(0))
            IN4DIM4[i].writeSP(0);

        if(IN4DIM4[i].IsChannelChanged(1))
            IN4DIM4[i].writeSP(1);

        if(IN4DIM4[i].IsChannelChanged(2))
            IN4DIM4[i].writeSP(2);

        if(IN4DIM4[i].IsChannelChanged(3))
            IN4DIM4[i].writeSP(3);

    }

}

void BoardIn4Dimmer4_hbeat(){

    for(int i=0; i<NumberOfBoardIn4Dimmer4; i++){

        IN4DIM4[i].manageHbeat(hbeat);

        if(!IN4DIM4[i].IsValid() &! IN4DIM4[i].IsNotifiedNOK()){

            IN4DIM4[i].AddLossCounter();

            Serial.print(RTC.year);Serial.print("/");Serial.print(RTC.month);Serial.print("/");Serial.print(RTC.day);Serial.print(" - ");Serial.print(RTC.hour);Serial.print(":");Serial.print(RTC.minute);Serial.print(":");Serial.print(RTC.second);
            Serial.print(F(" ! Warning : com KO with In4dim4 id="));
            Serial.print(i);
            Serial.print(", hbeat=");
            Serial.println(IN4DIM4[i].getHBeat());

            IN4DIM4[i].postNotifyNOK();


        }else if(IN4DIM4[i].IsValid() &! IN4DIM4[i].IsNotifiedOK()){

            IN4DIM4[i].postNotifyOK();

            Serial.print(RTC.year);Serial.print("/");Serial.print(RTC.month);Serial.print("/");Serial.print(RTC.day);Serial.print(" - ");Serial.print(RTC.hour);Serial.print(":");Serial.print(RTC.minute);Serial.print(":");Serial.print(RTC.second);
            Serial.print(F(" ! Warning : com OK with In4dim4 id="));
            Serial.print(i);
            Serial.print(", hbeat=");
            Serial.println(IN4DIM4[i].getHBeat());

        };

            #ifdef DEBUG_I2C_DIMMER
            Serial.print("in4dim4 id=");
            Serial.print(i);
            Serial.print(", hbeat=");
            Serial.println(IN4DIM4[i].getHBeat());
            #endif

    }

}
