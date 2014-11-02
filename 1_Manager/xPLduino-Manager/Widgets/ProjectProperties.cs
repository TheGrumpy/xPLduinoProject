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
using System.Threading;
using System.IO;

namespace xPLduinoManager
{
	//Classe ProjectProperties
	//Classe permettant de gérer les propriété d'un projet
	[System.ComponentModel.ToolboxItem(true)]
	public partial class ProjectProperties : Gtk.Bin
	{
		public DataManagement datamanagement;
		public MainWindow mainwindow;
		public Param param;
		public Int32 Project_Id;
		public string OptionName;
		public string NodeOldLangage;
		public string ParamOldLangage;
		
		//Utilisation de ses variables pour le treeview gerant les paramètres
		public global::Gtk.TreeViewColumn PropertiesTypeColumn;
		public global::Gtk.TreeViewColumn PropertiesValueColumn;
		public global::Gtk.TreeViewColumn PropertiesEmptyColumn;
		
		public global::Gtk.CellRendererText PropertiesTypeCell;
		public global::Gtk.CellRendererText PropertiesValueCell;
		
		public global::Gtk.TreeStore PropertiesListStore;	
		
		//Utilisation de ses variables pour le treeview gerant les options
		public global::Gtk.TreeViewColumn OptionNameColumn = new Gtk.TreeViewColumn();
		
		public global::Gtk.CellRendererPixbuf OptionPixCell = new Gtk.CellRendererPixbuf ();
		public global::Gtk.CellRendererText OptionCell = new Gtk.CellRendererText ();
		
		public global::Gtk.TreeStore OptionListStore;
		public global::Gdk.Pixbuf PngNode;
		public global::Gdk.Pixbuf PngParameter;
		
		public global::Gtk.TreeIter IterNode;
		
		public global::Gtk.TreeModel TreeModelOptionTreeView;
		
		//Utilisation de ses variables pour le treeview child : général
		
		public List<TreeViewColumn> ChildColumnList;
		public global::Gtk.TreeIter IterChild;
		public global::Gtk.TreeModel TreeModelChildTreeView;	
		
		//Utilisation de ses variables pour le treeview child : option noeud
		public global::Gtk.TreeViewColumn Child_Node_IdColumn;
		public global::Gtk.TreeViewColumn Child_Node_NameColumn;
		public global::Gtk.TreeViewColumn Child_Node_IPColumn;
		public global::Gtk.TreeViewColumn Child_Node_GTWIPColumn;
		public global::Gtk.TreeViewColumn Child_Node_DHCPColumn;
		public global::Gtk.TreeViewColumn Child_Node_MacColumn;
		public global::Gtk.TreeViewColumn Child_Node_CompileOkColumn;
		public global::Gtk.TreeViewColumn Child_Node_EmptyColumn;
		
		public global::Gdk.Pixbuf PngOK;
		public global::Gdk.Pixbuf PngNOK;
		
		public global::Gtk.CellRendererText Child_Node_IdCell;
		public global::Gtk.CellRendererText Child_Node_NameCell;
		public global::Gtk.CellRendererText Child_Node_IPCell;
		public global::Gtk.CellRendererText Child_Node_GTWIPCell;
		public global::Gtk.CellRendererText Child_Node_MacCell;
		public global::Gtk.CellRendererToggle Child_Node_DHCPCell;
		public global::Gtk.CellRendererPixbuf Child_Node_CompileOkCell;
		public global::Gtk.CellRendererText Child_Node_EmptyCell;
		
		public global::Gtk.TreeStore Child_Node_ListStore;
		
				
		public ProjectProperties (DataManagement _Datamanagement, MainWindow _MainWindow,Param _Param, Int32 _Project_Id)
		{
			this.param = _Param;
			this.datamanagement = _Datamanagement;
			this.mainwindow = _MainWindow;
			this.Project_Id = _Project_Id;
			this.Build ();
			ChildColumnList = new List<TreeViewColumn>();			
			InitWidget();
			
			InitOptionsTreeView();
			UpdateOptionsTreeView();
			
			OptionName = "";
			
			OptionTreeView.ButtonReleaseEvent += new ButtonReleaseEventHandler(OptionsTreeViewButtonRelease);
			
			//Permet de retourner l'id du projet
			datamanagement.CurrentProjectId = _Project_Id;
		}
		
