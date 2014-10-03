//
//  LanguageManager.cs
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
using System.Collections.Generic; // IList, Dictionary

namespace xPLduinoManager {
    public class LanguageManager {
        // FIXME: make MainWindow a general interface (without Gtk)
        public MainWindow calico;
        public Dictionary<string, Language> languages;
       /* public CustomStream stderr;
        public CustomStream stdout;
        public Microsoft.Scripting.Hosting.ScriptRuntime scriptRuntime;
        public Microsoft.Scripting.Hosting.ScriptScope scope;
        public Microsoft.Scripting.Hosting.ScriptRuntimeSetup scriptRuntimeSetup;*/
        public bool UseSharedScope = true;

        public LanguageManager(IList<string> visible_languages, string path, Dictionary<string,Language> langs) {
            languages = new Dictionary<string, Language>();
            foreach (string name in langs.Keys) {
                Register(langs[name], visible_languages.Contains(name)); // This may fail, which won't add language
            }
            Setup(path);
            Start(path);
        }

        public void Setup(string path) {
            // In case it needs it for DLR languages
            scriptRuntimeSetup = new Microsoft.Scripting.Hosting.ScriptRuntimeSetup();
            foreach (string language in getLanguages()) {
                if (languages[language].engine != null) {
                    try {
                        languages[language].engine.Setup(path);
                    } catch (Exception e) {
                        Console.Error.WriteLine("Language failed to initialize: {0} {1}", language, e.Message);
                        languages.Remove(language);
                    }
                }
            }
            // Language neutral scope:
            try {
                scriptRuntime = new Microsoft.Scripting.Hosting.ScriptRuntime(scriptRuntimeSetup);
                scope = scriptRuntime.CreateScope();
            } catch {
                Console.Error.WriteLine("No DLR languages were loaded.");
            }

        }

        public Language this[string name] {
            get {return languages[name];}
            set {languages[name] = value;}
        }

        public string[] getLanguages() {
            string[] keys = new string[languages.Count];
            languages.Keys.CopyTo(keys, 0);
            Array.Sort(keys);
            return keys;
        }

        public string[] getLanguagesProper() {
            string[] keys = new string[languages.Count];
            int i = 0;
            foreach (string key in languages.Keys) {
                keys[i] = languages[key].proper_name;
                i++;
            }
            Array.Sort(keys);
            return keys;
        }

        public void Register(Language language, bool visible) {
            if (visible) {
                try {
                    language.MakeEngine(this); // Makes a default engine
                } catch {
                    Console.WriteLine("Register failed; skipping language {0}", language.name);
                    return;
                }
            }
            languages[language.name] = language; // ok, save it
        }

        public void SetCalico(MainWindow calico) {
            this.calico = calico;
            if (scope != null)
                scope.SetVariable("calico", calico);
        }
		
		/*
        public void SetRedirects(CustomStream stdout, CustomStream stderr) {
            // textviews:
            this.stderr = stderr;
            this.stdout = stdout;
            foreach (string language in languages.Keys) {
                if (languages[language].engine != null)
                    languages[language].engine.SetRedirects(this.stdout, this.stderr);
            }
        }*/

        public void Start(string path) {
            foreach (string language in languages.Keys) {
                if (languages[language].engine != null)
                    languages[language].engine.Start(path);
            }
        }

        public void PostSetup(MainWindow calico) {
            foreach (string language in languages.Keys) {
                if (languages[language].engine != null)
                    languages[language].engine.PostSetup(calico);
            }
        }

        public void Reset(MainWindow calico) {
            Setup(calico.path);
            Start(calico.path);
            SetRedirects(stdout, stderr);
            PostSetup(calico);
        }

        public string GetLanguageFromExtension(string filename) {
            string file_ext = System.IO.Path.GetExtension(filename);
            if (file_ext != string.Empty && file_ext != "") {
                file_ext = file_ext.Substring(1);
                foreach (string language in languages.Keys) {
                    foreach (string ext in languages[language].extensions) {
                        if (ext == file_ext) {
                            return language;
                        }
                    }
                }
            }
            return null;
        }
    }
}


