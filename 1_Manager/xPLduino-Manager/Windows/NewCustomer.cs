using System;
using System.Collections.Generic;

namespace xPLduinoManager
{
	public partial class NewCustomer : Gtk.Dialog
	{
		public DataManagement datamanagement;
		public Param param;
		public Int32 NodeId;	
		
		public NewCustomer (DataManagement _datamanagement, Int32 _NodeId, Param _param)
		{
			this.Build ();
			this.param = _param;
			this.datamanagement = _datamanagement;
			this.NodeId = _NodeId;
			InitNewCustomer();
		}
		
		//Fonction InitNewCustomer
		//Fonction permettant d'initialiser le widget
		void InitNewCustomer()
		{
			this.Title = param.ParamT("NCTitle");
			LabelNameCustomer.Text = param.ParamT("NCLabelCustomerName");
			EntryCustomerName.Text = param.ParamT("NCDefaultCustomerName");
			buttonOk.Label = param.ParamT("NCButtonOK");
			buttonCancel.Label = param.ParamT("NCButtonCancel");			
		}
			
		
		protected void OnButtonCancelClicked (object sender, System.EventArgs e)
		{
			this.Destroy();
			datamanagement.mainwindow.Sensitive = true; //Activation de la fenetre principale
		}

		protected void OnButtonOkClicked (object sender, System.EventArgs e)
		{
			string _CustomerName = datamanagement.ReturnNewNameCustomer(EntryCustomerName.Text,NodeId);
			string _OldName = EntryCustomerName.Text;
			int SizeString =  _OldName.Length;	
			if(_OldName.Substring(SizeString - 4,4) != ".ino")
			{
				_OldName = _OldName.Replace(".","_");
				_OldName = _OldName.Replace(" ","_");
				_OldName = _OldName + ".ino";
			}
			else
			{
				_OldName = _OldName.Substring(0,SizeString - 4);
				_OldName = _OldName.Replace(".","_");
				_OldName = _OldName.Replace(" ","_");
				_OldName = _OldName + ".ino";
			}		
			
			if(_CustomerName != _OldName) //Si le nouveau nom est différent de l'ancien
			{
				LabelError.Text = _OldName + param.ParamT("NCCustomerExiste"); //on indique un message d'erreur
				EntryCustomerName.Text = _CustomerName; //On met un nouveau nom dans le cellule
			}	
			else if(_CustomerName == "") //Si la cellule est vide
			{
				LabelError.Text = param.ParamT("NCEmptyName"); //On indique un message d'erreur
				EntryCustomerName.Text = param.ParamT("NCDefaultCustomerName"); //On remplit la cellule avec un nom par défaut
			}
			else //Sinon
			{
				datamanagement.AddCustomerInNode(_CustomerName,NodeId,true);
				this.Destroy(); //On détruit la fenetre en cours
			}				
		}

		protected void OnDeleteEvent (object o, Gtk.DeleteEventArgs args)
		{
			datamanagement.mainwindow.Sensitive = true; //Activation de la fenetre principale
		}		

	}
}