		//Fonction InitWidget
		//Fonction permettant d'initialiser le widget
		public void InitWidget()
		{
			LabelChildTreeView.Text = param.ParamT("PP_LabTVChild_Name_Choose"); //On met à jour le nom du label au dessus du ChildTreeview
			NoteLabel.Text = param.ParamT("PP_NoteLabel"); //On met à jour le label des notes
			
			ButtonAddNode.Label = param.ParamT("PP_AddNode_Name_Button");
			ButtonDeleteNode.Label = param.ParamT("PP_DeleteNode_Name_Button");
			
			ButtonGenerateOneNode.Label = param.ParamT("PP_CompileOneNode_Name_Button");
			ButtonLoadOneNode.Label = param.ParamT("PP_LoadOneNode_Name_Button");		
			
			hpaned1.Position = (datamanagement.mainwindow.ReturnHpanedPosition() * param.ParamI("NoteHPanedPurcent")) / 100; //On met le hpaned à n% de la taille de la fenetre mere
			
			foreach(Project Pro in datamanagement.ListProject) //Pour chaque projet de la liste
			{
				if(Pro.Project_Id == Project_Id)//Si l'id du projet est égale à l'id de la liste
				{
					TextViewNote.Buffer.Text = Pro.Project_Note;//On affiche les note dans la textview			
				}
			}
			
			vpaned2.Position = (datamanagement.mainwindow.ReturnVpanedPosition() * param.ParamI("NoteVPanedPurcent")) / 100; //On met à jour le vpaned
			InitPropertiesTreeView();
			UpdatePropertiesTreeView();
		}
		
		
//###################### TV Options  ####################################
		
		//Fonction InitOptionsTreeView
		//Fcontion permettant d'initialiser le treeview affichant les différentes options pour un projet
		public void InitOptionsTreeView()
		{	
			//Nouveau Logo
			PngNode = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconNode"));	
			PngParameter = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconProperties"));	
			
			//Nous donnons un titre au colonnes
			OptionNameColumn.Title = param.ParamT("PP_TVOpt_NameLabel");
					
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
			OptionListStore.Clear();//On vide le listestore
			OptionListStore.AppendValues(PngParameter,param.ParamT("PP_TVOpt_OptionParametre"));//On ajoute paramètre dans la liste des option
			OptionListStore.AppendValues(PngNode,param.ParamT("PP_TVOpt_OptionNode"));//On ajoute noeud dans la liste des option
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
					
						if (OptionName == param.ParamT("PP_TVOpt_OptionNode"))//Si l'option sélectionné est de type noeud
						{
							NodeOldLangage = param.ParamT("PP_TVOpt_OptionNode");
							//Ce morceau permet de vider le child treeview
							foreach(TreeViewColumn TVchild in ChildColumnList)
							{
								ChildTreeView.RemoveColumn(TVchild);
							}
						
							if(ChildColumnList.Count >0)
							{ 
								ChildColumnList.RemoveRange(0,ChildColumnList.Count);
							}
							
							InitChildTreeView_Node();//Initialisation du childtreeview avec l'option noeud
							UpdateChildTreeView_Node();//Mise à jour du childtreeview avec la fonction noeud
						}
						else if(OptionName == param.ParamT("PP_TVOpt_OptionParametre"))
						{
							ParamOldLangage = param.ParamT("PP_TVOpt_OptionParametre");
							//Ce morceau permet de vider le child treeview
							foreach(TreeViewColumn TVchild in ChildColumnList)
							{
								ChildTreeView.RemoveColumn(TVchild);
							}
						
							if(ChildColumnList.Count >0)
							{ 
								ChildColumnList.RemoveRange(0,ChildColumnList.Count);
							}
						
							InitPropertiesTreeView();//Initialisation du childtreeview avec l'option propriété
							UpdatePropertiesTreeView();//Mise à jour du childtreeview avec la fonction propriété
						}
					}
				}
			}
		
