#define pulse(name)          SWITCH[name].isPulse()          // renvoi l'état pulse
#define dpulse(name)         SWITCH[name].isDoublePulse()    // renvoi l'état double pulse
#define on(name)             SWITCH[name].isOn()             // renvoi l'état On
#define on_ons(name)         SWITCH[name].isOnOSR()          // renvoi l'état du trigger Off -> On
#define off(name)            SWITCH[name].isOff()            // renvoi l'état Off
#define off_ons(name)        SWITCH[name].isOnOSF()          // renvoi l'état du trigger On -> Off


#define open(name)           SHUTTER[name].opening();
#define close(name)          SHUTTER[name].closing();
#define stop_shutter(name)   SHUTTER[name].stop();
#define toggle_shutter(name) SHUTTER[name].toggle();

#define toggle_light(name)   LIGHTING[name].toggle(); // allume au dernier setpoint ou eteint
#define tune(name)           LIGHTING[name].tune();
#define stop_tune(name)      LIGHTING[name].stop();


// Français
#define clic(name)           SWITCH[name].isPulse()          // renvoi l'état pulse
#define dclic(name)          SWITCH[name].isDoublePulse()    // renvoi l'état double pulse
#define on(name)             SWITCH[name].isOn()             // renvoi l'état On
#define on_fm(name)          SWITCH[name].isOnOSR()          // renvoi l'état du trigger Off -> On
#define off(name)            SWITCH[name].isOff()            // renvoi l'état Off
#define off_fm(name)         SWITCH[name].isOnOSF()          // renvoi l'état du trigger On -> Off

#define ouvrir(name)         SHUTTER[name].opening();
#define fermer(name)         SHUTTER[name].closing();
#define arreter(name)        SHUTTER[name].stop();

#define basculer(name)       LIGHTING[name].toggle(); // allume au dernier setpoint ou eteint
#define regler(name)         LIGHTING[name].tune();
#define figer(name)          LIGHTING[name].stop();
