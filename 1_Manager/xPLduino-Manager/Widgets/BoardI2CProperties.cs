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
	public partial class BoardI2CProperties : Gtk.Bin
	{
		public DataManagement datamanagement;
		public Param param;
		public Int32 Board_Id;
		public Int32 Project_Id;
		public string SelectOption;
		
		public string PropOldName;
		public string PinOldName = "";
		
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
		public global::Gdk.Pixbuf PngPin;
		public global::Gdk.Pixbuf PngPropertie;
		
		public global::Gtk.TreeIter IterNode;
		
		public global::Gtk.TreeModel TreeModelOptionTreeView;
		
		public string OptionName;
		public bool DisplayAllNode;	
		
		//Utilisation de ses variables pour le treeview child : général
		
		public List<TreeViewColumn> ChildColumnList;
		public global::Gtk.TreeIter IterChild;
		public global::Gtk.TreeModel TreeModelChildTreeView;		
		
		//Utilisation de ses variables pour le treeview child : option board
		public global::Gtk.TreeViewColumn Child_Pin_NameColumn;
		public global::Gtk.TreeViewColumn Child_Pin_DirectionColumn;
		public global::Gtk.TreeViewColumn Child_Pin_FallbackValueColumn;
		public global::Gtk.TreeViewColumn Child_Pin_LinkInstanceColumn;
		public global::Gtk.TreeViewColumn Child_Pin_EmptyColumn;
		public global::Gtk.TreeViewColumn Child_Pin_IdColumn;		
		
		public global::Gtk.CellRendererText Child_Pin_NameCell;
		public global::Gtk.CellRendererText Child_Pin_DirectionCell;
		public global::Gtk.CellRendererToggle Child_Pin_FallbackValueCell;
		public global::Gtk.CellRendererCombo Child_Pin_LinkInstanceCell;
		public global::Gtk.CellRendererText Child_Pin_EmptyCell;
		public global::Gtk.CellRendererText Child_Pin_IdCell;		
		
		public global::Gtk.TreeStore Child_Pin_ListStore;		
		public Gtk.ListStore ListInstance;
		
		public BoardI2CProperties (DataManagement _datamanagement, Param _param, Int32 _Board_Id)
		{
			this.datamanagement = _datamanagement;
			this.param = _param;
			this.Board_Id = _Board_Id;
			this.Build ();
			
			ChildColumnList = new List<TreeViewColumn>();	
			InitWidget();
			InitOptionsTreeView();
			UpdateOptionsTreeView();
			
			ListInstance = new Gtk.ListStore(typeof (string));
			
			OptionTreeView.ButtonReleaseEvent += new ButtonReleaseEventHandler(OptionsTreeViewButtonRelease);
			
			//Permet de retourner l'id du projet
			foreach(Project pro in datamanagement.ListProject.Values)
			{
				foreach(Node node in pro.ReturnListNode())
				{
					foreach(Network net in node.ReturnListNetwork())
					{
						foreach(Board boa in net.ReturnListBoard())
						{
							if(boa.Board_Id == _Board_Id)
							{
								datamanagement.CurrentProjectId = pro.Project_Id;
								Project_Id = pro.Project_Id;
							}
						}
					}    
				}
			}
		}
		
		//Fonction InitWidget
		//Fonction permettant d'initialiser le widget
		public void InitWidget()
		{
			LabelChildTreeView.Text = param.ParamT("BI2CP_LabTVChild_Name_Choose");
			NoteLabel.Text = param.ParamT("BI2CP_NoteLabel");
			vpaned1.Position = 0;
			hpaned1.Position = (datamanagement.mainwindow.ReturnHpanedPosition() * param.ParamI("NoteHPanedPurcent")) / 100;
			foreach(Project Pro in datamanagement.ListProject.Values)
			{
				foreach(Node node in Pro.ReturnListNode())
				{
					foreach(Network net in node.ReturnListNetwork())
					{
						foreach(Board boa in net.ReturnListBoard())
						{
							if(net.Network_Id == Board_Id)
							{
								TextViewNote.Buffer.Text = boa.Board_Note;			
							}
						}
					}
				}
			}
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
			PngPin = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconPin"));	
			PngPropertie = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconProperties"));
			
			//Nous donnons un titre au colonnes
			OptionNameColumn.Title = param.ParamT("PP_TVOpt_NameLabel");
			
			//Création des cellules
			OptionPixCell = new Gtk.CellRendererPixbuf ();
			OptionCell = new Gtk.CellRendererText ();
			
			//On associe des cellules au colonne
			OptionNameColumn.PackStart(OptionPixCell, false);
			OptionNameColumn.PackStart(OptionCell, true);	
			
			//Ajout des colonnes dans OptionTreeView
			OptionTreeView.AppendColumn (OptionNameColumn);
			
			//Ajout des attibut à chaque colonne
			OptionNameColumn.AddAttribute (OptionPixCell, "pixbuf", param.ParamI("PP_TVOpt_PostionPixBuff"));
			OptionNameColumn.AddAttribute(OptionCell,"text",param.ParamI("PP_TVOpt_PostionName"));	
			
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
			IterNode = OptionListStore.AppendValues(PngPropertie,param.ParamT("BI2CP_TVOpt_OptionProperties"));
			IterNode = OptionListStore.AppendValues(PngPin,param.ParamT("BI2CP_TVOpt_OptionPin"));			
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
						OptionName = (string) TreeModelOptionTreeView.GetValue (IterNode, param.ParamI("PP_TVOpt_PostionName")); //Nous mettons la valeur de la 1ere cellule dans un string (texte)
						SelectOption = OptionName;
					

						if (OptionName == param.ParamT("BI2CP_TVOpt_OptionPin"))
						{
							PinOldName = OptionName;
							foreach(TreeViewColumn TVchild in ChildColumnList)
							{
								ChildTreeView.RemoveColumn(TVchild);
							}
						
							if(ChildColumnList.Count >0)
							{ 
								ChildColumnList.RemoveRange(0,ChildColumnList.Count);
							}	
						
							InitChildTreeView_Pin();
							UpdateChildTreeView_Pin();
						}
						else if (OptionName == param.ParamT("BI2CP_TVOpt_OptionProperties"))
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
			LabelChildTreeView.Text = param.ParamT("BI2CP_LabTVChild_Name_PinProperties");			
			ChildTreeView.EnableGridLines = TreeViewGridLines.Both;
			
			//Ajout de deux colonnes dans TreeviewProperties
			PropertiesTypeColumn = new Gtk.TreeViewColumn();
			PropertiesValueColumn = new Gtk.TreeViewColumn();
			PropertiesEmptyColumn = new Gtk.TreeViewColumn();
			
			//Nous donnons un titre au colonne
			PropertiesTypeColumn.Title = param.ParamT("BI2CP_TVPro_TypeLabel");
			PropertiesValueColumn.Title = param.ParamT("BI2CP_TVPro_ValueLabel");
			
			ChildColumnList.Add(PropertiesTypeColumn);
			ChildColumnList.Add(PropertiesValueColumn);
			ChildColumnList.Add(PropertiesEmptyColumn);				
			
			//Taille des colonnes
			PropertiesValueColumn.MinWidth = 150;
			
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
			PropertiesTypeColumn.AddAttribute(PropertiesTypeCell,"text",param.ParamI("BI2CP_TVPro_TypePosition"));
			PropertiesValueColumn.AddAttribute (PropertiesValueCell, "text", param.ParamI("BI2CP_TVPro_ValuePosition"));
			
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

			foreach(Project Pro in datamanagement.ListProject.Values)
			{
				foreach(Node node in Pro.ReturnListNode())
				{
					foreach(Network net in node.ReturnListNetwork())
					{
						foreach(Board boa in net.ReturnListBoard())
						{
							if(boa.Board_Id == Board_Id)
							{						
								PropertiesListStore.AppendValues(param.ParamT("BI2CP_TVPro_LabelLine1"),boa.Board_Name);	
								PropertiesListStore.AppendValues(param.ParamT("BI2CP_TVPro_LabelLine2"),boa.Board_Type);
								PropertiesListStore.AppendValues(param.ParamT("BI2CP_TVPro_LabelLine3"),boa.Board_I2C_0.ToString());
								foreach(Boards boas in datamanagement.ListBoards)
								{
									if(boa.Board_Type == boas.Type)
									{
										if(boas.NumberI2CAdress == 2)
										{
											PropertiesListStore.AppendValues(param.ParamT("BI2CP_TVPro_LabelLine4"),boa.Board_I2C_1.ToString());
										}
									}
								}
								
								TextViewNote.Buffer.Text = boa.Board_Note;
							}
						}
					}
				}
			}				
			
			//On écrit le store dans le treeview
			ChildTreeView.Model = PropertiesListStore;
			ChildTreeView.ShowAll ();			
		}
		
			//Fonction PropertiesValueCell_Edited
			//Fonction permettant de mettre à jour les propriété d'une carte en cliquant sur une cellule
			private void PropertiesValueCell_Edited (object o, Gtk.EditedArgs args)
			{
				string BoardNameSelected = "";
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelOptionTreeView, out IterNode)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					BoardNameSelected = (string) TreeModelOptionTreeView.GetValue (IterNode, param.ParamI("BI2CP_TVPro_ValuePosition")); //Nous mettons la valeur de la 1ere cellule dans un string (texte)			
				}			
			
				Gtk.TreeIter iter; //Conception d'un TreeIter permettant de retourner la position de la valeur cliqué
				PropertiesListStore.GetIter (out iter, new Gtk.TreePath (args.Path)); //Nous allons cherché cette valeur qui est arg.Path ainsi que le nouveau text dans args.Text
			
			
			
				//Convert.ToByte(args.Path) : Nous retrourne la ligne sélectionné en INT
				//Puis on compare la valeur avec un paramètre enregistré
				if(Convert.ToByte(args.Path) == param.ParamI("BI2CP_TVPro_NamePostion") && (args.NewText != BoardNameSelected))
				{
					if(args.NewText != "") //Si le texte est pas vide
					{
						if(args.NewText.Length <= param.ParamI("CarSizeMax"))
						{				
							datamanagement.ModifyBoard(args.NewText,param.ParamI("MoBo_ChoiceName"),Board_Id);
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
				else if(Convert.ToByte(args.Path) == param.ParamI("BI2CP_TVPro_AdI2C0Postion"))
				{
					string CurrentTypeBoard = "";
					Int32 MaxI2CAdress = 0;
					foreach(Project Pro in datamanagement.ListProject.Values)
					{
						foreach(Node nod in Pro.ReturnListNode())
						{
							foreach(Network net in nod.ReturnListNetwork())
							{
								foreach(Board boa in net.ReturnListBoard())
								{
									if(boa.Board_Id == Board_Id)
									{
										CurrentTypeBoard = boa.Board_Type;
									}
								}
							}
						}
					}
					foreach(Boards boas in datamanagement.ListBoards)
					{
						if(boas.Type == CurrentTypeBoard)
						{
							MaxI2CAdress = boas.MaxI2CAdress;
						}
					}
					if(args.NewText.Length == 1)
					{		
						if((Convert.ToInt32(args.NewText[0]) >= 48) && (Convert.ToInt32(args.NewText[0]) <= 57))
				  		{								
							if(Convert.ToInt32(args.NewText) <= MaxI2CAdress-1)
							{
								datamanagement.ModifyBoard(args.NewText,param.ParamI("MoBo_ChoiceI2C0"),Board_Id);
							}	
							else
							{
								datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"I2CAdressTooBig");
							}
						}
						else
						{
							datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"I2CAdressNotCorrect");	
						}
					}
					else if (args.NewText.Length == 2)
					{
						if((Convert.ToInt32(args.NewText[0]) >= 48) && (Convert.ToInt32(args.NewText[0]) <= 57) && (Convert.ToInt32(args.NewText[1]) >= 48) && (Convert.ToInt32(args.NewText[1]) <= 57))
				  		{
							if(Convert.ToInt32(args.NewText) <= MaxI2CAdress-1)
							{
								datamanagement.ModifyBoard(args.NewText,param.ParamI("MoBo_ChoiceI2C0"),Board_Id);
							}	
							else
							{
								datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"I2CAdressTooBig");
							}
						}
						else
						{
							datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"I2CAdressNotCorrect");	
						}
					}
					else
					{
						datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"I2CAdressNotCorrect");
						datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"I2CAdressTooBig");
					}
					
				}
				else if(Convert.ToByte(args.Path) == param.ParamI("BI2CP_TVPro_AdI2C1Postion"))
				{
					string CurrentTypeBoard = "";
					Int32 MaxI2CAdress = 0;
					foreach(Project Pro in datamanagement.ListProject.Values)
					{
						foreach(Node nod in Pro.ReturnListNode())
						{
							foreach(Network net in nod.ReturnListNetwork())
							{
								foreach(Board boa in net.ReturnListBoard())
								{
									if(boa.Board_Id == Board_Id)
									{
										CurrentTypeBoard = boa.Board_Type;
									}
								}
							}
						}
					}
					foreach(Boards boas in datamanagement.ListBoards)
					{
						if(boas.Type == CurrentTypeBoard)
						{
							MaxI2CAdress = boas.MaxI2CAdress;
						}
					}				
					if(args.NewText.Length == 1)
					{		
						if((Convert.ToInt32(args.NewText[0]) >= 48) && (Convert.ToInt32(args.NewText[0]) <= 57))
				  		{								
							if(Convert.ToInt32(args.NewText) <= MaxI2CAdress - 1)
							{
								datamanagement.ModifyBoard(args.NewText,param.ParamI("MoBo_ChoiceI2C1"),Board_Id);
							}	
							else
							{
								datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"I2CAdressTooBig");
							}
						}
						else
						{
							datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"I2CAdressNotCorrect");	
						}
					}
					else if (args.NewText.Length == 2)
					{
						if((Convert.ToInt32(args.NewText[0]) >= 48) && (Convert.ToInt32(args.NewText[0]) <= 57) && (Convert.ToInt32(args.NewText[1]) >= 48) && (Convert.ToInt32(args.NewText[1]) <= 57))
				  		{
							if(Convert.ToInt32(args.NewText) <= MaxI2CAdress - 1)
							{
								datamanagement.ModifyBoard(args.NewText,param.ParamI("MoBo_ChoiceI2C1"),Board_Id);
							}	
							else
							{
								datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"I2CAdressTooBig");
							}						
						}	
						else
						{
							datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"I2CAdressNotCorrect");	
						}					
					}
					else
					{
						datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"I2CAdressNotCorrect");
						datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"I2CAdressTooBig");
					}
				}
		
				UpdatePropertiesTreeView(); //On met à jour le PropertiesTreeview
			}			
		
