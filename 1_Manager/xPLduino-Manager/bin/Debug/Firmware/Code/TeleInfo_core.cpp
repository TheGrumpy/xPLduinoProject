//      TeleInfo_core.cpp
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


#include "TeleInfo_core.h"
#include <SoftwareSerial.h>
#include "Arduino.h"
#include "config.h"

#define TELEINFO_CORE_VERSION 2
// ajout du flag perte de communication serie
// Normalisation des flag de debug
/********************************************************************************************
 * init() => initialise la connexion Teleinfo
 * read_TeleInfo() => Lit les informations du compteur
 Pour des meilleurs resultats augmenter le buffer du lien serie - /Harware/arduino/HardwareSerial.cpp de 64 à 128
 ********************************************************************************************/
TeleInfo::TeleInfo()
{
}

void TeleInfo::init(char *_name,byte _abo)
{
    Serial1.begin(1200);         // Port serie pour le diagnostique
  sprintf(name,"%s",_name);
  abo=_abo;
    debut_trame=0;
    fin_trame=0;
     Serial.print(F("Init teleinfo : "));Serial.print(name);Serial.print(F(" - Abonnement : "));
     if (abo==1){Serial.println(F(" Monophase standard"));}
     else if (abo==2){Serial.println(F(" Monophase Heures pleines/creuses"));}
     else if (abo==3){Serial.println(F(" Monophase effacement des jours de pointes"));}
     else if (abo==4){Serial.println(F(" Monophase tempo"));}
     else if (abo==5){Serial.println(F(" triphase standard"));}
     else if (abo==6){Serial.println(F(" triphase Heures pleines/creuses"));}
     else if (abo==7){Serial.println(F(" triphase effacement des jours de pointes"));}
     else if (abo==8){Serial.println(F(" triphase tempo"));}
     else if (abo>8){Serial.println(F(" inconnu"));}
}

///////////////////////////////////////////////////////////////////
// Fonction de récupération des données de groupe téléinfo EDF.
///////////////////////////////////////////////////////////////////

