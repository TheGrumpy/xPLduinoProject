using System;
using Gtk;
using Gdk;
using System.Collections.Generic;

namespace xPLduinoManager
{
	//Classe Properties
	//Cette classe va nous permettre d'afficher les paramètres et voir les enfants 
	//Eléments :
	//	DataManagement datamanagement : Ceci nous permettra de récupérer des données
	//	string Type : Ceci va nous retourner le dont il faut afficher les propriété
	//	Int32 Id : Id de l'élément à afficher
	//	string OldChildSelectedValue : Nous mémorisons l'id sélectionner dans le child
	//
	//	Gtk.TreeViewColumn PropertiesTypeColumn : Colonne du type de données dans TreeviewProperties
	//	Gtk.TreeViewColumn PropertiesValueColumn : Colonne de la valeur dans TreeviewProperties
	//
	//	Gtk.CellRendererText PropertiesTypeCell : Cellule pour le type dans TreeviewProperties
	//	Gtk.CellRendererText PropertiesValueCell : Ceullule pour la veleur dans TreeviewProperties
	//
	//	Gtk.TreeStore PropertiesListStore : Store contenant les données
	//
	//Fonction :
	//
	public partial class Properties : Gtk.Bin
	{
		public DataManagement datamanagement;
		public Param param;
		public string Type;
		public Int32 Id;
		
		//Utilisation de ses variables pour le treeview gerant les paramètres
		public global::Gtk.TreeViewColumn PropertiesTypeColumn;
		public global::Gtk.TreeViewColumn PropertiesValueColumn;
		public global::Gtk.TreeViewColumn PropertiesEmptyColumn;
		
		public global::Gtk.CellRendererText PropertiesTypeCell;
		public global::Gtk.CellRendererText PropertiesValueCell;
		
		public global::Gtk.TreeStore PropertiesListStore;
		
		//Utilisations de ses variables pour le treeview gerant les projets
		public global::Gtk.TreeViewColumn ChildIdNodeColumn;
		public global::Gtk.TreeViewColumn ChildNameNodeColumn;
		public global::Gtk.TreeViewColumn ChildIpNodeColumn;
		public global::Gtk.TreeViewColumn ChildMacNodeColumn;
		public global::Gtk.TreeViewColumn ChildDHCPNodeColumn;
		public global::Gtk.TreeViewColumn ChildEmptyColumn;
		
		public global::Gtk.CellRendererText ChildIdNodeCell;
		public global::Gtk.CellRendererText ChildNameNodeCell;
		public global::Gtk.CellRendererText ChildIpNodeCell;
		public global::Gtk.CellRendererText ChildMacNodeCell;
		public global::Gtk.CellRendererToggle ChildDHCPNodeCell;
		public global::Gtk.CellRendererText ChildEmptyCell;
		
		public global::Gtk.TreeStore ChildListStore;
		
		//Utilisations de ses variables pour le treeview gerant les noeuds

		public global::Gtk.TreeModel TreeModelChildTreeView;	
		public global::Gtk.TreeIter IterChild;
		
