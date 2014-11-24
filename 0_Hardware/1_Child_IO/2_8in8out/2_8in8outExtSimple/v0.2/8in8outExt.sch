EESchema Schematic File Version 2
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
LIBS:ULN2803
LIBS:ALPS_SWITCHS
LIBS:8in8outExt-cache
EELAYER 27 0
EELAYER END
$Descr A4 11693 8268
encoding utf-8
Sheet 1 1
Title "8in 8relays I2C board Extension"
Date "24 nov 2014"
Rev ""
Comp "XPLDUINO project"
Comment1 "GNU GPL v2"
Comment2 "Home automation Open Source "
Comment3 ""
Comment4 ""
$EndDescr
$Comp
L CONN_10X2 P1
U 1 1 5172743B
P 2650 1550
F 0 "P1" H 2650 1000 60  0000 C CNN
F 1 "CONN_10X2" V 2650 1650 50  0000 C CNN
F 2 "" H 2650 1550 60  0000 C CNN
F 3 "" H 2650 1550 60  0000 C CNN
	1    2650 1550
	1    0    0    -1  
$EndComp
Text GLabel 2050 1100 0    60   Output ~ 0
CMD0
Text GLabel 1600 1200 0    60   Output ~ 0
CMD1
Text GLabel 2050 1300 0    60   Output ~ 0
CMD2
Text GLabel 1600 1400 0    60   Output ~ 0
CMD3
Text GLabel 2050 1500 0    60   Output ~ 0
CMD4
Text GLabel 1600 1600 0    60   Output ~ 0
CMD5
Text GLabel 2050 1700 0    60   Output ~ 0
CMD6
Text GLabel 1600 1800 0    60   Output ~ 0
CMD7
Text GLabel 2050 1900 0    60   Input ~ 0
RESET
$Comp
L GND #PWR01
U 1 1 51727501
P 2200 2100
F 0 "#PWR01" H 2200 2100 30  0001 C CNN
F 1 "GND" H 2200 2170 30  0001 C CNN
F 2 "" H 2200 2100 60  0000 C CNN
F 3 "" H 2200 2100 60  0000 C CNN
	1    2200 2100
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR02
U 1 1 51727516
P 3150 2050
F 0 "#PWR02" H 3150 1960 20  0001 C CNN
F 1 "+5V" H 3150 1960 30  0000 C CNN
F 2 "" H 3150 2050 60  0000 C CNN
F 3 "" H 3150 2050 60  0000 C CNN
	1    3150 2050
	-1   0    0    1   
$EndComp
Text GLabel 3250 1100 2    60   Output ~ 0
OUT0
Text GLabel 3650 1200 2    60   Output ~ 0
OUT1
Text GLabel 3250 1300 2    60   Output ~ 0
OUT2
Text GLabel 3650 1400 2    60   Output ~ 0
OUT3
Text GLabel 3250 1500 2    60   Output ~ 0
OUT4
Text GLabel 3650 1600 2    60   Output ~ 0
OUT5
Text GLabel 3250 1700 2    60   Output ~ 0
OUT6
Text GLabel 3650 1800 2    60   Output ~ 0
OUT7
Text GLabel 3250 1900 2    60   Output ~ 0
LED
Text GLabel 5200 1000 0    60   Input ~ 0
CMD0
Text GLabel 5200 1650 0    60   Input ~ 0
CMD1
Text GLabel 5200 2300 0    60   Input ~ 0
CMD2
Text GLabel 5200 2950 0    60   Input ~ 0
CMD3
Text GLabel 5200 3650 0    60   Input ~ 0
CMD4
Text GLabel 5200 4300 0    60   Input ~ 0
CMD5
Text GLabel 5200 4950 0    60   Input ~ 0
CMD6
Text GLabel 5200 5600 0    60   Input ~ 0
CMD7
$Comp
L LED D1
U 1 1 51727982
P 6900 1200
F 0 "D1" H 6900 1100 50  0000 C CNN
F 1 "LED" H 6900 1300 50  0000 C CNN
F 2 "~" H 6900 1200 60  0000 C CNN
F 3 "~" H 6900 1200 60  0000 C CNN
	1    6900 1200
	1    0    0    -1  
$EndComp
$Comp
L R R17
U 1 1 517279C0
P 6450 1200
F 0 "R17" V 6530 1200 40  0000 C CNN
F 1 "R" V 6457 1199 40  0000 C CNN
F 2 "~" V 6380 1200 30  0000 C CNN
F 3 "~" H 6450 1200 30  0000 C CNN
	1    6450 1200
	0    1    1    0   
$EndComp
$Comp
L GND #PWR03
U 1 1 51727D2D
P 7150 1350
F 0 "#PWR03" H 7150 1350 30  0001 C CNN
F 1 "GND" H 7150 1420 30  0001 C CNN
F 2 "" H 7150 1350 60  0000 C CNN
F 3 "" H 7150 1350 60  0000 C CNN
	1    7150 1350
	1    0    0    -1  
$EndComp
Text GLabel 7850 1000 0    60   Input ~ 0
OUT0
Text GLabel 7850 1650 0    60   Input ~ 0
OUT1
Text GLabel 7850 2300 0    60   Input ~ 0
OUT2
Text GLabel 7850 2950 0    60   Input ~ 0
OUT3
Text GLabel 7850 3650 0    60   Input ~ 0
OUT4
Text GLabel 7850 4300 0    60   Input ~ 0
OUT5
Text GLabel 7850 4950 0    60   Input ~ 0
OUT6
Text GLabel 7850 5600 0    60   Input ~ 0
OUT7
$Comp
L LED D17
U 1 1 51739906
P 2100 6700
F 0 "D17" H 2100 6600 50  0000 C CNN
F 1 "LED" H 2100 6800 50  0000 C CNN
F 2 "~" H 2100 6700 60  0000 C CNN
F 3 "~" H 2100 6700 60  0000 C CNN
	1    2100 6700
	0    1    1    0   
$EndComp
$Comp
L R R41
U 1 1 5173990C
P 2100 7150
F 0 "R41" V 2180 7150 40  0000 C CNN
F 1 "R" V 2107 7149 40  0000 C CNN
F 2 "~" V 2030 7150 30  0000 C CNN
F 3 "~" H 2100 7150 30  0000 C CNN
	1    2100 7150
	-1   0    0    1   
