/*
    xPLduino - Manager
    Copyright (C) 2014  VOGEL Ludovic - xplduino@gmail.com

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using Gtk;
using Gdk;
using System.Collections.Generic;
using System.Diagnostics ;
using System.IO;
using System.Threading;

using System.Collections;
using System.Text;
using System.Net;
using System.Net.Sockets;


namespace xPLduinoManager
{
	//Classe MainWindows
	//Classe permettant de gerer la fenetre principale où se déroulera l'ensemble des action faite au projet
	//Eléments :
	//	DataManagement datamanagement : permet d'acceder au donnée du datamanagement	
	//	Param param : permet d'accéder à tous les paramètres
	//	List<string> ListeTitleTab : Nous allons sauvegarder les titres des tabulations
	//		public global :
	//			Gtk.TreeViewColumn ExplorerColumn : 1ere colonne => logo + text
	//			Gtk.TreeViewColumn TypeColumn : 2eme colonne => Type de l'objet
 	//			Gtk.TreeViewColumn NameColumn : 3eme colonne => Nom de l'objet
	//			Gtk.TreeViewColumn IdColumn : 4eme colonne => Id de l'objet
	//			
	//			Gtk.CellRendererPixbuf ExplorerPixCell : Premiere cellule d'ExplorerColumn (Logo)
	//			Gtk.CellRendererText ExplorerCell : Seconde cellule d'ExplorerColumn (Texte)
	//			Gtk.CellRendererText TypeCell : Cellule de TypeColumn (Texte)
	//			Gtk.CellRendererText NameCell : Cellule de NameColumn (Texte)
	//			Gtk.TreeViewColumn IdColumn : Cellule de IdColumn (Texte)
	//
	//			Gtk.TreeStore ExplorerListStore : store contenant l'ensemble des données
	//
	//			Gdk.Pixbuf PngFolder : logo dans ExplorerTreeView
	//
	//			Gtk.TreeIter IterProject : Iteration project pour la construction de l'explorer treeview
	//			Gtk.TreeIter IterSoftware : Idem
	//			Gtk.TreeIter IterNode : Idem
	//			Gtk.TreeIter IterNetworks : Idem
	//			Gtk.TreeIter IterNetwork : Idem
	//
	//			Gtk.Menu RightClicMenuExplorerTreeView : Permet de faire un menu lors d'un clic droit souris dans l'explorer Treeview
	//			Gtk.MenuItem RightClicMenuItemExplorerTreeView : Permet de construire le menu
	//			Gtk.TreeModel TreeModelExplorerTreeView : Permet de faire l'aqcuisition des données d'un treeview lors d'un clic
	//
	//			Gtk.TreeViewColumn LogoColumn : Colonne permettant d'afficher un logo d'information
	//			Gtk.TreeViewColumn InfoColumn : Colonne permettant d'afficher un texte d'information
	//
	//			Gtk.CellRendererPixbuf LogoPixCell : Cellule contenant le logo d'information
	//			Gtk.CellRendererText InfoCell : Cellule contenant l'information
	//
	//			Gtk.TreeStore OutputListStore : Création du store pour l'output treeview
	//
	//		public static :
	//			string TreeViewEplorerValCol1 : Valeur récupere de la colonne 1 
	//			string TreeViewEplorerValCol2 : Valeur récupere de la colonne 2
	//			string TreeViewEplorerValCol3 : Valeur récupere de la colonne 3 
	//			string TreeViewEplorerValCol4 : Valeur récupere de la colonne 4 	 
	//Fonctions :
	//  InitMainWindow : modifier des informations de la mainwindow	
	//	InitExplorerTreeView : Fonction permettant d'initialiser le treeview
	//	UpdateEplorerTreeView : Fonction permettant de mettre à jour l'explorerTreeview
	//	OnExplorerTreeViewButtonReleaseEvent : fonction implementer sur clic de la souris sur l'explorer TreeView
	//		CreateNewNode : Fonction appelant une fenetre pour la création d'un nouveau noeud dans  le projet
	//		CreateNewNetwork : Fonction appelant une fenetre pour la création d'un nouveau réseau dans un noeud
	//		DeleteProject : Fonction permettant l'autorisation et la suppression d'un projet
	//		DeleteNode : Fonction permettant l'autorisation et la suppression d'un noeud
	//		DeleteNetwork : Fonction permettant l'autorisation et la suppression d'un réseau
	//		DeleteBoard : Fonction permettant l'autorisation et la suppression d'une carte
	//	AddTabInMainNoteBook : permet d'ajouter un nouvel onglet dans le mainbook	
	//	InitOutputTreeview : Création de l'output treeview où nous pourrons afficher des informations pour l'utilisateur final
	//	OnDeleteEvent : permet de fermer la fenetre principal
	//	OnFileActionActivated : action sur clic New Project
	//	InitPanedAndMouvementAuthor : Autorisation des mouvements des paned
	//	ReturnVpanedPosition : Permet de retourner la valeur de la position du vpaned
	// 	ReturnHpanedPosition : Permet de retourner la valeur de la position du hpaned
	//	OnWidgetEventAfter : Mise à jour du scroll output treeview et blocage des paned
	public partial class MainWindow: Gtk.Window
	{	
		bool ProgDebug = false;
		bool ErrorPreference = false;
		
		public DataManagement datamanagement;
		public Param param;
		public Preference pref;
		public int WinWidth;
		public int WinHeight;
		public List<Notebook> Listnotebook;
		public List<OutputTreeView> ListOutputTreeview;
		public List<HistoricTreeView> ListHistoricTreeview;
		
		public TreePath TreeviewPath;
		
		//Liste des widget permettant la mise à jour automatique des fenêtre
		public List<ProjectProperties> ListProjectProperties;
		public List<InstanceProperties> ListInstanceProperties;
		public List<BoardI2CProperties> ListBoardI2CProperties;
		public List<NodeProperties> ListNodeProperties;
		public List<I2CProperties> ListI2CProperties;
		public List<OneWireProperties> ListOneWireProperties;
		public List<CustomerEdit> ListCustomerEdit;
		public List<ScenarioEdit> ListScenarioEdit;
		
		//Mise en place des variable pour l'explorer TreeView
		public global::Gtk.TreeViewColumn ExplorerColumn = new Gtk.TreeViewColumn ();	
		public global::Gtk.TreeViewColumn TypeColumn = new Gtk.TreeViewColumn ();
		public global::Gtk.TreeViewColumn NameColumn = new Gtk.TreeViewColumn ();
		public global::Gtk.TreeViewColumn IdColumn = new Gtk.TreeViewColumn ();	
		public global::Gtk.TreeViewColumn ExInfoColumn = new Gtk.TreeViewColumn ();	
		
		public global::Gtk.CellRendererPixbuf ExplorerPixCell = new Gtk.CellRendererPixbuf();
		public global::Gtk.CellRendererText ExplorerCell = new Gtk.CellRendererText();	
		public global::Gtk.CellRendererText TypeCell = new Gtk.CellRendererText();	
		public global::Gtk.CellRendererText NameCell = new Gtk.CellRendererText();	
		public global::Gtk.CellRendererText IdCell = new Gtk.CellRendererText();	
		public global::Gtk.CellRendererText ExInfoCell = new Gtk.CellRendererText();
		
		public global::Gtk.TreeStore ExplorerListStore;
		
		public global::Gdk.Pixbuf PngFolder;
		public global::Gdk.Pixbuf PngProject;
		public global::Gdk.Pixbuf PngNode;
		public global::Gdk.Pixbuf PngNetwork;
		public global::Gdk.Pixbuf PngLighting;
		public global::Gdk.Pixbuf PngSwitch;
		public global::Gdk.Pixbuf PngShutter;
		public global::Gdk.Pixbuf PngBoard;
		public global::Gdk.Pixbuf PngEmpty;
		public global::Gdk.Pixbuf PngCustomer;
		public global::Gdk.Pixbuf PngCustomerUse;
		public global::Gdk.Pixbuf PngScenario;
		
		public global::Gtk.TreeIter IterProject;
		public global::Gtk.TreeIter IterNode;
		public global::Gtk.TreeIter IterNetworks;
		public global::Gtk.TreeIter IterNetwork;
		public global::Gtk.TreeIter IterInstance;
		public global::Gtk.TreeIter IterLighting;
		public global::Gtk.TreeIter IterSwitch;
		public global::Gtk.TreeIter IterShutter;
		public global::Gtk.TreeIter IterCustomer;
		public global::Gtk.TreeIter IterScenario;
		public global::Gtk.TreeIter IterFile;		
		
		public global::Gtk.Menu RightClicMenuExplorerTreeView;
		public global::Gtk.MenuItem RightClicMenuItemExplorerTreeView;
		public global::Gtk.TreeModel TreeModelExplorerTreeView;	
		
		//Mise en place des variables pour l'output treeview
		public global::Gtk.TreeViewColumn LogoColumn = new Gtk.TreeViewColumn ();
		public global::Gtk.TreeViewColumn DateAndTimeColumn = new Gtk.TreeViewColumn ();
		public global::Gtk.TreeViewColumn InfoColumn = new Gtk.TreeViewColumn ();
		
		public global::Gtk.CellRendererPixbuf LogoPixCell = new Gtk.CellRendererPixbuf ();	
		public global::Gtk.CellRendererText DateAndTimeCell = new  Gtk.CellRendererText();
		public global::Gtk.CellRendererText InfoCell = new Gtk.CellRendererText();	
		
		public global::Gtk.TreeStore OutputListStore;
		
		//Mise en place des variables pour l'historic treeview
		public global::Gtk.TreeViewColumn LogoViewColumn = new Gtk.TreeViewColumn ();
		public global::Gtk.TreeViewColumn NumberViewColumn = new Gtk.TreeViewColumn ();
		public global::Gtk.TreeViewColumn DateAndTimeHistoricColumn = new Gtk.TreeViewColumn ();
		public global::Gtk.TreeViewColumn InfoHistoricColumn = new Gtk.TreeViewColumn ();
		
		public global::Gtk.CellRendererPixbuf LogoViewCell = new Gtk.CellRendererPixbuf ();
		public global::Gtk.CellRendererText NumberViewCell = new Gtk.CellRendererText();	
		public global::Gtk.CellRendererText DateAndTimeHistoricCell = new Gtk.CellRendererText();	
		public global::Gtk.CellRendererText InfoHistoricCell = new Gtk.CellRendererText();	
		
		public global::Gtk.TreeStore HistoricListStore;
		public global::Gtk.TreeModel TreeModelHistoricTreeView;
		
		//Mise en place en place des variable de mémorisation de l'arbre principal
		public string TreeViewEplorerValCol1 = "";
		public string TreeViewEplorerValCol2 = "";
		public string TreeViewEplorerValCol3 = "";
		public string TreeViewEplorerValCol4 = "";	
		
		public global::Gtk.TreeIter IterOutput;
		
		public Gtk.Image ButtonFilterInfoImage;
		public Gtk.Image ButtonFilterWarningImage;
		public Gtk.Image ButtonFilterErrorImage;
		public Gtk.Image ButtonFilterQuestionImage;
		public Gtk.Image ButtonEraseImage;			
			
		public int MemorisePositionInComboboxSelectNode = 0;
		
	    public string lastReceivedUDPPacket="";
	    public string allReceivedUDPPackets="";
		public UdpClient client; 		
		
		//Constructeur de la classe MainWindows
		//Arguments :
		//	DataManagement _datamanagement : permet de travaillé avec les données de datamanagement
		public MainWindow (DataManagement _datamanagement, Param _param, Preference _pref, bool _ErrorPreference): base (Gtk.WindowType.Toplevel)
		{
			this.datamanagement = _datamanagement;
			this.param = _param;
			this.pref = _pref;
			this.ErrorPreference = _ErrorPreference;
			Listnotebook = new List<Notebook>();
			ListOutputTreeview = new List<OutputTreeView>();
			ListHistoricTreeview = new List<HistoricTreeView>();
			TreeviewPath = new TreePath();
			
			ListProjectProperties = new List<ProjectProperties>();
			ListInstanceProperties = new List<InstanceProperties>();
			ListBoardI2CProperties = new List<BoardI2CProperties>();
			ListNodeProperties = new List<NodeProperties>();
			ListI2CProperties = new List<I2CProperties>();
			ListOneWireProperties = new List<OneWireProperties>();
			ListCustomerEdit = new List<CustomerEdit>();
			ListScenarioEdit = new List<ScenarioEdit>();
			
			Build ();
			InitMainWindow();
			ExplorerTreeView.ButtonPressEvent += new ButtonPressEventHandler(ExplorerTreeViewButtonPress); //Permet de d'implémenter le clic et double clic sur l'explorertreeview
			ExplorerTreeView.ButtonReleaseEvent += new ButtonReleaseEventHandler(ExplorerTreeViewButtonRelease);
		}
		
//################ Main Window #####################################		
		
		//Fonction InitMainWindow
		//Fcontion permettant de mettre à jour des information de la main windows à l'initialisation
		public void InitMainWindow()
		{
			this.Title = param.ParamT("MWTitle"); //Mise à jour du titre
						
			this.LabelTreeView.Text = param.ParamT("ExTVTitle");
			InitExplorerTreeView(); //Nous initialisons l'explorateur de projet
			InitOutputTreeview(); //Nous initialisons l'explorateur de sortie
			InitHistoricTreeview(); //Nous initialisons l'arbre de l'historic
			
			//Mise à jour du menu
			this.FileAction.Label = param.ParamT("MWFileMenu"); //Mise à jour du label nouveau fichier
			this.newAction.Label = param.ParamT("MWNewProjectMenu"); //Mise à jour label du nouveau projet
			this.openAction.Label = param.ParamT("MWOpenProjectMenu"); //Mise à jour label ouvrir projet
			this.ToolsAction.Label = param.ParamT("MWTools"); //Mise à jour label outil	
			this.FentreAction.Label = param.ParamT("MWWindow"); //Mise à jour label fenetre
			this.goForwardAction.Label = param.ParamT("MWNextWindow"); //Mise à jour label fenetre suivante
			this.goBackAction.Label = param.ParamT("MWPrevWindow"); //Mise à jour label fenetre précédente
			this.closeAction.Label = param.ParamT("MWDelWindow"); //Mise à jour label supprimer fenêtre
			this.EditAction.Label = param.ParamT("MWEdit"); //Mise à jour label Edition
			this.undoAction.Label = param.ParamT("MWUndo"); //Mise à jour label Annuler
			this.redoAction.Label = param.ParamT("MWRedo"); //Mise à jour label Rétablir
			this.saveAction.Label = param.ParamT("MWSaveProject"); //Mise à jour label Sauvegarder projet
			this.saveAsAction.Label = param.ParamT("MWSaveAllProject"); //Mise à jour label Tout Sauvegarder 
			this.HelpAction.Label = param.ParamT("MWHelp"); //Mise à jour label Aide
			this.aboutAction.Label = param.ParamT("MWAbout"); //Mise à jour label A propos
			
			this.ExtractEmbeddedAction.Label = param.ParamT("MWExtractEmbedded"); //Mise à jour label Extraction embarquée  
			this.LoadEmbeddedAction.Label = param.ParamT("MWLoadEmbedded"); //Mise à jour label Charger embarquée 
			
			this.LabelInfoProject.Text = "";
			this.LabelPathProject.Text = "";
			
//################################# TOOLTIP BUTTON #################################################			
			
			ButtonNewProject.TooltipText = param.ParamT("MWNewProjectMenu") + " (Ctrl + N)";
			ButtonOpenProject.TooltipText = param.ParamT("MWOpenProjectMenu") + " (Ctrl + O)";
			ButtonSaveProject.TooltipText = param.ParamT("MWSaveProjectMenu") + " (Ctrl + S)";
			ButtonSaveAllProject.TooltipText = param.ParamT("MWSaveAllProjectMenu") + " (Ctrl + D)";	
			ButtonCheckEmbedded.TooltipText = param.ParamT("MWVerifyEmbedded") + " (Ctrl + R)";
			ButtonCheckEmbedded.Sensitive = false;
			ButtonLoadEmbedded.TooltipText = param.ParamT("MWLoadEmbedded");	
			ButtonLoadEmbedded.Sensitive = false;
			ButtonReloadUSB.TooltipText = param.ParamT("MWReloadUSB");
			
//##################################################################################################				
			
			//Mise à jour du ViewNoteBook
			OutputTab.Text = param.ParamT("VNBOutputTab");
			HistoryTab.Text = param.ParamT("VNBHistoryTab");	
			LabelHistoric.Text = param.ParamT("VNBHistoryLabel");
			
			//Mise à jour de l'output
			this.LabelInformation.Text = param.ParamT("MWLabelInformation"); //Mise à jour du label Information

			//Mise à jour des boutons output
			ButtonFilterInfoImage = new Gtk.Image(param.ParamP("LogoOutputInformation"), IconSize.Button);
			this.ButtonFilterInfo.Image = ButtonFilterInfoImage;
				
			ButtonFilterWarningImage = new Gtk.Image(param.ParamP("LogoOutputWarning"), IconSize.Button);
			this.ButtonFilterWarning.Image = ButtonFilterWarningImage;
				
			ButtonFilterErrorImage = new Gtk.Image(param.ParamP("LogoOutputError"), IconSize.Button);
			this.ButtonFilterError.Image = ButtonFilterErrorImage;
				
			ButtonFilterQuestionImage = new Gtk.Image(param.ParamP("LogoOutputQuestion"), IconSize.Button);
			this.ButtonFilterQuestion.Image = ButtonFilterQuestionImage;
			
			EraseOutput.TooltipText = param.ParamT("TextOutputErase");
			ButtonEraseImage = new Gtk.Image(param.ParamP("LogoErase"), IconSize.Button);
			this.EraseOutput.Image = ButtonEraseImage;
			
			//Mise à jour des boutons de l'explorer treeview
			Up.TooltipText = param.ParamT("ExTVUpButton");
			Down.TooltipText = param.ParamT("ExTVDownButton");
			Reduce.TooltipText = param.ParamT("ExTVReduceWindow");
			Expand.TooltipText = param.ParamT("ExTVExpandWindow");
			
			//Mise à jour Bouton New
			VBoxButtonNews.Visible = false;				
			
			this.Maximize(); //Nous maximison la fenêtre pour quelle occupe l'ensemble de l'espace de l'écran du PC
			
			this.GetSize(out WinWidth , out WinHeight);	//On fait l'acquisition de la taille de la fenêtre que nous stockons dans deux variables	
			hpaned1.Position = 0; //Permet de masquer le hpaned au démarrage de l'application
			vpaned1.Position = WinHeight; //permet de masquer le vpaned au démarrage
			
			
			AddLineOutput(param.ParamI("OutputInformation"),"OutputWelcomeLine");	//Nous ajoutons une ligne dans la sortie
			
			if(pref.DisplayWelcomeTab)
				AddTabInMainNoteBook(param.ParamT("TabWelcomeTitle"),new Welcome(),param.ParamP("IconHome"),"HOME","HOME"); //Nous ajoutons un nouvel onglet (la page d'acceuil)	
		
			scrolledwindow3.Visible = false;
			vbox2.Visible = false;
			ViewNoteBook.Visible = false;
			
			SecondNoteBook.GroupId = 1;
			MainNoteBook.GroupId = 1;
			
			
			//Dans le cas où nous avons une erreur dans le fichier de préférence
			if(!ErrorPreference)
			{
				this.Sensitive = false; //Permet d'incativer la fenêtre principal
				if(pref.BeepOnDelete)
					System.Media.SystemSounds.Question.Play(); //Avertir du popup
				//Boite de dialogue permettant de valider la suppression d'un projet
				MessageDialog message = new MessageDialog (this, 
	                                      			  	   DialogFlags.DestroyWithParent,
		                              				       MessageType.Question, 
	                                                       ButtonsType.YesNo, param.ParamT("QuestionErrorPreference"));
	            
				ResponseType messageresult = (ResponseType)message.Run (); //On lance la box
					
				if (messageresult == ResponseType.Yes) //On regarde le résultat et si Oui
				{
					this.Sensitive = true; //Permet d'incativer la fenêtre principal
					new ChangePreferences(param,this,pref,datamanagement);	
					message.Destroy();//On détruit la boite de dialogue
				}
				else
				{
					this.Sensitive = true; //Permet d'incativer la fenêtre principal
					message.Destroy();//On détruit la boite de dialogue
				}
			}
			
			//Nous rendons les boutons Annuler et rétablir inactif
			UndoRedoInactif("undo",false);
			UndoRedoInactif("redo",false);
		
			
			ExpendHistoryPanel.Visible = false;
			ReduceHistoryPanel.Visible = false;
			
			/*
			Process p = new Process();
			p.StartInfo.FileName = "http://xplduino.org";
			p.Start();*/		
			
			//TextEdit te = new TextEdit("toto","text/x-csharp");
			//AddTabInMainNoteBook("customer.ino",te.widget,param.ParamP("IconFolder"),"TEST","TEST"); //Nous ajoutons un nouvel onglet (la page d'acceuil)				
			//AddTabInMainNoteBook("customer.ino",new CustomerEdit(),param.ParamP("IconFolder"),"TEST","TEST");				
		}	
		
		//Fonction UpdateMainWindow
		//Fonction permettant de mettre à jour la fenêtre principal dans le cas où nous changeons de langue
		public void UpdateMainWindow()
		{
			this.Title = param.ParamT("MWTitle"); //Mise à jour du titre
						
			this.LabelTreeView.Text = param.ParamT("ExTVTitle");
			UpdateEplorerTreeView(); //Nous initialisons l'explorateur de projet
	
			//Mise à jour du menu
			this.FileAction.Label = param.ParamT("MWFileMenu"); //Mise à jour du label nouveau fichier
			this.newAction.Label = param.ParamT("MWNewProjectMenu"); //Mise à jour label du nouveau projet
			this.openAction.Label = param.ParamT("MWOpenProjectMenu"); //Mise à jour label ouvrir projet
			this.ToolsAction.Label = param.ParamT("MWTools"); //Mise à jour label outil	
			this.FentreAction.Label = param.ParamT("MWWindow"); //Mise à jour label fenetre
			this.goForwardAction.Label = param.ParamT("MWNextWindow"); //Mise à jour label fenetre suivante
			this.goBackAction.Label = param.ParamT("MWPrevWindow"); //Mise à jour label fenetre précédente
			this.closeAction.Label = param.ParamT("MWDelWindow"); //Mise à jour label supprimer fenêtre
			this.EditAction.Label = param.ParamT("MWEdit"); //Mise à jour label Edition
			this.undoAction.Label = param.ParamT("MWUndo"); //Mise à jour label Annuler
			this.redoAction.Label = param.ParamT("MWRedo"); //Mise à jour label Rétablir	
			this.saveAction.Label = param.ParamT("MWSaveProject"); //Mise à jour label Sauvegarder projet
			this.saveAsAction.Label = param.ParamT("MWSaveAllProject"); //Mise à jour label Tout Sauvegarder			
			this.ExtractEmbeddedAction.Label = param.ParamT("MWExtractEmbedded"); //Mise à jour label Extraction embarquée 
			this.LoadEmbeddedAction.Label = param.ParamT("MWLoadEmbedded"); //Mise à jour label Charger embarquée 			
			this.HelpAction.Label = param.ParamT("MWHelp"); //Mise à jour label A propos
			this.aboutAction.Label = param.ParamT("MWAbout"); //Mise à jour label A propos
			
//################## BUTTON MENU ##########################################	
			
			ButtonNewProject.TooltipText = param.ParamT("MWNewProjectMenu") + " (Ctrl + N)";
			ButtonOpenProject.TooltipText = param.ParamT("MWOpenProjectMenu") + " (Ctrl + O)";	
			ButtonSaveProject.TooltipText = param.ParamT("MWSaveProjectMenu") + " (Ctrl + S)";
			ButtonSaveAllProject.TooltipText = param.ParamT("MWSaveAllProjectMenu") + " (Ctrl + D)";		
			ButtonCheckEmbedded.TooltipText = param.ParamT("MWVerifyEmbedded") + " (Ctrl + R)";
			ButtonLoadEmbedded.TooltipText = param.ParamT("MWLoadEmbedded");
			
//################## VIEW NOTEBOOK ##########################################		
			
			//Mise à jour de l'output
			this.LabelInformation.Text = param.ParamT("MWLabelInformation"); //Mise à jour du label Information
			
			EraseOutput.TooltipText = param.ParamT("TextOutputErase");			
			
			//Mise à jour des boutons de l'explorer treeview
			Up.TooltipText = param.ParamT("ExTVUpButton");
			Down.TooltipText = param.ParamT("ExTVDownButton");
			Reduce.TooltipText = param.ParamT("ExTVReduceWindow");
			Expand.TooltipText = param.ParamT("ExTVExpandWindow");				
			
			//Mise à jour du ViewNoteBook
			OutputTab.Text = param.ParamT("VNBOutputTab");
			HistoryTab.Text = param.ParamT("VNBHistoryTab");
			LabelHistoric.Text = param.ParamT("VNBHistoryLabel");		
			//ViewNoteBook.CurrentPage = 0;	
			
			//Mise à jour des titre dans l'arbre output
			DateAndTimeColumn.Title = param.ParamT ("OutTVDateAndTimeValue");
			InfoColumn.Title = param.ParamT("OutTVTitleValue");	

			//Mise à jour des titre dans l'abre des historique
			LogoViewColumn.Title = param.ParamT("HTVView");
			NumberViewColumn.Title = param.ParamT("HTVNumberView");
			DateAndTimeHistoricColumn.Title = param.ParamT("HTVDateAndTime");
			InfoHistoricColumn.Title = param.ParamT("HTVInfo");					
		}
				
