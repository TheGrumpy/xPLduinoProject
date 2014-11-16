EESchema Schematic File Version 2  date lun. 03 nov. 2014 15:26:54 CET
LIBS:g5la-1-12vdc
LIBS:power
LIBS:device
LIBS:transistors
LIBS:conn
LIBS:linear
LIBS:regul
LIBS:74xx
LIBS:cmos4000
LIBS:adc-dac
LIBS:memory
LIBS:xilinx
LIBS:special
LIBS:microcontrollers
LIBS:dsp
LIBS:microchip
LIBS:analog_switches
LIBS:motorola
LIBS:texas
LIBS:intel
LIBS:audio
LIBS:interface
LIBS:digital-audio
LIBS:philips
LIBS:display
LIBS:cypress
LIBS:siliconi
LIBS:opto
LIBS:atmel
LIBS:contrib
LIBS:valves
LIBS:relays
LIBS:g5t-1-12vdc
LIBS:max485
LIBS:ALPS_SWITCHS
LIBS:4in4dim-cache
EELAYER 27 0
EELAYER END
$Descr A2 23386 16535
encoding utf-8
Sheet 1 1
Title "8in 8relays I2C board"
Date "3 nov 2014"
Rev "V0.2"
Comp "XPLDUINO project"
Comment1 "GNU GPL v2"
Comment2 "Domotic Open Source "
Comment3 ""
Comment4 ""
$EndDescr
$Comp
L R R2
U 1 1 50659F9F
P 3150 7300
F 0 "R2" V 3230 7300 50  0000 C CNN
F 1 "10KR" V 3150 7300 50  0000 C CNN
F 2 "" H 3150 7300 60  0001 C CNN
F 3 "" H 3150 7300 60  0001 C CNN
	1    3150 7300
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR01
U 1 1 50659FAE
P 3150 6950
F 0 "#PWR01" H 3150 7040 20  0001 C CNN
F 1 "+5V" H 3150 7040 30  0000 C CNN
F 2 "" H 3150 6950 60  0001 C CNN
F 3 "" H 3150 6950 60  0001 C CNN
	1    3150 6950
	1    0    0    -1  
$EndComp
$Comp
L CONN_3X2 ISP1
U 1 1 50670675
P 6700 5000
F 0 "ISP1" H 6700 5250 50  0000 C CNN
F 1 "CONN_3X2" V 6700 5050 40  0000 C CNN
F 2 "" H 6700 5000 60  0001 C CNN
F 3 "" H 6700 5000 60  0001 C CNN
	1    6700 5000
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR02
U 1 1 50670686
P 7200 4800
F 0 "#PWR02" H 7200 4890 20  0001 C CNN
F 1 "+5V" H 7200 4890 30  0000 C CNN
F 2 "" H 7200 4800 60  0001 C CNN
F 3 "" H 7200 4800 60  0001 C CNN
	1    7200 4800
	1    0    0    -1  
$EndComp
Text Notes 5750 4650 0    60   ~ 0
ISP CONNECTION
$Comp
L +5V #PWR03
U 1 1 506710FC
P 6100 8600
F 0 "#PWR03" H 6100 8690 20  0001 C CNN
F 1 "+5V" H 6100 8690 30  0000 C CNN
F 2 "" H 6100 8600 60  0001 C CNN
F 3 "" H 6100 8600 60  0001 C CNN
	1    6100 8600
	1    0    0    -1  
$EndComp
$Comp
L LED D1
U 1 1 5067143C
P 5850 9850
F 0 "D1" H 5850 9950 50  0000 C CNN
F 1 "LED" H 5850 9750 50  0000 C CNN
F 2 "" H 5850 9850 60  0001 C CNN
F 3 "" H 5850 9850 60  0001 C CNN
	1    5850 9850
	0    1    1    0   
$EndComp
$Comp
L R R1
U 1 1 5067144B
P 5850 10450
F 0 "R1" V 5930 10450 50  0000 C CNN
F 1 "4.7KR" V 5850 10450 50  0000 C CNN
F 2 "" H 5850 10450 60  0001 C CNN
F 3 "" H 5850 10450 60  0001 C CNN
	1    5850 10450
	1    0    0    -1  
$EndComp
$Comp
L LED D2
U 1 1 50671F30
P 13750 5700
F 0 "D2" H 13750 5800 50  0000 C CNN
F 1 "LED" H 13750 5600 50  0000 C CNN
F 2 "" H 13750 5700 60  0001 C CNN
F 3 "" H 13750 5700 60  0001 C CNN
	1    13750 5700
	1    0    0    -1  
$EndComp
$Comp
L CONN_2 P2
U 1 1 50674024
P 8050 4800
F 0 "P2" V 8000 4800 40  0000 C CNN
F 1 "CONN_2" V 8100 4800 40  0000 C CNN
F 2 "" H 8050 4800 60  0001 C CNN
F 3 "" H 8050 4800 60  0001 C CNN
	1    8050 4800
	0    -1   -1   0   
$EndComp
$Comp
L CONN_2 P3
U 1 1 50674033
P 8400 4800
F 0 "P3" V 8350 4800 40  0000 C CNN
F 1 "CONN_2" V 8450 4800 40  0000 C CNN
F 2 "" H 8400 4800 60  0001 C CNN
F 3 "" H 8400 4800 60  0001 C CNN
	1    8400 4800
	0    -1   -1   0   
