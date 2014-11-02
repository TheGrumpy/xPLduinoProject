//**************************************************************//
//  Name    : test.pde                                          //
//  Author  : R .TISSOT                                         //
//  Date    : 23/02/2012                                        //
//  Version : 0.0                                               //
//  Notes   : Code for testing library                          //
//          :                                                   //
//****************************************************************

// ajout comptage tps de cycle

#include "config.h"

#include "WebServerSD.h"
#include <tinyFAT.h>

#include <EtherCard.h>
#include <xPL.h>
#include <xPL_Message.h>

#include <Wire.h>
#include "I2C16.h"

#include "pre.h"
#include "post.h"

#include "BoardIn4Dimmer4.h"
#include "BoardR8.h"
#include "BoardIn16.h"
#include "BoardIn8R8.h"

#include "switch_xpl.h"
#include "switch_core.h"

#include "lighting_xpl.h"
#include "lighting_core.h"

#include "shutter_core.h"
#include "shutter_xpl.h"

#include "eth_com.h"
#include "clock.h"
#include "MemoryFree.h"

#include "timer.h"

#include "bitlash.h"
#include <DS1307new.h>
#include "utility.h"

#include <string.h> // utile pour gestion ds2482. vraiement indispensable ?
#include <DS2482.h>
#include "temperature_core.h"
#include "temperature_xpl.h"

#include "TeleInfo_core.h"
#include "TeleInfo_xpl.h"

#include "Arduino.h"
void setup();
void loop();
void check_reset_cause();
void list_debug();
void list_lib_version();
void status();
void setup_bitlash();
void RTC_set();
void RTC_time();
void I2C_scan();
void I2C_read();
void IP_conf();
void temp_list();
void ow_read();
void board_list();
void teleinfo_status();
void switch_list();
void lighting_list();
void shutter_list();
void config_board();
void init_instance();
void pre_update();
void post_update();
void scenario();
void bitlash_custom();

[PERSONNALFUNCTION]

#line 60


byte hbeat=0; // life counter sent to others devices

extern char vendor_id[];    // vendor id
extern char instance_id[];    // instance id

extern const uint8_t int_i2c;

    // Global pointer


    /* */
    Lighting *LIGHTING=NULL;
    Switch *SWITCH=NULL;
    Shutter *SHUTTER=NULL;
    Temperature *TEMPERATURE=NULL;

    /* board pointer */
    In4Dimmer4 *IN4DIM4=NULL;
    Relay8 *R8=NULL;
    Input16 *IN16=NULL;
    In8R8 *IN8R8=NULL;

extern byte pulse_100ms;
extern byte pulse_1000ms;
extern byte pulse_500ms;
extern byte pulse_1mn;

uint8_t mcusr=0;

uint8_t heure =0; // hour since last restart
uint8_t minutes = 0; // minutes sinces last restart

uint16_t main_period_prev_millis=0;
uint8_t  main_period_max=0;
uint8_t  main_period_actual=0;

uint16_t main_prev_millis=0;
uint8_t  main_max=0;
uint8_t  main_actual=0;

uint16_t other_prev_millis=0;
uint8_t  other_max=0;
uint8_t  other_actual=0;

/// en dur pour le moment, dependra de la conf en eeprom
/// cf customer.ino
//~ extern int NumberOfBoard; // number of instances declared
extern int NumberOfLighting; // number of instances declared
extern int NumberOfSwitch; // number of instances declared
extern int NumberOfShutter; // number of instances declared
extern int NumberOfBoardIn4Dimmer4; // number of instances declared
extern int NumberOfBoardR8; // number of instances declared
extern int NumberOfBoardIn16; // number of instances declared
extern int NumberOfOneWire; // number of instances declared
extern int NumberOfBoardIn8R8; // number of instances declared
extern int NumberOfTemp; // number of instances declared

// instance teleinfo
TeleInfo TeleInfo;

