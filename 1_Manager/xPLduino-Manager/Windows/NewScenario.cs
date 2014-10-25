using System;
using System.Collections.Generic;

namespace xPLduinoManager
{
	public partial class NewScenario : Gtk.Dialog
	{
		public DataManagement datamanagement;
		public Param param;
		public Int32 NodeId;	
		
		public NewScenario (DataManagement _datamanagement, Int32 _NodeId, Param _param)
		{
			this.Build ();
			this.param = _param;
			this.datamanagement = _datamanagement;
			this.NodeId = _NodeId;
			InitNewScenario();
		}
		
		//Fonction InitNewCustomer
		//Fonction permettant d'initialiser le widget
		void InitNewScenario()
		{
			this.Title = param.ParamT("NSTitle");
			LabelNameScenario.Text = param.ParamT("NSLabelScenarioName");
			EntryScenarioName.Text = param.ParamT("NSDefaultScenarioName");
			buttonOk.Label = param.ParamT("NSButtonOK");
			buttonCancel.Label = param.ParamT("NSButtonCancel");			
		}
			
		
		protected void OnButtonCancelClicked (object sender, System.EventArgs e)
		{
			this.Destroy();
			datamanagement.mainwindow.Sensitive = true; //Activation de la fenetre principale
		}

		protected void OnButtonOkClicked (object sender, System.EventArgs e)
		{
			string _ScenarioName = datamanagement.ReturnNewNameScenario(EntryScenarioName.Text,NodeId);
			string _OldName = EntryScenarioName.Text;

			
			if(_ScenarioName != _OldName) //Si le nouveau nom est différent de l'ancien
			{
				LabelError.Text = _OldName + param.ParamT("NCScenarioExiste"); //on indique un message d'erreur
				EntryScenarioName.Text = _ScenarioName; //On met un nouveau nom dans le cellule
			}	
			else if(_ScenarioName == "") //Si la cellule est vide
			{
				LabelError.Text = param.ParamT("NCEmptyName"); //On indique un message d'erreur
				EntryScenarioName.Text = param.ParamT("NSDefaultScenarioName"); //On remplit la cellule avec un nom par défaut
			}
			else //Sinon
			{
				datamanagement.AddScenarioInNode(_ScenarioName,NodeId,true);
				this.Destroy(); //On détruit la fenetre en cours
			}				
		}

		protected void OnDeleteEvent (object o, Gtk.DeleteEventArgs args)
		{
			datamanagement.mainwindow.Sensitive = true; //Activation de la fenetre principale
		}		

	}
}

