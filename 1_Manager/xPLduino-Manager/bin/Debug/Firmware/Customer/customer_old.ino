//Name : [PROJECTNAME]
//Author : [PROJECTAUTHOR]
//Creation : [PROJECTDATETIMECREATION]
//Modification [PROJECTDATETIMEMODIFICATION]
//Version : [PROJECTVERSION]
//Note : [PROJECTNOTE]

#include "water_counter_core.h"
#include "water_counter_xpl.h"

#include "define.h"

#define INT_I2C [NODETYPE] // 2=SMB V0.0; 6=SMB V0.1

#define NUMBER_OF_IN4DIM4   [COUNTIN4DIM4]  // number of 4 inputs / 4 dimmers card
#define NUMBER_OF_R8        [COUNT8R]  		// number of 8 relays card
#define NUMBER_OF_IN16      [COUNT16IN]  	// number of 16 inputs card
#define NUMBER_OF_IN8R8     [COUNTIN8R8] 	// number of 8 inputs / 8 relays card

#define NUMBER_OF_LIGHTING  [COUNTLIGHTING] // number of instances declared
#define NUMBER_OF_SWITCH    [COUNTSWITCH] 	// number of SWITCH instances
#define NUMBER_OF_SHUTTER   [COUNTSHUTTER]  // number of instances declared

#define NUMBER_OF_TEMP      [COUNTTEMP]  // number of instances declared

#define NUMBER_OF_WATER_COUNTER 4
#define NUMBER_OF_TIMER         3

byte myip[] = { [NODEIP] };
byte gwip[] = { [NODEGWIP] };
byte mymac[] = { [NODEMAC] };


#define VENDOR_ID "xplduino"
#define INSTANCE_ID "[NODENAME]"

const uint8_t int_i2c=INT_I2C; // definition de l'adresse de la pin interruption I2C

char vendor_id[8+2]={VENDOR_ID};    // vendor id
char instance_id[18+1]={INSTANCE_ID};    // instance id
int NumberOfLighting=NUMBER_OF_LIGHTING; // number of instances declared
int NumberOfSwitch=NUMBER_OF_SWITCH; // number of instances declared
int NumberOfShutter=NUMBER_OF_SHUTTER; // number of instances declared

int NumberOfBoardIn4Dimmer4=NUMBER_OF_IN4DIM4; // number of instances declared
int NumberOfBoardR8=NUMBER_OF_R8; // number of instances declared
int NumberOfBoardIn16=NUMBER_OF_IN16; // number of instances declared
int NumberOfBoardIn8R8=NUMBER_OF_IN8R8; // number of instances declared;

int NumberOfTimer=NUMBER_OF_TIMER; // number of instances declared;
int NumberOfTemp=NUMBER_OF_TEMP; // number of instances declared
int NumberOfWaterCounter=NUMBER_OF_WATER_COUNTER; // number of instances declared

Timer TIMER[NUMBER_OF_TIMER];

water_counter WATER_COUNTER[NUMBER_OF_WATER_COUNTER];

void config_board(){

    Serial.println(F("------------ setup boards ------------"));

    IN4DIM4[0].init(8); // carte in4dim4 avec id i2c=1
    //~ IN16[0].init(0,1);
    //~ R8[0].init(0);
    IN8R8[0].init(0,0); // carte in8r8 avec id i2c=0

}

