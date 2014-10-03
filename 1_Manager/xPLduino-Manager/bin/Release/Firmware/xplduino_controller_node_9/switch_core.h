#ifndef switch_core_h
#define switch_core_h

#include "Arduino.h"
#include <stdio.h>
#include "config.h"

#define T_CMND 1
#define T_STAT 2
#define T_TRIG 3



#define HIGH 0x01
#define LOW  0x00

#define ADDR_TEMP           0   // 1=nouveau niveau de l'entree
#define ADDR_PULSE          1   // 1=appui pulse
#define ADDR_DPULSE         2   // 1=appui double pulse
#define ADDR_ON             3  // 1=appui maintenu
#define ADDR_ON_OSR         4   // 1=appui maintenu, sur un cycle
#define ADDR_ON_OSF         5   // 1=switch relaché, sur un cycle
#define ADDR_LEVEL          6  // 1=niveau precedent de l'entree
#define ADDR_INIT_OK        7

#define ADDR_HIGH           0   // 1=LOW, inverse le sens de l’entrée

#define R_LEVEL             bitRead(status, ADDR_LEVEL)
#define R_PULSE             bitRead(status, ADDR_PULSE)
#define R_DPULSE            bitRead(status, ADDR_DPULSE)
#define R_ON                bitRead(status, ADDR_ON)
#define R_ON_OSR            bitRead(status, ADDR_ON_OSR)
#define R_ON_OSF            bitRead(status, ADDR_ON_OSF)
#define R_TEMP              bitRead(status, ADDR_TEMP)
#define R_INIT_OK           bitRead(status, ADDR_INIT_OK)

#define R_HIGH              bitRead(parameter, ADDR_HIGH)

#define W_LEVEL(value)      bitWrite(status, ADDR_LEVEL, value)
#define W_PULSE(value)      bitWrite(status, ADDR_PULSE, value)
#define W_DPULSE(value)     bitWrite(status, ADDR_DPULSE, value)
#define W_ON(value)         bitWrite(status, ADDR_ON, value)
#define W_ON_OSR(value)     bitWrite(status, ADDR_ON_OSR, value)
#define W_ON_OSF(value)     bitWrite(status, ADDR_ON_OSF, value)
#define W_TEMP(value)       bitWrite(status, ADDR_TEMP, value)
#define W_INIT_OK(value)    bitWrite(status, ADDR_INIT_OK, value)

#define W_HIGH(value)       bitWrite(parameter, ADDR_HIGH, value)


class Switch
{
    public:

    //méthodes
    Switch();

    uint8_t init(char *_name, uint8_t _parameter, uint8_t _maintained_delay); // initialise les paramètres du switch
    uint8_t update(uint8_t _new_level); // mis à jour de l'état de l'entrée
    uint8_t isPulse();  // renvoi l'état pulse
    uint8_t isDoublePulse();  // renvoi l'état double pulse
    uint8_t isOn();    // renvoi l'état On
    uint8_t isOnOSR(); // renvoi l'état du trigger Off -> On
    uint8_t isOff();   // renvoi l'état Off
    uint8_t isOnOSF(); // renvoi l'état du trigger On -> Off
    uint8_t getStatus(); // renvoi l'état du registre status
    char*   getName();

    void printConfig(); // renvoi la config du switch

    private:

    uint8_t   maintained_delay;      // délai en ms button pour considérer comme mauint8_tenu
    uint8_t   timer_maintained;      // compteur de temps incrémenté tant que l’entrée est à 1 (x100ms)
    uint8_t   timer_doublepulse;     // compteur de temps décrémenté dès qu'une impulsion est détecté

    //~ public:

    char      name[16+1];            // nom du switch
    uint8_t   status;                // octet contenant les états calculés (cf détails)
    uint8_t   parameter;             // octet contenant un ensemble de paramètres (cf détails)


};


uint8_t aboutSwitchVersion();


#endif






