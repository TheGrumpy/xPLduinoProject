#ifndef temperature_xpl_h
#define temperature_xpl_h

#include "Arduino.h"
#include <stdio.h>
#include <string.h>
#include "config.h"
#include "temperature_core.h"
#include <xPL.h>
#include <xPL_Message.h>


#define T_CMND 1
#define T_STAT 2
#define T_TRIG 3

void temperature_status( Temperature * _temperature, byte _type);
void temperature_sendXPL();
byte aboutTemperatureXplVersion();
int sensor_request(xPL_Message * message);
int sensor_request(xPL_Message * message);
int temperature_find(char *_name, int first);
#endif
