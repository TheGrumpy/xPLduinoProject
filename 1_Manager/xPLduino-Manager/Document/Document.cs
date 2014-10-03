//
//  Document.cs
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
using System.Collections.Generic;

namespace xPLduinoManager {
    public class Document 
    {
        
        public MainWindow calico;
        public string language;
        public string filename;
        public string basename;
        public Document document;
        string _documentType;
        public Gtk.ScrolledWindow widget;
        public Gtk.Widget focus_widget;
        public Gtk.Widget tab_widget;
        public Gtk.Label tab_label;
        // label for notebook page
        public Gtk.Button close_button;
        // tab close button
        protected bool _isDirty = false;
        double _speedValue = 100;
        public string preferredNotebook = "editor"; // "editor", "main", "tools"
	    private bool _inCloud = false;

        public Document(MainWindow calico, string filename, string language) : base() 
        {
            this.calico = calico;
            this.filename = filename;
    	    if (filename != null && (filename.Contains("/Cloud/") || filename.Contains("\\Cloud\\"))) 
            {
    		inCloud = true;
    	    }
            this.language = language;
            widget = new Gtk.ScrolledWindow();
            tab_widget = new Gtk.HBox();
            tab_label = new Gtk.Label();
            ((Gtk.HBox)tab_widget).Add(tab_label);
            DocumentType = "Script";
            close_button = new Gtk.Button();
            Gtk.Image img = new Gtk.Image();
            close_button.Relief = Gtk.ReliefStyle.None;
            img = new Gtk.Image(Gtk.Stock.Close, Gtk.IconSize.Menu);
            close_button.Add(img);
            ((Gtk.HBox)tab_widget).Add(close_button);
            tab_label.TooltipText = filename;
            tab_widget.ShowAll();
        }

