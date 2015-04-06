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


namespace xPLduinoManager
{

	[System.ComponentModel.ToolboxItem(true)]
	public partial class NodeProperties : Gtk.Bin
	{
		public DataManagement datamanagement;
		public Param param;
		public Int32 Node_Id;
		public Int32 Project_Id;
		public string SelectOption;
		
		public string DebugOldLangage;
		public string ParamOldLangage;
		public string OptionOldLangage;
		public string CustomerOldLangage;
		public string ScenarioOldLangage;
		
		//Utilisation de ses variables pour le treeview gerant les paramètres
		public global::Gtk.TreeViewColumn PropertiesTypeColumn;
		public global::Gtk.TreeViewColumn PropertiesValueColumn;
		public global::Gtk.TreeViewColumn PropertiesEmptyColumn;
		
		public global::Gtk.CellRendererText PropertiesTypeCell;
		public global::Gtk.CellRendererText PropertiesValueCell;
		
		public global::Gtk.TreeStore PropertiesListStore;		
		
		//Utilisation de ses variables pour le treeview gerant les options
		public global::Gtk.TreeViewColumn OptionNameColumn;
		
		public global::Gtk.CellRendererPixbuf OptionPixCell;
		public global::Gtk.CellRendererText OptionCell;
		
		public global::Gtk.TreeStore ParameterListStore;
		public global::Gdk.Pixbuf PngDebug;
		public global::Gdk.Pixbuf PngParameter;		
		public global::Gdk.Pixbuf PngCustomer;
		public global::Gdk.Pixbuf PngScenario;
		
		public global::Gtk.TreeIter IterNode;
		
		public global::Gtk.TreeModel TreeModelOptionTreeView;
		
		public string OptionName;
		public bool DisplayAllNode;
		
		//Utilisation de ses variables pour le treeview child : général
		
		public List<TreeViewColumn> ChildColumnList;
		public global::Gtk.TreeIter IterChild;
		public global::Gtk.TreeModel TreeModelChildTreeView;	
		
		//Utilisation de ses variables pour le treeview child : option debug
		public global::Gtk.TreeViewColumn Child_Debug_NameColumn;
		public global::Gtk.TreeViewColumn Child_Debug_ValueColumn;
		public global::Gtk.TreeViewColumn Child_Debug_DescriptionColumn;
		public global::Gtk.TreeViewColumn Child_Debug_EmptyColumn;
		
		public global::Gtk.CellRendererText Child_Debug_NameCell;
		public global::Gtk.CellRendererToggle Child_Debug_ValueCell;
		public global::Gtk.CellRendererText Child_Debug_DescriptionCell;
		
		public global::Gtk.CellRendererText Child_Debug_EmptyCell;
		
		public global::Gtk.TreeStore Child_Debug_ListStore;		
		
		//Utilisation de ses variables pour le treeview gerant les options
		public global::Gtk.TreeViewColumn OptionTypeColumn;
		public global::Gtk.TreeViewColumn OptionValueColumn;
		public global::Gtk.TreeViewColumn OptionEmptyColumn;
		
		public global::Gtk.CellRendererText OptionTypeCell;
		public global::Gtk.CellRendererCombo OptionValueCell;
		
		public global::Gtk.TreeStore OptionListStore;	
		
		public Gtk.ListStore ChooseData;
		
		//Utilisation de ses variable pour le treeview des customer
		public global::Gtk.TreeViewColumn CustomerUseColumn;
		public global::Gtk.TreeViewColumn CustomerNameColumn;
		public global::Gtk.TreeViewColumn CustomerScenarioColumn;
		public global::Gtk.TreeViewColumn CustomerNoteColumn;
		public global::Gtk.TreeViewColumn CustomerIdColumn;	
		public global::Gtk.TreeViewColumn CustomerEmptyColumn;	
		
		public global::Gtk.CellRendererToggle CustomerUseCell;
		public global::Gtk.CellRendererText CustomerNameCell;
		public global::Gtk.CellRendererCombo CustomerScenarioCell;
		public global::Gtk.CellRendererText CustomerNoteCell;
		public global::Gtk.CellRendererText CustomerIdCell;
		
		public global::Gtk.TreeStore CustomerListStore;
		
		//Utilisation de ses variables pour le treeview des scénarios
		public global::Gtk.TreeViewColumn ScenarioNameColumn;
		public global::Gtk.TreeViewColumn ScenarioNoteColumn;
		public global::Gtk.TreeViewColumn ScenarioIdColumn;	
		public global::Gtk.TreeViewColumn ScenarioEmptyColumn;	
		
		public global::Gtk.CellRendererText ScenarioNameCell;
		public global::Gtk.CellRendererText ScenarioNoteCell;
		public global::Gtk.CellRendererText ScenarioIdCell;
		
		public global::Gtk.TreeStore ScenarioListStore;		
		public Gtk.ListStore ListScenario;
		
		//Constructeur NodeProperties
		public NodeProperties (DataManagement _datamanagement, Param _param, Int32 _Node_Id)
		{
			this.datamanagement = _datamanagement;
			this.param = _param;
			this.Node_Id = _Node_Id;
			this.Build ();
			
			ChildColumnList = new List<TreeViewColumn>();	
			ChooseData = new Gtk.ListStore(typeof (string));
			ListScenario = new Gtk.ListStore(typeof (string));
			
			InitWidget();
			InitOptionsTreeView();
			UpdateOptionsTreeView();
			
			SelectOption = "";
			
			OptionTreeView.ButtonReleaseEvent += new ButtonReleaseEventHandler(OptionsTreeViewButtonRelease);
			
			//Permet de retourner l'id du projet
			foreach(Project pro in datamanagement.ListProject.Values)
			{
				foreach(Node node in pro.ReturnListNode())
				{
					if(node.Node_Id == _Node_Id)
					{
						datamanagement.CurrentProjectId = pro.Project_Id;
						Project_Id = pro.Project_Id;
					}
				}
			}			
		}

		//Fonction InitWidget
		//Fonction permettant d'initialiser le widget
		public void InitWidget()
		{
			LabelChildTreeView.Text = param.ParamT("NP_LabTVChild_Name_Choose");
			NoteLabel.Text = param.ParamT("NP_NoteLabel");
			vpaned3.Position = 0;
			hpaned2.Position = (datamanagement.mainwindow.ReturnHpanedPosition() * param.ParamI("NoteHPanedPurcent")) / 100;
			
			foreach(Project Pro in datamanagement.ListProject.Values)
			{
				foreach(Node node in Pro.ReturnListNode())
				{
					if(node.Node_Id == Node_Id)
					{
						TextViewNote.Buffer.Text = node.Node_Note;			
					}
				}
			}	
			
			vpaned3.Position = (datamanagement.mainwindow.ReturnVpanedPosition() * param.ParamI("NoteVPanedPurcent") / 100);	
			InitChildTreeView_Properties();
			UpdateChildTreeView_Properties();
		}		
		
//###################### TV Options  ####################################
		
