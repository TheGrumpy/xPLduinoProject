using System;
using Gtk;

namespace xPLduinoManager
{
	//Classe NewBoard
	//Classe permettant d'afficher une nouvelle pour intégrer une nouvelle carte
	//Eléments :
	//	DataManagement datamanagement : Permet d'utiliser les données du datamanagement
	//	Int32 NetworkId : Id du réseau ou nous devons ajouter une carte
	//Fonctions :
	//	OnButtonCancelClicked : Destruction de la fenetre sur appui du bouton annuler
	//	OnButtonOkClicked : Ajout d'un nouveau réseau losqu'on appuie sur le bouton OK	
	//	InitNewBoard : Mise à jour des libéle de la page
	public partial class NewBoard : Gtk.Dialog
	{
		public DataManagement datamanagement;
		public Int32 NetworkID;	
		public string NetworkType;
		public Param param;
		
		//Constructeur de la classe NewBoard
		//Arguments :
		//	DataManagement _datamanagement : permet de gérer les données du datamangement
		//	Int32 _NetworkID : on envoie en paramètre l'id du réseau
		//	string _NetworkType : Type du réseau
		public NewBoard (DataManagement _datamanagement, Int32 _NetworkID, string _NetworkType, Param _param)
		{
			this.datamanagement = _datamanagement;
			this.param = _param;
			this.NetworkID = _NetworkID;
			this.NetworkType = _NetworkType;
			this.Build ();
			this.InitNewBoard();
			
			foreach(Boards boards in datamanagement.ListBoards) //Pour chaque carte de la liste des carte
			{
				if(boards.NetworkType == NetworkType) //Si une carte est de type envoyé en paramètre
				{
					ComboboxTypeBoard.AppendText(boards.Type); //Nous affichons la carte dans la liste					
				}
			}
		}
		
		//Fonction InitNewBoard
		//Fonction permettant d'initialiser la page new board
		public void InitNewBoard()
		{
			this.Title = param.ParamT("NBTitle");
			LabelName.Text = param.ParamT("NBLabelBoardName");
			EntryBoardName.Text = param.ParamT("NBDefaultBoardName");
			LabelType.Text = param.ParamT("NBLabelBoardType");
			buttonOk.Label = param.ParamT("NBButtonOK");
			buttonCancel.Label = param.ParamT("NBButtonCancel");
		}
		
		//Fonction OnButtonCancelClicked
		//Fonction permettant de détruire la fenêtre sur appuie du bouton annuler		
		protected void OnButtonCancelClicked (object sender, System.EventArgs e)
		{
			datamanagement.mainwindow.Sensitive = true; //Activation de la fenetre principale
			this.Destroy();
		}		

		//Fonction OnButtonOkClicked
		//Fonction permettant d'ajouter une carte sur appuie du bouton OK
		protected void OnButtonOkClicked (object sender, System.EventArgs e)
		{
			//Récuperer la valeur de la Combobox
			TreeIter tree = new TreeIter();
			string SelectedValue = "";
			ComboboxTypeBoard.GetActiveIter(out tree);	
			SelectedValue = (String) ComboboxTypeBoard.Model.GetValue (tree, 0);
				
			string _BoardName = datamanagement.ReturnNewNameBoard(EntryBoardName.Text.Replace(" ","_"),NetworkID);
			
			if(_BoardName != EntryBoardName.Text.Replace(" ","_")) //Si le nouveau nom est différent de l'ancien
			{
				LabelError.Text = EntryBoardName.Text.Replace(" ","_") + param.ParamT("NBBoardExiste"); //on indique un message d'erreur
				EntryBoardName.Text = _BoardName; //On met un nouveau nom dans le cellule
			}	
			else if(_BoardName == "") //Si la cellule est vide
			{
				LabelError.Text = param.ParamT("NBEmptyName"); //On indique un message d'erreur
				EntryBoardName.Text = param.ParamT("NBDefaultBoardName"); //On remplit la cellule avec un nom par défaut
			}
			else if(SelectedValue == null)
			{
				LabelError.Text = param.ParamT("NBChooseBoard");
			}
			else //Sinon
			{
				datamanagement.AddBoardInNetwork(SelectedValue,_BoardName,NetworkID); //On cree une nouvelle carte dans un réseau
				this.Destroy(); //On détruit la fenetre en cours
			}				
		}
		
		//Fonction OnDeleteEvent
		//Sur destruction de la fenêtre
		protected void OnDeleteEvent (object o, Gtk.DeleteEventArgs args)
		{
			datamanagement.mainwindow.Sensitive = true; //Activation de la fenetre principale
		}
	}
}
