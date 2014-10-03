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