		//Fonction InitOptionsTreeView
		//Fcontion permettant d'initialiser le treeview affichant les différentes options pour un projet
		public void InitOptionsTreeView()
		{
			//Création des colonne du treeview
			OptionNameColumn = new Gtk.TreeViewColumn();
			
			//Nouveau Logo
			PngDebug = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconDebug"));
			PngParameter = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconProperties"));	
			PngCustomer = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconCustomer"));
			PngScenario = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconScenario"));
			
			//Nous donnons un titre au colonnes
			OptionNameColumn.Title = param.ParamT("NP_TVParam_NameLabel");
			
			//Création des cellules
			OptionPixCell = new Gtk.CellRendererPixbuf ();
			OptionCell = new Gtk.CellRendererText ();
			
			//On associe des cellules au colonne
			OptionNameColumn.PackStart(OptionPixCell, false);
			OptionNameColumn.PackStart(OptionCell, true);	
			
			//Ajout des colonnes dans OptionTreeView
			OptionTreeView.AppendColumn (OptionNameColumn);
			
			//Ajout des attibut à chaque colonne
			OptionNameColumn.AddAttribute (OptionPixCell, "pixbuf", param.ParamI("NP_TVParam_PostionPixBuff"));
			OptionNameColumn.AddAttribute (OptionCell,"text",param.ParamI("NP_TVParam_PostionName"));	
			
			//Création d'un nouveau store, param : icon, texte explorer, texte type, texte name, texte id
			ParameterListStore = new Gtk.TreeStore (typeof (Gdk.Pixbuf),typeof (string));
			
			//On écrit le store dans le treeview
			OptionTreeView.Model = ParameterListStore;
			OptionTreeView.ShowAll ();						
		}
		
		//Fonction UpdateOptionsTreeView
		//Fonction permettant de mettre à jour l'arbre des options
		public void UpdateOptionsTreeView()
		{
			ParameterListStore.Clear();
			ParameterListStore.AppendValues(PngParameter,param.ParamT("NP_TVParam_ParameterParam"));
			ParameterListStore.AppendValues(PngParameter,param.ParamT("NP_TVParam_ParameterOption"));
			ParameterListStore.AppendValues(PngDebug,param.ParamT("NP_TVParam_ParameterDebug"));
			ParameterListStore.AppendValues(PngCustomer,param.ParamT("NP_TVParam_ParameterCustomer"));
			ParameterListStore.AppendValues(PngScenario,param.ParamT("NP_TVParam_ParameterScenario"));
		}
		
			//Fonction OptionsTreeViewButtonRelease
			//Fonction permettant d'executer des action sur appuie de la souris
			[GLib.ConnectBefore]
		    private void OptionsTreeViewButtonRelease(object o, ButtonReleaseEventArgs args)
			{			
				if(args.Event.Button == param.ParamI("LeftClic")) //Si le bouton cliquer est le clic gauche
				{
					TreeSelection selection = (o as TreeView).Selection; //Nous allons cree un arbre de selection
					if(selection.GetSelected(out TreeModelOptionTreeView, out IterNode)) //Nous cherchons la valeur selectionné dans l'arbre de selection
					{
						OptionName = (string) TreeModelOptionTreeView.GetValue (IterNode, param.ParamI("NP_TVParam_PostionName")); //Nous mettons la valeur de la 1ere cellule dans un string (texte)
						SelectOption = OptionName;
						
						if (OptionName == param.ParamT("NP_TVParam_ParameterDebug"))
						{
							DebugOldLangage = OptionName;
							foreach(TreeViewColumn TVchild in ChildColumnList)
							{
								ChildTreeView.RemoveColumn(TVchild);
							}
						
							if(ChildColumnList.Count >0)
							{ 
								ChildColumnList.RemoveRange(0,ChildColumnList.Count);
							}
							InitChildTreeView_Debug();
							UpdateChildTreeView_Debug();
						}
						else if(OptionName == param.ParamT("NP_TVParam_ParameterParam"))
						{
							ParamOldLangage = OptionName;
							foreach(TreeViewColumn TVchild in ChildColumnList)
							{
								ChildTreeView.RemoveColumn(TVchild);
							}
						
							if(ChildColumnList.Count >0)
							{ 
								ChildColumnList.RemoveRange(0,ChildColumnList.Count);
							}	
							InitChildTreeView_Properties();
							UpdateChildTreeView_Properties();
						}
						else if(OptionName == param.ParamT("NP_TVParam_ParameterOption"))
						{
							OptionOldLangage = OptionName;
							foreach(TreeViewColumn TVchild in ChildColumnList)
							{
								ChildTreeView.RemoveColumn(TVchild);
							}
						
							if(ChildColumnList.Count >0)
							{ 
								ChildColumnList.RemoveRange(0,ChildColumnList.Count);
							}	
							InitChildTreeView_Option();
							UpdateChildTreeView_Option();
						}	
						else if(OptionName == param.ParamT("NP_TVParam_ParameterCustomer"))
						{
							CustomerOldLangage = OptionName;
							foreach(TreeViewColumn TVchild in ChildColumnList)
							{
								ChildTreeView.RemoveColumn(TVchild);
							}
						
							if(ChildColumnList.Count >0)
							{ 
								ChildColumnList.RemoveRange(0,ChildColumnList.Count);
							}	
							InitChildTreeView_Customer();
							UpdateChildTreeView_Customer();
						}
						else if(OptionName == param.ParamT("NP_TVParam_ParameterScenario"))
						{
							ScenarioOldLangage = OptionName;
							foreach(TreeViewColumn TVchild in ChildColumnList)
							{
								ChildTreeView.RemoveColumn(TVchild);
							}
						
							if(ChildColumnList.Count >0)
							{ 
								ChildColumnList.RemoveRange(0,ChildColumnList.Count);
							}	
							InitChildTreeView_Scenario();
							UpdateChildTreeView_Scenario();
						}					
					}
				}
			}	
		
//####################### TV Child Choice Properties ####################################		
		
		//Fonction InitPropertiesTreeView
		//Fonction permettant d'initialiser l'arbre des propriété
		public void InitChildTreeView_Properties()
		{
			LabelChildTreeView.Text = param.ParamT("NP_LabTVChild_Name_Parameters");
			ChildTreeView.EnableGridLines = TreeViewGridLines.Both;
			
			//Ajout de deux colonnes dans TreeviewProperties
			PropertiesTypeColumn = new Gtk.TreeViewColumn();
			PropertiesValueColumn = new Gtk.TreeViewColumn();
			PropertiesEmptyColumn = new Gtk.TreeViewColumn();
			
			//Nous donnons un titre au colonne
			PropertiesTypeColumn.Title = param.ParamT("NP_TVPro_TypeLabel");
			PropertiesValueColumn.Title = param.ParamT("NP_TVPro_ValueLabel");
			
			ChildColumnList.Add (PropertiesTypeColumn);
			ChildColumnList.Add (PropertiesValueColumn);
			ChildColumnList.Add (PropertiesEmptyColumn);
			
			//Les colonnes sont redimensionnable
			PropertiesTypeColumn.Resizable = true;
			PropertiesValueColumn.Resizable = true;
					
			//Création des cellules
			PropertiesTypeCell = new Gtk.CellRendererText ();
			PropertiesValueCell = new Gtk.CellRendererText ();	
						
			//La cellule Value est éditable
			PropertiesValueCell.Editable = true;
			
			//Nous appelons une fonction lorsque nous cliquons que la cellule
			PropertiesValueCell.Edited += PropertiesValueCell_Edited;			
			
			//On associe des cellule au colonne
			PropertiesTypeColumn.PackStart(PropertiesTypeCell, true);
			PropertiesValueColumn.PackStart (PropertiesValueCell, true);
			
			//Ajout des colonnes dans ExplorerTreeView
			ChildTreeView.AppendColumn (PropertiesTypeColumn);
			ChildTreeView.AppendColumn (PropertiesValueColumn);
			ChildTreeView.AppendColumn (PropertiesEmptyColumn);
			
			//Ajout des attibuts à chaque colonne
			PropertiesTypeColumn.AddAttribute(PropertiesTypeCell,"text",param.ParamI("NP_TVPro_TypePosition"));
			PropertiesValueColumn.AddAttribute (PropertiesValueCell, "text", param.ParamI("NP_TVPro_ValuePosition"));
			
			//Création d'un nouveau store, param : texte Type, texte Value
			PropertiesListStore = new Gtk.TreeStore(typeof (string),typeof (string));
			
			//On écrit le store dans le treeview
			ChildTreeView.Model = PropertiesListStore;
			ChildTreeView.ShowAll ();				
		}
		