//################ Explorer Treeview #####################################				
		
		//Fonction InitExplorerTreeView
		//Fonction permettant d'initialiser ExplorerTreeView, ceci sera fait une seul fois à la construction de la classe
		//Pas d'argument
		public void InitExplorerTreeView ()
		{
			//Nous permettons la visualisation des lignes dans le treeview
			ExplorerTreeView.EnableTreeLines = true;
			ExplorerTreeView.HeadersVisible = false;
			//ExplorerTreeView.HeadersClickable = true;
		
			
			//Nouveau Logo que nous allons afficher à gauche dans l'explorateur de projet
			PngFolder = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconFolder"));
			PngProject = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconProject"));
			PngNode = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconNode"));
			PngNetwork = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconNetwork"));
			PngLighting = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconLighting"));
			PngSwitch = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconSwitch"));
			PngShutter = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconShutter"));
			PngBoard = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconBoard"));
			PngEmpty = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconEmpty"));
			PngCustomer = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconCustomer"));
			PngCustomerUse = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconCustomerUse"));
			PngScenario = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconScenario"));
			
			//Nous donnons un titre au colonnes
			ExplorerColumn.Title = param.ParamT("ExTVTitle");
			TypeColumn.Title = "Type";
			NameColumn.Title = "Name";
			IdColumn.Title = "Id";
			ExInfoColumn.Title = "";
			
			//On associe des cellules aux colonnes
			ExplorerColumn.PackStart(ExplorerPixCell, false);
			ExplorerColumn.PackStart(ExplorerCell, true);
			TypeColumn.PackStart (TypeCell, true);
			NameColumn.PackStart (NameCell, true);
			IdColumn.PackStart (IdCell,true);	
			ExInfoColumn.PackStart (ExInfoCell,true);	
			
			//Visibilité des colonnes (Invisble mais lecture celullue possible)
			TypeColumn.Visible = ProgDebug;
			NameColumn.Visible = ProgDebug;
			IdColumn.Visible = ProgDebug;			
					
			//Ajout des colonnes dans ExplorerTreeView
			ExplorerTreeView.AppendColumn (ExplorerColumn);
			ExplorerTreeView.AppendColumn (TypeColumn);		
			ExplorerTreeView.AppendColumn (NameColumn);			
			ExplorerTreeView.AppendColumn (IdColumn);
			ExplorerTreeView.AppendColumn (ExInfoColumn);
			
			//Ajout des attibut à chaque colonne
			ExplorerColumn.AddAttribute (ExplorerPixCell, "pixbuf", param.ParamI("ExTVPositionPixBuff"));
			ExplorerColumn.AddAttribute(ExplorerCell,"text",param.ParamI("ExTVPositionTexte"));
			NameColumn.AddAttribute (NameCell, "text", param.ParamI("ExTVPositionName"));
			TypeColumn.AddAttribute (TypeCell, "text", param.ParamI("ExTVPositionType"));
			IdColumn.AddAttribute (IdCell, "text", param.ParamI("ExTVPositionId"));	
			
			ExplorerListStore = new Gtk.TreeStore (typeof (Gdk.Pixbuf),typeof (string),typeof (string),typeof (string),typeof (string));	
			
			//On affiche l'explorer de projet
			ExplorerTreeView.ShowAll ();			
		}
		
		//Fonction UpdateExplorerTreeView()
		//Fonction permettant de mettre à jour l'explorer treeview
		//Pas d'argument
		public void UpdateEplorerTreeView()
		{
			string HeaderProjectName = "";
			ExplorerListStore.Clear(); //On efface EplorerTreeView
			foreach(Project Pro in datamanagement.ListProject) //Pour chaque Projet de la liste des projets
			{
				IterProject = ExplorerListStore.AppendValues(PngProject,Pro.Project_Name + " (Version : " + Pro.Project_Version + ")",param.ParamP("ExTVTypeProject"),Pro.Project_Name,Pro.Project_Id.ToString()); //On affiche les projets
				if(Pro.ReturnListNode().Count > 0) //Dans le cas où le nombre de noeud par projet et supérieur à 0
				{
					foreach(Node Nod in Pro.ReturnListNode()) //Pour chaque noeud présent dans la liste des noeud d'un projet
					{
						IterNode = ExplorerListStore.AppendValues(IterProject,PngNode,Nod.Node_Name,param.ParamP("ExTVTypeNode"),Nod.Node_Name,Nod.Node_Id.ToString()); //On affiche le noeud
						if(Nod.ReturnListNetwork().Count > 0) //Si la liste des réseau pour un noeud est supérieur à 0
						{
							IterNetworks = ExplorerListStore.AppendValues(IterNode,PngFolder,param.ParamT("ExTVNameNetwork")); //On affiche le conteneur des réseaux
							foreach(Network Net in Nod.ReturnListNetwork()) //Pour chaque réseau présent dans la liste des noeud pour un réseau
							{
								IterNetwork = ExplorerListStore.AppendValues(IterNetworks,PngNetwork,Net.Network_Type,Net.Network_Type,param.ParamP("ExTVTypeNetwork"),Net.Network_Id.ToString());//On affiche la liste des réseaux
								if(Net.ReturnListBoard().Count > 0) //Si le nombre de carte est supérieur à 0 dans le listes des carte pour un réseau
								{
									foreach(Board Boa in Net.ReturnListBoard()) //Pour chaque carte dans la liste des carte pour un réseau
									{
										ExplorerListStore.AppendValues(IterNetwork,PngBoard,Boa.Board_Name + " (" + Boa.Board_Type + ")", Boa.Board_Type,Boa.Board_Name,Boa.Board_Id.ToString());//On affiche la liste des cartes
									}
								}
							}
						}
						
						//if(Nod.Instance_.Count > 0) //Pour un noeud, si le nombre d'instance est supérieur à 0
						//{
							IterInstance = ExplorerListStore.AppendValues(IterNode,PngFolder,param.ParamT("ExTVNameInstance"));//On écrit le conteneur instance
							
							IterLighting = ExplorerListStore.AppendValues(IterInstance,PngLighting,param.ParamT("ExTVNameLighting"),param.ParamP("ExTVTypeLighting"),"",Nod.Node_Id.ToString()); //On affiche le conteneur lighting
							IterSwitch = ExplorerListStore.AppendValues(IterInstance,PngSwitch,param.ParamT("ExTVNameSwitch"),param.ParamP("ExTVTypeSwitch"),"",Nod.Node_Id.ToString()); //On affiche le conteneur switch
							IterShutter = ExplorerListStore.AppendValues(IterInstance,PngShutter,param.ParamT("ExTVNameShutter"),param.ParamP("ExTVTypeShutter"),"",Nod.Node_Id.ToString()); //On affiche le conteneur shutter
							
							/*
							if(Nod.CountInstancePerType(param.ParamP("InstLightingName")) > 0) //Si le nombre de lumière est supérieur à 0
							{
								IterLighting = ExplorerListStore.AppendValues(IterInstance,PngLighting,param.ParamT("ExTVNameLighting"),param.ParamP("ExTVTypeLighting"),"",Nod.Node_Id.ToString()); //On affiche le conteneur lighting
							}
							if(Nod.CountInstancePerType(param.ParamP("InstSwitchName")) > 0) //Si le nombre de switch est supérieur à 0
							{
								IterSwitch = ExplorerListStore.AppendValues(IterInstance,PngSwitch,param.ParamT("ExTVNameSwitch"),param.ParamP("ExTVTypeSwitch"),"",Nod.Node_Id.ToString()); //On affiche le conteneur switch
							}
							if(Nod.CountInstancePerType(param.ParamP("InstShutterName")) > 0)//Si le nombre de shutter est supérieur à 0
							{							
								IterShutter = ExplorerListStore.AppendValues(IterInstance,PngShutter,param.ParamT("ExTVNameShutter"),param.ParamP("ExTVTypeShutter"),"",Nod.Node_Id.ToString()); //On affiche le conteneur shutter
							}*/
							/*
							foreach(Instance ins in Nod.ReturnListInstance()) //Pour chaque instance de liste appartenant à un noeud
							{
								if(ins.Instance_Type == param.ParamP("InstLightingName")) //Si l'instance est de type lighting
								{
									ExplorerListStore.AppendValues(IterLighting,PngLighting,ins.Instance_Name,ins.Instance_Type,ins.Instance_Name,ins.Instance_Id.ToString()); //On l'ajoute dans le conteneur lighting
								}
								else if(ins.Instance_Type == param.ParamP("InstSwitchName")) //Si l'instance est de type switch
								{
									ExplorerListStore.AppendValues(IterSwitch,PngSwitch,ins.Instance_Name,ins.Instance_Type,ins.Instance_Name,ins.Instance_Id.ToString());//On l'ajoute dans le conteneur switch
								}
								else if(ins.Instance_Type == param.ParamP("InstShutterName")) //Si l'instance est de type shutter
								{
									ExplorerListStore.AppendValues(IterShutter,PngShutter,ins.Instance_Name,ins.Instance_Type,ins.Instance_Name,ins.Instance_Id.ToString());//On l'ajoute dans le conteneur shutter
								}								
							}*/
						//}
						IterFile = ExplorerListStore.AppendValues(IterNode,PngFolder,param.ParamT("ExTVNameFile"));
							IterCustomer = ExplorerListStore.AppendValues(IterFile,PngFolder,param.ParamT("ExTVNameCustomer"));
							foreach(Customer cus in Nod.ReturnListCustomer())
							{
								if(cus.CustomerUse)
								{
									ExplorerListStore.AppendValues(IterCustomer,PngCustomerUse,cus.CustomerName,param.ParamP("Customer"),cus.CustomerName,cus.CustomerId.ToString());
								}
								else
								{
									ExplorerListStore.AppendValues(IterCustomer,PngCustomer,cus.CustomerName,param.ParamP("Customer"),cus.CustomerName,cus.CustomerId.ToString());
								}
							}
							IterScenario = ExplorerListStore.AppendValues(IterFile,PngFolder,param.ParamT("ExTVNameScenario"));
							foreach(Scenario sce in Nod.ReturnListScenario())
							{
								ExplorerListStore.AppendValues(IterScenario,PngScenario,sce.ScenarioName,param.ParamP("Scenario"),sce.ScenarioName,sce.ScenarioId.ToString());
							}

					}
				}
			}
			ExplorerTreeView.Model = ExplorerListStore; //On met la liste store dans le treeview
			ExplorerTreeView.ExpandAll();
		}
		
		//Fonction ExplorerTreeViewButtonPress
		//Fonction permettant d'executer des action sur appuie de la souris
		//Nous allons surtout utiliser cette fonction pour gérer le double clic dans l'explorateur de projet
		[GLib.ConnectBefore]
	    private void ExplorerTreeViewButtonPress(object o, ButtonPressEventArgs args)
		{	
	        if (args.Event.Type == EventType.TwoButtonPress) //Sinon si nous faisons un double clic
			{
				TreeSelection selection = (o as TreeView).Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelExplorerTreeView, out IterProject)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					TreeViewEplorerValCol1 = (string) TreeModelExplorerTreeView.GetValue (IterProject, param.ParamI("ExTVPositionTexte")); //Nous mettons la valeur de la 1ere cellule dans un string (Texte)
					TreeViewEplorerValCol2 = (string) TreeModelExplorerTreeView.GetValue (IterProject, param.ParamI("ExTVPositionType")); //idem pou pour la deuxieme cellule (Type)
					TreeViewEplorerValCol3 = (string) TreeModelExplorerTreeView.GetValue (IterProject, param.ParamI("ExTVPositionName")); //idem pou pour la troisième cellule	(Nom)
	                TreeViewEplorerValCol4 = (string) TreeModelExplorerTreeView.GetValue (IterProject, param.ParamI("ExTVPositionId")); //idem pou pour la quatrième cellule	(Id)
					
					
					if(TreeViewEplorerValCol2 == param.ParamP("ExTVTypeProject")) //Dans le cas où la cellule sélectionné est de type projet
					{
						ListProjectProperties.Add (new ProjectProperties(datamanagement,param,Convert.ToInt32(TreeViewEplorerValCol4)));
						AddTabInMainNoteBook("Project",ListProjectProperties[ListProjectProperties.Count-1],param.ParamP("IconProject"),TreeViewEplorerValCol2,TreeViewEplorerValCol4); //On ouvre un nouvelle onglet permettant d'afficher les caratéristiques propre au projet					
					}	
					else if(TreeViewEplorerValCol2 == param.ParamP("ExTVTypeNode")) //Nous regardons la cellule 2 et nous comparons pour voir si elle est de type noeud
					{
						ListNodeProperties.Add(new NodeProperties(datamanagement,param,Convert.ToInt32(TreeViewEplorerValCol4)));
						AddTabInMainNoteBook("",ListNodeProperties[ListNodeProperties.Count-1],param.ParamP("IconNode"),TreeViewEplorerValCol2,TreeViewEplorerValCol4); //Dans ce cas nous ajoutons un nouvel onglet dans le notebook
					}
					else if(TreeViewEplorerValCol2 == param.ParamP("ExTVTypeNetworkI2C")) //Nous regardons la cellule 2 et nous comparons pour voir si elle est de type réseau
					{
						ListI2CProperties.Add (new I2CProperties(datamanagement,param,Convert.ToInt32(TreeViewEplorerValCol4)));
						AddTabInMainNoteBook("",ListI2CProperties[ListI2CProperties.Count-1],param.ParamP("IconNetwork"),TreeViewEplorerValCol2,TreeViewEplorerValCol4); //On ouvre un nouvel onglet permettant d'afficher les caractéristique du réseau I2C
					}
					else if(TreeViewEplorerValCol2 == param.ParamP("ExTVTypeNetwork1Wire")) //Nous regardons la cellule 2 et nous comparons pour voir si elle est de type réseau
					{
						ListOneWireProperties.Add(new OneWireProperties(datamanagement,param,Convert.ToInt32(TreeViewEplorerValCol4)));
						AddTabInMainNoteBook("",ListOneWireProperties[ListOneWireProperties.Count-1],param.ParamP("IconNetwork"),TreeViewEplorerValCol2,TreeViewEplorerValCol4);//On ouvre un nouvel onglet permettant d'afficher les caractéristique du réseau 1-Wire
					}	
					else if(TreeViewEplorerValCol2 == param.ParamP("InstLightingName") && TreeViewEplorerValCol3 == "")
					{
						ListInstanceProperties.Add(new InstanceProperties(datamanagement,param,TreeViewEplorerValCol2,Convert.ToInt32(TreeViewEplorerValCol4)));
						AddTabInMainNoteBook("",ListInstanceProperties[ListInstanceProperties.Count-1],param.ParamP("IconLighting"),TreeViewEplorerValCol2,TreeViewEplorerValCol4); //On ouvre un nouvel onglet permettant d'afficher les caractéristique de la carte												
					}
					else if(TreeViewEplorerValCol2 == param.ParamP("InstSwitchName") && TreeViewEplorerValCol3 == "")
					{
						ListInstanceProperties.Add(new InstanceProperties(datamanagement,param,TreeViewEplorerValCol2,Convert.ToInt32(TreeViewEplorerValCol4)));
						AddTabInMainNoteBook("",ListInstanceProperties[ListInstanceProperties.Count-1],param.ParamP("IconSwitch"),TreeViewEplorerValCol2,TreeViewEplorerValCol4); //On ouvre un nouvel onglet permettant d'afficher les caractéristique de la carte						
					}		
					else if(TreeViewEplorerValCol2 == param.ParamP("InstShutterName") && TreeViewEplorerValCol3 == "")
					{
						ListInstanceProperties.Add(new InstanceProperties(datamanagement,param,TreeViewEplorerValCol2,Convert.ToInt32(TreeViewEplorerValCol4)));
						AddTabInMainNoteBook("",ListInstanceProperties[ListInstanceProperties.Count-1],param.ParamP("IconShutter"),TreeViewEplorerValCol2,TreeViewEplorerValCol4); //On ouvre un nouvel onglet permettant d'afficher les caractéristique de la carte						
					}	
					else if(TreeViewEplorerValCol2 == param.ParamP("Customer"))
					{
						ListCustomerEdit.Add(new CustomerEdit(datamanagement,param,Convert.ToInt32(TreeViewEplorerValCol4),this));
						AddTabInMainNoteBook(TreeViewEplorerValCol3,ListCustomerEdit[ListCustomerEdit.Count-1],param.ParamP("IconCustomer"),TreeViewEplorerValCol2,TreeViewEplorerValCol4);//On ouvre un nouvel onglet permettant d'afiicher le treeview
					}
					else if(TreeViewEplorerValCol2 == param.ParamP("Scenario"))
					{
						ListScenarioEdit.Add(new ScenarioEdit(datamanagement,param,Convert.ToInt32(TreeViewEplorerValCol4),this,pref));
						AddTabInMainNoteBook(TreeViewEplorerValCol3,ListScenarioEdit[ListScenarioEdit.Count-1],param.ParamP("IconScenario"),TreeViewEplorerValCol2,TreeViewEplorerValCol4);//On ouvre un nouvel onglet permettant d'afiicher le treeview
					}					
					else //Dans les autres cas nous allons regarder la liste des carte et ouvrir la carte correspondante
					{
						foreach(Boards boards in datamanagement.ListBoards) //Pour chaque carte de la liste des carte xPLduino
						{
							if(TreeViewEplorerValCol2 == boards.Type)
							{
								if(boards.NetworkType == param.ParamP("ExTVTypeNetworkI2C")) //Si le type de la carte sélectionné est de type I2C
								{
									ListBoardI2CProperties.Add (new BoardI2CProperties(datamanagement,param,Convert.ToInt32(TreeViewEplorerValCol4)));
									AddTabInMainNoteBook("",ListBoardI2CProperties[ListBoardI2CProperties.Count-1],param.ParamP("IconBoard"),TreeViewEplorerValCol2,TreeViewEplorerValCol4); //On ouvre un nouvel onglet permettant d'afficher les caractéristique de la carte						
								}
							}
						}							
					}
				}
	        }		
			UpdateStatusBar();
		}
		
		//Fonction ExplorerTreeViewButtonPressHandler
		//Fonction permettant de faire des actions sur relachement de la souris
		[GLib.ConnectBefore]
	    private void ExplorerTreeViewButtonRelease(object o, ButtonReleaseEventArgs args)
		{	
			RightClicMenuExplorerTreeView = new Gtk.Menu();	//Nous allons creer un nouveau menu
			if(args.Event.Button == param.ParamI("LeftClic")) //Si le bouton cliquer est le clic gauche
			{
			}
			else if(args.Event.Button == param.ParamI("RightClic")) //Si le bouton cliquer est le clic droit
			{
				TreeSelection selection = (o as TreeView).Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelExplorerTreeView, out IterProject)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					TreeViewEplorerValCol1 = (string) TreeModelExplorerTreeView.GetValue (IterProject, param.ParamI("ExTVPositionTexte")); //Nous mettons la valeur de la 1ere cellule dans un string
					TreeViewEplorerValCol2 = (string) TreeModelExplorerTreeView.GetValue (IterProject, param.ParamI("ExTVPositionType")); //idem pou pour la deuxieme cellule
					TreeViewEplorerValCol3 = (string) TreeModelExplorerTreeView.GetValue (IterProject, param.ParamI("ExTVPositionName")); //idem pou pour la troisième cellule	
	                TreeViewEplorerValCol4 = (string) TreeModelExplorerTreeView.GetValue (IterProject, param.ParamI("ExTVPositionId")); //idem pou pour la quatrième cellule					
				
					if(TreeViewEplorerValCol2 == param.ParamP("ExTVTypeProject")) //Dans le cas où la cellule 2 est de type projet
					{
				        RightClicMenuItemExplorerTreeView = new MenuItem(param.ParamT("MenuNewNode"));  //Item permettant d'inserer un nouveau noeud dans le projet
				        RightClicMenuExplorerTreeView.Add(RightClicMenuItemExplorerTreeView); //On ajout l'item dans le treeview  
						RightClicMenuItemExplorerTreeView.Activated += CreateNewNode; //Fonction appelé lors du clic sur l'item
						
						SeparatorMenuItem sep = new SeparatorMenuItem();
						RightClicMenuExplorerTreeView.Add(sep);						
						
						RightClicMenuItemExplorerTreeView = new MenuItem(param.ParamT("MenuDeleteProject")); //Item permettant de supprimer un projet
				        RightClicMenuExplorerTreeView.Add(RightClicMenuItemExplorerTreeView);  //On ajout l'item dans le treeview  
						RightClicMenuItemExplorerTreeView.Activated += DeleteProject; //Fonction appelé lors du clic sur l'item
						RightClicMenuExplorerTreeView.ShowAll(); //On affiche le menu
						RightClicMenuExplorerTreeView.Popup();	//Sous forme de popup							
					}
					else if(TreeViewEplorerValCol2 == param.ParamP("ExTVTypeNode")) //Dans le cas où la cellule 2 est de type noeud
					{
				        RightClicMenuItemExplorerTreeView = new MenuItem(param.ParamT("MenuNewNetwork")); //Item permettant d'inserer un nouveau réseau
				        RightClicMenuExplorerTreeView.Add(RightClicMenuItemExplorerTreeView); //On ajout l'item dans le treeview    
						RightClicMenuItemExplorerTreeView.Activated += CreateNewNetwork; //Fonction appelé lors du clic sur l'item
						
						/*
				        RightClicMenuItemExplorerTreeView = new MenuItem(param.ParamT("MenuNewInstance")); //Item permettant d'inserer un nouveau réseau
				        RightClicMenuExplorerTreeView.Add(RightClicMenuItemExplorerTreeView); //On ajout l'item dans le treeview    
						RightClicMenuItemExplorerTreeView.Activated += CreateNewInstance; //Fonction appelé lors du clic sur l'item
						*/
						
						SeparatorMenuItem sep = new SeparatorMenuItem();
						RightClicMenuExplorerTreeView.Add(sep);						
						
				        RightClicMenuItemExplorerTreeView = new MenuItem(param.ParamT("MenuNewCustomer")); //Item permettant d'inserer un nouveau réseau
				        RightClicMenuExplorerTreeView.Add(RightClicMenuItemExplorerTreeView); //On ajout l'item dans le treeview    
						RightClicMenuItemExplorerTreeView.Activated += CreateNewCustomer; //Fonction appelé lors du clic sur l'item
									
				        RightClicMenuItemExplorerTreeView = new MenuItem(param.ParamT("MenuNewScenario")); //Item permettant d'inserer un nouveau réseau
				        RightClicMenuExplorerTreeView.Add(RightClicMenuItemExplorerTreeView); //On ajout l'item dans le treeview    
						RightClicMenuItemExplorerTreeView.Activated += CreateNewScenario; //Fonction appelé lors du clic sur l'item						
						
						SeparatorMenuItem sep2 = new SeparatorMenuItem();
						RightClicMenuExplorerTreeView.Add(sep2);
						
						RightClicMenuItemExplorerTreeView = new MenuItem(param.ParamT("MenuDeleteNode")); //Item permettant de supprimer un noeud
				        RightClicMenuExplorerTreeView.Add(RightClicMenuItemExplorerTreeView); //On ajout l'item dans le treeview    
						RightClicMenuItemExplorerTreeView.Activated += DeleteNode; //Fonction appelé lors du clic sur l'item
						RightClicMenuExplorerTreeView.ShowAll(); //On affiche le menu
						RightClicMenuExplorerTreeView.Popup();	//Sous forme de popup							
					}
					else if((TreeViewEplorerValCol2 == param.ParamP("InstLightingName") || TreeViewEplorerValCol2 == param.ParamP("InstSwitchName") || TreeViewEplorerValCol2 == param.ParamP("InstShutterName")) && TreeViewEplorerValCol3 != "")
					{
				        RightClicMenuItemExplorerTreeView = new MenuItem(param.ParamT("MenuDeleteInstance")); //Item permettant d'inserer un nouveau réseau
				        RightClicMenuExplorerTreeView.Add(RightClicMenuItemExplorerTreeView); //On ajout l'item dans le treeview    
						RightClicMenuItemExplorerTreeView.Activated += DeleteInstance; //Fonction appelé lors du clic sur l'item
		
						RightClicMenuExplorerTreeView.ShowAll(); //On affiche le menu
						RightClicMenuExplorerTreeView.Popup();	//Sous forme de popup						
					}
					else if(TreeViewEplorerValCol2 == param.ParamP("Customer")) //Dans le cas où la cellule 2 est de type noeud
					{
				        RightClicMenuItemExplorerTreeView = new MenuItem(param.ParamT("MenuDeleteCustomer")); //Item permettant d'inserer un nouveau réseau
				        RightClicMenuExplorerTreeView.Add(RightClicMenuItemExplorerTreeView); //On ajout l'item dans le treeview    
						RightClicMenuItemExplorerTreeView.Activated += DeleteCustomer; //Fonction appelé lors du clic sur l'item
		
						RightClicMenuExplorerTreeView.ShowAll(); //On affiche le menu
						RightClicMenuExplorerTreeView.Popup();	//Sous forme de popup							
					}
						else if(TreeViewEplorerValCol2 == param.ParamP("Scenario")) //Dans le cas où la cellule 2 est de type noeud
					{
				        RightClicMenuItemExplorerTreeView = new MenuItem(param.ParamT("MenuDeleteScenario")); //Item permettant d'inserer un nouveau réseau
				        RightClicMenuExplorerTreeView.Add(RightClicMenuItemExplorerTreeView); //On ajout l'item dans le treeview    
						RightClicMenuItemExplorerTreeView.Activated += DeleteScenario; //Fonction appelé lors du clic sur l'item
		
						RightClicMenuExplorerTreeView.ShowAll(); //On affiche le menu
						RightClicMenuExplorerTreeView.Popup();	//Sous forme de popup							
					}				
					else //Dans les autres cas
					{
						foreach(Boards boards in datamanagement.ListBoards) //Pour chaque carte de la liste des carte xPLduino
						{
							if(TreeViewEplorerValCol2 == boards.Type) //Si une carte est égale à la cellule lu alors on affiche un menu
							{
								RightClicMenuItemExplorerTreeView = new MenuItem(param.ParamT("MenuDeleteBoard")); //Item permettant de supprimer une carte
						        RightClicMenuExplorerTreeView.Add(RightClicMenuItemExplorerTreeView); //On ajout l'item dans le treeview     
								RightClicMenuItemExplorerTreeView.Activated += DeleteBoard; //Fonction appelé lors du clic sur l'item
								RightClicMenuExplorerTreeView.ShowAll(); //On affiche le menu
								RightClicMenuExplorerTreeView.Popup();	//Sous forme de popup									
							}
						}	
					}
					
					if(TreeViewEplorerValCol3 == param.ParamP("ExTVTypeNetwork")) //Dans le cas où la cellule trois est du type réseau
					{
				        RightClicMenuItemExplorerTreeView = new MenuItem(param.ParamT("MenuNewBoard")); //Item permettant de creer une nouvelle carte
				        RightClicMenuExplorerTreeView.Add(RightClicMenuItemExplorerTreeView);  //On ajout l'item dans le treeview
						RightClicMenuItemExplorerTreeView.Activated += CreateNewBoard; //Fonction appelé lors du clic sur l'item
						
						SeparatorMenuItem sep = new SeparatorMenuItem();
						RightClicMenuExplorerTreeView.Add(sep);							
						
						RightClicMenuItemExplorerTreeView = new MenuItem(param.ParamT("MenuDeleteNetwork")); //Item permettant de supprimer un réseau
				        RightClicMenuExplorerTreeView.Add(RightClicMenuItemExplorerTreeView);  //On ajout l'item dans le treeview 
						RightClicMenuItemExplorerTreeView.Activated += DeleteNetwork; //Fonction appelé lors du clic sur l'item
						RightClicMenuExplorerTreeView.ShowAll(); //On affiche le menu
						RightClicMenuExplorerTreeView.Popup();	//Sous forme de popup							
					}
				}
			}			
	    }	
				
			//Fonction CreateNewNode
			//Fonction permettant de créer un nouveau noeud à partir de l'arbre
			public void CreateNewNode (object o, EventArgs e)
			{
				this.Sensitive = false; //Permet d'incativer la fenêtre principal
				new NewNode(datamanagement,Convert.ToInt32(TreeViewEplorerValCol4),param); //Appel la fenêtre permettant l'ajout d'un nouveau noeud
			}
		
			//Fonction CreateNewNetwork
			//Fonction permettant la création d'un nouveau réseau
			public void CreateNewNetwork (object o, EventArgs e)
			{
				this.Sensitive = false; //Permet d'incativer la fenêtre principal
				new NewNetwork(datamanagement,Convert.ToInt32(TreeViewEplorerValCol4),param); //Appel la fenêtre permettant l'ajout d'un nouveau réseau
			}
		
			//Fonction CreateNewBoard
			//Fonction permettant d'afficher la fenêtre pour inserer une nouvelle carte
			public void CreateNewBoard (object o, EventArgs e)
			{
				this.Sensitive = false; //Permet d'incativer la fenêtre principal
				new NewBoard (datamanagement,Convert.ToInt32(TreeViewEplorerValCol4),TreeViewEplorerValCol2,param); //Appel la fenêtre permettant l'ajout d'une nouvelle carte
			}
		
			//Fonction CreateNewInstance
			//Fonction permettant d'afficher la fenetre pour inserer une nouvelle instance
			public void CreateNewInstance (object o, EventArgs e)
			{
				this.Sensitive = false; //Permet d'incativer la fenêtre principal
				new NewInstance(datamanagement,Convert.ToInt32(TreeViewEplorerValCol4),param); //Appel la fenêtre permettant l'ajout d'une nouvelle instance
			}
		
			//Fcontion CreateNewCustomer
			//Fonction permettant la création d'un nouveau fichier customer
			public void CreateNewCustomer (object o, EventArgs e)
			{
				this.Sensitive = false; //Permet d'incativer la fenêtre principal
				new NewCustomer(datamanagement,Convert.ToInt32(TreeViewEplorerValCol4),param);
			}

			//Fcontion CreateNewScenario
			//Fonction permettant la création d'un nouveau fichier scenario
			public void CreateNewScenario (object o, EventArgs e)
			{
				this.Sensitive = false; //Permet d'incativer la fenêtre principal
				new NewScenario(datamanagement,Convert.ToInt32(TreeViewEplorerValCol4),param);
			}
		
			//Fonction DeleteProject
			//Fonction perettant de supprimer un projet depuis l'explorer treeview
			public void DeleteProject (object o, EventArgs e)
			{
				this.Sensitive = false; //Permet d'incativer la fenêtre principal
				if(pref.BeepOnDelete)
					System.Media.SystemSounds.Question.Play(); //Avertir du popup
				//Boite de dialogue permettant de valider la suppression d'un projet
				MessageDialog message = new MessageDialog (this, 
	                                      			  	   DialogFlags.DestroyWithParent,
		                              				       MessageType.Question, 
	                                                       ButtonsType.YesNo, param.ParamT("QuestionDeleteProject") + TreeViewEplorerValCol3 + " ?");
	            
				ResponseType messageresult = (ResponseType)message.Run (); //On lance la box
				
				
				if (messageresult == ResponseType.Yes) //On regarde le résultat et si Oui
				{
					datamanagement.DeleteProject(Convert.ToInt32(TreeViewEplorerValCol4)); //On appel la fonction permettant de supprimer un projet à partir de son Id
					UpdateEplorerTreeView(); //On met  jour l'explorer treeview
					message.Destroy(); //On détruit la boite de dialogue
				}
				else
				{
					message.Destroy();//On détruit la boite de dialogue
				}
				this.Sensitive = true; //Permet d'activer la fenêtre principal
			}		
		
			//Fonction DeleteNode
			//Fonction permettant de supprimer un noeud depuis l'arbre
			public void DeleteNode (object o, EventArgs e)
			{
				this.Sensitive = false; //Permet d'incativer la fenêtre principal
				if(pref.BeepOnDelete)
					System.Media.SystemSounds.Question.Play(); //Avertir du popup
				//Boite de dialogue permettant de valider la suppression d'un noeud
				MessageDialog message = new MessageDialog (this, 
	                                      			  	   DialogFlags.DestroyWithParent,
		                              				       MessageType.Question, 
	                                                       ButtonsType.YesNo, param.ParamT("QuestionDeleteNode") + TreeViewEplorerValCol3 + " ?");
	            
				ResponseType messageresult = (ResponseType)message.Run (); //On lance la box
				
				if (messageresult == ResponseType.Yes) //On regarde le résultat et si Oui
				{			
					datamanagement.DeleteNodeInProject(Convert.ToInt32(TreeViewEplorerValCol4)); //On appel la fonction permettant de supprimer un noeud dans un projet
					UpdateEplorerTreeView(); //On met  jour l'explorer treeview
					message.Destroy(); //On détruit la boite de dialogue
				}
				else
				{
					message.Destroy(); //On détruit la boite de dialogue
				}
				this.Sensitive = true; //Permet d'activer la fenêtre principal
			}
		
			//Fonction DeleteNetwork
			//Fonction permettant de supprimer un réseau depuis l'arbre
			public void DeleteNetwork (object o, EventArgs e)
			{
				this.Sensitive = false; //Permet d'incativer la fenêtre principal
				if(pref.BeepOnDelete)
					System.Media.SystemSounds.Question.Play(); //Avertir du popup
				//Boite de dialogue permettant de valider la suppression d'un réseau
				MessageDialog message = new MessageDialog (this, 
	                                      			  	   DialogFlags.DestroyWithParent,
		                              				       MessageType.Question, 
	                                                       ButtonsType.YesNo, param.ParamT("QuestionDeleteNetwork") + TreeViewEplorerValCol2 + " ?");
	            
				ResponseType messageresult = (ResponseType)message.Run (); //On lance la box
			
				if (messageresult == ResponseType.Yes) //On regarde le résultat et si Oui
				{
					datamanagement.DeleteNetworkInNode(Convert.ToInt32(TreeViewEplorerValCol4)); //On appel la fonction permettant de supprimer un réseau dans un noeud
					UpdateEplorerTreeView(); //On met  jour l'explorer treeview
					message.Destroy(); //On détruit la boite de dialogue					
				}
				else
				{
					message.Destroy(); //On détruit la boite de dialogue
				}
				this.Sensitive = true; //Permet d'activer la fenêtre principal
			}
		
			//Fonction DeleteBoard
			//Fonction permettant de supprimer une carte
			public void DeleteBoard (object o, EventArgs e)
			{
				this.Sensitive = false; //Permet d'incativer la fenêtre principal
				if(pref.BeepOnDelete)
					System.Media.SystemSounds.Question.Play(); //Avertir du popup
				//Boite de dialogue permettant de valider la suppression d'un réseau
				MessageDialog message = new MessageDialog (this, 
	                                      			  	   DialogFlags.DestroyWithParent,
		                              				       MessageType.Question, 
	                                                       ButtonsType.YesNo, param.ParamT("QuestionDeleteBoard") + TreeViewEplorerValCol3 + " ?");
	            
				ResponseType messageresult = (ResponseType)message.Run (); //On lance la box
			
				if (messageresult == ResponseType.Yes) //On regarde le résultat et si Oui
				{
					datamanagement.DeleteBoardInNetwork(Convert.ToInt32(TreeViewEplorerValCol4));//On appel la fonction permettant de supprimer une carte dans un réseau
					UpdateEplorerTreeView(); //On met  jour l'explorer treeview
					message.Destroy(); //On détruit la boite de dialogue					
				}
				else
				{
					message.Destroy(); //On détruit la boite de dialogue
				}		
				this.Sensitive = true; //Permet d'activer la fenêtre principal
			}
		
			//Fonction DeleteInstance
			//Fonction permettant de supprimer une instance
			public void DeleteInstance (object o, EventArgs e)
			{
				this.Sensitive = false; //Permet d'incativer la fenêtre principal
				if(pref.BeepOnDelete)	
					System.Media.SystemSounds.Question.Play(); //Avertir du popup
				//Boite de dialogue permettant de valider la suppression d'un réseau
				MessageDialog message = new MessageDialog (this, 
	                                      			  	   DialogFlags.DestroyWithParent,
		                              				       MessageType.Question, 
	                                                       ButtonsType.YesNo, param.ParamT("QuestionDeleteInstance") + TreeViewEplorerValCol3 + " ?");
	            
				ResponseType messageresult = (ResponseType)message.Run (); //On lance la box
			
				if (messageresult == ResponseType.Yes) //On regarde le résultat et si Oui
				{
					datamanagement.DeleteInstanceInNode(Convert.ToInt32(TreeViewEplorerValCol4)); //On appel la fonction permettant de supprimer un réseau dans un noeud
					UpdateEplorerTreeView(); //On met  jour l'explorer treeview
					message.Destroy(); //On détruit la boite de dialogue					
				}
				else
				{
					message.Destroy(); //On détruit la boite de dialogue
				}
				this.Sensitive = true; //Permet d'activer la fenêtre principal			
			}
		
			//Fonction DeleteCustomer
			//Fonctioner permettant de supprimer un fichier customer
			public void DeleteCustomer (object o, EventArgs e)
			{
				this.Sensitive = false; //Permet d'incativer la fenêtre principal
				if(pref.BeepOnDelete)	
					System.Media.SystemSounds.Question.Play(); //Avertir du popup
				//Boite de dialogue permettant de valider la suppression d'un réseau
				MessageDialog message = new MessageDialog (this, 
	                                      			  	   DialogFlags.DestroyWithParent,
		                              				       MessageType.Question, 
	                                                       ButtonsType.YesNo, param.ParamT("QuestionDeleteCustomer") + TreeViewEplorerValCol3 + " ?");
	            
				ResponseType messageresult = (ResponseType)message.Run (); //On lance la box
			
				if (messageresult == ResponseType.Yes) //On regarde le résultat et si Oui
				{
					datamanagement.DeleteCustomerInNode(Convert.ToInt32(TreeViewEplorerValCol4)); //On appel la fonction permettant de supprimer un réseau dans un noeud
					UpdateEplorerTreeView(); //On met  jour l'explorer treeview
					message.Destroy(); //On détruit la boite de dialogue					
				}
				else
				{
					message.Destroy(); //On détruit la boite de dialogue
				}
				this.Sensitive = true; //Permet d'activer la fenêtre principal					
			}
		
			//Fonction DeleteScenario
			//Fonctioner permettant de supprimer un fichier scénario
			public void DeleteScenario (object o, EventArgs e)
			{
				this.Sensitive = false; //Permet d'incativer la fenêtre principal
				if(pref.BeepOnDelete)	
					System.Media.SystemSounds.Question.Play(); //Avertir du popup
				//Boite de dialogue permettant de valider la suppression d'un réseau
				MessageDialog message = new MessageDialog (this, 
	                                      			  	   DialogFlags.DestroyWithParent,
		                              				       MessageType.Question, 
	                                                       ButtonsType.YesNo, param.ParamT("QuestionDeleteCustomer") + TreeViewEplorerValCol3 + " ?");
	            
				ResponseType messageresult = (ResponseType)message.Run (); //On lance la box
			
				if (messageresult == ResponseType.Yes) //On regarde le résultat et si Oui
				{
					datamanagement.DeleteScenarioInNode(Convert.ToInt32(TreeViewEplorerValCol4)); //On appel la fonction permettant de supprimer un réseau dans un noeud
					UpdateEplorerTreeView(); //On met  jour l'explorer treeview
					message.Destroy(); //On détruit la boite de dialogue					
				}
				else
				{
					message.Destroy(); //On détruit la boite de dialogue
				}
				this.Sensitive = true; //Permet d'activer la fenêtre principal					
			}	
			
