using System;

namespace xPLduinoManager
{
	public partial class AskPassword : Gtk.Dialog
	{
		Param param;
		public string DirectoryName;
		MainWindow mainwindow;
		string Password;
		
		public AskPassword (string _DirectoryName, Param _param, MainWindow _mainwindow)
		{
			this.mainwindow = _mainwindow;
			this.param = _param;
			this.Build ();
			this.DirectoryName = _DirectoryName;
			this.Password = "";
			LabelPassword.Text = param.ParamT("Password");
		}

		protected void OnButtonOkClicked (object sender, System.EventArgs e)
		{
			Password = EntryPassword.Text;
			this.Destroy();
			mainwindow.OpenProject(DirectoryName,Password);
			
		}
	}
}