//###################### TV Child Choice Pin #########################	
		
		//Fonction InitChildTreeView_Pin
		//Fcontion permettant d'initialiser le treeview cbild lorsque le chois se fait pour une carte
		public void InitChildTreeView_Pin()
		{
			LabelChildTreeView.Text = param.ParamT("BI2CP_LabTVChild_Name_PinList");			
			
			//Visibilité des lignes et des colonnes
			ChildTreeView.EnableGridLines = TreeViewGridLines.Both;
			ChildTreeView.RubberBanding = true;
			
			//Ajout des colonnes
			Child_Pin_NameColumn = new Gtk.TreeViewColumn();
			Child_Pin_DirectionColumn = new Gtk.TreeViewColumn();
			Child_Pin_FallbackValueColumn = new Gtk.TreeViewColumn();
			Child_Pin_LinkInstanceColumn = new Gtk.TreeViewColumn();
			Child_Pin_IdColumn = new Gtk.TreeViewColumn();
			Child_Pin_EmptyColumn = new Gtk.TreeViewColumn();	
			
			//On ajoute les colonnes dans une liste
			ChildColumnList.Add(Child_Pin_NameColumn);
			ChildColumnList.Add(Child_Pin_DirectionColumn);
			ChildColumnList.Add(Child_Pin_FallbackValueColumn);
			ChildColumnList.Add(Child_Pin_LinkInstanceColumn);	
			ChildColumnList.Add(Child_Pin_IdColumn);			
			ChildColumnList.Add(Child_Pin_EmptyColumn);
			
			//Colonne redimensionnable
			Child_Pin_NameColumn.Resizable = true;
			Child_Pin_DirectionColumn.Resizable = true;
			Child_Pin_FallbackValueColumn.Resizable = true;
			Child_Pin_LinkInstanceColumn.Resizable = true;		
			Child_Pin_IdColumn.Resizable = true;	
			Child_Pin_EmptyColumn.Resizable = true;	
			
			//Taille de colonne
			Child_Pin_LinkInstanceColumn.MinWidth = 200;
			
			//Nom des colonne
			Child_Pin_NameColumn.Title = param.ParamT("BI2CP_TVChild_OpPin_Name");
			Child_Pin_DirectionColumn.Title = param.ParamT("BI2CP_TVChild_OpPin_Direction");
			Child_Pin_FallbackValueColumn.Title = param.ParamT("BI2CP_TVChild_OpPin_Fallbackvalue");
			Child_Pin_LinkInstanceColumn.Title = param.ParamT("BI2CP_TVChild_OpPin_LinkInstance");			
			Child_Pin_IdColumn.Title = param.ParamT("BI2CP_TVChild_OpPin_ID");
						
			//Visibilité des colonnes
			Child_Pin_IdColumn.Visible = false;
			
			//Création des cellules
			Child_Pin_NameCell = new Gtk.CellRendererText ();
			Child_Pin_DirectionCell = new Gtk.CellRendererText ();
			Child_Pin_FallbackValueCell = new Gtk.CellRendererToggle();
			Child_Pin_LinkInstanceCell = new Gtk.CellRendererCombo (); 
			Child_Pin_IdCell = new Gtk.CellRendererText ();	
			
			//On rend les cellules éditables
			Child_Pin_NameCell.Editable = false;
			Child_Pin_DirectionCell.Editable = false;
			Child_Pin_FallbackValueCell.Activatable = true;
			Child_Pin_LinkInstanceCell.Editable = true; 		
			Child_Pin_IdCell.Editable = false;
			
			Child_Pin_LinkInstanceCell.Model = ListInstance;
			Child_Pin_LinkInstanceCell.TextColumn = 0;
			
			//En cas d'edition des cellule, on appel les fonction associé
			Child_Pin_LinkInstanceCell.Edited += Child_Pin_LinkInstanceCell_Edited;
			Child_Pin_FallbackValueCell.Toggled += Child_Pin_FallbackValueCell_Toggled;
			
			//On associe des cellules aux colonnes			
			Child_Pin_NameColumn.PackStart (Child_Pin_NameCell,true);
			Child_Pin_DirectionColumn.PackStart (Child_Pin_DirectionCell,true);
			Child_Pin_FallbackValueColumn.PackStart (Child_Pin_FallbackValueCell,true);
			Child_Pin_LinkInstanceColumn.PackStart (Child_Pin_LinkInstanceCell,false); 			
			Child_Pin_IdColumn.PackStart (Child_Pin_IdCell,true);			
			
			//Ajout des colonnes dans ExplorerTreeView
			ChildTreeView.AppendColumn (Child_Pin_NameColumn);
			ChildTreeView.AppendColumn (Child_Pin_DirectionColumn);
			ChildTreeView.AppendColumn (Child_Pin_FallbackValueColumn);
			ChildTreeView.AppendColumn (Child_Pin_LinkInstanceColumn);
			ChildTreeView.AppendColumn (Child_Pin_IdColumn);			
			ChildTreeView.AppendColumn (Child_Pin_EmptyColumn);
			
			//Ajout des attibuts à chaque colonne
			Child_Pin_NameColumn.AddAttribute(Child_Pin_NameCell,"text",param.ParamI("BI2CP_TVChild_OpPin_PositionName"));
			Child_Pin_DirectionColumn.AddAttribute(Child_Pin_DirectionCell,"text",param.ParamI("BI2CP_TVChild_OpPin_PositionDirection"));
			Child_Pin_FallbackValueColumn.AddAttribute(Child_Pin_FallbackValueCell,"active",param.ParamI("BI2CP_TVChild_OpPin_PositionFallbackvalue"));
			Child_Pin_LinkInstanceColumn.AddAttribute(Child_Pin_LinkInstanceCell,"text",param.ParamI("BI2CP_TVChild_OpPin_PositionLinkInstance"));
			Child_Pin_IdColumn.AddAttribute(Child_Pin_IdCell,"text",param.ParamI("BI2CP_TVChild_OpPin_PositionID"));
			
			//Création d'un nouveau store
			Child_Pin_ListStore = new Gtk.TreeStore(typeof(string),typeof(string),typeof(bool),typeof(string),typeof(string));	
			
			//On écrit le store dans le treeview
			ChildTreeView.Model = Child_Pin_ListStore;
			ChildTreeView.ShowAll ();			
		}
		
		//Fonction UpdateChildTreeView_Board
		//Fonction permettant de mettre à jour le child treeview
		public void UpdateChildTreeView_Pin()
		{
			string InstanceName = "";
			string ExtensionInstanceName = "";
			Child_Pin_ListStore.Clear();
			foreach(Project Pro in datamanagement.ListProject.Values)
			{	
				foreach(Node node in Pro.ReturnListNode())
				{
					foreach(Network net in node.ReturnListNetwork())
					{
						foreach(Board boa in net.ReturnListBoard())
						{	
							if(boa.Board_Id == Board_Id)
							{
								foreach(Pin pin in boa.ReturnListPin())
								{
									foreach(Instance ins in node.ReturnListInstance())
									{
										if(ins.Instance_Id == pin.Instance_Id)
										{
											InstanceName = ins.Instance_Name;
											if(ins.Instance_Type == param.ParamP("InstShutterName"))
											{
												if(ins.Pin_Id_0 == pin.Pin_Id)
												{
													ExtensionInstanceName = param.ParamT("BI2CP_TVChild_Up");
												}
												else if(ins.Pin_Id_1 == pin.Pin_Id)
												{
													ExtensionInstanceName = param.ParamT("BI2CP_TVChild_Down");
												}	
												else if(ins.Pin_Id_2 == pin.Pin_Id)
												{
													ExtensionInstanceName = param.ParamT("BI2CP_TVChild_Stop");
												}		
											}
										}
									}
									
									if(ExtensionInstanceName == "")
									{
										IterChild = Child_Pin_ListStore.AppendValues(pin.Pin_Name,pin.Pin_Direction,pin.Pin_FallbackValue,InstanceName,pin.Pin_Id.ToString());
									}
									else
									{
										IterChild = Child_Pin_ListStore.AppendValues(pin.Pin_Name,pin.Pin_Direction,pin.Pin_FallbackValue,InstanceName + " (" + ExtensionInstanceName + ")",pin.Pin_Id.ToString());
									}
									InstanceName = "";
									ExtensionInstanceName = "";
								}
							}
						}
					}
				}
			}
			ChildTreeView.Model = Child_Pin_ListStore;
			ChildTreeView.ShowAll();			
		}		
		
			//Fonction Child_Pin_NameCell_Edited
			//Fonction permettant de modifier le nom d'une carte
			void Child_Pin_LinkInstanceCell_Edited (object o, EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				Int32 IdInstance = 0;
				
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("BI2CP_TVChild_OpPin_PositionID")); //Nous retournons l'id du noeud selectionné
				}	 
				
			foreach(Project Pro in datamanagement.ListProject.Values)
				{
					foreach(Node node in Pro.ReturnListNode())
					{
						foreach(Network net in node.ReturnListNetwork())
						{
							foreach(Board boa in net.ReturnListBoard())
							{		
								if(boa.Board_Id == Board_Id)
								{
									foreach(Instance ins in node.ReturnListInstance())
									{
										if(ins.Instance_Name == args.NewText)
										{	
											IdInstance = ins.Instance_Id;
										}
									}
								}
							}
						}
					}
				}
				
				datamanagement.ModifyInstancePinInBoard(IdInstance,Convert.ToInt32(IdSelected));
				datamanagement.UpdateInstanceUsed();
				
			}
		
			//Fonction Child_Pin_FallbackValueCell_Toggled
			//Fonction permettant de modifier la valeur combobox lors d'un appui
			void Child_Pin_FallbackValueCell_Toggled (object o, Gtk.ToggledArgs args)
			{
				bool Value = false; //Initialisation d'un bool permettant de récuperer la valeur lors d'un clic
				string Direction = "";
			
				Gtk.TreeIter iter;
				if (Child_Pin_ListStore.GetIter (out iter, new Gtk.TreePath(args.Path))) {
					Value = (bool) Child_Pin_ListStore.GetValue(iter,param.ParamI("BI2CP_TVChild_OpPin_PositionFallbackvalue")); //on fait l'acquisition de la valeur
					Direction = (string) Child_Pin_ListStore.GetValue(iter,param.ParamI("BI2CP_TVChild_OpPin_PositionDirection"));
				
					if(Direction == param.ParamP("Direction1"))
				  	{
						Child_Pin_ListStore.SetValue(iter,param.ParamI("BI2CP_TVChild_OpPin_PositionFallbackvalue"),!Value);//On affiche la valeur inversé
					}
				}					
			}
		