$EndComp
$Comp
L CONN_2 P4
U 1 1 50674042
P 8750 4800
F 0 "P4" V 8700 4800 40  0000 C CNN
F 1 "CONN_2" V 8800 4800 40  0000 C CNN
F 2 "" H 8750 4800 60  0001 C CNN
F 3 "" H 8750 4800 60  0001 C CNN
	1    8750 4800
	0    -1   -1   0   
$EndComp
$Comp
L CONN_2 P5
U 1 1 50674051
P 9100 4800
F 0 "P5" V 9050 4800 40  0000 C CNN
F 1 "CONN_2" V 9150 4800 40  0000 C CNN
F 2 "" H 9100 4800 60  0001 C CNN
F 3 "" H 9100 4800 60  0001 C CNN
	1    9100 4800
	0    -1   -1   0   
$EndComp
Text Notes 7750 4650 0    60   ~ 0
I2C ADDRESS
Text Notes 6000 2100 2    60   ~ 0
POWER
Text Notes 5600 8250 2    60   ~ 0
MCU
$Comp
L PWR_FLAG #FLG04
U 1 1 50675FF0
P 5850 3350
F 0 "#FLG04" H 5850 3445 30  0001 C CNN
F 1 "PWR_FLAG" H 5850 3530 30  0000 C CNN
F 2 "" H 5850 3350 60  0001 C CNN
F 3 "" H 5850 3350 60  0001 C CNN
	1    5850 3350
	1    0    0    -1  
$EndComp
$Comp
L VCC #PWR05
U 1 1 50676001
P 6100 3300
F 0 "#PWR05" H 6100 3400 30  0001 C CNN
F 1 "VCC" H 6100 3400 30  0000 C CNN
F 2 "" H 6100 3300 60  0001 C CNN
F 3 "" H 6100 3300 60  0001 C CNN
	1    6100 3300
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR06
U 1 1 50676010
P 6300 3300
F 0 "#PWR06" H 6300 3390 20  0001 C CNN
F 1 "+5V" H 6300 3390 30  0000 C CNN
F 2 "" H 6300 3300 60  0001 C CNN
F 3 "" H 6300 3300 60  0001 C CNN
	1    6300 3300
	1    0    0    -1  
$EndComp
$Comp
L C C1
U 1 1 5067601F
P 5850 3650
F 0 "C1" H 5700 3750 50  0000 L CNN
F 1 "0.1uF" H 5850 3550 50  0000 L CNN
F 2 "" H 5850 3650 60  0001 C CNN
F 3 "" H 5850 3650 60  0001 C CNN
	1    5850 3650
	1    0    0    -1  
$EndComp
$Comp
L C C2
U 1 1 5067602E
P 6100 3650
F 0 "C2" H 5950 3750 50  0000 L CNN
F 1 "0.1uF" H 6100 3550 50  0000 L CNN
F 2 "" H 6100 3650 60  0001 C CNN
F 3 "" H 6100 3650 60  0001 C CNN
	1    6100 3650
	1    0    0    -1  
$EndComp
Text GLabel 8500 10250 2    60   Output ~ 0
_OUT3
Text GLabel 8100 10150 2    60   Output ~ 0
_OUT2
Text GLabel 8500 10650 2    60   Output ~ 0
_OUT0
Text GLabel 8500 10050 2    60   Output ~ 0
_OUT1
Text GLabel 8500 9400 2    60   Output ~ 0
MOSI
Text GLabel 8100 9500 2    60   Input ~ 0
MISO
Text GLabel 8500 9600 2    60   Output ~ 0
SCK
Text GLabel 8100 9950 2    60   Output ~ 0
/INT
Text GLabel 9200 5200 3    60   Output ~ 0
A0
Text GLabel 8850 5200 3    60   Output ~ 0
A1
Text GLabel 8500 5200 3    60   Output ~ 0
A2
Text GLabel 8150 5200 3    60   Output ~ 0
A3
Text GLabel 8100 11100 2    60   Input ~ 0
A0
Text GLabel 8500 11200 2    60   Input ~ 0
A1
Text GLabel 8100 8850 2    60   Input ~ 0
A2
Text GLabel 8500 8950 2    60   Input ~ 0
A3
$Comp
L ATTINY48-A IC1
U 1 1 51359DD8
P 7050 10050
F 0 "IC1" H 6300 11600 50  0000 L BNN
F 1 "ATTINY48-A" H 7350 8400 50  0000 L BNN
F 2 "TQFP32" H 6450 8450 50  0001 C CNN
F 3 "" H 7050 10050 60  0000 C CNN
	1    7050 10050
	1    0    0    -1  
$EndComp
Text GLabel 8400 11400 2    60   Input ~ 0
CMD0
Text GLabel 8150 10350 2    60   BiDi ~ 0
SDA
Text GLabel 8500 10450 2    60   BiDi ~ 0
SCL
Text GLabel 5850 9500 1    60   Input ~ 0
LED
Text GLabel 8500 11000 2    60   Input ~ 0
ZC
$Comp
L GND #PWR07
U 1 1 5135A23A
P 6050 11700
F 0 "#PWR07" H 6050 11700 30  0001 C CNN
F 1 "GND" H 6050 11630 30  0001 C CNN
F 2 "" H 6050 11700 60  0001 C CNN
F 3 "" H 6050 11700 60  0001 C CNN
	1    6050 11700
	1    0    0    -1  