void setup() {

    // read the cause of the reset
    mcusr=MCUSR;
    MCUSR=0;


    pinMode(STATUS_LED, OUTPUT);
    pinMode(FAULT_LED, OUTPUT);

    digitalWrite(FAULT_LED, HIGH);
    digitalWrite(STATUS_LED, LOW);

    delay(500);

    initBitlash(115200);     // must be first to initialize serial port

    //start serial
    Serial.begin(115200);

    check_reset_cause();

    Serial.print(F("------------ Setup start - "));
    Serial.print(freeMemory());
    Serial.println(F("Ko  ------------"));

    list_debug();
    list_lib_version();
    /* memory allocation in function of the config stored in eeprom */

    Serial.println(F("---------- memory allocation  -----------"));

    if (NumberOfBoardIn8R8)
        IN8R8 = (In8R8*) malloc((NumberOfBoardIn8R8)*sizeof(In8R8));

        Serial.print(F("Create IN8R8: "));
        Serial.println(NumberOfBoardIn8R8);

    if (NumberOfBoardIn16)
        IN16 = (Input16*) malloc((NumberOfBoardIn16)*sizeof(Input16));

        Serial.print(F("Create IN16: "));
        Serial.println(NumberOfBoardIn16);

    if (NumberOfBoardR8)
        R8 = (Relay8*) malloc((NumberOfBoardR8)*sizeof(Relay8));

        Serial.print(F("Create R8: "));
        Serial.println(NumberOfBoardR8);

    if (NumberOfBoardIn4Dimmer4)
        IN4DIM4 = (In4Dimmer4*) malloc((NumberOfBoardIn4Dimmer4)*sizeof(In4Dimmer4));

        Serial.print(F("Create IN4DIM4 :"));
        Serial.println(NumberOfBoardIn4Dimmer4);

    if (NumberOfSwitch)
        SWITCH = (Switch*) malloc((NumberOfSwitch)*sizeof(Switch));

        Serial.print(F("Create SWITCH:"));
        Serial.println(NumberOfSwitch);

    if (NumberOfShutter)
        SHUTTER = (Shutter*) malloc((NumberOfShutter)*sizeof(Shutter));

        Serial.print(F("Create SHUTTER:"));
        Serial.println(NumberOfShutter);

    // create the instances of Lighting
    if (NumberOfLighting)
        LIGHTING = (Lighting*) malloc((NumberOfLighting)*sizeof(Lighting));

        Serial.print(F("Create LIGHTING:"));
        Serial.println(NumberOfLighting);

    // create the instances of Temperature
    if (NumberOfTemp)
        TEMPERATURE = (Temperature*) malloc((NumberOfTemp)*sizeof(Temperature));

        Serial.print(F("Create Temperature:"));
        Serial.println(NumberOfTemp);


    pinMode(int_i2c,INPUT); // configure the pin of the i2c interrupt signal as input
    digitalWrite(int_i2c, HIGH); // active the internal pull-up

    setup_Udp();  // start Ethernet


    clock_setup(); // start pulse generator

    Serial.println(F("------------ setup i2c  ------------"));
    Wire.begin(); // start i2c
    I2c16.begin() ; // start i2c
    I2c16.timeOut(1000); /// à valider, permet de ne pas rester bloquer par une operation i2c
    Serial.println(F("---------- setup i2c end -----------"));


    setup_bitlash(); // setup bitlash

    if(NumberOfTemp>0){
        Serial.println(F("------------ setup DS2482  ------------"));
        DS2482_Init();  //initialize the DS2482 for OneWire
        Serial.println(F("---------- setup DS2482 end -----------"));
        delay(100);
    }

    /* configuration des cartes */
    /// futurement sur lecture de l'eeprom
    config_board(); // cf custom.ino
    init_instance(); // cf custom.ino

    //~ Serial.println(F("---------- shutter initializ  -----------"));

    /// initialise les volets au démarrage (devrait être optionnel, par config) => desormais on relit l'epprom
    //~ if(NumberOfShutter){
        //~ for(int i=0; i<NumberOfShutter; i++){
            //~ SHUTTER[i].reset();
        //~ }
    //~ }



    digitalWrite(FAULT_LED, LOW);
    digitalWrite(STATUS_LED, HIGH);


    Serial.print(F("------------  Setup end - "));
    Serial.print(freeMemory());
    Serial.println(F("Ko   ------------"));

    main_period_prev_millis=millis();
    main_prev_millis=millis();
    other_prev_millis=millis();

}