        public string DocumentType {
            set {
                _documentType = value;
                if (filename != null)
                    basename = System.IO.Path.GetFileName(filename);
                else if (calico == null)
                    basename = System.IO.Path.GetFileName(filename);
                else
		    basename = String.Format(MainWindow._("New {0} {1}"), 
					     calico.manager.languages[language].proper_name, 
					     MainWindow._(_documentType));
		MainWindow._("Script"); // This is here to make sure
		// "Script" is translated
		if (inCloud) {
		    tab_label.Text = String.Format("{{{0}}}", basename); 
		} else {
		    tab_label.Text = basename; //.Replace("_", "__");
		}
            }
            get { return _documentType; }
        }
        public virtual string GetText() {
            return null;
        }
        public virtual Gtk.Widget GetPropertyNotebookWidget() {
            // By default, return null
            return null;
        }
        public virtual void UseLibrary (string fullname)
        {
            // pass
        }
        public virtual void Print(MainWindow mainwindow) {
            Save();
            if (filename != null) {
                new Printing(mainwindow, basename, GetText(), filename);
            }
        }
        public virtual void ExecuteFileInBackground() {
            // this allows document to handle it, if it wishes
            // otherwise pass to calico
            calico.ExecuteFileInBackground(filename, language);
        }
        public virtual void Export(MainWindow calico) {
        }
        public static string _(string message) {
            return global::Mono.Unix.Catalog.GetString(message);
        }
        public virtual bool GotoLine(int lineno) {
            return true;
        }
        public virtual void GotoEndOfLine() {
            // Move position to end of line
        }
        public virtual void SelectLine(int lineno) {
        }
        public virtual bool HasContent {
            get {return true;}
        }
        public virtual bool HasSelection {
            get {return false;}
        }
        public virtual object Selection {
            get {return null;}
        }
        public virtual bool AlwaysAllowSpeedAdjustment {
            get { return false; }
        }
		public virtual bool Paste(object obj) {
			return false;
		}
        public virtual bool IsDirty {
            get { return _isDirty; }
            set { _isDirty = value; }
        }
        public virtual bool inCloud {
            get { return _inCloud; }
            set { 
			  if (basename != null) {
				this.filename = System.IO.Path.Combine(
					(string)calico.config.GetValue("config", "cloud-path"),
					basename);
				tab_label.TooltipText = filename;
			  }
			  _inCloud = value; 
			}
        }
        public virtual void Configure() {
            // For setting defaults
        }
        public bool IsWritable(string filename) {
            if (filename == null)
                return false;
            string directory = System.IO.Path.GetDirectoryName(filename);
            if (directory == null)
                return false;
            string tempfile = System.IO.Path.Combine(directory, "tempfile.tmp");
            bool retval = true;
            try {
                System.IO.FileStream fp = System.IO.File.OpenWrite(tempfile);
                fp.Close();
                System.IO.File.Delete(tempfile);
            } catch {
                retval = false;
            }
            return retval;
        }
        public virtual bool SaveAs() {
            bool retval = false;
            string proposed_dir = "";
            if (filename != null) {
                proposed_dir = System.IO.Path.GetDirectoryName(filename);
                basename = System.IO.Path.GetFileName(filename);
            } else {
                proposed_dir = System.IO.Directory.GetCurrentDirectory();
                basename = "Untitled." + calico.manager[language].extensions[0]; // default
            }
            // first, let's make sure the directory is writable
            if (! IsWritable(proposed_dir)) {
                // if not, let's change dirs
                proposed_dir = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
            }
            if (System.IO.Directory.GetCurrentDirectory() != proposed_dir) {
                System.IO.Directory.SetCurrentDirectory(proposed_dir);
            }
            Gtk.FileChooserDialog fc = new Gtk.FileChooserDialog(_("Enter the file to save"),
                                       calico,
                                       Gtk.FileChooserAction.Save,
                                       _("Cancel"), Gtk.ResponseType.Cancel,
                                       _("Save"), Gtk.ResponseType.Accept);
            fc.CurrentName = basename;      // the file: entry text box
            fc.SelectFilename(basename);    // the file selection, if it exists
            fc.KeepAbove = true;
            if (fc.Run() == (int)Gtk.ResponseType.Accept) {
                fc.Hide();
                // FIXME: check to see if already exists
                // ask to overwrite
                if (System.IO.File.Exists(fc.Filename)) {
                    bool yesno = MainWindow.yesno(
                        String.Format("The file\n{0}\nexists. Overwrite it?", fc.Filename));
                    if (!yesno)
                        return false;
                }
                filename = fc.Filename;
		if (filename != null && (filename.Contains("/Cloud/") || filename.Contains("\\Cloud\\"))) {
		    inCloud = true;
		}
                // Update GUI:
                language = calico.manager.GetLanguageFromExtension(filename);
                basename = System.IO.Path.GetFileName(filename);
		if (inCloud) {
		    tab_label.Text = String.Format("{{{0}}}", basename); 
		} else {
		    tab_label.Text = basename; //.Replace("_", "__");
		}
                tab_label.TooltipText = filename;
                // Already agreed to overwrite:
                retval = Save(true); // force saveas
                if (retval) {
                    // Set CurrentDirectory here:
                    string dir = System.IO.Path.GetDirectoryName(filename);
                    System.IO.Directory.SetCurrentDirectory(dir);
                    //self.on_change_file();
                    calico.FileSavedAs(this, filename); // if that file is open not here, reload it
                }
            }
            fc.Destroy();
            return retval;
        }
	    public virtual void SetFilename(string filename) {
	  this.filename = filename;
	  if (filename != null && (filename.Contains("/Cloud/") || filename.Contains("\\Cloud\\"))) {
		inCloud = true;
	  }
	  basename = System.IO.Path.GetFileName(filename);
	  tab_label.TooltipText = filename;
	}
        public virtual bool Save() {
            return Save(false);
        }
        public virtual bool Save(bool force) {
            if (IsDirty || force) { // only Save if this is dirty or forced
                if (filename != null) {
		    if (SaveDocument())
			return true;
		    // fail.. let's try SaveAs...
                }
                return SaveAs();
            } else {
                return true;
            }
        }
        public virtual void Reload() {
        }
        public virtual bool SaveDocument() {
            // Save the document details; to be overridden in subclass
            return true;
        }
        public virtual bool Close() {
            // Close document, and return successful close status
            return true;
        }
        public virtual void UpdateDocument() {
            if (tab_label != null && basename != null) {
                if (IsDirty)
                    Gtk.Application.Invoke( delegate {
			    if (inCloud) {
				tab_label.Text = String.Format("*{{{0}}}", basename); 
			    } else {
				tab_label.Text = String.Format("*{0}", basename);
			    }
                    });
                else
                    Gtk.Application.Invoke( delegate {
			    if (inCloud) {
				tab_label.Text = String.Format("{{{0}}}", basename); 
			    } else {
				tab_label.Text = basename; 
			    }
                    });
            }
        }
        public virtual void UpdateZoom() {
        }
        public virtual void ZoomIn() {
        }
        public virtual void ZoomOut() {
        }
        public virtual void DefaultZoom() {
        }
        public virtual void SearchStart() {
        }
        public virtual bool SearchNext(string s) {
            return false;
        }
        public virtual bool SearchMore(string s) {
            return false;
        }
        public virtual bool SearchPrevious(string s) {
            return false;
        }
        public virtual void SearchStop() {
        }
	    public virtual void Replace(string searchText, string replaceText) {
	}
	    public virtual void ReplaceAll(string searchText, string replaceText) {
	}
        public virtual void ToggleBreakpoint() {
        }
        public virtual bool HasBreakpointSet {
            get { return false; }
        }
        public virtual bool HasBreakpointSetAtLine(int lineno) {
            return false;
        }
        public virtual void Stop() {
        }
        public virtual void OnPlayButton() {
            calico.playResetEvent.Set();
        }
        public virtual void OnPauseButton() {
            calico.manager[language].engine.RequestPause();
            calico.playResetEvent.Reset();
        }
        public virtual string [] GetAuthors() {
            return new string[] {
                "Douglas Blank <dblank@cs.brynmawr.edu>",
                "Keith O'Hara <kohara@bard.edu>",
                "Jim Marshall <jmarshall@sarahlawrence.edu>",
                "Mark Russo <russomf@gmail.com>",
                "Jennifer Kay <kay@rowan.edu>"
            };
        }
        public virtual void OnAbout() {
            string proper_name = calico.manager[language].proper_name;
            Gtk.AboutDialog aboutDialog = new Gtk.AboutDialog();
            aboutDialog.DefaultResponse = Gtk.ResponseType.Close;
            aboutDialog.Authors = GetAuthors();
            aboutDialog.Comments = (proper_name + " " + _("for Calico"));
            aboutDialog.Copyright = _("(c) 2012-2013, Institute for Personal Robots in Education");
            aboutDialog.ProgramName = "Calico " + proper_name;
            aboutDialog.Version = MainClass.Version;
            aboutDialog.Website = "http://CalicoProject.org/Calico_" + proper_name;
            aboutDialog.WrapLicense = true;
            aboutDialog.Run();
            aboutDialog.Destroy();
        }
        public virtual bool CanSaveAsPython() {
            return false;
        }
        public virtual bool CanRemoveModule() {
            return false;
        }
	    public virtual void RemoveModule() {
		  // nothing to do
		}
        public virtual double SpeedValue {
            get { return _speedValue; }
            set { _speedValue = value; }
        }
        public virtual void SetOptionsMenu(Gtk.MenuItem options_menu) {
            // can do it here, too, for document-specific configs
        }
    }
 
