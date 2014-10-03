using System;

namespace xPLduinoManager
{
	public partial class ConfirmAction : Gtk.Dialog
	{
		public NewProject widgetnewproject;
		
		public ConfirmAction (string _LabelText, NewProject _widgetnewproject)
		{
			this.Build ();
			LabelText.Text = _LabelText;
			widgetnewproject = _widgetnewproject;
		}

		protected void OnButtonOkClicked (object sender, System.EventArgs e)
		{
			widgetnewproject.ConfirmationOK();
			this.Destroy();
		}

		protected void OnButtonCancelClicked (object sender, System.EventArgs e)
		{
			this.Destroy();
		}
	}
}

