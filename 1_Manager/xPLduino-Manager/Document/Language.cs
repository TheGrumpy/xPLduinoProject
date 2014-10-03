//
//  Language.cs
//  
//  Author:
//       Douglas S. Blank <dblank@cs.brynmawr.edu>
// 
//  Copyright (c) 2011 The Calico Project
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;

namespace xPLduinoManager {

    public class Language {
        public string name;
        //public Engine engine;
        public string proper_name;
        public string[] extensions;
        public string mimetype;
        public bool IsTextLanguage = true;
        public string LineComment = "";
        //public Config local_config; // temp, for loading
       // public Config config; // pointer to Calico.config

        public Language() {
        }

        public Language(string language, string proper, string[] extensions, string mimetype) {
            this.name = language;
            this.proper_name = proper;
            this.extensions = extensions;
            this.mimetype = mimetype;
            InitializeConfig();
        }
        
        public virtual void InitializeConfig() {
            // Just used for init and load
            local_config = new Config();
            local_config.SetValue(System.String.Format("{0}-language", name), "reset-shell-on-run", "bool", true);
        }

        public void OnChangeResetShellOnRun (object sender, EventArgs e)
        {
            string section = String.Format("{0}-language", name);
            config.SetValue(section, "reset-shell-on-run", ((Gtk.CheckMenuItem)sender).Active);
        }
     
        public static string _(string message) {
            return global::Mono.Unix.Catalog.GetString(message);
        }

        public virtual void SetOptionsMenu(Gtk.MenuItem options_menu) {
            //Gtk.Menu menu = (Gtk.Menu)options_menu.Submenu;
            //menu.Detach();
            //options_menu.Submenu = null;
            options_menu.Submenu = new Gtk.Menu();
            string section = String.Format("{0}-language", name);
            if (config.HasValue(section, "reset-shell-on-run")) {
                bool reset_shell_on_run = (bool)config.GetValue(section, "reset-shell-on-run");
                Gtk.CheckMenuItem reset_shell_on_run_menu_item = new Gtk.CheckMenuItem(_("Reset Shell on Run"));
                reset_shell_on_run_menu_item.Active = reset_shell_on_run;
                reset_shell_on_run_menu_item.Activated += OnChangeResetShellOnRun;
                ((Gtk.Menu)options_menu.Submenu).Add(reset_shell_on_run_menu_item);
            } else {
                Gtk.MenuItem submenu = new Gtk.MenuItem(_("Reset Shell on Run")); // not available
                submenu.Sensitive = false;
                ((Gtk.Menu)options_menu.Submenu).Add(submenu);
            }
            SetAdditionalOptionsMenu((Gtk.Menu)options_menu.Submenu);
            options_menu.ShowAll();
        }

        public virtual void SetAdditionalOptionsMenu(Gtk.Menu submenu) {
            // Put language specific stuff in overloaded version
        }

 /*       public virtual void LoadConfig(Config global_config) {
            // Load the Config into the Global Config
            string section = System.String.Format("{0}-language", name);
            if (local_config != null && local_config.HasValue(section)) {
                foreach(string setting in local_config.GetValue(section)) {
                    if (! global_config.HasValue(section, setting)) {
                        global_config.SetValue(
                            section, 
                            setting, 
                            local_config.types[section][setting], 
                            local_config.values[section][setting]);
                    } // else already been defined
                }
            }
            config = global_config;
        }*/
        
        public virtual void MakeEngine(LanguageManager manager) {
            engine = new Engine(manager);
        }

        public virtual Document MakeDocument(MainWindow calico, string filename) {
          return new TextDocument(calico, filename, name, mimetype);
        }

        public virtual Document MakeDocument(MainWindow calico, string filename, string mimetype) {
          return new TextDocument(calico, filename, name, mimetype);
        }

        public static Language MakeLanguage() {
            return null;
        }

        public virtual string GetUseLibraryString (string fullname)
        {
            return "";
        }
        
        public virtual string getExamplesPath(string path) {
            return System.IO.Path.Combine(path, System.IO.Path.Combine("..", System.IO.Path.Combine("examples", name)));
        }
    }    
}
