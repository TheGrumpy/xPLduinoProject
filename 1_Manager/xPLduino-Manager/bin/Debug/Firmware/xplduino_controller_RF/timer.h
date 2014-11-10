#ifndef timer_h
#define timer_h

#include "Arduino.h"

#define TON 0
#define TOF 1
#define TP  2

#define MILLIS  0
#define SECOND  1
#define MINUTE  2

#define INPUT   0
#define Q_ONS   1
#define Q       2

class Timer
{
    public:

    Timer(void); // constructor

    uint8_t init(uint8_t _base, uint8_t _mode, uint16_t _setpoint);
    uint8_t q(uint8_t _input);
    uint8_t q();
    uint8_t q_ons(uint8_t _input);
    uint8_t q_ons();
    void reset();
    void update();

    private:
    uint8_t  status;      // status register
    uint8_t  base;        // base of time (ms, s...)
    uint8_t  mode;        // mode (ton, tof, tp...)
    uint16_t setpoint;   // setpoint
    uint16_t current;    // current value
    uint16_t temp;       // current value

    void update_ton();
    void update_tof();
    void update_tp();

};

    uint8_t aboutTimerVersion();

#endif

