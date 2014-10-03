#ifndef TeleInfo_core_h
#define TeleInfo_core_h

#include "Arduino.h"
#include "config.h"

// Configuration Téléinfo
  #define DEBTRAME 0x02                 //caractère STX en hexadécimal.
  #define FINTRAME 0x03                 //caractère ETX en hexadécimal.
  #define DEBLIG 0x0a                 //caractère LF en hexadécimal.
  #define FINLIG 0x0d                 //caractère CR en hexadécimal.
  #define MONOPHASE 1                 // Monophase=true ou Monophase=false pour triphase
  #define TRIPHASE 0                 // Monophase=true ou Monophase=false pour triphase

  #define BASE   1
  #define HCHP   2
  #define EJP    3
  #define TEMPO  4

  #define SIZE_OF_BUFFIN  30

#define T_CMND 1
#define T_STAT 2
#define T_TRIG 3

class TeleInfo
{
public:
  // Nom du compteur
  char name[16+1];
  byte abo;
  boolean pres;            // presence du compteur

  byte debut_trame;
  byte fin_trame;
  byte initialiser;

// Données Compteur
  char adco[13];           //soit 12 + fin de chaine.
  char optarif[5];         //soit 4+ fin de chaine.
  long isousc;
  char motdetat[7];      //soit 6+ fin de chaine.
  long papp;
  char ptec[5];            //soit 4+ fin de chaine.
                                    // TH.. => Toutes les Heures
                                    // HC.. => Heures Creuses
                                    // HP.. => Heures Pleines
                                    // HN.. => Heures Normales
                                    // PM.. => Heures de Pointe Mobile
                                    // HCJB => Heures Creuses Jours Bleus
                                    // HCJW => Heures Creuses Jours Blancs
                                    // HCJR => Heures Creuses Jours Rouges
                                    // HPJB => Heures Pleines Jours Bleus
                                    // HPJW => Heures Pleines Jours Blancs
                                    // HPJR => Heures Pleines Jours Rouges
    // monophasé
    unsigned int iinst;         // Monophasé - Intensité Instantanée, A  (intensité efficace instantanée)
    unsigned int imax;          // Monophasé - Intensité maximale appelée, A

    // triphasé
    unsigned long ADPS;         // Avertissement de Dépassement Puissance Souscrite, A (n'est émis que lorsque la puissance consommée dépasse la puissance souscrite - intensité efficace instantanée)
    unsigned int IINST1;        // Triphasé - Intensité Instantanée Phase 1, A  (intensité efficace instantanée)
    unsigned int IINST2;        // Triphasé - Intensité Instantanée Phase 2, A  (intensité efficace instantanée)
    unsigned int IINST3;        // Triphasé - Intensité Instantanée Phase 3, A  (intensité efficace instantanée)
    unsigned int IMAX1;         // Triphasé - Intensité maximale appelée Phase 1, A
    unsigned int IMAX2;         // Triphasé - Intensité maximale appelée Phase 2, A
    unsigned int IMAX3;         // Triphasé - Intensité maximale appelée Phase 3, A
    byte PPOT;                  // Triphasé - Présence des potentiels, (code hexa, voir specification EDF)
    unsigned int PMAX;          // Triphasé - Puissance maximale triphasée atteinte, W
    // Uniquement suite à un dépassement d'intensité de réglage sur l'une des trois phases
    unsigned int ADIR1;         // Triphasé - Avertissement de Dépassement d'intensité de réglage par phase - Phase 1, A
    unsigned int ADIR2;         // Triphasé - Avertissement de Dépassement d'intensité de réglage par phase - Phase 2, A
    unsigned int ADIR3;         // Triphasé - Avertissement de Dépassement d'intensité de réglage par phase - Phase 2, A
    // Abonnement Base
    unsigned long base;          // Index option Base, Wh
    // Abonnement heures creuses
     unsigned long hchc;         // Consommation heures creuses
     unsigned long hchp;         // Consommation heures pleines
     char hhphc;                 //char HHPHC[1] = ""; // Horaire Heures Pleines Heures Creuses: A, C, D, E ou Y selon programmation du compteur
    // abonnement Effacement des jours de pointes (EJP)
    unsigned long EJPHN;         // Index option EJP - Heures Normales, Wh
    unsigned long EJPHPM;        // Index option EJP - Heures de Pointe Mobile, Wh
    unsigned long PEJP;          // Option EPJ - Préavis Début EJP (30 min), min (pendant toute la période de préavis et pendant la période de pointe mobile, valeur fixe à 30)
    // abonnement Tempo (Bleu,blanc,rouge)
    unsigned long BBRHCJB;      // Index option Tempo - Heures Creuses Jours Bleus, Wh
    unsigned long BBRHPJB;      // Index option Tempo - Heures Pleines Jours Bleus, Wh
    unsigned long BBRHCJW;      // Index option Tempo - Heures Creuses Jours Blancs, Wh
    unsigned long BBRHPJW;      // Index option Tempo - Heures Pleines Jours Blancs, Wh
    unsigned long BBRHCJR;      // Index option Tempo - Heures Creuses Jours Rouges, Wh
    unsigned long BBRHPJR;      // Index option Tempo - Heures Pleines Jours Rouges, Wh
    unsigned long DEMAIN;       // Option Tempo - Couleur du lendemain, ---- : non connue, BLEU : jour BLEU, BLAN : jour BLANC, ROUG : jour ROUGE

public:
  TeleInfo(void);
  void init(char *_name,byte _abo);
  void read();
  void TeleInfo_traitbuf(char *_buff);


private :
  byte inByte;                 // byte reçu sur la liaison série du compteur électrique.
  byte oldByte;                 // byte reçu sur la liaison série du compteur électrique a n-1.
  char buffin[SIZE_OF_BUFFIN];
  int bufflen;                     // Longueur d'un groupe téléinfo

};

char TeleInfo_cksum(char *_buff, int len);
byte aboutTeleinfoVersion();

#endif


