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
	public partial class OneWireProperties : Gtk.Bin
	{
		public DataManagement datamanagement;
		public Param param;
		public Int32 Network_Id;
		public Int32 Project_Id;
		public string SelectOption;
		
		public string PropOldName;
		public string BoardOldName;		
		
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
		
		public global::Gtk.TreeStore OptionListStore;
		public global::Gdk.Pixbuf PngBoard;
		public global::Gdk.Pixbuf PngParameter;
		
		public global::Gtk.TreeIter IterNode;
		
		public global::Gtk.TreeModel TreeModelOptionTreeView;
		
		public string OptionName;
		public bool DisplayAllNode;	
		
		//Utilisation de ses variables pour le treeview child : général
		
		public List<TreeViewColumn> ChildColumnList;
		public global::Gtk.TreeIter IterChild;
		public global::Gtk.TreeModel TreeModelChildTreeView;		
		
		//Utilisation de ses variables pour le treeview child : option board
		public global::Gtk.TreeViewColumn Child_Board_NameColumn;
		public global::Gtk.TreeViewColumn Child_Board_TypeColumn;
		public global::Gtk.TreeViewColumn Child_Board_MacColumn;
		public global::Gtk.TreeViewColumn Child_Board_PrecisionColumn;
		public global::Gtk.TreeViewColumn Child_Board_I2C1Column;
		public global::Gtk.TreeViewColumn Child_Board_EmptyColumn;
		public global::Gtk.TreeViewColumn Child_Board_IdColumn;		
		
		public global::Gtk.CellRendererText Child_Board_NameCell;
		public global::Gtk.CellRendererText Child_Board_TypeCell;
		public global::Gtk.CellRendererText Child_Board_MacCell;
		public global::Gtk.CellRendererCombo Child_Board_PrecisionCell;
		public global::Gtk.CellRendererText Child_Board_EmptyCell;
		public global::Gtk.CellRendererText Child_Board_IdCell;		
		
		public global::Gtk.TreeStore Child_Board_ListStore;		
		public Gtk.ListStore ListI2CAddress;
		
		public Gtk.ListStore ListPrecision;
		
		public OneWireProperties (DataManagement _datamanagement, Param _param, Int32 _Network_Id)
		{
			this.datamanagement = _datamanagement;
			this.param = _param;
			this.Network_Id = _Network_Id;
			this.Build ();
			
			ChildColumnList = new List<TreeViewColumn>();
			ListPrecision = new Gtk.ListStore(typeof (string));
			ListPrecision.AppendValues("0.1");
			ListPrecision.AppendValues("0.5");
			InitWidget();
			InitOptionsTreeView();
			UpdateOptionsTreeView();
			
			OptionTreeView.ButtonReleaseEvent += new ButtonReleaseEventHandler(OptionsTreeViewButtonRelease);
			
			//Permet de retourner l'id du projet
			foreach(Project pro in datamanagement.ListProject)
			{
				foreach(Node node in pro.ReturnListNode())
				{
					foreach(Network net in node.ReturnListNetwork())
					{
						if(net.Network_Id == _Network_Id)
						{
							datamanagement.CurrentProjectId = pro.Project_Id;
							Project_Id = pro.Project_Id;
						}
					}    
				}
			}					
		}
		
		//Fonction InitWidget
		//Fonction permettant d'initialiser le widget
		public void InitWidget()
		{
			LabelChildTreeView.Text = param.ParamT("OWP_LabTVChild_Name_Choose");
			NoteLabel.Text = param.ParamT("OWP_NoteLabel");
			ButtonAddProbe.Label = param.ParamT("OWP_AddBoard_Name_Button");
			ButtonDeleteProbe.Label = param.ParamT("OWP_DeleteBoard_Name_Button");
			
			vpaned1.Position = 0;
			hpaned1.Position = (datamanagement.mainwindow.ReturnHpanedPosition() * param.ParamI("NoteHPanedPurcent")) / 100 ;			
			
			foreach(Project Pro in datamanagement.ListProject)
			{
				foreach(Node node in Pro.ReturnListNode())
				{
					foreach(Network net in node.ReturnListNetwork())
					{
						if(net.Network_Id == Network_Id)
						{
							TextViewNote.Buffer.Text = net.Network_Note;			
						}
					}
				}
			}		
	
			UpdateComboBoxNumberOfProbe();
			UpdateComboBoxTypeOfProbe();
			
			vpaned1.Position = (datamanagement.mainwindow.ReturnVpanedPosition() * param.ParamI("NoteVPanedPurcent")) / 100;
			InitPropertiesTreeView();
			UpdatePropertiesTreeView();
		}	

//###################### TV Options  ####################################
		
		//Fonction InitOptionsTreeView
		//Fcontion permettant d'initialiser le treeview affichant les différentes options pour un projet
		public void InitOptionsTreeView()
		{
			//Création des colonne du treeview
			OptionNameColumn = new Gtk.TreeViewColumn();
			
			//Nouveau Logo
			PngBoard = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconBoard"));
			PngParameter = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconProperties"));			
			
			//Nous donnons un titre au colonnes
			OptionNameColumn.Title = param.ParamT("OWP_TVOpt_Label");
			
			//Création des cellules
			OptionPixCell = new Gtk.CellRendererPixbuf ();
			OptionCell = new Gtk.CellRendererText ();
			
			//On associe des cellules au colonne
			OptionNameColumn.PackStart(OptionPixCell, false);
			OptionNameColumn.PackStart(OptionCell, true);	
			
			//Ajout des colonnes dans OptionTreeView
			OptionTreeView.AppendColumn (OptionNameColumn);
			
			//Ajout des attibut à chaque colonne
			OptionNameColumn.AddAttribute (OptionPixCell, "pixbuf", param.ParamI("OWP_TVOpt_PostionPixBuff"));
			OptionNameColumn.AddAttribute(OptionCell,"text",param.ParamI("OWP_TVOpt_PostionName"));	
			
			//Création d'un nouveau store, param : icon, texte explorer, texte type, texte name, texte id
			OptionListStore = new Gtk.TreeStore (typeof (Gdk.Pixbuf),typeof (string));
			
			//On écrit le store dans le treeview
			OptionTreeView.Model = OptionListStore;
			OptionTreeView.ShowAll ();						
		}
		
		//Fonction UpdateOptionsTreeView
		//Fonction permettant de mettre à jour l'arbre des options
		public void UpdateOptionsTreeView()
		{
			OptionListStore.Clear();
			IterNode = OptionListStore.AppendValues(PngParameter,param.ParamT("OWP_TVOpt_OptionParameter"));
			IterNode = OptionListStore.AppendValues(PngBoard,param.ParamT("OWP_TVOpt_OptionBoard"));
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
						OptionName = (string) TreeModelOptionTreeView.GetValue (IterNode, param.ParamI("OWP_TVOpt_PostionName")); //Nous mettons la valeur de la 1ere cellule dans un string (texte)
						SelectOption = OptionName;	
					
						if (OptionName == param.ParamT("OWP_TVOpt_OptionBoard"))
						{
							BoardOldName = OptionName;
							foreach(TreeViewColumn TVchild in ChildColumnList)
							{
								ChildTreeView.RemoveColumn(TVchild);
							}
						
							if(ChildColumnList.Count >0)
							{ 
								ChildColumnList.RemoveRange(0,ChildColumnList.Count);
							}	
						
							InitChildTreeView_Board();
							UpdateChildTreeView_Board(false);
						}
						else if(OptionName ==  param.ParamT("OWP_TVOpt_OptionParameter"))
						{
							PropOldName = OptionName;
							foreach(TreeViewColumn TVchild in ChildColumnList)
							{
								ChildTreeView.RemoveColumn(TVchild);
							}
						
							if(ChildColumnList.Count >0)
							{ 
								ChildColumnList.RemoveRange(0,ChildColumnList.Count);
							}
							InitPropertiesTreeView();
							UpdatePropertiesTreeView();
						}
					}
				}
			}		

