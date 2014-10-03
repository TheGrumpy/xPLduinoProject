#ifndef post_h
#define post_h

#include "Arduino.h"
#include "config.h"


/// temporaire
// type of class
#define T_MCU      0x01

#define T_SWITCH 0x0A
#define T_LIGHTING 0x0B
#define T_SHUTTER 0x0C
#define T_SHUTTER_DO1 0x0C
#define T_SHUTTER_DO2 0x2C

void lighting_to_dimmer(uint8_t lighting_id, uint8_t card_id, uint8_t card_channel);
void lighting_to_R8(uint8_t lighting_id, uint8_t card_id, uint8_t card_channel);
void lighting_to_in8R8(uint8_t lighting_id, uint8_t card_id, uint8_t card_channel);
void shutter_to_R8(uint8_t shutter_id, uint8_t card_id, uint8_t DO1_card_channel, uint8_t DO2_card_channel);
void shutter_to_in8R8(uint8_t shutter_id, uint8_t card_id, uint8_t DO1_card_channel, uint8_t DO2_card_channel);

void write_in8R8(uint8_t card_id, uint8_t card_channel, uint8_t value);
void set_in8R8(uint8_t card_id, uint8_t card_channel);
void clear_in8R8(uint8_t card_id, uint8_t card_channel);

byte aboutPostVersion();

#endif