    public class MyTextEditor : Mono.TextEditor.TextEditor {
        public MyTextEditor() : base() {
        }
        public MyTextEditor(Mono.TextEditor.Document doc, Mono.TextEditor.ITextEditorOptions options) : base(doc, options) {
        }
        protected override void OnDragDataReceived (Gdk.DragContext context, Int32 x, Int32 y, Gtk.SelectionData selection_data, UInt32 info, UInt32 time_) {
            // This is overriden because there seems to be an error in the base class:
            // Selection constructor tries to use line 0, line 0
            // FIXME: add in appropriate place
            Text += selection_data.Text;
       }
    }
    
    public class TextDocument : Document 
    {
        public Mono.TextEditor.TextEditor texteditor;
        public Mono.TextEditor.TextEditorOptions options;
 
        public TextDocument(MainWindow calico, string filename, string language, string mimetype) : base(calico, filename, language) 
        {
            Mono.TextEditor.Highlighting.SyntaxModeService.LoadStylesAndModes(
                System.IO.Path.Combine(calico.path, "SyntaxModes"));
            options = new Mono.TextEditor.TextEditorOptions();
            Mono.TextEditor.Document document = new Mono.TextEditor.Document();
            if (System.IO.File.Exists(filename)) {
                System.IO.TextReader reader = new System.IO.StreamReader(filename);
                document.Text = reader.ReadToEnd();
                reader.Close();
            } else {
                // FIXME: new file? invalid path? no longer exists?
            }
            texteditor = new MyTextEditor(document, options);
            focus_widget = texteditor;
            //texteditor.DragDataReceived += HandleTexteditorDragDataReceived;
            if (mimetype != null)
                texteditor.Document.MimeType = mimetype;
            widget.Add(texteditor);
            texteditor.Document.DocumentUpdated += OnDocumentUpdated;
            texteditor.ButtonPressEvent += OnPopupMenu;
	    //widget.PopulatePopup += HandlePopulatePopup;
            widget.ShowAll();
            calico.ProgramSpeed.Value = SpeedValue;
        }
  
        
        void HandlePopulatePopup(object o, Gtk.PopulatePopupArgs args) {
            // First, see what kind of line this is:
	    /*
            int position = Output.Buffer.CursorPosition;
            Gtk.TextIter currentiter = Output.Buffer.GetIterAtOffset(position);
            int char_offset = currentiter.CharsInLine;
            int line = currentiter.Line;
	    if (char_offset > 1) {
		Gtk.TextIter enditer = Output.Buffer.GetIterAtLineOffset(line, char_offset - 1);
		Gtk.TextIter textiter = Output.Buffer.GetIterAtLine(line);
		String text = textiter.GetVisibleText(enditer);
		AddGotoFileToMenu(text, args);
		AddGotoHelpToMenu(text, args);
	    }
	    */
        }

