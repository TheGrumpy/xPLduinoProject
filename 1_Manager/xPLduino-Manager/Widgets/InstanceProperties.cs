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
	public partial class InstanceProperties : Gtk.Bin
	{
		public DataManagement datamanagement;
		public Param param;
		public Int32 Node_Id;
		public Int32 Project_Id;
		public string TypeInstance;		
		public global::Gtk.TreeModel TreeModelChildTreeView;
		public global::Gtk.TreeIter IterChild;
		
		//Utilisation de ses variables pour le treeview child : option lighting
		public global::Gtk.TreeViewColumn Child_Lighting_NameColumn;
		public global::Gtk.TreeViewColumn Child_Lighting_DefaultValueColumn;
		public global::Gtk.TreeViewColumn Child_Lighting_FadeColumn;
		public global::Gtk.TreeViewColumn Child_Lighting_PathPinColumn;
		public global::Gtk.TreeViewColumn Child_Lighting_IdColumn;
		public global::Gtk.TreeViewColumn Child_Lighting_EmptyColumn;
		
		public global::Gtk.CellRendererText Child_Lighting_NameCell;
		public global::Gtk.CellRendererText Child_Lighting_DefaultValueCell;
		public global::Gtk.CellRendererText Child_Lighting_FadeCell;
		public global::Gtk.CellRendererText Child_Lighting_PathPinCell;
		public global::Gtk.CellRendererText Child_Lighting_IdCell;
		public global::Gtk.CellRendererText Child_Lighting_EmptyCell;
			
		//Utilisation de ses variables pour le treeview child : option Switch
		public global::Gtk.TreeViewColumn Child_Switch_NameColumn;
		public global::Gtk.TreeViewColumn Child_Switch_InverseColumn;
		public global::Gtk.TreeViewColumn Child_Switch_ImpulsionTimeColumn;
		public global::Gtk.TreeViewColumn Child_Switch_PathPinColumn;
		public global::Gtk.TreeViewColumn Child_Switch_IdColumn;
		public global::Gtk.TreeViewColumn Child_Switch_EmptyColumn;
		
		public global::Gtk.CellRendererText Child_Switch_NameCell;
		public global::Gtk.CellRendererToggle Child_Switch_InverseCell;
		public global::Gtk.CellRendererText Child_Switch_ImpulsionTimeCell;
		public global::Gtk.CellRendererText Child_Switch_PathPinCell;
		public global::Gtk.CellRendererText Child_Switch_IdCell;
		public global::Gtk.CellRendererText Child_Switch_EmptyCell;		
		
		//Utilisation de ses variables pour le treeview child : option Shutter		
		public global::Gtk.TreeViewColumn Child_Shutter_NameColumn;
		public global::Gtk.TreeViewColumn Child_Shutter_NumberOfOutputColumn;
		public global::Gtk.TreeViewColumn Child_Shutter_ShutterTypeColumn;
		public global::Gtk.TreeViewColumn Child_Shutter_ShutterTimeColumn;
		public global::Gtk.TreeViewColumn Child_Shutter_ShutterInitTimeColumn;
		public global::Gtk.TreeViewColumn Child_Shutter_PathPin1Column;
		public global::Gtk.TreeViewColumn Child_Shutter_PathPin2Column;
		public global::Gtk.TreeViewColumn Child_Shutter_PathPin3Column;		
		public global::Gtk.TreeViewColumn Child_Shutter_AppearenceOrderColumn;
		public global::Gtk.TreeViewColumn Child_Shutter_IdColumn;
		public global::Gtk.TreeViewColumn Child_Shutter_EmptyColumn;
		
		public global::Gtk.CellRendererText Child_Shutter_NameCell;
		public global::Gtk.CellRendererCombo Child_Shutter_NumberOfOutputCell;
		public global::Gtk.CellRendererCombo Child_Shutter_ShutterTypeCell;
		public global::Gtk.CellRendererText Child_Shutter_ShutterTimeCell;
		public global::Gtk.CellRendererText Child_Shutter_ShutterInitTimeCell;
		public global::Gtk.CellRendererText Child_Shutter_PathPin1Cell;
		public global::Gtk.CellRendererText Child_Shutter_PathPin2Cell;
		public global::Gtk.CellRendererText Child_Shutter_PathPin3Cell;
		public global::Gtk.CellRendererCombo Child_Shutter_AppearenceOrderCell;
		public global::Gtk.CellRendererText Child_Shutter_IdCell;
		public global::Gtk.CellRendererText Child_Shutter_EmptyCell;	
		
		public Gtk.ListStore ListUpDownStop;
		public Gtk.ListStore ListNumberOfOutput;
		public Gtk.ListStore ListTypeShutter;
		
		public global::Gtk.TreeStore Child_Instance_ListStore;	
		
		public string IDMemorised = "";
		
		public InstanceProperties (DataManagement _datamanagement, Param _param,string _TypeInstance, Int32 _Node_Id)
		{
			this.datamanagement = _datamanagement;
			this.param = _param;
			this.Node_Id = _Node_Id;
			this.TypeInstance = _TypeInstance;
			this.Build ();
			
			ListUpDownStop = new Gtk.ListStore(typeof (string));
			ListNumberOfOutput = new Gtk.ListStore(typeof (string));
			ListNumberOfOutput.AppendValues("2");
			ListNumberOfOutput.AppendValues("3");
			ListTypeShutter = new Gtk.ListStore(typeof (string));		
			ListTypeShutter.AppendValues(param.ParamT("InstShutterType0"));
			ListTypeShutter.AppendValues(param.ParamT("InstShutterType1"));
			ListTypeShutter.AppendValues(param.ParamT("InstShutterType2"));
			InitWidget();
			
			//Permet de retourner l'id du projet
			foreach(Project pro in datamanagement.ListProject)
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
		
//########### Init Widget ######################################		
		
		//Fonction InitWidget
		//Fonction permettant d'initialiser le widget
		public void InitWidget()
		{
			UpdateComboBoxNumberOfInstance();
			if(TypeInstance == param.ParamP("InstLightingName"))
			{		
				AddNewInstance.Label = param.ParamT("IP_LabTVChild_ButtonNewInstance") + param.ParamT("ExTVNameLighting");
				DeleteInstance.Label = param.ParamT("IP_LabTVChild_ButtonDeleteInstance") + param.ParamT("ExTVNameLighting");
				LabelChildTreeView.Text = param.ParamT("IP_LabTVChild_Name_InstanceProperties");
				NoteLabel.Text = param.ParamT("IP_NoteLabel");	
				InitChildTreeView_Lighing();
				UpdateChildTreeView_Lighting();				
			}
			else if(TypeInstance == param.ParamP("InstSwitchName"))
			{
				AddNewInstance.Label = param.ParamT("IP_LabTVChild_ButtonNewInstance") + param.ParamT("ExTVNameSwitch");
				DeleteInstance.Label = param.ParamT("IP_LabTVChild_ButtonDeleteInstance") + param.ParamT("ExTVNameSwitch");
				LabelChildTreeView.Text = param.ParamT("IP_LabTVChild_Name_InstanceProperties");
				NoteLabel.Text = param.ParamT("IP_NoteLabel");	
				InitChildTreeView_Switch();
				UpdateChildTreeView_Switch();				
			}
			else if(TypeInstance == param.ParamP("InstShutterName"))
			{
				AddNewInstance.Label = param.ParamT("IP_LabTVChild_ButtonNewInstance") + param.ParamT("ExTVNameShutter");
				DeleteInstance.Label = param.ParamT("IP_LabTVChild_ButtonDeleteInstance") + param.ParamT("ExTVNameShutter");
				LabelChildTreeView.Text = param.ParamT("IP_LabTVChild_Name_InstanceProperties");
				NoteLabel.Text = param.ParamT("IP_NoteLabel");	
				InitChildTreeView_Shutter();
				UpdateChildTreeView_Shutter();					
			}
		}
		
//########### Lighting #########################################		
		
		//Fonction InitChildTreeView_Lighing
		//Fcontion permettant d'initialiser le treeview cbild lorsque le chois se fait pour lighting
		public void InitChildTreeView_Lighing()
		{
			//Visibilité des lignes et des colonnes
			ChildTreeView.EnableGridLines = TreeViewGridLines.Both;
			ChildTreeView.RubberBanding = true;
			
			//Ajout des colonnes
			Child_Lighting_NameColumn = new Gtk.TreeViewColumn();
			Child_Lighting_DefaultValueColumn = new Gtk.TreeViewColumn();
			Child_Lighting_FadeColumn = new Gtk.TreeViewColumn();
			Child_Lighting_IdColumn = new Gtk.TreeViewColumn();
			Child_Lighting_PathPinColumn = new Gtk.TreeViewColumn();	
			Child_Lighting_EmptyColumn = new Gtk.TreeViewColumn();
			
			//Colonne redimensionnable
			Child_Lighting_NameColumn.Resizable = true;
			Child_Lighting_DefaultValueColumn.Resizable = true;
			Child_Lighting_FadeColumn.Resizable = true;		
			Child_Lighting_IdColumn.Resizable = true;	
			Child_Lighting_PathPinColumn.Resizable = true;	
					
			//Nom des colonne
			Child_Lighting_NameColumn.Title = param.ParamT("IP_LIG_Name");
			Child_Lighting_DefaultValueColumn.Title = param.ParamT("IP_LIG_DefaultValue");
			Child_Lighting_FadeColumn.Title = param.ParamT("IP_LIG_Fade");
			Child_Lighting_PathPinColumn.Title = param.ParamT("IP_LIG_Path");
			Child_Lighting_IdColumn.Title = param.ParamT("IP_LIG_Id");
						
			//Visibilité des colonnes
			Child_Lighting_IdColumn.Visible = false;
			
			//Création des cellules
			Child_Lighting_NameCell = new Gtk.CellRendererText ();
			Child_Lighting_DefaultValueCell = new Gtk.CellRendererText ();
			Child_Lighting_FadeCell = new Gtk.CellRendererText ();
			Child_Lighting_PathPinCell = new Gtk.CellRendererText ();
			Child_Lighting_IdCell = new Gtk.CellRendererText ();	
			
			//On rend les cellules éditables
			Child_Lighting_NameCell.Editable = true;
			Child_Lighting_DefaultValueCell.Editable = true;
			Child_Lighting_FadeCell.Editable = true; 		
			Child_Lighting_IdCell.Editable = false;
			
			//Appel des fonction pour modification des cellules
			Child_Lighting_NameCell.Edited += Child_Lighting_NameCell_Edited;
			Child_Lighting_DefaultValueCell.Edited += Child_Lighting_DefaultValueCell_Edited;
			Child_Lighting_FadeCell.Edited += Child_Lighting_FadeCell_Edited;
			
			//On associe des cellules aux colonnes			
			Child_Lighting_NameColumn.PackStart (Child_Lighting_NameCell,true);
			Child_Lighting_DefaultValueColumn.PackStart (Child_Lighting_DefaultValueCell,true);
			Child_Lighting_FadeColumn.PackStart (Child_Lighting_FadeCell,true); 	
			Child_Lighting_PathPinColumn.PackStart (Child_Lighting_PathPinCell,true); 
			Child_Lighting_IdColumn.PackStart (Child_Lighting_IdCell,true);			
			
			//Ajout des colonnes dans ExplorerTreeView
			ChildTreeView.AppendColumn (Child_Lighting_NameColumn);
			ChildTreeView.AppendColumn (Child_Lighting_DefaultValueColumn);
			ChildTreeView.AppendColumn (Child_Lighting_FadeColumn);
			ChildTreeView.AppendColumn (Child_Lighting_PathPinColumn);
			ChildTreeView.AppendColumn (Child_Lighting_IdColumn);			
			ChildTreeView.AppendColumn (Child_Lighting_EmptyColumn);
			
			//Ajout des attibuts à chaque colonne
			Child_Lighting_NameColumn.AddAttribute(Child_Lighting_NameCell,"text",param.ParamI("IP_TVChild_Lighting_PositionName"));
			Child_Lighting_DefaultValueColumn.AddAttribute(Child_Lighting_DefaultValueCell,"text",param.ParamI("IP_TVChild_Lighting_PositionDefaultValue"));
			Child_Lighting_FadeColumn.AddAttribute(Child_Lighting_FadeCell,"text",param.ParamI("IP_TVChild_Lighting_PositionFade"));
			Child_Lighting_PathPinColumn.AddAttribute(Child_Lighting_PathPinCell,"text",param.ParamI("IP_TVChild_Lighting_PositionPath"));
			Child_Lighting_IdColumn.AddAttribute(Child_Lighting_IdCell,"text",param.ParamI("IP_TVChild_Lighting_PositionID"));
			
			//Création d'un nouveau store
			Child_Instance_ListStore = new Gtk.TreeStore(typeof(string),typeof(string),typeof(string),typeof(string),typeof(string));	
			
			//On écrit le store dans le treeview
			ChildTreeView.Model = Child_Instance_ListStore;
			ChildTreeView.ShowAll ();			
		}
		
		//Fonction UpdateChildTreeView_Lighting
		//Fonction permettant de mettre à jour le child treeview
		public void UpdateChildTreeView_Lighting()
		{
			string PathPin = "";

			Child_Instance_ListStore.Clear();
			foreach(Project Pro in datamanagement.ListProject)
			{	
				foreach(Node node in Pro.ReturnListNode())
				{
					if(node.Node_Id == Node_Id)
					{
						foreach(Instance ins in node.Instance_)
						{
							if(ins.Instance_Type == TypeInstance)
							{
								foreach(Project _Pro in datamanagement.ListProject)
								{	
									foreach(Node _node in _Pro.ReturnListNode())
									{	
										foreach(Network _net in _node.ReturnListNetwork())
										{
											foreach(Board _boa in _net.ReturnListBoard())
											{
												foreach(Pin _pin in _boa.Pin_)
												{
													if(ins.Pin_Id_0 == _pin.Pin_Id)
													{
														PathPin = _boa.Board_Name + "/" + _pin.Pin_Name; 
													}
												}
											}
										}
									}
								}
								Child_Instance_ListStore.AppendValues(ins.Instance_Name,ins.Instance_LIG_DefaultValue.ToString(),ins.Instance_LIG_Fade.ToString(),PathPin,ins.Instance_Id.ToString());
								PathPin = "";
							}
						}
					}
				}
			}
			ChildTreeView.Model = Child_Instance_ListStore;
			ChildTreeView.ShowAll();			
		}		
		
			//Fonction Child_Lighting_NameCell_Edited
			//Fcontion permettant de modifier le nom d'un instance
			private void Child_Lighting_NameCell_Edited (object o, Gtk.EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				string NameSelected = "";	//variable permettant de stocker le nom selectionné
		
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Lighting_PositionID")); //Nous retournons l'id du noeud selectionné
					NameSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Lighting_PositionName")); //Nous retournons le nom du noeud selectionné		
				}	
			
				if(NameSelected != args.NewText.Replace(" ","_")) //Permet de verifier si on fait un changment dans le nom
				{
					if(args.NewText != "") //Si le texte est pas vide
					{
						if(args.NewText.Length <= param.ParamI("CarSizeMax"))//Si la taille de la nouvelle chaine de caractère est infererieur au nombre prédéfinis
						{
							if(datamanagement.ModifyInstance(args.NewText,param.ParamI("MoIn_ChoiceName"),Convert.ToInt32(IdSelected)))
							{
								datamanagement.mainwindow.UpdateEplorerTreeView(); //On met à jour ExplorerTreeView
							}
						}
						else //Dans le cas ou le nouveuau nom est supérieur à 16 caratère
						{
							datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameInstanceTooLong"); //on affiche un message d'erreur
						}
					}
					else //Dans le cas ou le texte est vide
					{
						datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameInstanceEmpty"); //on affiche un message d'erreur
					}
				}				
			}
		
			//Fcontion Child_Lighting_DefaultValueCell_Edited
			//Fonction permettant de modifier la valeur par défaut d'une lumière
			private void Child_Lighting_DefaultValueCell_Edited (object o, Gtk.EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				bool CarValide = true;	
			
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Lighting_PositionID")); //Nous retournons l'id du noeud selectionné
		
				}	
			

				if(args.NewText != "") //Si le texte est pas vide
				{
					for(int i=0;i<=args.NewText.Length-1;i++)
					{
						if(args.NewText[i] < 48 || args.NewText[i] > 57)
						{
							CarValide = false;
						}
					}
					if(CarValide)
					{
						datamanagement.ModifyInstance(args.NewText,param.ParamI("MoIn_ChoiceLIGDefaultValue"),Convert.ToInt32(IdSelected));
						UpdateChildTreeView_Lighting(); //Mise à jour du child treeview
					}
					else 
					{
						datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"DataNotValid"); //on affiche un message d'erreur
					}
				}
				else //Dans le cas ou le texte est vide
				{
					datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"DataNotValid"); //on affiche un message d'erreur
				}				
			}
		
			//Fonction Child_Lighting_FadeCell_Edited
			//Fonction permettant de modifier le fade d'une lampe
			private void Child_Lighting_FadeCell_Edited (object o, Gtk.EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				bool CarValide = true;	
			
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Lighting_PositionID")); //Nous retournons l'id du noeud selectionné
		
				}	
			

				if(args.NewText != "") //Si le texte est pas vide
				{
					for(int i=0;i<=args.NewText.Length-1;i++)
					{
						if(args.NewText[i] < 48 || args.NewText[i] > 57)
						{
							CarValide = false;
						}
					}
					if(CarValide)
					{
						datamanagement.ModifyInstance(args.NewText,param.ParamI("MoIn_ChoiceLIGFade"),Convert.ToInt32(IdSelected));
						UpdateChildTreeView_Lighting(); //Mise à jour du child treeview
					}
					else 
					{
						datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"DataNotValid"); //on affiche un message d'erreur
					}
				}
				else //Dans le cas ou le texte est vide
				{
					datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"DataNotValid"); //on affiche un message d'erreur
				}				
			}		
		