void loop() {

    clock_update();

    if(pulse_100ms){

        main_prev_millis=millis();

        main_period_actual=millis()-main_period_prev_millis;
        main_period_prev_millis=millis();

        if(main_period_actual>main_period_max)
            main_period_max=main_period_actual;

        //~ Serial.print("1");
        /* recupere les valeurs des cartes d'entrees si changement détecté*/

        if(NumberOfBoardIn4Dimmer4 && !digitalRead(int_i2c))
            BoardIn4Dimmer4_pre();

        if(NumberOfBoardIn16 && !digitalRead(int_i2c))
            BoardIn16_pre();

        if(NumberOfBoardIn8R8 && !digitalRead(int_i2c))
            BoardIn8R8_pre();
        //~ Serial.print("2");
        /* update all the preliminary condition */
        pre_update();  // cf custom.ino

        scenario(); // cf custom.ino


        /* update the status of the shutters */
        if (NumberOfShutter)
            shutter_post();
        //~ Serial.print("3");
        /* update the post conditions */
        post_update(); // cf custom.ino
        //~ Serial.print("4");
        /* update the output of the boards */
        if(NumberOfBoardIn4Dimmer4)
            BoardIn4Dimmer4_post();

        if(NumberOfBoardR8)
            BoardR8_post();

        if(NumberOfBoardIn8R8)
            BoardIn8R8_post();

        main_actual=millis()-main_prev_millis;

        if(main_actual>main_max)
            main_max=main_actual;


    digitalWrite(STATUS_LED, LOW);
/**/


   }

    other_prev_millis=millis();


    if(pulse_1000ms){

        hbeat++;

        RTC.getTime(); /// update time should be every 500ms or 1s

        if(NumberOfBoardIn4Dimmer4)
            BoardIn4Dimmer4_hbeat();

        if(NumberOfBoardIn8R8)
            BoardIn8R8_hbeat();

        if(NumberOfBoardIn16)
            BoardIn16_hbeat();

        if(NumberOfBoardR8)
            BoardR8_hbeat();


        #ifdef WITH_LIGHTING
        if (NumberOfLighting)
            lighting_hard_status();
        #endif
        //~ Serial.print("1");
        if(NumberOfShutter)
            shutter_hard_status();
        //~ Serial.print("2");
        if(NumberOfTemp){
            //~ Serial.print("3");
         Read_DS2482_Temp(true); //lecture des temperature sur les sodes OneWire en mode seconde
        }
        temperature_sendXPL(); /// temporaire

        TeleInfo.read();

    }

    if(pulse_1mn){

        if(minutes == 59){
            minutes =0;
            heure++;
        }else{
            minutes++;
        }

        TeleInfoSendXPL();  //envois de toutes les trame OneWire en XPL

    }

    loop_Udp();

    runBitlash();

    other_actual=millis()-other_prev_millis;
    //~ other_prev_millis=millis();

    if(other_actual>other_max)
        other_max=other_actual;

}


void check_reset_cause(){

    Serial.print(F("MCUSR="));
    Serial.println(mcusr,BIN);

    if(bitRead(mcusr,0))
        Serial.println(F("Power-on"));

    if(bitRead(mcusr,1))
        Serial.println(F("External"));

    if(bitRead(mcusr,2))
        Serial.println(F("brow-out"));

    if(bitRead(mcusr,3))
        Serial.println(F("Watchdog"));

}