$EndComp
$Comp
L GND #PWR08
U 1 1 5135D759
P 7200 5100
F 0 "#PWR08" H 7200 5100 30  0001 C CNN
F 1 "GND" H 7200 5030 30  0001 C CNN
F 2 "" H 7200 5100 60  0001 C CNN
F 3 "" H 7200 5100 60  0001 C CNN
	1    7200 5100
	1    0    0    -1  
$EndComp
$Comp
L GND #PWR09
U 1 1 5135DC92
P 6100 3950
F 0 "#PWR09" H 6100 3950 30  0001 C CNN
F 1 "GND" H 6100 3880 30  0001 C CNN
F 2 "" H 6100 3950 60  0001 C CNN
F 3 "" H 6100 3950 60  0001 C CNN
	1    6100 3950
	1    0    0    -1  
$EndComp
$Comp
L GND #PWR010
U 1 1 5135DD2A
P 7950 5250
F 0 "#PWR010" H 7950 5250 30  0001 C CNN
F 1 "GND" H 7950 5180 30  0001 C CNN
F 2 "" H 7950 5250 60  0001 C CNN
F 3 "" H 7950 5250 60  0001 C CNN
	1    7950 5250
	1    0    0    -1  
$EndComp
$Comp
L GND #PWR011
U 1 1 5135DD30
P 8300 5250
F 0 "#PWR011" H 8300 5250 30  0001 C CNN
F 1 "GND" H 8300 5180 30  0001 C CNN
F 2 "" H 8300 5250 60  0001 C CNN
F 3 "" H 8300 5250 60  0001 C CNN
	1    8300 5250
	1    0    0    -1  
$EndComp
$Comp
L GND #PWR012
U 1 1 5135DD36
P 8650 5250
F 0 "#PWR012" H 8650 5250 30  0001 C CNN
F 1 "GND" H 8650 5180 30  0001 C CNN
F 2 "" H 8650 5250 60  0001 C CNN
F 3 "" H 8650 5250 60  0001 C CNN
	1    8650 5250
	1    0    0    -1  
$EndComp
$Comp
L GND #PWR013
U 1 1 5135DD3C
P 9000 5250
F 0 "#PWR013" H 9000 5250 30  0001 C CNN
F 1 "GND" H 9000 5180 30  0001 C CNN
F 2 "" H 9000 5250 60  0001 C CNN
F 3 "" H 9000 5250 60  0001 C CNN
	1    9000 5250
	1    0    0    -1  
$EndComp
$Comp
L GND #PWR014
U 1 1 5135DD56
P 5850 10800
F 0 "#PWR014" H 5850 10800 30  0001 C CNN
F 1 "GND" H 5850 10730 30  0001 C CNN
F 2 "" H 5850 10800 60  0001 C CNN
F 3 "" H 5850 10800 60  0001 C CNN
	1    5850 10800
	1    0    0    -1  
$EndComp
Text GLabel 9200 10550 2    60   BiDi ~ 0
RESET
Text GLabel 6200 5100 0    60   BiDi ~ 0
RESET
Text GLabel 6200 4950 0    60   BiDi ~ 0
SCK
Text GLabel 6200 4800 0    60   BiDi ~ 0
MISO
Text GLabel 7100 4950 2    60   BiDi ~ 0
MOSI
$Comp
L CONN_8X2 P1
U 1 1 5136E504
P 7600 13800
F 0 "P1" H 7600 14350 60  0000 C CNN
F 1 "CONN_8X2" V 7600 13700 50  0000 C CNN
F 2 "" H 7600 13800 60  0000 C CNN
F 3 "" H 7600 13800 60  0000 C CNN
	1    7600 13800
	1    0    0    -1  
$EndComp
Text GLabel 6650 13750 0    60   Input ~ 0
OUT3
Text GLabel 7000 13650 0    60   Input ~ 0
OUT2
Text GLabel 6650 13550 0    60   Input ~ 0
OUT1
Text GLabel 7000 13450 0    60   Input ~ 0
OUT0
Text GLabel 8200 13450 2    60   Output ~ 0
CMD3
Text GLabel 8600 13550 2    60   Output ~ 0
CMD2
Text GLabel 8200 13650 2    60   Output ~ 0
CMD1
Text GLabel 8600 13750 2    60   Output ~ 0
CMD0
Text GLabel 8200 13850 2    60   Input ~ 0
ZC
$Comp
L GND #PWR015
U 1 1 5136F04A
P 7100 14200
F 0 "#PWR015" H 7100 14200 30  0001 C CNN
F 1 "GND" H 7100 14130 30  0001 C CNN
F 2 "" H 7100 14200 60  0001 C CNN
F 3 "" H 7100 14200 60  0001 C CNN
	1    7100 14200
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR016
U 1 1 5136F050
P 8100 14250
F 0 "#PWR016" H 8100 14340 20  0001 C CNN
F 1 "+5V" H 8100 14340 30  0000 C CNN
F 2 "" H 8100 14250 60  0001 C CNN
F 3 "" H 8100 14250 60  0001 C CNN
	1    8100 14250
	-1   0    0    1   
