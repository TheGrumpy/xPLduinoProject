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
#define NUMBER_OF_R8        [COUNTR8]  		// number of 8 relays card
#define NUMBER_OF_IN16      [COUNTIN16]  	// number of 16 inputs card
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

int NumberOfTemp=NUMBER_OF_TEMP; // number of instances declared


void config_board()
{

    Serial.println(F("------------ setup boards ------------"));

	[CONFIGBOARD]
}

void init_instance()
{

    Serial.println(F("------------  setup SWITCH   ------------"));

	[CONFIGSWITCH]

    Serial.println(F("------------ setup SHUTTER  ------------"));

	[CONFIGSHUTTER]

    Serial.println(F("------------ setup LIGHTING ------------"));

	[CONFIGLIGHTING]

    Serial.println(F("------------ setup TEMPERATURE ------------"));
    
	[CONFIGTEMPERATURE]
}



void pre_update()
{

	[PREUPDATE]
}

void post_update()
{

	[POSTUPDATE]

}

[VARIABLE]

void scenario()
{
	
[SCENARIO]
	
}

[FUNCTION]