void list_debug(){

    Serial.println(F("------------ debug activated ------------"));

    #ifdef DEBUG_LIGHTING_XPL
    Serial.println(F("DEBUG_LIGHTING_XPL"));
    #endif
    #ifdef DEBUG_LIGHTING_CORE
    Serial.println(F("DEBUG_LIGHTING_CORE"));
    #endif
    #ifdef DEBUG_SHUTTER_XPL
    Serial.println(F("DEBUG_SHUTTER_XPL"));
    #endif
    #ifdef DEBUG_SHUTTER_CORE
    Serial.println(F("DEBUG_SHUTTER_CORE"));
    #endif
    #ifdef DEBUG_SWITCH_XPL
    Serial.println(F("DEBUG_SWITCH_XPL"));
    #endif
    #ifdef DEBUG_SWITCH_CORE
    Serial.println(F("DEBUG_SWITCH_CORE"));
    #endif
    #ifdef DEBUG_ETHERNET
    Serial.println(F("DEBUG_ETHERNET"));
    #endif
    #ifdef DEBUG_INCOMING
    Serial.println(F("DEBUG_INCOMING"));
    #endif
    #ifdef DEBUG_OUTCOMING
    Serial.println(F("DEBUG_OUTCOMING"));
    #endif
    #ifdef DEBUG_I2C_DIMMER
    Serial.println(F("DEBUG_I2C_DIMMER"));
    #endif
    #ifdef DEBUG_BOARD_IN_16
    Serial.println(F("DEBUG_BOARD_IN_16"));
    #endif
    #ifdef DEBUG_BOARD_R8
    Serial.println(F("DEBUG_BOARD_R8"));
    #endif

    #ifdef DEBUG_TEMPERATURE_CORE
    Serial.println(F("DEBUG_TEMPERATURE_CORE"));
    #endif
    #ifdef DEBUG_TEMPERATURE_XPL
        Serial.println(F("DEBUG_TEMPERATURE_XPL"));
    #endif

    #ifdef DEBUG_TELEINFO_SERIAL_CORE
    Serial.println(F("DEBUG_TELEINFO_SERIAL_CORE"));
    #endif
    #ifdef DEBUG_TELEINFO_CORE
        Serial.println(F("DEBUG_TELEINFO_CORE"));
    #endif
    #ifdef DEBUG_TELEINFO_XPL
        Serial.println(F("DEBUG_TELEINFO_XPL"));
    #endif

}

void list_lib_version(){
    Serial.println(F("------------  libs version   ------------"));

    Serial.print(F("Lighting Core Version="));
    Serial.println(aboutLightingVersion());
    Serial.print(F("Lighting Xpl Version="));
    Serial.println(aboutLightingXplVersion());

    Serial.print(F("Switch Core Version="));
    Serial.println(aboutSwitchVersion());
    Serial.print(F("Switch Xpl Version="));
    Serial.println(aboutSwitchXplVersion());

    Serial.print(F("Shutter Core Version="));
    Serial.println(aboutShutterVersion());
    Serial.print(F("Shutter Xpl Version="));
    Serial.println(aboutShutterXplVersion());


    Serial.print(F("Temperature Version="));
    Serial.println(aboutTemperatureVersion());
    Serial.print(F("Temperature XPL Version="));
    Serial.println(aboutTemperatureXplVersion());

    Serial.print(F("Pre Version="));
    Serial.println(aboutPreVersion());

    Serial.print(F("Post Version="));
    Serial.println(aboutPostVersion());

    Serial.print(F("Teleinfo Core Version="));
    Serial.println(aboutTeleinfoVersion());
    Serial.print(F("Teleinfo XPL Version="));
    Serial.println(aboutTeleinfoXplVersion());

}


void status() {

    Serial.print(F("Runtime="));
    Serial.print(heure);
    Serial.print(":");
    Serial.println(minutes);

    check_reset_cause();

    Serial.print(F("freeMemory="));
    Serial.print(freeMemory());
    Serial.println(F("Ko"));

    Serial.print("main cycle actual=");
    Serial.print(main_actual);
    Serial.print(", max=");
    Serial.println(main_max);

    Serial.print("main period actual=");
    Serial.print(main_period_actual);
    Serial.print(", max=");
    Serial.println(main_period_max);

    Serial.print("other cycle actual=");
    Serial.print(other_actual);
    Serial.print(", max=");
    Serial.println(other_max);

}

#line 1 "bitlash.ino"
// Ajout commande ifconfig
// ajout commande list_debug
// ajout commande list_version
// Modification de la fonction temp.list pour utiliser les fonctions public

// *********************************************
// SETUP
// *********************************************
void setup_bitlash()
{

    Serial.println(F("------------ setup bitlash ------------"));

    // system
    addBitlashFunction("status", (bitlash_function) status);
    addBitlashFunction("board.list", (bitlash_function) board_list);
    addBitlashFunction("i2c.scan", (bitlash_function) I2C_scan);
    //~ addBitlashFunction("i2c.read", (bitlash_function) I2C_read);
    addBitlashFunction("ifconfig", (bitlash_function) IP_conf);
    addBitlashFunction("custom", (bitlash_function) bitlash_custom);
    addBitlashFunction("debug.list", (bitlash_function) list_debug);
    addBitlashFunction("version.list", (bitlash_function) list_lib_version);


    // real time clock
    addBitlashFunction("rtc.time", (bitlash_function) RTC_time);
    addBitlashFunction("rtc.set", (bitlash_function) RTC_set);

    // temperature
    addBitlashFunction("temp.list", (bitlash_function) temp_list);
    addBitlashFunction("onewire.list", (bitlash_function) ow_read);

    // teleinfo
    addBitlashFunction("teleinfo", (bitlash_function) teleinfo_status);

    // switch
    addBitlashFunction("switch.list", (bitlash_function) switch_list);

    // lighting
    addBitlashFunction("lighting.list", (bitlash_function) lighting_list);

    // shutter
    addBitlashFunction("shutter.list", (bitlash_function) shutter_list);

    Serial.println(F("----------- setup bitlash end ----------"));

}