$EndComp
$Comp
L LED D18
U 1 1 51739912
P 2550 7200
F 0 "D18" H 2550 7100 50  0000 C CNN
F 1 "LED" H 2550 7300 50  0000 C CNN
F 2 "~" H 2550 7200 60  0000 C CNN
F 3 "~" H 2550 7200 60  0000 C CNN
	1    2550 7200
	0    1    1    0   
$EndComp
$Comp
L R R42
U 1 1 51739918
P 2550 6750
F 0 "R42" V 2630 6750 40  0000 C CNN
F 1 "R" V 2557 6749 40  0000 C CNN
F 2 "~" V 2480 6750 30  0000 C CNN
F 3 "~" H 2550 6750 30  0000 C CNN
	1    2550 6750
	-1   0    0    1   
$EndComp
$Comp
L GND #PWR04
U 1 1 5173991E
P 2100 7400
F 0 "#PWR04" H 2100 7400 30  0001 C CNN
F 1 "GND" H 2100 7470 30  0001 C CNN
F 2 "" H 2100 7400 60  0000 C CNN
F 3 "" H 2100 7400 60  0000 C CNN
	1    2100 7400
	1    0    0    -1  
$EndComp
$Comp
L GND #PWR05
U 1 1 51739924
P 2550 7400
F 0 "#PWR05" H 2550 7400 30  0001 C CNN
F 1 "GND" H 2550 7470 30  0001 C CNN
F 2 "" H 2550 7400 60  0000 C CNN
F 3 "" H 2550 7400 60  0000 C CNN
	1    2550 7400
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR06
U 1 1 5173992A
P 2100 6500
F 0 "#PWR06" H 2100 6410 20  0001 C CNN
F 1 "+5V" H 2100 6410 30  0000 C CNN
F 2 "" H 2100 6500 60  0000 C CNN
F 3 "" H 2100 6500 60  0000 C CNN
	1    2100 6500
	1    0    0    -1  
$EndComp
Text GLabel 1550 5950 0    60   Output ~ 0
LED
$Comp
L +5V #PWR07
U 1 1 51766852
P 6200 800
F 0 "#PWR07" H 6200 710 20  0001 C CNN
F 1 "+5V" H 6200 710 30  0000 C CNN
F 2 "" H 6200 800 60  0000 C CNN
F 3 "" H 6200 800 60  0000 C CNN
	1    6200 800 
	1    0    0    -1  
$EndComp
Text GLabel 5200 6575 2    60   Output ~ 0
RESET
$Comp
L GND #PWR08
U 1 1 517986C9
P 4100 6750
F 0 "#PWR08" H 4100 6750 30  0001 C CNN
F 1 "GND" H 4100 6820 30  0001 C CNN
F 2 "" H 4100 6750 60  0000 C CNN
F 3 "" H 4100 6750 60  0000 C CNN
	1    4100 6750
	1    0    0    -1  
$EndComp
$Comp
L PNP Q1
U 1 1 522CC7FB
P 6100 1000
F 0 "Q1" H 6100 850 60  0000 R CNN
F 1 "PNP" H 6100 1150 60  0000 R CNN
F 2 "" H 6100 1000 60  0000 C CNN
F 3 "" H 6100 1000 60  0000 C CNN
	1    6100 1000
	1    0    0    1   
$EndComp
$Comp
L R R9
U 1 1 522CC83F
P 5650 1000
F 0 "R9" V 5730 1000 40  0000 C CNN
F 1 "R" V 5657 1001 40  0000 C CNN
F 2 "" V 5580 1000 30  0000 C CNN
F 3 "" H 5650 1000 30  0000 C CNN
	1    5650 1000
	0    -1   -1   0   
$EndComp
$Comp
L LED D2
U 1 1 522CC8A4
P 6900 1850
F 0 "D2" H 6900 1750 50  0000 C CNN
F 1 "LED" H 6900 1950 50  0000 C CNN
F 2 "~" H 6900 1850 60  0000 C CNN
F 3 "~" H 6900 1850 60  0000 C CNN
	1    6900 1850
	1    0    0    -1  
$EndComp
$Comp
L R R18
U 1 1 522CC8AA
P 6450 1850
F 0 "R18" V 6530 1850 40  0000 C CNN
F 1 "R" V 6457 1849 40  0000 C CNN
F 2 "~" V 6380 1850 30  0000 C CNN
F 3 "~" H 6450 1850 30  0000 C CNN
	1    6450 1850
	0    1    1    0   
$EndComp
$Comp
L GND #PWR09
U 1 1 522CC8B0
P 7150 2000
F 0 "#PWR09" H 7150 2000 30  0001 C CNN
F 1 "GND" H 7150 2070 30  0001 C CNN
F 2 "" H 7150 2000 60  0000 C CNN
F 3 "" H 7150 2000 60  0000 C CNN
	1    7150 2000
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR010
U 1 1 522CC8B6
P 6200 1450
F 0 "#PWR010" H 6200 1360 20  0001 C CNN
F 1 "+5V" H 6200 1360 30  0000 C CNN
F 2 "" H 6200 1450 60  0000 C CNN
F 3 "" H 6200 1450 60  0000 C CNN
	1    6200 1450
	1    0    0    -1  
$EndComp
$Comp
L PNP Q2
U 1 1 522CC8BC
P 6100 1650
F 0 "Q2" H 6100 1500 60  0000 R CNN
F 1 "PNP" H 6100 1800 60  0000 R CNN
F 2 "" H 6100 1650 60  0000 C CNN
F 3 "" H 6100 1650 60  0000 C CNN
	1    6100 1650
	1    0    0    1   
$EndComp
$Comp
L R R10
U 1 1 522CC8C2
P 5650 1650
F 0 "R10" V 5730 1650 40  0000 C CNN
F 1 "R" V 5657 1651 40  0000 C CNN
F 2 "" V 5580 1650 30  0000 C CNN
F 3 "" H 5650 1650 30  0000 C CNN
	1    5650 1650
	0    -1   -1   0   
$EndComp
$Comp
L LED D3
U 1 1 522CC8CB
P 6900 2500
F 0 "D3" H 6900 2400 50  0000 C CNN
F 1 "LED" H 6900 2600 50  0000 C CNN
F 2 "~" H 6900 2500 60  0000 C CNN
F 3 "~" H 6900 2500 60  0000 C CNN
	1    6900 2500
	1    0    0    -1  