void init_instance(){


    Serial.println(F("------------  setup SWITCH   ------------"));

    #define timer_couloir TIMER[0]
    #define timer_1 TIMER[1]
    #define timer_2 TIMER[2]

    // uint8_t init(uint8_t _base, uint8_t _mode, uint16_t _setpoint);
    // base de temps: MILLIS, SECOND, MINUTE
    TIMER[0].init(MINUTE,TON, 5);
    TIMER[1].init(SECOND,TOF, 5);
    TIMER[2].init(MILLIS,TP, 2000);

    SWITCH[0].init("bp_salon_volet_1", HIGH, 4);
    #define bp_salon_volet_1 0
    SWITCH[1].init("bp_salon_volet_2", HIGH, 4);
    #define bp_salon_volet_2 1
    SWITCH[2].init("bp_salon_volet_3", HIGH, 4);
    #define bp_salon_volet_3 2
    SWITCH[3].init("bp_ch3_volet", HIGH, 4);
    #define bp_ch3_volet 3
    SWITCH[4].init("eau_chaude", HIGH, 2);
    #define eau_chaude 4
    SWITCH[5].init("eau_froide", HIGH, 2);
    #define eau_froide 5
    SWITCH[6].init("porte", HIGH, 10);
    #define porte 6
    SWITCH[7].init("in_0_7", HIGH, 2);
    #define in_0_7 7


    SWITCH[8].init("bp_couloir", HIGH, 5);
    #define bp_couloir 8
    SWITCH[9].init("in_2_1", HIGH, 5);
    #define in_2_1 9
    SWITCH[10].init("in_2_2", HIGH, 5);
    #define in_2_2 10
    SWITCH[11].init("in_2_3", HIGH, 5);
    #define in_2_3 11

    Serial.println(F("------------ setup SHUTTER  ------------"));

    #define POSITION_SALON_1  300
    #define POSITION_SALON_2  302
    #define POSITION_SALON_3  304
    #define POSITION_CHAMBRE3 306

    SHUTTER[0].init("salon_1", TYPE_PERMANENT, 300); //type permanent up/down
    #define salon_1 0
    SHUTTER[0].postCurrent(read16(POSITION_SALON_1));
    Serial.println(read16(POSITION_SALON_1));

    SHUTTER[1].init("salon_2", TYPE_PERMANENT, 300); //type permanent up/down
    #define salon_2 1
    SHUTTER[1].postCurrent(read16(POSITION_SALON_2));
    Serial.println(read16(POSITION_SALON_2));

    SHUTTER[2].init("salon_3", TYPE_PERMANENT, 170); //type permanent up/down
    #define salon_3 2
    SHUTTER[2].postCurrent(read16(POSITION_SALON_3));
    Serial.println(read16(POSITION_SALON_3));

    SHUTTER[3].init("chambre3", TYPE_PERMANENT, 200); //type permanent up/down
    #define chambre3 3
    SHUTTER[3].postCurrent(read16(POSITION_CHAMBRE3));
    Serial.println(read16(POSITION_CHAMBRE3));

    Serial.println(F("------------ setup LIGHTING ------------"));

    //~ LIGHTING[0].init("led_0", 0, 0);
    //~ #define led_0 0
    //~ LIGHTING[1].init("led_1", 0, 0);
    //~ #define led_1 1
    //~ LIGHTING[2].init("led_2", 0, 0);
    //~ #define led_2 2
    //~ LIGHTING[3].init("led_3", 0, 0);
    //~ #define led_3 3
    //~ LIGHTING[4].init("led_4", 0, 0);
    //~ #define led_4 4
    //~ LIGHTING[5].init("led_5", 0, 0);
    //~ #define led_5 5
    //~ LIGHTING[6].init("led_6", 0, 0);
    //~ #define led_6 6
    //~ LIGHTING[7].init("led_7", 0, 0);
    //~ #define led_7 7

    LIGHTING[0].init("couloir", 50, 1);
    #define couloir 0
    LIGHTING[1].init("dim_1", 0, 6);
    #define dim_1 1
    LIGHTING[2].init("dim_2", 0, 8);
    #define dim_2 2
    LIGHTING[3].init("dim_3", 0, 0);
    #define dim_3 3


    Serial.println(F("------------ setup TEMPERATURE ------------"));
    // Declaration des sondes TEMPERTATURE[X].init("name",adr0,adr1,adr2,adr3,adr4,adr5,adr6,adr7,arrondi_0,5,periode,time_sec)        // periode pour le temps entre 2 traitements (0 pour le traitement sur changement de valeur)   // time_sec pour le choix de la base de temps sec ou minutes
    //~ TEMPERATURE[0].init("temp_salon",0x10,0xFC,0x84,0x94,0x01,0x08,0x00,0x12,true);
    TEMPERATURE[0].init("temp_salon_old",0x28,0x4E,0xA0,0x08,0x05,0x00,0x00,0x62,true);
    TEMPERATURE[1].init("temp_salon",0x28,0x7E,0x6B,0x08,0x05,0x00,0x00,0xDD,true);
//28/4E/A0/8/5/0/0/62
//28/6B/A1/8/5/0/0/F2
//28/C1/46/9/5/0/0/65
//28/21/B8/9/5/0/0/48
//28/7E/6B/8/5/0/0/DD

    #define CPT_EAU_CHAUDE 200
    #define CPT_EAU_FROIDE 202

    WATER_COUNTER[0].init("cpt_eau_chaude_I", 1, 0);
    #define cpt_eau_chaude 0
    WATER_COUNTER[1].init("cpt_eau_chaude_T", 1, read16(CPT_EAU_CHAUDE));
    #define cpt_eau_chaude_tot 1
    WATER_COUNTER[2].init("cpt_eau_froide_I", 1, 0);
    #define cpt_eau_froide 2
    WATER_COUNTER[3].init("cpt_eau_froide_T", 1, read16(CPT_EAU_FROIDE));
    #define cpt_eau_froide_tot 3

    //~ Serial.println(write16(202, 0xABCD), HEX);
    //~ delay(100);

    //~ Serial.print("lu:");
    //~ Serial.println(read16(202), HEX);

}