//########### Switch #########################################	
		
		//Fonction InitChildTreeView_Switch
		//Fcontion permettant d'initialiser le treeview cbild lorsque le chois se fait pour switch
		public void InitChildTreeView_Switch()
		{
			//Visibilité des lignes et des colonnes
			ChildTreeView.EnableGridLines = TreeViewGridLines.Both;
			ChildTreeView.RubberBanding = true;
			
			//Ajout des colonnes
			Child_Switch_NameColumn = new Gtk.TreeViewColumn();
			Child_Switch_InverseColumn = new Gtk.TreeViewColumn();
			Child_Switch_ImpulsionTimeColumn = new Gtk.TreeViewColumn();
			Child_Switch_IdColumn = new Gtk.TreeViewColumn();
			Child_Switch_PathPinColumn = new Gtk.TreeViewColumn();	
			Child_Switch_EmptyColumn = new Gtk.TreeViewColumn();
			
			//Colonne redimensionnable
			Child_Switch_NameColumn.Resizable = true;
			Child_Switch_InverseColumn.Resizable = true;
			Child_Switch_ImpulsionTimeColumn.Resizable = true;		
			Child_Switch_IdColumn.Resizable = true;	
			Child_Switch_PathPinColumn.Resizable = true;	
					
			//Nom des colonne
			Child_Switch_NameColumn.Title = param.ParamT("IP_SWI_Name");
			Child_Switch_InverseColumn.Title = param.ParamT("IP_SWI_Inverse");
			Child_Switch_ImpulsionTimeColumn.Title = param.ParamT("IP_SWI_ImpulsionTime");
			Child_Switch_PathPinColumn.Title = param.ParamT("IP_SWI_Path");
			Child_Switch_IdColumn.Title = param.ParamT("IP_SWI_Id");
						
			//Visibilité des colonnes
			Child_Switch_IdColumn.Visible = false;
			
			//Création des cellules
			Child_Switch_NameCell = new Gtk.CellRendererText ();
			Child_Switch_InverseCell = new Gtk.CellRendererToggle ();
			Child_Switch_ImpulsionTimeCell = new Gtk.CellRendererText ();
			Child_Switch_PathPinCell = new Gtk.CellRendererText ();
			Child_Switch_IdCell = new Gtk.CellRendererText ();	
			
			//On rend les cellules éditables
			Child_Switch_NameCell.Editable = true;
			Child_Switch_InverseCell.Activatable = true;
			Child_Switch_ImpulsionTimeCell.Editable = true; 		
			Child_Switch_IdCell.Editable = false;
			
			//Appel des fonction pour modification des cellules
			Child_Switch_NameCell.Edited += Child_Switch_NameCell_Edited;
			Child_Switch_InverseCell.Toggled += Child_Switch_InverseCell_Toggled;
			Child_Switch_ImpulsionTimeCell.Edited += Child_Switch_ImpulsionTimeCell_Edited;
			
			//On associe des cellules aux colonnes			
			Child_Switch_NameColumn.PackStart (Child_Switch_NameCell,true);
			Child_Switch_InverseColumn.PackStart (Child_Switch_InverseCell,true);
			Child_Switch_ImpulsionTimeColumn.PackStart (Child_Switch_ImpulsionTimeCell,true); 	
			Child_Switch_PathPinColumn.PackStart (Child_Switch_PathPinCell,true); 
			Child_Switch_IdColumn.PackStart (Child_Switch_IdCell,true);			
			
			//Ajout des colonnes dans ExplorerTreeView
			ChildTreeView.AppendColumn (Child_Switch_NameColumn);
			ChildTreeView.AppendColumn (Child_Switch_InverseColumn);
			ChildTreeView.AppendColumn (Child_Switch_ImpulsionTimeColumn);
			ChildTreeView.AppendColumn (Child_Switch_PathPinColumn);
			ChildTreeView.AppendColumn (Child_Switch_IdColumn);			
			ChildTreeView.AppendColumn (Child_Switch_EmptyColumn);
			
			//Ajout des attibuts à chaque colonne
			Child_Switch_NameColumn.AddAttribute(Child_Switch_NameCell,"text",param.ParamI("IP_TVChild_Switch_PositionName"));
			Child_Switch_InverseColumn.AddAttribute(Child_Switch_InverseCell,"active",param.ParamI("IP_TVChild_Switch_PositionInverse"));
			Child_Switch_ImpulsionTimeColumn.AddAttribute(Child_Switch_ImpulsionTimeCell,"text",param.ParamI("IP_TVChild_Switch_PositionImpulsionTime"));
			Child_Switch_PathPinColumn.AddAttribute(Child_Switch_PathPinCell,"text",param.ParamI("IP_TVChild_Switch_PositionPath"));
			Child_Switch_IdColumn.AddAttribute(Child_Switch_IdCell,"text",param.ParamI("IP_TVChild_Switch_PositionID"));
			
			//Création d'un nouveau store
			Child_Instance_ListStore = new Gtk.TreeStore(typeof(string),typeof(bool),typeof(string),typeof(string),typeof(string));	
			
			//On écrit le store dans le treeview
			ChildTreeView.Model = Child_Instance_ListStore;
			ChildTreeView.ShowAll ();		
		}
		
		//Fonction UpdateChildTreeView_Board
		//Fonction permettant de mettre à jour le child treeview
		public void UpdateChildTreeView_Switch()
		{
			string PathPin = "";

			Child_Instance_ListStore.Clear();
			foreach(Project Pro in datamanagement.ListProject)
			{	
				foreach(Node node in Pro.ReturnListNode())
				{
					if(node.Node_Id == Node_Id)
					{
						foreach(Instance ins in node.Instance_)
						{
							if(ins.Instance_Type == TypeInstance)
							{
								foreach(Project _Pro in datamanagement.ListProject)
								{	
									foreach(Node _node in _Pro.ReturnListNode())
									{	
										foreach(Network _net in _node.ReturnListNetwork())
										{
											foreach(Board _boa in _net.ReturnListBoard())
											{
												foreach(Pin _pin in _boa.Pin_)
												{
													if(ins.Pin_Id_0 == _pin.Pin_Id)
													{
														PathPin = _boa.Board_Name + "/" + _pin.Pin_Name; 
													}
												}
											}
										}
									}
								}
								Child_Instance_ListStore.AppendValues(ins.Instance_Name,ins.Instance_SWI_Inverse,ins.Instance_SWI_ImpulsionTime.ToString(),PathPin,ins.Instance_Id.ToString());
								PathPin = "";
							}
						}
					}
				}
			}
			ChildTreeView.Model = Child_Instance_ListStore;
			ChildTreeView.ShowAll();			
		}		
		
			//Fonction Child_Switch_NameCell_Edited
			//Fcontion permettant de modifier le nom d'une instance
			private void Child_Switch_NameCell_Edited (object o, Gtk.EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				string NameSelected = "";	//variable permettant de stocker le nom selectionné
		
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Switch_PositionID")); //Nous retournons l'id du noeud selectionné
					NameSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Switch_PositionName")); //Nous retournons le nom du noeud selectionné		
				}	
			
				if(NameSelected != args.NewText.Replace(" ","_")) //Permet de verifier si on fait un changment dans le nom
				{
					if(args.NewText != "") //Si le texte est pas vide
					{
						if(args.NewText.Length <= param.ParamI("CarSizeMax"))//Si la taille de la nouvelle chaine de caractère est infererieur au nombre prédéfinis
						{
							if(datamanagement.ModifyInstance(args.NewText,param.ParamI("MoIn_ChoiceName"),Convert.ToInt32(IdSelected)))
							{
								datamanagement.mainwindow.UpdateEplorerTreeView(); //On met à jour ExplorerTreeView
							}
						}
						else //Dans le cas ou le nouveuau nom est supérieur à 16 caratère
						{
							datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameInstanceTooLong"); //on affiche un message d'erreur
						}
					}
					else //Dans le cas ou le texte est vide
					{
						datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameInstanceEmpty"); //on affiche un message d'erreur
					}
				}				
			}	
		
			//Fontion Child_Switch_InverseCell_Toggled
			//Fonction faire lors d'un clic sur le check button
			private void Child_Switch_InverseCell_Toggled (object o, Gtk.ToggledArgs args)
			{
				bool Value = false; //Initialisation d'un bool permettant de récuperer la valeur lors d'un clic
				Gtk.TreeIter iter;
				if (Child_Instance_ListStore.GetIter (out iter, new Gtk.TreePath(args.Path))) {
					Value = (bool) Child_Instance_ListStore.GetValue(iter,param.ParamI("IP_TVChild_Switch_PositionInverse")); //on fait l'acquisition de la valeur
					Child_Instance_ListStore.SetValue(iter,param.ParamI("IP_TVChild_Switch_PositionInverse"),!Value);//On affiche la valeur inversé
				}				
			}
		
			//Fonction Child_Switch_ImpulsionTimeCell_Edited
			//Fonction permettant de modififer l'impulsion time d'un shutter
			private void Child_Switch_ImpulsionTimeCell_Edited (object o, Gtk.EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				bool CarValide = true;	
		
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Switch_PositionID")); //Nous retournons l'id du noeud selectionné
				}	
			
				if(args.NewText != "") //Si le texte est pas vide
				{
					for(int i=0;i<=args.NewText.Length-1;i++)
					{
						if(args.NewText[i] < 48 || args.NewText[i] > 57)
						{
							CarValide = false;
						}
					}
					if(CarValide)
					{
						datamanagement.ModifyInstance(args.NewText,param.ParamI("MoIn_ChoiceSWIImpulsionTime"),Convert.ToInt32(IdSelected));
						UpdateChildTreeView_Switch(); //Mise à jour du child treeview
					}
					else 
					{
						datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"DataNotValid"); //on affiche un message d'erreur
					}
				}
				else //Dans le cas ou le texte est vide
				{
					datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"DataNotValid"); //on affiche un message d'erreur
				}						
			}
		