//####################### TV Properties ####################################		
		
		//Fonction InitPropertiesTreeView
		//Fonction permettant d'initialiser l'arbre des propriété
		public void InitPropertiesTreeView()
		{
			LabelChildTreeView.Text = param.ParamT("OWP_LabTVChild_Name_Properties");			
			ChildTreeView.EnableGridLines = TreeViewGridLines.Both;
			
			//Ajout de deux colonnes dans TreeviewProperties
			PropertiesTypeColumn = new Gtk.TreeViewColumn();
			PropertiesValueColumn = new Gtk.TreeViewColumn();
			PropertiesEmptyColumn = new Gtk.TreeViewColumn();
			
			//Nous donnons un titre au colonne
			PropertiesTypeColumn.Title = param.ParamT("OWP_TVPro_TypeLabel");
			PropertiesValueColumn.Title = param.ParamT("OWP_TVPro_ValueLabel");
			
			ChildColumnList.Add(PropertiesTypeColumn);
			ChildColumnList.Add(PropertiesValueColumn);
			ChildColumnList.Add(PropertiesEmptyColumn);
			
			//Les colonnes sont redimensionnable
			PropertiesTypeColumn.Resizable = true;
			PropertiesValueColumn.Resizable = true;
					
			//Création des cellules
			PropertiesTypeCell = new Gtk.CellRendererText ();
			PropertiesValueCell = new Gtk.CellRendererText ();	
						
			//La cellule Value est éditable
			//PropertiesValueCell.Editable = true;
			
			//Nous appelons une fonction lorsque nous cliquons que la cellule
			//PropertiesValueCell.Edited += PropertiesValueCell_Edited;			
			
			//On associe des cellule au colonne
			PropertiesTypeColumn.PackStart(PropertiesTypeCell, true);
			PropertiesValueColumn.PackStart (PropertiesValueCell, true);
			
			//Ajout des colonnes dans ExplorerTreeView
			ChildTreeView.AppendColumn (PropertiesTypeColumn);
			ChildTreeView.AppendColumn (PropertiesValueColumn);
			ChildTreeView.AppendColumn (PropertiesEmptyColumn);
			
			//Ajout des attibuts à chaque colonne
			PropertiesTypeColumn.AddAttribute(PropertiesTypeCell,"text",param.ParamI("OWP_TVPro_TypePosition"));
			PropertiesValueColumn.AddAttribute (PropertiesValueCell, "text", param.ParamI("OWP_TVPro_ValuePosition"));
			
			//Création d'un nouveau store, param : texte Type, texte Value
			PropertiesListStore = new Gtk.TreeStore(typeof (string),typeof (string));
			
			//On écrit le store dans le treeview
			ChildTreeView.Model = PropertiesListStore;
			ChildTreeView.ShowAll ();				
		}
		
		//Fonction UpdatePropertiesTreeView
		//Fonction permettant de mettre à jour l'arbre des propriétés
		public void UpdatePropertiesTreeView()
		{
			PropertiesListStore.Clear();

			foreach(Project Pro in datamanagement.ListProject)
			{
				foreach(Node node in Pro.ReturnListNode())
				{
					foreach(Network net in node.ReturnListNetwork())
					{
						if(net.Network_Id == Network_Id)
						{						
							PropertiesListStore.AppendValues(param.ParamT("OWP_TVPro_LabelLine1"),net.Network_Type);	
							TextViewNote.Buffer.Text = net.Network_Note;
						}
					}
				}
			}				
			
			//On écrit le store dans le treeview
			ChildTreeView.Model = PropertiesListStore;
			ChildTreeView.ShowAll ();			
		}
		
			//Fonction PropertiesValueCell_Edited
				//Fonction permettant de mettre à jour des information en cliquant sur une cellule
			/*	private void PropertiesValueCell_Edited (object o, Gtk.EditedArgs args)
				{
					Gtk.TreeIter iter; //Conception d'un TreeIter permettant de retourner la position de la valeur cliqué
					PropertiesListStore.GetIter (out iter, new Gtk.TreePath (args.Path)); //Nous allons cherché cette valeur qui est arg.Path ainsi que le nouveau text dans args.Text
				
					if(Convert.ToByte(args.Path) == param.NP_TVPro_NamePostion || Convert.ToByte(args.Path) == param.NP_TVPro_IOWPosition)
					{
						datamanagement.ModifyNode(args.NewText,Convert.ToByte(args.Path),Node_Id); //On fait la mise à jour du nom ou IP
					}
					else if(Convert.ToByte(args.Path) == param.NP_TVPro_MacPosition)
					{
						datamanagement.ModifyNode("",Convert.ToByte(args.Path),Node_Id); //On fait la mise à jour du mac
					}
					else if(Convert.ToByte(args.Path) == param.NP_TVPro_DHCOWPosition)
					{
						if(args.NewText == "X")
						{
							datamanagement.ModifyNode("True",Convert.ToByte(args.Path),Node_Id); //On fait la mise à jour du dhcp
						}
						else if(args.NewText == "V")
						{
							datamanagement.ModifyNode("False",Convert.ToByte(args.Path),Node_Id); //On fait la mise à jour du dhcp
						}
					}
					
					UpdatePropertiesTreeView(); //On met à jour le PropertiesTreeview
					datamanagement.mainwindow.UpdateEplorerTreeView(); //On met à jour ExplorerTreeView
				}	*/			
		