//################ Output Treeview #####################################	
		
		//Fonction InitOutputTreeview
		//Fonction permettant d'initialiser l'output treeview où nous pourrons afficher des informations nécessaire à l'utilisateur final
		//Pas d'argument en entrée
		public void InitOutputTreeview()
		{
			//Permet les ligne vertical et honrizontal dans l'output treeview
			OutputTreeview.EnableGridLines = TreeViewGridLines.Both;
			
			//Nous donnons un titre au deux colonnes
			LogoColumn.Title = "";
			DateAndTimeColumn.Title = param.ParamT ("OutTVDateAndTimeValue");
			InfoColumn.Title = param.ParamT("OutTVTitleValue");				

			//On insere les cellules dans les colonnes
			LogoColumn.PackStart(LogoPixCell, false);
			DateAndTimeColumn.PackStart(DateAndTimeCell, true);
			InfoColumn.PackStart(InfoCell, true);	
			
			//On insere les colonnes dans l'OutputTreeview
			OutputTreeview.AppendColumn (LogoColumn);
			OutputTreeview.AppendColumn (DateAndTimeColumn);
			OutputTreeview.AppendColumn (InfoColumn);	
			
			//On importe des attributs
			LogoColumn.AddAttribute (LogoPixCell, "pixbuf", param.ParamI("OutTVPositionLogo"));
			DateAndTimeColumn.AddAttribute (DateAndTimeCell, "text", param.ParamI("OutTVPositionDateAndTime"));
			InfoColumn.AddAttribute(InfoCell,"text",param.ParamI("OutTVPositionInfo"));
			
			//Création d'un nouveau store pour l'output treeview
			OutputListStore = new Gtk.TreeStore (typeof (Gdk.Pixbuf),typeof (string),typeof (string));				
		}
		
		//Fonction AddLineOutput
		//Fonction permettant d'ajouter une ligne et un logo dans l'output treeview
		//Arguments :
		//	int criticity : indique le niveau de l'information : 0 : Info, 1 : warning 2 :erreur 3 : Question
		//	string Info : Texte à afficher
		public void AddLineOutput(int criticity, string Info)
		{			
			//On ajoute les infos output dans le ListOutput
			if(criticity==param.ParamI("OutputInformation")) //Si la criticité est de type information 
			{
				ListOutputTreeview.Add(new OutputTreeView(Info,param.ParamP("LogoOutputInformation"),DateTime.Now));	//On ajoute une nouvelle ligne dans l'output treeview avec le logo information
			}
			else if(criticity==param.ParamI("OutputWarning"))//Si la criticité est de type warning
			{
				ListOutputTreeview.Add(new OutputTreeView(Info,param.ParamP("LogoOutputWarning"),DateTime.Now));//On ajoute une nouvelle ligne dans l'output treeview avec le logo warning								
			}		
			else if(criticity==param.ParamI("OutputError")) //Si la criticité est de type erreur
			{
				if(pref.BeepOnError)
					System.Media.SystemSounds.Exclamation.Play();
				ListOutputTreeview.Add(new OutputTreeView(Info,param.ParamP("LogoOutputError"),DateTime.Now));//On ajoute une nouvelle ligne dans l'output treeview avec le logo erreur
				
				//On rend visible le panel pour voir l'erreur
				ViewNoteBook.Visible = true;
				ExpendHistoryPanel.Visible = false;
				ReduceHistoryPanel.Visible = true;
				
				foreach(ScenarioEdit sc in ListScenarioEdit)
				{
					sc.UpdateVpaned(70);
				}				
			}	
			else if(criticity==param.ParamI("OutputQuestion")) //Si la criticité est de type question
			{
				ListOutputTreeview.Add(new OutputTreeView(Info,param.ParamP("LogoOutputQuestion"),DateTime.Now));//On ajoute une nouvelle ligne dans l'output treeview avec le logo question							
			}	
			
			RenameTooltipTextButtonOutput();
			UpdateOutputTreeview();

		}	
		
		//Fonction RenameTooltipTextButtonOutput
		//Permet de renommer les info bulles des boutons de l'output
		public void RenameTooltipTextButtonOutput()
		{
			int CountInfo = 0;
			int CountWarning = 0;
			int CountError = 0;
			int CountQuestion = 0;
			
			foreach(OutputTreeView outTV in ListOutputTreeview)
			{
				if(outTV.OutputTreviewCriticity == param.ParamP("LogoOutputInformation"))
				{
					CountInfo++;
				}
				else if(outTV.OutputTreviewCriticity == param.ParamP("LogoOutputWarning"))
				{
					CountWarning++;
				}
				else if(outTV.OutputTreviewCriticity == param.ParamP("LogoOutputError"))
				{
					CountError++;
				}
				else if(outTV.OutputTreviewCriticity == param.ParamP("LogoOutputQuestion"))
				{
					CountQuestion++;
				}		
			}
			
			this.ButtonFilterInfo.TooltipText = CountInfo + " " + param.ParamT("TextOutputInformation");
			this.ButtonFilterInfo.Image = ButtonFilterInfoImage;
				
			this.ButtonFilterWarning.TooltipText = CountWarning + " " + param.ParamT("TextOutputWarning");
			this.ButtonFilterWarning.Image = ButtonFilterWarningImage;
				
			this.ButtonFilterError.TooltipText = CountError + " " + param.ParamT("TextOutputError");
			this.ButtonFilterError.Image = ButtonFilterErrorImage;
				
			this.ButtonFilterQuestion.TooltipText = CountQuestion + " " + param.ParamT("TextOutputQuestion");
			this.ButtonFilterQuestion.Image = ButtonFilterQuestionImage;				
		}
		
		//Fonction UpdateOutputTreeview
		//Fonction permettant de remettre à jour l'outputtreeview
		public void UpdateOutputTreeview()
		{
			OutputListStore.Clear();
			for(int i=ListOutputTreeview.Count-1;i>=0;i--)
			{
				if(ButtonFilterInfo.Active == false && ListOutputTreeview[i].OutputTreviewCriticity == param.ParamP("LogoOutputInformation"))
				{
					OutputListStore.AppendValues(Stetic.IconLoader.LoadIcon(this, ListOutputTreeview[i].OutputTreviewCriticity, Gtk.IconSize.Menu),ListOutputTreeview[i].OutputTreviewDateAndTime.ToString(),param.ParamT(ListOutputTreeview[i].OutputTreviewText));
				}
				if(ButtonFilterError.Active == false && ListOutputTreeview[i].OutputTreviewCriticity == param.ParamP("LogoOutputError"))
				{
					OutputListStore.AppendValues(Stetic.IconLoader.LoadIcon(this, ListOutputTreeview[i].OutputTreviewCriticity, Gtk.IconSize.Menu),ListOutputTreeview[i].OutputTreviewDateAndTime.ToString(),param.ParamT(ListOutputTreeview[i].OutputTreviewText));
				}
				if(ButtonFilterWarning.Active == false && ListOutputTreeview[i].OutputTreviewCriticity == param.ParamP("LogoOutputWarning"))
				{
					OutputListStore.AppendValues(Stetic.IconLoader.LoadIcon(this, ListOutputTreeview[i].OutputTreviewCriticity, Gtk.IconSize.Menu),ListOutputTreeview[i].OutputTreviewDateAndTime.ToString(),param.ParamT(ListOutputTreeview[i].OutputTreviewText));
				}
				if(ButtonFilterQuestion.Active == false && ListOutputTreeview[i].OutputTreviewCriticity == param.ParamP("LogoOutputQuestion"))
				{
					OutputListStore.AppendValues(Stetic.IconLoader.LoadIcon(this, ListOutputTreeview[i].OutputTreviewCriticity, Gtk.IconSize.Menu),ListOutputTreeview[i].OutputTreviewDateAndTime.ToString(),param.ParamT(ListOutputTreeview[i].OutputTreviewText));
				}					
				
			}
			ViewNoteBook.CurrentPage = 0;
			OutputTreeview.Model = OutputListStore;//On ajoute le store dans le treeview		
			OutputTreeview.ShowAll ();//On affiche le tous				
		}
		