//###################### TV Child Choice Node #########################	
		
		//Fonction InitChildTreeView_Node
		//Fcontion permettant d'initialiser le treeview cbild lorsque le chois se fait pour un noeud
		public void InitChildTreeView_Node()
		{
			LabelChildTreeView.Text = param.ParamT("PP_LabTVChild_Name_Nodes");//On modifie la label childtreeview
						
			PngOK = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconOK"));	
			PngNOK = global::Gdk.Pixbuf.LoadFromResource(param.ParamP("IconNOK"));	
				
			//Visibilité des lignes et des colonnes
			ChildTreeView.EnableGridLines = TreeViewGridLines.Both;
				
			//Création des colonnes
			Child_Node_IdColumn = new Gtk.TreeViewColumn();
			Child_Node_NameColumn = new Gtk.TreeViewColumn();
			Child_Node_IPColumn = new Gtk.TreeViewColumn();
			Child_Node_GTWIPColumn = new Gtk.TreeViewColumn();
			Child_Node_DHCPColumn = new Gtk.TreeViewColumn();
			Child_Node_MacColumn = new Gtk.TreeViewColumn();
			Child_Node_CompileOkColumn = new Gtk.TreeViewColumn();
			Child_Node_EmptyColumn = new Gtk.TreeViewColumn();			
			
			//On ajoute les colonnes dans une liste
			ChildColumnList.Add(Child_Node_NameColumn);
			ChildColumnList.Add(Child_Node_IPColumn);
			ChildColumnList.Add(Child_Node_GTWIPColumn);
			ChildColumnList.Add(Child_Node_MacColumn);
			ChildColumnList.Add(Child_Node_DHCPColumn);		
			ChildColumnList.Add(Child_Node_IdColumn);
			ChildColumnList.Add(Child_Node_CompileOkColumn);
			ChildColumnList.Add(Child_Node_EmptyColumn);
			
			//Colonne redimensionnable
			Child_Node_IdColumn.Resizable = true;
			Child_Node_NameColumn.Resizable = true;
			Child_Node_IPColumn.Resizable = true;
			Child_Node_GTWIPColumn.Resizable = true;
			Child_Node_DHCPColumn.Resizable = true;
			Child_Node_MacColumn.Resizable = true;
			Child_Node_CompileOkColumn.Resizable = false;
			Child_Node_EmptyColumn.Resizable = true;	
			
			//Nom des colonne
			Child_Node_IdColumn.Title = param.ParamT("PP_TVChild_OpNode_ID");
			Child_Node_NameColumn.Title = param.ParamT("PP_TVChild_OpNode_Name");
			Child_Node_IPColumn.Title = param.ParamT("PP_TVChild_OpNode_IP");
			Child_Node_GTWIPColumn.Title = param.ParamT("PP_TVChild_OpNode_GTWIP");
			Child_Node_DHCPColumn.Title = param.ParamT("PP_TVChild_OpNode_DHCP");
			Child_Node_MacColumn.Title = param.ParamT("PP_TVChild_OpNode_MAC");
			Child_Node_CompileOkColumn.Title = param.ParamT("PP_TVChild_OpNode_CompileOK");
						
			//Visibilité des colonnes
			Child_Node_IdColumn.Visible = false;
			
			//Création des cellules
			Child_Node_IdCell = new Gtk.CellRendererText ();
			Child_Node_NameCell = new Gtk.CellRendererText ();
			Child_Node_IPCell = new Gtk.CellRendererText ();
			Child_Node_GTWIPCell = new Gtk.CellRendererText ();
			Child_Node_MacCell = new Gtk.CellRendererText ();
			Child_Node_DHCPCell = new Gtk.CellRendererToggle ();
			Child_Node_CompileOkCell = new Gtk.CellRendererPixbuf ();
			Child_Node_EmptyCell = new Gtk.CellRendererText ();			
			
			//On rend les cellules éditables
			Child_Node_IdCell.Editable = false;
			Child_Node_NameCell.Editable = true;
			Child_Node_IPCell.Editable = true;
			Child_Node_GTWIPCell.Editable = true;
			Child_Node_DHCPCell.Activatable = true;
			Child_Node_MacCell.Editable = true;
			
			//En cas d'edition des cellule, on appel les fonction associé
			Child_Node_NameCell.Edited += Child_Node_NameCell_Edited;
			Child_Node_IPCell.Edited += Child_Node_IPCell_Edited;
			Child_Node_GTWIPCell.Edited += Child_Node_GTWIPCell_Edited;
			Child_Node_MacCell.Edited += Child_Node_MacCell_Edited;				
			Child_Node_DHCPCell.Toggled += Child_Node_DHCPCell_Toggled;
			
			//On associe des cellules aux colonnes			
			Child_Node_IdColumn.PackStart (Child_Node_IdCell,true);
			Child_Node_NameColumn.PackStart (Child_Node_NameCell,true);
			Child_Node_IPColumn.PackStart (Child_Node_IPCell,true);
			Child_Node_GTWIPColumn.PackStart (Child_Node_GTWIPCell, true);
			Child_Node_MacColumn.PackStart (Child_Node_MacCell,true);			
			Child_Node_DHCPColumn.PackStart (Child_Node_DHCPCell,true);
			Child_Node_CompileOkColumn.PackStart (Child_Node_CompileOkCell, true);
			
			//Ajout des colonnes dans ExplorerTreeView
			ChildTreeView.AppendColumn (Child_Node_NameColumn);
			ChildTreeView.AppendColumn (Child_Node_IPColumn);
			ChildTreeView.AppendColumn (Child_Node_GTWIPColumn);
			ChildTreeView.AppendColumn (Child_Node_MacColumn);			
			ChildTreeView.AppendColumn (Child_Node_DHCPColumn);
			ChildTreeView.AppendColumn (Child_Node_CompileOkColumn);			
			ChildTreeView.AppendColumn (Child_Node_IdColumn);	
			ChildTreeView.AppendColumn (Child_Node_EmptyColumn);
			
			//Ajout des attibuts à chaque colonne
			Child_Node_NameColumn.AddAttribute(Child_Node_NameCell,"text",param.ParamI("PP_TVChild_OpNode_PositionName"));
			Child_Node_IPColumn.AddAttribute(Child_Node_IPCell,"text",param.ParamI("PP_TVChild_OpNode_PositionIP"));
			Child_Node_GTWIPColumn.AddAttribute(Child_Node_GTWIPCell,"text",param.ParamI("PP_TVChild_OpNode_PositionGTWIP"));
			Child_Node_MacColumn.AddAttribute(Child_Node_MacCell,"text",param.ParamI("PP_TVChild_OpNode_PositionMAC"));				
			Child_Node_DHCPColumn.AddAttribute(Child_Node_DHCPCell,"active",param.ParamI("PP_TVChild_OpNode_PositionDHCP"));
			Child_Node_CompileOkColumn.AddAttribute(Child_Node_CompileOkCell,"pixbuf",param.ParamI("PP_TVChild_OpNode_PositionCompileOK"));
			Child_Node_IdColumn.AddAttribute(Child_Node_IdCell,"text",param.ParamI("PP_TVChild_OpNode_PositionID"));
			
			//Création d'un nouveau store
			Child_Node_ListStore = new Gtk.TreeStore(typeof(string),typeof(string),typeof(string),typeof(string),typeof(bool),typeof (Gdk.Pixbuf),typeof(string));	
			
			//On écrit le store dans le treeview
			ChildTreeView.Model = Child_Node_ListStore;
			ChildTreeView.ShowAll ();				
		}
		
		//Fonction UpdateChildTreeView_Node
		//Fonction permettant de mettre à jour le child treeview
		public void UpdateChildTreeView_Node()
		{
			Child_Node_ListStore.Clear();//On vide le store
			foreach(Project Pro in datamanagement.ListProject)//Pour chaque projet de la liste
			{	
				if(Pro.Project_Id == Project_Id)//si un projet est égale à l'id project mis en paramètre
				{
					foreach(Node node in Pro.ReturnListNode())//Pour chaque noeud présent dans un projet
					{
						if(node.Node_Compile)
						{
							IterChild = Child_Node_ListStore.AppendValues(node.Node_Name,node.Node_IP,node.Node_GTWIP,node.Node_Mac,node.Node_DHCP,PngOK,node.Node_Id.ToString());//On affiche le noeud
						}
						else
						{
							IterChild = Child_Node_ListStore.AppendValues(node.Node_Name,node.Node_IP,node.Node_GTWIP,node.Node_Mac,node.Node_DHCP,PngNOK,node.Node_Id.ToString());//On affiche le noeud
						}
					}
				}
			}
			ChildTreeView.Model = Child_Node_ListStore;
			ChildTreeView.ShowAll();			
		}
		
				//Fonction ChildNameNodeValueCell_Edited
				//Fonction permettant de mettre à jour les données lors d'un clic dans la cellule NOM dans le child treeview		
				private void Child_Node_NameCell_Edited (object o, Gtk.EditedArgs args)
				{
					string IdSelected = "";		//variable permettant de stocker l'id selectionné
					string NameSelected = "";	//variable permettant de stocker le nom selectionné
			
					TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
					if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
					{
						IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("PP_TVChild_OpNode_PositionID")); //Nous retournons l'id du noeud selectionné
						NameSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("PP_TVChild_OpNode_PositionName")); //Nous retournons le nom du noeud selectionné		
					}	 
			
					if(NameSelected != args.NewText.Replace(" ","_")) //Permet de verifier si on fait un changment dans le nom
					{
						if(args.NewText != "") //Si le texte est pas vide
						{
							if(args.NewText.Length <= param.ParamI("CarSizeMax"))//Si la taille de la nouvelle chaine de caractère est infererieur au nombre prédéfinis
							{
								datamanagement.ModifyNode(args.NewText,param.ParamI("MoNo_ChoiceName"),Convert.ToInt32(IdSelected));//On modifie le nom
								UpdateChildTreeView_Node(); //On met à jour le PropertiesTreeview
								UpdateOptionsTreeView(); //On met à jour l'option treeview
								OptionTreeView.ExpandAll();//On devellope l'arbre
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
				}		
		
				//Fonction ChildIPNodeValueCell_Edited
				//Fonction permettant de mettre à jour les données lors d'un clic dans la cellule L'ADRESSE IP dans le child treeview
				private void Child_Node_IPCell_Edited (object o, Gtk.EditedArgs args)
				{
					string IdSelected = "";		//variable permettant de stocker l'id selectionné
					string IpSelected = "";	//variable permettant de stocker le nom selectionné
			
					TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
					if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
					{
						IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("PP_TVChild_OpNode_PositionID")); //Nous retournons l'id du noeud selectionné
						IpSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("PP_TVChild_OpNode_PositionIP")); //Nous retournons le nom du noeud selectionné		
					}	 
			
					if(IpSelected != args.NewText) //Permet de verifier si on fait un changment dans le nom
					{
						if(args.NewText != "") //Si le texte est pas vide
						{
							if(datamanagement.AnalyseIP(args.NewText))
							{
								datamanagement.ModifyNode(args.NewText,param.ParamI("MoNo_ChoiceIP"),Convert.ToInt32(IdSelected));//On modifie le nom
								UpdateChildTreeView_Node(); //On met à jour le PropertiesTreeview
							}
							else
							{
								datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"IPNodeInvalid"); //on affiche un message d'erreur
							}
						}
						else //Dans le cas ou le texte est vide
						{
							datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"IPNodeInvalid"); //on affiche un message d'erreur
						}
					}
				}					
		
				//Fonction ChildGTWIPNodeValueCell_Edited
				//Fonction permettant de mettre à jour les données lors d'un clic dans la cellule L'ADRESSE GTW IP dans le child treeview
				private void Child_Node_GTWIPCell_Edited (object o, Gtk.EditedArgs args)
				{
					string IdSelected = "";		//variable permettant de stocker l'id selectionné
					string IpSelected = "";	//variable permettant de stocker le nom selectionné
			
					TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
					if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
					{
						IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("PP_TVChild_OpNode_PositionID")); //Nous retournons l'id du noeud selectionné
						IpSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("PP_TVChild_OpNode_PositionGTWIP")); //Nous retournons le nom du noeud selectionné		
					}	 
			
					if(IpSelected != args.NewText) //Permet de verifier si on fait un changment dans le nom
					{
						if(args.NewText != "") //Si le texte est pas vide
						{
							if(datamanagement.AnalyseIP(args.NewText))
							{
								datamanagement.ModifyNode(args.NewText,param.ParamI("MoNo_ChoiceGTWIP"),Convert.ToInt32(IdSelected));//On modifie le nom
								UpdateChildTreeView_Node(); //On met à jour le PropertiesTreeview
							}
							else
							{
								datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"IPNodeInvalid"); //on affiche un message d'erreur
							}							
						}
						else //Dans le cas ou le texte est vide
						{
							datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"IPNodeInvalid"); //on affiche un message d'erreur
						}
					}
				}		
		
				//Fonction ChildMacNodeValueCell_Edited
				//Fonction permettant la modification de l'adresse mac d'un noeud 
				private void Child_Node_MacCell_Edited (object o, Gtk.EditedArgs args)
				{
					string IdSelected = "";
			
					TreeSelection selection = ChildTreeView.Selection; //Nous allons cree un arbre de selection
					if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
					{
						IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("PP_TVChild_OpNode_PositionID")); //Nous retournons l'id du noeud selectionné
					}
					datamanagement.ModifyNode("", param.ParamI("MoNo_ChoiceMac"),Convert.ToInt32(IdSelected));//On modifie l'adresse mac en automatique
					UpdateChildTreeView_Node(); //On met à jour le PropertiesTreeview
				}
		
				//Fonction ChildDHCPNodeCell_Toggled
				//Fonction faite lors d'un clic sur un check button
				private void Child_Node_DHCPCell_Toggled (object o, Gtk.ToggledArgs args) 
				{
					bool Value = false; //Initialisation d'un bool permettant de récuperer la valeur lors d'un clic
					Gtk.TreeIter iter;
					if (Child_Node_ListStore.GetIter (out iter, new Gtk.TreePath(args.Path))) {
						Value = (bool) Child_Node_ListStore.GetValue(iter,param.ParamI("PP_TVChild_OpNode_PositionDHCP")); //on fait l'acquisition de la valeur
						Child_Node_ListStore.SetValue(iter,param.ParamI("PP_TVChild_OpNode_PositionDHCP"),!Value);//On affiche la valeur inversé
					}	
				}	
		