// set the system time. Arguments reached are (D,M,Y,H,M)
void RTC_set() {

    Serial.print(F("Previous: "));
    printDateTime();
    Serial.println();

     RTC.fillByYMD((int) getarg(3),(int)  getarg(2),(int)  getarg(1));
     RTC.fillByHMS((int)  getarg(4), (int)  getarg(5),0);
     RTC.setTime();
     RTC.startClock();

     RTC.getTime();

    RTC_time();


}

// read the system time. No argument reached
void RTC_time() {

    Serial.print("Now : ");
    printDateTime();
    Serial.println();

}

// scan the i2c bus. No argument reached
void I2C_scan() {

    I2c16.scan();

}

// send a value to a device on the i2c bus.
void I2C_read() {


    if(getarg(0) && getarg(1) && getarg(2)){

    uint8_t dataBuffer[4]={0,0,0,0};

    Serial.print("Read from address 0x");
    Serial.print(getarg(1));
    Serial.print(", command 0x");
    Serial.println(getarg(2));

    //~ I2c16.read(getarg(1), getarg(2), 2, dataBuffer);



    I2c16.read(getarg(1), getarg(2), 2, dataBuffer);

    Serial.print("0=");
    Serial.println(dataBuffer[0]);
    Serial.print("1=");
    Serial.println(dataBuffer[1]);
    Serial.print("2=");
    Serial.println(dataBuffer[2]);
    Serial.print("3=");
    Serial.println(dataBuffer[3]);

    }

}

// Show the IP configuration. No argument reached
void IP_conf() {

    ether.printIp("IP:  ", ether.myip);
    ether.printIp("GW:  ", ether.gwip);
    ether.printIp("DNS: ", ether.dnsip);

}

// list the temperature sensor. No argument reached.
void temp_list() {

    for( uint8_t i=0; i<NumberOfTemp; i++){
        Serial.print("device : ");Serial.print(TEMPERATURE[i].getName());Serial.print(" @:");
          for (uint8_t j = 0; j < 8; j++){
            if (j>0) {
              Serial.print("/");
            }
          Serial.print(TEMPERATURE[i].getAddr(j), HEX);
        }
        if (TEMPERATURE[i].IsArrondi()){Serial.print(F(" - arrondi a 0,5"));}else{Serial.print(F(" - arrondi a 0,1"));}
        char tmp[60]="\0";
	    dtostrf(TEMPERATURE[i].getValue(), 3, 2, tmp);
        if (TEMPERATURE[i].IsPres()){Serial.print(F(" - Temp : "));Serial.println(tmp);}else{Serial.println(F(" - Non Connectee "));}
    }
}

void ow_read() {
  Read_DS2482_DeviceAdr();
}

