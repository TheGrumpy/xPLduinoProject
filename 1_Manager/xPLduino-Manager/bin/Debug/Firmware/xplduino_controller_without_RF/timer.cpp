

/*       Part of the xplduino project - http://code.google.com/p/xplduino/
 *       Write by Romain TISSOT CHARLOD
 *       Contact: xplduino@gmail.com
 *
 *       This program is free software; you can redistribute it and/or modify
 *       it under the terms of the GNU General Public License as published by
 *       the Free Software Foundation; either version 2 of the License, or
 *       (at your option) any later version.
 *
 *       This program is distributed in the hope that it will be useful,
 *       but WITHOUT ANY WARRANTY; without even the implied warranty of
 *       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *       GNU General Public License for more details.
 *
 *       You should have received a copy of the GNU General Public License
 *       along with this program; if not, write to the Free Software
 *       Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
 *       MA 02110-1301, USA
 *
 *       version 0: creation
 *       version 1: ajout support TP et TOF
 *
 */

#include "timer.h"

#define TEMPO_CORE_VERSION 2

// version 1: first release
// version 2: add reset() method to clear current register


// Constructor
Timer::Timer(void){}

// this routin, called one time at the start-up, initialize the command
uint8_t Timer::init(uint8_t _base, uint8_t _mode, uint16_t _setpoint)
{

    base=_base;         // base of time (ms, s...)
    mode=_mode;         // mode (ton, tof, tp...)
    setpoint=_setpoint; // setpoint
    current=0;          // current value
    temp=0;             // current value
    status=0;           // status register

    return 1;
}

/// probleme potentiel: si la tempo est dans une equation et qu'elle n'est pas évalué, ca va merder au prochain usage
/// il faudrait alors mettre à jour le soft pour évaluer systematiquement les tempos, meme si pas appelé
/// on pourrait mettre un flag à vrai en interne et l'update se ferait hors scenarios

uint8_t Timer::q(uint8_t _input)
{

    bitWrite(status, INPUT, _input);

    return bitRead(status, Q);

}

uint8_t Timer::q()
{

    return bitRead(status, Q);

}

uint8_t Timer::q_ons(uint8_t _input)
{

    bitWrite(status, INPUT, _input);

    return bitRead(status, Q_ONS);

}

uint8_t Timer::q_ons()
{

    return bitRead(status, Q_ONS);

}

void Timer::reset()
{

    current=0;

}

// this routin must be called every 100ms
void Timer::update()
{

    switch(mode){

        case TON:
            update_ton();
            break;
        case TOF:
            update_tof();
            break;
        case TP:
            update_tp();
            break;
        default:
        break;
   }
}

// this routin must be called every 100ms
void Timer::update_tof()
{

    bitClear(status, Q_ONS);

    if(bitRead(status, INPUT))
        bitSet(status, Q);

    if(bitRead(status, Q) &! bitRead(status, INPUT)){

        if(current){ // on continue de compter si on a pas déjà atteint la consigne
            switch(base){

                case MILLIS:
                    current -= 100;
                    break;
                case SECOND:
                    if(++temp>=10){
                        temp=0;
                        current--;
                    }
                    break;
                case MINUTE:
                    if(++temp>=600){
                        temp=0;
                        current--;
                    }
                break;
            }

        }else{ // on a terminé de compter

            //~ if(bitRead(status, Q)) // si on ne compte plus et qu'on a pas encore basculé la sortie on bascule le one shot
            bitSet(status, Q_ONS);

            bitClear(status, Q);

        }

    }else{

        current=setpoint;
        temp=0;
        //~ bitSet(status, Q);

    }

    bitClear(status, INPUT); // on efface le flag d'entrée, il doit normalement être remis à vrai dans le scenario. Si ce n'est pas le cas, la tempo est remise à zéro.

}

void Timer::update_ton()
{

    bitClear(status, Q_ONS);

    if(bitRead(status, INPUT)){

        if(current < setpoint){ // on continue de compter si on a pas déjà atteint la consigne
            switch(base){

                case MILLIS:
                    current += 100;
                    break;
                case SECOND:
                    if(++temp>=10){
                        temp=0;
                        current++;
                    }
                    break;
                case MINUTE:
                    if(++temp>=600){
                        temp=0;
                        current++;
                    }
                break;
            }
        }else{ // on a terminé de compter

            if(!bitRead(status, Q)) // si on ne compte plus et qu'on a pas encore basculé la sortie on bascule le one shot
                bitSet(status, Q_ONS);

            bitSet(status, Q);

        }

    }else{

        current=0;
        temp=0;
        bitClear(status, Q);

    }

    bitClear(status, INPUT); // on efface le flag d'entrée, il doit normalement être remis à vrai dans le scenario. Si ce n'est pas le cas, la tempo est remise à zéro.

}

void Timer::update_tp()
{

    bitClear(status, Q_ONS);

    if(bitRead(status, INPUT) || bitRead(status, Q)){

        if(current < setpoint){ // on continue de compter si on a pas déjà atteint la consigne
            switch(base){

                case MILLIS:
                    current += 100;
                    break;
                case SECOND:
                    if(++temp>=10){
                        temp=0;
                        current++;
                    }
                    break;
                case MINUTE:
                    if(++temp>=600){
                        temp=0;
                        current++;
                    }
                break;
            }

            bitSet(status, Q);

        }else{ // on a terminé de compter

            if(bitRead(status, Q)) // si on ne compte plus et qu'on a pas encore basculé la sortie on bascule le one shot
                bitSet(status, Q_ONS);

            current=0;
            temp=0;
            bitClear(status, Q);

        }

    }

    bitClear(status, INPUT); // on efface le flag d'entrée, il doit normalement être remis à vrai dans le scenario. Si ce n'est pas le cas, la tempo est remise à zéro.

}

uint8_t aboutTimerVersion(){
    return TEMPO_CORE_VERSION;
}
