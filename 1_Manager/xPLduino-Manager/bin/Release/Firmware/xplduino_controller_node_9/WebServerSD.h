// works only with tinyfat library
// with SD library packets lost will occurs
// don't know why ! 
// tested with arduino mega 1280 and uno
// send 240 Megabyte file without packet loss
// at 100 kbyte/s
// tinyfat read a block of 512 bytes on SD card , 
// so the buffer must be 512 + 60 bytes
// on the arduino mega with bigger buffer you can adjust 
// if (cur>=512) { // 512 to 1024 with 1100 bytes buffer


#ifndef WEBSERVER_H
#define WEBSERVER_H

#include <EtherCard.h>
#include <tinyFAT.h>
#include <avr/pgmspace.h>
#include "Arduino.h"

void setup_WebServerSD();
int loop_WebServerSD(int _len);

boolean checkUrl(const __FlashStringHelper *val, const char* data);
void writeHeaders(BufferFiller& buf);
void listTempJson(BufferFiller& buf);
void listTeleinfoJson(BufferFiller& buf);

void listLightJson(BufferFiller& buf);
void listSwitchJson(BufferFiller& buf);
void listShutterJson(BufferFiller& buf);
void listIn8R8Json(BufferFiller& buf);
void listIn4dim4Json(BufferFiller& buf);

void not_found();
byte streamfile (char* name , byte lastflag);
byte sendfiles(char* name);
void origin();

#endif