void TeleInfo::read()
{
  if (Serial1.available()==0){pres=false;initialiser=false;}
    while (Serial1.available()){ /// ici on risque de bloquer si on recoit en permanence qquechose
        inByte = Serial1.read() & 0x7F; // recuperation et mise au format de la donnée
        if(inByte == DEBTRAME){ // on est dans le cas d'un debut de trame, on raz tous les flags sauf debut trame
            bufflen = 0;
            buffin[bufflen]='\0';
            debut_trame=true;
            fin_trame=false;
            pres=true;
            #ifdef DEBUG_TELEINFO_SERIAL
                Serial.println("debut trame");
            #endif
        }else if(inByte == FINTRAME){ // on est dans le cas d'une fin de trame
            #ifdef DEBUG_TELEINFO_SERIAL
                Serial.println("finTrame");
            #endif
            if (debut_trame) { // as-t-on eu le debut ?
                #ifdef DEBUG_TELEINFO_SERIAL
                    Serial.println(" et complete");
                #endif
                fin_trame=true;
                initialiser=true;
            }
        }else if(debut_trame){ // on est en train de recuperer une ligne complete
            buffin[bufflen] = inByte;
            bufflen++;
            if (bufflen > SIZE_OF_BUFFIN){
                bufflen=0;//traitement anomalie sur la longueur d'un groupe du téléinformation.
                debut_trame=0;
                fin_trame=0;
                pres=false;
                #ifdef DEBUG_TELEINFO_SERIAL
                    Serial.println("anomalie");
                #endif
            }
            if(inByte == FINLIG){ // Fin de ligne trouvée.
                #ifdef DEBUG_TELEINFO_SERIAL
                    Serial.print("-> EOL, CRC=");Serial.print(buffin[bufflen-2], HEX);Serial.print(" calcul=");Serial.print(TeleInfo_cksum(buffin, bufflen-1), HEX);
                #endif
                if (TeleInfo_cksum(buffin,bufflen-1) == buffin[bufflen-2]){ // Test du CKsum
                    #ifdef DEBUG_TELEINFO_SERIAL
                        Serial.print("-> CRC ok");
                    #endif
                    TeleInfo_traitbuf(buffin); // CkSum OK => Visualisation de la Trame
                }
                bufflen=0;
            }
        }
  }
}
///////////////////////////////////////////////////////////////////
// Calcul Checksum Téléinfo
///////////////////////////////////////////////////////////////////
char TeleInfo_cksum(char *_buff, int len){
    int i;
    char sum = 0;
    for (i=1; i<(len-2); i++) sum = sum + _buff[i];
    sum = (sum & 0x3F) + 0x20;
    return(sum);
}
///////////////////////////////////////////////////////////////////
// Visualisation de la ligne de Teleinfo
///////////////////////////////////////////////////////////////////
void TeleInfo::TeleInfo_traitbuf(char *_buff){
    if (strncmp("ADCO ", &_buff[1] , 5)==0){
        strncpy(adco, &_buff[6], 12);
        adco[12]='\0';
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" ADCO "); Serial.println(adco);
        #endif
    }
    if (strncmp("OPTARIF ", &_buff[1] , 8)==0){
        strncpy(optarif, &_buff[9], 4);
        optarif[4]='\0';
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" OPTARIF ");Serial.println(optarif);
        #endif
    }
    if (strncmp("ISOUSC ", &_buff[1] , 7)==0){
        isousc = atol(&_buff[8]);
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" ISOUSC "); Serial.println(isousc,DEC);
        #endif
    }
        if (strncmp("MOTDETAT ", &_buff[1] , 9)==0){
        strncpy(motdetat, &_buff[10], 6);
        motdetat[6]='\0';
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" MOTDETAT "); Serial.println(motdetat);
        #endif
    }
    if (strncmp("PTEC ", &_buff[1] , 5)==0){
        strncpy(ptec, &_buff[6], 4);
        ptec[4]='\0';
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" PTEC "); Serial.println(ptec);
        #endif
    }
    if (strncmp("PAPP ", &_buff[1] , 5)==0){
        papp = atol(&_buff[6]);
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" PAPP ");Serial.println(papp,DEC);
        #endif
    }
  //Monophasee
  if (abo<=4){
    if (strncmp("IINST ", &_buff[1] , 6)==0){
        iinst = atol(&_buff[7]);
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" IINST "); Serial.println(iinst,DEC);
        #endif
    }
    if (strncmp("IMAX ", &_buff[1] , 5)==0){
        imax = atol(&_buff[6]);
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" IMAX "); Serial.println(imax,DEC);
        #endif
    }
  }
  //Triphasé
  if (abo>=5){
    if (strncmp("ADPS ", &_buff[1] , 5)==0){
        ADPS = atol(&_buff[6]);
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" ADPS "); Serial.println(ADPS,DEC);
        #endif
    }
        if (strncmp("IINST1 ", &_buff[1] , 7)==0){
        IINST1 = atol(&_buff[8]);
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" IINST1 "); Serial.println(IINST1,DEC);
        #endif
    }
    if (strncmp("IINST2 ", &_buff[1] , 7)==0){
        IINST2 = atol(&_buff[8]);
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" IINST2 "); Serial.println(IINST2,DEC);
        #endif
    }
    if (strncmp("IINST3 ", &_buff[1] , 7)==0){
        IINST3 = atol(&_buff[8]);
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" IINST3 "); Serial.println(IINST3,DEC);
        #endif
    }
    if (strncmp("IMAX1 ", &_buff[1] , 6)==0){
        IMAX1 = atol(&_buff[7]);
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" IMAX1 "); Serial.println(IMAX1,DEC);
        #endif
    }
    if (strncmp("IMAX2 ", &_buff[1] , 6)==0){
        IMAX2 = atol(&_buff[7]);
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" IMAX2 "); Serial.println(IMAX2,DEC);
        #endif
    }
    if (strncmp("IMAX3 ", &_buff[1] , 6)==0){
        IMAX3 = atol(&_buff[7]);
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" IMAX3 "); Serial.println(IMAX3,DEC);
        #endif
    }
    if (strncmp("PPOT ", &_buff[1] , 5)==0){
        PPOT = atol(&_buff[6]);
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" PPOT "); Serial.println(PPOT,DEC);
        #endif
    }
    if (strncmp("PMAX ", &_buff[1] , 5)==0){
        PMAX = atol(&_buff[6]);
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" PMAX "); Serial.println(PMAX,DEC);
        #endif
    }
    if (strncmp("ADIR1 ", &_buff[1] , 6)==0){
        ADIR1 = atol(&_buff[7]);
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" ADIR1 "); Serial.println(ADIR1,DEC);
        #endif
    }
    if (strncmp("ADIR2 ", &_buff[1] , 6)==0){
        ADIR2 = atol(&_buff[7]);
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" ADIR2 "); Serial.println(ADIR2,DEC);
        #endif
        }
        if (strncmp("ADIR3 ", &_buff[1] , 6)==0){
        ADIR3 = atol(&_buff[7]);
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" ADIR3 "); Serial.println(ADIR3,DEC);
        #endif
        }
  }
  // Abo de base
  if (abo==1){
        if (strncmp("BASE ", &_buff[1] , 5)==0){
                base = atol(&_buff[6]);
                #ifdef DEBUG_TELEINFO_CORE
                    Serial.print(" BASE ");Serial.println(base,DEC);
              #endif
    }
  }
  // Abo Heures pleines/Heures creuses
  if (abo==2){
        if (strncmp("HCHC ", &_buff[1] , 5)==0){
                hchc = atol(&_buff[6]);
                #ifdef DEBUG_TELEINFO_CORE
                    Serial.print(" HCHC ");Serial.println(hchc,DEC);
                #endif
    }
    if (strncmp("HCHP ", &_buff[1] , 5)==0){
        hchp = atol(&_buff[6]);
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" HCHP "); Serial.println(hchp,DEC);
        #endif
    }
    if (strncmp("HHPHC ", &_buff[1] , 6)==0){
        hhphc = _buff[7];
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" HHPHC ");Serial.println(hhphc);
        #endif
    }
  }
  // Abo Effacement des jours de pointe
  if (abo==3){
        if (strncmp("EJPHN ", &_buff[1] , 6)==0){
                EJPHN = atol(&_buff[7]);
                #ifdef DEBUG_TELEINFO_CORE
                    Serial.print(" EJPHN ");Serial.println(EJPHN,DEC);
                #endif
    }
    if (strncmp("EJPHPM ", &_buff[1] , 7)==0){
        EJPHPM = atol(&_buff[8]);
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" EJPHPM "); Serial.println(EJPHPM,DEC);
        #endif
    }
    if (strncmp("PEJP ", &_buff[1] , 5)==0){
        PEJP = _buff[6];
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" PEJP ");Serial.println(PEJP);
        #endif
    }
  }
