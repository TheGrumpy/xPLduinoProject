#ifndef lighting_core_h
#define lighting_core_h

#include "Arduino.h"
#include "config.h"

#define DEFAULT     100
#define OFF         0
#define REDUCE      50

#define ADDR_POST0  4   fonction de post-traitement associé (cf tableau)
#define ADDR_POST1  5   fonction de post-traitement associé (cf tableau)
#define ADDR_POST2  6   fonction de post-traitement associé (cf tableau)
#define ADDR_POST3  7   fonction de post-traitement associé (cf tableau)

#define LIGHTING_ADDR_FADE           0   // 1=fade autorisée
#define LIGHTING_ADDR_LAST_DIRECTION 1   // 1=derniere direction était UP
#define LIGHTING_ADDR_SP_CHANGED     3   // 1=consigne a changé
#define LIGHTING_ADDR_STATUS         4   // 1=status envoyé

#define R_getFade()            bitRead(status, LIGHTING_ADDR_FADE)
#define R_LAST_DIRECTION()  bitRead(status, LIGHTING_ADDR_LAST_DIRECTION)
#define R_STATUS()          bitRead(status, LIGHTING_ADDR_STATUS)
#define R_SP_CHANGED()      bitRead(status, LIGHTING_ADDR_SP_CHANGED)


#define W_FADE(value)               bitWrite(status, LIGHTING_ADDR_FADE, value)
#define W_LAST_DIRECTION(value)     bitWrite(status, LIGHTING_ADDR_LAST_DIRECTION, value)
#define W_STATUS(value)             bitWrite(status, LIGHTING_ADDR_STATUS, value)
#define W_SP_CHANGED(value)         bitWrite(status, LIGHTING_ADDR_SP_CHANGED, value)


class Lighting
{

    private:

    Lighting(void); // constructor

    char   name[16+1];            // nom de l'instance
    uint8_t   status;                // status (bit0: 1=changement de valeur  en cours)
    uint8_t   fade;                  // fade rate par défaut (0=fade interdit)
    uint8_t   default_fade;          // fade rate par défaut (0=fade interdit)
    uint8_t   setpoint;              // consigne demandee
    uint8_t   default_setpoint;      // consigne par defaut (init)
    uint8_t   current;               // consigne demandee
    uint8_t   last_setpoint;         // dernière consigne demandée


    public:
    uint8_t init(char *_name, uint8_t _start_level, uint8_t _default_fade); // initialise l'instance

    void printConfig();

    uint8_t new_setpoint(uint8_t _new_setpoint, uint8_t _fade);   // force une nouvelle consigne sans tenir compte de la current actuelle
    uint8_t new_setpoint(uint8_t _new_setpoint);   // force une nouvelle consigne sans tenir compte de la current actuelle
    uint8_t toggle(); // allume au dernier setpoint ou eteint
    uint8_t toggle(uint8_t _mini, uint8_t _maxi, uint8_t _fade, uint8_t _timer); // bascule la sortie sur une nouvelle current mini ou maxi en fonction de la current actuelle
    uint8_t tune(void);
    uint8_t stop(void);
    uint8_t gotoLastSetpoint(void);
    uint8_t gotoDefaultSetpoint(void);

    uint8_t IsOn();
    uint8_t IsOff();
    uint8_t IsChanging();
    uint8_t IspostStatusSended();
    uint8_t IsFlagSPChanged();
    void    ResetFlagSPChanged();

    void    postStatusSended();
    void    postCurrent(uint8_t _current);

    uint8_t getSetpoint();
    uint8_t getFade();
    uint8_t getCurrent();
    char*   getName();



};

uint8_t aboutLightingVersion();
uint8_t lighting_hard_status();

#endif