$EndComp
$Comp
L R R19
U 1 1 522CC8D1
P 6450 2500
F 0 "R19" V 6530 2500 40  0000 C CNN
F 1 "R" V 6457 2499 40  0000 C CNN
F 2 "~" V 6380 2500 30  0000 C CNN
F 3 "~" H 6450 2500 30  0000 C CNN
	1    6450 2500
	0    1    1    0   
$EndComp
$Comp
L GND #PWR011
U 1 1 522CC8D7
P 7150 2650
F 0 "#PWR011" H 7150 2650 30  0001 C CNN
F 1 "GND" H 7150 2720 30  0001 C CNN
F 2 "" H 7150 2650 60  0000 C CNN
F 3 "" H 7150 2650 60  0000 C CNN
	1    7150 2650
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR012
U 1 1 522CC8DD
P 6200 2100
F 0 "#PWR012" H 6200 2010 20  0001 C CNN
F 1 "+5V" H 6200 2010 30  0000 C CNN
F 2 "" H 6200 2100 60  0000 C CNN
F 3 "" H 6200 2100 60  0000 C CNN
	1    6200 2100
	1    0    0    -1  
$EndComp
$Comp
L PNP Q3
U 1 1 522CC8E3
P 6100 2300
F 0 "Q3" H 6100 2150 60  0000 R CNN
F 1 "PNP" H 6100 2450 60  0000 R CNN
F 2 "" H 6100 2300 60  0000 C CNN
F 3 "" H 6100 2300 60  0000 C CNN
	1    6100 2300
	1    0    0    1   
$EndComp
$Comp
L R R11
U 1 1 522CC8E9
P 5650 2300
F 0 "R11" V 5730 2300 40  0000 C CNN
F 1 "R" V 5657 2301 40  0000 C CNN
F 2 "" V 5580 2300 30  0000 C CNN
F 3 "" H 5650 2300 30  0000 C CNN
	1    5650 2300
	0    -1   -1   0   
$EndComp
$Comp
L LED D4
U 1 1 522CC8FC
P 6900 3150
F 0 "D4" H 6900 3050 50  0000 C CNN
F 1 "LED" H 6900 3250 50  0000 C CNN
F 2 "~" H 6900 3150 60  0000 C CNN
F 3 "~" H 6900 3150 60  0000 C CNN
	1    6900 3150
	1    0    0    -1  
$EndComp
$Comp
L R R20
U 1 1 522CC902
P 6450 3150
F 0 "R20" V 6530 3150 40  0000 C CNN
F 1 "R" V 6457 3149 40  0000 C CNN
F 2 "~" V 6380 3150 30  0000 C CNN
F 3 "~" H 6450 3150 30  0000 C CNN
	1    6450 3150
	0    1    1    0   
$EndComp
$Comp
L GND #PWR013
U 1 1 522CC908
P 7150 3300
F 0 "#PWR013" H 7150 3300 30  0001 C CNN
F 1 "GND" H 7150 3370 30  0001 C CNN
F 2 "" H 7150 3300 60  0000 C CNN
F 3 "" H 7150 3300 60  0000 C CNN
	1    7150 3300
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR014
U 1 1 522CC90E
P 6200 2750
F 0 "#PWR014" H 6200 2660 20  0001 C CNN
F 1 "+5V" H 6200 2660 30  0000 C CNN
F 2 "" H 6200 2750 60  0000 C CNN
F 3 "" H 6200 2750 60  0000 C CNN
	1    6200 2750
	1    0    0    -1  
$EndComp
$Comp
L PNP Q4
U 1 1 522CC914
P 6100 2950
F 0 "Q4" H 6100 2800 60  0000 R CNN
F 1 "PNP" H 6100 3100 60  0000 R CNN
F 2 "" H 6100 2950 60  0000 C CNN
F 3 "" H 6100 2950 60  0000 C CNN
	1    6100 2950
	1    0    0    1   
$EndComp
$Comp
L R R12
U 1 1 522CC91A
P 5650 2950
F 0 "R12" V 5730 2950 40  0000 C CNN
F 1 "R" V 5657 2951 40  0000 C CNN
F 2 "" V 5580 2950 30  0000 C CNN
F 3 "" H 5650 2950 30  0000 C CNN
	1    5650 2950
	0    -1   -1   0   
$EndComp
$Comp
L LED D5
U 1 1 522CC924
P 6900 3850
F 0 "D5" H 6900 3750 50  0000 C CNN
F 1 "LED" H 6900 3950 50  0000 C CNN
F 2 "~" H 6900 3850 60  0000 C CNN
F 3 "~" H 6900 3850 60  0000 C CNN
	1    6900 3850
	1    0    0    -1  
$EndComp
$Comp
L R R21
U 1 1 522CC92A
P 6450 3850
F 0 "R21" V 6530 3850 40  0000 C CNN
F 1 "R" V 6457 3849 40  0000 C CNN
F 2 "~" V 6380 3850 30  0000 C CNN
F 3 "~" H 6450 3850 30  0000 C CNN
	1    6450 3850
	0    1    1    0   
$EndComp
$Comp
L GND #PWR015
U 1 1 522CC930
P 7150 4000
F 0 "#PWR015" H 7150 4000 30  0001 C CNN
F 1 "GND" H 7150 4070 30  0001 C CNN
F 2 "" H 7150 4000 60  0000 C CNN
F 3 "" H 7150 4000 60  0000 C CNN
	1    7150 4000
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR016
U 1 1 522CC936
P 6200 3450
F 0 "#PWR016" H 6200 3360 20  0001 C CNN
F 1 "+5V" H 6200 3360 30  0000 C CNN
F 2 "" H 6200 3450 60  0000 C CNN
F 3 "" H 6200 3450 60  0000 C CNN
	1    6200 3450
	1    0    0    -1  
$EndComp
$Comp
L PNP Q5
U 1 1 522CC93C
P 6100 3650
F 0 "Q5" H 6100 3500 60  0000 R CNN
F 1 "PNP" H 6100 3800 60  0000 R CNN
F 2 "" H 6100 3650 60  0000 C CNN
F 3 "" H 6100 3650 60  0000 C CNN
	1    6100 3650
	1    0    0    1   
