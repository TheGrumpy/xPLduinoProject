PCBNEW-LibModule-V1  mer. 15 août 2012 11:15:41 CEST
# encoding utf-8
Units deci-mils
$INDEX
ELC-18B470L
HFSRA_C
$EndINDEX
$MODULE ELC-18B470L
Po 0 0 0 15 502B689D 00000000 ~~
Li ELC-18B470L
Sc 0
AR /502AB6B1
Op 0 0 0
T0 0 3000 600 600 0 120 N V 21 N "FB1"
T1 0 -2550 600 600 0 120 N V 21 N "FILTER"
DC 0 0 3975 100 100 21
$PAD
Sh "1" R 882 882 0 0 0
Dr 320 0 0
At STD N 00E0FFFF
Ne 1 "N-000002"
Po -1476 0
$EndPAD
$PAD
Sh "2" C 882 882 0 0 0
Dr 320 0 0
At STD N 00E0FFFF
Ne 2 "N-000013"
Po 1476 -50
$EndPAD
$SHAPE3D
Na "inductances/HFSRA_cy.wrl"
Sc 1 1 1
Of 0 0 0
Ro 0 0 0
$EndSHAPE3D
$EndMODULE ELC-18B470L
$MODULE HFSRA_C
Po 0 0 0 15 48E736DB 00000000 ~~
Li HFSRA_C
Sc 0
AR /502AB6B1
Op 0 0 0
T0 0 3000 600 600 0 120 N V 21 N "FB1"
T1 -1000 -2500 600 600 0 120 N V 21 N "FILTER"
DC 0 0 2000 500 150 21
$PAD
Sh "1" R 882 882 0 0 0
Dr 320 0 0
At STD N 00E0FFFF
Ne 1 "N-000002"
Po -1000 0
$EndPAD
$PAD
Sh "2" C 882 882 0 0 0
Dr 320 0 0
At STD N 00E0FFFF
Ne 2 "N-000013"
Po 1000 0
$EndPAD
$SHAPE3D
Na "inductances/HFSRA_cy.wrl"
Sc 1 1 1
Of 0 0 0
Ro 0 0 0
$EndSHAPE3D
$EndMODULE HFSRA_C
$EndLIBRARY