//################ Historic Treeview #####################################
		
		//Fonction InitHistoricTreeview
		//Fonction permettant d'initialiser l'arbre permettant d'afficher  l'historique
		public void InitHistoricTreeview()
		{
			//Permet les ligne vertical et honrizontal dans l'output treeview			
			HistoricTreeView.EnableGridLines = TreeViewGridLines.Both;
			
			//On écrit les titre
			LogoViewColumn.Title = param.ParamT("HTVView");
			NumberViewColumn.Title = param.ParamT("HTVNumberView");
			DateAndTimeHistoricColumn.Title = param.ParamT("HTVDateAndTime");
			InfoHistoricColumn.Title = param.ParamT("HTVInfo");
			
			//On insere les cellules dans les colonnes
			LogoViewColumn.PackStart(LogoViewCell,false);
			NumberViewColumn.PackStart(NumberViewCell,true);
			DateAndTimeHistoricColumn.PackStart(DateAndTimeHistoricCell,true);
			InfoHistoricColumn.PackStart(InfoHistoricCell,true);
			
			//On insere les colonnes dans l'historictreeview
			HistoricTreeView.AppendColumn(LogoViewColumn);
			HistoricTreeView.AppendColumn(NumberViewColumn);
			HistoricTreeView.AppendColumn(DateAndTimeHistoricColumn);
			HistoricTreeView.AppendColumn(InfoHistoricColumn);
			
			//On importe des attributs
			LogoViewColumn.AddAttribute(LogoViewCell,"pixbuf",param.ParamI("HTVPositionView"));
			NumberViewColumn.AddAttribute(NumberViewCell,"text",param.ParamI("HTVPositionNumberView"));
			DateAndTimeHistoricColumn.AddAttribute(DateAndTimeHistoricCell,"text",param.ParamI("HTVPositionDateAndTime"));		
			InfoHistoricColumn.AddAttribute(InfoHistoricCell,"text",param.ParamI("HTVPositionInfo"));		
			
			//Création d'un nouveau store pour l'output treeview
			HistoricListStore = new Gtk.TreeStore (typeof (Gdk.Pixbuf),typeof (string),typeof (string),typeof (string));
		
			HistoricTreeView.Model = HistoricListStore;//On ajoute le store dans le treeview		
			HistoricTreeView.ShowAll ();//On affiche le tous				
		}
			
		//Fonction UpdateHistoricTreeView
		//Fonction permettant de mettre à jour l'historic treeview
		public void UpdateHistoricTreeView()
		{
			HistoricListStore.Clear();
			for(int i=ListHistoricTreeview.Count-1;i>=0;i--)
			{
				if(ListHistoricTreeview[i].HistoricTreeviewNumberView == datamanagement.ViewCopy)
				{
					HistoricListStore.AppendValues(Stetic.IconLoader.LoadIcon(this,"gtk-go-forward", Gtk.IconSize.Menu),ListHistoricTreeview[i].HistoricTreeviewNumberView.ToString(),ListHistoricTreeview[i].HistoricTreeviewDateAndTime.ToString(),param.ParamT(ListHistoricTreeview[i].HistoricTreeviewTextParam) + " " + ListHistoricTreeview[i].HistoricTreeviewTextExt);
				}
				else
				{
					HistoricListStore.AppendValues(PngEmpty,ListHistoricTreeview[i].HistoricTreeviewNumberView.ToString(),ListHistoricTreeview[i].HistoricTreeviewDateAndTime.ToString(),param.ParamT(ListHistoricTreeview[i].HistoricTreeviewTextParam) + " " + ListHistoricTreeview[i].HistoricTreeviewTextExt);					
				}
			}
			HistoricTreeView.Model = HistoricListStore;
			ViewNoteBook.CurrentPage = 1;
		}
		
