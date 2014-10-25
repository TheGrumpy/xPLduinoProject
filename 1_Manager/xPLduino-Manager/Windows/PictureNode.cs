using System;

namespace xPLduinoManager
{
	public partial class PictureNode : Gtk.Window
	{
		public PictureNode () : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}
	}
}