$EndComp
$Comp
L R R13
U 1 1 522CC942
P 5650 3650
F 0 "R13" V 5730 3650 40  0000 C CNN
F 1 "R" V 5657 3651 40  0000 C CNN
F 2 "" V 5580 3650 30  0000 C CNN
F 3 "" H 5650 3650 30  0000 C CNN
	1    5650 3650
	0    -1   -1   0   
$EndComp
$Comp
L LED D6
U 1 1 522CC94B
P 6900 4500
F 0 "D6" H 6900 4400 50  0000 C CNN
F 1 "LED" H 6900 4600 50  0000 C CNN
F 2 "~" H 6900 4500 60  0000 C CNN
F 3 "~" H 6900 4500 60  0000 C CNN
	1    6900 4500
	1    0    0    -1  
$EndComp
$Comp
L R R22
U 1 1 522CC951
P 6450 4500
F 0 "R22" V 6530 4500 40  0000 C CNN
F 1 "R" V 6457 4499 40  0000 C CNN
F 2 "~" V 6380 4500 30  0000 C CNN
F 3 "~" H 6450 4500 30  0000 C CNN
	1    6450 4500
	0    1    1    0   
$EndComp
$Comp
L GND #PWR017
U 1 1 522CC957
P 7150 4650
F 0 "#PWR017" H 7150 4650 30  0001 C CNN
F 1 "GND" H 7150 4720 30  0001 C CNN
F 2 "" H 7150 4650 60  0000 C CNN
F 3 "" H 7150 4650 60  0000 C CNN
	1    7150 4650
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR018
U 1 1 522CC95D
P 6200 4100
F 0 "#PWR018" H 6200 4010 20  0001 C CNN
F 1 "+5V" H 6200 4010 30  0000 C CNN
F 2 "" H 6200 4100 60  0000 C CNN
F 3 "" H 6200 4100 60  0000 C CNN
	1    6200 4100
	1    0    0    -1  
$EndComp
$Comp
L PNP Q6
U 1 1 522CC963
P 6100 4300
F 0 "Q6" H 6100 4150 60  0000 R CNN
F 1 "PNP" H 6100 4450 60  0000 R CNN
F 2 "" H 6100 4300 60  0000 C CNN
F 3 "" H 6100 4300 60  0000 C CNN
	1    6100 4300
	1    0    0    1   
$EndComp
$Comp
L R R14
U 1 1 522CC969
P 5650 4300
F 0 "R14" V 5730 4300 40  0000 C CNN
F 1 "R" V 5657 4301 40  0000 C CNN
F 2 "" V 5580 4300 30  0000 C CNN
F 3 "" H 5650 4300 30  0000 C CNN
	1    5650 4300
	0    -1   -1   0   
$EndComp
$Comp
L LED D7
U 1 1 522CC972
P 6900 5150
F 0 "D7" H 6900 5050 50  0000 C CNN
F 1 "LED" H 6900 5250 50  0000 C CNN
F 2 "~" H 6900 5150 60  0000 C CNN
F 3 "~" H 6900 5150 60  0000 C CNN
	1    6900 5150
	1    0    0    -1  
$EndComp
$Comp
L R R23
U 1 1 522CC978
P 6450 5150
F 0 "R23" V 6530 5150 40  0000 C CNN
F 1 "R" V 6457 5149 40  0000 C CNN
F 2 "~" V 6380 5150 30  0000 C CNN
F 3 "~" H 6450 5150 30  0000 C CNN
	1    6450 5150
	0    1    1    0   
$EndComp
$Comp
L GND #PWR019
U 1 1 522CC97E
P 7150 5300
F 0 "#PWR019" H 7150 5300 30  0001 C CNN
F 1 "GND" H 7150 5370 30  0001 C CNN
F 2 "" H 7150 5300 60  0000 C CNN
F 3 "" H 7150 5300 60  0000 C CNN
	1    7150 5300
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR020
U 1 1 522CC984
P 6200 4750
F 0 "#PWR020" H 6200 4660 20  0001 C CNN
F 1 "+5V" H 6200 4660 30  0000 C CNN
F 2 "" H 6200 4750 60  0000 C CNN
F 3 "" H 6200 4750 60  0000 C CNN
	1    6200 4750
	1    0    0    -1  
$EndComp
$Comp
L PNP Q7
U 1 1 522CC98A
P 6100 4950
F 0 "Q7" H 6100 4800 60  0000 R CNN
F 1 "PNP" H 6100 5100 60  0000 R CNN
F 2 "" H 6100 4950 60  0000 C CNN
F 3 "" H 6100 4950 60  0000 C CNN
	1    6100 4950
	1    0    0    1   
$EndComp
$Comp
L R R15
U 1 1 522CC990
P 5650 4950
F 0 "R15" V 5730 4950 40  0000 C CNN
F 1 "R" V 5657 4951 40  0000 C CNN
F 2 "" V 5580 4950 30  0000 C CNN
F 3 "" H 5650 4950 30  0000 C CNN
	1    5650 4950
	0    -1   -1   0   
$EndComp
$Comp
L LED D8
U 1 1 522CC999
P 6900 5800
F 0 "D8" H 6900 5700 50  0000 C CNN
F 1 "LED" H 6900 5900 50  0000 C CNN
F 2 "~" H 6900 5800 60  0000 C CNN
F 3 "~" H 6900 5800 60  0000 C CNN
	1    6900 5800
	1    0    0    -1  
$EndComp
$Comp
L R R24
U 1 1 522CC99F
P 6450 5800
F 0 "R24" V 6530 5800 40  0000 C CNN
F 1 "R" V 6457 5799 40  0000 C CNN
F 2 "~" V 6380 5800 30  0000 C CNN
F 3 "~" H 6450 5800 30  0000 C CNN
	1    6450 5800
	0    1    1    0   
$EndComp
$Comp
L GND #PWR021
U 1 1 522CC9A5
P 7150 5950
F 0 "#PWR021" H 7150 5950 30  0001 C CNN
F 1 "GND" H 7150 6020 30  0001 C CNN
F 2 "" H 7150 5950 60  0000 C CNN
F 3 "" H 7150 5950 60  0000 C CNN
	1    7150 5950
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR022
U 1 1 522CC9AB
P 6200 5400
F 0 "#PWR022" H 6200 5310 20  0001 C CNN
F 1 "+5V" H 6200 5310 30  0000 C CNN
F 2 "" H 6200 5400 60  0000 C CNN
F 3 "" H 6200 5400 60  0000 C CNN
	1    6200 5400
	1    0    0    -1  
