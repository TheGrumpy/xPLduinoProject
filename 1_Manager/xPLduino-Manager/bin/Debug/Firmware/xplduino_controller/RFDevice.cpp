#include "RFDevice.h"
#include "config.h"
#include "RCSwitch_mod.h"

#define RFDEVICES_VERSION 0

RCSwitch mySwitch = RCSwitch();

RFDevice::RFDevice(void)
{   
    status=0; 
}    
uint8_t RFDevice::init(uint16_t _ActiveCode, uint16_t _DesactiveCode, uint8_t _lengh){

	status=0;
	ActiveCode=_ActiveCode;
	DesactiveCode=_DesactiveCode;
	lengh=_lengh;
	
	// display warning if active code is pair (normally it's impair)
	if (ActiveCode%2==0){Serial.println("Warning - Active Code is pair (Generally this code is impair)");}
	// display warning if active code is  impair (normally it's pair)
	if (DesactiveCode%2==1){Serial.println("Warning - Desactive Code is impair (Generally this code is pair)");}

    return 0;
}

// Input
uint8_t RFDevice::inputIsOn(){
    return bitRead(status, ADDR_INPUT_On);
}
uint8_t RFDevice::inputPostOn(){
    return bitSet(status, ADDR_INPUT_On);
}
uint8_t RFDevice::inputResetOn(){
    return bitClear(status, ADDR_INPUT_On);
}
uint8_t RFDevice::inputIsOff(){
    return bitRead(status, ADDR_INPUT_Off);
}
uint8_t RFDevice::inputPostOff(){
    return bitSet(status, ADDR_INPUT_Off);
}
uint8_t RFDevice::inputResetOff(){
    return bitClear(status, ADDR_INPUT_Off);
}
// Output
uint8_t RFDevice::IsOutputChange(){
    return bitRead(status, ADDR_OUTPUT_CHANGE);
}
uint8_t RFDevice::PostOutputChange(){
    return bitSet(status, ADDR_OUTPUT_CHANGE);
}
uint8_t RFDevice::PostNotOutputChange(){
    return bitClear(status, ADDR_OUTPUT_CHANGE);
}
uint8_t RFDevice::IsOn(){
    return bitRead(status, ADDR_OUTPUT);
}
uint8_t RFDevice::IsOff(){
    if (bitRead(status, ADDR_OUTPUT)){
    	return (0);
    }else {
    	return (1);
    }
}
uint8_t RFDevice::PostOn(){
	PostOutputChange();
    return bitSet(status, ADDR_OUTPUT);
}
uint8_t RFDevice::PostOff(){
	PostOutputChange();
    return bitClear(status, ADDR_OUTPUT);
}
void RFDevice::Post(uint8_t setpoint){
	if (setpoint==0){
			PostOff();
		} 
		else {
			PostOn();
		} 
}

extern RFDevice *RFDEVICE;

void RFDevice_pre(){
int value=0;
	if (mySwitch.available()) {
		value = mySwitch.getReceivedValue();
		#ifdef DEBUG_DEVICERF
			Serial.println(value);
		#endif
    	mySwitch.resetAvailable();
  	 }
    for(int i=0; i<NumberOfRFDevice; i++){
    	if (value==RFDEVICE[i].ActiveCode){
    		RFDEVICE[i].inputPostOn();	
    		#ifdef DEBUG_DEVICERF
    			Serial.print(F("RF Device: "));Serial.print(i);Serial.println(F(" is On "));
    		#endif
    	}else if (value==RFDEVICE[i].DesactiveCode){
    		RFDEVICE[i].inputPostOff();
    		#ifdef DEBUG_DEVICERF
    			Serial.print(F("RF Device: "));Serial.print(i);Serial.println(F(" is Off "));
    		#endif
    	} else if (RFDEVICE[i].inputIsOn()){
    		RFDEVICE[i].inputResetOn();	
    		#ifdef DEBUG_DEVICERF
    			Serial.print(F("RF Device: "));Serial.print(i);Serial.println(F(" is reset (On)"));
    		#endif
    	} else if (RFDEVICE[i].inputIsOff()){
    		RFDEVICE[i].inputResetOff();	
    		#ifdef DEBUG_DEVICERF
    			Serial.print(F("RF Device: "));Serial.print(i);Serial.println(F(" is reset (Off)"));
    		#endif
    	}
    }
}

void RFDevice_post(){
				
    for(int i=0; i<NumberOfRFDevice; i++){
        if (RFDEVICE[i].IsOutputChange()){
        	if ((RFDEVICE[i].IsOff()==1)){
        		mySwitch.send((int) RFDEVICE[i].DesactiveCode,RFDEVICE[i].lengh);
        		#ifdef DEBUG_DEVICERF
        			Serial.print(F("RF Device : "));Serial.print(i);Serial.println(F(" post Off "));
        		#endif 
        	}else if ((RFDEVICE[i].IsOn()==1)) {
        		mySwitch.send((int) RFDEVICE[i].ActiveCode,RFDEVICE[i].lengh);
        		#ifdef DEBUG_DEVICERF
        			Serial.print(F("RF Device : "));Serial.print(i);Serial.println(F(" post On "));
        		#endif
        	}   
		RFDEVICE[i].PostNotOutputChange();  						
        }
    }
}

void Setup_RF(){
  mySwitch.enableReceivePCINT(1);  // Receiver on inerrupt PCINT0
  Serial.println(F("RF Device receive activate on PCINT1"));
  mySwitch.enableTransmit(20);
  Serial.println(F("RF Device transmit activate on PA1"));
}
