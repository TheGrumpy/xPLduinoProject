//Name : Project_0
//Author : Ludovic
//Creation : 09/02/2014 19:02:30
//Modification 09/02/2014 19:02:30
//Version : 0
//Note : 

#include "water_counter_core.h"
#include "water_counter_xpl.h"

#include "define.h"

#define INT_I2C 6 // 2=SMB V0.0; 6=SMB V0.1

#define NUMBER_OF_IN4DIM4   0  // number of 4 inputs / 4 dimmers card
#define NUMBER_OF_R8        0  		// number of 8 relays card
#define NUMBER_OF_IN16      0  	// number of 16 inputs card
#define NUMBER_OF_IN8R8     1 	// number of 8 inputs / 8 relays card

#define NUMBER_OF_LIGHTING  3 // number of instances declared
#define NUMBER_OF_SWITCH    3 	// number of SWITCH instances
#define NUMBER_OF_SHUTTER   0  // number of instances declared

#define NUMBER_OF_TEMP      0  // number of instances declared

#define NUMBER_OF_WATER_COUNTER 4
#define NUMBER_OF_TIMER         3

byte myip[] = { 0,0,0,0 };
byte gwip[] = { 0,0,0,0 };
byte mymac[] = { 0xD3,0x24,0x76,0x1D,0x6B,0x56 };


#define VENDOR_ID "xplduino"
#define INSTANCE_ID "smb_0"

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

int NumberOfTemp=NUMBER_OF_TEMP; // number of instances declared


void config_board(){

    Serial.println(F("------------ setup boards ------------"));

	IN8R8[0].init(0);
	
}

void init_instance(){

    Serial.println(F("------------  setup SWITCH   ------------"));

	SWITCH[0].init("Bp_Cuisine",HIGH,4);
	#define Bp_Cuisine	0
		#define Impulsion(Bp_Cuisine)	SWITCH[Bp_Cuisine].isPulse()
		#define DoubleImpulsion(Bp_Cuisine)	SWITCH[Bp_Cuisine].isDoublePulse()
		#define Appuie(Bp_Cuisine)	SWITCH[Bp_Cuisine].isOn()
		#define FrontMontant(Bp_Cuisine)	SWITCH[Bp_Cuisine].isOnOSR()
		#define Relachement(Bp_Cuisine)	SWITCH[Bp_Cuisine].isOff()
		#define FrontDescendant(Bp_Cuisine)	SWITCH[Bp_Cuisine].isOnOSF()

	SWITCH[1].init("Bp_Salon",HIGH,4);
	#define Bp_Salon	1
		#define Impulsion(Bp_Salon)	SWITCH[Bp_Salon].isPulse()
		#define DoubleImpulsion(Bp_Salon)	SWITCH[Bp_Salon].isDoublePulse()
		#define Appuie(Bp_Salon)	SWITCH[Bp_Salon].isOn()
		#define FrontMontant(Bp_Salon)	SWITCH[Bp_Salon].isOnOSR()
		#define Relachement(Bp_Salon)	SWITCH[Bp_Salon].isOff()
		#define FrontDescendant(Bp_Salon)	SWITCH[Bp_Salon].isOnOSF()

	SWITCH[2].init("Bp_Garage",HIGH,4);
	#define Bp_Garage	2
		#define Impulsion(Bp_Garage)	SWITCH[Bp_Garage].isPulse()
		#define DoubleImpulsion(Bp_Garage)	SWITCH[Bp_Garage].isDoublePulse()
		#define Appuie(Bp_Garage)	SWITCH[Bp_Garage].isOn()
		#define FrontMontant(Bp_Garage)	SWITCH[Bp_Garage].isOnOSR()
		#define Relachement(Bp_Garage)	SWITCH[Bp_Garage].isOff()
		#define FrontDescendant(Bp_Garage)	SWITCH[Bp_Garage].isOnOSF()

	

    Serial.println(F("------------ setup SHUTTER  ------------"));

	
	

    Serial.println(F("------------ setup LIGHTING ------------"));

	LIGHTING[0].init("Lamp_Cuisine",8, 8);
	#define Lamp_Cuisine	0
		#define Basculer(Lamp_Cuisine)	LIGHTING[Lamp_Cuisine].toggle()
		#define Regler(Lamp_Cuisine)	LIGHTING[Lamp_Cuisine].tune()
		#define Figer(Lamp_Cuisine)	LIGHTING[Lamp_Cuisine].stop()

	LIGHTING[1].init("Lamp_Salon",8, 8);
	#define Lamp_Salon	1
		#define Basculer(Lamp_Salon)	LIGHTING[Lamp_Salon].toggle()
		#define Regler(Lamp_Salon)	LIGHTING[Lamp_Salon].tune()
		#define Figer(Lamp_Salon)	LIGHTING[Lamp_Salon].stop()

	LIGHTING[2].init("Lamp_Garage",8, 8);
	#define Lamp_Garage	2
		#define Basculer(Lamp_Garage)	LIGHTING[Lamp_Garage].toggle()
		#define Regler(Lamp_Garage)	LIGHTING[Lamp_Garage].tune()
		#define Figer(Lamp_Garage)	LIGHTING[Lamp_Garage].stop()

	

    Serial.println(F("------------ setup TEMPERATURE ------------"));
    
	
}



void pre_update(){

	in8R8_to_switch(Bp_Cuisine,0,0);
	in8R8_to_switch(Bp_Salon,0,1);
	in8R8_to_switch(Bp_Garage,0,2);
	in8R8_to_lighting(Lamp_Cuisine,0,0);
	in8R8_to_lighting(Lamp_Salon,0,1);
	in8R8_to_lighting(Lamp_Garage,0,2);
	
}

void post_update(){

	lighting_to_in8R8(Lamp_Cuisine,0,0);
	lighting_to_in8R8(Lamp_Salon,0,1);
	lighting_to_in8R8(Lamp_Garage,0,2);
	

}



void scenario(){
	
	if(Impulsion(Bp_Cuisine))
	{
		Basculer(Lamp_Cuisine);
	}
	
}


// action exécuté sur entrée de la commande "custom" dans le serial monitor
void bitlash_custom(){


}



