
// This file has been generated by the GUI designer. Do not modify.
namespace xPLduinoManager
{
	public partial class NewScenario
	{
		private global::Gtk.HBox hbox2;
		private global::Gtk.Label LabelNameScenario;
		private global::Gtk.Entry EntryScenarioName;
		private global::Gtk.Label LabelError;
		private global::Gtk.Button buttonCancel;
		private global::Gtk.Button buttonOk;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget xPLduinoManager.NewScenario
			this.Name = "xPLduinoManager.NewScenario";
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child xPLduinoManager.NewScenario.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.hbox2 = new global::Gtk.HBox ();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			// Container child hbox2.Gtk.Box+BoxChild
			this.LabelNameScenario = new global::Gtk.Label ();
			this.LabelNameScenario.WidthRequest = 120;
			this.LabelNameScenario.Name = "LabelNameScenario";
			this.LabelNameScenario.Xalign = 0F;
			this.LabelNameScenario.LabelProp = global::Mono.Unix.Catalog.GetString ("Nom du fichier type :");
			this.hbox2.Add (this.LabelNameScenario);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.LabelNameScenario]));
			w2.Position = 0;
			// Container child hbox2.Gtk.Box+BoxChild
			this.EntryScenarioName = new global::Gtk.Entry ();
			this.EntryScenarioName.CanFocus = true;
			this.EntryScenarioName.Name = "EntryScenarioName";
			this.EntryScenarioName.Text = global::Mono.Unix.Catalog.GetString ("NomFichierType");
			this.EntryScenarioName.IsEditable = true;
			this.EntryScenarioName.MaxLength = 16;
			this.EntryScenarioName.InvisibleChar = '●';
			this.hbox2.Add (this.EntryScenarioName);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.EntryScenarioName]));
			w3.Position = 1;
			w1.Add (this.hbox2);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(w1 [this.hbox2]));
			w4.Position = 0;
			w4.Expand = false;
			w4.Fill = false;
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.LabelError = new global::Gtk.Label ();
			this.LabelError.Name = "LabelError";
			this.LabelError.Xalign = 0F;
			w1.Add (this.LabelError);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(w1 [this.LabelError]));
			w5.Position = 1;
			w5.Expand = false;
			w5.Fill = false;
			// Internal child xPLduinoManager.NewScenario.ActionArea
			global::Gtk.HButtonBox w6 = this.ActionArea;
			w6.Name = "dialog1_ActionArea";
			w6.Spacing = 10;
			w6.BorderWidth = ((uint)(5));
			w6.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button ();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			this.AddActionWidget (this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w7 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w6 [this.buttonCancel]));
			w7.Expand = false;
			w7.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button ();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-ok";
			this.AddActionWidget (this.buttonOk, -5);
			global::Gtk.ButtonBox.ButtonBoxChild w8 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w6 [this.buttonOk]));
			w8.Position = 1;
			w8.Expand = false;
			w8.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 400;
			this.DefaultHeight = 89;
			this.Show ();
			this.buttonCancel.Clicked += new global::System.EventHandler (this.OnButtonCancelClicked);
			this.buttonOk.Clicked += new global::System.EventHandler (this.OnButtonOkClicked);
		}
	}
}