// Abo Tempo ou Bleu,Blanc, rouge
  if (abo==4){
        if (strncmp("BBRHCJB ", &_buff[1] , 8)==0){
                BBRHCJB = atol(&_buff[9]);
                #ifdef DEBUG_TELEINFO_CORE
                    Serial.print(" BBRHCJB ");Serial.println(BBRHCJB,DEC);
                #endif
    }
        if (strncmp("BBRHPJB ", &_buff[1] , 8)==0){
                BBRHPJB = atol(&_buff[9]);
                #ifdef DEBUG_TELEINFO_CORE
                    Serial.print(" BBRHPJB ");Serial.println(BBRHPJB,DEC);
                #endif
    }
        if (strncmp("BBRHCJW ", &_buff[1] , 8)==0){
                BBRHCJW = atol(&_buff[9]);
                #ifdef DEBUG_TELEINFO_CORE
                    Serial.print(" BBRHCJW ");Serial.println(BBRHCJW,DEC);
                #endif
    }
        if (strncmp("BBRHPJW ", &_buff[1] , 8)==0){
                BBRHPJW = atol(&_buff[9]);
                #ifdef DEBUG_TELEINFO_CORE
                    Serial.print(" BBRHPJW ");Serial.println(BBRHPJW,DEC);
                #endif
    }
        if (strncmp("BBRHCJR ", &_buff[1] , 8)==0){
                BBRHCJR = atol(&_buff[9]);
                #ifdef DEBUG_TELEINFO_CORE
                    Serial.print(" BBRHCJR ");Serial.println(BBRHCJR,DEC);
                #endif
    }
        if (strncmp("BBRHPJR ", &_buff[1] , 8)==0){
                BBRHPJR = atol(&_buff[9]);
                #ifdef DEBUG_TELEINFO_CORE
                    Serial.print(" BBRHPJR ");Serial.println(BBRHPJR,DEC);
                #endif
    }
    if (strncmp("DEMAIN ", &_buff[1] , 5)==0){
        DEMAIN = atol(&_buff[6]);
        #ifdef DEBUG_TELEINFO_CORE
            Serial.print(" DEMAIN ");Serial.println(DEMAIN);
        #endif
    }
  }
}
byte aboutTeleinfoVersion(){
    return TELEINFO_CORE_VERSION;
}