		//Fonction UpdatePropertiesTreeView
		//Fonction permettant de mettre à jour l'arbre des propriétés
		public void UpdateChildTreeView_Properties()
		{
			PropertiesListStore.Clear();
			foreach(Project Pro in datamanagement.ListProject.Values)
			{
				foreach(Node node in Pro.ReturnListNode())
				{
					if(node.Node_Id == Node_Id)
					{						
						PropertiesListStore.AppendValues(param.ParamT("NP_TVPro_LabelLine1"),node.Node_Name);
						PropertiesListStore.AppendValues(param.ParamT("NP_TVPro_LabelLine2"),node.Node_IP);
						PropertiesListStore.AppendValues(param.ParamT("NP_TVPro_LabelLine3"),node.Node_GTWIP);
						PropertiesListStore.AppendValues(param.ParamT("NP_TVPro_LabelLine4"),node.Node_Mac);
						if(node.Node_DHCP)
						{
							PropertiesListStore.AppendValues(param.ParamT("NP_TVPro_LabelLine5"),param.ParamT("NP_YesValue"));
						}
						else
						{
							PropertiesListStore.AppendValues(param.ParamT("NP_TVPro_LabelLine5"),param.ParamT("NP_NoValue"));
						}
						if(node.Node_WebServer)
						{
							PropertiesListStore.AppendValues(param.ParamT("NP_TVPro_LabelLine6"),param.ParamT("NP_YesValue"));
						}
						else
						{
							PropertiesListStore.AppendValues(param.ParamT("NP_TVPro_LabelLine6"),param.ParamT("NP_NoValue"));
						}
					}
				}
			}						
		}
		
			//Fonction PropertiesValueCell_Edited
			//Fonction permettant de mettre à jour des information en cliquant sur une cellule
			private void PropertiesValueCell_Edited (object o, Gtk.EditedArgs args)
			{
				string NodeNameSelected = "";
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelOptionTreeView, out IterNode)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					NodeNameSelected = (string) TreeModelOptionTreeView.GetValue (IterNode, param.ParamI("NP_TVPro_ValuePosition")); //Nous mettons la valeur de la 1ere cellule dans un string (texte)			
				}
		
				Gtk.TreeIter iter; //Conception d'un TreeIter permettant de retourner la position de la valeur cliqué
				PropertiesListStore.GetIter (out iter, new Gtk.TreePath (args.Path)); //Nous allons cherché cette valeur qui est arg.Path ainsi que le nouveau text dans args.Text
			
				if(Convert.ToByte(args.Path) == param.ParamI("NP_TVPro_NamePostion") && (args.NewText != NodeNameSelected))
				{
					if(args.NewText != "") //Si le texte est pas vide
					{
						if(args.NewText.Length <= param.ParamI("CarSizeMax"))
						{				
							datamanagement.ModifyNode(args.NewText,param.ParamI("MoNo_ChoiceName"),Node_Id); //On fait la mise à jour du nom
							datamanagement.mainwindow.UpdateEplorerTreeView(); //On met à jour ExplorerTreeView
						}
						else //Dans le cas ou le nouveuau nom est supérieur à 16 caratère
						{
							datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameNodeTooLong"); //on affiche un message d'erreur
						}
					}
					else //Dans le cas ou le texte est vide
					{
						datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameNodeEmpty"); //on affiche un message d'erreur
					}
				}
				else if(Convert.ToByte(args.Path) == param.ParamI("NP_TVPro_IPPosition"))
				{
					if(datamanagement.AnalyseIP(args.NewText)) //On verifie que l'adresse IP est correct
					{				
						datamanagement.ModifyNode(args.NewText,param.ParamI("MoNo_ChoiceIP"),Node_Id);
					}
					else
					{
						datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"IPNodeInvalid"); //on affiche un message d'erreur
					}	
				}
				else if(Convert.ToByte(args.Path) == param.ParamI("NP_TVPro_GTWIPPosition"))
				{
					if(datamanagement.AnalyseIP(args.NewText)) //On verifie que l'adresse IP est correct
					{							
						datamanagement.ModifyNode(args.NewText,param.ParamI("MoNo_ChoiceGTWIP"),Node_Id);
					}
					else
					{
						datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"IPNodeInvalid"); //on affiche un message d'erreur
					}					
				}			
				else if(Convert.ToByte(args.Path) == param.ParamI("NP_TVPro_MacPosition"))
				{
					datamanagement.ModifyNode("",param.ParamI("MoNo_ChoiceMac"),Node_Id); //On fait la mise à jour du mac
				}
				else if(Convert.ToByte(args.Path) == param.ParamI("NP_TVPro_DHCPPosition"))
				{
					if(args.NewText == param.ParamT("NP_NoValue"))
					{
						datamanagement.ModifyNode("True",param.ParamI("MoNo_ChoiceDHCP"),Node_Id); //On fait la mise à jour du dhcp
					}
					else if(args.NewText == param.ParamT("NP_YesValue"))
					{
						datamanagement.ModifyNode("False",param.ParamI("MoNo_ChoiceDHCP"),Node_Id); //On fait la mise à jour du dhcp
					}
				}
				else if(Convert.ToByte(args.Path) == param.ParamI("NP_TVPro_WebServerPosition"))
				{
					if(args.NewText == param.ParamT("NP_NoValue"))
					{
						datamanagement.ModifyNode("True",param.ParamI("MoNo_ChoiceWebServer"),Node_Id); //On fait la mise à jour du dhcp
					}
					else if(args.NewText == param.ParamT("NP_YesValue"))
					{
						datamanagement.ModifyNode("False",param.ParamI("MoNo_ChoiceWebServer"),Node_Id); //On fait la mise à jour du dhcp
					}
				}				
			
			
				UpdateChildTreeView_Properties(); //On met à jour le PropertiesTreeview
			}			
		
