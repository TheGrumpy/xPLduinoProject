/*
 * Arduino ENC28J60 Ethernet shield UDP broadcast client
 */


#ifndef ETH_COM_H
#define ETH_COM_H

#include <EtherCard.h>
#include <stdint.h>
#include <xPL_Message.h>


#define udpPayload_SIZE 300

void setup_Udp();
void AfterParseAction(xPL_Message * message);
boolean isXpl( uint8_t *buffer);
void loop_Udp();

#endif
