
// This file has been generated by the GUI designer. Do not modify.
namespace xPLduinoManager
{
	public partial class ScenarioEdit
	{
		private global::Gtk.VPaned vpaned3;
		
		private global::Gtk.HPaned hpaned3;
		
		private global::Gtk.ScrolledWindow ScrolledWindowScenario;
		
		private global::Gtk.ScrolledWindow ScrolledWindowFonction;
		
		private global::Gtk.HPaned hpaned2;
		
		private global::Gtk.ScrolledWindow GtkScrolledWindow;
		
		private global::Gtk.TreeView TreeviewVariable;
		
		private global::Gtk.HPaned hpaned4;
		
		private global::Gtk.ScrolledWindow GtkScrolledWindow1;
		
		private global::Gtk.TreeView TreeviewFunction;
		
		private global::Gtk.ScrolledWindow GtkScrolledWindow2;
		
		private global::Gtk.TreeView TreeviewArgsFunction;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget xPLduinoManager.ScenarioEdit
			global::Stetic.BinContainer.Attach (this);
			this.Name = "xPLduinoManager.ScenarioEdit";
			// Container child xPLduinoManager.ScenarioEdit.Gtk.Container+ContainerChild
			this.vpaned3 = new global::Gtk.VPaned ();
			this.vpaned3.CanFocus = true;
			this.vpaned3.Name = "vpaned3";
			this.vpaned3.Position = 443;
			// Container child vpaned3.Gtk.Paned+PanedChild
			this.hpaned3 = new global::Gtk.HPaned ();
			this.hpaned3.CanFocus = true;
			this.hpaned3.Name = "hpaned3";
			this.hpaned3.Position = 483;
			// Container child hpaned3.Gtk.Paned+PanedChild
			this.ScrolledWindowScenario = new global::Gtk.ScrolledWindow ();
			this.ScrolledWindowScenario.CanFocus = true;
			this.ScrolledWindowScenario.Name = "ScrolledWindowScenario";
			this.ScrolledWindowScenario.ShadowType = ((global::Gtk.ShadowType)(1));
			this.hpaned3.Add (this.ScrolledWindowScenario);
			global::Gtk.Paned.PanedChild w1 = ((global::Gtk.Paned.PanedChild)(this.hpaned3 [this.ScrolledWindowScenario]));
			w1.Resize = false;
			// Container child hpaned3.Gtk.Paned+PanedChild
			this.ScrolledWindowFonction = new global::Gtk.ScrolledWindow ();
			this.ScrolledWindowFonction.CanFocus = true;
			this.ScrolledWindowFonction.Name = "ScrolledWindowFonction";
			this.ScrolledWindowFonction.ShadowType = ((global::Gtk.ShadowType)(1));
			this.hpaned3.Add (this.ScrolledWindowFonction);
			this.vpaned3.Add (this.hpaned3);
			global::Gtk.Paned.PanedChild w3 = ((global::Gtk.Paned.PanedChild)(this.vpaned3 [this.hpaned3]));
			w3.Resize = false;
			// Container child vpaned3.Gtk.Paned+PanedChild
			this.hpaned2 = new global::Gtk.HPaned ();
			this.hpaned2.CanFocus = true;
			this.hpaned2.Name = "hpaned2";
			this.hpaned2.Position = 483;
			// Container child hpaned2.Gtk.Paned+PanedChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.TreeviewVariable = new global::Gtk.TreeView ();
			this.TreeviewVariable.CanFocus = true;
			this.TreeviewVariable.Name = "TreeviewVariable";
			this.GtkScrolledWindow.Add (this.TreeviewVariable);
			this.hpaned2.Add (this.GtkScrolledWindow);
			global::Gtk.Paned.PanedChild w5 = ((global::Gtk.Paned.PanedChild)(this.hpaned2 [this.GtkScrolledWindow]));
			w5.Resize = false;
			// Container child hpaned2.Gtk.Paned+PanedChild
			this.hpaned4 = new global::Gtk.HPaned ();
			this.hpaned4.CanFocus = true;
			this.hpaned4.Name = "hpaned4";
			this.hpaned4.Position = 242;
			// Container child hpaned4.Gtk.Paned+PanedChild
			this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
			this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
			this.TreeviewFunction = new global::Gtk.TreeView ();
			this.TreeviewFunction.CanFocus = true;
			this.TreeviewFunction.Name = "TreeviewFunction";
			this.GtkScrolledWindow1.Add (this.TreeviewFunction);
			this.hpaned4.Add (this.GtkScrolledWindow1);
			global::Gtk.Paned.PanedChild w7 = ((global::Gtk.Paned.PanedChild)(this.hpaned4 [this.GtkScrolledWindow1]));
			w7.Resize = false;
			// Container child hpaned4.Gtk.Paned+PanedChild
			this.GtkScrolledWindow2 = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow2.Name = "GtkScrolledWindow2";
			this.GtkScrolledWindow2.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow2.Gtk.Container+ContainerChild
			this.TreeviewArgsFunction = new global::Gtk.TreeView ();
			this.TreeviewArgsFunction.CanFocus = true;
			this.TreeviewArgsFunction.Name = "TreeviewArgsFunction";
			this.GtkScrolledWindow2.Add (this.TreeviewArgsFunction);
			this.hpaned4.Add (this.GtkScrolledWindow2);
			this.hpaned2.Add (this.hpaned4);
			this.vpaned3.Add (this.hpaned2);
			this.Add (this.vpaned3);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
			this.TreeviewVariable.ButtonReleaseEvent += new global::Gtk.ButtonReleaseEventHandler (this.OnTreeviewVariableButtonReleaseEvent);
			this.TreeviewFunction.ButtonReleaseEvent += new global::Gtk.ButtonReleaseEventHandler (this.OnTreeviewFunctionButtonReleaseEvent);
		}
	}
}
