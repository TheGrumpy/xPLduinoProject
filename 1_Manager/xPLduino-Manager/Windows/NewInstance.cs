using System;
using Gtk;

namespace xPLduinoManager
{
	//Classe NewNInstance
	//Classe permettant d'ajouter une nouvelle instance à un noeud
	//Eléments :
	//	DataManagement datamanagement : Permet d'utiliser les données du datamanagement
	//	Int32 NodeId : Id du noeud ou nous devons ajouter un réseau
	//Fonctions :
	//	InitNewInstance : Fonction permettant de mettre à jour les libellé de la page
	//	OnButtonCancelClicked : Destruction de la fenetre sur appui du bouton annuler
	//	OnButtonOkClicked : Ajout d'un nouveau réseau losqu'on appuie sur le bouton OK	
	public partial class NewInstance : Gtk.Dialog
	{
		public DataManagement datamanagement;
		public Param param;
		public Int32 NodeId;		
		
		//Constructeur de la classe NewInstance
		//Arguments :
		//	DataManagement _datamanagement : Permet d'utiliser les données du datamanagement
		//	Int32 _NodeId : Id du noeud ou nous devons ajouter un réseau		
		public NewInstance (DataManagement _datamanagement, Int32 _NodeId, Param _param)
		{
			this.datamanagement = _datamanagement;
			this.param = _param;
			this.NodeId = _NodeId;			
			this.Build ();
			InitNNewInstance();

			ComboboxTypeInstance.AppendText(param.ParamT("ExTVNameLighting"));
			ComboboxTypeInstance.AppendText(param.ParamT("ExTVNameSwitch"));
			ComboboxTypeInstance.AppendText(param.ParamT("ExTVNameShutter"));
		}
		
		//Fonction InitNewInstance
		//Fonction permettant d'initialiser la page new board
		public void InitNNewInstance()
		{
			this.Title = param.ParamT("NITitle");
			LabelName.Text = param.ParamT("NILabelInstanceName");
			EntryInstanceName.Text = param.ParamT("NIDefaultInstanceName");
			LabelType.Text = param.ParamT("NILabelInstanceType");
			buttonOk.Label = param.ParamT("NIButtonOK");
			buttonCancel.Label = param.ParamT("NIButtonCancel");
		}
				
		
		protected void OnButtonCancelClicked (object sender, System.EventArgs e)
		{
			this.Destroy();
			datamanagement.mainwindow.Sensitive = true; //Activation de la fenetre principale
		}

		protected void OnButtonOkClicked (object sender, System.EventArgs e)
		{
			//Récuperer la valeur de la Combobox
			TreeIter tree = new TreeIter();
			string SelectedValue = "";
			string InstanceValue = "";
			ComboboxTypeInstance.GetActiveIter(out tree);	
			
			SelectedValue = (String) ComboboxTypeInstance.Model.GetValue (tree, 0);
			
			string _InstanceName = datamanagement.ReturnNewNameInstance(EntryInstanceName.Text,NodeId);
			
			if(SelectedValue == param.ParamT("ExTVNameLighting"))
			{
				InstanceValue = param.ParamP("InstLightingName");
			}
			else if(SelectedValue == param.ParamT("ExTVNameSwitch"))
			{
				InstanceValue = param.ParamP("InstSwitchName");
			}	
			else if(SelectedValue == param.ParamT("ExTVNameShutter"))
			{
				InstanceValue = param.ParamP("InstShutterName");
			}			
					
			if(_InstanceName != EntryInstanceName.Text.Replace(" ","_")) //Si le nouveau nom est différent de l'ancien
			{
				LabelError.Text = EntryInstanceName.Text.Replace(" ","_") + param.ParamT("NIInstanceExiste"); //on indique un message d'erreur
				EntryInstanceName.Text = _InstanceName; //On met un nouveau nom dans le cellule
			}	
			else if(_InstanceName == "") //Si la cellule est vide
			{
				LabelError.Text = param.ParamT("NBEmptyName"); //On indique un message d'erreur
				EntryInstanceName.Text = param.ParamT("NIDefaultInstanceName"); //On remplit la cellule avec un nom par défaut
			}
			else if(SelectedValue == null)
			{
				LabelError.Text = param.ParamT("NIChooseInstance");
			}
			else //Sinon
			{
				datamanagement.AddInstanceInNode(InstanceValue,EntryInstanceName.Text,NodeId);
				datamanagement.mainwindow.Sensitive = true; //Activation de la fenetre principale
				this.Destroy();
			}					
		}

		protected void OnDeleteEvent (object o, Gtk.DeleteEventArgs args)
		{
			datamanagement.mainwindow.Sensitive = true; //Activation de la fenetre principale
		}
	}
}