// read a temperature sensor. Argument reached is (number of the sensor declared)
void board_list() {


//~ int NumberOfBoardIn4Dimmer4=NUMBER_OF_IN4DIM4; // number of instances declared
//~ int NumberOfBoardR8=NUMBER_OF_R8; // number of instances declared
//~ int NumberOfBoardIn16=NUMBER_OF_IN16; // number of instances declared
//~ int NumberOfBoardIn8R8=NUMBER_OF_IN8R8; // number of instances declared;

    if(NumberOfBoardIn8R8){

        Serial.println(F("-> board in8R8"));
        Serial.println(F("num|id|rev|com|err"));

///utiliser sprintf pour améliorer le formatage ?


        for( uint8_t i=0; i<NumberOfBoardIn8R8; i++){
            Serial.print(i);
            Serial.print(F("   "));
            Serial.print(IN8R8[i].getCardId());
            Serial.print(F("  "));

            uint8_t version=IN8R8[i].aboutVersion();

            if(version != 255){
                Serial.print(version);
            }else{
                Serial.print(F(" ? "));
            }

            if(IN8R8[i].IsValid()){
                Serial.print(F("   OK "));
            }else{
                Serial.print(F(" KO "));
            }

            Serial.print(F("   "));
            Serial.println(IN8R8[i].getLossCounter());

        }

    }

    if(NumberOfBoardIn4Dimmer4){

        Serial.println(F("-> board in4dim4"));
        Serial.println(F("num|id|rev|com|err"));

        for( uint8_t i=0; i<NumberOfBoardIn4Dimmer4; i++){
            Serial.print(i);
            Serial.print(F("   "));
            Serial.print(IN4DIM4[i].getCardId());
            Serial.print(F("  "));

            uint8_t version=IN4DIM4[i].aboutVersion();

            if(version != 255){
                Serial.print(version);
            }else{
                Serial.print(F(" ? "));
            }

            if(IN4DIM4[i].IsValid()){
                Serial.print(F("   OK "));
            }else{
                Serial.print(F(" KO "));
            }

            Serial.print(F("   "));
            Serial.println(IN4DIM4[i].getLossCounter());

        }

    }

    if(NumberOfBoardIn16){

        Serial.println(F("-> board in16"));
        Serial.println(F("num|id0|id1|com|err"));

        for( uint8_t i=0; i<NumberOfBoardIn16; i++){
            Serial.print(i);
            Serial.print(F("   "));
            Serial.print(IN16[i].card_id_0);
            Serial.print(F("    "));
            Serial.print(IN16[i].card_id_1);
            Serial.print(F("  "));

            if(IN16[i].IsValid()){
                Serial.print(F(" OK "));
            }else{
                Serial.print(F(" KO "));
            }

            Serial.print(F("   "));
            Serial.println(IN16[i].loss_counter);

        }
    }

    if(NumberOfBoardR8){

        Serial.println(F("-> board R8"));
        Serial.println(F("num|id|com|err"));

        for( uint8_t i=0; i<NumberOfBoardR8; i++){
            Serial.print(i);
            Serial.print(F("   "));
            Serial.print(R8[i].getCardId());
            Serial.print(F("    "));

            if(R8[i].IsValid()){
                Serial.print(F(" OK "));
            }else{
                Serial.print(F(" KO "));
            }

            Serial.print(F("   "));
            Serial.println(R8[i].getLossCounter());

        }
    }
}

