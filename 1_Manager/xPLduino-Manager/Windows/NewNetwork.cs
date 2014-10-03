using System;
using Gtk;

namespace xPLduinoManager
{
	//Classe NewNetwork
	//Classe permettant d'ajouter un nouveau réseau à un noeud
	//Eléments :
	//	DataManagement datamanagement : Permet d'utiliser les données du datamanagement
	//	Int32 NodeId : Id du noeud ou nous devons ajouter un réseau
	//Fonctions :
	//	InitNewNetwork : Fonction permettant de mettre à jour les libellé de la page
	//	OnButtonCancelClicked : Destruction de la fenetre sur appui du bouton annuler
	//	OnButtonOkClicked : Ajout d'un nouveau réseau losqu'on appuie sur le bouton OK
	public partial class NewNetwork : Gtk.Dialog
	{
		public DataManagement datamanagement;
		public Param param;
		public Int32 NodeId;
		
		//Constructeur de la classe NewNetwork
		//Arguments :
		//	DataManagement _datamanagement : Permet d'utiliser les données du datamanagement
		//	Int32 _NodeId : Id du noeud ou nous devons ajouter un réseau
		public NewNetwork (DataManagement _datamanagement, Int32 _NodeId, Param _param)
		{
			this.datamanagement = _datamanagement;
			this.param = _param;
			this.NodeId = _NodeId;
			this.Build ();
			InitNewNetwork();
			
			ComboBoxNetworks.AppendText(param.ParamP ("I2CType"));
			ComboBoxNetworks.AppendText(param.ParamP ("OneWireType"));
			ComboBoxNetworks.AppendText(param.ParamP ("RS485Type"));
	
		}
		
		//Fonction InitNewNetwork
		//Fonction permettant de mettre à jour les libélé de la page
		public void InitNewNetwork()
		{
			this.Title = param.ParamT("NNWTitle");
			LabelNetworks.Text = param.ParamT("NNWLabelNetworkName");
			buttonOk.Label = param.ParamT("NNWButtonOK");
			buttonCancel.Label = param.ParamT("NNWButtonCancel");
		}
		
		//Fonction OnButtonCancelClicked
		//Fonction permettant de détruire la fenêtre sur appuie du bouton annuler
		protected void OnButtonCancelClicked (object sender, System.EventArgs e)
		{
			datamanagement.mainwindow.Sensitive = true;	//Activation de la fenetre principal
			this.Destroy();
		}
		
		//Fonction OnButtonOkClicked
		//Fonction permettant d'ajouter un réseau sur appuie du bouton OK
		protected void OnButtonOkClicked (object sender, System.EventArgs e)
		{
			TreeIter tree = new TreeIter();
			string SelectedValue = "";
			
 			ComboBoxNetworks.GetActiveIter(out tree);	
			SelectedValue = (String) ComboBoxNetworks.Model.GetValue (tree, 0);
			
			if(SelectedValue != null)
			{
				datamanagement.mainwindow.Sensitive = true;	//Activation de la fenetre principale
				datamanagement.AddNetworkInNode(SelectedValue,NodeId);
				this.Destroy();
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

