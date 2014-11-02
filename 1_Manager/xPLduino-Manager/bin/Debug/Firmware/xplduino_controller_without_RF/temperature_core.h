#ifndef temperature_core_h
#define temperature_core_h

#include "Arduino.h"
#include "config.h"

#define MAX_Sensors 255

// Dallas commands
#define ConvertT    0x44
#define ReadScratchPad 0xBE
#define WriteScratchPad 0x4E
#define CopyScratchPad 0x48 // only Th Tl and configuration register
#define RecallEE 0xB8
#define ReadPowerSupply 0xB4


// Model IDs
#define DS18S20MODEL 0x10
#define DS18B20MODEL 0x28
#define DS1822MODEL  0x22

// Address locations
#define TEMP_LSB        0
#define TEMP_MSB        1
#define HIGH_ALARM_TEMP 2
#define LOW_ALARM_TEMP  3
#define CONFIGURATION   4
#define INTERNAL_BYTE   5
#define COUNT_REMAIN    6
#define COUNT_PER_C     7
#define SCRATCHPAD_CRC  8

// Device resolution
#define TEMP_9_BIT  0x1F //  9 bit
#define TEMP_10_BIT 0x3F // 10 bit
#define TEMP_11_BIT 0x5F // 11 bit
#define TEMP_12_BIT 0x7F // 12 bit

// Error Codes
#define DEVICE_DISCONNECTED -127


#define T_CMND 1
#define T_STAT 2
#define T_TRIG 3

// byte Temp_status
#define Addr_arrondi 0    // flag pour le type d'arrondi
#define Addr_sendXPL 1    // flag pour l'envois du message XPL
#define Addr_pres 2      // flag de presence de la sonde
#define Addr_confOK 3      // flag de validation de la configuration selon la precision

class Temperature
{
  private:
    Temperature();
    float tempC;
    byte addr[8];                   // addresse du capteur
    char name[16+1];                // nom du capteur
    float value;                    // temperature en Celcius


public:


    void read_temp();
    void configurePres();
    void request_temp();
    uint8_t init(char *_name,byte _adress0,byte _adress1,byte _adress2,byte _adress3,byte _adress4,byte _adress5,byte _adress6,byte _adress7,boolean arrond);
    byte getAddr(uint8_t Addr_ID);
    char* getName();
    float getValue();

    uint8_t IsPres();
    uint8_t IsSendToXPL();
    uint8_t IsArrondi();
    uint8_t IsConf();

    void postPresOk();
    void postPresNok();
    void postSendToXPL();
    void postNotSendToXPL();
    void postArrondi();
    void postNotArrondi();
    void postConfOk();
    void postConfNok();

    byte Temp_status;                // byte use to store status information

};
float Arrondi(float var);
void DS2482_Init();
void Read_DS2482_DeviceAdr();
void Read_DS2482_Temp(boolean timeSec);
byte aboutTemperatureVersion();
uint8_t crc8(const uint8_t *addr, uint8_t len);

#endif