//################ MainNoteBook ##########################################
		
		//Fonction AddTabInMainNoteBook
		//Fonction permttant d'ajouter un onglet dans la listenotebook
		//Arguments :
		//	string name : Nom de l'onglet
		//	Widget child : widget enfant que l'onglet doit aficher
		public void AddTabInMainNoteBook(string _Name, Widget _Child, string _Images, string _SelectType, string _SelectId)
		{
			bool TabUsed = false; //Variable permettant de savoir si un notebook est présent
			int PageNum = 0; //Retourne le numéro de la page présent
			foreach(Notebook not in Listnotebook)  //Pour chaque élément de la liste des notebook
			{
				if(not.SelectType == _SelectType && not.SelectID == _SelectId) //Si le type et l'id sont identique à un élément de la liste
				{
					TabUsed = true; //Il est alors utilisé
					PageNum = MainNoteBook.PageNum(not.widget); //On retourne le numéro de page
				}
			}
			if(!TabUsed) //Si il n'est pas utilé 
			{
				Listnotebook.Add(new Notebook(_Name,_Child,_SelectType,_SelectId,_Images,this)); //on l'joute dans la liste des notebook
				UpdateMainNoteBook(); //On met à jour le notebook
				for(int i=MainNoteBook.Page; i<=MainNoteBook.NPages; i++) //Nous allons à la dernière par du NoteBook
				{
					MainNoteBook.NextPage(); //Nous allons à la dernière page que nous ouvrons	
				}					
			}
			else//Si il est utilisé
			{
				//Nous allons vers la page de l'onglet
				if(MainNoteBook.Page < PageNum)
				{
					int CurrentPage = MainNoteBook.Page;
					for(int i=CurrentPage; i<=PageNum-1; i++)
					{
						MainNoteBook.NextPage(); 
					}						
				}
				else if(MainNoteBook.Page > PageNum)
				{
					int CurrentPage = MainNoteBook.Page;
					for(int i=PageNum; i<=CurrentPage-1; i++)
					{
						MainNoteBook.PrevPage();
					}						
				}
			}
		}	
		
		//Fonction UpdateMainNoteBook
		//Fonction permettant de mettre à jour le notebook
		public void UpdateMainNoteBook()
		{
			foreach(Notebook not in Listnotebook)
			{
				if(!not.Display)
				{
					MainNoteBook.AppendPage(not.widget,not.TabLayout); //Création du nouvel onglet avec en argument le widget et le header box
					MainNoteBook.PageAdded += delegate 
											  { 
												MainNoteBook.SetTabReorderable(MainNoteBook.GetNthPage(-1),true); //Permet de faire bouger les onglets
												MainNoteBook.SetTabDetachable(MainNoteBook.GetNthPage(-1),true);
											  }; 
					not.Display = true;	
				}
			}
			MainNoteBook.ShowAll();	//On affiche le notebook	
			
			
			foreach(Notebook not in Listnotebook)
			{
				if(not.SelectType == param.ParamP("ExTVTypeProject"))
				{
					foreach(Project Pro in datamanagement.ListProject)
					{
						if(Pro.Project_Id.ToString() == not.SelectID)
						{	
							not.CreateLayout(Pro.Project_Name.Replace("_",param.ParamP("ReplaceUnderscor")) + " (Version " + Pro.Project_Version + ")");
							MainNoteBook.SetTabLabel(not.widget,not.TabLayout);
						}
					}
				}
				else if(not.SelectType == param.ParamP("ExTVTypeNode"))
				{
					foreach(Project Pro in datamanagement.ListProject)
					{
						foreach(Node nod in Pro.ReturnListNode())
						{
							if(nod.Node_Id.ToString() == not.SelectID)
							{
								not.CreateLayout(nod.Node_Name.Replace("_",param.ParamP("ReplaceUnderscor")) + " (" + Pro.Project_Name.Replace("_",param.ParamP("ReplaceUnderscor")) + ")");
								MainNoteBook.SetTabLabel(not.widget,not.TabLayout);
							}
						}
					}
				}
				else if(not.SelectType == param.ParamP("Customer"))
				{
					foreach(Project Pro in datamanagement.ListProject)
					{
						foreach(Node nod in Pro.ReturnListNode())
						{
							foreach(Customer cus in nod.ReturnListCustomer())
							{
								if(cus.CustomerId.ToString() == not.SelectID)
								{
									not.CreateLayout(cus.CustomerName.Replace("_",param.ParamP("ReplaceUnderscor")) + " (" + nod.Node_Name.Replace("_",param.ParamP("ReplaceUnderscor")) + ")");
									MainNoteBook.SetTabLabel(not.widget,not.TabLayout);
								}
							}
						}
					}
				}
				else if(not.SelectType == param.ParamP("Scenario"))
				{
					foreach(Project Pro in datamanagement.ListProject)
					{
						foreach(Node nod in Pro.ReturnListNode())
						{
							foreach(Scenario sce in nod.ReturnListScenario())
							{
								if(sce.ScenarioId.ToString() == not.SelectID)
								{
									not.CreateLayout(sce.ScenarioName.Replace("_",param.ParamP("ReplaceUnderscor")) + " (" + nod.Node_Name.Replace("_",param.ParamP("ReplaceUnderscor")) + ")");
									MainNoteBook.SetTabLabel(not.widget,not.TabLayout);
								}
							}
						}
					}
				}				
				else if(not.SelectType == param.ParamP("ExTVTypeNetworkI2C") || not.SelectType == param.ParamP("ExTVTypeNetwork1Wire"))
				{
					foreach(Project Pro in datamanagement.ListProject)
					{
						foreach(Node nod in Pro.ReturnListNode())
						{	
							foreach(Network net in nod.ReturnListNetwork())
							{
								if(net.Network_Id.ToString() == not.SelectID)
								{
									not.CreateLayout(net.Network_Type + " (" + nod.Node_Name.Replace("_",param.ParamP("ReplaceUnderscor")) + ")");
									MainNoteBook.SetTabLabel(not.widget,not.TabLayout);
								}
							}
						}
					}
				}				
				else
				{
					foreach(Boards boards in datamanagement.ListBoards) //Pour chaque carte de la liste des carte xPLduino
					{
						if(not.SelectType == boards.Type) 
						{		
							foreach(Project Pro in datamanagement.ListProject)
							{
								foreach(Node nod in Pro.ReturnListNode())
								{	
									foreach(Network net in nod.ReturnListNetwork())
									{
										foreach(Board boa in net.ReturnListBoard())
										{
											if(boa.Board_Id.ToString() == not.SelectID)
											{
												not.CreateLayout(boa.Board_Name.Replace("_",param.ParamP("ReplaceUnderscor")) + " (" + nod.Node_Name.Replace("_",param.ParamP("ReplaceUnderscor")) + ")");
												MainNoteBook.SetTabLabel(not.widget,not.TabLayout);
											}
										}
									}
								}		
							}
						}
					}
					
					if(not.SelectType == param.ParamP("InstLightingName") || not.SelectType == param.ParamP("InstSwitchName") || not.SelectType == param.ParamP("InstShutterName"))
					{
						foreach(Project Pro in datamanagement.ListProject)
						{
							foreach(Node nod in Pro.ReturnListNode())
							{	
								if(nod.Node_Id.ToString() == not.SelectID)
								{
									if(not.SelectType == param.ParamP("InstLightingName"))
									{
									not.CreateLayout(param.ParamT("ExTVNameLighting") + " (" + nod.Node_Name.Replace("_",param.ParamP("ReplaceUnderscor")) + ")");
									MainNoteBook.SetTabLabel(not.widget,not.TabLayout);		
									}
									else if(not.SelectType == param.ParamP("InstSwitchName"))
									{
									not.CreateLayout(param.ParamT("ExTVNameSwitch") + " (" + nod.Node_Name.Replace("_",param.ParamP("ReplaceUnderscor")) + ")");
									MainNoteBook.SetTabLabel(not.widget,not.TabLayout);	
									}
									else if(not.SelectType == param.ParamP("InstShutterName"))
									{
									not.CreateLayout(param.ParamT("ExTVNameShutter") + " (" + nod.Node_Name.Replace("_",param.ParamP("ReplaceUnderscor")) + ")");
									MainNoteBook.SetTabLabel(not.widget,not.TabLayout);	
									}									
								}
							}
						}							
					}
				}			
			}			
		}
		
		//Fonction UpdateListNoteBook
		//Fontion permttant de supprimer du listnotebook l'élément que nous détruisons avec la croix
		public bool UpdateListNoteBook(Widget _wid)
		{
			foreach(Notebook not in Listnotebook) //Pour chaque élément de la liste des notebook
			{
				if(not.widget == _wid)//Si le widget et celui ue nous avons en paramètre
				{
					Listnotebook.Remove(not); //Nous enlevons le notebook  de la liste
					return true;
				}
			}	
			return false;
		}
		
		//Fonction UpdateWidgetInTab
		//Fonction permettant de mettre à jour les widget
		public void UpdateWidgetInTab()
		{
			foreach(InstanceProperties ip in ListInstanceProperties)
			{
				ip.UpdateWidget();
			}
			foreach(ProjectProperties pp in ListProjectProperties)
			{
				pp.UpdateWidget();
			}
			foreach(NodeProperties np in ListNodeProperties)
			{
				np.UpdateWidget();
			}
			foreach(I2CProperties i2cp in ListI2CProperties)
			{
				i2cp.UpdateWidget();
			}
			foreach(OneWireProperties owp in ListOneWireProperties)
			{
				owp.UpdateWidget();
			}
			foreach(BoardI2CProperties bi2cp in ListBoardI2CProperties)
			{
				bi2cp.UpdateWidget();
			}
			foreach(CustomerEdit ce in ListCustomerEdit)
			{
				ce.UpdateWidget();
			}
			foreach(ScenarioEdit sc in ListScenarioEdit)
			{
				sc.UpdateWidget();
			}
		}
		
		//Fonction DeleteWidgetInTab
		//Fonction permettant de supprimer les widget des liste une fois que nous fermons un onglet
		//Nous allons boucler sur toutes les listes des widget
		//et dans le cas ou le widget que nous passons en paramètre est égale à celui que nous avons dans une de nos liste, nous le supprimons		
		public bool DeleteWidgetInTab(Widget widget)
		{
			foreach(InstanceProperties ip in ListInstanceProperties)
			{
				if(widget == ip)
				{
					ListInstanceProperties.Remove (ip);
					return true;
				}
			}
			foreach(ProjectProperties pp in ListProjectProperties)
			{
				if(widget == pp)
				{
					ListProjectProperties.Remove (pp);
					return true;
				}
			}
			foreach(NodeProperties np in ListNodeProperties)
			{
				if(widget == np)
				{
					ListNodeProperties.Remove (np);
					return true;
				}
			}
			foreach(CustomerEdit ce in ListCustomerEdit)
			{
				if(widget == ce)
				{
					ListCustomerEdit.Remove (ce);
					return true;
				}
			}
			foreach(ScenarioEdit se in ListScenarioEdit)
			{
				if(widget == se)
				{
					ListScenarioEdit.Remove (se);
					return true;
				}
			}
			foreach(I2CProperties i2cp in ListI2CProperties)
			{
				if(widget == i2cp)
				{
					ListI2CProperties.Remove (i2cp);
					return true;
				}
			}
			foreach(OneWireProperties owp in ListOneWireProperties)
			{
				if(widget == owp)
				{
					ListOneWireProperties.Remove (owp);
					return true;
				}
			}
			foreach(BoardI2CProperties bi2cp in ListBoardI2CProperties)
			{
				if(widget == bi2cp)
				{
					ListBoardI2CProperties.Remove (bi2cp);
					return true;
				}
			}	
			return false;
		}
		
		//Fonction CloseTabInCaseOfDelete
		//Fonction permettant de fermer un onglet dans le cas où nous supprimons un élément
		public void CloseTabInCaseOfDelete()
		{
			Step0 :
			//Dans cette partie nous allons nous occuper de fermer les fenêtre des projets ci ceux ci ne sont pas valable
			foreach(ProjectProperties pp in ListProjectProperties)
			{
				if(!pp.WidgetIsCorrect())
				{
					foreach(Notebook not in Listnotebook)
					{		
						if(pp == not.widget)
						{
							not.widget.Destroy();//Destruction du widget	
							Listnotebook.Remove(not);
							ListProjectProperties.Remove (pp);
							goto Step0;
						}
					}
				}
			}	
			
			Step1 :
			//Dans cette partie nous allons nous occuper de fermer les fenêtre des noeud ci ceux ci ne sont pas valable			
			foreach(NodeProperties np in ListNodeProperties)
			{			
				if(!np.WidgetIsCorrect())
				{
					foreach(Notebook not in Listnotebook)
					{		
						if(np == not.widget)
						{
							not.widget.Destroy();//Destruction du widget	
							Listnotebook.Remove(not);
							ListNodeProperties.Remove(np);
							goto Step1;							
						}
					}
				}				
			}
			
			Step2 :
			//Dans cette partie nous allons nous occuper de fermer les fenêtre des réseaux I2C ci ceux ci ne sont pas valable			
			foreach(I2CProperties i2cp in ListI2CProperties)
			{			
				if(!i2cp.WidgetIsCorrect())
				{
					foreach(Notebook not in Listnotebook)
					{		
						if(i2cp == not.widget)
						{
							not.widget.Destroy();//Destruction du widget	
							Listnotebook.Remove(not);
							ListI2CProperties.Remove(i2cp);
							goto Step2;								
						}
					}
				}				
			}		
			
			Step3 :
			//Dans cette partie nous allons nous occuper de fermer les fenêtre des réseaux 1-Wire ci ceux ci ne sont pas valable			
			foreach(OneWireProperties owp in ListOneWireProperties)
			{			
				if(!owp.WidgetIsCorrect())
				{
					foreach(Notebook not in Listnotebook)
					{		
						if(owp == not.widget)
						{
							not.widget.Destroy();//Destruction du widget
							Listnotebook.Remove(not);
							ListOneWireProperties.Remove(owp);
							goto Step3;								
						}
					}
				}				
			}
			
			Step4 :
			//Dans cette partie nous allons nous occuper de fermer les fenêtre des cartes ci ceux ci ne sont pas valable			
			foreach(BoardI2CProperties bi2cp in ListBoardI2CProperties)
			{			
				if(!bi2cp.WidgetIsCorrect())
				{
					foreach(Notebook not in Listnotebook)
					{		
						if(bi2cp == not.widget)
						{
							not.widget.Destroy();//Destruction du widget
							Listnotebook.Remove(not);
							ListBoardI2CProperties.Remove(bi2cp);
							goto Step4;								
						}
					}
				}				
			}	
			
			Step5 :
			//Dans cette partie nous allons nous occuper de fermer les fenêtre des cartes ci ceux ci ne sont pas valable			
			foreach(InstanceProperties ip in ListInstanceProperties)
			{			
				if(!ip.WidgetIsCorrect())
				{
					foreach(Notebook not in Listnotebook)
					{		
						if(ip == not.widget)
						{
							not.widget.Destroy();//Destruction du widget	
							Listnotebook.Remove(not);
							ListInstanceProperties.Remove(ip);
							goto Step5;								
						}
					}
				}				
			}	
			
			Step6 : 
			//Dans cette partie nous allons nous occuper de fermer les fenêtre customer dans le cas où elle ne sont pas valide
			foreach(CustomerEdit ce in ListCustomerEdit)
			{
				if(!ce.WidgetIsCorrect())
				{
					foreach(Notebook not in Listnotebook)
					{
						if(ce == not.widget)
						{
							not.widget.Destroy();
							Listnotebook.Remove(not);
							ListCustomerEdit.Remove(ce);
							goto Step6;	
						}
					}
				}
			}
			
			Step7 : 
			//Dans cette partie nous allons nous occuper de fermer les fenêtre customer dans le cas où elle ne sont pas valide
			foreach(ScenarioEdit se in ListScenarioEdit)
			{
				if(!se.WidgetIsCorrect())
				{
					foreach(Notebook not in Listnotebook)
					{
						if(se == not.widget)
						{
							not.widget.Destroy();
							Listnotebook.Remove(not);
							ListScenarioEdit.Remove(se);
							goto Step7;	
						}
					}
				}
			}			
			
		}
		
		//Fonction OnGoForwardActionActivated
		//Page suivante dans le MainNoteBook
		protected void OnGoForwardActionActivated (object sender, System.EventArgs e)
		{
			MainNoteBook.NextPage();
		}
		
		//Fonction OnGoBackActionActivated
		//Page précédente dans le MainNoteBook
		protected void OnGoBackActionActivated (object sender, System.EventArgs e)
		{
			MainNoteBook.PrevPage();
		}
		
		//Fonction OnCloseActionActivated
		//Permet de fermer toute les fenêtres
		protected void OnCloseActionActivated (object sender, System.EventArgs e)
		{
			Step0 :
			//Dans cette partie nous allons nous occuper de fermer les fenêtre des projets ci ceux ci ne sont pas valable
			foreach(ProjectProperties pp in ListProjectProperties)
			{
				foreach(Notebook not in Listnotebook)
				{		
					if(pp == not.widget)
					{
						not.widget.Destroy();//Destruction du widget	
						Listnotebook.Remove(not);
						ListProjectProperties.Remove (pp);
						goto Step0;
					}
				}	
			}	
			
			Step1 :
			//Dans cette partie nous allons nous occuper de fermer les fenêtre des noeud ci ceux ci ne sont pas valable			
			foreach(NodeProperties np in ListNodeProperties)
			{			
				foreach(Notebook not in Listnotebook)
				{		
					if(np == not.widget)
					{
						not.widget.Destroy();//Destruction du widget	
						Listnotebook.Remove(not);
						ListNodeProperties.Remove(np);
						goto Step1;							
					}
				}				
			}
			
			Step2 :
			//Dans cette partie nous allons nous occuper de fermer les fenêtre des réseaux I2C ci ceux ci ne sont pas valable			
			foreach(I2CProperties i2cp in ListI2CProperties)
			{			
				foreach(Notebook not in Listnotebook)
				{		
					if(i2cp == not.widget)
					{
						not.widget.Destroy();//Destruction du widget	
						Listnotebook.Remove(not);
						ListI2CProperties.Remove(i2cp);
						goto Step2;								
					}
				}					
			}		
			
			Step3 :
			//Dans cette partie nous allons nous occuper de fermer les fenêtre des réseaux 1-Wire ci ceux ci ne sont pas valable			
			foreach(OneWireProperties owp in ListOneWireProperties)
			{			
				foreach(Notebook not in Listnotebook)
				{		
					if(owp == not.widget)
					{
						not.widget.Destroy();//Destruction du widget
						Listnotebook.Remove(not);
						ListOneWireProperties.Remove(owp);
						goto Step3;								
					}
				}			
			}
			
			Step4 :
			//Dans cette partie nous allons nous occuper de fermer les fenêtre des cartes ci ceux ci ne sont pas valable			
			foreach(BoardI2CProperties bi2cp in ListBoardI2CProperties)
			{			
				foreach(Notebook not in Listnotebook)
				{		
					if(bi2cp == not.widget)
					{
						not.widget.Destroy();//Destruction du widget
						Listnotebook.Remove(not);
						ListBoardI2CProperties.Remove(bi2cp);
						goto Step4;								
					}
				}						
			}	
			
			Step5 :
			//Dans cette partie nous allons nous occuper de fermer les fenêtre des cartes ci ceux ci ne sont pas valable			
			foreach(InstanceProperties ip in ListInstanceProperties)
			{			
				foreach(Notebook not in Listnotebook)
				{		
					if(ip == not.widget)
					{
						not.widget.Destroy();//Destruction du widget	
						Listnotebook.Remove(not);
						ListInstanceProperties.Remove(ip);
						goto Step5;								
					}
				}				
			}	
		}		
		
		//Fonction OnMainNoteBookSwitchPage
		//Fonction permettant de détecter un changement de page
		protected void OnMainNoteBookSwitchPage (object o, Gtk.SwitchPageArgs args)
		{
			Gtk.Widget CurrentWidget = MainNoteBook.GetNthPage(MainNoteBook.CurrentPage);
			foreach(InstanceProperties ip in ListInstanceProperties)
			{
				if(ip == CurrentWidget)
				{
					datamanagement.CurrentProjectId = ip.Project_Id;
					UpdateStatusBar();
				}
			}
			foreach(ProjectProperties pp in ListProjectProperties)
			{
				if(pp == CurrentWidget)
				{
					datamanagement.CurrentProjectId = pp.Project_Id;
					UpdateStatusBar();
				}
			}
			foreach(NodeProperties np in ListNodeProperties)
			{
				if(np == CurrentWidget)
				{
					datamanagement.CurrentProjectId = np.Project_Id;
					UpdateStatusBar();
				}
			}
			foreach(I2CProperties i2cp in ListI2CProperties)
			{
				if(i2cp == CurrentWidget)
				{
					datamanagement.CurrentProjectId = i2cp.Project_Id;
					UpdateStatusBar();
				}
			}
			foreach(OneWireProperties owp in ListOneWireProperties)
			{
				if(owp == CurrentWidget)
				{
					datamanagement.CurrentProjectId = owp.Project_Id;
					UpdateStatusBar();
				}
			}
			foreach(BoardI2CProperties bi2cp in ListBoardI2CProperties)
			{
				if(bi2cp == CurrentWidget)
				{
					datamanagement.CurrentProjectId = bi2cp.Project_Id;
					UpdateStatusBar();
				}
			}
			foreach(CustomerEdit ce in ListCustomerEdit)
			{
				if(ce == CurrentWidget)
				{
					datamanagement.CurrentProjectId = ce.Project_Id;
					UpdateStatusBar();
				}
			}
			foreach(ScenarioEdit sc in ListScenarioEdit)
			{
				if(sc == CurrentWidget)
				{
					datamanagement.CurrentProjectId = sc.Project_Id;
					UpdateStatusBar();
				}
			}			
		}		
		