//###################### TV Child Choice Debug #########################	
		
		//Fonction InitChildTreeView_Debug
		//Fcontion permettant d'initialiser le treeview cbild lorsque le choix option debug
		public void InitChildTreeView_Debug()
		{
			LabelChildTreeView.Text = param.ParamT("NP_LabTVChild_Name_DebugList");
			//Visibilité des lignes et des colonnes
			ChildTreeView.EnableGridLines = TreeViewGridLines.Both;
			
			//Ajout des colonnes
			Child_Debug_NameColumn = new Gtk.TreeViewColumn();
			Child_Debug_ValueColumn = new Gtk.TreeViewColumn();
			Child_Debug_DescriptionColumn = new Gtk.TreeViewColumn();
			Child_Debug_EmptyColumn = new Gtk.TreeViewColumn();	
			
			//On ajoute les colonnes dans une liste
			ChildColumnList.Add(Child_Debug_NameColumn);
			ChildColumnList.Add(Child_Debug_ValueColumn);
			ChildColumnList.Add(Child_Debug_DescriptionColumn);
			ChildColumnList.Add(Child_Debug_EmptyColumn);
			
			//Colonne redimensionnable
			Child_Debug_NameColumn.Resizable = true;
			Child_Debug_ValueColumn.Resizable = true;
			Child_Debug_DescriptionColumn.Resizable = true;
			Child_Debug_EmptyColumn.Resizable = true;	
			
			//Nom des colonne
			Child_Debug_NameColumn.Title = param.ParamT("NP_TVChild_OpDebug_TypeDebug");
			Child_Debug_ValueColumn.Title = param.ParamT("NP_TVChild_OpDebug_ValueDebug");
			Child_Debug_DescriptionColumn.Title = param.ParamT("NP_TVChild_OpDebug_DescDebug");
							
			//Création des cellules
			Child_Debug_NameCell = new Gtk.CellRendererText ();
			Child_Debug_ValueCell = new Gtk.CellRendererToggle ();
			Child_Debug_DescriptionCell = new Gtk.CellRendererText ();
			
			//On rend les cellules éditables
			Child_Debug_NameCell.Editable = false;
			Child_Debug_ValueCell.Activatable = true;
			Child_Debug_DescriptionCell.Editable = false;
			
			//En cas d'edition des cellule, on appel les fonction associé		
		    Child_Debug_ValueCell.Toggled += Child_Debug_ValueCell_Toggled;
			
			//On associe des cellules aux colonnes			
			Child_Debug_NameColumn.PackStart (Child_Debug_NameCell,true);		
			Child_Debug_ValueColumn.PackStart (Child_Debug_ValueCell,true);
			Child_Debug_DescriptionColumn.PackStart (Child_Debug_DescriptionCell,true);	
			
			
			//Ajout des colonnes dans ExplorerTreeView
			ChildTreeView.AppendColumn (Child_Debug_NameColumn);		
			ChildTreeView.AppendColumn (Child_Debug_ValueColumn);	
			ChildTreeView.AppendColumn (Child_Debug_DescriptionColumn);	
			ChildTreeView.AppendColumn (Child_Debug_EmptyColumn);
			
			
			//Ajout des attibuts à chaque colonne
			Child_Debug_NameColumn.AddAttribute(Child_Debug_NameCell,"text",param.ParamI("NP_TVChild_OpDebug_PositionType"));			
			Child_Debug_ValueColumn.AddAttribute(Child_Debug_ValueCell,"active",param.ParamI("NP_TVChild_OpDebug_PositionValue"));
			Child_Debug_DescriptionColumn.AddAttribute(Child_Debug_DescriptionCell,"text",param.ParamI("NP_TVChild_OpDebug_PositionDesc"));	
			
			//Création d'un nouveau store
			Child_Debug_ListStore = new Gtk.TreeStore(typeof(string),typeof(bool),typeof(string),typeof(string));	
			
			//On écrit le store dans le treeview
			ChildTreeView.Model = Child_Debug_ListStore;
			ChildTreeView.ShowAll ();			
					
		}		
		
		//Fonction UpdateChildTreeView_Node
		//Fonction permettant de mettre à jour le child treeview
		public void UpdateChildTreeView_Debug()
		{
			Child_Debug_ListStore.Clear();
			foreach(Project Pro in datamanagement.ListProject.Values)
			{	
				foreach(Node node in Pro.ReturnListNode())
				{
					if(node.Node_Id == Node_Id)
					{
						foreach(Debug deb in node.Debug_)
						{
							if(param.Langage == param.ParamP("FrenchLangage"))
							{
								Child_Debug_ListStore.AppendValues(deb.Name,deb.Value,deb.FrenchDescriptionDebug);
							}
							else if(param.Langage == param.ParamP("EnglishLangage"))
							{
								Child_Debug_ListStore.AppendValues(deb.Name,deb.Value,deb.EnglishDescriptionDebug);	
							}
						}
						
					}
				}
			}
			
			ChildTreeView.Model = Child_Debug_ListStore;
			ChildTreeView.ShowAll();			
		}	
		
			//Fonction Child_Debug_ValueCell_Toggled
			//Fonction faite lors d'un clic sur un check button
			private void Child_Debug_ValueCell_Toggled (object o, Gtk.ToggledArgs args) 
			{
				bool Value = false; //Initialisation d'un bool permettant de récuperer la valeur lors d'un clic
				Gtk.TreeIter iter;
				if (Child_Debug_ListStore.GetIter (out iter, new Gtk.TreePath(args.Path))) {
					Value = (bool) Child_Debug_ListStore.GetValue(iter,param.ParamI("NP_TVChild_OpDebug_PositionValue")); //on fait l'acquisition de la valeur
					Child_Debug_ListStore.SetValue(iter,param.ParamI("NP_TVChild_OpDebug_PositionValue"),!Value);//On affiche la valeur inversé
				}	
			}
		
