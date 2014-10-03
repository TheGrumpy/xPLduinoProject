using System;
using Pango;

namespace xPLduinoManager
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class Draw : Gtk.Bin
	{		
		Pango.Layout layout;
		
		public Draw ()
		{
			this.Build ();
                layout = new Pango.Layout (this.PangoContext);
                layout.Wrap = Pango.WrapMode.Word;
                layout.FontDescription = FontDescription.FromString ("Tahoma 16");
                layout.SetMarkup ("Hello Pango.Layout");			
		}
	}
}

