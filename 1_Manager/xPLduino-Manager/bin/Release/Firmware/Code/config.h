#ifndef config_h
#define config_h


    /* desactivation DHCP    */
    #define STATIC 1  // set to 1 to disable DHCP (adjust myip/gwip values below)

    /* activation du webserver (necessite uSD) */
    #define WEBSERVER 0 // set to 1 to enable webserver (need a uSD in SMB)

    /* debug LIGHTING        */
    #define WITH_LIGHTING
    //~ #define DEBUG_LIGHTING_XPL
    #define DEBUG_LIGHTING_CORE

    /* debug SHUTTER         */
    #define WITH_SHUTTER
    //~ #define DEBUG_SHUTTER_XPL
    #define DEBUG_SHUTTER_CORE

    /* debug SWITCH          */
    #define WITH_SWITCH
    //~ #define DEBUG_SWITCH_XPL
    #define DEBUG_SWITCH_CORE

    /* debug compteurs d'eau */
    #define DEBUG_WATER_COUNTER_CORE
    #define DEBUG_WATER_COUNTER_XPL

    /* debug Ethernet        */
    #define DEBUG_ETHERNET

    /* debug message xpl     */
    //~ #define DEBUG_INCOMING
    //~ #define DEBUG_OUTCOMING

    /* debug board           */
    //~ #define DEBUG_I2C_DIMMER
    //~ #define DEBUG_BOARD_IN16
    //~ #define DEBUG_BOARD_R8
    //~ #define DEBUG_I2C_IN8R8
    
    //~ #define DEBUG_WEB_SERVER  


    /* XPL layer             */
    #define XPL_PORT 3865
    #define T_CMND 1
    #define T_STAT 2
    #define T_TRIG 3

    /* définition d'I/O */
    #define STATUS_LED      26
    #define FAULT_LED       30


    /* One Wire */
    //#define DEBUG_TEMP_OW
    //#define DEBUG_TEMP_XPL

    /* Teleinformation */
	//#define DEBUG_TELEINFO_CORE        // drapeau activation messages debug pour le core TeleInfo
	//#define DEBUG_TELEINFO_SERIAL        // drapeau activation messages debug pour la liaison série
	//#define DEBUG_TELEINFO_XPL          // drapeau activation messages debug pour l'envois XPL TeleInfo

#endif