//####################### TV Child Choice Properties ####################################		
		
		//Fonction InitPropertiesTreeView
		//Fonction permettant d'initialiser l'arbre des propriété
		public void InitPropertiesTreeView()
		{	
			LabelChildTreeView.Text = param.ParamT("PP_LabTVChild_Name_Properties"); //On modifie la label childtreeview
			ChildTreeView.EnableGridLines = TreeViewGridLines.Both;
			
			//Ajout de deux colonnes dans TreeviewProperties
			PropertiesTypeColumn = new Gtk.TreeViewColumn();
			PropertiesValueColumn = new Gtk.TreeViewColumn();
			PropertiesEmptyColumn = new Gtk.TreeViewColumn();
			
			//Nous donnons un titre au colonne
			PropertiesTypeColumn.Title = param.ParamT("PP_TVPro_TypeLabel");
			PropertiesValueColumn.Title = param.ParamT("PP_TVPro_ValueLabel");
			
			//On ajoute les colonnes dans une liste
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
			PropertiesTypeColumn.AddAttribute(PropertiesTypeCell,"text",param.ParamI("PP_TVPro_TypePosition"));
			PropertiesValueColumn.AddAttribute (PropertiesValueCell, "text", param.ParamI("PP_TVPro_ValuePosition"));
			
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
			PropertiesListStore.Clear();//On vide le listestore

			foreach(Project Pro in datamanagement.ListProject)//Pour chaque projet de la liste
			{
				if(Pro.Project_Id == Project_Id)//Si l'id du projet est celui choisit
				{						
					PropertiesListStore.AppendValues(param.ParamT("PP_TVPro_LabelLine1"),Pro.Project_Name);
					PropertiesListStore.AppendValues(param.ParamT("PP_TVPro_LabelLine2"),Pro.Project_Author);
					PropertiesListStore.AppendValues(param.ParamT("PP_TVPro_LabelLine3"),Pro.Project_CreationDateAndTime.ToString());
					PropertiesListStore.AppendValues(param.ParamT("PP_TVPro_LabelLine4"),Pro.Project_ModificationDateAndTime.ToString());
					PropertiesListStore.AppendValues(param.ParamT("PP_TVPro_LabelLine5"),Pro.Project_Version.ToString());
					PropertiesListStore.AppendValues(param.ParamT("PP_TVPro_LabelLine6"),Pro.Project_SavePath);
					if(Pro.Project_Password != "")
					{
						PropertiesListStore.AppendValues(param.ParamT("PP_TVPro_LabelLine7"),"●●●●●●●●");
					}
					else
					{
						PropertiesListStore.AppendValues(param.ParamT("PP_TVPro_LabelLine7"),"");
					}
					
				}
			}				
			
			//On écrit le store dans le treeview
			ChildTreeView.Model = PropertiesListStore;
			ChildTreeView.ShowAll ();			
		}
		
				//Fonction PropertiesValueCell_Edited
				//Fonction permettant de mettre à jour des information en cliquant sur une cellule
				private void PropertiesValueCell_Edited (object o, Gtk.EditedArgs args)
				{
					Gtk.TreeIter iter; //Conception d'un TreeIter permettant de retourner la position de la valeur cliqué
					PropertiesListStore.GetIter (out iter, new Gtk.TreePath (args.Path)); //Nous allons cherché cette valeur qui est arg.Path ainsi que le nouveau text dans args.Text
				
					if(Convert.ToByte(args.Path) == param.ParamI("PP_TVPro_PathPostion")) //Si nous voulons modifier le chemin
					{
						FileChooserDialog chooser = new FileChooserDialog(param.ParamT("ChooseAFolder"),datamanagement.mainwindow,FileChooserAction.SelectFolder, param.ParamT("Cancel"), ResponseType.Cancel,param.ParamT("Open"),ResponseType.Accept); //Permet d'ouvrir un fenêtre de dialogue permettant de choisir un dossier
						if( chooser.Run() == (int)ResponseType.Accept ) //Si nous faisons OK
						{
							datamanagement.ModifyProject(chooser.Filename,param.ParamI("MoPo_ChoicePath"),Project_Id); //Nous modifions le projet
						}
						chooser.Destroy(); //Destruction de la fenêtre
				
					}
					else if(Convert.ToByte(args.Path) == param.ParamI("PP_TVPro_NamePostion")) //Si nous voulons modifier le nom
					{
						if(args.NewText == "") //Si le nouveau texte est vide
						{
							datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameProjectEmpty");		
						}
						else //Sinon on vient mettre  à jour les données avec les nouvelles informations cf fonction de mise à jour dans le datamanagement
						{
							if(args.NewText.Length <= param.ParamI("CarSizeMax"))
							{
								datamanagement.ModifyProject(args.NewText,param.ParamI("MoPo_ChoiceName"),Project_Id);
								datamanagement.mainwindow.UpdateEplorerTreeView(); //On met à jour ExplorerTreeView
							}
							else //Dans le cas où le nom du projet dépasse 16 caractère
							{
								datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameProjectTooLong"); //on affiche un message d'erreur						
							}
						}					
					}
					else if(Convert.ToByte(args.Path) == param.ParamI("PP_TVPro_AuthorPostion")) //Si nous voulons modifier le nom
					{
						if(args.NewText == "") //Si le nouveau texte est vide
						{
							datamanagement.mainwindow.AddLineOutput(param.ParamI("OutputError"),"AuthorProjectEmpty");		
						}
						else  //Sinon on vient mettre  à jour les données avec les nouvelles informations cf fonction de mise à jour dans le datamanagement
						{
							datamanagement.ModifyProject(args.NewText,param.ParamI("MoPo_ChoiceAuthor"),Project_Id);
						}					
					}	
					else if(Convert.ToByte(args.Path) == param.ParamI("PP_TVPro_PasswordPostion")) //Si nous voulons modifier le nom
					{
						datamanagement.ModifyProject(args.NewText,param.ParamI("MoPo_ChoicePassword"),Project_Id);			
					}			
			
					UpdatePropertiesTreeView(); //On met à jour le PropertiesTreeview
				}				