//################ Menu ##################################################
		
		//Fonction OnFileActionActivated
		//Fonction permettant de faire des action sur clic du menu New Project
		protected void OnFileActionActivated (object sender, System.EventArgs e)
		{
			this.Sensitive = false; //On désactive l'écran de fond
			new NewProject(datamanagement,param);//On affiche la fenêtre permettant la création d'un nouveau projet
		}
		
		//#####CREATE CONFIGURATION A SUPPRIMER###################
		//Fonction pour les test creation automatique d'une configuration prédéfini
		protected void OnCreateConfigurationActionActivated (object sender, System.EventArgs e)
		{
			datamanagement.CreateConfiguration();
			UpdateEplorerTreeView();
		}			
		
//################ TreeViewButton #################################################		
		
		//Fonction OnReduceClicked
		//Permet de réduire la fenetre explorateur de projet
		protected void OnReduceClicked (object sender, System.EventArgs e)
		{
			hpaned1.Position = 30;
			scrolledwindow1.Visible = false;
			LabelTreeView.Visible = false;
			Reduce.Visible = false;
			Up.Visible = false;
			Down.Visible = false;
			Expand.Visible = true;
			VBoxButtonNews.Visible = true;
			
			foreach(CustomerEdit ce in ListCustomerEdit)
			{
				ce.UpdateHpaned();
			}
			
			foreach(ScenarioEdit se in ListScenarioEdit)
			{
				se.UpdateHpaned();
			}			
			
			if(TreeViewEplorerValCol2 == param.ParamP("ExTVTypeProject")) //Dans le cas où la cellule 2 est de type projet
			{	
				LabelButton.Visible = true;
				LabelButton.Text = TreeViewEplorerValCol1;
				LabelButton.Angle = 90;
				ButtonNew1.Visible = true;
				ButtonNew1.Label = param.ParamT("MenuNewNode");
				ButtonNew2.Visible = false;
				ButtonNew3.Visible = false;
				RotateTextButton(90);
			}
			else if(TreeViewEplorerValCol2 == param.ParamP("ExTVTypeNode")) //Dans le cas où la cellule 2 est de type projet
			{
				LabelButton.Visible = true;
				foreach(Project Pro in datamanagement.ListProject)
				{
					foreach(Node node in Pro.ReturnListNode())
					{
						if(node.Node_Id == Convert.ToInt32(TreeViewEplorerValCol4))
						{
							LabelButton.Text = TreeViewEplorerValCol1 + " (" + Pro.Project_Name + ")";
						}
					}
				}
				LabelButton.Angle = 90;
				ButtonNew1.Visible = true;
				ButtonNew1.Label = param.ParamT("MenuNewInstance");
				ButtonNew2.Visible = false;
				ButtonNew3.Visible = false;
				RotateTextButton(90);				
			}
			else if(TreeViewEplorerValCol3 == param.ParamP("ExTVTypeNetwork")) //Dans le cas où la cellule 2 est de type projet
			{
				LabelButton.Visible = true;
				foreach(Project Pro in datamanagement.ListProject)
				{
					foreach(Node node in Pro.ReturnListNode())
					{
						foreach(Network net in node.ReturnListNetwork())
						{
							if(net.Network_Id == Convert.ToInt32(TreeViewEplorerValCol4))
							{
								LabelButton.Text = TreeViewEplorerValCol1 + " (" + node.Node_Name + ")";
							}							
						}
					}
				}	
				LabelButton.Angle = 90;
				ButtonNew1.Visible = true;
				ButtonNew1.Label = param.ParamT("MenuNewBoard");
				ButtonNew2.Visible = false;
				ButtonNew3.Visible = false;
				RotateTextButton(90);					
			}
			else
			{
				LabelButton.Visible = false;
				ButtonNew1.Visible = false;
				ButtonNew2.Visible = false;
				ButtonNew3.Visible = false;
			}			
		}
		
		//Fonction OnIncreaseClicked
		//Permet d'agrandir la fenêtre explorateur de projet
		protected void OnIncreaseClicked (object sender, System.EventArgs e)
		{
			//hpaned1.Position = (WinWidth * param.ParamI("MWHPanedExtendDefaultValue")) / 100;
			hpaned1.Position = param.ParamI("MWHPanedExtendDefaultValue");
			scrolledwindow1.Visible = true;
			LabelTreeView.Visible = true;
			Reduce.Visible = true;
			Up.Visible = true;
			Down.Visible = true;			
			Expand.Visible = false;
			VBoxButtonNews.Visible = false;	
			
			foreach(CustomerEdit ce in ListCustomerEdit)
			{
				ce.UpdateHpaned();
			}	
			
			foreach(ScenarioEdit se in ListScenarioEdit)
			{
				se.UpdateHpaned();
			}			
						
		}
		
		//Fonction OnUpClicked
		//Fonction permettant de dérouler un l'arbre depuis un endroit cliqué
		protected void OnUpClicked (object sender, System.EventArgs e)
		{
			ExplorerTreeView.CollapseRow(TreeviewPath);
			Console.WriteLine(hpaned1.Position);
		}
		
		//Fonction OnDownClicked
		//Fonction permettant d'enrouler l'arbre depuis un endroit cliqué
		protected void OnDownClicked (object sender, System.EventArgs e)
		{
			ExplorerTreeView.ExpandRow(TreeviewPath,true);
		}

//################ Menu Button #################################################		
	
		//Fonction OnButtonNewProjectClicked
		//Sur clic sur le bouton nouveau projet on ouvre la fenêtre
		protected void OnButtonNewProjectClicked (object sender, System.EventArgs e)
		{
			this.Sensitive = false; //On désactive l'écran de fond
			new NewProject(datamanagement,param);//On affiche la fenêtre permettant la création d'un nouveau projet			
		}
		
		//Fonction OnMyPreferenceActionActivated
		//Fonction peremettant d'ouvrir la fenêtre des préférences
		protected void OnMyPreferenceActionActivated (object sender, System.EventArgs e)
		{
			this.Sensitive = false; //Permet d'incativer la fenêtre principal
			new ChangePreferences(param,this,pref,datamanagement);
		}
		
		//Fonction OnUndoActionActivated
		//Fonction permettant d'annuler une action
		protected void OnUndoActionActivated (object sender, System.EventArgs e)
		{
			if(Focus is Mono.TextEditor.TextEditor)
			{
                Mono.TextEditor.TextEditor editor = (Mono.TextEditor.TextEditor)Focus;
                Mono.TextEditor.MiscActions.Undo(editor.GetTextEditorData());				
			}
			else
			{
				datamanagement.UndoProject();
			}
		}

		//Fonction OnRedoActionActivated
		//Fonction permettant de rétablir une action
		protected void OnRedoActionActivated (object sender, System.EventArgs e)
		{
			if(Focus is Mono.TextEditor.TextEditor)
			{
                Mono.TextEditor.TextEditor editor = (Mono.TextEditor.TextEditor)Focus;
                Mono.TextEditor.MiscActions.Redo(editor.GetTextEditorData());				
			}
			else
			{			
				datamanagement.RedoProject();
			}
		}
		
		//Fonction OnUndoButtonClicked
		//Fonction permettant d'annuler une action		
		protected void OnUndoButtonClicked (object sender, System.EventArgs e)
		{
			if(Focus is Mono.TextEditor.TextEditor)
			{
                Mono.TextEditor.TextEditor editor = (Mono.TextEditor.TextEditor)Focus;
                Mono.TextEditor.MiscActions.Undo(editor.GetTextEditorData());				
			}
			else
			{
				datamanagement.UndoProject();
			}
		}
		
		//Fonction OnRedoButtonClicked
		//Fonction permettant de rétablir une action		
		protected void OnRedoButtonClicked (object sender, System.EventArgs e)
		{
			if(Focus is Mono.TextEditor.TextEditor)
			{
                Mono.TextEditor.TextEditor editor = (Mono.TextEditor.TextEditor)Focus;
                Mono.TextEditor.MiscActions.Redo(editor.GetTextEditorData());				
			}
			else
			{			
				datamanagement.RedoProject();
			}
		}		
		
		