$EndComp
Text Notes 5500 12800 0    60   ~ 0
EXTERNAL CONNECTOR (extension)
$Comp
L R R3
U 1 1 5165BE3E
P 9450 6800
F 0 "R3" V 9530 6800 50  0000 C CNN
F 1 "10KR" V 9450 6800 50  0000 C CNN
F 2 "" H 9450 6800 60  0001 C CNN
F 3 "" H 9450 6800 60  0001 C CNN
	1    9450 6800
	1    0    0    -1  
$EndComp
$Comp
L R R4
U 1 1 5165BE57
P 9750 6800
F 0 "R4" V 9830 6800 50  0000 C CNN
F 1 "10KR" V 9750 6800 50  0000 C CNN
F 2 "" H 9750 6800 60  0001 C CNN
F 3 "" H 9750 6800 60  0001 C CNN
	1    9750 6800
	1    0    0    -1  
$EndComp
Text GLabel 9450 7050 3    60   BiDi ~ 0
SDA
Text GLabel 9750 7050 3    60   BiDi ~ 0
SCL
$Comp
L +5V #PWR017
U 1 1 5165C1A4
P 9450 6550
F 0 "#PWR017" H 9450 6460 20  0001 C CNN
F 1 "+5V" H 9450 6460 30  0000 C CNN
F 2 "" H 9450 6550 60  0000 C CNN
F 3 "" H 9450 6550 60  0000 C CNN
	1    9450 6550
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR018
U 1 1 5165C1B1
P 9750 6550
F 0 "#PWR018" H 9750 6460 20  0001 C CNN
F 1 "+5V" H 9750 6460 30  0000 C CNN
F 2 "" H 9750 6550 60  0000 C CNN
F 3 "" H 9750 6550 60  0000 C CNN
	1    9750 6550
	1    0    0    -1  
$EndComp
$Comp
L C C3
U 1 1 51AB4C03
P 6300 3650
F 0 "C3" H 6200 3750 50  0000 L CNN
F 1 "4.7uF" H 6300 3550 50  0000 L CNN
F 2 "" H 6300 3650 60  0001 C CNN
F 3 "" H 6300 3650 60  0001 C CNN
	1    6300 3650
	1    0    0    -1  
$EndComp
Text GLabel 8450 9800 2    60   Input ~ 0
CMD2
Text GLabel 8100 11300 2    60   Input ~ 0
CMD1
Text GLabel 8100 9700 2    60   Input ~ 0
CMD3
NoConn ~ 8000 9300
NoConn ~ 8000 8750
NoConn ~ 8000 8650
Text GLabel 8100 10900 2    60   Output ~ 0
TX
Text GLabel 8500 10800 2    60   Input ~ 0
RX
Text Notes 8750 10900 0    60   ~ 0
vers MAX485 ?
Text GLabel 8100 11500 2    60   Output ~ 0
SEL
Text GLabel 8600 13950 2    60   BiDi ~ 0
SDA
Text GLabel 8200 14050 2    60   BiDi ~ 0
SCL
Text GLabel 6650 13950 0    60   Output ~ 0
/INT
NoConn ~ 7200 14050
NoConn ~ 7200 13850
Text GLabel 14050 5700 2    60   Output ~ 0
OUT0
Text GLabel 14050 6050 2    60   Output ~ 0
OUT1
Text GLabel 14050 6750 2    60   Output ~ 0
OUT3
Text GLabel 14050 6400 2    60   Output ~ 0
OUT2
Text GLabel 13450 5700 0    60   Input ~ 0
_OUT0
Text GLabel 13450 6050 0    60   Input ~ 0
_OUT1
Text GLabel 13450 6400 0    60   Input ~ 0
_OUT2
Text GLabel 13450 6750 0    60   Input ~ 0
_OUT3
$Comp
L LED D3
U 1 1 535580DB
P 13750 6050
F 0 "D3" H 13750 6150 50  0000 C CNN
F 1 "LED" H 13750 5950 50  0000 C CNN
F 2 "" H 13750 6050 60  0001 C CNN
F 3 "" H 13750 6050 60  0001 C CNN
	1    13750 6050
	1    0    0    -1  
$EndComp
$Comp
L LED D4
U 1 1 535580E5
P 13750 6400
F 0 "D4" H 13750 6500 50  0000 C CNN
F 1 "LED" H 13750 6300 50  0000 C CNN
F 2 "" H 13750 6400 60  0001 C CNN
F 3 "" H 13750 6400 60  0001 C CNN
	1    13750 6400
	1    0    0    -1  
$EndComp
$Comp
L LED D5
U 1 1 535580ED
P 13750 6750
F 0 "D5" H 13750 6850 50  0000 C CNN
F 1 "LED" H 13750 6650 50  0000 C CNN
F 2 "" H 13750 6750 60  0001 C CNN
F 3 "" H 13750 6750 60  0001 C CNN
	1    13750 6750
	1    0    0    -1  
$EndComp
$Comp
L PNP Q1
U 1 1 535581D6
P 13750 8000
F 0 "Q1" H 13750 7850 60  0000 R CNN
F 1 "PNP" H 13750 8150 60  0000 R CNN
F 2 "" H 13750 8000 60  0000 C CNN
F 3 "" H 13750 8000 60  0000 C CNN
	1    13750 8000
	1    0    0    -1  
$EndComp
$Comp
L R R5
U 1 1 535581E3
P 13300 8000
F 0 "R5" V 13380 8000 50  0000 C CNN
F 1 "10KR" V 13300 8000 50  0000 C CNN
F 2 "" H 13300 8000 60  0001 C CNN
F 3 "" H 13300 8000 60  0001 C CNN
	1    13300 8000
	0    1    1    0   