//###################### Other  ####################################
		
		//OnTreeViewChildButtonReleaseEvent
		//Fonction permettant de faire des action sur clic dans le treeview
		//Cette fonction va notemment nous servir lorsque nous cliquons sur une box pour enregistrer les nouvelle valeur
		protected void OnChildTreeViewButtonReleaseEvent (object o, Gtk.ButtonReleaseEventArgs args)
		{
			if(OptionName == param.ParamT("PP_TVOpt_OptionNode") || OptionName == NodeOldLangage)
			{
				bool ChildDHCPSelect = false; //Variable permettant de stocker la valeur DHCP que nous allons récuperer
				string ChildIDSelect = ""; //Variable permettant de stocker la valeur Id que nous allons récuperer
				TreeSelection selection = (o as TreeView).Selection; //Nous allons creer un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					ChildDHCPSelect = (bool) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("PP_TVChild_OpNode_PositionDHCP")); //Nous récuperons la valeur que nous stockons
					ChildIDSelect = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("PP_TVChild_OpNode_PositionID")); //Nous récuperons la valeur que nous stockons
					
					foreach(Project Pro in datamanagement.ListProject) //Pour chaque projet 
					{
						foreach(Node node in Pro.ReturnListNode()) //pour chaque noeud
						{
							if(node.Node_Id == Convert.ToInt32(ChildIDSelect) && node.Node_DHCP != ChildDHCPSelect) //Si l'id est égale mais que la valeur DHCP est différente
							{
								datamanagement.ModifyNode(ChildDHCPSelect.ToString(),param.ParamI("MoNo_ChoiceDHCP"),Convert.ToInt32(ChildIDSelect)); //On fait la mise à jour du DHCP
							}
						}
					}
				}	
			}
		}	
		
		//Fonction UpdateWidget
		//Fcontion permettant de mettre à jour le widget dans le tab
		public void UpdateWidget()
		{
			OptionNameColumn.Title = param.ParamT("PP_TVOpt_NameLabel");
			NoteLabel.Text = param.ParamT("PP_NoteLabel"); //On met à jour le label des notes
			
			ButtonAddNode.Label = param.ParamT("PP_AddNode_Name_Button");
			ButtonDeleteNode.Label = param.ParamT("PP_DeleteNode_Name_Button");
			ButtonGenerateOneNode.Label = param.ParamT("PP_CompileOneNode_Name_Button");
			ButtonLoadOneNode.Label = param.ParamT("PP_LoadOneNode_Name_Button");
			
			//Init and update OptionTreeView
			OptionNameColumn.Title = param.ParamT("PP_TVOpt_NameLabel");
			UpdateOptionsTreeView();
			
			foreach(Project Pro in datamanagement.ListProject) //Pour chaque projet de la liste
			{
				if(Pro.Project_Id == Project_Id)//Si l'id du projet est égale à l'id de la liste
				{
					TextViewNote.Buffer.Text = Pro.Project_Note;//On affiche les note dans la textview			
				}
			}			
			
			if (OptionName == null || OptionName == "")
			{
				//Ce morceau permet de vider le child treeview
				foreach(TreeViewColumn TVchild in ChildColumnList)
				{
					ChildTreeView.RemoveColumn(TVchild);
				}
			
				if(ChildColumnList.Count >0)
				{ 
					ChildColumnList.RemoveRange(0,ChildColumnList.Count);
				}
			
				InitPropertiesTreeView();//Initialisation du childtreeview avec l'option propriété
				UpdatePropertiesTreeView();//Mise à jour du childtreeview avec la fonction propriété						
			}
			else if (OptionName == NodeOldLangage)//Si l'option sélectionné est de type noeud
			{
				//Ce morceau permet de vider le child treeview
				foreach(TreeViewColumn TVchild in ChildColumnList)
				{
					ChildTreeView.RemoveColumn(TVchild);
				}
			
				if(ChildColumnList.Count >0)
				{ 
					ChildColumnList.RemoveRange(0,ChildColumnList.Count);
				}
				
				InitChildTreeView_Node();//Initialisation du childtreeview avec l'option noeud
				UpdateChildTreeView_Node();//Mise à jour du childtreeview avec la fonction noeud
			}
			else if(OptionName == ParamOldLangage)
			{
				//Ce morceau permet de vider le child treeview
				foreach(TreeViewColumn TVchild in ChildColumnList)
				{
					ChildTreeView.RemoveColumn(TVchild);
				}
			
				if(ChildColumnList.Count >0)
				{ 
					ChildColumnList.RemoveRange(0,ChildColumnList.Count);
				}
			
				InitPropertiesTreeView();//Initialisation du childtreeview avec l'option propriété
				UpdatePropertiesTreeView();//Mise à jour du childtreeview avec la fonction propriété
			}	
			else
			{
				//Ce morceau permet de vider le child treeview
				foreach(TreeViewColumn TVchild in ChildColumnList)
				{
					ChildTreeView.RemoveColumn(TVchild);
				}
			
				if(ChildColumnList.Count >0)
				{ 
					ChildColumnList.RemoveRange(0,ChildColumnList.Count);
				}
			
				InitPropertiesTreeView();//Initialisation du childtreeview avec l'option propriété
				UpdatePropertiesTreeView();//Mise à jour du childtreeview avec la fonction propriété		
			}
		}
		
		//Fonction WidgetIsCorrect
		//Fcontion peremettant de saoir si un widget est correct, si il nous retourne de la data
		public bool WidgetIsCorrect()
		{
			foreach(Project Pro in datamanagement.ListProject)//Pour chaque projet de la liste
			{	
				if(Pro.Project_Id == Project_Id)//si un projet est égale à l'id project mis en paramètre
				{
					return true;
				}
			}
			return false;
		}
		
		//Fonction OnTextViewNoteFocusOutEvent
		//Fonction permettant d'enregistrer une note dans le cas ou le focus quitte le textview
		protected void OnTextViewNoteFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
		{
			datamanagement.ModifyProject(TextViewNote.Buffer.Text,param.ParamI("MoPo_ChoiceNote"),Project_Id); //Modification des notes du projet
		}
		
		//Fonction OnButtonAddNodeClicked
		//Fonction permettant d'ajouter un noeud
		protected void OnButtonAddNodeClicked (object sender, System.EventArgs e)
		{
			datamanagement.AddNodeInProject(datamanagement.ReturnNewNameNode(param.ParamT("NNDefaultNodeName"), Project_Id),Project_Id);
		}
		
		//Fonction OnButtonDeleteNodeClicked
		//Fonction permettant de supprimer un noeud
		protected void OnButtonDeleteNodeClicked (object sender, System.EventArgs e)
		{
			string IdSelected = "";	//variable permettant de stocker l'id de l'instance sélectionné
	
			TreeSelection selection = ChildTreeView.Selection; //Nous allons crée un arbre de selection
			if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
			{
				IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("PP_TVChild_OpNode_PositionID")); //Nous retournons l'id de l'instance		
			}	
			if(IdSelected != "")
			{
				datamanagement.DeleteNodeInProject(Convert.ToInt32(IdSelected));
			}		
			else
			{
				mainwindow.AddLineOutput(param.ParamI("OutputError"),param.ParamT("PP_ErrorMessage_ChooseANode"));
			}			
		}
		
		//Fonction OnButtonGenerateOneNodeClicked
		//Fonction permettant de compiler un noeud que nous avons choisit
		protected void OnButtonGenerateOneNodeClicked (object sender, System.EventArgs e)
		{
			string IdSelected = "";	//variable permettant de stocker l'id de l'instance sélectionné
	
			TreeSelection selection = ChildTreeView.Selection; //Nous allons crée un arbre de selection
			if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
			{
				IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("PP_TVChild_OpNode_PositionID")); //Nous retournons l'id de l'instance		
			}				
			
			if(IdSelected != "")
			{
				if(datamanagement.SaveProject(false, datamanagement.CurrentProjectId))
				{			
					mainwindow.EraseOutputFunction();
					Thread threadcompil = new Thread(()=> datamanagement.CompileFile(Project_Id,Convert.ToInt32(IdSelected)));
					threadcompil.IsBackground = true;
					threadcompil.Start();
				}	
			}
			else
			{
				mainwindow.AddLineOutput(param.ParamI("OutputError"),param.ParamT("PP_ErrorMessage_ChooseANode"));
			}
		}

		//Fonction OnButtonLoadOneNodeClicked
		//Fonction permettant de charger une carte		
		protected void OnButtonLoadOneNodeClicked (object sender, System.EventArgs e)
		{
			string IdSelected = "";	//variable permettant de stocker l'id de l'instance sélectionné
	
			TreeSelection selection = ChildTreeView.Selection; //Nous allons crée un arbre de selection
			if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
			{
				IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("PP_TVChild_OpNode_PositionID")); //Nous retournons l'id de l'instance		
			}	
			
			foreach(Project pro in datamanagement.ListProject)
			{
				foreach(Node node in pro.ReturnListNode())
				{
					if(IdSelected != "")
					{
						if(node.Node_Id == Convert.ToInt32(IdSelected))
						{
							if(pro.ProjectIsSave)
							{
								if(node.Node_Compile)
								{
									if(File.Exists("/dev/tty" + mainwindow.ReturnNameUSB()))
									{		
										Thread threadload =new Thread(()=> datamanagement.LoadBoard(mainwindow.ReturnNameUSB(),Project_Id,node.Node_Id));
										threadload.IsBackground = true;
										threadload.Start();								
									}
									else
									{
										mainwindow.AddLineOutput(param.ParamI("OutputError"),"ConnectAProgrammer");
										mainwindow.UpdateComboboxSelectUsb();
									}								
								}
								else
								{
									//Message noeud non compilé
									mainwindow.AddLineOutput(param.ParamI("OutputError"),param.ParamT("PP_ErrorMessage_NodeNotCompile"));
								}
							}
							else
							{
								//Message projet non enregistré
								mainwindow.AddLineOutput(param.ParamI("OutputError"),param.ParamT("PP_ErrorMessage_ProjectNotSave"));
							}
						}
					}
					else
					{
						mainwindow.AddLineOutput(param.ParamI("OutputError"),param.ParamT("PP_ErrorMessage_ChooseANode"));
					}
				}
			}
			Console.WriteLine("End of Load");
		}
	}
}