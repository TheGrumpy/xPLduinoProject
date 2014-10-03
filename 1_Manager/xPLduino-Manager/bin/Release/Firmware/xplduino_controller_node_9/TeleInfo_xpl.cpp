//      Teleinfo_xpl.cpp
//
//      Copyright 2012
//
//      This program is free software; you can redistribute it and/or modify
//      it under the terms of the GNU General Public License as published by
//      the Free Software Foundation; either version 2 of the License, or
//      (at your option) any later version.
//
//      This program is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU General Public License for more details.
//
//      You should have received a copy of the GNU General Public License
//      along with this program; if not, write to the Free Software
//      Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
//      MA 02110-1301, USA.

#include "TeleInfo_xpl.h"
#include "TeleInfo_core.h"
#include <avr/pgmspace.h>

extern char vendor_id[];        // vendor id
extern char device_id[];        // device id
extern char instance_id[];      // instance id

#define TELEINFO_XPL_VERSION 2
// ajout du flag initialisé
// Normalisation des flag de debug
/*-------------------------------------------------------------------*/
extern TeleInfo TeleInfo;
extern xPL xpl;
void TeleInfoSendXPL(){
if (TeleInfo.pres==true && TeleInfo.initialiser==true){
char tmp[20]="\0";
      // creation du message XPL
      xPL_Message msg;
      msg.hop = 1;
      msg.type = XPL_STAT;
      msg.SetSource("xplduino","edf",instance_id);
      msg.SetTarget_P(PSTR("*"));
      msg.SetSchema_P(PSTR("teleinfo"), PSTR("basic"));
      msg.AddCommand(("device"),(TeleInfo.name));
      msg.AddCommand(("adco"),(TeleInfo.adco));
      msg.AddCommand(("optarif"),(TeleInfo.optarif));
    tmp[0]='\0';
    sprintf(tmp, "%i", TeleInfo.isousc);
    msg.AddCommand(("isousc"), tmp);
    msg.AddCommand(("motdetat"),(TeleInfo.motdetat));
    tmp[0]='\0';
    sprintf( tmp, "%s%i",tmp, TeleInfo.papp);
    msg.AddCommand(("papp"),tmp);
    if (TeleInfo.abo<=4) {
        tmp[0]='\0';
        sprintf( tmp, "%s%i",tmp, TeleInfo.iinst);
        msg.AddCommand(("iinst"),tmp);
        tmp[0]='\0';
        sprintf( tmp, "%s%i",tmp, TeleInfo.imax);
        msg.AddCommand(("imax"),tmp);
      }
      if (TeleInfo.abo>=5) {
        tmp[0]='\0';
        sprintf( tmp, "%s%i",tmp, TeleInfo.IINST1);
        msg.AddCommand(("IINST1"),tmp);
        tmp[0]='\0';
        sprintf( tmp, "%s%i",tmp, TeleInfo.IINST2);
        msg.AddCommand(("IINST2"),tmp);
        tmp[0]='\0';
        sprintf( tmp, "%s%i",tmp, TeleInfo.IINST3);
        msg.AddCommand(("IINST3"),tmp);
        tmp[0]='\0';
        sprintf("%d", tmp, TeleInfo.IMAX1);
        msg.AddCommand(("IMAX1"),tmp);
        tmp[0]='\0';
        sprintf( tmp, "%s%i",tmp, TeleInfo.IMAX2);
        msg.AddCommand(("IMAX2"),tmp);
        tmp[0]='\0';
        sprintf( tmp, "%s%i",tmp, TeleInfo.IMAX3);
        msg.AddCommand(("IMAX3"),(tmp));
        tmp[0]='\0';
        sprintf( tmp, "%s%i",tmp, TeleInfo.PPOT);
        msg.AddCommand(("PPOT"),tmp);
        tmp[0]='\0';
        sprintf( tmp, "%s%i",tmp, TeleInfo.PMAX);
        msg.AddCommand(("PMAX"),tmp);
        tmp[0]='\0';
        sprintf( tmp, "%s%i",tmp, TeleInfo.ADIR1);
        msg.AddCommand(("ADIR1"),tmp);
        tmp[0]='\0';
        sprintf( tmp, "%s%i",tmp, TeleInfo.ADIR2);
        msg.AddCommand(("ADIR2"),tmp);
        tmp[0]='\0';
        sprintf( tmp, "%s%i",tmp, TeleInfo.ADIR3);
        msg.AddCommand(("ADIR3"),tmp);
      }
      if (TeleInfo.abo==1) {
        tmp[0]='\0';
        itoa(TeleInfo.base, tmp, 10); //conversion
        msg.AddCommand(("BASE"),tmp);
      }
      // Abonnement heures creuses
      if (TeleInfo.abo==2) {
        tmp[0]='\0';
        sprintf(tmp, "%lu", TeleInfo.hchc);
        msg.AddCommand(("hchc"),tmp);
        tmp[0]='\0';
        sprintf(tmp, "%lu", TeleInfo.hchp);
        msg.AddCommand(("hchp"),tmp);
        tmp[0]='\0';
        sprintf(tmp, "%c", TeleInfo.hhphc);
        msg.AddCommand(("hhphc"),tmp);
      }
     // abonnement Effacement des jours de pointes (EJP)
      if (TeleInfo.abo==3) {
      tmp[0]='\0';
      itoa(TeleInfo.EJPHN, tmp, 10); //conversion
        msg.AddCommand(("EJPHN"),tmp);
    tmp[0]='\0';
        itoa(TeleInfo.EJPHPM, tmp, 10); //conversion
        msg.AddCommand(("EJPHPM"),tmp);
    tmp[0]='\0';
        itoa(TeleInfo.PEJP, tmp, 10); //conversion
        msg.AddCommand(("PEJP"),tmp);
      }
      // abonnement Tempo (Bleu,blanc,rouge)
      if (TeleInfo.abo==4) {
    tmp[0]='\0';
        itoa(TeleInfo.BBRHCJB, tmp, 10); //conversion
        //~ msg.AddCommand_P(PSTR("type"),PSTR("BBRHCJB"));
        msg.AddCommand(("BBRHCJB"),tmp);
    tmp[0]='\0';
        itoa(TeleInfo.BBRHPJB, tmp, 10); //conversion
        msg.AddCommand(("BBRHPJB"),tmp);
    tmp[0]='\0';
        itoa(TeleInfo.BBRHCJW, tmp, 10); //conversion
        msg.AddCommand(("BBRHCJW"),(tmp));
    tmp[0]='\0';
        itoa(TeleInfo.BBRHPJW, tmp, 10); //conversion
        msg.AddCommand(("BBRHPJW"),tmp);
    tmp[0]='\0';
        itoa(TeleInfo.BBRHCJR, tmp, 10); //conversion
        msg.AddCommand(("BBRHCJR"),tmp);
     tmp[0]='\0';
        itoa(TeleInfo.BBRHPJR, tmp, 10); //conversion
        msg.AddCommand(("BBRHPJR"),tmp);
    tmp[0]='\0';
        itoa(TeleInfo.DEMAIN, tmp, 10); //conversion
        msg.AddCommand(("DEMAIN"),tmp);
      }
      //* envoi du message */
    #ifdef  DEBUG_TELEINFO_XPL
        Serial.println(F("------------  teleinfo xPL  ------------"));Serial.println(msg.toString());
    #endif
    xpl.SendMessage(msg.toString());
    #ifdef DEBUG_TELEINFO_XPL
        Serial.println(F("++++++++++++  teleinfo xPL  ++++++++++++"));
    #endif
}
}
byte aboutTeleinfoXplVersion(){
    return TELEINFO_XPL_VERSION;
}



