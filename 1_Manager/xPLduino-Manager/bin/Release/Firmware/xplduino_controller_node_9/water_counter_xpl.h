#ifndef water_counter_xpl_h
#define water_counter_xpl_h

#include "Arduino.h"
#include <stdio.h>
#include <string.h>
#include "water_counter_core.h"
#include "config.h"
#include <xPL_Message.h>
#include <xPL.h>

    int water_counter_status(water_counter *_water_counter, byte _type);
    int water_counter_request(xPL_Message * message);
    int water_counter_find(char *_name, int first);
    byte aboutWaterCounterXplVersion();

#endif


