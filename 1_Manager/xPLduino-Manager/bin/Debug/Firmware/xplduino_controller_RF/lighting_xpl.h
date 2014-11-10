#ifndef lighting_xpl_h
#define lighting_xpl_h

#include "Arduino.h"
#include <stdio.h>
#include <string.h>
#include "lighting_core.h"
#include "config.h"
#include <xPL_Message.h>
#include <xPL.h>


    int lighting_basic(xPL_Message * message);
    void lighting_status( Lighting * _lighting, byte _type);
    int lighting_find(char *_name, int first);
    int lighting_request(xPL_Message * message);
    void lighting_sendGoto(char *_instance_id, char *_device, byte _new_setpoint, byte _new_fade);
    void lighting_sendToggle(char *_instance_id, char *_device);
    void lighting_sendStop(char *_instance_id, char *_device);
    char* lighting_version();
    byte aboutLightingXplVersion();

#endif




