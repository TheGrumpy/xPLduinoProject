#ifndef RFDevice_h
#define RFDevice_h

#include "config.h"
#include "Arduino.h"
#include "RCSwitch_mod.h"

extern int NumberOfRFDevice; // number of instances declared

#define ADDR_INPUT_On         0
#define ADDR_INPUT_Off	      1
#define ADDR_OUTPUT_CHANGE    2
#define ADDR_OUTPUT		      3

class RFDevice
{

public:
    
    uint16_t   ActiveCode;     		// id
    uint16_t   DesactiveCode;     	// id
    uint8_t    lengh;     			// id
    char*  	   sGroup;     			// id
    char*	   sFamily;				//
    char*	   sDevice;				//
    
    uint8_t   status;       // Inputs status


	RFDevice(); // constructor

public:

    uint8_t init(uint16_t _ActiveCode, uint16_t _DesactiveCode, uint8_t _lengh);
// Input 
	uint8_t inputIsOn();
	uint8_t inputIsOff();
	uint8_t inputPostOn();
	uint8_t inputPostOff();
	uint8_t inputResetOn();
	uint8_t inputResetOff();
// Output	
	uint8_t IsOutputChange();
	uint8_t PostOutputChange();
	uint8_t PostNotOutputChange();
	
	uint8_t IsOn();
	uint8_t IsOff();
	uint8_t PostOn();
	uint8_t PostOff();
	void Post(uint8_t setpoint);
};

void RFDevice_pre();
void RFDevice_post();

void Setup_RF();

#endif
