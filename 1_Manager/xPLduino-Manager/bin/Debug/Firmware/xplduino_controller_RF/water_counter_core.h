#ifndef water_counter_core_h
#define water_counter_core_h

#include "Arduino.h"


class water_counter
{
    public:

    water_counter(void); // constructor

    uint8_t  init(char *name, uint8_t _ratio, uint8_t _storage_address);
    void     reset();             // raz le compteur
    void     ctu();               // incrémente le compteur à chaque appel
    void     ctu(uint8_t _value); // incrémente le compteur de la valeur passé en paramètre
    uint16_t getValue();
    uint8_t  getTempValue();
    char*    getName();

    private:
    char     name[16+1];            // name of the counter
    uint8_t  status;                // status register
    uint16_t storage_address;       // where is store the water_counter value in ROM
    uint16_t current;               // current value
    uint8_t  temp_current;          // temp value
    uint8_t  ratio;                 // number of pulse to count a litre

};

    uint8_t aboutwater_counterVersion();

#endif

