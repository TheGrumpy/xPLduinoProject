using System;
using System.IO;
using xPLduinoManager;

namespace xPLduinoManager
{
	//Classe NewProject
	//Cette classe permet de gerer la fenêtre nouveau projet
	//Eléments :
	//	DataManagement datamanagement : Permet d'utiliser les information du datamanagement
	//Fonctions :
	//	OnButtonCancelClicked : fonction permettant de détruire la fenêtre sur appui de bouton cancel
	//	OnButtonOkClicked : Permet d'effectuer des action lors d'un clic sur le bouton OK
	//	OnEntryNameProjectKeyReleaseEvent :	Permet d'effection des action lors d'une saisie clavier sur la 
	//	OnDeleteEvent : Permet d'effectuer des action sur clic cross	
	public partial class NewProject : Gtk.Dialog
	{
		public DataManagement datamanagement;
		public Param param;
		
		//Constructeur NewProject
		//Arguments :
		//	DataManagement _datamanagement : permet d'utiliser les fonctions de la classe DataManagement
		//Fonctions :
		//	OnButtonCancelClicked : Action à faire sur bouton annuler
		//	OnButtonOkClicked : Action à faire sur bouton OK
		//	OnEntryNameProjectKeyReleaseEvent : Action à faire sur appuie bouton clavier dans la case nom de projet
		//	OnDeleteEvent : Action à faire sur ClicCross
		public NewProject (DataManagement _datamanagement, Param _param)
		{
			this.datamanagement = _datamanagement;
			this.param = _param;
			this.Build ();
			InitNewProject();
		}
		
		//Fonction InitNewProject
		//Fonction permettant d'initialiser les textes dans cette fenêtre
		public void InitNewProject()
		{
			this.Title = param.ParamT("NPTitle");
			LabelProjectName.Text = param.ParamT("NPLabelProjectName");
			LabelAuhorName.Text = param.ParamT("NPLabelProjectAuthor");
			LabelPath.Text = param.ParamT("NPLabelProjectPath");	
			buttonOk.Label = param.ParamT("NPButtonOk");
			buttonCancel.Label = param.ParamT("NPButtonCancel");
			EntryNameProject.Text = param.ParamT("NPDefaultProjectName");
			EntryAuthorProject.Text =param.ParamT("NPDefaultProjectAuthor");
		}
		
		//Fonction OnButtonCancelClicked
		//Fonction perttant de détruire la fenetre sur appui du bouton annuler
		protected void OnButtonCancelClicked (object sender, System.EventArgs e)
		{
			datamanagement.mainwindow.Sensitive = true; //Activation de la fenetre principale
			this.Destroy();
		}

		//Fonction OnButtonOkClicked
		//Fonction permettant de faire des action sur l'appui enregistrement
		protected void OnButtonOkClicked (object sender, System.EventArgs e)
		{
			bool ErrorData = false;
			string _ProjectName = EntryNameProject.Text.Replace(" ","_"); //On enleve les espaces dans le nom

			if(_ProjectName == "")	//Sinon on verifie que la case est bien pleine
			{
				LabelError.Text = param.ParamT("NPNameEmpty"); //Message d'erreur
				EntryNameProject.Text = param.ParamT("NPDefaultProjectName"); //On met un texte dans la cellule
			}
			else 
			{
				if(!File.Exists(ButtonChooseFolder.Filename + "/" + _ProjectName + param.ParamP("ExtensionFile"))) //On verifie que le projet existe pas au chemin indiqué par l'utilisateur
				{
					foreach(Project Pro in datamanagement.ListProject)
					{
						if(Pro.Project_Name == _ProjectName && ButtonChooseFolder.Filename == Pro.Project_SavePath)
						{
							LabelError.Text = param.ParamT("NPOtherProjectExist");
							ErrorData = true;
						}
					}
					if(!ErrorData)
					{
						datamanagement.CreateNewProject(_ProjectName,EntryAuthorProject.Text,ButtonChooseFolder.Filename); //Si le fichier existe pas on peut le creé
						datamanagement.mainwindow.Sensitive = true; //Activation de la fenetre principale
						datamanagement.mainwindow.InitPanedAndMouvementAuthor();
						this.Destroy(); //Puis nous détruisons la fenêtre
					}
				}
				else //Sinon
				{
					LabelError.Text = _ProjectName + param.ParamT("NPProjectExistInList"); //Nous indiquons le message d'erreur
				}
			}
			
		}
			
		//Fonction OnEntryNameProjectKeyReleaseEvent
		//Fonction permttant de vider LabelError lors d'un appuie bouton dans la cellule du nom
		protected void OnEntryNameProjectKeyReleaseEvent (object o, Gtk.KeyReleaseEventArgs args)
		{
			LabelError.Text = "";
		}

		//Fonction OnDeleteEvent
		//Fonction permettant de faire des actions sur clic cross
		protected void OnDeleteEvent (object o, Gtk.DeleteEventArgs args)
		{
			datamanagement.mainwindow.Sensitive = true; //Activation de la fenetre principale
		}
		
	}
}