//###################### Other  ####################################	
		
		//Fonction OnChildTreeViewButtonReleaseEvent
		//Mise à jour de ListInstance
		protected void OnChildTreeViewButtonReleaseEvent (object o, Gtk.ButtonReleaseEventArgs args)
		{
			if(SelectOption == param.ParamT("BI2CP_TVOpt_OptionPin") || SelectOption == PinOldName)
			{
				ListInstance.Clear();
				string DirectionSelected = "";	//variable permettant de stocker le nom selectionné
				bool ChildFallbackValue = false;
				string ChildIDSelect = "";
				
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					DirectionSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("BI2CP_TVChild_OpPin_PositionDirection")); //Nous retournons le nom du noeud selectionné		
					ChildFallbackValue = (bool) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("BI2CP_TVChild_OpPin_PositionFallbackvalue")); //Nous récuperons la valeur que nous stockons
					ChildIDSelect = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("BI2CP_TVChild_OpPin_PositionID")); //Nous récuperons la valeur que nous stockons	
				}	
				
				foreach(Project Pro in datamanagement.ListProject.Values)
				{
					foreach(Node node in Pro.ReturnListNode())
					{
						foreach(Network net in node.ReturnListNetwork())
						{
							foreach(Board boa in net.ReturnListBoard())
							{
								if(boa.Board_Id == Board_Id)
								{
									foreach(Instance ins in node.ReturnListInstance())
									{
										if(ins.Instance_Type != param.ParamP("InstShutterName"))
										{
											if((ins.Instance_Direction == DirectionSelected) && !ins.Instance_Used)
											{
												ListInstance.AppendValues(ins.Instance_Name);
											}
										}
										else
										{
											if(ins.Instance_SHU_NumberOfOutput == 2)
											{
												if((ins.Instance_Used_0 > 0) && (boa.Board_Type != param.ParamP("BI2CP_BoardForbidderForShutter")))
												{
													if((ins.Instance_Used_0 == Board_Id) && (ins.Instance_Direction == DirectionSelected) && !ins.Instance_Used)
													{
														ListInstance.AppendValues(ins.Instance_Name);
													}
												}
												else if((ins.Instance_Used_0 == 0) &&  (ins.Instance_Direction == DirectionSelected)  && !ins.Instance_Used && (boa.Board_Type != param.ParamP("BI2CP_BoardForbidderForShutter")))
												{
													ListInstance.AppendValues(ins.Instance_Name);
												}
											}
											else if(ins.Instance_SHU_NumberOfOutput == 3)
											{
												if((ins.Instance_Used_0 > 0) && (ins.Instance_Used_1 > 0) && (boa.Board_Type != param.ParamP("BI2CP_BoardForbidderForShutter")) && !ins.Instance_Used && (ins.Instance_Direction == DirectionSelected) && !ins.Instance_Used)
												{
													if((ins.Instance_Used_0 == Board_Id))
													{
														ListInstance.AppendValues(ins.Instance_Name);
													}
												}
												else if((ins.Instance_Used_0 > 0) && (ins.Instance_Used_1 == 0) && (ins.Instance_Direction == DirectionSelected) &&  !ins.Instance_Used && (boa.Board_Type != param.ParamP("BI2CP_BoardForbidderForShutter")) )
												{
													if((ins.Instance_Used_0 == Board_Id))
													{
														ListInstance.AppendValues(ins.Instance_Name);
													}												
												}
												else if((ins.Instance_Used_0 == 0) && (ins.Instance_Used_1 == 0) && (ins.Instance_Direction == DirectionSelected)  &&  !ins.Instance_Used && (boa.Board_Type != param.ParamP("BI2CP_BoardForbidderForShutter")))
												{
													ListInstance.AppendValues(ins.Instance_Name);
												}											
											}
										}
									}
									foreach(Pin pin in boa.ReturnListPin())
									{
										if(ChildIDSelect != "")
										{
											if(pin.Pin_Id == Convert.ToInt32(ChildIDSelect) && pin.Pin_FallbackValue != ChildFallbackValue) //Si l'id est égale mais que la valeur DHCP est différente
											{
												//datamanagement.ModifyNode(ChildDHCPSelect.ToString(),param.ParamI("MoNo_ChoiceDHCP"),Convert.ToInt32(ChildIDSelect));
												datamanagement.ModifyPin(ChildFallbackValue.ToString(),param.ParamI("MoPi_ChoiceFallbackValue"),Convert.ToInt32(ChildIDSelect));
											}
										}
									}
								}
							}
						}
					}
				}
				ListInstance.AppendValues(param.ParamT("BI2CP_EmptyLabel"));					
			}
		}
		
		//Fonction OnChildTreeViewKeyReleaseEvent
		//Va nous permettre de supprimer un lien en cas d'appuie sur la touche suppr
		protected void OnChildTreeViewKeyReleaseEvent (object o, Gtk.KeyReleaseEventArgs args)
		{
			if(OptionName == param.ParamT("BI2CP_TVOpt_OptionPin"))
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				string IdInstanceSelected = "";
		
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("BI2CP_TVChild_OpPin_PositionID")); //Nous retournons l'id du noeud selectionné
					IdInstanceSelected  = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("BI2CP_TVChild_OpPin_PositionLinkInstance")); 
				}				
				
				if(args.Event.Key.ToString() == param.ParamP("BI2CP_TouchDelete") && IdInstanceSelected != "")
				{
					datamanagement.ModifyInstancePinInBoard(0,Convert.ToInt32(IdSelected));
					UpdateChildTreeView_Pin(); //On met à jour le ChildTreeview	
					datamanagement.UpdateInstanceUsed();				
				}
			}
		}
		
		//Fonction UpdateWidget
		//Fonction permettant de mettre à jour le widget
		public void UpdateWidget()
		{
			NoteLabel.Text = param.ParamT("BI2CP_NoteLabel");
			
			//Init and update OptionTreeView		
			OptionNameColumn.Title = param.ParamT("PP_TVOpt_NameLabel");
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
			else if (OptionName == PinOldName)
			{
				foreach(TreeViewColumn TVchild in ChildColumnList)
				{
					ChildTreeView.RemoveColumn(TVchild);
				}
			
				if(ChildColumnList.Count >0)
				{ 
					ChildColumnList.RemoveRange(0,ChildColumnList.Count);
				}	
			
				InitChildTreeView_Pin();
				UpdateChildTreeView_Pin();
			}
			else if (OptionName == PropOldName)
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
			foreach(Project Pro in datamanagement.ListProject.Values)//Pour chaque projet de la liste
			{	
				foreach(Node nod in Pro.ReturnListNode())
				{
					foreach(Network net in nod.ReturnListNetwork())
					{
						foreach(Board boa in net.ReturnListBoard())
						{
							if(boa.Board_Id == Board_Id)//si un projet est égale à l'id project mis en paramètre
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}			
		
		//Fonction OnTextViewNoteFocusOutEvent
		//Fonction permettant d'enregistrer les notes d'un carte I2C
		protected void OnTextViewNoteFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
		{
			datamanagement.ModifyBoard(TextViewNote.Buffer.Text,param.ParamI("MoBo_ChoiceNote"),Board_Id);
		}
	}
}