//###################### TV Child Choice Customer #######################
		
		//Fonction InitChildTreeView_Customer
		//Fonction permettant d'initialiser l'arbre pour afficher les customer
		public void InitChildTreeView_Customer()
		{
			LabelChildTreeView.Text = param.ParamT("NP_LabTVChild_Name_CustomerList");
			//Visibilité des lignes et des colonnes
			ChildTreeView.EnableGridLines = TreeViewGridLines.Both;		
			
			//Ajout des colonnes
			CustomerUseColumn = new Gtk.TreeViewColumn();
			CustomerNameColumn = new Gtk.TreeViewColumn();
			CustomerScenarioColumn = new Gtk.TreeViewColumn();
			CustomerNoteColumn = new Gtk.TreeViewColumn();
			CustomerIdColumn = new Gtk.TreeViewColumn();	
			CustomerEmptyColumn = new Gtk.TreeViewColumn();
			
			//On ajoute les colonnes dans une liste
			ChildColumnList.Add(CustomerUseColumn);
			ChildColumnList.Add(CustomerNameColumn);
			ChildColumnList.Add(CustomerScenarioColumn);
			ChildColumnList.Add(CustomerNoteColumn);			
			ChildColumnList.Add(CustomerIdColumn);
			ChildColumnList.Add(CustomerEmptyColumn);	
			
			//Colonne redimensionnable
			CustomerUseColumn.Resizable = false;
			CustomerNameColumn.Resizable = true;
			CustomerScenarioColumn.Resizable = true;
			CustomerNoteColumn.Resizable = true;
			
			//Taille des colonnes
			CustomerNoteColumn.MinWidth = 350;
			
			//Nom des colonnes
			CustomerUseColumn.Title = param.ParamT("NP_TVChild_OpCustomer_Use");
			CustomerScenarioColumn.Title = param.ParamT("NP_TVChild_OpCustomer_Scenario");
			CustomerNameColumn.Title = param.ParamT("NP_TVChild_OpCustomer_Name");
			CustomerNoteColumn.Title = param.ParamT("NP_TVChild_OpCustomer_Note");		
			
			//Visibilité des colonnes
			CustomerIdColumn.Visible = false;
			
			//Création des cellules
			CustomerUseCell = new CellRendererToggle();
			CustomerNameCell = new CellRendererText();
			CustomerScenarioCell = new CellRendererCombo();
			CustomerNoteCell = new CellRendererText();
			CustomerIdCell = new CellRendererText();
			
			//Cellule éditable
			CustomerUseCell.Activatable = true;
			CustomerNameCell.Editable = true;
			CustomerScenarioCell.Editable = true;
			CustomerNoteCell.Editable = true;
			
			CustomerScenarioCell.Model = ListScenario;
			CustomerScenarioCell.TextColumn = 0;
			
			//Appel des fonction pour la modification des cellules
			CustomerUseCell.Toggled += CustomerUseCell_Toggled;
			CustomerNameCell.Edited += CustomerNameCell_Edited;
			CustomerScenarioCell.Edited += CustomerScenarioCell_Edited;
			CustomerNoteCell.Edited += CustomerNoteCell_Edited;
			
			//On associe des cellules aux colonnes		
			CustomerUseColumn.PackStart(CustomerUseCell,true);
			CustomerNameColumn.PackStart(CustomerNameCell,true);
			CustomerScenarioColumn.PackStart(CustomerScenarioCell,true);			
			CustomerNoteColumn.PackStart(CustomerNoteCell,true);
			CustomerIdColumn.PackStart(CustomerIdCell,true);
			
			//Ajout des colonnes dans ExplorerTreeView
			ChildTreeView.AppendColumn (CustomerUseColumn);
			ChildTreeView.AppendColumn (CustomerNameColumn);
			ChildTreeView.AppendColumn (CustomerScenarioColumn);
			ChildTreeView.AppendColumn (CustomerNoteColumn);	
			ChildTreeView.AppendColumn (CustomerIdColumn);
			ChildTreeView.AppendColumn (CustomerEmptyColumn);	
			
			//Ajout des attibuts à chaque colonne
			CustomerUseColumn.AddAttribute(CustomerUseCell,"active",param.ParamI("NP_TVChild_OpCustomer_PositionUse"));
			CustomerNameColumn.AddAttribute(CustomerNameCell,"text",param.ParamI("NP_TVChild_OpCustomer_PositionName"));
			CustomerScenarioColumn.AddAttribute(CustomerScenarioCell,"text",param.ParamI("NP_TVChild_OpCustomer_PositionScenario"));
			CustomerNoteColumn.AddAttribute(CustomerNoteCell,"text",param.ParamI("NP_TVChild_OpCustomer_PositionNote"));
			CustomerIdColumn.AddAttribute(CustomerIdCell,"text",param.ParamI("NP_TVChild_OpCustomer_PositionId"));
			
			//Création d'un nouveau store
			CustomerListStore = new Gtk.TreeStore(typeof(bool),typeof(string),typeof(string),typeof(string),typeof(string));	
		}
		
		//Fonction UpdateChildTreeView_Customer()
		//Fonction permettant de mettre à jour le treeview permettant de gerer les fichier customer
		public void UpdateChildTreeView_Customer()
		{
			string LinkScenario = "";
			CustomerListStore.Clear();
			foreach(Project Pro in datamanagement.ListProject.Values)
			{	
				foreach(Node node in Pro.ReturnListNode())
				{
					if(node.Node_Id == Node_Id)
					{	
						foreach(Customer cus in node.ReturnListCustomer())
						{
							LinkScenario = "";
							foreach(Scenario sce in node.ReturnListScenario())
							{
								if(sce.ScenarioId == cus.ScenarioId)
								{
									LinkScenario = sce.ScenarioName;
								}
							}
							CustomerListStore.AppendValues(cus.CustomerUse,cus.CustomerName,LinkScenario,cus.CustomerNote,cus.CustomerId.ToString());
						}
					}
				}
			}
			ChildTreeView.Model = CustomerListStore;
			ChildTreeView.ShowAll();
		}
	
			//Fonction CustomerUseCell_Toggled
			//Fonction permettant de modifier l'utilisation d'un fichier customer
			private void CustomerUseCell_Toggled (object o, Gtk.ToggledArgs args)
			{
				bool Value = false; //Initialisation d'un bool permettant de récuperer la valeur lors d'un clic
			
				Gtk.TreeIter iter;
				if (CustomerListStore.GetIter (out iter, new Gtk.TreePath(args.Path))) 
				{
				foreach(Project Pro in datamanagement.ListProject.Values) //Pour chaque projet 
					{
						foreach(Node node in Pro.ReturnListNode()) //pour chaque noeud
						{
							if(node.Node_Id == Node_Id) //Si l'id est égale mais que la valeur DHCP est différente
							{
								Value = (bool) CustomerListStore.GetValue(iter,param.ParamI("NP_TVChild_OpCustomer_PositionUse")); //on fait l'acquisition de la valeur
								if(node.Customer_.Count > 1 && !Value)
								{
									CustomerListStore.SetValue(iter,param.ParamI("NP_TVChild_OpCustomer_PositionUse"),!Value);//On affiche la valeur inversé								
								}
							}
						}
					}				
				}					
			}
		
			//Fonction CustomerNameCell_Edited
			//Fonction permettant de modifier le nom des fichiers customer
			private void CustomerNameCell_Edited (object o, Gtk.EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				string NameSelected = "";	//variable permettant de stocker le nom selectionné
		
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("NP_TVChild_OpCustomer_PositionId")); //Nous retournons l'id du noeud selectionné
					NameSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("NP_TVChild_OpCustomer_PositionName")); //Nous retournons le nom du noeud selectionné		
				}		
			
				if(NameSelected != args.NewText) //Permet de verifier si on fait un changment dans le nom
				{
					if(args.NewText != "") //Si le texte est pas vide
					{
						datamanagement.ModifyCustomer(datamanagement.ReturnNewNameCustomer(args.NewText,Node_Id),param.ParamI("MoCu_ChoiceName"),Convert.ToInt32(IdSelected));//On modifie le nom
						UpdateChildTreeView_Customer();
						datamanagement.mainwindow.UpdateEplorerTreeView(); //On met à jour ExplorerTreeView		
					}
					else if(args.NewText == "")//Dans le cas ou le texte est vide
					{
						datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameCustomerEmpty"); //on affiche un message d'erreur
					}
				}					
			}

			//Fonction CustomerNoteCell_Edited
			//Fonction permettant de modifier les notes des fichiers customer
			private void CustomerNoteCell_Edited (object o, Gtk.EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				string NoteSelected = "";	//variable permettant de stocker le nom selectionné
		
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("NP_TVChild_OpCustomer_PositionId")); //Nous retournons l'id du noeud selectionné
					NoteSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("NP_TVChild_OpCustomer_PositionNote")); //Nous retournons le nom du noeud selectionné		
				}		
			
				if(NoteSelected != args.NewText) //Permet de verifier si on fait un changment dans le nom
				{		
					datamanagement.ModifyCustomer(args.NewText,param.ParamI("MoCu_ChoiceNote"),Convert.ToInt32(IdSelected));
				    UpdateChildTreeView_Customer();                          
				}
			}
		
			//Fonction CustomerScenarioCell_Edited
			//Fonction permettant d'associer un scénario à un fichier customer
			private void CustomerScenarioCell_Edited (object o, Gtk.EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				string ScenarioSelected = "";	//variable permettant de stocker le nom selectionné
		
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("NP_TVChild_OpCustomer_PositionId")); //Nous retournons l'id du noeud selectionné
					ScenarioSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("NP_TVChild_OpCustomer_PositionScenario")); //Nous retournons le nom du noeud selectionné		
				}		
			
				if(ScenarioSelected != args.NewText) //Permet de verifier si on fait un changment dans le nom
				{		
					datamanagement.ModifyCustomer(args.NewText,param.ParamI("MoCu_ChoiceScenario"),Convert.ToInt32(IdSelected));
				    UpdateChildTreeView_Customer();                          
				}			
			}	
		
