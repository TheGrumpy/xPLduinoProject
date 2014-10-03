using System;
using Gtk;
using Gdk;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace xPLduinoManager
{
	public partial class ChangePreferences : Gtk.Dialog
	{
		public Param param;
		public MainWindow mainwindow;
		public DataManagement datamanagement;
		public Preference pref;
		
		private FileStream fileStream;
		private StreamWriter sw;
		
		public ChangePreferences (Param _Param, MainWindow _Mainwindow, Preference _Pref, DataManagement _datamanagement)
		{
			this.Build ();
			GeneralNoteBook.CurrentPage = 0;
			this.param = _Param;
			this.mainwindow = _Mainwindow;
			this.datamanagement = _datamanagement;
			this.pref = _Pref;
			InitWindows();
			
		}
		
		public void InitWindows()
		{
			this.Title = param.ParamT("PreferenceWindowTitle");
			
			
			//NoteBookGeneral
			GeneralLabelTab.Text = param.ParamT("GeneralLabelTab");
			StartTab.Text = param.ParamT("StartTab");
			VariousTab.Text = param.ParamT("VariousTab");
			
				//Tab Général
				StartLabel.Markup = "<b>" + param.ParamT("StartLabel") + "</b>";
				EndLabel.Markup = "<b>" + param.ParamT("EndLabel") + "</b>";
				
				DisplayWelcomeTab.Label = param.ParamT("DisplayWelcomeTab");
				DisplayWelcomeTab.Active = pref.DisplayWelcomeTab;
				
				ConfirmClose.Label = param.ParamT("ConfirmClose");
				ConfirmClose.Active = pref.ConfirmClose;
				
				//Tab Divers
				VariousLabel.Markup = "<b>" + param.ParamT("VariousLabel") + "</b>";
				
				BeepOnDelete.Label = param.ParamT("BeepOnDelete");
				BeepOnDelete.Active = pref.BeepOnDelete;
			
				BeepOnError.Label = param.ParamT("BeepOnError");
				BeepOnError.Active = pref.BeepOnError;
			
				//Tab Lighting
				LightingLabelTab.LabelProp = param.ParamT("ExTVNameLighting");
				LIGDefaultValueLabel.Markup = "<b>" + param.ParamT("LIGDefaultValueLabel") + "</b>";
			
				DefaultValueLabel.Text = param.ParamT("DefaultValueLabel");
				SpinButtonDefaultValue.Value = pref.LIGDefaultValue;
				
				FadeLabel.Text = param.ParamT("FadeLabel");
				SpinButtonFade.Value = pref.LIGFade;
				
				LIGActionLabel.Markup = "<b>" + param.ParamT("LightingActionName") + "</b>";
				LIGToogleLabel.Text = param.ParamT("ToogleLabel");
				LIGTuneLabel.Text = param.ParamT("TuneLabel");
				LIGStopTuneLabel.Text = param.ParamT("StopTuneLabel");
				LIGSetValueLabel.Text = param.ParamT("SetValueLabel");
			
				LIGEntryToggle.Text = pref.LIGToggleActionName;
				LIGEntryTune.Text = pref.LIGTuneActionName;
				LIGEntryStop.Text = pref.LIGStopActionName;
				LIGEntrySetValue.Text = pref.LIGSetActionName;
			
				//Tab Switch
				SwitchLabelTab.LabelProp = param.ParamT("ExTVNameSwitch");
				SWIDefaultValueLabel.Markup = "<b>" + param.ParamT("SWIDefaultValueLabel") + "</b>";
			
				InverseLabel.Text = param.ParamT("InverseLabel");
				CheckButtonInverse.Active = pref.SWIInverse;
			
				ImpulstionTimeLabel.Text = param.ParamT("ImpulstionTimeLabel");
				SpinButtonImpulsionTime.Value = pref.SWIImpusionTime;				
			
				SWIActionLabel.Markup = "<b>" + param.ParamT("SwitchActionName") + "</b>";
				SWIClicLabel.Text = param.ParamT("ClicLabel");
				SWIDoubleClicLabel.Text = param.ParamT("DoubleClicLabel");
				SWIOnLabel.Text = param.ParamT("OnLabel");
				SWIOnFmLabel.Text = param.ParamT("OnFmLabel");
				SWIOffLabel.Text = param.ParamT("OffLabel");
				SWIOffFmLabel.Text = param.ParamT("OffFmLabel");
			
				SWIEntryClic.Text = pref.SWIClicActionName;
				SWIEntryDoubleClic.Text = pref.SWIDoubleClicActionName;
				SWIEntryOn.Text = pref.SWIOnActionName;
				SWIEntryOnFm.Text = pref.SWIOnFmActionName;
				SWIEntryOff.Text = pref.SWIOffActionName;
				SWIEntryOffFm.Text = pref.SWIOffFmActionName;
								
				//Tab Shutter
				ShutterLabelTab.LabelProp = param.ParamT("ExTVNameShutter");
				SHUDefaultValueLabel.Markup = "<b>" + param.ParamT("SHUDefaultValueLabel") + "</b>";
				ShutterTypeLabel.Text = param.ParamT("SHUTypeLabel");
				TravelTimeLabel.Text = param.ParamT("SHUTravelTimeLabel");
				InitTimeLabel.Text = param.ParamT("SHUInitTimeLabel");
				SHUActionLabel.Markup = "<b>" + param.ParamT("ShutterActionName") + "</b>";
				SHUOpenLabel.Text = param.ParamT("SHUOpenLabel");
				SHUCloseLabel.Text = param.ParamT("SHUCloseLabel");
				SHUStopLabel.Text = param.ParamT("SHUStopLabel");
				SHUToggleLabel.Text = param.ParamT("SHUToggleLabel");
			
					//C'est moche mais ça marche ^^
					List<Int32> values = new List<Int32>(){0, 1, 2};
					ComboboxShutterType.AppendText(param.ParamT("InstShutterType0"));
					ComboboxShutterType.AppendText(param.ParamT("InstShutterType1"));
					ComboboxShutterType.AppendText(param.ParamT("InstShutterType2"));			

					int row = values.IndexOf(pref.SHUType);
					Gtk.TreeIter iter;
					ComboboxShutterType.Model.IterNthChild (out iter, row);
					ComboboxShutterType.SetActiveIter (iter);		
	
		
				SpinButtonTravelTime.Value = pref.SHUTravelTime;
				SpinButtonInitTime.Value = pref.SHUInitTime;
				SHUEntryOpen.Text = pref.SHUOpenActionName;
				SHUEntryClose.Text = pref.SHUCloseActionName;
				SHUEntryStop.Text = pref.SHUStopActionName;		
				SHUEntryToggle.Text = pref.SHUToggleActionName;
				
				//Tab TEMPERATURE
				TemperatureLabelTab.LabelProp = param.ParamT("TemperatureLabelTab");
				TEMPActionLabel.Markup = "<b>" + param.ParamT("TempActionName") + "</b>";
				TEMPGetLabel.Text = param.ParamT("TEMPGetValue");
				
				TEMPEntryGet.Text = pref.TEMPGetValue;
			
			//InterfaceLabelTab
			InterfaceLabelTab.Text = param.ParamT("InterfaceLabelTab");
			InterfaceTab.Text = param.ParamT("InterfaceTab");
			OngletTab.Text = param.ParamT("OngletTab");
			
				//Tab Interface
				LangageLabel.Markup = "<b>" + param.ParamT("LangageLabel") + "</b>";
				if(pref.Langage == param.ParamP ("FrenchLangage"))
				{
					FrenchRadioButton.Active = true;
				}
				else if(pref.Langage == param.ParamP ("EnglishLangage"))
				{
					EnglishRadioButton.Active = true;
				}
		}
		
		//Fonction OnButtonOkClicked
		//Fonction efféctué lors d'un clic sur le bouton OK
		protected void OnButtonOkClicked (object sender, System.EventArgs e)
		{
			if(!ReturnErrorActionName())
			{
				ErrorLabel.Text = param.ParamT("ErrorActionName");
				return;
			}
			
			pref.DisplayWelcomeTab = DisplayWelcomeTab.Active;
			pref.ConfirmClose = ConfirmClose.Active;
			
			using (StringWriter str = new StringWriter())
			using (XmlTextWriter xml = new XmlTextWriter(str))
			{
			    // Root.
			    xml.WriteStartDocument();
				xml.WriteWhitespace("\n");
				
				xml.WriteStartElement("Preferences");
					AddElement(xml,"Preference","DisplayWelcomeTab",DisplayWelcomeTab.Active.ToString(),1);
					AddElement(xml,"Preference","ConfirmClose",ConfirmClose.Active.ToString(),1);
					AddElement(xml,"Preference","BeepOnDelete",BeepOnDelete.Active.ToString(),1);
					AddElement(xml,"Preference","BeepOnError",BeepOnError.Active.ToString(),1);
					AddElement(xml,"Preference","Langage",ReturnLangage(),1);
				
					AddElement(xml,"Preference","LIGDefaultValue",SpinButtonDefaultValue.Text,1);
					AddElement(xml,"Preference","LIGFade",SpinButtonFade.Text,1);
					AddElement(xml,"Preference","LIGToggleActionName",LIGEntryToggle.Text,1);
					AddElement(xml,"Preference","LIGTuneActionName",LIGEntryTune.Text,1);
					AddElement(xml,"Preference","LIGStopActionName",LIGEntryStop.Text,1);
					AddElement(xml,"Preference","LIGSetActionName",LIGEntrySetValue.Text,1);
				
					AddElement(xml,"Preference","SWIInverse",CheckButtonInverse.Active.ToString(),1);
					AddElement(xml,"Preference","SWIImpusionTime",SpinButtonImpulsionTime.Text,1);
					AddElement(xml,"Preference","SWIClicActionName",SWIEntryClic.Text,1);
					AddElement(xml,"Preference","SWIDoubleClicActionName",SWIEntryDoubleClic.Text,1);
					AddElement(xml,"Preference","SWIOnActionName",SWIEntryOn.Text,1);
					AddElement(xml,"Preference","SWIOnFmActionName",SWIEntryOnFm.Text,1);
					AddElement(xml,"Preference","SWIOffActionName",SWIEntryOff.Text,1);
					AddElement(xml,"Preference","SWIOffFmActionName",SWIEntryOffFm.Text,1);				
				
					AddElement(xml,"Preference","SHUType",ComboboxShutterType.Active.ToString(),1);
					AddElement(xml,"Preference","SHUTravelTime",SpinButtonTravelTime.Text,1);
					AddElement(xml,"Preference","SHUInitTime",SpinButtonInitTime.Text,1);
					AddElement(xml,"Preference","SHUOpenActionName",SHUEntryOpen.Text,1);
					AddElement(xml,"Preference","SHUCloseActionName",SHUEntryClose.Text,1);
					AddElement(xml,"Preference","SHUStopActionName",SHUEntryStop.Text,1);	
					AddElement(xml,"Preference","SHUToggleActionName",SHUEntryToggle.Text,1);	
				
					AddElement(xml,"Preference","TEMPGetValue",TEMPEntryGet.Text,1);
					
				xml.WriteWhitespace("\n");
				xml.WriteEndElement();
				
			    xml.WriteEndDocument();							
				
				//Suppression de tous les fichiers du dossier
				//Nous allons vérifier la présence d'un fichier dans le dossier param
				string[] files;
				// pour avoir les noms des fichiers et sous-répertoires
				files = Directory.GetFiles(Environment.CurrentDirectory + param.ParamP("FolderPreference"));
				foreach (string fName in files)
	            {
	                File.Delete(fName);
	            }
				
				
				//Permet d'enregistrer dans un fichier le xml généré
				if(!Directory.Exists(Environment.CurrentDirectory + param.ParamP("FolderPreference"))) 
				{
					 System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + param.ParamP("FolderPreference"));
				}
				
				this.fileStream = new FileStream(Environment.CurrentDirectory + param.ParamP("PreferenceXml"),FileMode.Create);
				this.sw = new StreamWriter(this.fileStream);
				this.sw.WriteLine(str.ToString().Replace("encoding=\"utf-16\"","encoding=\"utf-8\""));	
				this.sw.Close();
				this.fileStream.Close();
				
				//On le réouvre
				string FileXml = System.IO.File.ReadAllText(Environment.CurrentDirectory + param.ParamP("PreferenceXml"));
				string CRCCalcul = CalculHash(FileXml);				
				
				File.Move(Environment.CurrentDirectory + param.ParamP("PreferenceXml"), Environment.CurrentDirectory + param.ParamP("FolderPreference") + "/" + CRCCalcul);
			}
			
			pref.DisplayWelcomeTab = DisplayWelcomeTab.Active;
			pref.ConfirmClose = ConfirmClose.Active;
			pref.BeepOnDelete = BeepOnDelete.Active;
			pref.BeepOnError = BeepOnError.Active;
			pref.Langage = ReturnLangage();
			
			//##### LIGHTING ########
			
			datamanagement.UpdateActionInScenarioAndFunction(pref.LIGToggleActionName,LIGEntryToggle.Text);
			datamanagement.UpdateActionInScenarioAndFunction(pref.LIGTuneActionName,LIGEntryTune.Text);
			datamanagement.UpdateActionInScenarioAndFunction(pref.LIGStopActionName,LIGEntryStop.Text);
			datamanagement.UpdateActionInScenarioAndFunction(pref.LIGSetActionName,LIGEntrySetValue.Text);
			
			pref.LIGDefaultValue = Convert.ToInt32(SpinButtonDefaultValue.Value);
			pref.LIGFade = Convert.ToInt32(SpinButtonFade.Value);
			pref.LIGToggleActionName = LIGEntryToggle.Text;
			pref.LIGTuneActionName = LIGEntryTune.Text;
			pref.LIGStopActionName = LIGEntryStop.Text;
			pref.LIGSetActionName = LIGEntrySetValue.Text;
			
			//##### SWITCH ########
			
			datamanagement.UpdateActionInScenarioAndFunction(pref.SWIClicActionName,SWIEntryClic.Text);
			datamanagement.UpdateActionInScenarioAndFunction(pref.SWIDoubleClicActionName,SWIEntryDoubleClic.Text);
			datamanagement.UpdateActionInScenarioAndFunction(pref.SWIOnActionName,SWIEntryOn.Text);			
			datamanagement.UpdateActionInScenarioAndFunction(pref.SWIOnFmActionName,SWIEntryOnFm.Text);
			datamanagement.UpdateActionInScenarioAndFunction(pref.SWIOffActionName,SWIEntryOff.Text);
			datamanagement.UpdateActionInScenarioAndFunction(pref.SWIOffFmActionName,SWIEntryOffFm.Text);		
			
			pref.SWIInverse = CheckButtonInverse.Active;
			pref.SWIImpusionTime = Convert.ToInt32(SpinButtonImpulsionTime.Value);
			pref.SWIClicActionName = SWIEntryClic.Text;
			pref.SWIDoubleClicActionName = SWIEntryDoubleClic.Text;
			pref.SWIOnActionName = SWIEntryOn.Text;
			pref.SWIOnFmActionName = SWIEntryOnFm.Text;	 	
			pref.SWIOffActionName =	SWIEntryOff.Text;	
			pref.SWIOffFmActionName = SWIEntryOffFm.Text;		
				
			//##### SHUTTER ########
			
			datamanagement.UpdateActionInScenarioAndFunction(pref.SHUOpenActionName,SHUEntryOpen.Text);
			datamanagement.UpdateActionInScenarioAndFunction(pref.SHUCloseActionName,SHUEntryClose.Text);
			datamanagement.UpdateActionInScenarioAndFunction(pref.SHUStopActionName,SHUEntryStop.Text);				
			datamanagement.UpdateActionInScenarioAndFunction(pref.SHUToggleActionName,SHUEntryToggle.Text);	
			
			pref.SHUType = ComboboxShutterType.Active;
			pref.SHUTravelTime = Convert.ToInt32(SpinButtonTravelTime.Value);
			pref.SHUInitTime = Convert.ToInt32(SpinButtonInitTime.Value);
			pref.SHUOpenActionName = SHUEntryOpen.Text;
			pref.SHUCloseActionName = SHUEntryClose.Text;
			pref.SHUStopActionName = SHUEntryStop.Text;
			pref.SHUToggleActionName = SHUEntryToggle.Text;
					
			//#### TEMPERATURE ######
			
			datamanagement.UpdateActionInScenarioAndFunction(pref.TEMPGetValue,TEMPEntryGet.Text);
						
			pref.TEMPGetValue = TEMPEntryGet.Text;
			
			
			
			
			param.ChangeLangage(pref.Langage);
			
			mainwindow.Sensitive = true;
			mainwindow.UpdateMainNoteBook();
			mainwindow.UpdateWidgetInTab();
			mainwindow.UpdateMainWindow();
			mainwindow.RenameTooltipTextButtonOutput();
			mainwindow.UpdateOutputTreeview();
			mainwindow.UpdateHistoricTreeView();
			mainwindow.UpdateStatusBar();
			this.Destroy();
		}
		
			//Fonction AddElement
			//Fonction peremttant d'ajouter une ligne dans le fichier xml
			public void	AddElement(XmlTextWriter xml, string _Element, string _Attribute, string _ValueAttribue, byte _NumberOfTab)
			{	
				string tabulation = "";
				for(byte i=0;i<_NumberOfTab;i++)
				{
					tabulation = tabulation + "\t";
				}
				xml.WriteWhitespace("\n"+tabulation);
				xml.WriteStartElement(_Element);
				xml.WriteStartAttribute(_Attribute);
				xml.WriteValue(_ValueAttribue);
				xml.WriteEndElement();
			}
		
			//Fonction CalculHash
			//Fonction permettant de calculer le hash d'un fichier
			public string CalculHash(string _Text)
			{
				string hash;
				using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
				{
				  hash = BitConverter.ToString(md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(_Text))).Replace("-", String.Empty);
				}	
				return hash;
			}
			
			//Fonction ReturnLangage
			//Fonction permettant de retourner la langue en fonction du choix dans les radio button
			public string ReturnLangage()
			{
				if(FrenchRadioButton.Active == true)
				{
					return param.ParamP ("FrenchLangage");
				}
				else if(EnglishRadioButton.Active == true)
				{
					return param.ParamP ("EnglishLangage");
				}		
				return param.ParamP ("EnglishLangage");
			}
		
		
		//Fonction OnButtonCancelClicked
		//Fonction effectué lors de l'appui sur le bouton cancel
		protected void OnButtonCancelClicked (object sender, System.EventArgs e)
		{
			mainwindow.Sensitive = true;
			this.Destroy();
		}
		
		//Fonction OnDeleteEvent
		//Fonction éffectué lors de la destruction de la fenêtre
		protected void OnDeleteEvent (object o, Gtk.DeleteEventArgs args)
		{
			mainwindow.Sensitive = true;
		}
		
		
			//Fonction OnEntryToggleFocusOutEvent
			//Fonction permettant de faire des actions lorsque le focus quitte la cellule toggle
			protected void OnEntryToggleFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
			{
				LIGEntryToggle.Text = datamanagement.ReturnCorrectName(LIGEntryToggle.Text);
			}
			
			//Fonction OnEntryTuneFocusOutEvent
			//Fonction permettant de faire des actions lorsque le focus quitte la cellule toggle		
			protected void OnEntryTuneFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
			{
				LIGEntryTune.Text = datamanagement.ReturnCorrectName(LIGEntryTune.Text);
			}
	
			//Fonction OnEntryStopFocusOutEvent
			//Fonction permettant de faire des actions lorsque le focus quitte la cellule toggle	
			protected void OnEntryStopFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
			{
				LIGEntryStop.Text = datamanagement.ReturnCorrectName(LIGEntryStop.Text);
			}
		
			//Fonction OnSWIEntryClicFocusOutEvent
			//Fonction permettant de faire des actions lorsque le focus quitte la cellule
			protected void OnSWIEntryClicFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
			{
				SWIEntryClic.Text = datamanagement.ReturnCorrectName(SWIEntryClic.Text);
			}

			//Fonction OnSWIEntryDoubleClicFocusOutEvent
			//Fonction permettant de faire des actions lorsque le focus quitte la cellule
			protected void OnSWIEntryDoubleClicFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
			{
				SWIEntryDoubleClic.Text = datamanagement.ReturnCorrectName(SWIEntryDoubleClic.Text);
			}
		
			//Fonction OnSWIEntryOnFocusOutEvent
			//Fonction permettant de faire des actions lorsque le focus quitte la cellule		
			protected void OnSWIEntryOnFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
			{
				SWIEntryOn.Text = datamanagement.ReturnCorrectName(SWIEntryOn.Text);
			}

			//Fonction OnSWIEntryOnFmFocusOutEvent
			//Fonction permettant de faire des actions lorsque le focus quitte la cellule		
			protected void OnSWIEntryOnFmFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
			{
				SWIEntryOnFm.Text = datamanagement.ReturnCorrectName(SWIEntryOnFm.Text);
			}
		
			//Fonction OnSWIEntryOffFocusOutEvent
			//Fonction permettant de faire des actions lorsque le focus quitte la cellule		
			protected void OnSWIEntryOffFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
			{
				SWIEntryOff.Text = datamanagement.ReturnCorrectName(SWIEntryOff.Text);
			}
		
			//Fonction OnSWIEntryOffFmFocusOutEvent
			//Fonction permettant de faire des actions lorsque le focus quitte la cellule		
			protected void OnSWIEntryOffFmFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
			{
				SWIEntryOffFm.Text = datamanagement.ReturnCorrectName(SWIEntryOffFm.Text);
			}
		
			//Fonction OnSHUEntryOpenFocusOutEvent
			//Fonction permettant de faire des actions lorsque le focus quitte la cellule		
			protected void OnSHUEntryOpenFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
			{
				SHUEntryOpen.Text = datamanagement.ReturnCorrectName(SHUEntryOpen.Text);
			}
	
			//Fonction OnSHUEntryCloseFocusOutEvent
			//Fonction permettant de faire des actions lorsque le focus quitte la cellule		
			protected void OnSHUEntryCloseFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
			{
				SHUEntryClose.Text = datamanagement.ReturnCorrectName(SHUEntryClose.Text);
			}
	
			//Fonction OnSHUEntryStopFocusOutEvent
			//Fonction permettant de faire des actions lorsque le focus quitte la cellule		
			protected void OnSHUEntryStopFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
			{
				SHUEntryStop.Text = datamanagement.ReturnCorrectName(SHUEntryStop.Text);
			}
		
			//Fonction OnSHUEntryToggleFocusOutEvent
			//Fonction permettant de faire des actions lorsque le focus quitte la cellule				
			protected void OnSHUEntryToggleFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
			{
				SHUEntryToggle.Text = datamanagement.ReturnCorrectName(SHUEntryToggle.Text);
			}			
		
			//Fonction OnTEMPEntryGetFocusOutEvent
			//Fonction permettant de faire des actions lorsque le focus quitte la cellule				
			protected void OnTEMPEntryGetFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
			{
				TEMPEntryGet.Text = datamanagement.ReturnCorrectName(TEMPEntryGet.Text);
			}		
		
		//Fonction ReturnErrorActionName
		//Fonction permettant de verifier qu'un nom d'action est pas en double
		public bool ReturnErrorActionName()
		{
			List<string> ValuesEnter = new List<string>(){LIGEntryToggle.Text, 		
												 		  LIGEntryTune.Text, 
												 		  LIGEntryStop.Text,
													 	  SWIEntryClic.Text,
												 		  SWIEntryDoubleClic.Text,
												 		  SWIEntryOn.Text,
												 		  SWIEntryOnFm.Text,
												 		  SWIEntryOff.Text,
												 		  SWIEntryOffFm.Text,
												 		  SHUEntryOpen.Text,
												 		  SHUEntryClose.Text,
												 		  SHUEntryStop.Text};
			
			for(int i=0;i<ValuesEnter.Count;i++)
			{
				for(int j=0;j<ValuesEnter.Count;j++)
				{
					if((ValuesEnter[i] == ValuesEnter[j] && i != j) || ValuesEnter[i] == "")
					{
						return false;
					}
				}				
			}
			return true;
		}



	}
}