$EndComp
$Comp
L PNP Q8
U 1 1 522CC9B1
P 6100 5600
F 0 "Q8" H 6100 5450 60  0000 R CNN
F 1 "PNP" H 6100 5750 60  0000 R CNN
F 2 "" H 6100 5600 60  0000 C CNN
F 3 "" H 6100 5600 60  0000 C CNN
	1    6100 5600
	1    0    0    1   
$EndComp
$Comp
L R R16
U 1 1 522CC9B7
P 5650 5600
F 0 "R16" V 5730 5600 40  0000 C CNN
F 1 "R" V 5657 5601 40  0000 C CNN
F 2 "" V 5580 5600 30  0000 C CNN
F 3 "" H 5650 5600 30  0000 C CNN
	1    5650 5600
	0    -1   -1   0   
$EndComp
$Comp
L LED D9
U 1 1 522CC9D8
P 9550 1200
F 0 "D9" H 9550 1100 50  0000 C CNN
F 1 "LED" H 9550 1300 50  0000 C CNN
F 2 "~" H 9550 1200 60  0000 C CNN
F 3 "~" H 9550 1200 60  0000 C CNN
	1    9550 1200
	1    0    0    -1  
$EndComp
$Comp
L R R33
U 1 1 522CC9DE
P 9100 1200
F 0 "R33" V 9180 1200 40  0000 C CNN
F 1 "R" V 9107 1199 40  0000 C CNN
F 2 "~" V 9030 1200 30  0000 C CNN
F 3 "~" H 9100 1200 30  0000 C CNN
	1    9100 1200
	0    1    1    0   
$EndComp
$Comp
L GND #PWR023
U 1 1 522CC9E4
P 9800 1350
F 0 "#PWR023" H 9800 1350 30  0001 C CNN
F 1 "GND" H 9800 1420 30  0001 C CNN
F 2 "" H 9800 1350 60  0000 C CNN
F 3 "" H 9800 1350 60  0000 C CNN
	1    9800 1350
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR024
U 1 1 522CC9EA
P 8850 800
F 0 "#PWR024" H 8850 710 20  0001 C CNN
F 1 "+5V" H 8850 710 30  0000 C CNN
F 2 "" H 8850 800 60  0000 C CNN
F 3 "" H 8850 800 60  0000 C CNN
	1    8850 800 
	1    0    0    -1  
$EndComp
$Comp
L R R25
U 1 1 522CC9F6
P 8300 1000
F 0 "R25" V 8380 1000 40  0000 C CNN
F 1 "R" V 8307 1001 40  0000 C CNN
F 2 "" V 8230 1000 30  0000 C CNN
F 3 "" H 8300 1000 30  0000 C CNN
	1    8300 1000
	0    -1   -1   0   
$EndComp
$Comp
L NPN Q9
U 1 1 522CCA12
P 8750 1000
F 0 "Q9" H 8750 850 50  0000 R CNN
F 1 "NPN" H 8750 1150 50  0000 R CNN
F 2 "" H 8750 1000 60  0000 C CNN
F 3 "" H 8750 1000 60  0000 C CNN
	1    8750 1000
	1    0    0    -1  
$EndComp
$Comp
L LED D10
U 1 1 522CCA20
P 9550 1850
F 0 "D10" H 9550 1750 50  0000 C CNN
F 1 "LED" H 9550 1950 50  0000 C CNN
F 2 "~" H 9550 1850 60  0000 C CNN
F 3 "~" H 9550 1850 60  0000 C CNN
	1    9550 1850
	1    0    0    -1  
$EndComp
$Comp
L R R34
U 1 1 522CCA26
P 9100 1850
F 0 "R34" V 9180 1850 40  0000 C CNN
F 1 "R" V 9107 1849 40  0000 C CNN
F 2 "~" V 9030 1850 30  0000 C CNN
F 3 "~" H 9100 1850 30  0000 C CNN
	1    9100 1850
	0    1    1    0   
$EndComp
$Comp
L GND #PWR025
U 1 1 522CCA2C
P 9800 2000
F 0 "#PWR025" H 9800 2000 30  0001 C CNN
F 1 "GND" H 9800 2070 30  0001 C CNN
F 2 "" H 9800 2000 60  0000 C CNN
F 3 "" H 9800 2000 60  0000 C CNN
	1    9800 2000
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR026
U 1 1 522CCA32
P 8850 1450
F 0 "#PWR026" H 8850 1360 20  0001 C CNN
F 1 "+5V" H 8850 1360 30  0000 C CNN
F 2 "" H 8850 1450 60  0000 C CNN
F 3 "" H 8850 1450 60  0000 C CNN
	1    8850 1450
	1    0    0    -1  
$EndComp
$Comp
L R R26
U 1 1 522CCA38
P 8300 1650
F 0 "R26" V 8380 1650 40  0000 C CNN
F 1 "R" V 8307 1651 40  0000 C CNN
F 2 "" V 8230 1650 30  0000 C CNN
F 3 "" H 8300 1650 30  0000 C CNN
	1    8300 1650
	0    -1   -1   0   
$EndComp
$Comp
L NPN Q10
U 1 1 522CCA40
P 8750 1650
F 0 "Q10" H 8750 1500 50  0000 R CNN
F 1 "NPN" H 8750 1800 50  0000 R CNN
F 2 "" H 8750 1650 60  0000 C CNN
F 3 "" H 8750 1650 60  0000 C CNN
	1    8750 1650
	1    0    0    -1  
$EndComp
$Comp
L LED D11
U 1 1 522CCA47
P 9550 2500
F 0 "D11" H 9550 2400 50  0000 C CNN
F 1 "LED" H 9550 2600 50  0000 C CNN
F 2 "~" H 9550 2500 60  0000 C CNN
F 3 "~" H 9550 2500 60  0000 C CNN
	1    9550 2500
	1    0    0    -1  
$EndComp
$Comp
L R R35
U 1 1 522CCA4D
P 9100 2500
F 0 "R35" V 9180 2500 40  0000 C CNN
F 1 "R" V 9107 2499 40  0000 C CNN
F 2 "~" V 9030 2500 30  0000 C CNN
F 3 "~" H 9100 2500 30  0000 C CNN
	1    9100 2500
	0    1    1    0   