void teleinfo_status(){
    if (TeleInfo.pres==true && TeleInfo.initialiser==true){
     Serial.print(F("Nom du compteur : "));Serial.print(TeleInfo.name);Serial.print(" - reference ADCO : "); Serial.println(TeleInfo.adco);
     Serial.print(F(" Abonnement : "));
     if (TeleInfo.abo==1){Serial.println(F(" Monophase standard"));}
     else if (TeleInfo.abo==2){Serial.println(F(" Monophase Heures pleines/creuses"));}
     else if (TeleInfo.abo==3){Serial.println(F(" Monophase effacement des jours de pointes"));}
     else if (TeleInfo.abo==4){Serial.println(F(" Monophase tempo"));}
     else if (TeleInfo.abo==5){Serial.println(F(" triphase standard"));}
     else if (TeleInfo.abo==6){Serial.println(F(" triphase Heures pleines/creuses"));}
     else if (TeleInfo.abo==7){Serial.println(F(" triphase effacement des jours de pointes"));}
     else if (TeleInfo.abo==8){Serial.println(F(" triphase tempo"));}
     else if (TeleInfo.abo>8){Serial.println(F(" inconnu"));}

     Serial.print(" Option tarifaire "); Serial.println(TeleInfo.optarif);
     Serial.print(" Intensite souscrite "); Serial.println(TeleInfo.isousc);
     Serial.print(" Puissance apparente "); Serial.println(TeleInfo.papp);
      if (TeleInfo.abo<=4) {
        Serial.print(" Intensite instantanee "); Serial.println(TeleInfo.iinst);
        Serial.print(" Intensite maximale appelee "); Serial.println(TeleInfo.imax);
      }else if (TeleInfo.abo>=5) {
        Serial.print(" Intensite instantanee phase 1 "); Serial.println(TeleInfo.IINST1);
        Serial.print(" Intensite instantanee phase 2 "); Serial.println(TeleInfo.IINST2);
        Serial.print(" Intensite instantanee phase 3 "); Serial.println(TeleInfo.IINST3);
        Serial.print(" Intensite maximale phase 1 "); Serial.println(TeleInfo.IMAX1);
        Serial.print(" Intensite maximale phase 2 "); Serial.println(TeleInfo.IMAX2);
        Serial.print(" Intensite maximale phase 3 "); Serial.println(TeleInfo.IMAX3);
        Serial.print(" Presence des potentiels "); Serial.println(TeleInfo.PPOT);
        Serial.print(" Puissance maximale triphasee "); Serial.println(TeleInfo.PMAX);
        Serial.print(" Avertissement de depassement d'intensite phase 1 "); Serial.println(TeleInfo.ADIR1);
        Serial.print(" Avertissement de depassement d'intensite phase 2 "); Serial.println(TeleInfo.ADIR2);
        Serial.print(" Avertissement de depassement d'intensite phase 3 "); Serial.println(TeleInfo.ADIR3);
      }
      if (TeleInfo.abo==1) {
        Serial.print(" Index option base "); Serial.println(TeleInfo.base);
      }else if (TeleInfo.abo==2) {      // Abonnement heures creuses
        Serial.print(" Heures creuses "); Serial.println(TeleInfo.hchc);
        Serial.print(" Heures pleines "); Serial.println(TeleInfo.hchp);
        Serial.print(" Groupe horaire "); Serial.println(TeleInfo.hhphc);
      }else if (TeleInfo.abo==3) { // abonnement Effacement des jours de pointes (EJP)
        Serial.print(" Heures normales "); Serial.println(TeleInfo.EJPHN);
        Serial.print(" Heures de pointe "); Serial.println(TeleInfo.EJPHPM);
        Serial.print(" Preavis debut EJP (30min) "); Serial.println(TeleInfo.PEJP);
      }else if (TeleInfo.abo==4) { // abonnement Tempo (Bleu,blanc,rouge)
        Serial.print(" Heures creuses jours bleus "); Serial.println(TeleInfo.BBRHCJB);
        Serial.print(" Heures pleines jours bleus "); Serial.println(TeleInfo.BBRHPJB);
        Serial.print(" Heures creuses jours blancs "); Serial.println(TeleInfo.BBRHCJW);
        Serial.print(" Heures pleines jours blancs "); Serial.println(TeleInfo.BBRHPJW);
        Serial.print(" Heures creuses jours rouges "); Serial.println(TeleInfo.BBRHCJR);
        Serial.print(" Heures pleines jours rouges "); Serial.println(TeleInfo.BBRHPJR);
        Serial.print(" Couleur du lendemain "); Serial.println(TeleInfo.DEMAIN);
      }
    }else
    {Serial.println(F(" not connected or declared"));}
}

// list the switch
void switch_list() {

    if(NumberOfSwitch){

        for( uint8_t i=0; i<NumberOfSwitch; i++){

            Serial.print(i);
            Serial.print(F(" - "));
            SWITCH[i].printConfig();

        }

    } else
        Serial.println(F("no switch to list"));

}

// list the lighting
void lighting_list() {

    if(NumberOfLighting){

        for( uint8_t i=0; i<NumberOfLighting; i++){

            Serial.print(i);
            Serial.print(F(" - "));
            LIGHTING[i].printConfig();

            Serial.print(F(", current="));
            Serial.print(LIGHTING[i].getCurrent());
            Serial.print("%");

            Serial.print(F(", current fade="));
            Serial.println(LIGHTING[i].getFade());

        }

    } else
        Serial.println(F("no lighting to list"));

}

// list the shutter
void shutter_list() {

    if(NumberOfShutter){

        for( uint8_t i=0; i<NumberOfShutter; i++){

            Serial.print(i);
            Serial.print(F(" - "));
            SHUTTER[i].printConfig();

            Serial.print(F(", current="));
            Serial.print(SHUTTER[i].getPosition());
            Serial.println("%");

        }

    } else
        Serial.println(F("no shutter to list"));

}

#line 1 "[CUSTOMERNAME]"