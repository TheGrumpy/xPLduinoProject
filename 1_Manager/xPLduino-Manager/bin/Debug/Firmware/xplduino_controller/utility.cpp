#include <DS1307new.h>

void printDate(){

    Serial.print(RTC.year);Serial.print(F("/"));Serial.print(RTC.month);Serial.print(F("/"));Serial.print(RTC.day);

}


void printTime(){

    Serial.print(RTC.hour);Serial.print(F(":"));Serial.print(RTC.minute);Serial.print(F(":"));Serial.print(RTC.second);

}


void printDateTime(){

    printDate();
    Serial.print(F(" - "));
    printTime();

}