$EndComp
$Comp
L GND #PWR027
U 1 1 522CCA53
P 9800 2650
F 0 "#PWR027" H 9800 2650 30  0001 C CNN
F 1 "GND" H 9800 2720 30  0001 C CNN
F 2 "" H 9800 2650 60  0000 C CNN
F 3 "" H 9800 2650 60  0000 C CNN
	1    9800 2650
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR028
U 1 1 522CCA59
P 8850 2100
F 0 "#PWR028" H 8850 2010 20  0001 C CNN
F 1 "+5V" H 8850 2010 30  0000 C CNN
F 2 "" H 8850 2100 60  0000 C CNN
F 3 "" H 8850 2100 60  0000 C CNN
	1    8850 2100
	1    0    0    -1  
$EndComp
$Comp
L R R27
U 1 1 522CCA5F
P 8300 2300
F 0 "R27" V 8380 2300 40  0000 C CNN
F 1 "R" V 8307 2301 40  0000 C CNN
F 2 "" V 8230 2300 30  0000 C CNN
F 3 "" H 8300 2300 30  0000 C CNN
	1    8300 2300
	0    -1   -1   0   
$EndComp
$Comp
L NPN Q11
U 1 1 522CCA67
P 8750 2300
F 0 "Q11" H 8750 2150 50  0000 R CNN
F 1 "NPN" H 8750 2450 50  0000 R CNN
F 2 "" H 8750 2300 60  0000 C CNN
F 3 "" H 8750 2300 60  0000 C CNN
	1    8750 2300
	1    0    0    -1  
$EndComp
$Comp
L LED D12
U 1 1 522CCA6E
P 9550 3150
F 0 "D12" H 9550 3050 50  0000 C CNN
F 1 "LED" H 9550 3250 50  0000 C CNN
F 2 "~" H 9550 3150 60  0000 C CNN
F 3 "~" H 9550 3150 60  0000 C CNN
	1    9550 3150
	1    0    0    -1  
$EndComp
$Comp
L R R36
U 1 1 522CCA74
P 9100 3150
F 0 "R36" V 9180 3150 40  0000 C CNN
F 1 "R" V 9107 3149 40  0000 C CNN
F 2 "~" V 9030 3150 30  0000 C CNN
F 3 "~" H 9100 3150 30  0000 C CNN
	1    9100 3150
	0    1    1    0   
$EndComp
$Comp
L GND #PWR029
U 1 1 522CCA7A
P 9800 3300
F 0 "#PWR029" H 9800 3300 30  0001 C CNN
F 1 "GND" H 9800 3370 30  0001 C CNN
F 2 "" H 9800 3300 60  0000 C CNN
F 3 "" H 9800 3300 60  0000 C CNN
	1    9800 3300
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR030
U 1 1 522CCA80
P 8850 2750
F 0 "#PWR030" H 8850 2660 20  0001 C CNN
F 1 "+5V" H 8850 2660 30  0000 C CNN
F 2 "" H 8850 2750 60  0000 C CNN
F 3 "" H 8850 2750 60  0000 C CNN
	1    8850 2750
	1    0    0    -1  
$EndComp
$Comp
L R R28
U 1 1 522CCA86
P 8300 2950
F 0 "R28" V 8380 2950 40  0000 C CNN
F 1 "R" V 8307 2951 40  0000 C CNN
F 2 "" V 8230 2950 30  0000 C CNN
F 3 "" H 8300 2950 30  0000 C CNN
	1    8300 2950
	0    -1   -1   0   
$EndComp
$Comp
L NPN Q12
U 1 1 522CCA8E
P 8750 2950
F 0 "Q12" H 8750 2800 50  0000 R CNN
F 1 "NPN" H 8750 3100 50  0000 R CNN
F 2 "" H 8750 2950 60  0000 C CNN
F 3 "" H 8750 2950 60  0000 C CNN
	1    8750 2950
	1    0    0    -1  
$EndComp
$Comp
L LED D13
U 1 1 522CCA95
P 9550 3850
F 0 "D13" H 9550 3750 50  0000 C CNN
F 1 "LED" H 9550 3950 50  0000 C CNN
F 2 "~" H 9550 3850 60  0000 C CNN
F 3 "~" H 9550 3850 60  0000 C CNN
	1    9550 3850
	1    0    0    -1  
$EndComp
$Comp
L R R37
U 1 1 522CCA9B
P 9100 3850
F 0 "R37" V 9180 3850 40  0000 C CNN
F 1 "R" V 9107 3849 40  0000 C CNN
F 2 "~" V 9030 3850 30  0000 C CNN
F 3 "~" H 9100 3850 30  0000 C CNN
	1    9100 3850
	0    1    1    0   
$EndComp
$Comp
L GND #PWR031
U 1 1 522CCAA1
P 9800 4000
F 0 "#PWR031" H 9800 4000 30  0001 C CNN
F 1 "GND" H 9800 4070 30  0001 C CNN
F 2 "" H 9800 4000 60  0000 C CNN
F 3 "" H 9800 4000 60  0000 C CNN
	1    9800 4000
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR032
U 1 1 522CCAA7
P 8850 3450
F 0 "#PWR032" H 8850 3360 20  0001 C CNN
F 1 "+5V" H 8850 3360 30  0000 C CNN
F 2 "" H 8850 3450 60  0000 C CNN
F 3 "" H 8850 3450 60  0000 C CNN
	1    8850 3450
	1    0    0    -1  
$EndComp
$Comp
L R R29
U 1 1 522CCAAD
P 8300 3650
F 0 "R29" V 8380 3650 40  0000 C CNN
F 1 "R" V 8307 3651 40  0000 C CNN
F 2 "" V 8230 3650 30  0000 C CNN
F 3 "" H 8300 3650 30  0000 C CNN
	1    8300 3650
	0    -1   -1   0   
$EndComp
$Comp
L NPN Q13
U 1 1 522CCAB5
P 8750 3650
F 0 "Q13" H 8750 3500 50  0000 R CNN
F 1 "NPN" H 8750 3800 50  0000 R CNN
F 2 "" H 8750 3650 60  0000 C CNN
F 3 "" H 8750 3650 60  0000 C CNN
	1    8750 3650
	1    0    0    -1  