void pre_update(){


    // <type de carte>_to_switch (<n° instance switch>, <n° instance de board>, <n° entrée carte>)

    // mis à jour des switch en fonction de l'état de l'entrée de la carte d'entrée
    in8R8_to_switch(bp_ch3_volet, 0, 0);
    in8R8_to_switch(bp_salon_volet_3, 0, 1);
    in8R8_to_switch(bp_salon_volet_1, 0, 2);
    in8R8_to_switch(bp_salon_volet_2, 0, 3);
    in8R8_to_switch(eau_chaude, 0, 4);
    in8R8_to_switch(eau_froide, 0, 5);
    in8R8_to_switch(porte, 0, 6);
    in8R8_to_switch(in_0_7, 0, 7);

    dimmer_to_switch(bp_couloir, 0, 0);
    dimmer_to_switch(in_2_1, 0, 1);
    dimmer_to_switch(in_2_2, 0, 2);
    dimmer_to_switch(in_2_3, 0, 3);

    TIMER[0].update();
    TIMER[1].update();
    TIMER[2].update();

    // relecture de l'état de la lampe
    //~ R8_to_lighting(led_0, 0, 0);
    //~ R8_to_lighting(led_1, 0, 1);
    //~ R8_to_lighting(led_2, 0, 2);
    //~ R8_to_lighting(led_3, 0, 3);
    //~ R8_to_lighting(led_4, 0, 4);
    //~ R8_to_lighting(led_5, 0, 5);
    //~ R8_to_lighting(led_6, 0, 6);
    //~ R8_to_lighting(led_7, 0, 7);

    dimmer_to_lighting(couloir, 0, 0);
    dimmer_to_lighting(dim_1, 0, 1);
    dimmer_to_lighting(dim_2, 0, 2);
    dimmer_to_lighting(dim_3, 0, 3);


}

void post_update(){

    shutter_to_in8R8(chambre3, 0, 1, 0);
    shutter_to_in8R8(salon_3, 0, 3, 2);
    shutter_to_in8R8(salon_2, 0, 5, 4);
    shutter_to_in8R8(salon_1, 0, 7, 6);

    lighting_to_dimmer(couloir, 0, 0);
    lighting_to_dimmer(dim_1, 0, 1);
    lighting_to_dimmer(dim_2, 0, 2);
    lighting_to_dimmer(dim_3, 0, 3);

}

byte oneshot_0=0; // variable pour mémoriser le 'un coup'
byte oneshot_1=0; // variable pour mémoriser le 'un coup'
byte oneshot_2=0; // variable pour mémoriser le 'un coup'
byte oneshot_3=0; // variable pour mémoriser le 'un coup'

uint16_t prev_eau_froide=0;     // valeur compteur precedente
uint16_t prev_eau_chaude=0;     // valeur compteur precedente
uint8_t  cpt_stop=0;            // valeur courant compteur de minutes stop
uint8_t  mem_envoi_zero=0;      // memoire trig 0 envoyé