//###################### TV Child Choice Board #########################	
		
		//Fonction InitChildTreeView_Board
		//Fcontion permettant d'initialiser le treeview cbild lorsque le chois se fait pour une carte
		public void InitChildTreeView_Board()
		{		
			LabelChildTreeView.Text = param.ParamT("OWP_LabTVChild_Name_BoardList");
			
			//Visibilité des lignes et des colonnes
			ChildTreeView.EnableGridLines = TreeViewGridLines.Both;
			
			//Ajout des colonnes
			Child_Board_NameColumn = new Gtk.TreeViewColumn();
			Child_Board_TypeColumn = new Gtk.TreeViewColumn();
			Child_Board_MacColumn = new Gtk.TreeViewColumn();
			Child_Board_PrecisionColumn = new Gtk.TreeViewColumn();
			Child_Board_IdColumn = new Gtk.TreeViewColumn();
			Child_Board_EmptyColumn = new Gtk.TreeViewColumn();	
			
			//On ajoute les colonnes dans une liste
			ChildColumnList.Add(Child_Board_NameColumn);
			ChildColumnList.Add(Child_Board_TypeColumn);
			ChildColumnList.Add(Child_Board_MacColumn);
			ChildColumnList.Add(Child_Board_PrecisionColumn);
			ChildColumnList.Add(Child_Board_IdColumn);			
			ChildColumnList.Add(Child_Board_EmptyColumn);
			
			//Colonne redimensionnable
			Child_Board_NameColumn.Resizable = true;
			Child_Board_TypeColumn.Resizable = true;
			Child_Board_MacColumn.Resizable = true;	
			Child_Board_PrecisionColumn.Resizable = true;
			Child_Board_IdColumn.Resizable = true;	
			Child_Board_EmptyColumn.Resizable = true;	
			
			//Nom des colonne
			Child_Board_NameColumn.Title = param.ParamT("OWP_TVChild_OpBoard_Name");
			Child_Board_TypeColumn.Title = param.ParamT("OWP_TVChild_OpBoard_Type");
			Child_Board_MacColumn.Title = param.ParamT("OWP_TVChild_OpBoard_Mac");	
			Child_Board_PrecisionColumn.Title = param.ParamT("OWP_TVChild_OpBoard_Precision");
			Child_Board_IdColumn.Title = param.ParamT("OWP_TVChild_OpBoard_ID");
						
			//Visibilité des colonnes
			Child_Board_IdColumn.Visible = false;
			
			//Création des cellules
			Child_Board_NameCell = new Gtk.CellRendererText ();
			Child_Board_TypeCell = new Gtk.CellRendererText ();
			Child_Board_MacCell = new Gtk.CellRendererText (); 
			Child_Board_PrecisionCell = new Gtk.CellRendererCombo (); 
			Child_Board_IdCell = new Gtk.CellRendererText ();	
			
			//On rend les cellules éditables
			Child_Board_NameCell.Editable = true;
			Child_Board_TypeCell.Editable = false;
			Child_Board_MacCell.Editable = true;  	
			Child_Board_PrecisionCell.Editable = true; 
			Child_Board_IdCell.Editable = false;
			
			Child_Board_PrecisionCell.Model = ListPrecision;
			Child_Board_PrecisionCell.TextColumn = 0;
			
			//En cas d'edition des cellule, on appel les fonction associé
			Child_Board_NameCell.Edited += Child_Board_NameCell_Edited;
			Child_Board_MacCell.Edited += Child_Board_1WMacCell_Edited;
			Child_Board_PrecisionCell.Edited += Child_Board_PrecisionCell_Edited;
			
			//On associe des cellules aux colonnes			
			Child_Board_NameColumn.PackStart (Child_Board_NameCell,true);
			Child_Board_TypeColumn.PackStart (Child_Board_TypeCell,true);
			Child_Board_MacColumn.PackStart (Child_Board_MacCell,false);
			Child_Board_PrecisionColumn.PackStart (Child_Board_PrecisionCell,false);
			Child_Board_IdColumn.PackStart (Child_Board_IdCell,true);			
			
			//Ajout des colonnes dans ExplorerTreeView
			ChildTreeView.AppendColumn (Child_Board_NameColumn);
			ChildTreeView.AppendColumn (Child_Board_TypeColumn);
			ChildTreeView.AppendColumn (Child_Board_MacColumn);
			ChildTreeView.AppendColumn (Child_Board_PrecisionColumn);
			ChildTreeView.AppendColumn (Child_Board_IdColumn);			
			ChildTreeView.AppendColumn (Child_Board_EmptyColumn);
			
			//Ajout des attibuts à chaque colonne
			Child_Board_NameColumn.AddAttribute(Child_Board_NameCell,"text",param.ParamI("OWP_TVChild_OpBoard_PositionName"));
			Child_Board_TypeColumn.AddAttribute(Child_Board_TypeCell,"text",param.ParamI("OWP_TVChild_OpBoard_PositionType"));
			Child_Board_MacColumn.AddAttribute(Child_Board_MacCell,"text",param.ParamI("OWP_TVChild_OpBoard_PositionMac"));	 
			Child_Board_PrecisionColumn.AddAttribute(Child_Board_PrecisionCell,"text",param.ParamI("OWP_TVChild_OpBoard_PositionPrecision"));
			Child_Board_IdColumn.AddAttribute(Child_Board_IdCell,"text",param.ParamI("OWP_TVChild_OpBoard_PositionID"));
			
			//Création d'un nouveau store
			Child_Board_ListStore = new Gtk.TreeStore(typeof(string),typeof(string),typeof(string),typeof(string),typeof(string));	
			
			//On écrit le store dans le treeview
			ChildTreeView.Model = Child_Board_ListStore;
			ChildTreeView.ShowAll ();			
		}
		
		//Fonction UpdateChildTreeView_Board
		//Fonction permettant de mettre à jour le child treeview
		public void UpdateChildTreeView_Board(bool DisplayUnit)
		{
			Child_Board_ListStore.Clear();
			if(!DisplayUnit)
			{
				foreach(Project Pro in datamanagement.ListProject)
				{	
					foreach(Node node in Pro.ReturnListNode())
					{
						foreach(Network net in node.ReturnListNetwork())
						{
							if(net.Network_Id == Network_Id)
							{
								foreach(Board boa in net.ReturnListBoard())
								{
									IterChild = Child_Board_ListStore.AppendValues(boa.Board_Name,boa.Board_Type,boa.Board_1Wire_Mac,boa.Board_1Wire_Precision,boa.Board_Id.ToString());
								}
							}
						}
					}
				}
			}
			ChildTreeView.Model = Child_Board_ListStore;
			ChildTreeView.ShowAll();			
		}		
		
			//Fonction Child_Board_NameCell_Edited
			//Fonction permettant de modifier le nom d'une carte
			void Child_Board_NameCell_Edited (object o, EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				string NameSelected = "";	//variable permettant de stocker le nom selectionné
		
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("OWP_TVChild_OpBoard_PositionID")); //Nous retournons l'id du noeud selectionné
					NameSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("OWP_TVChild_OpBoard_PositionName")); //Nous retournons le nom du noeud selectionné		
				}	 
		
				if(NameSelected != args.NewText) //Permet de verifier si on fait un changment dans le nom
				{
					if(args.NewText != "") //Si le texte est pas vide
					{
						if(args.NewText.Length <= param.ParamI("CarSizeMax"))
						{
							datamanagement.ModifyBoard(args.NewText,param.ParamI("MoBo_ChoiceName"),Convert.ToInt32(IdSelected));//On modifie le nom
							UpdateChildTreeView_Board(false); //On met à jour le PropertiesTreeview
							UpdateOptionsTreeView(); //On met à jour l'option treeview
							datamanagement.mainwindow.UpdateEplorerTreeView(); //On met à jour ExplorerTreeView
						}
						else //Dans le cas ou le nouveuau nom est supérieur à 16 caratère
						{
							datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameBoardTooLong"); //on affiche un message d'erreur
						}
					}
					else //Dans le cas ou le texte est vide
					{
						datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameBoardEmpty"); //on affiche un message d'erreur
					}
				}			
			}
		
			//Fonction Child_Board_MacCell_Edited
			//Fonction permettant de modifier l'adresse mac d'une carte 1-Wire
			void Child_Board_1WMacCell_Edited (object o, EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				string MacSelected = "";	//variable permettant de stocker le mac selectionné
		
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("OWP_TVChild_OpBoard_PositionID")); //Nous retournons l'id du noeud selectionné
					MacSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("OWP_TVChild_OpBoard_PositionMac")); //Nous retournons le mac de la carte 1-Wire		
				}	 
		
				if(MacSelected != args.NewText) //Permet de verifier si on fait un changment dans le nom
				{
					if(datamanagement.AnalyseMac(args.NewText))
					{
						datamanagement.ModifyBoard(args.NewText,param.ParamI("MoBo_Choice1WMac"),Convert.ToInt32(IdSelected));//On modifie le nom
						UpdateChildTreeView_Board(false); //On met à jour le PropertiesTreeview
					}
					else
					{
						datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"OWP_ErrorMac"); //on affiche un message d'erreur
						datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputQuestion"),"OWP_QuestionMac"); //on affiche un message d'erreur
					}						
				}			
			}	
		
			//Fonction Child_Board_PrecisionCell_Edited
			//Fonction permettant de modifier la précision d'un carte
			void Child_Board_PrecisionCell_Edited (object o, EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				string PrecisionSelected = "";	//variable permettant de stocker le mac selectionné
		
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("OWP_TVChild_OpBoard_PositionID")); //Nous retournons l'id du noeud selectionné
					PrecisionSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("OWP_TVChild_OpBoard_PositionMac")); //Nous retournons le mac de la carte 1-Wire		
				}	 
		
				if(PrecisionSelected != args.NewText) //Permet de verifier si on fait un changment dans le nom
				{
					datamanagement.ModifyBoard(args.NewText,param.ParamI("MoBo_Choice1WPrecision"),Convert.ToInt32(IdSelected));//On modifie le nom
					UpdateChildTreeView_Board(false); //On met à jour le PropertiesTreeview
				}					
			}