$EndComp
$Comp
L LED D14
U 1 1 522CCABC
P 9550 4500
F 0 "D14" H 9550 4400 50  0000 C CNN
F 1 "LED" H 9550 4600 50  0000 C CNN
F 2 "~" H 9550 4500 60  0000 C CNN
F 3 "~" H 9550 4500 60  0000 C CNN
	1    9550 4500
	1    0    0    -1  
$EndComp
$Comp
L R R38
U 1 1 522CCAC2
P 9100 4500
F 0 "R38" V 9180 4500 40  0000 C CNN
F 1 "R" V 9107 4499 40  0000 C CNN
F 2 "~" V 9030 4500 30  0000 C CNN
F 3 "~" H 9100 4500 30  0000 C CNN
	1    9100 4500
	0    1    1    0   
$EndComp
$Comp
L GND #PWR033
U 1 1 522CCAC8
P 9800 4650
F 0 "#PWR033" H 9800 4650 30  0001 C CNN
F 1 "GND" H 9800 4720 30  0001 C CNN
F 2 "" H 9800 4650 60  0000 C CNN
F 3 "" H 9800 4650 60  0000 C CNN
	1    9800 4650
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR034
U 1 1 522CCACE
P 8850 4100
F 0 "#PWR034" H 8850 4010 20  0001 C CNN
F 1 "+5V" H 8850 4010 30  0000 C CNN
F 2 "" H 8850 4100 60  0000 C CNN
F 3 "" H 8850 4100 60  0000 C CNN
	1    8850 4100
	1    0    0    -1  
$EndComp
$Comp
L R R30
U 1 1 522CCAD4
P 8300 4300
F 0 "R30" V 8380 4300 40  0000 C CNN
F 1 "R" V 8307 4301 40  0000 C CNN
F 2 "" V 8230 4300 30  0000 C CNN
F 3 "" H 8300 4300 30  0000 C CNN
	1    8300 4300
	0    -1   -1   0   
$EndComp
$Comp
L NPN Q14
U 1 1 522CCADC
P 8750 4300
F 0 "Q14" H 8750 4150 50  0000 R CNN
F 1 "NPN" H 8750 4450 50  0000 R CNN
F 2 "" H 8750 4300 60  0000 C CNN
F 3 "" H 8750 4300 60  0000 C CNN
	1    8750 4300
	1    0    0    -1  
$EndComp
$Comp
L LED D15
U 1 1 522CCAE3
P 9550 5150
F 0 "D15" H 9550 5050 50  0000 C CNN
F 1 "LED" H 9550 5250 50  0000 C CNN
F 2 "~" H 9550 5150 60  0000 C CNN
F 3 "~" H 9550 5150 60  0000 C CNN
	1    9550 5150
	1    0    0    -1  
$EndComp
$Comp
L R R39
U 1 1 522CCAE9
P 9100 5150
F 0 "R39" V 9180 5150 40  0000 C CNN
F 1 "R" V 9107 5149 40  0000 C CNN
F 2 "~" V 9030 5150 30  0000 C CNN
F 3 "~" H 9100 5150 30  0000 C CNN
	1    9100 5150
	0    1    1    0   
$EndComp
$Comp
L GND #PWR035
U 1 1 522CCAEF
P 9800 5300
F 0 "#PWR035" H 9800 5300 30  0001 C CNN
F 1 "GND" H 9800 5370 30  0001 C CNN
F 2 "" H 9800 5300 60  0000 C CNN
F 3 "" H 9800 5300 60  0000 C CNN
	1    9800 5300
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR036
U 1 1 522CCAF5
P 8850 4750
F 0 "#PWR036" H 8850 4660 20  0001 C CNN
F 1 "+5V" H 8850 4660 30  0000 C CNN
F 2 "" H 8850 4750 60  0000 C CNN
F 3 "" H 8850 4750 60  0000 C CNN
	1    8850 4750
	1    0    0    -1  
$EndComp
$Comp
L R R31
U 1 1 522CCAFB
P 8300 4950
F 0 "R31" V 8380 4950 40  0000 C CNN
F 1 "R" V 8307 4951 40  0000 C CNN
F 2 "" V 8230 4950 30  0000 C CNN
F 3 "" H 8300 4950 30  0000 C CNN
	1    8300 4950
	0    -1   -1   0   
$EndComp
$Comp
L NPN Q15
U 1 1 522CCB03
P 8750 4950
F 0 "Q15" H 8750 4800 50  0000 R CNN
F 1 "NPN" H 8750 5100 50  0000 R CNN
F 2 "" H 8750 4950 60  0000 C CNN
F 3 "" H 8750 4950 60  0000 C CNN
	1    8750 4950
	1    0    0    -1  
$EndComp
$Comp
L LED D16
U 1 1 522CCB0A
P 9550 5800
F 0 "D16" H 9550 5700 50  0000 C CNN
F 1 "LED" H 9550 5900 50  0000 C CNN
F 2 "~" H 9550 5800 60  0000 C CNN
F 3 "~" H 9550 5800 60  0000 C CNN
	1    9550 5800
	1    0    0    -1  
$EndComp
$Comp
L R R40
U 1 1 522CCB10
P 9100 5800
F 0 "R40" V 9180 5800 40  0000 C CNN
F 1 "R" V 9107 5799 40  0000 C CNN
F 2 "~" V 9030 5800 30  0000 C CNN
F 3 "~" H 9100 5800 30  0000 C CNN
	1    9100 5800
	0    1    1    0   
$EndComp
$Comp
L GND #PWR037
U 1 1 522CCB16
P 9800 5950
F 0 "#PWR037" H 9800 5950 30  0001 C CNN
F 1 "GND" H 9800 6020 30  0001 C CNN
F 2 "" H 9800 5950 60  0000 C CNN
F 3 "" H 9800 5950 60  0000 C CNN
	1    9800 5950
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR038
U 1 1 522CCB1C
P 8850 5400
F 0 "#PWR038" H 8850 5310 20  0001 C CNN
F 1 "+5V" H 8850 5310 30  0000 C CNN
F 2 "" H 8850 5400 60  0000 C CNN
F 3 "" H 8850 5400 60  0000 C CNN
	1    8850 5400
	1    0    0    -1  
$EndComp
$Comp
L R R32
U 1 1 522CCB22
P 8300 5600
F 0 "R32" V 8380 5600 40  0000 C CNN
F 1 "R" V 8307 5601 40  0000 C CNN
F 2 "" V 8230 5600 30  0000 C CNN
F 3 "" H 8300 5600 30  0000 C CNN
	1    8300 5600
	0    -1   -1   0   
