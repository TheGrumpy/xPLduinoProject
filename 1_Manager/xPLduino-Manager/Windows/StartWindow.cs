using System;

namespace xPLduinoManager
{
	public partial class StartWindow : Gtk.Window
	{
		public StartWindow () : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}
	}
}

