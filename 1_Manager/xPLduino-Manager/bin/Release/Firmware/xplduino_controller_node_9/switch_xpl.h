#ifndef switch_xpl_h
#define switch_xpl_h

#include "Arduino.h"
#include <stdio.h>
#include <string.h>
#include "switch_core.h"
#include "config.h"
#include <xPL_Message.h>
#include <xPL.h>

    int switch_status(Switch *_switch, byte _type);
    int switch_request(xPL_Message * message);
    int switch_find(char *_name, int first);
    byte aboutSwitchXplVersion();

#endif