$EndComp
$Comp
L NPN Q16
U 1 1 522CCB2A
P 8750 5600
F 0 "Q16" H 8750 5450 50  0000 R CNN
F 1 "NPN" H 8750 5750 50  0000 R CNN
F 2 "" H 8750 5600 60  0000 C CNN
F 3 "" H 8750 5600 60  0000 C CNN
	1    8750 5600
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR039
U 1 1 52681A86
P 2550 5750
F 0 "#PWR039" H 2550 5660 20  0001 C CNN
F 1 "+5V" H 2550 5660 30  0000 C CNN
F 2 "" H 2550 5750 60  0000 C CNN
F 3 "" H 2550 5750 60  0000 C CNN
	1    2550 5750
	1    0    0    -1  
$EndComp
$Comp
L R R43
U 1 1 52681A8C
P 2000 5950
F 0 "R43" V 2080 5950 40  0000 C CNN
F 1 "R" V 2007 5951 40  0000 C CNN
F 2 "" V 1930 5950 30  0000 C CNN
F 3 "" H 2000 5950 30  0000 C CNN
	1    2000 5950
	0    -1   -1   0   
$EndComp
$Comp
L NPN Q19
U 1 1 52681A92
P 2450 5950
F 0 "Q19" H 2450 5800 50  0000 R CNN
F 1 "NPN" H 2450 6100 50  0000 R CNN
F 2 "" H 2450 5950 60  0000 C CNN
F 3 "" H 2450 5950 60  0000 C CNN
	1    2450 5950
	1    0    0    -1  
$EndComp
$Comp
L SKQGAKE010 SW1
U 1 1 54737AD3
P 4700 6600
F 0 "SW1" H 4715 6480 60  0000 C CNN
F 1 "SKQGAKE010" H 4715 6710 60  0000 C CNN
F 2 "" H 4700 6639 60  0000 C CNN
F 3 "" H 4700 6600 60  0000 C CNN
	1    4700 6600
	1    0    0    -1  
$EndComp
Wire Wire Line
	2050 1100 2250 1100
Wire Wire Line
	1600 1200 2250 1200
Wire Wire Line
	2050 1300 2250 1300
Wire Wire Line
	1600 1400 2250 1400
Wire Wire Line
	2050 1500 2250 1500
Wire Wire Line
	1600 1600 2250 1600
Wire Wire Line
	2250 1700 2050 1700
Wire Wire Line
	1600 1800 2250 1800
Wire Wire Line
	2050 1900 2250 1900
Wire Wire Line
	3050 1100 3250 1100
Wire Wire Line
	3050 1200 3650 1200
Wire Wire Line
	3050 1300 3250 1300
Wire Wire Line
	3050 1400 3650 1400
Wire Wire Line
	3050 1500 3250 1500
Wire Wire Line
	3050 1600 3650 1600
Wire Wire Line
	3050 1700 3250 1700
Wire Wire Line
	3050 1800 3650 1800
Wire Wire Line
	3050 1900 3250 1900
Wire Wire Line
	2200 2100 2200 2000
Wire Wire Line
	2200 2000 2250 2000
Wire Wire Line
	3050 2000 3150 2000
Wire Wire Line
	3150 2000 3150 2050
Wire Wire Line
	5200 1000 5400 1000
Wire Wire Line
	4100 6575 4100 6750
Wire Wire Line
	7100 1200 7150 1200
Wire Wire Line
	7150 1200 7150 1350
Wire Wire Line
	5200 1650 5400 1650
Wire Wire Line
	7100 1850 7150 1850
Wire Wire Line
	7150 1850 7150 2000
Wire Wire Line
	5200 2300 5400 2300
Wire Wire Line
	7100 2500 7150 2500
Wire Wire Line
	7150 2500 7150 2650
Wire Wire Line
	5200 2950 5400 2950
Wire Wire Line
	7100 3150 7150 3150
Wire Wire Line
	7150 3150 7150 3300
Wire Wire Line
	5200 3650 5400 3650
Wire Wire Line
	7100 3850 7150 3850
Wire Wire Line
	7150 3850 7150 4000
Wire Wire Line
	5200 4300 5400 4300
Wire Wire Line
	7100 4500 7150 4500
Wire Wire Line
	7150 4500 7150 4650
Wire Wire Line
	5200 4950 5400 4950
Wire Wire Line
	7100 5150 7150 5150
Wire Wire Line
	7150 5150 7150 5300
Wire Wire Line
	5200 5600 5400 5600
Wire Wire Line
	7100 5800 7150 5800
Wire Wire Line
	7150 5800 7150 5950
Wire Wire Line
	7850 1000 8050 1000
Wire Wire Line
	9750 1200 9800 1200
Wire Wire Line
	9800 1200 9800 1350
Wire Wire Line
	7850 1650 8050 1650
Wire Wire Line
	9750 1850 9800 1850
Wire Wire Line
	9800 1850 9800 2000
Wire Wire Line
	7850 2300 8050 2300
Wire Wire Line
	9750 2500 9800 2500
Wire Wire Line
	9800 2500 9800 2650
Wire Wire Line
	7850 2950 8050 2950
Wire Wire Line
	9750 3150 9800 3150
Wire Wire Line
	9800 3150 9800 3300
Wire Wire Line
	7850 3650 8050 3650
Wire Wire Line
	9750 3850 9800 3850
Wire Wire Line
	9800 3850 9800 4000
Wire Wire Line
	7850 4300 8050 4300
Wire Wire Line
	9750 4500 9800 4500
Wire Wire Line
	9800 4500 9800 4650
Wire Wire Line
	7850 4950 8050 4950
Wire Wire Line
	9750 5150 9800 5150
Wire Wire Line
	9800 5150 9800 5300
Wire Wire Line
	7850 5600 8050 5600
Wire Wire Line
	9750 5800 9800 5800
Wire Wire Line
	9800 5800 9800 5950
Wire Wire Line
	1550 5950 1750 5950
Wire Wire Line
	2550 6500 2550 6150
Wire Wire Line
	4100 6575 4550 6575
Wire Wire Line
	4550 6575 4550 6625
Wire Wire Line
	4850 6575 5200 6575
Wire Wire Line
	4850 6575 4850 6625
$EndSCHEMATC