//########### Shutter ########################################	
		
		//Fonction InitChildTreeView_Shutter
		//Fonction permettant d'initialiser le treeview pour un volet
		public void InitChildTreeView_Shutter()
		{
			//Visibilité des lignes et des colonnes
			ChildTreeView.EnableGridLines = TreeViewGridLines.Both;
			ChildTreeView.RubberBanding = true;
			
			//Ajout des colonnes
			Child_Shutter_NameColumn = new Gtk.TreeViewColumn();
			Child_Shutter_NumberOfOutputColumn = new Gtk.TreeViewColumn();
			Child_Shutter_ShutterTypeColumn = new Gtk.TreeViewColumn();
			Child_Shutter_ShutterTimeColumn = new Gtk.TreeViewColumn();
			Child_Shutter_ShutterInitTimeColumn = new Gtk.TreeViewColumn();
			Child_Shutter_PathPin1Column = new Gtk.TreeViewColumn();
			Child_Shutter_PathPin2Column = new Gtk.TreeViewColumn();
			Child_Shutter_PathPin3Column = new Gtk.TreeViewColumn();
			Child_Shutter_AppearenceOrderColumn = new Gtk.TreeViewColumn();
			Child_Shutter_IdColumn = new Gtk.TreeViewColumn();
			Child_Shutter_EmptyColumn = new Gtk.TreeViewColumn();
			
			//Colonne redimensionnable		
			Child_Shutter_NameColumn.Resizable = true;
			Child_Shutter_NumberOfOutputColumn.Resizable = true;
			Child_Shutter_ShutterTypeColumn.Resizable = true;
			Child_Shutter_ShutterTimeColumn.Resizable = true;
			Child_Shutter_ShutterInitTimeColumn.Resizable = true;
			Child_Shutter_PathPin1Column.Resizable = true;
			Child_Shutter_PathPin2Column.Resizable = true;
			Child_Shutter_PathPin3Column.Resizable = true;
			Child_Shutter_AppearenceOrderColumn.Resizable = true;
			Child_Shutter_IdColumn.Resizable = true;
			
			//Nom des colonnes
			Child_Shutter_NameColumn.Title = param.ParamT("IP_SHU_Name");
			Child_Shutter_NumberOfOutputColumn.Title = param.ParamT("IP_SHU_NumberOfOutput");
			Child_Shutter_ShutterTypeColumn.Title = param.ParamT("IP_SHU_Type");
			Child_Shutter_ShutterTimeColumn.Title = param.ParamT("IP_SHU_Time");
			Child_Shutter_ShutterInitTimeColumn.Title = param.ParamT("IP_SHU_InitTime");
			Child_Shutter_PathPin1Column.Title = param.ParamT("IP_SHU_PathUp");
			Child_Shutter_PathPin2Column.Title = param.ParamT("IP_SHU_PathDown");
			Child_Shutter_PathPin3Column.Title = param.ParamT("IP_SHU_PathStop");
			Child_Shutter_AppearenceOrderColumn.Title = param.ParamT("IP_SHU_AppearenceOrder");
			Child_Shutter_IdColumn.Title = param.ParamT("IP_SHU_Id");

			//Visibilité des colonnes
			Child_Shutter_IdColumn.Visible = false;		
			
			//Création des cellule
			Child_Shutter_NameCell = new CellRendererText();
			Child_Shutter_NumberOfOutputCell = new CellRendererCombo();
			Child_Shutter_ShutterTypeCell = new CellRendererCombo();
			Child_Shutter_ShutterTimeCell = new CellRendererText();
			Child_Shutter_ShutterInitTimeCell = new CellRendererText();
			Child_Shutter_PathPin1Cell = new CellRendererText();
			Child_Shutter_PathPin2Cell = new CellRendererText();
			Child_Shutter_PathPin3Cell = new CellRendererText();
			Child_Shutter_AppearenceOrderCell = new CellRendererCombo();
			Child_Shutter_IdCell = new CellRendererText();
			
			//Cellule Editable
			Child_Shutter_NameCell.Editable = true;
			Child_Shutter_NumberOfOutputCell.Editable = true;
			Child_Shutter_ShutterTypeCell.Editable = true;
			Child_Shutter_ShutterTimeCell.Editable = true;	
			Child_Shutter_ShutterInitTimeCell.Editable = true;	
			Child_Shutter_AppearenceOrderCell.Editable = true;
			
			Child_Shutter_AppearenceOrderCell.Model = ListUpDownStop;
			Child_Shutter_AppearenceOrderCell.TextColumn = 0;
			
			Child_Shutter_NumberOfOutputCell.Model = ListNumberOfOutput;
			Child_Shutter_NumberOfOutputCell.TextColumn = 0;	
			
			Child_Shutter_ShutterTypeCell.Model = ListTypeShutter;
			Child_Shutter_ShutterTypeCell.TextColumn = 0;				
			
			//Appel des fonction pour la modification des cellules
			Child_Shutter_NameCell.Edited += Child_Shutter_NameCell_Edited;
			Child_Shutter_NumberOfOutputCell.Edited += Child_Shutter_NumberOfOutputCell_Edited;
			Child_Shutter_ShutterTypeCell.Edited += Child_Shutter_ShutterTypeCell_Edited;
			Child_Shutter_ShutterTimeCell.Edited += Child_Shutter_ShutterTimeCell_Edited;
			Child_Shutter_ShutterInitTimeCell.Edited += Child_Shutter_ShutterInitTimeCell_Edited;
			Child_Shutter_AppearenceOrderCell.Edited += Child_Shutter_AppearenceOrderCell_Edited;
			
			//On associe des cellules aux colonnes				
			Child_Shutter_NameColumn.PackStart(Child_Shutter_NameCell,true);
			Child_Shutter_NumberOfOutputColumn.PackStart(Child_Shutter_NumberOfOutputCell,true);
			Child_Shutter_ShutterTypeColumn.PackStart(Child_Shutter_ShutterTypeCell,true);
			Child_Shutter_ShutterTimeColumn.PackStart(Child_Shutter_ShutterTimeCell,true);
			Child_Shutter_ShutterInitTimeColumn.PackStart(Child_Shutter_ShutterInitTimeCell,true);
			Child_Shutter_PathPin1Column.PackStart(Child_Shutter_PathPin1Cell,true);
			Child_Shutter_PathPin2Column.PackStart(Child_Shutter_PathPin2Cell,true);
			Child_Shutter_PathPin3Column.PackStart(Child_Shutter_PathPin3Cell,true);
			Child_Shutter_AppearenceOrderColumn.PackStart(Child_Shutter_AppearenceOrderCell,true);
			Child_Shutter_IdColumn.PackStart(Child_Shutter_IdCell,true);
			
			//Ajout des colonnes dans ExplorerTreeView
			ChildTreeView.AppendColumn (Child_Shutter_NameColumn);
			ChildTreeView.AppendColumn (Child_Shutter_NumberOfOutputColumn);
			ChildTreeView.AppendColumn (Child_Shutter_ShutterTypeColumn);
			ChildTreeView.AppendColumn (Child_Shutter_ShutterTimeColumn);
			ChildTreeView.AppendColumn (Child_Shutter_ShutterInitTimeColumn);
			ChildTreeView.AppendColumn (Child_Shutter_PathPin1Column);			
			ChildTreeView.AppendColumn (Child_Shutter_PathPin2Column);		
			ChildTreeView.AppendColumn (Child_Shutter_PathPin3Column);
			ChildTreeView.AppendColumn (Child_Shutter_AppearenceOrderColumn);
			ChildTreeView.AppendColumn (Child_Shutter_IdColumn);
			ChildTreeView.AppendColumn (Child_Shutter_EmptyColumn);
			
			//Ajout des attibuts à chaque colonne
			Child_Shutter_NameColumn.AddAttribute(Child_Shutter_NameCell,"text",param.ParamI("IP_TVChild_Shutter_PositionName"));
			Child_Shutter_NumberOfOutputColumn.AddAttribute(Child_Shutter_NumberOfOutputCell,"text",param.ParamI("IP_TVChild_Shutter_PositionNumberOfOutput"));
			Child_Shutter_ShutterTypeColumn.AddAttribute(Child_Shutter_ShutterTypeCell,"text",param.ParamI("IP_TVChild_Shutter_PositionType"));
			Child_Shutter_ShutterTimeColumn.AddAttribute(Child_Shutter_ShutterTimeCell,"text",param.ParamI("IP_TVChild_Shutter_PositionTime"));
			Child_Shutter_ShutterInitTimeColumn.AddAttribute(Child_Shutter_ShutterInitTimeCell,"text",param.ParamI("IP_TVChild_Shutter_InitPositionTime"));
			Child_Shutter_PathPin1Column.AddAttribute(Child_Shutter_PathPin1Cell,"text",param.ParamI("IP_TVChild_Shutter_PositionPath1"));
			Child_Shutter_PathPin2Column.AddAttribute(Child_Shutter_PathPin2Cell,"text",param.ParamI("IP_TVChild_Shutter_PositionPath2"));
			Child_Shutter_PathPin3Column.AddAttribute(Child_Shutter_PathPin3Cell,"text",param.ParamI("IP_TVChild_Shutter_PositionPath3"));
			Child_Shutter_AppearenceOrderColumn.AddAttribute(Child_Shutter_AppearenceOrderCell,"text",param.ParamI("IP_TVChild_Shutter_PositionAppearenceOrder"));
			Child_Shutter_IdColumn.AddAttribute(Child_Shutter_IdCell,"text",param.ParamI("IP_TVChild_Shutter_PositionID"));
			
			//Création d'un nouveau store
			Child_Instance_ListStore = new Gtk.TreeStore(typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string));	
			
			//On écrit le store dans le treeview
			ChildTreeView.Model = Child_Instance_ListStore;
			ChildTreeView.ShowAll ();					
		}
		
		//Fonction UpdateChildTreeView_Switch
		//Fonction permettant de mettre à jour le treeview
		public void UpdateChildTreeView_Shutter()
		{
			string PathPinUp = "";
			string PathPinDown = "";
			string PathPinStop = "";
			string UpDownStop = "";
			string TypeShutter = "";

			Child_Instance_ListStore.Clear();
			foreach(Project Pro in datamanagement.ListProject)
			{	
				foreach(Node node in Pro.ReturnListNode())
				{
					if(node.Node_Id == Node_Id)
					{
						foreach(Instance ins in node.Instance_)
						{
							if(ins.Instance_Type == TypeInstance)
							{
								foreach(Project _Pro in datamanagement.ListProject)
								{	
									foreach(Node _node in _Pro.ReturnListNode())
									{	
										foreach(Network _net in _node.ReturnListNetwork())
										{
											foreach(Board _boa in _net.ReturnListBoard())
											{
												foreach(Pin _pin in _boa.Pin_)
												{
													if(ins.Pin_Id_0 == _pin.Pin_Id)
													{
														PathPinUp = _boa.Board_Name + "/" + _pin.Pin_Name; 
													}
													else if(ins.Pin_Id_1 == _pin.Pin_Id)
													{
														PathPinDown = _boa.Board_Name + "/" + _pin.Pin_Name; 
													}
													else if(ins.Pin_Id_2 == _pin.Pin_Id && ins.Instance_SHU_NumberOfOutput == 3)
													{
														PathPinStop = _boa.Board_Name + "/" + _pin.Pin_Name; 
													}	
													else if(ins.Instance_SHU_NumberOfOutput == 2)
													{
														PathPinStop = param.ParamT("IP_SHU_NotUsed");
													}
												}
											}
										}
									}
								}
								if(ins.Instance_SHU_NumberOfOutput == 2)
								{
									if(ins.Instance_Up_Down_Stop == 0)
									{
										UpDownStop = param.ParamT("IP_SHU_2_0");
									}
									else
									{
										UpDownStop = param.ParamT("IP_SHU_2_1");
									}
								}
								else
								{
									if(ins.Instance_Up_Down_Stop == 0)
									{
										UpDownStop = param.ParamT("IP_SHU_3_0");
									}
									else if(ins.Instance_Up_Down_Stop == 1)
									{
										UpDownStop = param.ParamT("IP_SHU_3_1");
									}
									else if(ins.Instance_Up_Down_Stop == 2)
									{
										UpDownStop = param.ParamT("IP_SHU_3_2");
									}
									else if(ins.Instance_Up_Down_Stop == 3)
									{
										UpDownStop = param.ParamT("IP_SHU_3_3");
									}
									else if(ins.Instance_Up_Down_Stop == 4)
									{
										UpDownStop = param.ParamT("IP_SHU_3_4");
									}
									else if(ins.Instance_Up_Down_Stop == 5)
									{
										UpDownStop = param.ParamT("IP_SHU_3_5");
									}									
								}
								
								if(ins.Instance_SHU_Type == 0)
								{
									TypeShutter = param.ParamT("InstShutterType0");
								}
								else if(ins.Instance_SHU_Type == 1)
								{
									TypeShutter = param.ParamT("InstShutterType1");
								}
								else if(ins.Instance_SHU_Type == 2)
								{
									TypeShutter = param.ParamT("InstShutterType2");
								}								
								Child_Instance_ListStore.AppendValues(ins.Instance_Name,ins.Instance_SHU_NumberOfOutput.ToString(),TypeShutter,ins.Instance_SHU_Time.ToString(),ins.Instance_SHU_InitTime.ToString(),PathPinUp,PathPinDown,PathPinStop,UpDownStop,ins.Instance_Id.ToString());
								PathPinUp = "";
								PathPinDown = "";
								PathPinStop = "";
								UpDownStop = "";
								TypeShutter = "";
							}
						}
					}
				}
			}
			ChildTreeView.Model = Child_Instance_ListStore;
			ChildTreeView.ShowAll();			
		}
		
			//Fonction Child_Shutter_NameCell_Edited
			//Fcontion permettant de modifier le nom d'une instance
			private void Child_Shutter_NameCell_Edited (object o, Gtk.EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				string NameSelected = "";	//variable permettant de stocker le nom selectionné
		
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Shutter_PositionID")); //Nous retournons l'id du noeud selectionné
					NameSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Shutter_PositionName")); //Nous retournons le nom du noeud selectionné		
				}	
			
				if(NameSelected != args.NewText.Replace(" ","_")) //Permet de verifier si on fait un changment dans le nom
				{
					if(args.NewText != "") //Si le texte est pas vide
					{
						if(args.NewText.Length <= param.ParamI("CarSizeMax"))//Si la taille de la nouvelle chaine de caractère est infererieur au nombre prédéfinis
						{
							if(datamanagement.ModifyInstance(args.NewText,param.ParamI("MoIn_ChoiceName"),Convert.ToInt32(IdSelected)))
							{
								datamanagement.mainwindow.UpdateEplorerTreeView(); //On met à jour ExplorerTreeView
							}
						}
						else //Dans le cas ou le nouveuau nom est supérieur à 16 caratère
						{
							datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameInstanceTooLong"); //on affiche un message d'erreur
						}
					}
					else //Dans le cas ou le texte est vide
					{
						datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameInstanceEmpty"); //on affiche un message d'erreur
					}
				}				
			}			
		
			//Fonction Child_Shutter_AppearenceOrderCell_Edited
			//Fonction permettant de modifier la cellule d'ordre d'apparence
			private void Child_Shutter_AppearenceOrderCell_Edited(object o, Gtk.EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné	
		
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Shutter_PositionID")); //Nous retournons l'id du noeud selectionné
				}			
			
				if(args.NewText == param.ParamT("IP_SHU_2_0") || args.NewText == param.ParamT("IP_SHU_3_0"))
				{
					datamanagement.ModifyInstance("0",param.ParamI("MoIn_ChoiceSHUAppearenceOrder"),Convert.ToInt32(IdSelected));
				}
				else if(args.NewText == param.ParamT("IP_SHU_2_1") || args.NewText == param.ParamT("IP_SHU_3_1"))
				{
					datamanagement.ModifyInstance("1",param.ParamI("MoIn_ChoiceSHUAppearenceOrder"),Convert.ToInt32(IdSelected));
				}
				else if(args.NewText == param.ParamT("IP_SHU_3_2"))
				{
					datamanagement.ModifyInstance("2",param.ParamI("MoIn_ChoiceSHUAppearenceOrder"),Convert.ToInt32(IdSelected));
				}
				else if(args.NewText == param.ParamT("IP_SHU_3_3"))
				{
					datamanagement.ModifyInstance("3",param.ParamI("MoIn_ChoiceSHUAppearenceOrder"),Convert.ToInt32(IdSelected));
				}
				else if(args.NewText == param.ParamT("IP_SHU_3_4"))
				{
					datamanagement.ModifyInstance("4",param.ParamI("MoIn_ChoiceSHUAppearenceOrder"),Convert.ToInt32(IdSelected));
				}
				else if(args.NewText == param.ParamT("IP_SHU_3_5"))
				{
					datamanagement.ModifyInstance("5",param.ParamI("MoIn_ChoiceSHUAppearenceOrder"),Convert.ToInt32(IdSelected));
				}	
			}
		
			//Fonction Child_Shutter_NumberOfOutputCell_Edited
			//Fonction permettant de modifier le nombre do sortie de l'arbre
			private void Child_Shutter_NumberOfOutputCell_Edited(object o, Gtk.EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné	
				string NumberofOutputSelected = ""; //variable permettant de stocker le nombre de sortie
		
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Shutter_PositionID"));
					NumberofOutputSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Shutter_PositionNumberOfOutput"));
				}	
			
				//Dans le cas ou le nouveau texte est différent de l'ancien
				if(NumberofOutputSelected != args.NewText)
				{
					foreach(Project Pro in datamanagement.ListProject) //Dans la liste des projets
					{
						foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
						{
							foreach(Network net in node.ReturnListNetwork()) //Dans la liste des réseau de chaque noeud
							{
								foreach(Board boa in net.ReturnListBoard()) //Dans la liste des carte de chaque réseau
								{
									foreach(Pin pin in boa.ReturnListPin()) //Dans la liste des pin de chaque carte
									{
										if(pin.Instance_Id == Convert.ToInt32(IdSelected)) //Si l'id de la broche est celle que nous avons rentré en paramètre
										{
											pin.Instance_Id = 0; //On vient remettre à 0 l'instance associé à une broche
										}
									}
								}
							}
							foreach(Instance ins in node.ReturnListInstance())
							{
								if(ins.Instance_Id == Convert.ToInt32(IdSelected))
								{
									ins.Instance_Up_Down_Stop = 0; //nous remettons aussi l'ordre d'apparition à 0
								}
							}
						}
					}
					datamanagement.ModifyInstance(args.NewText,param.ParamI("MoIn_ChoiceSHUNumberOfOutput"),Convert.ToInt32(IdSelected)); //Enfin nous modifions le nombre de sortie du volet
				}
			}
		
			//Fonction Child_Shutter_ShutterTimeCell_Edited
			//Fonction permettant de modifier le temps de parcours du volet
			private void Child_Shutter_ShutterTimeCell_Edited(object o, Gtk.EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				bool CarValide = true;	
		
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Shutter_PositionID")); //Nous retournons l'id du noeud selectionné
				}	
			
				if(args.NewText != "") //Si le texte est pas vide
				{
					for(int i=0;i<=args.NewText.Length-1;i++)
					{
						if(args.NewText[i] < 48 || args.NewText[i] > 57)
						{
							CarValide = false;
						}
					}
					if(CarValide)
					{
						datamanagement.ModifyInstance(args.NewText,param.ParamI("MoIn_ChoiceSHUTravelTime"),Convert.ToInt32(IdSelected));
						UpdateChildTreeView_Shutter(); //Mise à jour du child treeview
					}
					else 
					{
						datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"DataNotValid"); //on affiche un message d'erreur
					}
				}
				else //Dans le cas ou le texte est vide
				{
					datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"DataNotValid"); //on affiche un message d'erreur
				}					
			}
		
			//Fonction Child_Shutter_ShutterInitTimeCell_Edited
			//Fonction permettant de modifier le temps de parcours du volet
			private void Child_Shutter_ShutterInitTimeCell_Edited(object o, Gtk.EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				bool CarValide = true;	
		
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Shutter_PositionID")); //Nous retournons l'id du noeud selectionné
				}	
			
				if(args.NewText != "") //Si le texte est pas vide
				{
					for(int i=0;i<=args.NewText.Length-1;i++)
					{
						if(args.NewText[i] < 48 || args.NewText[i] > 57)
						{
							CarValide = false;
						}
					}
					if(CarValide)
					{
						datamanagement.ModifyInstance(args.NewText,param.ParamI("MoIn_ChoiceSHUInitTime"),Convert.ToInt32(IdSelected));
						UpdateChildTreeView_Shutter(); //Mise à jour du child treeview
					}
					else 
					{
						datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"DataNotValid"); //on affiche un message d'erreur
					}
				}
				else //Dans le cas ou le texte est vide
				{
					datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"DataNotValid"); //on affiche un message d'erreur
				}					
			}		
		
			//Fonction Child_Shutter_ShutterTypeCell_Edited
			//Fonction permettant de modifier le type d'un volet
			private void Child_Shutter_ShutterTypeCell_Edited(object o, Gtk.EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné	
		
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Shutter_PositionID"));
				}	
			
				if(args.NewText == param.ParamT("InstShutterType0"))
				{
					datamanagement.ModifyInstance("0",param.ParamI("MoIn_ChoiceSHUShutterType"),Convert.ToInt32(IdSelected));
					UpdateChildTreeView_Shutter(); //Mise à jour du child treeview				
				}
				else if(args.NewText == param.ParamT("InstShutterType1"))
				{
					datamanagement.ModifyInstance("1",param.ParamI("MoIn_ChoiceSHUShutterType"),Convert.ToInt32(IdSelected));
					UpdateChildTreeView_Shutter(); //Mise à jour du child treeview					
				}
				else if(args.NewText == param.ParamT("InstShutterType2"))
				{
					datamanagement.ModifyInstance("2",param.ParamI("MoIn_ChoiceSHUShutterType"),Convert.ToInt32(IdSelected));
					UpdateChildTreeView_Shutter(); //Mise à jour du child treeview					
				}			
			}
		