$EndComp
$Comp
L R R9
U 1 1 535581E9
P 14100 8200
F 0 "R9" V 14180 8200 50  0000 C CNN
F 1 "4.7kR" V 14100 8200 50  0000 C CNN
F 2 "" H 14100 8200 60  0001 C CNN
F 3 "" H 14100 8200 60  0001 C CNN
	1    14100 8200
	0    1    1    0   
$EndComp
$Comp
L +5V #PWR019
U 1 1 535581EF
P 13850 7800
F 0 "#PWR019" H 13850 7890 20  0001 C CNN
F 1 "+5V" H 13850 7890 30  0000 C CNN
F 2 "" H 13850 7800 60  0001 C CNN
F 3 "" H 13850 7800 60  0001 C CNN
	1    13850 7800
	1    0    0    -1  
$EndComp
$Comp
L LED D6
U 1 1 535581F5
P 14550 8200
F 0 "D6" H 14550 8300 50  0000 C CNN
F 1 "LED" H 14550 8100 50  0000 C CNN
F 2 "" H 14550 8200 60  0001 C CNN
F 3 "" H 14550 8200 60  0001 C CNN
	1    14550 8200
	1    0    0    -1  
$EndComp
$Comp
L GND #PWR020
U 1 1 535581FB
P 14900 8400
F 0 "#PWR020" H 14900 8400 30  0001 C CNN
F 1 "GND" H 14900 8330 30  0001 C CNN
F 2 "" H 14900 8400 60  0001 C CNN
F 3 "" H 14900 8400 60  0001 C CNN
	1    14900 8400
	1    0    0    -1  
$EndComp
Text GLabel 13050 8000 0    60   Input ~ 0
CMD0
$Comp
L PNP Q2
U 1 1 53558292
P 13750 8800
F 0 "Q2" H 13750 8650 60  0000 R CNN
F 1 "PNP" H 13750 8950 60  0000 R CNN
F 2 "" H 13750 8800 60  0000 C CNN
F 3 "" H 13750 8800 60  0000 C CNN
	1    13750 8800
	1    0    0    -1  
$EndComp
$Comp
L R R6
U 1 1 53558298
P 13300 8800
F 0 "R6" V 13380 8800 50  0000 C CNN
F 1 "10KR" V 13300 8800 50  0000 C CNN
F 2 "" H 13300 8800 60  0001 C CNN
F 3 "" H 13300 8800 60  0001 C CNN
	1    13300 8800
	0    1    1    0   
$EndComp
$Comp
L R R10
U 1 1 5355829E
P 14100 9000
F 0 "R10" V 14180 9000 50  0000 C CNN
F 1 "4.7kR" V 14100 9000 50  0000 C CNN
F 2 "" H 14100 9000 60  0001 C CNN
F 3 "" H 14100 9000 60  0001 C CNN
	1    14100 9000
	0    1    1    0   
$EndComp
$Comp
L +5V #PWR021
U 1 1 535582A4
P 13850 8600
F 0 "#PWR021" H 13850 8690 20  0001 C CNN
F 1 "+5V" H 13850 8690 30  0000 C CNN
F 2 "" H 13850 8600 60  0001 C CNN
F 3 "" H 13850 8600 60  0001 C CNN
	1    13850 8600
	1    0    0    -1  
$EndComp
$Comp
L LED D7
U 1 1 535582AA
P 14550 9000
F 0 "D7" H 14550 9100 50  0000 C CNN
F 1 "LED" H 14550 8900 50  0000 C CNN
F 2 "" H 14550 9000 60  0001 C CNN
F 3 "" H 14550 9000 60  0001 C CNN
	1    14550 9000
	1    0    0    -1  
$EndComp
$Comp
L GND #PWR022
U 1 1 535582B0
P 14900 9200
F 0 "#PWR022" H 14900 9200 30  0001 C CNN
F 1 "GND" H 14900 9130 30  0001 C CNN
F 2 "" H 14900 9200 60  0001 C CNN
F 3 "" H 14900 9200 60  0001 C CNN
	1    14900 9200
	1    0    0    -1  
$EndComp
Text GLabel 13050 8800 0    60   Input ~ 0
CMD1
$Comp
L PNP Q3
U 1 1 535582B9
P 13750 9600
F 0 "Q3" H 13750 9450 60  0000 R CNN
F 1 "PNP" H 13750 9750 60  0000 R CNN
F 2 "" H 13750 9600 60  0000 C CNN
F 3 "" H 13750 9600 60  0000 C CNN
	1    13750 9600
	1    0    0    -1  
$EndComp
$Comp
L R R7
U 1 1 535582BF
P 13300 9600
F 0 "R7" V 13380 9600 50  0000 C CNN
F 1 "10KR" V 13300 9600 50  0000 C CNN
F 2 "" H 13300 9600 60  0001 C CNN
F 3 "" H 13300 9600 60  0001 C CNN
	1    13300 9600
	0    1    1    0   
$EndComp
$Comp
L R R11
U 1 1 535582C5
P 14100 9800
F 0 "R11" V 14180 9800 50  0000 C CNN
F 1 "4.7kR" V 14100 9800 50  0000 C CNN
F 2 "" H 14100 9800 60  0001 C CNN
F 3 "" H 14100 9800 60  0001 C CNN
	1    14100 9800
	0    1    1    0   