//###################### TV Child Choice Scenario #######################
		
		//Fonction InitChildTreeView_Scenario
		//Fonction permettant d'initialiser le treeviex contenant les scénarios
		public void InitChildTreeView_Scenario()
		{
			LabelChildTreeView.Text = param.ParamT("NP_LabTVChild_Name_ScenarioList");
			//Visibilité des lignes et des colonnes
			ChildTreeView.EnableGridLines = TreeViewGridLines.Both;	
			
			//Ajout des colonnes
			ScenarioNameColumn = new Gtk.TreeViewColumn();
			ScenarioNoteColumn = new Gtk.TreeViewColumn();
			ScenarioIdColumn = new Gtk.TreeViewColumn();	
			ScenarioEmptyColumn = new Gtk.TreeViewColumn();
			
			//On ajoute les colonnes dans une liste
			ChildColumnList.Add(ScenarioNameColumn);
			ChildColumnList.Add(ScenarioNoteColumn);			
			ChildColumnList.Add(ScenarioIdColumn);
			ChildColumnList.Add(ScenarioEmptyColumn);	
			
			//Colonne redimensionnable
			ScenarioNameColumn.Resizable = true;
			ScenarioNoteColumn.Resizable = true;
			
			//Taille des colonnes
			ScenarioNoteColumn.MinWidth = 350;
			
			//Nom des colonnes
			ScenarioNameColumn.Title = param.ParamT("NP_TVChild_OpScenario_Name");
			ScenarioNoteColumn.Title = param.ParamT("NP_TVChild_OpScenario_Note");		
			
			//Visibilité des colonnes
			ScenarioIdColumn.Visible = false;
			
			//Création des cellules
			ScenarioNameCell = new CellRendererText();
			ScenarioNoteCell = new CellRendererText();
			ScenarioIdCell = new CellRendererText();
			
			//Cellule éditable
			ScenarioNameCell.Editable = true;
			ScenarioNoteCell.Editable = true;
			
			//Appel des fonction pour la modification des cellules
			ScenarioNameCell.Edited += ScenarioNameCell_Edited;
			ScenarioNoteCell.Edited += ScenarioNoteCell_Edited;
			
			//On associe des cellules aux colonnes		
			ScenarioNameColumn.PackStart(ScenarioNameCell,true);
			ScenarioNoteColumn.PackStart(ScenarioNoteCell,true);
			ScenarioIdColumn.PackStart(ScenarioIdCell,true);
			
			//Ajout des colonnes dans ExplorerTreeView
			ChildTreeView.AppendColumn (ScenarioNameColumn);
			ChildTreeView.AppendColumn (ScenarioNoteColumn);	
			ChildTreeView.AppendColumn (ScenarioIdColumn);
			ChildTreeView.AppendColumn (ScenarioEmptyColumn);	
			
			//Ajout des attibuts à chaque colonne
			ScenarioNameColumn.AddAttribute(ScenarioNameCell,"text",param.ParamI("NP_TVChild_OpScenario_PositionName"));
			ScenarioNoteColumn.AddAttribute(ScenarioNoteCell,"text",param.ParamI("NP_TVChild_OpScenario_PositionNote"));
			ScenarioIdColumn.AddAttribute(ScenarioIdCell,"text",param.ParamI("NP_TVChild_OpScenario_PositionId"));
			
			//Création d'un nouveau store
			ScenarioListStore = new Gtk.TreeStore(typeof(string),typeof(string),typeof(string));				
		}
		
		//Fonction UpdateChildTreeView_Scenario
		//Fonction permettant de mettre à jour le treeview scenario
		public void UpdateChildTreeView_Scenario()
		{
			ScenarioListStore.Clear();
			foreach(Project Pro in datamanagement.ListProject.Values)
			{	
				foreach(Node node in Pro.ReturnListNode())
				{
					if(node.Node_Id == Node_Id)
					{	
						foreach(Scenario sce in node.ReturnListScenario())
						{
							ScenarioListStore.AppendValues(sce.ScenarioName,sce.ScenarioNotes,sce.ScenarioId.ToString());
						}
					}
				}
			}
			ChildTreeView.Model = ScenarioListStore;
			ChildTreeView.ShowAll();			
		}
		
			//Fonction ScenarioNameCell_Edited
			//Fonction permettant de modifier le nom d'un scénario
			private void ScenarioNameCell_Edited (object o, Gtk.EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				string NameSelected = "";	//variable permettant de stocker le nom selectionné
		
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("NP_TVChild_OpScenario_PositionId")); //Nous retournons l'id du noeud selectionné
					NameSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("NP_TVChild_OpScenario_PositionName")); //Nous retournons le nom du noeud selectionné		
				}		
			
				if(NameSelected != args.NewText) //Permet de verifier si on fait un changment dans le nom
				{
					if(args.NewText != "") //Si le texte est pas vide
					{
						datamanagement.ModifyScenario(datamanagement.ReturnNewNameScenario(args.NewText,Node_Id),param.ParamI("MoSc_ChoiceName"),Convert.ToInt32(IdSelected));//On modifie le nom
						UpdateChildTreeView_Scenario();
						datamanagement.mainwindow.UpdateEplorerTreeView(); //On met à jour ExplorerTreeView		
					}
					else if(args.NewText == "")//Dans le cas ou le texte est vide
					{
						datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameScenarioEmpty"); //on affiche un message d'erreur
					}
				}				
			}
		
			//Fonction ScenarioNoteCell_Edited
			//Fonction permettant de modifier les notes d'un sceénario
			private void ScenarioNoteCell_Edited (object o, Gtk.EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				string NoteSelected = "";	//variable permettant de stocker le nom selectionné
		
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("NP_TVChild_OpScenario_PositionId")); //Nous retournons l'id du noeud selectionné
					NoteSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("NP_TVChild_OpScenario_PositionNote")); //Nous retournons le nom du noeud selectionné		
				}		
			
				if(NoteSelected != args.NewText) //Permet de verifier si on fait un changment dans le nom
				{		
					datamanagement.ModifyScenario(args.NewText,param.ParamI("MoSc_ChoiceNote"),Convert.ToInt32(IdSelected));
				    UpdateChildTreeView_Scenario();                          
				}
			}
			
		