        [GLib.ConnectBeforeAttribute]
        void HandleTexteditorDragDataReceived (object o, Gtk.DragDataReceivedArgs args)
        {
            args.RetVal = false;
        }

        Gdk.Point menuPopupLocation;

        void OnPopupMenu (object o, Gtk.ButtonPressEventArgs args)
        {
            if (args.Event.Button == 3) {
                double textEditorXOffset = args.Event.X - texteditor.TextViewMargin.XOffset;
                if (textEditorXOffset < 0)
                  return;
                this.menuPopupLocation = new Gdk.Point ((int)args.Event.X, (int)args.Event.Y);
                //DocumentLocation loc= this.TextViewMargin.VisualToDocumentLocation (textEditorXOffset, (int)args.Event.Y);
                //if (!this.IsSomethingSelected || !this.SelectionRange.Contains (Document.LocationToOffset (loc)))
                //  Caret.Location = loc;
                ShowPopup ();
                //texteditor.ResetMouseState ();
            }
        }

        void PositionPopupMenu (Gtk.Menu menu, out int x, out int y, out bool pushIn)
        {
              texteditor.GdkWindow.GetOrigin (out x, out y);
              x += this.menuPopupLocation.X;
              y += this.menuPopupLocation.Y;
              pushIn = true;
        }