$EndComp
$Comp
L +5V #PWR023
U 1 1 535582CB
P 13850 9400
F 0 "#PWR023" H 13850 9490 20  0001 C CNN
F 1 "+5V" H 13850 9490 30  0000 C CNN
F 2 "" H 13850 9400 60  0001 C CNN
F 3 "" H 13850 9400 60  0001 C CNN
	1    13850 9400
	1    0    0    -1  
$EndComp
$Comp
L LED D8
U 1 1 535582D1
P 14550 9800
F 0 "D8" H 14550 9900 50  0000 C CNN
F 1 "LED" H 14550 9700 50  0000 C CNN
F 2 "" H 14550 9800 60  0001 C CNN
F 3 "" H 14550 9800 60  0001 C CNN
	1    14550 9800
	1    0    0    -1  
$EndComp
$Comp
L GND #PWR024
U 1 1 535582D7
P 14900 10000
F 0 "#PWR024" H 14900 10000 30  0001 C CNN
F 1 "GND" H 14900 9930 30  0001 C CNN
F 2 "" H 14900 10000 60  0001 C CNN
F 3 "" H 14900 10000 60  0001 C CNN
	1    14900 10000
	1    0    0    -1  
$EndComp
Text GLabel 13050 9600 0    60   Input ~ 0
CMD2
$Comp
L PNP Q4
U 1 1 535582E0
P 13750 10400
F 0 "Q4" H 13750 10250 60  0000 R CNN
F 1 "PNP" H 13750 10550 60  0000 R CNN
F 2 "" H 13750 10400 60  0000 C CNN
F 3 "" H 13750 10400 60  0000 C CNN
	1    13750 10400
	1    0    0    -1  
$EndComp
$Comp
L R R8
U 1 1 535582E6
P 13300 10400
F 0 "R8" V 13380 10400 50  0000 C CNN
F 1 "10KR" V 13300 10400 50  0000 C CNN
F 2 "" H 13300 10400 60  0001 C CNN
F 3 "" H 13300 10400 60  0001 C CNN
	1    13300 10400
	0    1    1    0   
$EndComp
$Comp
L R R12
U 1 1 535582EC
P 14100 10600
F 0 "R12" V 14180 10600 50  0000 C CNN
F 1 "4.7kR" V 14100 10600 50  0000 C CNN
F 2 "" H 14100 10600 60  0001 C CNN
F 3 "" H 14100 10600 60  0001 C CNN
	1    14100 10600
	0    1    1    0   
$EndComp
$Comp
L +5V #PWR025
U 1 1 535582F2
P 13850 10200
F 0 "#PWR025" H 13850 10290 20  0001 C CNN
F 1 "+5V" H 13850 10290 30  0000 C CNN
F 2 "" H 13850 10200 60  0001 C CNN
F 3 "" H 13850 10200 60  0001 C CNN
	1    13850 10200
	1    0    0    -1  
$EndComp
$Comp
L LED D9
U 1 1 535582F8
P 14550 10600
F 0 "D9" H 14550 10700 50  0000 C CNN
F 1 "LED" H 14550 10500 50  0000 C CNN
F 2 "" H 14550 10600 60  0001 C CNN
F 3 "" H 14550 10600 60  0001 C CNN
	1    14550 10600
	1    0    0    -1  
$EndComp
$Comp
L GND #PWR026
U 1 1 535582FE
P 14900 10800
F 0 "#PWR026" H 14900 10800 30  0001 C CNN
F 1 "GND" H 14900 10730 30  0001 C CNN
F 2 "" H 14900 10800 60  0001 C CNN
F 3 "" H 14900 10800 60  0001 C CNN
	1    14900 10800
	1    0    0    -1  
$EndComp
Text GLabel 13050 10400 0    60   Input ~ 0
CMD3
Text GLabel 8550 9200 2    60   Output ~ 0
LED
$Comp
L GND #PWR027
U 1 1 5355867B
P 6850 3950
F 0 "#PWR027" H 6850 3950 30  0001 C CNN
F 1 "GND" H 6850 3880 30  0001 C CNN
F 2 "" H 6850 3950 60  0001 C CNN
F 3 "" H 6850 3950 60  0001 C CNN
	1    6850 3950
	1    0    0    -1  
$EndComp
$Comp
L PWR_FLAG #FLG028
U 1 1 53558681
P 6850 3350
F 0 "#FLG028" H 6850 3445 30  0001 C CNN
F 1 "PWR_FLAG" H 6850 3530 30  0000 C CNN
F 2 "" H 6850 3350 60  0001 C CNN
F 3 "" H 6850 3350 60  0001 C CNN
	1    6850 3350
	1    0    0    -1  
$EndComp
$Comp
L LED D10
U 1 1 536E836C
P 7800 3100
F 0 "D10" H 7800 3200 50  0000 C CNN
F 1 "LED" H 7800 3000 50  0000 C CNN
F 2 "" H 7800 3100 60  0001 C CNN
F 3 "" H 7800 3100 60  0001 C CNN
	1    7800 3100
	0    1    1    0   
$EndComp
$Comp
L R R13
U 1 1 536E8372
P 7800 3700
F 0 "R13" V 7880 3700 50  0000 C CNN
F 1 "4.7KR" V 7800 3700 50  0000 C CNN
F 2 "" H 7800 3700 60  0001 C CNN
F 3 "" H 7800 3700 60  0001 C CNN
	1    7800 3700
	1    0    0    -1  