//###################### TV Child Choice Option #########################			
		
		//Fonction InitChildTreeView_Option
		//Fonction permettant d'initialiser l'arbre des option d'un node
		public void InitChildTreeView_Option()
		{
			LabelChildTreeView.Text = param.ParamT("NP_LabTVChild_Name_Parameters");
			ChildTreeView.EnableGridLines = TreeViewGridLines.Both;
			
			//Ajout de deux colonnes dans TreeviewProperties
			OptionTypeColumn = new Gtk.TreeViewColumn();
			OptionValueColumn = new Gtk.TreeViewColumn();
			OptionEmptyColumn = new Gtk.TreeViewColumn();
			
			//Nous donnons un titre au colonne
			OptionTypeColumn.Title = param.ParamT("NP_TVPro_TypeLabel");
			OptionValueColumn.Title = param.ParamT("NP_TVPro_ValueLabel");
			
			//Taille de la colonne
			OptionValueColumn.MinWidth = 150;
			
			ChildColumnList.Add (OptionTypeColumn);
			ChildColumnList.Add (OptionValueColumn);
			ChildColumnList.Add (OptionEmptyColumn);
			
			//Les colonnes sont redimensionnable
			OptionTypeColumn.Resizable = true;
			OptionValueColumn.Resizable = true;
					
			//Création des cellules
			OptionTypeCell = new Gtk.CellRendererText ();
			OptionValueCell = new Gtk.CellRendererCombo ();		
			
			//La cellule Value est éditable
			OptionValueCell.Editable = true;
			
			OptionValueCell.Model = ChooseData;
			OptionValueCell.TextColumn = 0;				
			
			//Nous appelons une fonction lorsque nous cliquons que la cellule
			OptionValueCell.Edited += OptionValueCell_Edited;			
			
			//On associe des cellule au colonne
			OptionTypeColumn.PackStart(OptionTypeCell, true);
			OptionValueColumn.PackStart (OptionValueCell, true);
			
			//Ajout des colonnes dans ExplorerTreeView
			ChildTreeView.AppendColumn (OptionTypeColumn);
			ChildTreeView.AppendColumn (OptionValueColumn);
			ChildTreeView.AppendColumn (OptionEmptyColumn);
			
			//Ajout des attibuts à chaque colonne
			OptionTypeColumn.AddAttribute(OptionTypeCell,"text",param.ParamI("NP_TVOpt_TypePosition"));
			OptionValueColumn.AddAttribute (OptionValueCell, "text", param.ParamI("NP_TVOpt_ValuePosition"));
			
			//Création d'un nouveau store, param : texte Type, texte Value
			OptionListStore = new Gtk.TreeStore(typeof (string),typeof (string));
			
			//On écrit le store dans le treeview
			ChildTreeView.Model = OptionListStore;
			ChildTreeView.ShowAll ();				
		}	
		
		//Fonction UpdateChildTreeView_Option
		//Fonction permettant de mettre à jour l'arbre des options
		public void UpdateChildTreeView_Option()
		{
			OptionListStore.Clear();

			foreach(Project Pro in datamanagement.ListProject.Values)
			{
				foreach(Node node in Pro.ReturnListNode())
				{
					if(node.Node_Id == Node_Id)
					{						
						OptionListStore.AppendValues(param.ParamT("NP_TVOption_LabelLine1"),node.Node_Type);
						if(node.Node_Clock == "0")
						{
							OptionListStore.AppendValues(param.ParamT("NP_TVOption_LabelLine2"),param.ParamT("NP_NoValue"));
						}
						else
						{
							OptionListStore.AppendValues(param.ParamT("NP_TVOption_LabelLine2"),node.Node_Clock);
						}
						
						if(node.Node_1Wire)
						{
							OptionListStore.AppendValues(param.ParamT("NP_TVOption_LabelLine3"),param.ParamT("NP_YesValue"));
						}
						else
						{
							OptionListStore.AppendValues(param.ParamT("NP_TVOption_LabelLine3"),param.ParamT("NP_NoValue"));
						}
					}
				}
			}						
		}	
		
			//Fonction OptionValueCell_Edited
			//Permet la modification des cellules lors du choix option
			private void OptionValueCell_Edited (object o, Gtk.EditedArgs args)
			{
				Gtk.TreeIter iter; //Conception d'un TreeIter permettant de retourner la position de la valeur cliqué
				PropertiesListStore.GetIter (out iter, new Gtk.TreePath (args.Path)); //Nous allons cherché cette valeur qui est arg.Path ainsi que le nouveau text dans args.Text
			
				if(Convert.ToByte(args.Path) == param.ParamI("NP_TVChild_OpOption_PositionType"))
				{
					if(args.NewText == param.ParamP("NP_SMBv00") || args.NewText == param.ParamP("NP_SMBv01"))
					{
						datamanagement.ModifyNode(args.NewText,param.ParamI("MoNo_ChoiceType"),Node_Id); //On fait la mise à jour du type de noeud
					}
					else
					{
						datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"NP_DataNoValid"); //on affiche un message d'erreur
					}
				}
				else if(Convert.ToByte(args.Path) == param.ParamI("NP_TVChild_OpOption_PositionClock"))
				{
					if(args.NewText == param.ParamP("NP_CLockRTC") || args.NewText == param.ParamP("NP_CLockDCF77") || args.NewText == param.ParamT("NP_NoValue"))
					{
						datamanagement.ModifyNode(args.NewText,param.ParamI("MoNo_ChoiceClock"),Node_Id); //On fait la mise à jour l'horloge du noeud
					}
					else
					{
						datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"NP_DataNoValid"); //on affiche un message d'erreur
					}
				}
				else if(Convert.ToByte(args.Path) == param.ParamI("NP_TVChild_OpOption_Position1Wire"))
				{
					if(args.NewText == param.ParamT("NP_YesValue") || args.NewText == param.ParamT("NP_NoValue"))
					{
						if(args.NewText == param.ParamT("NP_YesValue"))
						{
							datamanagement.ModifyNode("True",param.ParamI("MoNo_Choice1Wire"),Node_Id); //On fait la mise à jour du 1-wire
						}
						else
						{
							datamanagement.ModifyNode("False",param.ParamI("MoNo_Choice1Wire"),Node_Id); //On fait la mise à jour du 1-wire
						}
					}
					else
					{
						datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"NP_DataNoValid"); //on affiche un message d'erreur
					}				
				}	
				UpdateChildTreeView_Option(); //On met à jour le PropertiesTreeview
			}
		
