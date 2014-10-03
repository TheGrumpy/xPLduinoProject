/*
    xPLduino - Manager
    Copyright (C) 2014  VOGEL Ludovic - xplduino@gmail.com

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;

namespace xPLduinoManager
{
	public class TextEdit
	{
		public Mono.TextEditor.TextEditor texteditor;
        public Mono.TextEditor.TextEditorOptions options;
		
		public Gtk.Widget focus_widget;
		public Gtk.ScrolledWindow widget;
		
		public TextEdit (string filename, string mimetype)
		{
			widget = new Gtk.ScrolledWindow();
			
         	Mono.TextEditor.Highlighting.SyntaxModeService.LoadStylesAndModes(System.IO.Path.Combine(Environment.CurrentDirectory, "SyntaxModes"));			
			options = new Mono.TextEditor.TextEditorOptions();
			Mono.TextEditor.Document document = new Mono.TextEditor.Document();
            if (System.IO.File.Exists(filename)) 
			{
                System.IO.TextReader reader = new System.IO.StreamReader(filename);
                document.Text = reader.ReadToEnd();
                reader.Close();
            } 
			else 
			{
                // FIXME: new file? invalid path? no longer exists?
            }	
			texteditor = new MyTextEditor();
			focus_widget = texteditor;
            if (mimetype != null)
                texteditor.Document.MimeType = mimetype;
            widget.Add(texteditor);
            widget.ShowAll();
			Configure();
			texteditor.Text = "#define\nint i = 0";

		}
		
		public class MyTextEditor : Mono.TextEditor.TextEditor 
		{
	        public MyTextEditor() : base() 
			{
	        }
        	public MyTextEditor(Mono.TextEditor.Document doc, Mono.TextEditor.ITextEditorOptions options) : base(doc, options) 
			{
        	}
       		protected override void OnDragDataReceived (Gdk.DragContext context, Int32 x, Int32 y, Gtk.SelectionData selection_data, UInt32 info, UInt32 time_) 
			{
            // This is overriden because there seems to be an error in the base class:
            // Selection constructor tries to use line 0, line 0
            // FIXME: add in appropriate place
            Text += selection_data.Text;
       		}
		}
    
		public void Configure()
		{
			texteditor.Options.ShowInvalidLines = false;
            texteditor.Options.ShowLineNumberMargin = true;
            texteditor.Options.TabsToSpaces = true;
            texteditor.Options.HighlightCaretLine = true;
            texteditor.Options.HighlightMatchingBracket = true;
            texteditor.Options.OverrideDocumentEolMarker = true;
            texteditor.Options.DefaultEolMarker = "\n";
            texteditor.Options.ShowIconMargin = true;
            texteditor.Options.ShowFoldMargin = true;
		}
	}
}

