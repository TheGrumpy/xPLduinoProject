EESchema Schematic File Version 2  date 10/02/2013 13:01:35
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
LIBS:hr911105_rj45
LIBS:Small_connectors
LIBS:74hc08
LIBS:lm1117imp-3
LIBS:mp2562ds
LIBS:usd_conn
LIBS:74lvc1g126gw
LIBS:dual_self
LIBS:ALPS_SWITCHS
LIBS:SMB-cache
EELAYER 25  0
EELAYER END
$Descr A4 11700 8267
encoding utf-8
Sheet 2 4
Title "Short Main Board - SMB controler"
Date "10 feb 2013"
Rev "0"
Comp "XPLDUINO project"
Comment1 "GNU GPL v2"
Comment2 "Domotic Open Source "
Comment3 ""
Comment4 ""
$EndDescr
Connection ~ 1775 3575
Wire Wire Line
	1850 3575 1600 3575
Wire Notes Line
	2675 3800 2675 800 
Wire Notes Line
	2675 3800 1150 3800
Wire Notes Line
	1150 3800 1150 800 
Wire Wire Line
	2350 3475 2300 3475
Wire Wire Line
	3450 2425 3675 2425
Wire Wire Line
	3575 2750 3575 2800
Wire Wire Line
	1800 2650 1850 2650
Wire Wire Line
	2100 2700 2100 2750
Wire Wire Line
	2100 2350 2100 2400
Connection ~ 2100 1750
Wire Wire Line
	2100 1750 1800 1750
Wire Wire Line
	1800 1750 1800 1850
Wire Wire Line
	1800 1850 1850 1850
Wire Wire Line
	2100 2075 2100 2125
Wire Wire Line
	2350 1925 2300 1925
Wire Wire Line
	2100 1475 2100 1525
Connection ~ 2100 1150
Wire Wire Line
	2100 1150 1800 1150
Wire Wire Line
	1800 1150 1800 1250
Wire Wire Line
	1800 1250 1850 1250
Wire Wire Line
	2350 1325 2300 1325
Connection ~ 3575 2275
Wire Wire Line
	3575 2275 3675 2275
Wire Wire Line
	1800 1425 1850 1425
Wire Wire Line
	2100 1125 2100 1175
Wire Wire Line
	3500 2175 3675 2175
Wire Wire Line
	3500 2225 3675 2225
Wire Wire Line
	1800 2025 1850 2025
Wire Wire Line
	2100 1725 2100 1775
Wire Wire Line
	3500 2325 3675 2325
Wire Wire Line
	2100 2375 1800 2375
Connection ~ 2100 2375
Wire Wire Line
	1800 2375 1800 2475
Wire Wire Line
	1800 2475 1850 2475
Wire Wire Line
	2350 2550 2300 2550
Wire Notes Line
	2675 2900 1150 2900
Wire Notes Line
	1150 800  2675 800 
Wire Wire Line
	3575 2075 3575 2550
Wire Wire Line
	3675 2375 3500 2375
Wire Wire Line
	3500 2375 3500 2775
Wire Wire Line
	3500 2775 3575 2775
Connection ~ 3575 2775
Wire Wire Line
	2100 3275 2100 3325
Wire Wire Line
	2100 3625 2100 3675
Wire Wire Line
	1850 3400 1775 3400
Wire Wire Line
	1775 3400 1775 3575
Text Notes 1150 3950 0    60   ~ 0
Need to tie OE to ground to \nhave a high impedance output !
Text Notes 2450 3125 2    100  ~ 0
3.3V -> 5V
Text GLabel 1600 3575 0    25   Input ~ 0
MISO_33
Text GLabel 2350 3475 2    25   Output ~ 0
SPI_MISO
$Comp
L GND #PWR030
U 1 1 4FF08F4C
P 2100 3675
F 0 "#PWR030" H 2100 3675 30  0001 C CNN
F 1 "GND" H 2100 3605 30  0001 C CNN
	1    2100 3675
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR031
U 1 1 4FF08F3A
P 2100 3275
F 0 "#PWR031" H 2100 3365 20  0001 C CNN
F 1 "+5V" H 2100 3365 30  0000 C CNN
	1    2100 3275
	1    0    0    -1  
$EndComp
$Comp
L 74LVC1G126GW U206
U 1 1 4FF08F14
P 2000 3525
F 0 "U206" H 2275 3400 60  0000 C CNN
F 1 "74LVC1G126GW" H 1525 3775 60  0000 C CNN
F 2 "SOT353" H 1600 3675 60  0000 C CNN
	1    2000 3525
	1    0    0    -1  
