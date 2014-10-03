/*
 * version 0.1 : ajout pulse minute
 * version 0.2 : passage en mode 16bits (Timer 1), correction de la base de temps
 */

#define CLOCK_VERSION 0.2


#include "clock.h"

#include "Arduino.h"
#include <avr/io.h>
#include <avr/interrupt.h>
#include <avr/sleep.h>

volatile byte c10ms=0;
volatile byte c500ms=0;
volatile byte c1000ms=0;
volatile byte c1mn=0;

byte pulse_100ms=0;
byte pulse_1000ms=0;
byte pulse_500ms=0;
byte pulse_1mn=0;


ISR(TIMER1_COMPA_vect){
  c10ms++;
}

void clock_setup() {

    Serial.println(F("-------------- setup clock --------------"));

TCCR1A = 0; // TCCRnA – Timer/Counter n Control Register A
TCCR1B = 0; // TCCRnB – Timer/Counter n Control Register B
TCCR1C = 0; // TCCRnC – Timer/Counter n Control Register C

// 25000 pour faire 10ms
OCR1A=0x61A8;


TIMSK1=(0<<TOIE1)|(1<<OCIE1A)|(0<<OCIE1B);           // activate the  comparator A
TCCR1B=(0<<CS12)|(1<<CS11)|(0<<CS10)|(1<<WGM12);    // prescaler 1/8 and CTC mode

sei();

delay(1);

//~ set_sleep_mode(SLEEP_MODE_IDLE);   // sleep mode is set here

    Serial.println(F("------------- setup clock end -----------"));

}

void clock_update() {

  pulse_100ms=0;
  pulse_1000ms=0;
  pulse_500ms=0;
  pulse_1mn=0;

  if (c10ms>=10){ // 100 ms
    c10ms=0;
    c500ms++;
    c1000ms++;
    pulse_100ms=1;
  }

  if (c500ms>=5){ // 500 ms
    c500ms=0;
    pulse_500ms=1;
  }

  if (c1000ms>=10){ // 1 seconde
    c1000ms=0;
    pulse_1000ms=1;
    c1mn++;
  }

  if (c1mn>=60){ // 1 minute
    c1mn=0;
    pulse_1mn=1;
  }

  //~ sleep_enable();          // enables the sleep bit in the mcucr register
}