//###################### Other  ####################################		
		
		//Fonction OnChildTreeViewButtonReleaseEvent
		//Fonction permettant de faire des modifications sur clic dans le child treeview notemment d'enregistrer les valeurs des debug losque nous cliquons sur une checkbox
		protected void OnChildTreeViewButtonReleaseEvent (object o, Gtk.ButtonReleaseEventArgs args)
		{
			if(SelectOption == param.ParamT("NP_TVParam_ParameterDebug") || SelectOption == DebugOldLangage)
			{
				bool ChildDebugValueSelect = false; //Variable permettant de stocker la valeur DHCP que nous allons récuperer
				string ChildDebugNameSelect = ""; //Variable permettant de stocker la valeur Id que nous allons récuperer
				TreeSelection selection = (o as TreeView).Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					ChildDebugValueSelect = (bool) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("NP_TVChild_OpDebug_PositionValue")); //Nous récuperons la valeur que nous stockons
					ChildDebugNameSelect = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("NP_TVChild_OpDebug_PositionType")); //Nous récuperons la valeur que nous stockons
					
					foreach(Project Pro in datamanagement.ListProject.Values) //Pour chaque projet 
					{
						foreach(Node node in Pro.ReturnListNode()) //pour chaque noeud
						{
							if(node.Node_Id == Node_Id) //Si l'id est égale mais que la valeur DHCP est différente
							{
								datamanagement.ModifyNodeDebug(ChildDebugValueSelect,ChildDebugNameSelect,Node_Id);
							}
						}
					}
				}	
			}
			else if(SelectOption == param.ParamT("NP_TVParam_ParameterOption") || SelectOption == OptionOldLangage)
			{
				ChooseData.Clear ();
				string TypeOption = "";
		
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					TypeOption = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("NP_TVOpt_TypePosition")); //Nous retournons le type de carte sélectionné		
				}	
				
				if(TypeOption == param.ParamT("NP_TVOption_LabelLine1"))
				{
					ChooseData.AppendValues(param.ParamP("NP_SMBv00"));
					ChooseData.AppendValues(param.ParamP("NP_SMBv01"));
				}
				else if(TypeOption == param.ParamT("NP_TVOption_LabelLine2"))
				{
					ChooseData.AppendValues(param.ParamP("NP_CLockRTC"));
					ChooseData.AppendValues(param.ParamP("NP_CLockDCF77"));
					ChooseData.AppendValues(param.ParamT("NP_NoValue"));
				}		
				else if(TypeOption == param.ParamT("NP_TVOption_LabelLine3"))
				{
					ChooseData.AppendValues(param.ParamT("NP_YesValue"));
					ChooseData.AppendValues(param.ParamT("NP_NoValue"));
				}
				else if(TypeOption == param.ParamT("NP_TVOption_LabelLine4"))
				{
					ChooseData.AppendValues(param.ParamT("NP_YesValue"));
					ChooseData.AppendValues(param.ParamT("NP_NoValue"));
				}				
			}
			else if(SelectOption == param.ParamT("NP_TVParam_ParameterCustomer") || SelectOption == OptionOldLangage)
			{
				ListScenario.Clear();
				

				bool UseValueSelect = false; //Variable permettant de stocker la valeur DHCP que nous allons récuperer
				string UseIdSelect = ""; //Variable permettant de stocker la valeur Id que nous allons récuperer
				
				TreeSelection selection = (o as TreeView).Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					UseValueSelect = (bool) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("NP_TVChild_OpCustomer_PositionUse")); //Nous récuperons la valeur que nous stockons
					UseIdSelect = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("NP_TVChild_OpCustomer_PositionId")); //Nous récuperons la valeur que nous stockons
					
					foreach(Project Pro in datamanagement.ListProject.Values) //Pour chaque projet 
					{
						foreach(Node node in Pro.ReturnListNode()) //pour chaque noeud
						{
							if(node.Node_Id == Node_Id) //Si l'id est égale mais que la valeur DHCP est différente
							{
								foreach(Scenario sce in node.ReturnListScenario())
								{
									ListScenario.AppendValues(sce.ScenarioName);
								}
								
								foreach(Customer cus in node.ReturnListCustomer())
								{
									if(cus.CustomerId.ToString() == UseIdSelect)
									{
										if(cus.CustomerUse != UseValueSelect  && (node.ReturnListCustomer().Count > 1))
										{
											foreach(Customer _cus in node.ReturnListCustomer())
											{
												_cus.CustomerUse = false;
											}
											datamanagement.ModifyCustomer(UseValueSelect.ToString(),param.ParamI("MoCu_ChoiceUse"),Convert.ToInt32(UseIdSelect));
										}
									}
								}
							}
						}
					}
					ListScenario.AppendValues(param.ParamT("BI2CP_EmptyLabel"));
				}					
			}
		}
		
		//Fonction UpdateWidget
		//Fonction permettant de mettre à jour le widget
		public void UpdateWidget()
		{
			LabelChildTreeView.Text = param.ParamT("NP_LabTVChild_Name_Choose");
			NoteLabel.Text = param.ParamT("NP_NoteLabel");
			
			//Option treeview
			OptionNameColumn.Title = param.ParamT("NP_TVParam_NameLabel");
			UpdateOptionsTreeView();
		
			foreach(Project Pro in datamanagement.ListProject.Values)
			{
				foreach(Node node in Pro.ReturnListNode())
				{
					if(node.Node_Id == Node_Id)
					{
						TextViewNote.Buffer.Text = node.Node_Note;			
					}
				}
			}			
			
			if (OptionName == null || OptionName == "")
			{
				foreach(TreeViewColumn TVchild in ChildColumnList)
				{
					ChildTreeView.RemoveColumn(TVchild);
				}
			
				if(ChildColumnList.Count >0)
				{ 
					ChildColumnList.RemoveRange(0,ChildColumnList.Count);
				}	
				InitChildTreeView_Properties();
				UpdateChildTreeView_Properties();					
			}
			else if (OptionName == DebugOldLangage)
			{
				foreach(TreeViewColumn TVchild in ChildColumnList)
				{
					ChildTreeView.RemoveColumn(TVchild);
				}
			
				if(ChildColumnList.Count >0)
				{ 
					ChildColumnList.RemoveRange(0,ChildColumnList.Count);
				}
				InitChildTreeView_Debug();
				UpdateChildTreeView_Debug();
			}
			else if(OptionName == ParamOldLangage)
			{
				foreach(TreeViewColumn TVchild in ChildColumnList)
				{
					ChildTreeView.RemoveColumn(TVchild);
				}
			
				if(ChildColumnList.Count >0)
				{ 
					ChildColumnList.RemoveRange(0,ChildColumnList.Count);
				}	
				InitChildTreeView_Properties();
				UpdateChildTreeView_Properties();
			}
			else if(OptionName == OptionOldLangage)
			{
				foreach(TreeViewColumn TVchild in ChildColumnList)
				{
					ChildTreeView.RemoveColumn(TVchild);
				}
			
				if(ChildColumnList.Count >0)
				{ 
					ChildColumnList.RemoveRange(0,ChildColumnList.Count);
				}	
				InitChildTreeView_Option();
				UpdateChildTreeView_Option();
			}	
			else if(OptionName == CustomerOldLangage)
			{
				foreach(TreeViewColumn TVchild in ChildColumnList)
				{
					ChildTreeView.RemoveColumn(TVchild);
				}
			
				if(ChildColumnList.Count >0)
				{ 
					ChildColumnList.RemoveRange(0,ChildColumnList.Count);
				}	
				InitChildTreeView_Customer();
				UpdateChildTreeView_Customer();
			}	
			else if(OptionName == ScenarioOldLangage)
			{
				foreach(TreeViewColumn TVchild in ChildColumnList)
				{
					ChildTreeView.RemoveColumn(TVchild);
				}
			
				if(ChildColumnList.Count >0)
				{ 
					ChildColumnList.RemoveRange(0,ChildColumnList.Count);
				}	
				InitChildTreeView_Scenario();
				UpdateChildTreeView_Scenario();
			}				
			else
			{
				foreach(TreeViewColumn TVchild in ChildColumnList)
				{
					ChildTreeView.RemoveColumn(TVchild);
				}
			
				if(ChildColumnList.Count >0)
				{ 
					ChildColumnList.RemoveRange(0,ChildColumnList.Count);
				}	
				InitChildTreeView_Properties();
				UpdateChildTreeView_Properties();				
			}
			
		}
		
		//Fonction WidgetIsCorrect
		//Fcontion peremettant de saoir si un widget est correct, si il nous retourne de la data
		public bool WidgetIsCorrect()
		{
			foreach(Project Pro in datamanagement.ListProject.Values)//Pour chaque projet de la liste
			{	
				foreach(Node nod in Pro.ReturnListNode())
				{
					if(nod.Node_Id == Node_Id)//si un projet est égale à l'id project mis en paramètre
					{
						return true;
					}
				}
			}
			return false;
		}		

		//Fonction OnTextViewNoteFocusOutEvent
		//Fonction permettant de modifier les notes d'un noeud
		protected void OnTextViewNoteFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
		{
			datamanagement.ModifyNode(TextViewNote.Buffer.Text,param.ParamI("MoNo_ChoiceNote"),Node_Id); //On fait la mise à jour des notes
		}			
			
	}				
}