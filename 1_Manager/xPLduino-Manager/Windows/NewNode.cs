using System;
using System.IO;
using xPLduinoManager;

namespace xPLduinoManager
{
	//Classe NewNode
	//Cette classe permet de gerer la fenêtre nouveau noeud
	//Eléments :
	//	DataManagement datamanagement : Permet d'utiliser les information du datamanagement
	//Fonctions :
	//	InitNewNode : fonction permettant d'initialiser la fenêtre
	//	OnButtonCancelClicked destruction de la fenetre sur appuie du bouton Annulation
	//	OnButtonOkClicked : permet d'effectuer des actions lors de l'appui du bouton OK
	//	OnDeleteEvent : Permet d'effectuer des action sur clic cross
	public partial class NewNode : Gtk.Dialog
	{
		public DataManagement datamanagement;
		public Param param;
		public Int32 Project_Id;
		
		//Constructeur de la classe NewNode
		//Arguments :
		//	DataManagement _datamanagement : Permet d'utiliser les données de datamanagement
		//	Int32 _Project_Id : on vient creer un noeud dans le projet portant cette Id
		public NewNode (DataManagement _datamanagement, Int32 _Project_Id, Param _param)
		{
			this.datamanagement = _datamanagement;
			this.param = _param;
			this.Project_Id = _Project_Id;
			this.Build ();
			InitNewNode();
		}
		
		//Fonction InitNewNode
		//Fonction permettant d'initialiser la fenetre
		public void InitNewNode()
		{
			this.Title = param.ParamT("NNTitle");
			LabelNodeName.Text = param.ParamT("NNLabelNodeName");
			buttonOk.Label = param.ParamT("NNButtonOK");
			buttonCancel.Label = param.ParamT("NNButtonCancel");
			EntryNodeName.Text = param.ParamT("NNDefaultNodeName");
		}
		
		//Fonction OnButtonCancelClicked
		//Fonction permettant de détruire la fenêtre en appuyant sur le bouton annuler
		protected void OnButtonCancelClicked (object sender, System.EventArgs e)
		{
			datamanagement.mainwindow.Sensitive = true;	//Activation de la fenetre principale
			this.Destroy();
		}
		
		//Fonction OnButtonOkClicked
		//Fonction permettant d'enregistrer le nouveau noeud dans un projet
		protected void OnButtonOkClicked (object sender, System.EventArgs e)
		{
			string _NodeName = datamanagement.ReturnNewNameNode(EntryNodeName.Text.Replace(" ","_"),Project_Id); //Nous allons verifier que le nom existe pas sinon nous le renommons grace à la fonction
			if(_NodeName != EntryNodeName.Text.Replace(" ","_")) //Si le nouveau nom est différent de l'ancien
			{
				LabelError.Text = EntryNodeName.Text.Replace(" ","_") + param.ParamT("NNNodeExiste"); //on indique un message d'erreur
				EntryNodeName.Text = _NodeName; //On met un nouveau nom dans le cellule
			}	
			else if(_NodeName == "") //Si la cellule est vide
			{
				LabelError.Text = param.ParamT("NNEmptyName"); //On indique un message d'erreur
				EntryNodeName.Text = param.ParamT("NNDefaultNodeName"); //On remplit la cellule avec un nom par défaut
			}
			else //Sinon
			{
				datamanagement.AddNodeInProject(_NodeName,Project_Id); //On cree un nouveau noeud dans le projet
				this.Destroy(); //On détruit la fenetre en cours
			}			
		}
		
		//Fonction OnDeleteEvent
		//Fonction permettant de faire des actions sur clic cross
		protected void OnDeleteEvent (object o, Gtk.DeleteEventArgs args)
		{
			datamanagement.mainwindow.Sensitive = true; //Activation de la fenetre principale
		}
	}
}

