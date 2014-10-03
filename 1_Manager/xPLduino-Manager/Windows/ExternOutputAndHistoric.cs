using System;
using Gtk;
using Gdk;
using System.Collections.Generic;
using System.Diagnostics ;


namespace xPLduinoManager
{
	public partial class ExternOutputAndHistoric : Gtk.Window
	{
		public ExternOutputAndHistoric () : base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}
		
		public void CopyWidget(Gtk.Notebook _NoteBookSource)
		{
			ViewNoteBook = _NoteBookSource;
			ViewNoteBook.ShowAll();
		}
	}
}