//###################### Update Combobox  ####################################		
		
		//Fonction UpdateComboBoxNumberOfBoard
		//Fonction permettant de mettre à jour la combobox pour afficher le nombre de carte que nous voulons créer
		public void UpdateComboBoxNumberOfInstance()
		{
			for(int i = 1;i<21;i++)
			{
				ComboboxNumberOfInstance.AppendText(i.ToString());
			}			
			Gtk.TreeIter iter;
			ComboboxNumberOfInstance.Model.IterNthChild(out iter,0);
			ComboboxNumberOfInstance.SetActiveIter(iter);				
		}		
		
//########### Autre ##########################################
		
		//Fonction OnChildTreeViewButtonReleaseEvent
		//Fonction permettant d'effectuer des actions en fonction de modes
		protected void OnChildTreeViewButtonReleaseEvent (object o, Gtk.ButtonReleaseEventArgs args)
		{	
			if(TypeInstance == param.ParamP("InstLightingName"))
			{
				string IdSelected = "";	//variable permettant de stocker l'id de l'instance sélectionné
		
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Lighting_PositionID")); //Nous retournons l'id de l'instance		
				}		
				
				if(IdSelected != "")
				{
					foreach(Project pro in datamanagement.ListProject)
					{
						foreach(Node nod in pro.ReturnListNode())
						{
							foreach(Instance ins in nod.ReturnListInstance())
							{
								if(ins.Instance_Id == Convert.ToInt32(IdSelected))
								{
									TextViewNote.Buffer.Text = ins.Instance_Note;
								}
							}
						}
					}
				}
			}
			else if(TypeInstance == param.ParamP("InstSwitchName"))
			{
				string IdSelected = "";	//variable permettant de stocker l'id de l'instance sélectionné
				bool CheckButtonValueSelected = false;
				
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Switch_PositionID")); //Nous retournons l'id de l'instance		
					CheckButtonValueSelected = (bool) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Switch_PositionInverse"));
				}		
				
				if(IdSelected != "")
				{
					datamanagement.ModifyInstance(CheckButtonValueSelected.ToString(),param.ParamI("MoIn_ChoiceSWIInverse"),Convert.ToInt32(IdSelected));
				}
				//Mise à jour du TextViewNote lors d'un clic sur l'instance
				if(IdSelected != "")
				{
					foreach(Project pro in datamanagement.ListProject)
					{
						foreach(Node nod in pro.ReturnListNode())
						{
							foreach(Instance ins in nod.ReturnListInstance())
							{
								if(ins.Instance_Id == Convert.ToInt32(IdSelected))
								{
									TextViewNote.Buffer.Text = ins.Instance_Note;
								}
							}
						}
					}
				}			
			}
			else if(TypeInstance == param.ParamP("InstShutterName"))
			{
				ListUpDownStop.Clear();		
				
				string IdSelected = "";	//variable permettant de stocker l'id de l'instance sélectionné
		
				TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Shutter_PositionID")); //Nous retournons l'id de l'instance		
				}		
				
				if(IdSelected != "")
				{
					foreach(Project pro in datamanagement.ListProject)
					{
						foreach(Node nod in pro.ReturnListNode())
						{
							foreach(Instance ins in nod.ReturnListInstance())
							{
								if(ins.Instance_Id == Convert.ToInt32(IdSelected))
								{
									TextViewNote.Buffer.Text = ins.Instance_Note;
									if(ins.Instance_SHU_NumberOfOutput == 2)
									{
										ListUpDownStop.AppendValues(param.ParamT("IP_SHU_2_0"));
										ListUpDownStop.AppendValues(param.ParamT("IP_SHU_2_1"));
									}
									else
									{
										ListUpDownStop.AppendValues(param.ParamT("IP_SHU_3_0"));
										ListUpDownStop.AppendValues(param.ParamT("IP_SHU_3_1"));
										ListUpDownStop.AppendValues(param.ParamT("IP_SHU_3_2"));
										ListUpDownStop.AppendValues(param.ParamT("IP_SHU_3_3"));
										ListUpDownStop.AppendValues(param.ParamT("IP_SHU_3_4"));
										ListUpDownStop.AppendValues(param.ParamT("IP_SHU_3_5"));
									}
								}
							}
						}
					}
				}				
			}			
		}
		
		
		//Fonction UpdateWidget
		//Fcontion permettant de mettre à jour le widget		
		public void UpdateWidget()
		{
			if(TypeInstance == param.ParamP("InstLightingName"))
			{
				AddNewInstance.Label = param.ParamT("IP_LabTVChild_ButtonNewInstance") + param.ParamT("ExTVNameLighting");
				DeleteInstance.Label = param.ParamT("IP_LabTVChild_ButtonDeleteInstance") + param.ParamT("ExTVNameLighting");
				LabelChildTreeView.Text = param.ParamT("IP_LabTVChild_Name_InstanceProperties");
				NoteLabel.Text = param.ParamT("IP_NoteLabel");

				//Nom des colonne
				Child_Lighting_NameColumn.Title = param.ParamT("IP_LIG_Name");
				Child_Lighting_DefaultValueColumn.Title = param.ParamT("IP_LIG_DefaultValue");
				Child_Lighting_FadeColumn.Title = param.ParamT("IP_LIG_Fade");
				Child_Lighting_PathPinColumn.Title = param.ParamT("IP_LIG_Path");
				Child_Lighting_IdColumn.Title = param.ParamT("IP_LIG_Id");
				
				UpdateChildTreeView_Lighting();		
				TextViewNote.Buffer.Text = "";
			}
			else if(TypeInstance == param.ParamP("InstSwitchName"))
			{
				AddNewInstance.Label = param.ParamT("IP_LabTVChild_ButtonNewInstance") + param.ParamT("ExTVNameSwitch");
				DeleteInstance.Label = param.ParamT("IP_LabTVChild_ButtonDeleteInstance") + param.ParamT("ExTVNameSwitch");
				LabelChildTreeView.Text = param.ParamT("IP_LabTVChild_Name_InstanceProperties");
				NoteLabel.Text = param.ParamT("IP_NoteLabel");	
				
				//Nom des colonne
				Child_Switch_NameColumn.Title = param.ParamT("IP_SWI_Name");
				Child_Switch_InverseColumn.Title = param.ParamT("IP_SWI_Inverse");
				Child_Switch_ImpulsionTimeColumn.Title = param.ParamT("IP_SWI_ImpulsionTime");
				Child_Switch_PathPinColumn.Title = param.ParamT("IP_SWI_Path");
				Child_Switch_IdColumn.Title = param.ParamT("IP_SWI_Id");				
				
				UpdateChildTreeView_Switch();		
				TextViewNote.Buffer.Text = "";			
			}
			else if(TypeInstance == param.ParamP("InstShutterName"))
			{
				AddNewInstance.Label = param.ParamT("IP_LabTVChild_ButtonNewInstance") + param.ParamT("ExTVNameShutter");
				DeleteInstance.Label = param.ParamT("IP_LabTVChild_ButtonDeleteInstance") + param.ParamT("ExTVNameShutter");
				LabelChildTreeView.Text = param.ParamT("IP_LabTVChild_Name_InstanceProperties");
				NoteLabel.Text = param.ParamT("IP_NoteLabel");
				
				ListTypeShutter.Clear();
				ListTypeShutter.AppendValues(param.ParamT("InstShutterType0"));
				ListTypeShutter.AppendValues(param.ParamT("InstShutterType1"));
				ListTypeShutter.AppendValues(param.ParamT("InstShutterType2"));				
				
				//Nom des colonnes
				Child_Shutter_NameColumn.Title = param.ParamT("IP_SHU_Name");
				Child_Shutter_NumberOfOutputColumn.Title = param.ParamT("IP_SHU_NumberOfOutput");
				Child_Shutter_ShutterTypeColumn.Title = param.ParamT("IP_SHU_Type");
				Child_Shutter_ShutterTimeColumn.Title = param.ParamT("IP_SHU_Time");
				Child_Shutter_ShutterInitTimeColumn.Title = param.ParamT("IP_SHU_InitTime");				
				Child_Shutter_PathPin1Column.Title = param.ParamT("IP_SHU_PathUp");
				Child_Shutter_PathPin2Column.Title = param.ParamT("IP_SHU_PathDown");
				Child_Shutter_PathPin3Column.Title = param.ParamT("IP_SHU_PathStop");
				Child_Shutter_AppearenceOrderColumn.Title = param.ParamT("IP_SHU_AppearenceOrder");
				Child_Shutter_IdColumn.Title = param.ParamT("IP_SHU_Id");			
				
				UpdateChildTreeView_Shutter();
				TextViewNote.Buffer.Text = "";	
				
			}
		}
		
		//Fonction WidgetIsCorrect
		//Fcontion peremettant de saoir si un widget est correct, si il nous retourne de la data
		public bool WidgetIsCorrect()
		{
			foreach(Project Pro in datamanagement.ListProject)
			{	
				foreach(Node node in Pro.ReturnListNode())
				{
					if(node.Node_Id == Node_Id)
					{
						foreach(Instance ins in node.Instance_)
						{
							if(ins.Instance_Type == TypeInstance)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}		
		
		//Fonction OnTextViewNoteKeyReleaseEvent
		//Fonction permettant de verifier qu'un champ est bien selectionné
		protected void OnTextViewNoteKeyReleaseEvent (object o, Gtk.KeyReleaseEventArgs args)
		{
			string IdSelected = "";	//variable permettant de stocker l'id de l'instance sélectionné
	
			TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
			if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
			{
				if(TypeInstance == param.ParamP("InstLightingName"))
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Lighting_PositionID")); //Nous retournons l'id de l'instance		
				}
				else if(TypeInstance == param.ParamP("InstSwitchName"))
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Switch_PositionID")); //Nous retournons l'id de l'instance		
				}
				else if(TypeInstance == param.ParamP("InstShutterName"))
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Shutter_PositionID")); //Nous retournons l'id de l'instance		
				}
				if(IDMemorised == "")
				{
					IDMemorised = IdSelected;
				}
			}	
			if(IdSelected == "")
			{
				TextViewNote.Buffer.Text = param.ParamT("IP_SelectInstance");
			}		
		}		
		
		//Fonction OnTextViewNoteFocusOutEvent
		//Fonction permettant d'enregistrer les notes d'une instances
		protected void OnTextViewNoteFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
		{
			if(TypeInstance == param.ParamP("InstLightingName"))
			{
				if(IDMemorised != "" && TextViewNote.Buffer.Text != param.ParamT("IP_SelectInstance"))
				{
					datamanagement.ModifyInstance(TextViewNote.Buffer.Text,param.ParamI("MoIn_ChoiceNote"),Convert.ToInt32(IDMemorised));
					IDMemorised = "";
				}
			}
			else if(TypeInstance == param.ParamP("InstSwitchName"))
			{		
				if(IDMemorised != "" && TextViewNote.Buffer.Text != param.ParamT("IP_SelectInstance"))
				{
					datamanagement.ModifyInstance(TextViewNote.Buffer.Text,param.ParamI("MoIn_ChoiceNote"),Convert.ToInt32(IDMemorised));
					IDMemorised = "";
				}			
			}
			else if(TypeInstance == param.ParamP("InstShutterName"))
			{		
				if(IDMemorised != "" && TextViewNote.Buffer.Text != param.ParamT("IP_SelectInstance"))
				{
					datamanagement.ModifyInstance(TextViewNote.Buffer.Text,param.ParamI("MoIn_ChoiceNote"),Convert.ToInt32(IDMemorised));
					IDMemorised = "";
				}				
			}
		}
		
		//Fonction OnAddNewInstanceClicked
		//Fonction permettant d'ajouter un objet rapidement
		protected void OnAddNewInstanceClicked (object sender, System.EventArgs e)
		{
			for(int i = 0;i<Convert.ToInt16(ComboboxNumberOfInstance.ActiveText);i++)
			{			
				if(TypeInstance == "LIGHTING")
				{
					datamanagement.AddInstanceInNode(TypeInstance,datamanagement.ReturnNewNameInstance("Lighting",Node_Id),Node_Id);
				}
				else if(TypeInstance == "SWITCH")
				{
					datamanagement.AddInstanceInNode(TypeInstance,datamanagement.ReturnNewNameInstance("Switch",Node_Id),Node_Id);
				}
				else if(TypeInstance == "SHUTTER")
				{
					datamanagement.AddInstanceInNode(TypeInstance,datamanagement.ReturnNewNameInstance("Shutter",Node_Id),Node_Id);
				}
			}
		}
		
		//Fonction OnDeleteInstanceClicked
		//Fonction permettant de supprimer une instance
		protected void OnDeleteInstanceClicked (object sender, System.EventArgs e)
		{
			string IdSelected = "";	//variable permettant de stocker l'id de l'instance sélectionné
	
			TreeSelection selection = ChildTreeView.Selection; //Nous allons crée un arbre de selection
			if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
			{
				if(TypeInstance == param.ParamP("InstLightingName"))
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Lighting_PositionID")); //Nous retournons l'id de l'instance		
				}
				else if(TypeInstance == param.ParamP("InstSwitchName"))
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Switch_PositionID")); //Nous retournons l'id de l'instance		
				}
				else if(TypeInstance == param.ParamP("InstShutterName"))
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("IP_TVChild_Shutter_PositionID")); //Nous retournons l'id de l'instance		
				}
				if(IDMemorised == "")
				{
					IDMemorised = IdSelected;
				}
			}	
			if(IdSelected != "")
			{
				datamanagement.DeleteInstanceInNode(Convert.ToInt32(IdSelected));
			}			
			
		}
	}
}