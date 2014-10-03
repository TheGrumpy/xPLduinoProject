#ifndef shutter_xpl_h
#define shutter_xpl_h

#include "Arduino.h"
#include <stdio.h>
#include <string.h>
#include "shutter_core.h"
#include "config.h"
#include <xPL_Message.h>
#include <xPL.h>

    int shutter_basic(xPL_Message * message);
    void shutter_status( Shutter * _shutter, byte _type);
    int shutter_find(char *_name, int first);
    void shutter_sendGoto(char *_instance_id, char *_device, byte _new_setpoint);
    uint8_t shutter_sendCmd(char *_instance_id, char *_device, byte _command);
    uint8_t shutter_sendOpen(char *_instance_id, char *_device);
    uint8_t shutter_sendClose(char *_instance_id, char *_device);
    uint8_t shutter_sendStop(char *_instance_id, char *_device);
    uint8_t shutter_sendToggle(char *_instance_id, char *_device);
    int shutter_request(xPL_Message * message);
    byte aboutShutterXplVersion();

#endif