void scenario(){


    if(get_time()==800){ //on compare l'heure
        // c'est l'heure
        if(! oneshot_0){ // permet d'envoyer une seule fois la commande. Sinon on serait en permanence en train d'envoyer la commmande tant que c'est l'heure
            printDateTime();
            Serial.println("bonjour !");
            open(salon_1);
            open(salon_2);
            open(salon_3);
            oneshot_0=true;
        }

    }else{
        oneshot_0=false; // ce n'est plus l'heure, on reset le flag
    }


    if(get_time()==1730){ //on compare l'heure
        // c'est l'heure
        if(! oneshot_1){ // permet d'envoyer une seule fois la commande. Sinon on serait en permanence en train d'envoyer la commmande tant que c'est l'heure
            printDateTime();
            Serial.println("bonsoir !");
            close(salon_1);
            close(salon_2);
            close(salon_3);
            close(chambre3);
            oneshot_1=true;
        }

    }else{
        oneshot_1=false; // ce n'est plus l'heure, on reset le flag
    }

    if(pulse(bp_couloir))
        toggle_light(couloir);
    if(on_ons(bp_couloir))
        tune(couloir);
    if(off_ons(bp_couloir))
        stop_tune(couloir);

    if(timer_couloir.q_ons(LIGHTING[couloir].IsOn())){ // evenement au bout de 5 min d'eclairage
        LIGHTING[couloir].new_setpoint(0); // on eteint
    }

    if(pulse(in_2_1))
        toggle_light(dim_1);
    if(on_ons(in_2_1))
        tune(dim_1);
    if(off_ons(in_2_1))
        stop_tune(dim_1);

    if(pulse(in_2_2))
        toggle_light(dim_2);
    if(on_ons(in_2_2))
        tune(dim_2);
    if(off_ons(in_2_2))
        stop_tune(dim_2);

    if(pulse(in_2_3))
        toggle_light(dim_3);
    if(on_ons(in_2_3))
        tune(dim_3);
    if(off_ons(in_2_3))
        stop_tune(dim_3);




    if(on_ons(bp_salon_volet_1)){
        toggle_shutter(salon_1);
        shutter_sendToggle("home", "salon_1");
    }

    if(on_ons(bp_salon_volet_2)){
        toggle_shutter(salon_2);
        shutter_sendToggle("home", "salon_2");
    }

    if(on_ons(bp_salon_volet_3)){
        toggle_shutter(salon_3);
        shutter_sendToggle("home", "salon_3");
    }

    if(on_ons(bp_ch3_volet))
        toggle_shutter(chambre3);

    if(on_ons(eau_chaude))
    {
        WATER_COUNTER[cpt_eau_chaude].ctu();     // incrementation compteur conso instantanée
        WATER_COUNTER[cpt_eau_chaude_tot].ctu(); // incrementation compteur conso totale
        water_counter_status(&WATER_COUNTER[cpt_eau_chaude_tot], T_TRIG); // envoi message xPL conso totale vers Domogik
        write16(CPT_EAU_CHAUDE, WATER_COUNTER[cpt_eau_chaude_tot].getValue()); // sauvegarde en eeprom
    }

    if(on_ons(eau_froide))
    {
        WATER_COUNTER[cpt_eau_froide].ctu();
        WATER_COUNTER[cpt_eau_froide_tot].ctu();
        water_counter_status(&WATER_COUNTER[cpt_eau_froide_tot], T_TRIG);
        write16(CPT_EAU_FROIDE, WATER_COUNTER[cpt_eau_froide_tot].getValue());
    }

    byte changement =
    (WATER_COUNTER[cpt_eau_chaude].getValue() > prev_eau_chaude)        // pas de changement
    || (WATER_COUNTER[cpt_eau_froide].getValue() > prev_eau_froide);    // pas de changement


    if(pulse_1000ms){

        if(!SHUTTER[0].IsPositionSaved() && SHUTTER[0].IsSetpointReached()){
            write16(POSITION_SALON_1, SHUTTER[0].getCurrent());
            SHUTTER[0].postPositionSaved();
            Serial.print("save POSITION_SALON_1=");
            Serial.println(read16(POSITION_SALON_1));
            //~ delay(1);
        }

        if(!SHUTTER[1].IsPositionSaved() && SHUTTER[1].IsSetpointReached()){
            write16(POSITION_SALON_2, SHUTTER[1].getCurrent());
            SHUTTER[1].postPositionSaved();
            Serial.print("save POSITION_SALON_2=");
            Serial.println(read16(POSITION_SALON_2));
            //~ delay(1);
        }

        if(!SHUTTER[2].IsPositionSaved() && SHUTTER[2].IsSetpointReached()){
            write16(POSITION_SALON_3, SHUTTER[2].getCurrent());
            SHUTTER[2].postPositionSaved();
            Serial.print("save POSITION_SALON_3=");
            Serial.println(read16(POSITION_SALON_3));
            //~ delay(1);
        }

        if(!SHUTTER[3].IsPositionSaved() && SHUTTER[3].IsSetpointReached()){
            write16(POSITION_CHAMBRE3, SHUTTER[3].getCurrent());
            SHUTTER[3].postPositionSaved();
            Serial.print("save POSITION_CHAMBRE3=");
            Serial.println(read16(POSITION_CHAMBRE3));
            //~ delay(1);
        }

    }

    if(pulse_1mn){

    if( ! prev_eau_chaude &! prev_eau_froide &! mem_envoi_zero){
        mem_envoi_zero=true;
        water_counter_status(&WATER_COUNTER[cpt_eau_chaude], T_TRIG);
        water_counter_status(&WATER_COUNTER[cpt_eau_froide], T_TRIG);
    }


        if(changement){
            Serial.println("changement");
            prev_eau_chaude=WATER_COUNTER[cpt_eau_chaude].getValue();
            prev_eau_froide=WATER_COUNTER[cpt_eau_froide].getValue();
            cpt_stop=0;
        }else if(prev_eau_chaude || prev_eau_froide){
            Serial.println("conso mais plus de chgt");
            if(cpt_stop < 1){
                cpt_stop++;
                Serial.println("plus qu'une minute");
            }else{
                Serial.println("envoi xPL");
                water_counter_status(&WATER_COUNTER[cpt_eau_chaude], T_TRIG);
                water_counter_status(&WATER_COUNTER[cpt_eau_froide], T_TRIG);
                WATER_COUNTER[cpt_eau_chaude].reset();
                WATER_COUNTER[cpt_eau_froide].reset();
                prev_eau_chaude=0;
                prev_eau_froide=0;
                cpt_stop=0;
                mem_envoi_zero=0;
            }

        }
    }


    if(on_ons(porte)){
        printDateTime();
        Serial.println(" - ouverture porte");
    }

    if(off_ons(porte)){
        printDateTime();
        Serial.println(" - fermeture porte");
    }

    //~ if(pulse_1000ms)
        //~ WATER_COUNTER[cpt_eau_froide2].ctu(IN8R8[0].readCounter(5));

}


