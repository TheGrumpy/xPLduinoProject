
// This file has been generated by the GUI designer. Do not modify.
namespace xPLduinoManager
{
	public partial class OneWireProperties
	{
		private global::Gtk.HPaned hpaned1;
		private global::Gtk.ScrolledWindow scrolledwindow1;
		private global::Gtk.TreeView OptionTreeView;
		private global::Gtk.VPaned vpaned1;
		private global::Gtk.VBox vbox1;
		private global::Gtk.HBox hbox2;
		private global::Gtk.Label LabelChildTreeView;
		private global::Gtk.ComboBox ComboboxNumberOfProbe;
		private global::Gtk.Label Labelx;
		private global::Gtk.ComboBox ComboboxTypeOfProbe;
		private global::Gtk.Button ButtonAddProbe;
		private global::Gtk.VSeparator vseparator2;
		private global::Gtk.Button ButtonDeleteProbe;
		private global::Gtk.ScrolledWindow scrolledwindow3;
		private global::Gtk.TreeView ChildTreeView;
		private global::Gtk.VBox vbox4;
		private global::Gtk.Label NoteLabel;
		private global::Gtk.ScrolledWindow scrolledwindow4;
		private global::Gtk.TextView TextViewNote;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget xPLduinoManager.OneWireProperties
			global::Stetic.BinContainer.Attach (this);
			this.Name = "xPLduinoManager.OneWireProperties";
			// Container child xPLduinoManager.OneWireProperties.Gtk.Container+ContainerChild
			this.hpaned1 = new global::Gtk.HPaned ();
			this.hpaned1.CanFocus = true;
			this.hpaned1.Name = "hpaned1";
			this.hpaned1.Position = 159;
			// Container child hpaned1.Gtk.Paned+PanedChild
			this.scrolledwindow1 = new global::Gtk.ScrolledWindow ();
			this.scrolledwindow1.CanFocus = true;
			this.scrolledwindow1.Name = "scrolledwindow1";
			this.scrolledwindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child scrolledwindow1.Gtk.Container+ContainerChild
			this.OptionTreeView = new global::Gtk.TreeView ();
			this.OptionTreeView.CanFocus = true;
			this.OptionTreeView.Name = "OptionTreeView";
			this.scrolledwindow1.Add (this.OptionTreeView);
			this.hpaned1.Add (this.scrolledwindow1);
			global::Gtk.Paned.PanedChild w2 = ((global::Gtk.Paned.PanedChild)(this.hpaned1 [this.scrolledwindow1]));
			w2.Resize = false;
			// Container child hpaned1.Gtk.Paned+PanedChild
			this.vpaned1 = new global::Gtk.VPaned ();
			this.vpaned1.CanFocus = true;
			this.vpaned1.Name = "vpaned1";
			this.vpaned1.Position = 222;
			// Container child vpaned1.Gtk.Paned+PanedChild
			this.vbox1 = new global::Gtk.VBox ();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbox2 = new global::Gtk.HBox ();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			// Container child hbox2.Gtk.Box+BoxChild
			this.LabelChildTreeView = new global::Gtk.Label ();
			this.LabelChildTreeView.Name = "LabelChildTreeView";
			this.LabelChildTreeView.Xalign = 0F;
			this.LabelChildTreeView.LabelProp = global::Mono.Unix.Catalog.GetString ("label1");
			this.hbox2.Add (this.LabelChildTreeView);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.LabelChildTreeView]));
			w3.Position = 0;
			// Container child hbox2.Gtk.Box+BoxChild
			this.ComboboxNumberOfProbe = global::Gtk.ComboBox.NewText ();
			this.ComboboxNumberOfProbe.Name = "ComboboxNumberOfProbe";
			this.hbox2.Add (this.ComboboxNumberOfProbe);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.ComboboxNumberOfProbe]));
			w4.Position = 1;
			w4.Expand = false;
			w4.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.Labelx = new global::Gtk.Label ();
			this.Labelx.Name = "Labelx";
			this.Labelx.LabelProp = global::Mono.Unix.Catalog.GetString ("x");
			this.hbox2.Add (this.Labelx);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.Labelx]));
			w5.Position = 2;
			w5.Expand = false;
			w5.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.ComboboxTypeOfProbe = global::Gtk.ComboBox.NewText ();
			this.ComboboxTypeOfProbe.Name = "ComboboxTypeOfProbe";
			this.hbox2.Add (this.ComboboxTypeOfProbe);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.ComboboxTypeOfProbe]));
			w6.Position = 3;
			w6.Expand = false;
			w6.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.ButtonAddProbe = new global::Gtk.Button ();
			this.ButtonAddProbe.CanFocus = true;
			this.ButtonAddProbe.Name = "ButtonAddProbe";
			this.ButtonAddProbe.UseUnderline = true;
			this.ButtonAddProbe.Label = global::Mono.Unix.Catalog.GetString ("GtkButton");
			this.hbox2.Add (this.ButtonAddProbe);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.ButtonAddProbe]));
			w7.Position = 4;
			w7.Expand = false;
			w7.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.vseparator2 = new global::Gtk.VSeparator ();
			this.vseparator2.Name = "vseparator2";
			this.hbox2.Add (this.vseparator2);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.vseparator2]));
			w8.Position = 5;
			w8.Expand = false;
			w8.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.ButtonDeleteProbe = new global::Gtk.Button ();
			this.ButtonDeleteProbe.CanFocus = true;
			this.ButtonDeleteProbe.Name = "ButtonDeleteProbe";
			this.ButtonDeleteProbe.UseUnderline = true;
			this.ButtonDeleteProbe.Label = global::Mono.Unix.Catalog.GetString ("GtkButton");
			this.hbox2.Add (this.ButtonDeleteProbe);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.ButtonDeleteProbe]));
			w9.Position = 6;
			w9.Expand = false;
			w9.Fill = false;
			this.vbox1.Add (this.hbox2);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox2]));
			w10.Position = 0;
			w10.Expand = false;
			w10.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.scrolledwindow3 = new global::Gtk.ScrolledWindow ();
			this.scrolledwindow3.CanFocus = true;
			this.scrolledwindow3.Name = "scrolledwindow3";
			this.scrolledwindow3.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child scrolledwindow3.Gtk.Container+ContainerChild
			this.ChildTreeView = new global::Gtk.TreeView ();
			this.ChildTreeView.CanFocus = true;
			this.ChildTreeView.Name = "ChildTreeView";
			this.scrolledwindow3.Add (this.ChildTreeView);
			this.vbox1.Add (this.scrolledwindow3);
			global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.scrolledwindow3]));
			w12.Position = 1;
			this.vpaned1.Add (this.vbox1);
			global::Gtk.Paned.PanedChild w13 = ((global::Gtk.Paned.PanedChild)(this.vpaned1 [this.vbox1]));
			w13.Resize = false;
			// Container child vpaned1.Gtk.Paned+PanedChild
			this.vbox4 = new global::Gtk.VBox ();
			this.vbox4.Name = "vbox4";
			this.vbox4.Spacing = 6;
			// Container child vbox4.Gtk.Box+BoxChild
			this.NoteLabel = new global::Gtk.Label ();
			this.NoteLabel.Name = "NoteLabel";
			this.NoteLabel.Xalign = 0F;
			this.vbox4.Add (this.NoteLabel);
			global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.NoteLabel]));
			w14.Position = 0;
			w14.Expand = false;
			w14.Fill = false;
			// Container child vbox4.Gtk.Box+BoxChild
			this.scrolledwindow4 = new global::Gtk.ScrolledWindow ();
			this.scrolledwindow4.CanFocus = true;
			this.scrolledwindow4.Name = "scrolledwindow4";
			this.scrolledwindow4.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child scrolledwindow4.Gtk.Container+ContainerChild
			this.TextViewNote = new global::Gtk.TextView ();
			this.TextViewNote.CanFocus = true;
			this.TextViewNote.Name = "TextViewNote";
			this.scrolledwindow4.Add (this.TextViewNote);
			this.vbox4.Add (this.scrolledwindow4);
			global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.scrolledwindow4]));
			w16.Position = 1;
			this.vpaned1.Add (this.vbox4);
			this.hpaned1.Add (this.vpaned1);
			this.Add (this.hpaned1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
			this.ButtonAddProbe.Clicked += new global::System.EventHandler (this.OnButtonAddProbeClicked);
			this.ButtonDeleteProbe.Clicked += new global::System.EventHandler (this.OnButtonDeleteProbeClicked);
			this.TextViewNote.FocusOutEvent += new global::Gtk.FocusOutEventHandler (this.OnTextViewNoteFocusOutEvent);
		}
	}
}