$EndComp
Text GLabel 3450 2425 0    25   Output ~ 0
MISO_33
Text Notes 2425 975  2    100  ~ 0
5V -> 3.3V
Text GLabel 2350 2550 2    25   Output ~ 0
CLK_33
Text GLabel 1800 2650 0    25   Input ~ 0
SPI_CLK
$Comp
L GND #PWR032
U 1 1 4FF08D9D
P 2100 2750
F 0 "#PWR032" H 2100 2750 30  0001 C CNN
F 1 "GND" H 2100 2680 30  0001 C CNN
	1    2100 2750
	1    0    0    -1  
$EndComp
$Comp
L +3.3V #PWR033
U 1 1 4FF08D91
P 2100 2350
F 0 "#PWR033" H 2100 2310 30  0001 C CNN
F 1 "+3.3V" H 2100 2460 30  0000 C CNN
	1    2100 2350
	1    0    0    -1  
$EndComp
$Comp
L 74LVC1G126GW U205
U 1 1 4FF08D87
P 2000 2600
F 0 "U205" H 2275 2475 60  0000 C CNN
F 1 "74LVC1G126GW" H 1600 2650 60  0000 C CNN
F 2 "SOT353" H 1600 2750 60  0000 C CNN
	1    2000 2600
	1    0    0    -1  
$EndComp
Text GLabel 3500 2325 0    25   Input ~ 0
CLK_33
$Comp
L +3.3V #PWR034
U 1 1 4FF07933
P 2100 1725
F 0 "#PWR034" H 2100 1685 30  0001 C CNN
F 1 "+3.3V" H 2100 1835 30  0000 C CNN
	1    2100 1725
	1    0    0    -1  
$EndComp
$Comp
L GND #PWR035
U 1 1 4FF0791F
P 2100 2125
F 0 "#PWR035" H 2100 2125 30  0001 C CNN
F 1 "GND" H 2100 2055 30  0001 C CNN
	1    2100 2125
	1    0    0    -1  
$EndComp
Text GLabel 1800 2025 0    25   Input ~ 0
SPI_MOSI
Text GLabel 2350 1925 2    25   Output ~ 0
MOSI_33
Text GLabel 3500 2225 0    25   Input ~ 0
MOSI_33
$Comp
L GND #PWR036
U 1 1 4FF077F6
P 2100 1525
F 0 "#PWR036" H 2100 1525 30  0001 C CNN
F 1 "GND" H 2100 1455 30  0001 C CNN
	1    2100 1525
	1    0    0    -1  
$EndComp
Text GLabel 3500 2175 0    25   Input ~ 0
CS_33
Text GLabel 2350 1325 2    25   Output ~ 0
CS_33
$Comp
L 74LVC1G126GW U204
U 1 1 4FF07749
P 2000 1975
F 0 "U204" H 2275 1850 60  0000 C CNN
F 1 "74LVC1G126GW" H 1600 2025 60  0000 C CNN
F 2 "SOT353" H 1600 2125 60  0000 C CNN
	1    2000 1975
	1    0    0    -1  
$EndComp
Text GLabel 1800 1425 0    25   Input ~ 0
uSD_CS
$Comp
L +3.3V #PWR037
U 1 1 4FF0764D
P 2100 1125
F 0 "#PWR037" H 2100 1085 30  0001 C CNN
F 1 "+3.3V" H 2100 1235 30  0000 C CNN
	1    2100 1125
	1    0    0    -1  
$EndComp
$Comp
L 74LVC1G126GW U203
U 1 1 4FF07632
P 2000 1375
F 0 "U203" H 2275 1250 60  0000 C CNN
F 1 "74LVC1G126GW" H 1600 1425 60  0000 C CNN
F 2 "SOT353" H 1600 1525 60  0000 C CNN
	1    2000 1375
	1    0    0    -1  
$EndComp
$Comp
L +3.3V #PWR038
U 1 1 4FF06B96
P 3575 2075
F 0 "#PWR038" H 3575 2035 30  0001 C CNN
F 1 "+3.3V" H 3575 2185 30  0000 C CNN
	1    3575 2075
	1    0    0    -1  
$EndComp
$Comp
L CSMALL C215
U 1 1 4FF06B5D
P 3575 2650
F 0 "C215" H 3600 2700 30  0000 L CNN
F 1 "0.1uF" V 3500 2600 30  0000 L CNN
F 2 "SM0603" V 3600 2600 30  0000 C CNN
	1    3575 2650
	-1   0    0    1   
$EndComp
$Comp
L GND #PWR039
U 1 1 4FF06B3E
P 3575 2800
F 0 "#PWR039" H 3575 2800 30  0001 C CNN
F 1 "GND" H 3575 2730 30  0001 C CNN
	1    3575 2800
	1    0    0    -1  
$EndComp
$Comp
L USD_CONN J202
U 1 1 4FF052FB
P 3900 2300
F 0 "J202" H 4100 2125 60  0000 C CNN
F 1 "USD_CONN" H 3950 2575 60  0000 C CNN
F 2 "DM3CS-SF" H 3950 2675 60  0000 C CNN
	1    3900 2300
	1    0    0    -1  
$EndComp
$EndSCHEMATC