        void ShowPopup() {
              Gtk.Menu menu = new Gtk.Menu();
              Gtk.MenuItem menuitem = new Gtk.MenuItem("Toggle Breakpoint");
              menuitem.Activated += (sender, e) => ToggleBreakpoint();
              menu.Append (menuitem);
              menu.Destroyed += delegate {
                texteditor.QueueDraw ();
              };
              menu.ShowAll();
              menu.Popup (null, null, new Gtk.MenuPositionFunc (PositionPopupMenu), 0, Gtk.Global.CurrentEventTime);
        }

        public override void UpdateZoom() {
	        texteditor.Options.FontName = calico.GetFont().ToString();
        }

        public virtual void OnDocumentUpdated(object obj, System.EventArgs args) {
            if (IsDirty) {
		if (inCloud) {
		    tab_label.Text = String.Format("*{{{0}}}", basename); 
		} else {
		    tab_label.Text = String.Format("*{0}", basename);
		}
            } else {
		if (inCloud) {
		    tab_label.Text = String.Format("{{{0}}}", basename); 
		} else {
		    tab_label.Text = basename; 
		}
	    }
            calico.updateControls(this, true); 
        }

        public override void Configure() {
            // FIXME: take into account user's defaults
            texteditor.Options.ShowInvalidLines = false;
            texteditor.Options.ShowLineNumberMargin = true;
            texteditor.Options.TabsToSpaces = true;
            texteditor.Options.HighlightCaretLine = true;
            texteditor.Options.HighlightMatchingBracket = true;
            texteditor.Options.OverrideDocumentEolMarker = true;
            texteditor.Options.DefaultEolMarker = "\n";
            texteditor.Options.ShowIconMargin = true;
            texteditor.Options.ShowFoldMargin = true;
	        texteditor.Options.FontName = calico.GetFont().ToString();
        }

        public override bool GotoLine(int lineno) {
	    texteditor.GrabFocus();
            var data = texteditor.GetTextEditorData();
            data.Caret.Line = lineno;
            data.Caret.Column = 1;
            texteditor.ScrollToCaret();
            return true;
        }

        public override void GotoEndOfLine() {
            var data = texteditor.GetTextEditorData();
            var curLine = texteditor.GetLine (data.Caret.Line);
            data.Caret.Column = System.Math.Min (curLine.EditableLength, System.Math.Max (0, curLine.Length)) + 1;
        }

        public override void SelectLine(int lineno) {
            var data = texteditor.GetTextEditorData();
            var curLine = texteditor.GetLine (data.Caret.Line);
    	    texteditor.SetSelection(lineno, 1, lineno, System.Math.Min (curLine.EditableLength, System.Math.Max (0, curLine.Length)) + 1);
        }

        public override string GetText() {
            return texteditor.Document.Text;
        }

        public override bool HasContent {
            get { return texteditor.Document.Text != ""; }
        }

        public override bool HasSelection {
            get {return texteditor.SelectedText != null;}
        }

        public override object Selection {
            get {return texteditor.SelectedText;}
        }

        public override bool IsDirty {
            get { return texteditor.Document.IsDirty || base.IsDirty; }
            set { base.IsDirty = value; } // force it dirty
        }

        public override bool Close() {
            return true;
        }

        public override void Stop() {
            calico.AbortThread();
        }

        public override bool SaveDocument() {
		  try {
			System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
			sw.Write(texteditor.Document.Text);
			sw.Close();
			string possible_new_mime_type = calico.GetMimeType(filename);
			if (possible_new_mime_type != null &&
				texteditor.Document.MimeType != possible_new_mime_type) {
			  texteditor.Document.MimeType = possible_new_mime_type;
			  // FAIL:
			  //texteditor.Document.UpdateHighlighting();
			  //texteditor.Document.CommitUpdateAll();
			  //Mono.TextEditor.Highlighting.SyntaxModeService.WaitUpdate(texteditor.Document);
			  // HACK:
			  texteditor.Document.Text = texteditor.Document.Text;
			}
			texteditor.Document.SetNotDirtyState();
			tab_label.TooltipText = filename;
			base.IsDirty = false;
			if (inCloud) {
			  calico.SaveToCloud(filename);
			}
			return true;
		  } catch {
			return false;
		  }
        }