//###################### Update Combobox  ####################################		
		
		//Fonction UpdateComboBoxNumberOfBoard
		//Fonction permettant de mettre à jour la combobox pour afficher le nombre de carte que nous voulons créer
		public void UpdateComboBoxNumberOfProbe()
		{
			for(int i = 1;i<11;i++)
			{
				ComboboxNumberOfProbe.AppendText(i.ToString());
			}			
			Gtk.TreeIter iter;
			ComboboxNumberOfProbe.Model.IterNthChild(out iter,0);
			ComboboxNumberOfProbe.SetActiveIter(iter);				
		}
		
		//Fonction UpdateComboBoxTypeOfBoard
		//Fonction permettant de mettre à jour la combobox pour afficher le type de carte que nous voulons créer
		public void UpdateComboBoxTypeOfProbe()
		{
			foreach(Boards boas in datamanagement.ListBoards)
			{
				if(boas.NetworkType == "1-Wire")
				{
					ComboboxTypeOfProbe.AppendText(boas.Type);
				}
			}
			Gtk.TreeIter iter;
			ComboboxTypeOfProbe.Model.IterNthChild(out iter,0);
			ComboboxTypeOfProbe.SetActiveIter(iter);				
		}		
		
//###################### Other  ####################################	
		
		//Fonction UpdateWidget
		//Fonction permettant de mettre à jour le widget
		public void UpdateWidget()
		{
			LabelChildTreeView.Text = param.ParamT("OWP_LabTVChild_Name_Choose");
			NoteLabel.Text = param.ParamT("OWP_NoteLabel");
			
			ButtonAddProbe.Label = param.ParamT("OWP_AddBoard_Name_Button");
			ButtonDeleteProbe.Label = param.ParamT("OWP_DeleteBoard_Name_Button");
			
			//Init and update OptionTreeView
			OptionNameColumn.Title = param.ParamT("OWP_TVOpt_Label");
			UpdateOptionsTreeView();
			
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
				InitPropertiesTreeView();
				UpdatePropertiesTreeView();				
			}
			else if (OptionName == BoardOldName)
			{
				foreach(TreeViewColumn TVchild in ChildColumnList)
				{
					ChildTreeView.RemoveColumn(TVchild);
				}
			
				if(ChildColumnList.Count >0)
				{ 
					ChildColumnList.RemoveRange(0,ChildColumnList.Count);
				}	
			
				InitChildTreeView_Board();
				UpdateChildTreeView_Board(false);
			}
			else if(OptionName ==  PropOldName)
			{
				foreach(TreeViewColumn TVchild in ChildColumnList)
				{
					ChildTreeView.RemoveColumn(TVchild);
				}
			
				if(ChildColumnList.Count >0)
				{ 
					ChildColumnList.RemoveRange(0,ChildColumnList.Count);
				}
				InitPropertiesTreeView();
				UpdatePropertiesTreeView();
			}			
		}
		
		//Fonction WidgetIsCorrect
		//Fcontion peremettant de saoir si un widget est correct, si il nous retourne de la data
		public bool WidgetIsCorrect()
		{
			foreach(Project Pro in datamanagement.ListProject)//Pour chaque projet de la liste
			{	
				foreach(Node nod in Pro.ReturnListNode())
				{
					foreach(Network net in nod.ReturnListNetwork())
					{
						if(net.Network_Id == Network_Id)//si un projet est égale à l'id project mis en paramètre
						{
							return true;
						}
					}
				}
			}
			return false;
		}			
		
		//Fonction OnTextViewNoteFocusOutEvent
		//Fonction permettant de modifier les notes d'un réseau 1-Wire
		protected void OnTextViewNoteFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
		{
			datamanagement.ModifyNetwork(TextViewNote.Buffer.Text,param.ParamI("MoNet_ChoiceNote"),Network_Id);
		}
		
		//Fonction OnButtonAddProbeClicked
		//Fonction permettant d'ajouter une sonde à partir d'un bouton
		protected void OnButtonAddProbeClicked (object sender, System.EventArgs e)
		{
			for(int i = 0;i<Convert.ToInt16(ComboboxNumberOfProbe.ActiveText);i++)
			{
				datamanagement.AddBoardInNetwork(ComboboxTypeOfProbe.ActiveText,datamanagement.ReturnNewNameBoard(param.ParamT("OWP_DefaultProbeName"),Network_Id),Network_Id);
			}		
		}

		//Fonction OnButtonDeleteProbeClicked
		//Fonction permettant de supprimer une sonde à partir d'un bouton
		protected void OnButtonDeleteProbeClicked (object sender, System.EventArgs e)
		{
			string IdSelected = "";	//variable permettant de stocker l'id de l'instance sélectionné
	
			TreeSelection selection = ChildTreeView.Selection; //Nous allons crée un arbre de selection
			if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
			{
				IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("OWP_TVChild_OpBoard_PositionID")); //Nous retournons l'id de l'instance		
			}	
			if(IdSelected != "")
			{
				datamanagement.DeleteBoardInNetwork(Convert.ToInt32(IdSelected));
			}
		}
	}
}