
// This file has been generated by the GUI designer. Do not modify.
namespace xPLduinoManager
{
	public partial class NewBoard
	{
		private global::Gtk.VBox vbox3;
		private global::Gtk.HBox hbox2;
		private global::Gtk.Label LabelName;
		private global::Gtk.Entry EntryBoardName;
		private global::Gtk.HBox hbox3;
		private global::Gtk.Label LabelType;
		private global::Gtk.ComboBox ComboboxTypeBoard;
		private global::Gtk.HBox hbox4;
		private global::Gtk.Label LabelError;
		private global::Gtk.Button buttonCancel;
		private global::Gtk.Button buttonOk;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget xPLduinoManager.NewBoard
			this.Name = "xPLduinoManager.NewBoard";
			this.Title = global::Mono.Unix.Catalog.GetString ("Nouvelle carte");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child xPLduinoManager.NewBoard.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.vbox3 = new global::Gtk.VBox ();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.hbox2 = new global::Gtk.HBox ();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			// Container child hbox2.Gtk.Box+BoxChild
			this.LabelName = new global::Gtk.Label ();
			this.LabelName.WidthRequest = 120;
			this.LabelName.Name = "LabelName";
			this.LabelName.Xalign = 0F;
			this.LabelName.LabelProp = global::Mono.Unix.Catalog.GetString ("Nom de la carte :");
			this.hbox2.Add (this.LabelName);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.LabelName]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.EntryBoardName = new global::Gtk.Entry ();
			this.EntryBoardName.CanFocus = true;
			this.EntryBoardName.Name = "EntryBoardName";
			this.EntryBoardName.Text = global::Mono.Unix.Catalog.GetString ("NomDeLaCarte");
			this.EntryBoardName.IsEditable = true;
			this.EntryBoardName.MaxLength = 16;
			this.EntryBoardName.InvisibleChar = '●';
			this.hbox2.Add (this.EntryBoardName);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.EntryBoardName]));
			w3.Position = 1;
			this.vbox3.Add (this.hbox2);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.hbox2]));
			w4.Position = 0;
			w4.Expand = false;
			w4.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.hbox3 = new global::Gtk.HBox ();
			this.hbox3.Name = "hbox3";
			this.hbox3.Spacing = 6;
			// Container child hbox3.Gtk.Box+BoxChild
			this.LabelType = new global::Gtk.Label ();
			this.LabelType.WidthRequest = 120;
			this.LabelType.Name = "LabelType";
			this.LabelType.Xalign = 0F;
			this.LabelType.LabelProp = global::Mono.Unix.Catalog.GetString ("Type de carte :");
			this.hbox3.Add (this.LabelType);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.LabelType]));
			w5.Position = 0;
			w5.Expand = false;
			w5.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.ComboboxTypeBoard = global::Gtk.ComboBox.NewText ();
			this.ComboboxTypeBoard.Name = "ComboboxTypeBoard";
			this.hbox3.Add (this.ComboboxTypeBoard);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.ComboboxTypeBoard]));
			w6.Position = 1;
			this.vbox3.Add (this.hbox3);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.hbox3]));
			w7.Position = 1;
			w7.Expand = false;
			w7.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.hbox4 = new global::Gtk.HBox ();
			this.hbox4.Name = "hbox4";
			this.hbox4.Spacing = 6;
			// Container child hbox4.Gtk.Box+BoxChild
			this.LabelError = new global::Gtk.Label ();
			this.LabelError.Name = "LabelError";
			this.LabelError.Xalign = 0F;
			this.hbox4.Add (this.LabelError);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.LabelError]));
			w8.Position = 0;
			this.vbox3.Add (this.hbox4);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.hbox4]));
			w9.Position = 2;
			w9.Expand = false;
			w9.Fill = false;
			w1.Add (this.vbox3);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(w1 [this.vbox3]));
			w10.Position = 0;
			w10.Expand = false;
			w10.Fill = false;
			// Internal child xPLduinoManager.NewBoard.ActionArea
			global::Gtk.HButtonBox w11 = this.ActionArea;
			w11.Name = "dialog1_ActionArea";
			w11.Spacing = 10;
			w11.BorderWidth = ((uint)(5));
			w11.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button ();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			this.AddActionWidget (this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w12 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w11 [this.buttonCancel]));
			w12.Expand = false;
			w12.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button ();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-ok";
			this.AddActionWidget (this.buttonOk, -5);
			global::Gtk.ButtonBox.ButtonBoxChild w13 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w11 [this.buttonOk]));
			w13.Position = 1;
			w13.Expand = false;
			w13.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 400;
			this.DefaultHeight = 125;
			this.Show ();
			this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
			this.buttonCancel.Clicked += new global::System.EventHandler (this.OnButtonCancelClicked);
			this.buttonOk.Clicked += new global::System.EventHandler (this.OnButtonOkClicked);
		}
	}
}
