
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
		private global::Gtk.Label LabelChildTreeView;
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
			this.LabelChildTreeView = new global::Gtk.Label ();
			this.LabelChildTreeView.Name = "LabelChildTreeView";
			this.LabelChildTreeView.Xalign = 0F;
			this.LabelChildTreeView.LabelProp = global::Mono.Unix.Catalog.GetString ("label1");
			this.vbox1.Add (this.LabelChildTreeView);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.LabelChildTreeView]));
			w3.Position = 0;
			w3.Expand = false;
			w3.Fill = false;
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
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.scrolledwindow3]));
			w5.Position = 1;
			this.vpaned1.Add (this.vbox1);
			global::Gtk.Paned.PanedChild w6 = ((global::Gtk.Paned.PanedChild)(this.vpaned1 [this.vbox1]));
			w6.Resize = false;
			// Container child vpaned1.Gtk.Paned+PanedChild
			this.vbox4 = new global::Gtk.VBox ();
			this.vbox4.Name = "vbox4";
			this.vbox4.Spacing = 6;
			// Container child vbox4.Gtk.Box+BoxChild
			this.NoteLabel = new global::Gtk.Label ();
			this.NoteLabel.Name = "NoteLabel";
			this.NoteLabel.Xalign = 0F;
			this.vbox4.Add (this.NoteLabel);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.NoteLabel]));
			w7.Position = 0;
			w7.Expand = false;
			w7.Fill = false;
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
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.scrolledwindow4]));
			w9.Position = 1;
			this.vpaned1.Add (this.vbox4);
			this.hpaned1.Add (this.vpaned1);
			this.Add (this.hpaned1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
			this.TextViewNote.FocusOutEvent += new global::Gtk.FocusOutEventHandler (this.OnTextViewNoteFocusOutEvent);
		}
	}
}
