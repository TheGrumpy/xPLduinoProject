using System;
using Gtk;
using Gdk;
using System.Collections.Generic;

namespace xPLduinoManager
{
	public partial class UpdatePreference : Gtk.Dialog
	{
		public Param param;
		public MainWindow mainwindow;
		
		public UpdatePreference (Param _Param, MainWindow _Mainwindow)
		{
			this.Build ();
			this.param = _Param;
			this.mainwindow = _Mainwindow;
			InitWindows();
		}
		
		public void InitWindows()
		{
			GeneralLabelTab.Text = param.ParamT("GeneralLabelTab");
			StartTab.Text = param.ParamT("StartTab");
			VariousTab.Text = param.ParamT("VariousTab");
		}

		protected void OnButtonOkClicked (object sender, System.EventArgs e)
		{
			mainwindow.Sensitive = true;
			this.Destroy();
		}

		protected void OnButtonCancelClicked (object sender, System.EventArgs e)
		{
			mainwindow.Sensitive = true;
			this.Destroy();
		}

		protected void OnDeleteEvent (object o, Gtk.DeleteEventArgs args)
		{
			mainwindow.Sensitive = true;
		}
	}
}