$EndComp
$Comp
L GND #PWR029
U 1 1 536E8378
P 7800 4050
F 0 "#PWR029" H 7800 4050 30  0001 C CNN
F 1 "GND" H 7800 3980 30  0001 C CNN
F 2 "" H 7800 4050 60  0001 C CNN
F 3 "" H 7800 4050 60  0001 C CNN
	1    7800 4050
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR030
U 1 1 536E8380
P 7800 2900
F 0 "#PWR030" H 7800 2990 20  0001 C CNN
F 1 "+5V" H 7800 2990 30  0000 C CNN
F 2 "" H 7800 2900 60  0001 C CNN
F 3 "" H 7800 2900 60  0001 C CNN
	1    7800 2900
	1    0    0    -1  
$EndComp
$Comp
L CONN_6 P6
U 1 1 54576F4D
P 3650 6200
F 0 "P6" V 3600 6200 60  0000 C CNN
F 1 "CONN_6" V 3700 6200 60  0000 C CNN
F 2 "" H 3650 6200 60  0000 C CNN
F 3 "" H 3650 6200 60  0000 C CNN
	1    3650 6200
	0    -1   -1   0   
$EndComp
Text GLabel 3500 6700 3    60   Output ~ 0
TX
Text GLabel 3600 6950 3    60   Input ~ 0
RX
$Comp
L GND #PWR031
U 1 1 545772BD
P 3900 6950
F 0 "#PWR031" H 3900 6950 30  0001 C CNN
F 1 "GND" H 3900 6880 30  0001 C CNN
F 2 "" H 3900 6950 60  0001 C CNN
F 3 "" H 3900 6950 60  0001 C CNN
	1    3900 6950
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR032
U 1 1 545772D8
P 3700 6650
F 0 "#PWR032" H 3700 6740 20  0001 C CNN
F 1 "+5V" H 3700 6740 30  0000 C CNN
F 2 "" H 3700 6650 60  0001 C CNN
F 3 "" H 3700 6650 60  0001 C CNN
	1    3700 6650
	-1   0    0    1   
$EndComp
$Comp
L C C4
U 1 1 545772E0
P 3400 7350
F 0 "C4" H 3450 7450 50  0000 L CNN
F 1 "C" H 3450 7250 50  0000 L CNN
F 2 "" H 3400 7350 60  0000 C CNN
F 3 "" H 3400 7350 60  0000 C CNN
	1    3400 7350
	1    0    0    -1  
$EndComp
Text GLabel 3400 7950 3    60   BiDi ~ 0
RESET
$Comp
L GND #PWR033
U 1 1 54577716
P 3150 8550
F 0 "#PWR033" H 3150 8550 30  0001 C CNN
F 1 "GND" H 3150 8480 30  0001 C CNN
F 2 "" H 3150 8550 60  0001 C CNN
F 3 "" H 3150 8550 60  0001 C CNN
	1    3150 8550
	1    0    0    -1  
$EndComp
Text GLabel 8150 9100 2    60   Output ~ 0
RES1
Wire Wire Line
	3150 6950 3150 7050
Wire Wire Line
	7100 5050 7200 5050
Wire Notes Line
	5700 5200 7600 5200
Wire Notes Line
	7600 5200 7600 4550
Wire Notes Line
	7600 4550 5700 4550
Wire Notes Line
	5700 4550 5700 5200
Wire Wire Line
	6100 8600 6100 8950
Wire Wire Line
	5850 10200 5850 10050
Wire Wire Line
	13550 5700 13450 5700
Wire Wire Line
	14050 5700 13950 5700
Wire Wire Line
	5850 10800 5850 10700
Wire Wire Line
	7950 5250 7950 5150
Wire Wire Line
	8300 5250 8300 5150
Wire Wire Line
	8650 5250 8650 5150
Wire Wire Line
	9000 5250 9000 5150
Wire Notes Line
	7700 4550 9400 4550
Wire Notes Line
	9400 4550 9400 5400
Wire Notes Line
	9400 5400 7700 5400
Wire Notes Line
	7700 5400 7700 4550
Wire Notes Line
	7350 4100 5650 4100
Wire Notes Line
	5650 4100 5650 2000
Wire Notes Line
	5650 2000 7350 2000
Wire Notes Line
	7350 2000 7350 4100
Wire Notes Line
	5350 8100 9650 8100
Wire Wire Line
	5850 3350 5850 3450
Wire Wire Line
	6300 3450 6300 3300
Wire Wire Line
	6100 3300 6100 3450
Wire Wire Line
	5850 3850 6300 3850
Wire Wire Line
	6100 3850 6100 3950
Connection ~ 6100 3850
Wire Notes Line
	9650 8100 9650 11450
Wire Wire Line
	8150 5150 8150 5200
Wire Wire Line
	8500 5150 8500 5200
Wire Wire Line
	8850 5150 8850 5200
Wire Wire Line
	9200 5150 9200 5200
Wire Notes Line
	5350 8100 5350 11450
Wire Wire Line
	8000 10550 9200 10550
Wire Wire Line
	5850 9500 5850 9650
Wire Wire Line
	6100 11500 6050 11500
Wire Wire Line
	6050 11400 6050 11700
Wire Wire Line
	6100 11400 6050 11400