        Mono.TextEditor.SearchResult Search(string s, bool from_selection_start) {
            // internal to handle next/next with more text
            int offset;
            Mono.TextEditor.SearchResult search_result = null;
            texteditor.SearchPattern = s;
            var selection_range = texteditor.SelectionRange;
            if (selection_range != null) {
                if (from_selection_start) {
                    offset = selection_range.Offset;
                } else {
                    offset = selection_range.Offset + selection_range.Length;
                }
            } else {
                offset = texteditor.Caret.Offset;
            }
            search_result = texteditor.SearchForward(offset);
            if (search_result != null) {
                offset = search_result.Offset;
                int length = search_result.Length;
                texteditor.Caret.Offset = offset + length;
                texteditor.SetSelection(offset, offset + length);
                texteditor.ScrollToCaret();
            }
            return search_result;
        }

        public override void Replace(string searchText, string replaceText) {
	    Mono.TextEditor.SearchResult sr = Search(searchText, true);
	    if (sr != null) {
		texteditor.SearchEngine.Replace(sr, replaceText);
		sr = Search(searchText, true);
	    }
	}

        public override void ReplaceAll(string searchText, string replaceText) {
	    Mono.TextEditor.SearchResult sr = Search(searchText, true);
	    while (sr != null) {
		texteditor.SearchEngine.Replace(sr, replaceText);
		sr = Search(searchText, true);
	    }
	}

        public override bool SearchNext(string s) {
            return Search(s, false) != null;
        }
        public override bool SearchMore(string s) {
            // continue with more text to search
            return Search(s, true) != null;
        }

        public override bool SearchPrevious(string s) {
            texteditor.SearchPattern = s;
            int offset;
            var selection_range = texteditor.SelectionRange;
            if (selection_range != null) {
                offset = selection_range.Offset;
            } else {
                offset = texteditor.Caret.Offset;
            }
            var search_result = texteditor.SearchBackward(offset);
            if (search_result != null) {
                offset = search_result.Offset;
                int length = search_result.Length;
                texteditor.Caret.Offset = offset + length;
                texteditor.SetSelection(offset, offset + length);
                texteditor.ScrollToCaret();
            }
            return (search_result != null);
        }

        public MonoDevelop.Debugger.DebugTextMarker GetBreakpointAtLine(int lineno) {
            Mono.TextEditor.LineSegment lineSegment = texteditor.GetLine(lineno);
            if (lineSegment.MarkerCount == 1) {
                List<object> list = new List<object>(lineSegment.Markers);
                return (MonoDevelop.Debugger.DebugTextMarker)list[0];
            }
            return null;
        }
        private int breakpointCount = 0;
        public override void ToggleBreakpoint() {
            // First, find out what line we are on
            int lineno = texteditor.Caret.Line;
            // Is there a breakpoint there?
            MonoDevelop.Debugger.DebugTextMarker marker = GetBreakpointAtLine(lineno);
            if (marker != null) {
                texteditor.Document.RemoveMarker(marker, true);
                breakpointCount--;
            } else {
                marker = new MonoDevelop.Debugger.BreakpointTextMarker(texteditor, false);
                texteditor.Document.AddMarker(lineno, marker);
                breakpointCount++;
            }
        }

        public override bool HasBreakpointSet {
            get { return breakpointCount > 0; }
        }

        public override bool HasBreakpointSetAtLine(int lineno) {
            return GetBreakpointAtLine(lineno) != null;
        }

        public override void UseLibrary (string fullname)
        {
            texteditor.Insert (0, calico.manager[language].GetUseLibraryString(fullname));
        }

        public override void Reload() {
            if (System.IO.File.Exists(filename)) {
                System.IO.TextReader reader = new System.IO.StreamReader(filename);
                texteditor.Text = reader.ReadToEnd();
                reader.Close();
            }
        }
    }
}