		//Constructeur de la classe Properties
		//Arguments :
		//	DataManagement _datamanagement : Nous avons besoin des informations provenant du datamangement
		//	string _Type : Nous passons le type (Projet, Noeud, carte ...) où nous voulons voir les propriété
		//	Int32 _Id : Id du type
		public Properties (DataManagement _datamanagement, Param _param, string _Type, Int32 _Id)
		{
			this.datamanagement = _datamanagement;
			param = _param;
			
			this.Type = _Type;
			this.Id = _Id;
			this.Build ();

			/*
			InitTreeviewProperties();
			UpdateTreeviewProperties();
			
			
			if(Type == param.ExTVTypeProject) //Dans le cas ou nous double cliquons sur un projet
			{
				InitTreeviewChildOnProject(); //On initialise le bon treeview child project
				UpdateTreeViewChildOnProject();	//On le met à jour						
			}
			else if(Type == param.ExTVTypeNode) //Dans le cas ou nous double cliquons sur un noeud
			{
				
			}*/
		}
		
//###################Fonction TreeViewProperties#########################		
/*		
		//Fonction InitTreeviewProperties
		//Cette fonction va nous permettre d'initialiser le TreeView qui affichera les paramètres
		public void InitTreeviewProperties()
		{
			TreeviewProperties.EnableGridLines = TreeViewGridLines.Both;
			
			//Ajout de deux colonnes dans TreeviewProperties
			PropertiesTypeColumn = new Gtk.TreeViewColumn();
			PropertiesValueColumn = new Gtk.TreeViewColumn();
			
			//Colonne vide
			PropertiesEmptyColumn = new Gtk.TreeViewColumn();
			
			//Nous donnons un titre au colonne
			PropertiesTypeColumn.Title = param.PropTVTypeLabel;
			PropertiesValueColumn.Title = param.PropTVValueLabel;
			
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
			TreeviewProperties.AppendColumn (PropertiesTypeColumn);
			TreeviewProperties.AppendColumn (PropertiesValueColumn);
			TreeviewProperties.AppendColumn (PropertiesEmptyColumn);
			
			//Ajout des attibuts à chaque colonne
			PropertiesTypeColumn.AddAttribute(PropertiesTypeCell,"text",param.PropTVPositionType);
			PropertiesValueColumn.AddAttribute (PropertiesValueCell, "text", param.PropTVPositionValue);
			
			//Création d'un nouveau store, param : texte Type, texte Value
			PropertiesListStore = new Gtk.TreeStore(typeof (string),typeof (string));
			
			//On écrit le store dans le treeview
			TreeviewProperties.Model = PropertiesListStore;
			this.ShowAll ();			
		}
	
		//Fonction UpdateTreeviewProperties
		//Fonction permettant de mettre à jour le TreeviewProperties
		public void UpdateTreeviewProperties()
		{
			PropertiesListStore.Clear();
			if(Type == param.ExTVTypeProject)
			{
				foreach(Project Pro in datamanagement.ListProject)
				{
					if(Pro.Project_Id == Id)
					{
						LabelProperties.LabelProp = param.PropExpLabel_Pro + Pro.Project_Name.Replace("_"," ");
						GeneralLabel.LabelProp = param.PropExpGeneralLabel_Pro;
						NoteLabel.LabelProp = param.PropExpNoteLabel_Pro;
						LabelChild.LabelProp = param.PropExpChildLabel_Pro;
							
						PropertiesListStore.AppendValues(param.PropTVLine1_Pro,Pro.Project_Name);
						PropertiesListStore.AppendValues(param.PropTVLine2_Pro,Pro.Project_Author);
						PropertiesListStore.AppendValues(param.PropTVLine3_Pro,Pro.Project_CreationDateAndTime.ToString());
						PropertiesListStore.AppendValues(param.PropTVLine4_Pro,Pro.Project_ModificationDateAndTime.ToString());
						PropertiesListStore.AppendValues(param.PropTVLine5_Pro,Pro.Project_Version.ToString());
						PropertiesListStore.AppendValues(param.PropTVLine6_Pro,Pro.Project_SavePath);
						
						TextViewNote.Buffer.Text = Pro.Project_Comment;
					}
				}				
			}
			else if(Type == param.ExTVTypeNode)
			{
				PropertiesValueCell.Editable = false;
				foreach(Project Pro in datamanagement.ListProject)
				{
					foreach(Node node in Pro.ReturnListNode())
					{
						if(node.Node_Id == Id)
						{
							LabelProperties.LabelProp = param.PropExpLabel_Nod + node.Node_Name.Replace("_"," ");
							GeneralLabel.LabelProp = param.PropExpGeneralLabel_Nod;
							NoteLabel.LabelProp = param.PropExpNoteLabel_Nod;
							LabelChild.LabelProp = param.PropExpChildLabel_Nod;
						
							PropertiesListStore.AppendValues(param.PropTVLine1_Nod,node.Node_Name);
							PropertiesListStore.AppendValues(param.PropTVLine2_Nod,node.Node_IP);
							PropertiesListStore.AppendValues(param.PropTVLine3_Nod,node.Node_Mac);
							PropertiesListStore.AppendValues(param.PropTVLine4_Nod,node.Node_DHCP.ToString());
						}
					}
				}				
			}
			
			
			//On écrit le store dans le treeview
			TreeviewProperties.Model = PropertiesListStore;
			TreeviewProperties.ShowAll ();
		}
		
				//Fonction PropertiesValueCell_Edited
				//Fonction permettant de mettre à jour des information en cliquant sur une cellule
				private void PropertiesValueCell_Edited (object o, Gtk.EditedArgs args)
				{
					Gtk.TreeIter iter; //Conception d'un TreeIter permettant de retourner la position de la valeur cliqué
					PropertiesListStore.GetIter (out iter, new Gtk.TreePath (args.Path)); //Nous allons cherché cette valeur qui est arg.Path ainsi que le nouveau text dans args.Text
				
					if(Convert.ToByte(args.Path) == param.PropTVPathPosition_Pro) //Si nous voulons modifier le chemin
					{
						FileChooserDialog chooser = new FileChooserDialog(param.ChooseAFolder,datamanagement.mainwindow,FileChooserAction.SelectFolder, param.Cancel, ResponseType.Cancel,param.Open,ResponseType.Accept); //Permet d'ouvrir un fenêtre de dialogue permettant de choisir un dossier
						if( chooser.Run() == (int)ResponseType.Accept ) //Si nous faisons OK
						{
							datamanagement.ModifyProject(chooser.Filename,Convert.ToByte(args.Path),Id); //Nous modifions le projet
						}
						chooser.Destroy(); //Destruction de la fenêtre
				
					}
					else if(Convert.ToByte(args.Path) != param.PropTVDTCreationPosition_Pro && Convert.ToByte(args.Path) != param.PropTVDTModificationPosition_Pro && Convert.ToByte(args.Path) != param.PropTVVersionPosition_Pro) //On fait pas la mise à jour des dates et de la version
					{
						if(args.NewText == "") //Si le nouveau texte est vide
						{
							datamanagement.mainwindow.AddLineOutput(param.OutputError,param.NameProjectEmpty);		
						}
						else //Sinon on vient mettre  à jour les données avec les nouvelles informations cf fonction de mise à jour dans le datamanagement
						{
							if(args.NewText.Length <= param.CarSizeMax)
							{
								datamanagement.ModifyProject(args.NewText,Convert.ToByte(args.Path),Id);
							}
							else //Dans le cas où le nom du projet dépasse 16 caractère
							{
								datamanagement.mainwindow.AddLineOutput(param.OutputError,param.NameProjectTooLong); //on affiche un message d'erreur						
							}
						}
					}
					UpdateTreeviewProperties(); //On met à jour le PropertiesTreeview
					datamanagement.mainwindow.UpdateEplorerTreeView(); //On met à jour ExplorerTreeView
				}	
		
//################### Fonction ChildProperties #######################		
		
		//$$$$$$$$$$$$$$$ PROJECT $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
		
		//Fonction InitTreeviewChildOnProject
		//Permet d'initialier le TreeViewChild lorsque que nous sommes dans les propriété d'un projet
		public void InitTreeviewChildOnProject()
		{	
				
			//Ajout de trois colonnes dans TreeViewChild
			ChildIdNodeColumn = new Gtk.TreeViewColumn();
			ChildNameNodeColumn = new Gtk.TreeViewColumn();
			ChildIpNodeColumn = new Gtk.TreeViewColumn();
			ChildDHCPNodeColumn	= new Gtk.TreeViewColumn();
			ChildMacNodeColumn = new Gtk.TreeViewColumn();		
			ChildEmptyColumn = new Gtk.TreeViewColumn();
			
			//Les colonnes sont redimensionnable
			ChildIdNodeColumn.Resizable = true;
			ChildNameNodeColumn.Resizable = true;
			ChildIpNodeColumn.Resizable = true;
			ChildMacNodeColumn.Resizable = true;
			ChildDHCPNodeColumn.Resizable = true;
			
			//Nous donnons un titre au colonne
			ChildNameNodeColumn.Title = param.ChildTVColumn1_Nod;
			ChildIpNodeColumn.Title = param.ChildTVColumn2_Nod;
			ChildMacNodeColumn.Title = param.ChildTVColumn3_Nod;
			ChildDHCPNodeColumn.Title = param.ChildTVColumn4_Nod;
			ChildIdNodeColumn.Title = param.ChildTVColumn5_Nod;
			
			//Visibilité des colonnes
			ChildIdNodeColumn.Visible = false;
			
			//Création des cellules
			ChildIdNodeCell = new Gtk.CellRendererText ();
			ChildNameNodeCell = new Gtk.CellRendererText ();
			ChildIpNodeCell = new Gtk.CellRendererText ();	
			ChildMacNodeCell = new Gtk.CellRendererText ();
			ChildDHCPNodeCell = new Gtk.CellRendererToggle ();
				
			//Les Cellules sont éditable
			ChildIdNodeCell.Editable = false;
			ChildNameNodeCell.Editable = true;
			ChildIpNodeCell.Editable = true;
			ChildMacNodeCell.Editable = true;
			ChildDHCPNodeCell.Activatable = true;
					
			//On appel la fonction pour le nom
			ChildNameNodeCell.Edited += ChildNameNodeValueCell_Edited;
			//On appel la fonction pour l'adresse IP
			ChildIpNodeCell.Edited += ChildIPNodeValueCell_Edited;
			//On appel la fonction pour l'adresse MAC
			ChildMacNodeCell.Edited += ChildMacNodeValueCell_Edited;
			//On appel la fonction pour la checkBox DHCP
			ChildDHCPNodeCell.Toggled += ChildDHCPNodeCell_Toggled;
			
			//Mettre les box à gauche
			ChildDHCPNodeCell.Xalign = 0;

			//On associe des cellule au colonne
			ChildIdNodeColumn.PackStart(ChildIdNodeCell,true);
			ChildNameNodeColumn.PackStart(ChildNameNodeCell, true);
			ChildIpNodeColumn.PackStart (ChildIpNodeCell, true);
			ChildMacNodeColumn.PackStart (ChildMacNodeCell, true);
			ChildDHCPNodeColumn.PackStart (ChildDHCPNodeCell, true);
			
			//Ajout des colonnes dans ExplorerTreeView
			TreeViewChild.AppendColumn (ChildNameNodeColumn);
			TreeViewChild.AppendColumn (ChildIpNodeColumn);	
			TreeViewChild.AppendColumn (ChildMacNodeColumn);
			TreeViewChild.AppendColumn (ChildDHCPNodeColumn);	
			TreeViewChild.AppendColumn (ChildIdNodeColumn);
			TreeViewChild.AppendColumn (ChildEmptyColumn);
			
			//Ajout des attibuts à chaque colonne
			ChildNameNodeColumn.AddAttribute(ChildNameNodeCell,"text",param.ChildTvPositionName_Nod);
			ChildIpNodeColumn.AddAttribute (ChildIpNodeCell,"text",param.ChildTvPositionIP_Nod);
			ChildMacNodeColumn.AddAttribute (ChildMacNodeCell,"text",param.ChildTvPositionMAC_Nod);
			ChildDHCPNodeColumn.AddAttribute (ChildDHCPNodeCell,"active",param.ChildTvPositionDHCP_Nod);
			ChildIdNodeColumn.AddAttribute(ChildIdNodeCell,"text",param.ChildTvPositionId_Nod);
			
			//Création d'un nouveau store, param :texte Id texte Name, texte IP, bool DHCP
			ChildListStore = new Gtk.TreeStore(typeof(string),typeof(string),typeof(string),typeof(bool),typeof(string));			
		
			//Visibilité des lignes et des colonnes
			TreeViewChild.EnableGridLines = TreeViewGridLines.Both;
			
			//On écrit le store dans le treeview
			TreeViewChild.Model = ChildListStore;
			this.ShowAll ();	
		}
		
		//Fonction UpdateTreeViewChildOnProject
		//Fcontion permettant de faire la mise à jour du TreeviewChild losque nous sommes sur un projet
		public void UpdateTreeViewChildOnProject()
		{
			ChildListStore.Clear();
			foreach(Project Pro in datamanagement.ListProject)
			{	
				if(Pro.Project_Id == Id)
				{
					foreach(Node node in Pro.ReturnListNode())
					{
						IterChild = ChildListStore.AppendValues(node.Node_Name,node.Node_IP,node.Node_Mac,node.Node_DHCP,node.Node_Id.ToString());
					}
				}
			}
			TreeViewChild.Model = ChildListStore;
			TreeViewChild.ShowAll();
		}		
		
				//Fonction ChildNameNodeValueCell_Edited
				//Fonction permettant de mettre à jour les données lors d'un clic dans la cellule NOM dans le child treeview		
				private void ChildNameNodeValueCell_Edited (object o, Gtk.EditedArgs args)
				{
					string IdSelected = "";		//variable permettant de stocker l'id selectionné
					string NameSelected = "";	//variable permettant de stocker le nom selectionné
			
					TreeSelection selection = TreeViewChild.Selection; //Nous allons cree un arbre de selection
					if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
					{
						IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ChildTvPositionId_Nod); //Nous retournons l'id du noeud selectionné
						NameSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ChildTvPositionName_Nod); //Nous retournons le nom du noeud selectionné		
					}	 
			
					if(NameSelected != args.NewText) //Permet de verifier si on fait un changment dans le nom
					{
						if(args.NewText != "") //Si le texte est pas vide
						{
							if(args.NewText.Length <= param.CarSizeMax)
							{
								datamanagement.ModifyNode(args.NewText,param.ChildTvPositionName_Nod,Convert.ToInt32(IdSelected));//On modifie le nom
								UpdateTreeViewChildOnProject(); //On met à jour le PropertiesTreeview
								datamanagement.mainwindow.UpdateEplorerTreeView(); //On met à jour ExplorerTreeView
							}
							else //Dans le cas ou le nouveuau nom est supérieur à 16 caratère
							{
								datamanagement.mainwindow.AddLineOutput(param.OutputError,param.NameNodeTooLong); //on affiche un message d'erreur
							}
						}
						else //Dans le cas ou le texte est vide
						{
							datamanagement.mainwindow.AddLineOutput(param.OutputError,param.NameNodeEmpty); //on affiche un message d'erreur
						}
					}
				}		
		
				//Fonction ChildIPNodeValueCell_Edited
				//Fonction permettant de mettre à jour les données lors d'un clic dans la cellule L'ADRESSE IP dans le child treeview
				private void ChildIPNodeValueCell_Edited (object o, Gtk.EditedArgs args)
				{
					string IdSelected = "";		//variable permettant de stocker l'id selectionné
					string IpSelected = "";	//variable permettant de stocker le nom selectionné
			
					TreeSelection selection = TreeViewChild.Selection; //Nous allons cree un arbre de selection
					if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
					{
						IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ChildTvPositionId_Nod); //Nous retournons l'id du noeud selectionné
						IpSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ChildTvPositionIP_Nod); //Nous retournons le nom du noeud selectionné		
					}	 
			
					if(IpSelected != args.NewText) //Permet de verifier si on fait un changment dans le nom
					{
						if(args.NewText != "") //Si le texte est pas vide
						{
							datamanagement.ModifyNode(args.NewText,param.ChildTvPositionIP_Nod,Convert.ToInt32(IdSelected));//On modifie le nom
							UpdateTreeViewChildOnProject(); //On met à jour le PropertiesTreeview
							datamanagement.mainwindow.UpdateEplorerTreeView(); //On met à jour ExplorerTreeView
						}
						else //Dans le cas ou le texte est vide
						{
							datamanagement.mainwindow.AddLineOutput(param.OutputError,param.IPNodeInvalid); //on affiche un message d'erreur
						}
					}
				}					
		
				//Fonction ChildMacNodeValueCell_Edited
				//Fonction permettant la modification de l'adresse mac d'un noeud 
				private void ChildMacNodeValueCell_Edited (object o, Gtk.EditedArgs args)
				{
					string IdSelected = "";
			
					TreeSelection selection = TreeViewChild.Selection; //Nous allons cree un arbre de selection
					if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
					{
						IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ChildTvPositionId_Nod); //Nous retournons l'id du noeud selectionné
					}
					datamanagement.ModifyNode("", param.ChildTvPositionMAC_Nod,Convert.ToInt32(IdSelected));//On modifie l'adresse mac en automatique
					UpdateTreeViewChildOnProject(); //On met à jour le PropertiesTreeview
				}
		
				//Fonction ChildDHCPNodeCell_Toggled
				//Fonction faite lors d'un clic sur un check button
				private void ChildDHCPNodeCell_Toggled (object o, Gtk.ToggledArgs args) 
				{
						bool Value = false; //Initialisation d'un bool permettant de récuperer la valeur lors d'un clic
						Gtk.TreeIter iter;
						if (ChildListStore.GetIter (out iter, new Gtk.TreePath(args.Path))) {
							Value = (bool) ChildListStore.GetValue(iter,param.ChildTvPositionDHCP_Nod); //on fait l'acquisition de la valeur
							ChildListStore.SetValue(iter,param.ChildTvPositionDHCP_Nod,!Value);//On affiche la valeur inversé
						}	
				}
		
		//Fonction OnTextViewNoteKeyReleaseEvent
		//Fonction permettant d'enregitrer des commentaire lors de la saise de texte sur TextView
		protected void OnTextViewNoteKeyReleaseEvent (object o, Gtk.KeyReleaseEventArgs args)
		{
			datamanagement.ModifyProject(TextViewNote.Buffer.Text,param.Note_Pro,Id); //Modification des notes du projet
		}
		
		
		
		//OnTreeViewChildButtonReleaseEvent
		//Fonction permettant de faire des action sur clic dans le treeview
		//Cette fonction va notemment nous servir lorsque nous cliquons sur une box pour enregistrer les nouvelle valeur
		protected void OnTreeViewChildButtonReleaseEvent (object o, Gtk.ButtonReleaseEventArgs args)
		{
			if(Type == param.ExTVTypeProject)
			{
				bool ChildDHCPSelect = false; //Variable permettant de stocker la valeur DHCP que nous allons récuperer
				string ChildIDSelect = ""; //Variable permettant de stocker la valeur Id que nous allons récuperer
				TreeSelection selection = (o as TreeView).Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					ChildDHCPSelect = (bool) TreeModelChildTreeView.GetValue (IterChild, param.ChildTvPositionDHCP_Nod); //Nous récuperons la valeur que nous stockons
					ChildIDSelect = (string) TreeModelChildTreeView.GetValue (IterChild, param.ChildTvPositionId_Nod); //Nous récuperons la valeur que nous stockons
					
					foreach(Project Pro in datamanagement.ListProject) //Pour chaque projet 
					{
						foreach(Node node in Pro.ReturnListNode()) //pour chaque noeud
						{
							if(node.Node_Id == Convert.ToInt32(ChildIDSelect) && node.Node_DHCP != ChildDHCPSelect) //Si l'id est égale mais que la valeur DHCP est différente
							{
								datamanagement.ModifyNode(ChildDHCPSelect.ToString(),param.ChildTvPositionDHCP_Nod,Convert.ToInt32(ChildIDSelect)); //On fait la mise à jour du DHCP
							}
						}
					}
				}				
			}
		}
*/		
	}
}