//################ Output Button #################################################			
		
		//Fonction OnButtonFilterInfoToggled
		//Fcontion perttant de filtrer les informations
		protected void OnButtonFilterInfoToggled (object sender, System.EventArgs e)
		{
			OutputListStore.Clear();
			for(int i=ListOutputTreeview.Count-1;i>=0;i--)
			{
				if(ButtonFilterInfo.Active == false && ListOutputTreeview[i].OutputTreviewCriticity == param.ParamP("LogoOutputInformation"))
				{
					OutputListStore.AppendValues(Stetic.IconLoader.LoadIcon(this, ListOutputTreeview[i].OutputTreviewCriticity, Gtk.IconSize.Menu),ListOutputTreeview[i].OutputTreviewDateAndTime.ToString(),param.ParamT(ListOutputTreeview[i].OutputTreviewText));
				}
				if(ButtonFilterError.Active == false && ListOutputTreeview[i].OutputTreviewCriticity == param.ParamP("LogoOutputError"))
				{
					OutputListStore.AppendValues(Stetic.IconLoader.LoadIcon(this, ListOutputTreeview[i].OutputTreviewCriticity, Gtk.IconSize.Menu),ListOutputTreeview[i].OutputTreviewDateAndTime.ToString(),param.ParamT(ListOutputTreeview[i].OutputTreviewText));
				}
				if(ButtonFilterWarning.Active == false && ListOutputTreeview[i].OutputTreviewCriticity == param.ParamP("LogoOutputWarning"))
				{
					OutputListStore.AppendValues(Stetic.IconLoader.LoadIcon(this, ListOutputTreeview[i].OutputTreviewCriticity, Gtk.IconSize.Menu),ListOutputTreeview[i].OutputTreviewDateAndTime.ToString(),param.ParamT(ListOutputTreeview[i].OutputTreviewText));
				}
				if(ButtonFilterQuestion.Active == false && ListOutputTreeview[i].OutputTreviewCriticity == param.ParamP("LogoOutputQuestion"))
				{
					OutputListStore.AppendValues(Stetic.IconLoader.LoadIcon(this, ListOutputTreeview[i].OutputTreviewCriticity, Gtk.IconSize.Menu),ListOutputTreeview[i].OutputTreviewDateAndTime.ToString(),param.ParamT(ListOutputTreeview[i].OutputTreviewText));
				}					
				
			}
			OutputTreeview.Model = OutputListStore;//On ajoute le store dans le treeview		
			OutputTreeview.ShowAll ();//On affiche le tous			
		}
		
		//Fonction OnButtonFilterErrorToggled
		//Fcontion perttant de filtrer les erreurs		
		protected void OnButtonFilterErrorToggled (object sender, System.EventArgs e)
		{
			OutputListStore.Clear();
			for(int i=ListOutputTreeview.Count-1;i>=0;i--)
			{
				if(ButtonFilterInfo.Active == false && ListOutputTreeview[i].OutputTreviewCriticity == param.ParamP("LogoOutputInformation"))
				{
					OutputListStore.AppendValues(Stetic.IconLoader.LoadIcon(this, ListOutputTreeview[i].OutputTreviewCriticity, Gtk.IconSize.Menu),ListOutputTreeview[i].OutputTreviewDateAndTime.ToString(),param.ParamT(ListOutputTreeview[i].OutputTreviewText));
				}
				if(ButtonFilterError.Active == false && ListOutputTreeview[i].OutputTreviewCriticity == param.ParamP("LogoOutputError"))
				{
					OutputListStore.AppendValues(Stetic.IconLoader.LoadIcon(this, ListOutputTreeview[i].OutputTreviewCriticity, Gtk.IconSize.Menu),ListOutputTreeview[i].OutputTreviewDateAndTime.ToString(),param.ParamT(ListOutputTreeview[i].OutputTreviewText));
				}
				if(ButtonFilterWarning.Active == false && ListOutputTreeview[i].OutputTreviewCriticity == param.ParamP("LogoOutputWarning"))
				{
					OutputListStore.AppendValues(Stetic.IconLoader.LoadIcon(this, ListOutputTreeview[i].OutputTreviewCriticity, Gtk.IconSize.Menu),ListOutputTreeview[i].OutputTreviewDateAndTime.ToString(),param.ParamT(ListOutputTreeview[i].OutputTreviewText));
				}
				if(ButtonFilterQuestion.Active == false && ListOutputTreeview[i].OutputTreviewCriticity == param.ParamP("LogoOutputQuestion"))
				{
					OutputListStore.AppendValues(Stetic.IconLoader.LoadIcon(this, ListOutputTreeview[i].OutputTreviewCriticity, Gtk.IconSize.Menu),ListOutputTreeview[i].OutputTreviewDateAndTime.ToString(),param.ParamT(ListOutputTreeview[i].OutputTreviewText));
				}					
				
			}
			OutputTreeview.Model = OutputListStore;//On ajoute le store dans le treeview		
			OutputTreeview.ShowAll ();//On affiche le tous			
		}
		
		//Fonction OnButtonFilterQuestionToggled
		//Fcontion perttant de filtrer les questions			
		protected void OnButtonFilterQuestionToggled (object sender, System.EventArgs e)
		{
			OutputListStore.Clear();
			for(int i=ListOutputTreeview.Count-1;i>=0;i--)
			{
				if(ButtonFilterInfo.Active == false && ListOutputTreeview[i].OutputTreviewCriticity == param.ParamP("LogoOutputInformation"))
				{
					OutputListStore.AppendValues(Stetic.IconLoader.LoadIcon(this, ListOutputTreeview[i].OutputTreviewCriticity, Gtk.IconSize.Menu),ListOutputTreeview[i].OutputTreviewDateAndTime.ToString(),param.ParamT(ListOutputTreeview[i].OutputTreviewText));
				}
				if(ButtonFilterError.Active == false && ListOutputTreeview[i].OutputTreviewCriticity == param.ParamP("LogoOutputError"))
				{
					OutputListStore.AppendValues(Stetic.IconLoader.LoadIcon(this, ListOutputTreeview[i].OutputTreviewCriticity, Gtk.IconSize.Menu),ListOutputTreeview[i].OutputTreviewDateAndTime.ToString(),param.ParamT(ListOutputTreeview[i].OutputTreviewText));
				}
				if(ButtonFilterWarning.Active == false && ListOutputTreeview[i].OutputTreviewCriticity == param.ParamP("LogoOutputWarning"))
				{
					OutputListStore.AppendValues(Stetic.IconLoader.LoadIcon(this, ListOutputTreeview[i].OutputTreviewCriticity, Gtk.IconSize.Menu),ListOutputTreeview[i].OutputTreviewDateAndTime.ToString(),param.ParamT(ListOutputTreeview[i].OutputTreviewText));
				}
				if(ButtonFilterQuestion.Active == false && ListOutputTreeview[i].OutputTreviewCriticity == param.ParamP("LogoOutputQuestion"))
				{
					OutputListStore.AppendValues(Stetic.IconLoader.LoadIcon(this, ListOutputTreeview[i].OutputTreviewCriticity, Gtk.IconSize.Menu),ListOutputTreeview[i].OutputTreviewDateAndTime.ToString(),param.ParamT(ListOutputTreeview[i].OutputTreviewText));
				}					
				
			}
			OutputTreeview.Model = OutputListStore;//On ajoute le store dans le treeview		
			OutputTreeview.ShowAll ();//On affiche le tous					
		}
		
		//Fonction OnButtonFilterWarningToggled
		//Fcontion perttant de filtrer les warning			
		protected void OnButtonFilterWarningToggled (object sender, System.EventArgs e)
		{
			OutputListStore.Clear();
			for(int i=ListOutputTreeview.Count-1;i>=0;i--)
			{
				if(ButtonFilterInfo.Active == false && ListOutputTreeview[i].OutputTreviewCriticity == param.ParamP("LogoOutputInformation"))
				{
					OutputListStore.AppendValues(Stetic.IconLoader.LoadIcon(this, ListOutputTreeview[i].OutputTreviewCriticity, Gtk.IconSize.Menu),ListOutputTreeview[i].OutputTreviewDateAndTime.ToString(),param.ParamT(ListOutputTreeview[i].OutputTreviewText));
				}
				if(ButtonFilterError.Active == false && ListOutputTreeview[i].OutputTreviewCriticity == param.ParamP("LogoOutputError"))
				{
					OutputListStore.AppendValues(Stetic.IconLoader.LoadIcon(this, ListOutputTreeview[i].OutputTreviewCriticity, Gtk.IconSize.Menu),ListOutputTreeview[i].OutputTreviewDateAndTime.ToString(),param.ParamT(ListOutputTreeview[i].OutputTreviewText));
				}
				if(ButtonFilterWarning.Active == false && ListOutputTreeview[i].OutputTreviewCriticity == param.ParamP("LogoOutputWarning"))
				{
					OutputListStore.AppendValues(Stetic.IconLoader.LoadIcon(this, ListOutputTreeview[i].OutputTreviewCriticity, Gtk.IconSize.Menu),ListOutputTreeview[i].OutputTreviewDateAndTime.ToString(),param.ParamT(ListOutputTreeview[i].OutputTreviewText));
				}
				if(ButtonFilterQuestion.Active == false && ListOutputTreeview[i].OutputTreviewCriticity == param.ParamP("LogoOutputQuestion"))
				{
					OutputListStore.AppendValues(Stetic.IconLoader.LoadIcon(this, ListOutputTreeview[i].OutputTreviewCriticity, Gtk.IconSize.Menu),ListOutputTreeview[i].OutputTreviewDateAndTime.ToString(),param.ParamT(ListOutputTreeview[i].OutputTreviewText));
				}					
				
			}
			OutputTreeview.Model = OutputListStore;//On ajoute le store dans le treeview		
			OutputTreeview.ShowAll ();//On affiche le tous					
		}
		
		//Fonction OnEraseOutputClicked
		//Fcontion perttant de vider l'output		
		protected void OnEraseOututClicked (object sender, System.EventArgs e)
		{
			EraseOutputFunction();
		}	
		
		//Fonction EraseOutput
		//Fonction permettant d'effacer l'output
		public void EraseOutputFunction()
		{
			OutputListStore.Clear();
			for(int i=ListOutputTreeview.Count-1;i>=0;i--)
			{
				ListOutputTreeview.Remove(ListOutputTreeview[i]);
			}
			this.ButtonFilterInfo.TooltipText = "0 " + param.ParamT("TextOutputInformation");
			this.ButtonFilterInfo.Image = ButtonFilterInfoImage;
				
			this.ButtonFilterWarning.TooltipText =  "0 " + param.ParamT("TextOutputWarning");
			this.ButtonFilterWarning.Image = ButtonFilterWarningImage;
				
			this.ButtonFilterError.TooltipText =  "0 " + param.ParamT("TextOutputError");
			this.ButtonFilterError.Image = ButtonFilterErrorImage;
				
			this.ButtonFilterQuestion.TooltipText =  "0 " + param.ParamT("TextOutputQuestion");
			this.ButtonFilterQuestion.Image = ButtonFilterQuestionImage;					
		}
		
//###### Button New (Node, Board ...) quand l'explorateur de projet est réduit  ##########			
		
		//Fonction OnButtonNew1Clicked
		//Ouverture des fenetres nouveau
		protected void OnButtonNew1Clicked (object sender, System.EventArgs e)
		{
			if(TreeViewEplorerValCol2 == param.ParamP("ExTVTypeProject")) //Dans le cas où la cellule 2 est de type projet
			{	
				this.Sensitive = false; //Permet d'incativer la fenêtre principal
				new NewNode(datamanagement,Convert.ToInt32(TreeViewEplorerValCol4),param); //Appel la fenêtre permettant l'ajout d'un nouveau noeud
			}
			else if(TreeViewEplorerValCol2 == param.ParamP("ExTVTypeNode")) //Dans le cas où la cellule 2 est de type projet
			{
				this.Sensitive = false; //Permet d'incativer la fenêtre principal
				new NewInstance(datamanagement,Convert.ToInt32(TreeViewEplorerValCol4),param); //Appel la fenêtre permettant l'ajout d'une nouvelle instance
			}
			else if(TreeViewEplorerValCol3 == param.ParamP("ExTVTypeNetwork")) //Dans le cas où la cellule 2 est de type projet
			{
				this.Sensitive = false; //Permet d'incativer la fenêtre principal
				new NewBoard (datamanagement,Convert.ToInt32(TreeViewEplorerValCol4),TreeViewEplorerValCol2,param); //Appel la fenêtre permettant l'ajout d'une nouvelle carte					
			}
			else
			{

			}	
		}
		
//################ Save Button #############################################		
		
		//Fonction OnSaveActionActivated
		//Fonction permettant d'effectuer des action lors d'un clic sur le bouton save
		protected void OnSaveActionActivated (object sender, System.EventArgs e)
		{
			RecDataInMemoryBeforeSave();
			datamanagement.SaveProject(false,datamanagement.CurrentProjectId);
		}
		
		//Fonction OnSaveAsActionActivated
		//Permet d'enregistrer tous les projets
		protected void OnSaveAsActionActivated (object sender, System.EventArgs e)
		{
			RecDataInMemoryBeforeSave();	
			datamanagement.SaveProject(true,0);
		}	
		
		//Fonction OnButtonSaveProjectClicked
		//Fonction permettant d'enregistrer un projet
		protected void OnButtonSaveProjectClicked (object sender, System.EventArgs e)
		{
			RecDataInMemoryBeforeSave();
			datamanagement.SaveProject(false,datamanagement.CurrentProjectId);
		}

		//Fonction OnButtonSaveProjectClicked
		//Fonction permettant d'enregistrer tous les projets
		protected void OnButtonSaveAllProjectClicked (object sender, System.EventArgs e)
		{
			if(Focus is Mono.TextEditor.TextEditor)
			{
				Gtk.Widget CurrentWidget = MainNoteBook.GetNthPage(MainNoteBook.CurrentPage);
				foreach(CustomerEdit ce in ListCustomerEdit)
				{
					if(ce == CurrentWidget)
					{
						ce.SaveCurrentData();
					}
				}
				foreach(ScenarioEdit sc in ListScenarioEdit)
				{
					if(sc == CurrentWidget)
					{
						sc.SaveCurrentDataScenario();
						sc.SaveCurrentDataFunction();
					}
				}					
			}			
			datamanagement.SaveProject(true,0);
		}		
		
		//Fonction RecDataInMemoryBeforeSave
		//Fonction permettant d'enregistrer en mémoire les texteeditor avant de fair un save
		public void RecDataInMemoryBeforeSave()
		{
			if(Focus is Mono.TextEditor.TextEditor)
			{
				Gtk.Widget CurrentWidget = MainNoteBook.GetNthPage(MainNoteBook.CurrentPage);
				foreach(CustomerEdit ce in ListCustomerEdit)
				{
					if(ce == CurrentWidget)
					{
						ce.SaveCurrentData();
					}
				}
				foreach(ScenarioEdit sc in ListScenarioEdit)
				{
					if(sc == CurrentWidget)
					{
						sc.SaveCurrentDataScenario();
						sc.SaveCurrentDataFunction();
					}
				}					
			}
		}
		
//################ Open Button ##################################################		
		
		//Fonction OnOpenActionActivated
		//Fonction permettant d'ouvrir un projet
		protected void OnOpenActionActivated (object sender, System.EventArgs e)
		{
			//this.Sensitive = false;
			FileChooserDialog chooser = new FileChooserDialog(param.ParamT("ChooseAProject"),this,FileChooserAction.Open, param.ParamT("Cancel"), ResponseType.Cancel,param.ParamT("Open"),ResponseType.Accept); //Permet d'ouvrir un fenêtre de dialogue permettant de choisir un dossier
			if( chooser.Run() == (int)ResponseType.Accept ) //Si nous faisons OK
			{
				string Filename = chooser.Filename;
				if(datamanagement.FileIsCrypte(Filename))
				{
					new AskPassword(Filename,param,this);
					chooser.Destroy();
					return;
				}
				else
				{
					chooser.Destroy();
					OpenProject(Filename,"");
				}
			}	
			else
			{
				chooser.Destroy();
			}
			this.Sensitive = true;
		}
		
		//Fonction OnButtonOpenProjectClicked
		//Fonction permettant d'ouvrir un projet avec le bouton
		protected void OnButtonOpenProjectClicked (object sender, System.EventArgs e)
		{
			//this.Sensitive = false;
			FileChooserDialog chooser = new FileChooserDialog(param.ParamT("ChooseAProject"),this,FileChooserAction.Open, param.ParamT("Cancel"), ResponseType.Cancel,param.ParamT("Open"),ResponseType.Accept); //Permet d'ouvrir un fenêtre de dialogue permettant de choisir un dossier
			if( chooser.Run() == (int)ResponseType.Accept ) //Si nous faisons OK
			{
				string Filename = chooser.Filename;
				if(datamanagement.FileIsCrypte(Filename))
				{
					new AskPassword(Filename,param,this);
					chooser.Destroy();
					return;
				}
				else
				{
					chooser.Destroy();
					OpenProject(Filename,"");
				}
			}	
			else
			{
				chooser.Destroy();
			}			
			this.Sensitive = true;			
		}		
		
		//Fonction OpenProject
		//Fonction permettant d'ouvrir un projet
		public void OpenProject(string _DirectoryName, string _password)
		{
			if(datamanagement.OpenProject(_DirectoryName,_password) == 0)
			{
				
				MessageDialog message = new MessageDialog (this, 
	                                      			  	   DialogFlags.DestroyWithParent,
		                              				       MessageType.Error, 
	                                                       ButtonsType.Close, param.ParamT("ErrorPasswordOrInValidFile"));	
				ResponseType messageresult = (ResponseType)message.Run ();
				if (messageresult == ResponseType.Close)
					message.Destroy();//On détruit la boite de dialogue
			}		
		}
		