// action exécuté sur entrée de la commande "custom" dans le serial monitor
void bitlash_custom(){

    Serial.println("custom compteur:");
    Serial.print("Eau froide inst=");
    Serial.print(WATER_COUNTER[cpt_eau_froide].getValue());
    Serial.print(", total=");
    Serial.print(WATER_COUNTER[cpt_eau_froide_tot].getValue());
    Serial.print(", rom=");
    Serial.println(read16(CPT_EAU_FROIDE));

    Serial.print("Eau chaude inst=");
    Serial.print(WATER_COUNTER[cpt_eau_chaude].getValue());
    Serial.print(", total=");
    Serial.print(WATER_COUNTER[cpt_eau_chaude_tot].getValue());
    Serial.print(", rom=");
    Serial.println(read16(CPT_EAU_CHAUDE));


    toggle_shutter(salon_1);
    //~ SHUTTER[0].new_setpoint(50);
    //~ SHUTTER[1].new_setpoint(60);
    //~ SHUTTER[2].new_setpoint(70);
    //~ SHUTTER[3].new_setpoint(80);

}



/// à déplacer
// renvoi hh:mm au format decimal hhmm
uint16_t get_time(){

    return RTC.hour*100+RTC.minute;

}

uint16_t read16(uint16_t _address){

    uint8_t dataBuffer[2]={0,0};

    uint8_t com_status=I2c16.read16(0x50, _address, 2, dataBuffer);

    //~ Serial.print("dataBuffer[1]=0x");
    //~ Serial.println(dataBuffer[1], HEX);
    //~ Serial.print("dataBuffer[0]=0x");
    //~ Serial.println(dataBuffer[0], HEX);

    //~ Serial.print("Rvalue=0x");
    uint16_t value=dataBuffer[1];
    value=value << 8;
    value=value + dataBuffer[0];
    //~ Serial.println(value, HEX);

    return ((dataBuffer[1] << 8) + dataBuffer[0]);

}

//~ perf SMB temp moyen max etc


uint8_t write16(uint16_t _address, uint16_t _value){

    uint8_t dataBuffer[2]={0,0};

    //~ Serial.print("Svalue=");
    //~ Serial.println(_value, HEX);
    dataBuffer[1]= _value >> 8;
    //~ Serial.print("dataBuffer[1]=");
    //~ Serial.println(dataBuffer[1], HEX);
    dataBuffer[0]= _value & 0x00FF;
    //~ Serial.print("dataBuffer[0]=");
    //~ Serial.println(dataBuffer[0], HEX);

    return I2c16.write16(0x50, _address, dataBuffer, 2);

}