Connection ~ 6050 11500
Connection ~ 6100 8650
Wire Wire Line
	8000 11200 8500 11200
Wire Wire Line
	8000 11100 8100 11100
Wire Wire Line
	8000 11000 8500 11000
Wire Wire Line
	8000 10450 8500 10450
Wire Wire Line
	8000 10350 8150 10350
Wire Wire Line
	8000 9600 8500 9600
Wire Wire Line
	8000 9500 8100 9500
Wire Wire Line
	8000 9400 8500 9400
Wire Wire Line
	8000 8950 8500 8950
Wire Wire Line
	8000 8850 8100 8850
Wire Wire Line
	6200 4800 6300 4800
Wire Wire Line
	6300 4800 6300 4850
Wire Wire Line
	6200 4950 6300 4950
Wire Wire Line
	6200 5100 6300 5100
Wire Wire Line
	6300 5100 6300 5050
Wire Wire Line
	7000 13450 7200 13450
Wire Wire Line
	6650 13550 7200 13550
Wire Wire Line
	7000 13650 7200 13650
Wire Wire Line
	6650 13750 7200 13750
Wire Wire Line
	7100 14200 7100 14150
Wire Wire Line
	7100 14150 7200 14150
Wire Wire Line
	8000 14150 8100 14150
Wire Wire Line
	8100 14150 8100 14250
Wire Wire Line
	8600 13750 8000 13750
Wire Wire Line
	8000 13650 8200 13650
Wire Wire Line
	8000 13450 8200 13450
Wire Wire Line
	8000 13550 8600 13550
Wire Bus Line
	5350 11400 5350 12250
Wire Bus Line
	5350 12250 9650 12250
Wire Bus Line
	9650 12250 9650 11450
Wire Notes Line
	5350 12600 5350 14750
Wire Notes Line
	5350 14750 9650 14750
Wire Notes Line
	9650 14750 9650 12600
Wire Notes Line
	9650 12600 5350 12600
Connection ~ 6100 3450
Wire Wire Line
	5850 3450 6300 3450
Wire Wire Line
	7200 5050 7200 5100
Wire Wire Line
	7100 4850 7200 4850
Wire Wire Line
	7200 4850 7200 4800
Wire Wire Line
	8000 9950 8100 9950
Wire Wire Line
	8000 10800 8500 10800
Wire Wire Line
	8000 10900 8100 10900
Wire Wire Line
	8000 11500 8100 11500
Wire Wire Line
	8000 13950 8600 13950
Wire Wire Line
	6650 13950 7200 13950
Wire Wire Line
	13550 6050 13450 6050
Wire Wire Line
	14050 6050 13950 6050
Wire Wire Line
	13550 6400 13450 6400
Wire Wire Line
	14050 6400 13950 6400
Wire Wire Line
	13550 6750 13450 6750
Wire Wire Line
	14050 6750 13950 6750
Wire Wire Line
	14750 8200 14900 8200
Wire Wire Line
	14900 8200 14900 8400
Wire Wire Line
	14750 9000 14900 9000
Wire Wire Line
	14900 9000 14900 9200
Wire Wire Line
	14750 9800 14900 9800
Wire Wire Line
	14900 9800 14900 10000
Wire Wire Line
	14750 10600 14900 10600
Wire Wire Line
	14900 10600 14900 10800
Wire Wire Line
	8000 9200 8550 9200
Wire Wire Line
	6850 3350 6850 3950
Wire Wire Line
	7800 3450 7800 3300
Wire Wire Line
	7800 4050 7800 3950
Wire Wire Line
	8000 9700 8100 9700
Wire Wire Line
	8000 9800 8450 9800
Wire Wire Line
	8000 11300 8100 11300
Wire Wire Line
	8000 11400 8400 11400
Wire Wire Line
	8000 10050 8500 10050
Wire Wire Line
	8000 10150 8100 10150
Wire Wire Line
	8000 10250 8500 10250
Wire Wire Line
	8000 10650 8500 10650
Wire Wire Line
	8000 13850 8200 13850
Wire Wire Line
	8000 14050 8200 14050
Wire Wire Line
	3500 6550 3500 6700
Wire Wire Line
	3400 6550 3400 7150
Wire Wire Line
	3800 6550 3800 6650
Wire Wire Line
	3800 6650 3900 6650
Wire Wire Line
	3900 6550 3900 6950
Connection ~ 3900 6650
Wire Wire Line
	3700 6550 3700 6650
Wire Wire Line
	3600 6550 3600 6950
Connection ~ 3400 7650
Connection ~ 3150 7650
Wire Wire Line
	3150 7550 3150 7850
Wire Wire Line
	3150 8150 3150 8550
Wire Wire Line
	3400 7550 3400 7950
Wire Wire Line
	3150 7650 3400 7650
Wire Wire Line
	8000 9100 8150 9100
$Comp
L SKQGAKE010 SW1
U 1 1 545777AC
P 3125 8000
F 0 "SW1" H 3140 7880 60  0000 C CNN
F 1 "SKQGAKE010" H 3140 8110 60  0000 C CNN
F 2 "" H 3125 8000 60  0000 C CNN
F 3 "" H 3125 8000 60  0000 C CNN
	1    3125 8000
	0    -1   -1   0   
$EndComp
Wire Wire Line
	3150 7850 3100 7850
Wire Wire Line
	3100 8150 3150 8150
$EndSCHEMATC