//################ Compilation des fichiers ##################################################			
		
		//Fonction OnButtonCheckEmbeddedClicked
		//Fonction permettant de générer du code
		protected void OnButtonCheckEmbeddedClicked (object sender, System.EventArgs e)
		{
			RecDataInMemoryBeforeSave();
			EraseOutputFunction();
			if(datamanagement.SaveProject(false, datamanagement.CurrentProjectId))
			{
				ButtonCheckEmbedded.Sensitive = false;
				ButtonLoadEmbedded.Sensitive = false;
				LoadEmbeddedAction.Sensitive = false;
				ExtractEmbeddedAction.Sensitive = false;
				
				Thread threadcompil =new Thread(()=> datamanagement.CompileFile(ComboboxSelectNode.ActiveText));
				threadcompil.IsBackground = true;
				threadcompil.Start();
			}
		}		
		
		//Fonction OnVerifyEmbeddedActionActivated
		//Fonction permettant de générer du code
		protected void OnVerifyEmbeddedActionActivated (object sender, System.EventArgs e)
		{
			RecDataInMemoryBeforeSave();
			EraseOutputFunction();
			if(datamanagement.SaveProject(false, datamanagement.CurrentProjectId))
			{
				ButtonCheckEmbedded.Sensitive = false;
				ButtonLoadEmbedded.Sensitive = false;
				LoadEmbeddedAction.Sensitive = false;
				ExtractEmbeddedAction.Sensitive = false;
				
				Thread threadcompil =new Thread(()=> datamanagement.CompileFile(ComboboxSelectNode.ActiveText));
				threadcompil.IsBackground = true;
				threadcompil.Start();
			}
		}		
		
		//Fonction ActiveCompileAndLoadButton
		//Fonction permettant d'activer les boutons à la fin d'une compilation
		public void ActiveCompileAndLoadButtonCompilation(bool CompilationIsCorrect)
		{
			ButtonCheckEmbedded.Sensitive = true;
			ButtonLoadEmbedded.Sensitive = true;
			LoadEmbeddedAction.Sensitive = true;		
			ExtractEmbeddedAction.Sensitive = true;
			
			if(CompilationIsCorrect)	
			{
				AddLineOutput(param.ParamI("OutputInformation"),"CompleteCompilation");
			}
			else
			{
				AddLineOutput(param.ParamI("OutputError"),"ErrorCompilation");
			}
		}
		
		//Fonction UpdateProgressBar
		//Fonction permettant de mettre à jour la progress bar
		public void UpdateProgressBar(double _Fraction)
		{
			ProgressBar.Adjustment.Value = _Fraction;
		}
		
//################ Chargement sur cible #####################################################

		protected void OnButtonLoadEmbeddedClicked (object sender, System.EventArgs e)
		{
			if(File.Exists("/dev/tty" + ComboboxSelectUsb.ActiveText.Replace(" ","")))
			{
				ButtonCheckEmbedded.Sensitive = false;
				ButtonLoadEmbedded.Sensitive = false;
				LoadEmbeddedAction.Sensitive = false;	
				ExtractEmbeddedAction.Sensitive = false;
				
				Thread threadload =new Thread(()=> datamanagement.LoadBoard(ComboboxSelectUsb.ActiveText.Replace(" ",""),ComboboxSelectNode.ActiveText));
				threadload.IsBackground = true;
				threadload.Start();
			}
			else
			{
				AddLineOutput(param.ParamI("OutputError"),"ConnectAProgrammer");
				UpdateComboboxSelectUsb();
			}
		}	
				
		//Fonction ActiveCompileAndLoadButton
		//Fonction permettant d'activer les boutons à la fin d'une compilation
		public void ActiveCompileAndLoadButtonLoad(bool LoadIsCorrect)
		{
			ButtonCheckEmbedded.Sensitive = true;
			ButtonLoadEmbedded.Sensitive = true;
			ExtractEmbeddedAction.Sensitive = true;
			LoadEmbeddedAction.Sensitive = true;		
			
			if(LoadIsCorrect)	
			{
				AddLineOutput(param.ParamI("OutputInformation"),"CompleteLoad");
			}
			else
			{
				AddLineOutput(param.ParamI("OutputError"),"ErrorLoad");
			}
		}		
		
//################ Fermeture de la fenêtre ##################################################				
		
		//Fonction OnDeleteEvent
		//Fonction permetant la fermeture de la fenetre
		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			CloseProject();
			Application.Quit ();
			a.RetVal = true;	
		}				
		
		//Fonction OnQuitActionActivated
		//Fonction permettant de faire des action sur Appuie du bouton quit
		protected void OnQuitActionActivated (object sender, System.EventArgs e)
		{
			this.Sensitive = false; //Permet d'incativer la fenêtre principal			
			if(pref.ConfirmClose)
			{
				MessageDialog massagequit = new MessageDialog (this, 
                          			  	   	DialogFlags.DestroyWithParent,
                      				       	MessageType.Question, 
                                           	ButtonsType.YesNo, param.ParamT("QuestionQuitProject"));
				
				ResponseType messageresultquit = (ResponseType)massagequit.Run (); //On lance la box
				
				if (messageresultquit == ResponseType.Yes) //On regarde le résultat et si Oui
				{
					CloseProject();
					massagequit.Destroy();
					Application.Quit ();
				}	
				else
				{
					massagequit.Destroy();
				}
			}
			else
			{
				CloseProject();
				Application.Quit ();
			}
			this.Sensitive = true; //Permet d'activer la fenêtre principal	
		}
		
		//Fonction CloseProject
		//Fonction permettant d'enregistrer les projets sur fermeture
		public void CloseProject()
		{
			foreach(Project pro in datamanagement.ListProject)
			{
				if(!pro.ProjectIsSave)
				{
					MessageDialog message = new MessageDialog (this, 
		                                      			  	   DialogFlags.DestroyWithParent,
			                              				       MessageType.Question, 
		                                                       ButtonsType.YesNo, param.ParamT("QuestionSaveProject") + pro.Project_Name + " ?");
		            
					ResponseType messageresult = (ResponseType)message.Run (); //On lance la box
					
					if (messageresult == ResponseType.Yes) //On regarde le résultat et si Oui
					{
						datamanagement.SaveProject(false,pro.Project_Id);
						message.Destroy();
					}
					else
					{
						message.Destroy();
					}			
				}
			}				
		}
		

//################ Autre ################################################################
	
	//################ Status Bar  ######################################################	
		
		//Fonction UpdateStatusBar
		//Fonction permettant de mettre à jour la barre du bas
		public void UpdateStatusBar()
		{	
			string ProjectRecord = "";
			foreach(Project pro in datamanagement.ListProject)
			{
				if( pro.Project_Id == datamanagement.CurrentProjectId )
				{
					if(!pro.ProjectIsSave)
					{
						ProjectRecord = "*";
					}
					LabelInfoProject.Markup = "  " + param.ParamT("CurrentProject") + "<b>" + pro.Project_Name + "</b>" + " " + ProjectRecord;
					LabelPathProject.Markup = param.ParamT("PathCurrentProject") + "<b>" + pro.Project_SavePath + "</b>";				
				}
			}
			if(datamanagement.CurrentProjectId == 0)
			{
				LabelInfoProject.Markup = "  " + param.ParamT("CurrentProject") + "<b>" + param.ParamT("EmptyProject") + "</b>" + " " + ProjectRecord;
				LabelPathProject.Markup = param.ParamT("PathCurrentProject") + "<b>" + param.ParamT("EmptyProject") + "</b>";				
			}
		}
		
	//################ Combo Box Select Node ############################################			
		
		//Fonction UpdateComboboxSelectNode
		//Fonction permettant de mettre à jour la selection du noeud pour le chargement d'une carte
		public void UpdateComboboxSelectNode()
		{
			
			for(int i = 0;i<1000;i++)
			{
				ComboboxSelectNode.RemoveText(0);
			}
			
				
			foreach(Project pro in datamanagement.ListProject)
			{					
				foreach(Node node in pro.ReturnListNode())
				{
					ComboboxSelectNode.AppendText(pro.Project_Name + "/" + node.Node_Name);
					
					Gtk.TreeIter iter;
					ComboboxSelectNode.Model.IterNthChild(out iter,MemorisePositionInComboboxSelectNode);
					ComboboxSelectNode.SetActiveIter (iter);					
				}					
			}	
			
			if(ComboboxSelectNode.ActiveText != "" && ComboboxSelectNode.ActiveText != null)
			{
				ExtractEmbeddedAction.Sensitive = true;
				LoadEmbeddedAction.Sensitive = true;	
				ButtonCheckEmbedded.Sensitive = true;
				ButtonLoadEmbedded.Sensitive = true;
			}
			else
			{
				ExtractEmbeddedAction.Sensitive = false;
				LoadEmbeddedAction.Sensitive = false;	
				ButtonCheckEmbedded.Sensitive = false;
				ButtonLoadEmbedded.Sensitive = false;				
			}
		}
		
		//Fonction ClearComboboxSelectNode
		//Fonction permettant de vider le ComboboxSelectNode
		public void ClearComboboxSelectNode()
		{
			foreach(Project pro in datamanagement.ListProject)
			{					
				foreach(Node node in pro.ReturnListNode())
				{
					ComboboxSelectNode.RemoveText(0);
				}
			}	
			if(ComboboxSelectNode.ActiveText != "" && ComboboxSelectNode.ActiveText != null)
			{
				ExtractEmbeddedAction.Sensitive = true;
				LoadEmbeddedAction.Sensitive = true;	
				ButtonCheckEmbedded.Sensitive = true;
				ButtonLoadEmbedded.Sensitive = true;
			}
			else
			{
				ExtractEmbeddedAction.Sensitive = false;
				LoadEmbeddedAction.Sensitive = false;	
				ButtonCheckEmbedded.Sensitive = false;
				ButtonLoadEmbedded.Sensitive = false;				
			}			
		}
		
			//Fonction OnComboboxSelectNodeChanged
			//Fonction permettant de faire des actions sur changement du ComboboxSelectNode
			protected void OnComboboxSelectNodeChanged (object sender, System.EventArgs e)
			{
				if(ComboboxSelectNode.Active >= 0)
				{
					MemorisePositionInComboboxSelectNode = ComboboxSelectNode.Active;
				}
			}
		
	//################ Combo Box Select Usb ############################################			
		
		//Fonction UpdateComboboxSelectUsb
		//Fonction permettant de mettre à jour la liste des port USB
		public void UpdateComboboxSelectUsb()
		{
			LoadEmbeddedAction.Sensitive = false;	
			ButtonLoadEmbedded.Sensitive = false;
			for(int i=0; i<20; i++)
			{
				ComboboxSelectUsb.RemoveText(0);
			}
			for(int i=0; i<20; i++)
			{
				if(File.Exists("/dev/ttyUSB" + i))
				{
					ComboboxSelectUsb.AppendText("USB " + i);
					LoadEmbeddedAction.Sensitive = true;	
					ButtonLoadEmbedded.Sensitive = true;
				}
			}
			
			Gtk.TreeIter iter;
			ComboboxSelectUsb.Model.IterNthChild(out iter,0);
			if(iter.Stamp != 0)
			{
				ComboboxSelectUsb.SetActiveIter (iter);			
			}
		}
		
		//Fonction OnButtonReloadUSBClicked
		//Fonction permettant de mettre à jour les port USB
		protected void OnButtonReloadUSBClicked (object sender, System.EventArgs e)
		{
			UpdateComboboxSelectUsb();		
		}		
		
		
	
		//Fonction InitPanedAndMouvementAuthor
		//Fonction permettant l'initialisation de la taille des paned et l'autorisation de les déplacer
		public void InitPanedAndMouvementAuthor()
		{
			if (datamanagement.ListProject.Count >= 1) //Dans le cas ou le nombre de projet est supérieur à 0
			{
				vbox2.Visible = true;
				ViewNoteBook.Visible = true;
				this.GetSize(out WinWidth , out WinHeight);	//On récupere la taille de la fenêtre
				//hpaned1.Position = (WinWidth * param.ParamI("MWHPanedExtendDefaultValue")) / 100; //On impose du hpaned à un certain pourcentage
				this.hpaned1.Position = param.ParamI("MWHPanedExtendDefaultValue") - 70;
				this.vpaned1.Position = (WinHeight * param.ParamI("MWVPanedExtendDefaultValue")) / 100; //On impose du vpaned à un certain pourcentage
				ReduceHistoryPanel.Visible = true;
			}		
		}
		
		//Fonction ReturVpanedPosition
		//Fonction de permettant de retouner la position de Vpaned pour la calibration des fenetre dans le notebook
		public int ReturnVpanedPosition()
		{
			return vpaned1.Position;
		}
		
		//Fonction ReturVpanedPosition
		//Fonction de permettant de retouner la position de Hpaned pour la calibration des fenetre dans le notebook	
		public int ReturnHpanedPosition()
		{
			int height =0;
			int width = 0;
			this.GetSize(out width , out height);		
			return (width) - hpaned1.Position;
		}
		
		//Fonction RotateTextButton
		//Fcontion permettant de mettre le texte des bouton à angle donnée
		public void RotateTextButton(int angle)
		{
		    for (int i = 0; i < ButtonNew1.Children.Length; i++) {
		        Gtk.Widget w = ButtonNew1.Children [i];
		        if (w.GetType () == typeof(Gtk.Label)) {
		            Gtk.Label labelWidget = (Gtk.Label)w;
		            labelWidget.Angle = angle;
		        }
		    }
			
		    for (int i = 0; i < ButtonNew2.Children.Length; i++) {
		        Gtk.Widget w = ButtonNew2.Children [i];
		        if (w.GetType () == typeof(Gtk.Label)) {
		            Gtk.Label labelWidget = (Gtk.Label)w;
		            labelWidget.Angle = angle;
		        }
		    }
			
		    for (int i = 0; i < ButtonNew3.Children.Length; i++) {
		        Gtk.Widget w = ButtonNew3.Children [i];
		        if (w.GetType () == typeof(Gtk.Label)) {
		            Gtk.Label labelWidget = (Gtk.Label)w;
		            labelWidget.Angle = angle;
		        }
		    }				
		}
		
		//Fonction OnExplorerTreeViewCursorChanged
		//Dans le cas où nous avons une modification d'un entité séléctionne dans l'explorateur de projet
		//On mémorise le chemin utilisé pour le pliage ou de le dépliage de l'arbre
		protected void OnExplorerTreeViewCursorChanged (object sender, System.EventArgs e)
		{
			TreeSelection selection = (sender as TreeView).Selection;
			
			TreeModel model;
			TreeIter iter;
			
			// The iter will point to the selected row
			if(selection.GetSelected(out model, out iter))
			{
				TreeviewPath = model.GetPath(iter);
			}
		}
		
		//Fonction UndoRedoInactif
		//Permet de rendre inactif les boutons annuler et rétablir
		public void UndoRedoInactif(string _UndoRedo, bool _Inactif)
		{
			if(_UndoRedo == "undo")
			{
				this.undoAction.Sensitive = _Inactif;
				this.UndoButton.Sensitive = _Inactif;
			}
			else if(_UndoRedo == "redo")
			{
				this.redoAction.Sensitive = _Inactif;
				this.RedoButton.Sensitive = _Inactif;
			}
		}
		
		//Fonction OnHistoricTreeViewButtonReleaseEvent
		//Fonction permettant de changer la position du view dans l'historique
		protected void OnHistoricTreeViewButtonReleaseEvent (object o, Gtk.ButtonReleaseEventArgs args)
		{
				string PositionNumberView = "";
				TreeSelection selection = HistoricTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelHistoricTreeView, out IterProject)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					PositionNumberView = (string) TreeModelHistoricTreeView.GetValue (IterProject, param.ParamI("HTVPositionNumberView"));
					datamanagement.ViewProject(Convert.ToInt16(PositionNumberView));
				}
				
		}

		//Fonction OnReduceHistoryPanelClicked
		//Fonction permettant de réduire le panel historic et erreur
		protected void OnReduceHistoryPanelClicked (object sender, System.EventArgs e)
		{
			ViewNoteBook.Visible = false;
			ExpendHistoryPanel.Visible = true;
			ReduceHistoryPanel.Visible = false;
			
			foreach(ScenarioEdit sc in ListScenarioEdit)
			{
				sc.UpdateVpaned(90);
			}				
		}
		
		//Fonction OnExpendHistoryPanelClicked
		//Fonction permettant d'afficher l'historic panel
		protected void OnExpendHistoryPanelClicked (object sender, System.EventArgs e)
		{
			ViewNoteBook.Visible = true;
			ExpendHistoryPanel.Visible = false;
			ReduceHistoryPanel.Visible = true;
			
			foreach(ScenarioEdit sc in ListScenarioEdit)
			{
				sc.UpdateVpaned(70);
			}				
		}

		//Fonction OnEraseOutputClicked
		//Permet de vider l'output
		protected void OnEraseOutputClicked (object sender, System.EventArgs e)
		{
			EraseOutputFunction();
		}
		
		//Fonction OnAboutActionActivated
		//Fonction permettant d'afficher la version
		protected void OnAboutActionActivated (object sender, System.EventArgs e)
		{
				string about = param.ParamT("VersionText") + param.ParamP("Version") + "\n" +
							   param.ParamT("EmailText") + param.ParamP("Email");
			
				MessageDialog message = new MessageDialog (this, 
	                                      			  	   DialogFlags.DestroyWithParent,
		                              				       MessageType.Info,
	                                                       ButtonsType.Ok, about);
	            
				ResponseType messageresult = (ResponseType)message.Run (); //On lance la box
				if(messageresult == ResponseType.Ok)
				{
					message.Destroy();
				}
		}

		//################ Lecture des données Ethernet ############################################
		
		//Fonction OnButtonReadEthernetClicked
		//Fonction permettant de lancer un thread pour lire les données provenant de l'ethernet (trame UDP)
		protected void OnButtonReadEthernetClicked (object sender, System.EventArgs e)
		{
		/*	
			Thread ReadUdp =new Thread(()=> datamanagement.ReadEthernetData());
			ReadUdp.IsBackground = true;
			ReadUdp.Start();	*/

int Port = 3865;

var server = new UdpClient(Port);
server.BeginReceive(result => {
		IPEndPoint sender1 = null;
        var data = server.EndReceive(result, ref sender1);
        var value1 = Encoding.UTF8.GetString(data);
		Console.WriteLine(value1);
        server.Close ();
}, null);			
			
		}
		

		
		
	}
}