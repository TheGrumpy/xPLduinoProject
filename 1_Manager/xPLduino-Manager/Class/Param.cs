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
using xPLduinoManager;
using System.Collections.Generic;

namespace xPLduinoManager
{
	//Classe Param
	//Classe permettant de stocker l'ensemble des paramètre
	public class Param
	{
		public List<Parameters> ListParameter = new List<Parameters>();
		public string Langage;
			
		public Param (string _Langage)
		{	
			this.Langage = _Langage;
			
			General();
			Pictures();
			
			MainWindow();
			ExplorerTreeView();
			MenuExplorerTreeView();
			NoteBook();
			ViewNoteBook();
			OutputTreeview();
			HistoricTreeView();
			
			NewProjectWindow();
			NewNodeWindow();
			NewNetworkWindow();		
			NewBoardWindow();
			NewInstanceWindow();
			NewCustomerWindow();
			NewScenarioWindow();
			
			ProjectPropertieWidget();
			NodePropertieWidget();
			I2CPropertieWidget();	
			OWPropertieWidget();
			I2CBoardPropertieWidget();
			InstancePropertieWidget();
			CustomerEditWidget();
			ScenarioEditWidget();

			Question();
			AlertMessage();

			ModifyProject();
			ModifyNode();
			ModifyNetwork();
			ModifyBoard();
			ModifyInstance();
			ModifyCustomer();
			ModifyScenario();
			ModifyVariable();
			ModifyFunction();
			
			XMLArgument();
			
			ClicMouse();
			Instance();
			PinDirection();
			PreferenceWindow();
			HistoricText();
			ActionParameter();
			ClicRightMenu();
		}

		public void General()
		{
			//INT
			ListParameter.Add(new Parameters("CarSizeMax",16));
			ListParameter.Add(new Parameters("MaxI2CAdress",15));
			ListParameter.Add(new Parameters("NoteVPanedPurcent",75));
			ListParameter.Add(new Parameters("NoteHPanedPurcent",10));
			
			//LANGAGE
			ListParameter.Add(new Parameters("VersionText","Version : ","Version : "));
			ListParameter.Add(new Parameters("EmailText","Email : ","Email : "));
			
			ListParameter.Add(new Parameters("ChooseAProject","Choisir un projet","Choose a project"));
			ListParameter.Add(new Parameters("ChooseAFolder","Choisir un dossier","Choose a folder"));
			ListParameter.Add(new Parameters("Cancel","Annuler","Cancel"));
			ListParameter.Add(new Parameters("Open","Ouvrir","Open"));
			ListParameter.Add(new Parameters("TextPath","Chemin : ","Path : "));
			ListParameter.Add(new Parameters("ErrorPathCustomer","Erreur de chemin pour le fichier customer","Error path customer"));
			
			ListParameter.Add(new Parameters("SaveSucceful","Enregistrement réussi","Your registration is successful"));
			ListParameter.Add(new Parameters("Password","Mot de passe :","Password :"));
			ListParameter.Add(new Parameters("CurrentProject","Projet en cours : ","Current project :"));
			ListParameter.Add(new Parameters("PathCurrentProject","Chemin du projet : ","Path project :"));
			ListParameter.Add(new Parameters("IncorectPath","Le chemin du projet est incorrect","Incorrect path"));
			ListParameter.Add(new Parameters("EmptyProject","Pas de projet selectionné","No project selected"));
			
			ListParameter.Add(new Parameters("ExtractionFinished","Extraction terminée","Extraction Finished"));
			ListParameter.Add(new Parameters("SourceCodeNotFound","Code Source non présent","Source Code not found"));
			
			ListParameter.Add(new Parameters("ChooseAScenario","Veuillez associé un scénario à votre customer","Please attach a scenario in your customer"));
			ListParameter.Add(new Parameters("CompleteCompilation","Compilation terminé","Compilation is finished"));
			ListParameter.Add(new Parameters("ErrorCompilation","Erreur dans la compilation","Compilation Error"));
			
			ListParameter.Add(new Parameters("CompleteLoad","Chargement terminé","Load is finished"));
			ListParameter.Add(new Parameters("ErrorLoad","Erreur dans le chargement","Load Error"));
			
			ListParameter.Add(new Parameters("ConnectAProgrammer","Veuillez connecter un programmateur","Connect a programmer"));
			
			//MULTI-LANGAGE
			ListParameter.Add(new Parameters("Version","1.0.0"));
			ListParameter.Add(new Parameters("Email","xplduino@gmail.com"));
			
			ListParameter.Add(new Parameters("ExtensionFile",".xdo"));
			ListParameter.Add(new Parameters("PathPreferenceXML","/Xml/Preference.xml"));
			ListParameter.Add(new Parameters("ReplaceUnderscor","-"));
			ListParameter.Add(new Parameters("FrLangage","French","English"));
			ListParameter.Add(new Parameters("FrenchLangage","Français"));
			ListParameter.Add(new Parameters("EnglishLangage","English"));
			
			ListParameter.Add(new Parameters("I2CType","I2C"));
			ListParameter.Add(new Parameters("OneWireType","1-Wire"));
			ListParameter.Add(new Parameters("RS485Type","RS485"));
			
			ListParameter.Add(new Parameters("FolderPreference","/Preference-xPLduino-Manager"));
			ListParameter.Add(new Parameters("FolderSourceFirmware","/Firmware/xplduino_controller"));
			ListParameter.Add(new Parameters("FolderHardware","/hardware/tools/avr/bin/"));
			ListParameter.Add(new Parameters("FolderTools","/hardware/tools/"));
			ListParameter.Add(new Parameters("FolderLibrary","/libraries"));
			ListParameter.Add(new Parameters("FolderCore","/hardware/arduino/cores/arduino"));
			ListParameter.Add(new Parameters("FolderXplduinoVariant","/hardware/arduino/variants/xplduino"));
			ListParameter.Add(new Parameters("FolderTargetFirmware","/xplduino_controller"));
			ListParameter.Add(new Parameters("PreferenceXml","/Preference-xPLduino-Manager/Preference.xml"));
			ListParameter.Add(new Parameters("CRCPreferenceXml","/Preference-xPLduino-Manager/CRC"));
			ListParameter.Add(new Parameters("Preference","Preference"));
			
			ListParameter.Add(new Parameters("Customer","CUSTOMER"));
			ListParameter.Add(new Parameters("CustomerInoName","customer.ino"));
			ListParameter.Add(new Parameters("PathCustomer","/Firmware/Customer/customer.ino"));
			
			ListParameter.Add(new Parameters("Scenario","SCENARIO"));
			ListParameter.Add(new Parameters("ScenarioName","scenario"));
			
			
		}
		
		public void Pictures()
		{
			//MULTI-LANGAGE
			ListParameter.Add(new Parameters("IconHome","xPLduinoManager.Pictures.Home.png"));
			ListParameter.Add(new Parameters("IconFolder","xPLduinoManager.Pictures.Folder.png"));
			ListParameter.Add(new Parameters("IconProject","xPLduinoManager.Pictures.Project.png"));
			ListParameter.Add(new Parameters("IconNode","xPLduinoManager.Pictures.Node.png"));
			ListParameter.Add(new Parameters("IconNetwork","xPLduinoManager.Pictures.Network.png"));
			ListParameter.Add(new Parameters("IconBoard","xPLduinoManager.Pictures.Board.png"));
			ListParameter.Add(new Parameters("IconLighting","xPLduinoManager.Pictures.Lighting.png"));
			ListParameter.Add(new Parameters("IconSwitch","xPLduinoManager.Pictures.Switch.png"));
			ListParameter.Add(new Parameters("IconShutter","xPLduinoManager.Pictures.Shutter.png"));
			ListParameter.Add(new Parameters("IconProperties","xPLduinoManager.Pictures.Parameter.png"));
			ListParameter.Add(new Parameters("IconDebug","xPLduinoManager.Pictures.Debug.png"));
			ListParameter.Add(new Parameters("IconPin","xPLduinoManager.Pictures.Pin.png"));
			ListParameter.Add(new Parameters("IconEmpty","xPLduinoManager.Pictures.Empty.png"));
			ListParameter.Add(new Parameters("IconCustomer","xPLduinoManager.Pictures.Customer.png"));
			ListParameter.Add(new Parameters("IconCustomerUse","xPLduinoManager.Pictures.CustomerUse.png"));
			ListParameter.Add(new Parameters("IconScenario","xPLduinoManager.Pictures.Scenario.png"));
			ListParameter.Add(new Parameters("IconOK","xPLduinoManager.Pictures.OK.png"));
			ListParameter.Add(new Parameters("IconNOK","xPLduinoManager.Pictures.NOK.png"));			
			
		}
		public void MainWindow()
		{
			//INT
			ListParameter.Add(new Parameters("MWHPanedExtendDefaultValue",324));
			ListParameter.Add(new Parameters("MWVPanedExtendDefaultValue",65));
			//LANGAGE
			ListParameter.Add(new Parameters("MWTitle","xPLduino - Manager","xPLduino - Manager"));
			ListParameter.Add(new Parameters("MWFileMenu","Fichier","File"));
			ListParameter.Add(new Parameters("MWNewProjectMenu","Nouveau Projet","New Project"));
			ListParameter.Add(new Parameters("MWOpenProjectMenu","Ouvrir Projet","Open Project"));
			ListParameter.Add(new Parameters("MWSaveProjectMenu","Sauvegarder Projet","Save Project"));
			ListParameter.Add(new Parameters("MWSaveAllProjectMenu","Sauvegarder tous les projets","Save all Project"));
			ListParameter.Add(new Parameters("MWTools","Outils","Tools"));
			ListParameter.Add(new Parameters("MWWindow","Fenêtre","Window"));
			ListParameter.Add(new Parameters("MWNextWindow","Fenêtre suivante","Next window"));
			ListParameter.Add(new Parameters("MWPrevWindow","Fenêtre précédente","Previous window"));
			ListParameter.Add(new Parameters("MWDelWindow","Supprimer fenêtre","Delete window"));
			ListParameter.Add(new Parameters("MWEdit","Editer","Edit"));
			ListParameter.Add(new Parameters("MWUndo","Annuler","Undo"));
			ListParameter.Add(new Parameters("MWRedo","Rétablir","Redo"));
			ListParameter.Add(new Parameters("MWSaveProject","Enregistrer projet","Save project"));
			ListParameter.Add(new Parameters("MWSaveAllProject","Tout enregistrer","Save all"));
			ListParameter.Add(new Parameters("MWExtractEmbedded","Extraction de l'embarqué","Extract embedded"));
			ListParameter.Add(new Parameters("MWVerifyEmbedded","Compilation de l'embarqué","Compile embedded"));
			ListParameter.Add(new Parameters("MWLoadEmbedded","Chargement de l'embarqué","Load embedded"));
			ListParameter.Add(new Parameters("MWReloadUSB","Rafraichissement des port USB","Refresh USB port"));
			ListParameter.Add(new Parameters("MWHelp","Aide","Help"));
			ListParameter.Add(new Parameters("MWAbout","A propos","About"));
		}
		public void ExplorerTreeView()
		{
			//INT
			ListParameter.Add(new Parameters("ExTVPositionPixBuff",0));
			ListParameter.Add(new Parameters("ExTVPositionTexte",1));
			ListParameter.Add(new Parameters("ExTVPositionType",2));
			ListParameter.Add(new Parameters("ExTVPositionName",3));
			ListParameter.Add(new Parameters("ExTVPositionId",4));
			
			//LANGAGE
			ListParameter.Add(new Parameters("ExTVTitle","Explorateur de projet","Project Explorer"));
			ListParameter.Add(new Parameters("ExTVNameNetwork","Réseaux","Network"));
			ListParameter.Add(new Parameters("ExTVNameInstance","Objets","Devices"));
			ListParameter.Add(new Parameters("ExTVNameLighting","Lumière","Lighting"));
			ListParameter.Add(new Parameters("ExTVNameSwitch","Commande","Switch"));
			ListParameter.Add(new Parameters("ExTVNameShutter","Volet","Shutter"));
			ListParameter.Add(new Parameters("ExTVNameFile","Fichier","File"));
			ListParameter.Add(new Parameters("ExTVNameCustomer","Fichier type","Customer file"));
			ListParameter.Add(new Parameters("ExTVNameScenario","Fichier scénario","Scenario file"));
			
			//MULTI-LANGAGE
			ListParameter.Add(new Parameters("ExTVTypeProject","PROJECT"));
			ListParameter.Add(new Parameters("ExTVTypeNetwork","NETWORK"));
			ListParameter.Add(new Parameters("ExTVTypeNetworkI2C","I2C"));
			ListParameter.Add(new Parameters("ExTVTypeNetwork1Wire","1-Wire"));
			ListParameter.Add(new Parameters("ExTVTypeNode","NODE"));
			ListParameter.Add(new Parameters("ExTVTypeLighting","LIGHTING"));
			ListParameter.Add(new Parameters("ExTVTypeSwitch","SWITCH"));
			ListParameter.Add(new Parameters("ExTVTypeShutter","SHUTTER"));
			ListParameter.Add(new Parameters("ExTVUpButton","Plier","Fold"));
			ListParameter.Add(new Parameters("ExTVDownButton","Déplier","Unfold"));
			ListParameter.Add(new Parameters("ExTVReduceWindow","Réduire","Reduce"));
			ListParameter.Add(new Parameters("ExTVExpandWindow","Développer","Expand"));
		}
		public void MenuExplorerTreeView()
		{
			//LANGAGE
			ListParameter.Add(new Parameters("MenuDeleteProject","Fermer projet","Close project"));
			ListParameter.Add(new Parameters("MenuNewNode","Nouveau noeud","New node"));
			ListParameter.Add(new Parameters("MenuDeleteNode","Supprimer noeud","Delete node"));
			ListParameter.Add(new Parameters("MenuNewNetwork","Nouveau réseau","New network"));
			ListParameter.Add(new Parameters("MenuDeleteNetwork","Supprimer réseau","Delete network"));
			ListParameter.Add(new Parameters("MenuNewBoard","Nouvelle carte","New board"));
			ListParameter.Add(new Parameters("MenuDeleteBoard","Supprimer carte","Delete board"));
			ListParameter.Add(new Parameters("MenuNewInstance","Nouvel objet","New devide"));
			ListParameter.Add(new Parameters("MenuNewCustomer","Nouveau fichier type","New customer file"));
			ListParameter.Add(new Parameters("MenuNewScenario","Nouveau fichier scenario","New scenario file"));
			ListParameter.Add(new Parameters("MenuDeleteInstance","Supprimer objet","Delete device"));
			ListParameter.Add(new Parameters("MenuDeleteCustomer","Supprimer fichier type","Delete customer file"));
			ListParameter.Add(new Parameters("MenuDeleteScenario","Supprimer fichier scénario","Delete scenario file"));
		}
		public void NoteBook()
		{
			//LANGAGE
			ListParameter.Add(new Parameters("TabWelcomeTitle","Bienvenue","Welcome"));
			
			//MULTI-LANGAGE
			ListParameter.Add(new Parameters("TabIconClose","gtk-close"));
		}
		public void ViewNoteBook()
		{
			//LANGAGE
			ListParameter.Add(new Parameters("VNBOutputTab","Sortie","Output"));
			ListParameter.Add(new Parameters("VNBHistoryTab","Historique","Historic"));
			ListParameter.Add(new Parameters("VNBLogCompilTab","Log de compilation","Compilation log"));
			
			ListParameter.Add(new Parameters("VNBInformationLabel","Informations","Informations"));			
			ListParameter.Add(new Parameters("VNBHistoryLabel","Historique des travaux","Works historic"));
			ListParameter.Add(new Parameters("VNBCompilLogLabel","Log de compilation","Compilation log"));
			
		}
		public void OutputTreeview()
		{
			//INT
			ListParameter.Add(new Parameters("OutTVPositionLogo",0));
			ListParameter.Add(new Parameters("OutTVPositionDateAndTime",1));
			ListParameter.Add(new Parameters("OutTVPositionInfo",2));
			ListParameter.Add(new Parameters("OutputInformation",0));
			ListParameter.Add(new Parameters("OutputWarning",1));
			ListParameter.Add(new Parameters("OutputError",2));
			ListParameter.Add(new Parameters("OutputQuestion",3));
			
			//LANGAGE
			ListParameter.Add(new Parameters("OutputWelcomeLine","Bienvenue sur le Manager xPLduino","Welcome on xPLduino Manager"));
			ListParameter.Add(new Parameters("OutTVTitleValue","Sortie","Output"));
			ListParameter.Add(new Parameters("OutTVDateAndTimeValue","Date et heure","Date and time"));
			ListParameter.Add(new Parameters("TextOutputInformation","Informations","Informations"));
			ListParameter.Add(new Parameters("TextOutputWarning","Alertes","Warnings"));
			ListParameter.Add(new Parameters("TextOutputError","Erreurs","Errors"));
			ListParameter.Add(new Parameters("TextOutputQuestion","Questions","Questions"));
			ListParameter.Add(new Parameters("TextOutputErase","Vider","Erase"));
			
			//MULTI-LANGAGE
			ListParameter.Add(new Parameters("LogoOutputInformation","gtk-dialog-info"));    
			ListParameter.Add(new Parameters("LogoOutputWarning","gtk-dialog-warning"));    
			ListParameter.Add(new Parameters("LogoOutputError","gtk-dialog-error"));
			ListParameter.Add(new Parameters("LogoOutputQuestion","gtk-dialog-question"));  
			ListParameter.Add(new Parameters("LogoErase","gtk-clear"));  
		}
		public void HistoricTreeView()
		{
			//INT
			ListParameter.Add(new Parameters("HTVPositionView",0));
			ListParameter.Add(new Parameters("HTVPositionNumberView",1));
			ListParameter.Add(new Parameters("HTVPositionDateAndTime",2));
			ListParameter.Add(new Parameters("HTVPositionInfo",3));					                                 
			
			//LANGAGE
			ListParameter.Add(new Parameters("HTVView","Vue","View"));
			ListParameter.Add(new Parameters("HTVNumberView","N° de la vue","Number view"));
			ListParameter.Add(new Parameters("HTVDateAndTime","Date et heure","Date and time"));
			ListParameter.Add(new Parameters("HTVInfo","Informations","Informations"));			
		}		
		public void NewProjectWindow()
		{
			//LANGAGE
			ListParameter.Add(new Parameters("NPTitle","Nouveau Projet","New Project"));
			ListParameter.Add(new Parameters("NPLabelProjectName","Nom du projet :","Project Name :"));
			ListParameter.Add(new Parameters("NPDefaultProjectName","NomDuProjet","ProjectName"));
			ListParameter.Add(new Parameters("NPLabelProjectAuthor","Auteur du projet :","Project author :"));
			ListParameter.Add(new Parameters("NPDefaultProjectAuthor","AuteurDuProjet","ProjectAuthor"));
			ListParameter.Add(new Parameters("NPLabelProjectPath","Chemin du projet :","Project path:"));
			ListParameter.Add(new Parameters("NPButtonOk","Valider","Ok"));
			ListParameter.Add(new Parameters("NPButtonCancel","Annuler","Cancel"));
			ListParameter.Add(new Parameters("NPNameEmpty","Nom de projet vide","Project name is empty"));
			ListParameter.Add(new Parameters("NPOtherProjectExist","Un autre projet de ce nom existe au chemin indiqué","An other project with this name exist in the path"));
			ListParameter.Add(new Parameters("NPProjectExistInList"," existe au chemin indiqué"," exist in this path"));
		}
		public void NewNodeWindow()
		{
			//LANGAGE
			ListParameter.Add(new Parameters("NNTitle","Nouveau Noeud","New Node"));
			ListParameter.Add(new Parameters("NNLabelNodeName","Nom du noeud :","Node name :"));
			ListParameter.Add(new Parameters("NNDefaultNodeName","Noeud","Node"));
			ListParameter.Add(new Parameters("NNButtonOK","Valider","Ok"));
			ListParameter.Add(new Parameters("NNButtonCancel","Annuler","Cancel"));
			ListParameter.Add(new Parameters("NNEmptyName","Nom de noeud vide","Node name empty"));
			ListParameter.Add(new Parameters("NNNodeExiste"," existe dans la liste des noeuds"," exist in node list"));
		}
		public void NewNetworkWindow()
		{
			//LANGAGE
			ListParameter.Add(new Parameters("NNWTitle","Nouveau réseau","New network"));
			ListParameter.Add(new Parameters("NNWLabelNetworkName","Nom du réseau :","Network name :"));
			ListParameter.Add(new Parameters("NNWButtonOK","Valider","Ok"));
			ListParameter.Add(new Parameters("NNWButtonCancel","Annuler","Cancel"));
		}
		public void NewBoardWindow()
		{
			//LANGAGE
			ListParameter.Add(new Parameters("NBTitle","Nouvel Carte","New Board"));
			ListParameter.Add(new Parameters("NBLabelBoardName","Nom de la carte :","Board name :"));
			ListParameter.Add(new Parameters("NBDefaultBoardName","NomDeLaCarte","BoardName"));
			ListParameter.Add(new Parameters("NBDefaultProbeName","NomDeLaSonde","ProbeName"));
			ListParameter.Add(new Parameters("NBLabelBoardType","Type de carte :","Board type :"));
			ListParameter.Add(new Parameters("NBButtonOK","Valider","Ok"));
			ListParameter.Add(new Parameters("NBButtonCancel","Annuler","Cancel"));
			ListParameter.Add(new Parameters("NBEmptyName","Nom de carte vide","Board name empty"));
			ListParameter.Add(new Parameters("NBChooseBoard","Veuillez sélectionner un type de carte","Choose a board type"));
			ListParameter.Add(new Parameters("NBBoardExiste"," existe dans la liste des réseaux"," exist in network list"));
		}
		public void NewInstanceWindow()
		{
			//LANGAGE
			ListParameter.Add(new Parameters("NITitle","Nouvel objet","New device"));
			ListParameter.Add(new Parameters("NILabelInstanceName","Nom de l'objet :","Device name :"));
			ListParameter.Add(new Parameters("NIDefaultInstanceName","NomObjet","DeviceName"));
			ListParameter.Add(new Parameters("NILabelInstanceType","Type de l'objet :","Device type :"));
			ListParameter.Add(new Parameters("NIButtonOK","Valider","Ok"));
			ListParameter.Add(new Parameters("NIButtonCancel","Annuler","Cancel"));
			ListParameter.Add(new Parameters("NIEmptyName","Nom de l'objet vide","Device name empty"));
			ListParameter.Add(new Parameters("NIChooseInstance","Veuillez sélectionner un type d'objet","Choose an device type"));
			ListParameter.Add(new Parameters("NIInstanceExiste"," existe dans la liste des noeud"," exist in node list"));			
		}
		public void NewCustomerWindow()
		{
			//LANGAGE
			ListParameter.Add(new Parameters("NCTitle","Nouvel fichier type","New customer file"));
			ListParameter.Add(new Parameters("NCLabelCustomerName","Nom du fichier type :","Customer file name :"));
			ListParameter.Add(new Parameters("NCDefaultCustomerName","NomFichierType","CustomerFileName"));
			ListParameter.Add(new Parameters("NCButtonOK","Valider","Ok"));
			ListParameter.Add(new Parameters("NCButtonCancel","Annuler","Cancel"));
			ListParameter.Add(new Parameters("NCEmptyName","Nom de fichier type vide","Customer file name empty"));
			ListParameter.Add(new Parameters("NCCustomerExiste"," existe dans la liste des fichiers types"," exist in customers files list"));
		}
		public void NewScenarioWindow()
		{
			//LANGAGE
			ListParameter.Add(new Parameters("NSTitle","Nouvel fichier scénario","New scenario file"));
			ListParameter.Add(new Parameters("NSLabelScenarioName","Nom du fichier scénario :","Scenario file name :"));
			ListParameter.Add(new Parameters("NSDefaultScenarioName","NomFicScenario","ScenarFileName"));
			ListParameter.Add(new Parameters("NSButtonOK","Valider","Ok"));
			ListParameter.Add(new Parameters("NSButtonCancel","Annuler","Cancel"));
			ListParameter.Add(new Parameters("NSEmptyName","Nom de fichier scenario vide","Scenario file name empty"));
			ListParameter.Add(new Parameters("NSCustomerExiste"," existe dans la liste des fichiers scénario"," exist in scenario files list"));
		}		
		public void ProjectPropertieWidget()
		{
			//INT
			ListParameter.Add(new Parameters("PP_TVPro_TypePosition",0));
			ListParameter.Add(new Parameters("PP_TVPro_ValuePosition",1));
			ListParameter.Add(new Parameters("PP_TVPro_NamePostion",0));
			ListParameter.Add(new Parameters("PP_TVPro_AuthorPostion",1));
			ListParameter.Add(new Parameters("PP_TVPro_DTCreationPostion",2));
			ListParameter.Add(new Parameters("PP_TVPro_DTModificationPostion",3));
			ListParameter.Add(new Parameters("PP_TVPro_DTVersionPostion",4));
			ListParameter.Add(new Parameters("PP_TVPro_PathPostion",5));
			ListParameter.Add(new Parameters("PP_TVPro_PasswordPostion",6));
			ListParameter.Add(new Parameters("PP_TVOpt_PostionPixBuff",0));
			ListParameter.Add(new Parameters("PP_TVOpt_PostionName",1));
			ListParameter.Add(new Parameters("PP_TVChild_OpNode_PositionName",0));
			ListParameter.Add(new Parameters("PP_TVChild_OpNode_PositionIP",1));
			ListParameter.Add(new Parameters("PP_TVChild_OpNode_PositionGTWIP",2));
			ListParameter.Add(new Parameters("PP_TVChild_OpNode_PositionMAC",3));
			ListParameter.Add(new Parameters("PP_TVChild_OpNode_PositionDHCP",4));
			ListParameter.Add(new Parameters("PP_TVChild_OpNode_PositionCompileOK",5));
			ListParameter.Add(new Parameters("PP_TVChild_OpNode_PositionID",6));
			
			//LANGAGE
			ListParameter.Add(new Parameters("PP_LabTVChild_Name_Choose","Choisir une option","Choose an option"));
			ListParameter.Add(new Parameters("PP_LabTVChild_Name_Nodes","Noeuds du projet :","Project nodes :"));
			ListParameter.Add(new Parameters("PP_LabTVChild_Name_Properties","Propriétés du projet :","Project properties :"));
			ListParameter.Add(new Parameters("PP_NoteLabel","Notes personnelles :","Personal notes :"));
			ListParameter.Add(new Parameters("PP_TVPro_TypeLabel","Type","Type"));
			ListParameter.Add(new Parameters("PP_TVPro_ValueLabel","Valeur","Value"));
			ListParameter.Add(new Parameters("PP_TVPro_LabelLine1","Nom du projet :","Project name :"));
			ListParameter.Add(new Parameters("PP_TVPro_LabelLine2","Auteur du projet :","Project author :"));
			ListParameter.Add(new Parameters("PP_TVPro_LabelLine3","Date et heure de création :","Creation date and time :"));
			ListParameter.Add(new Parameters("PP_TVPro_LabelLine4","Date et heure de modification :","Modification date and time :"));
			ListParameter.Add(new Parameters("PP_TVPro_LabelLine5","Version du projet :","Project version :"));
			ListParameter.Add(new Parameters("PP_TVPro_LabelLine6","Chemin du projet :","Project path :"));
			ListParameter.Add(new Parameters("PP_TVPro_LabelLine7","Mot de passe :","Password :"));			
			ListParameter.Add(new Parameters("PP_TVOpt_NameLabel","Paramétrage","Settings"));
			ListParameter.Add(new Parameters("PP_TVOpt_OptionParametre","Propriétés","Properties"));
			ListParameter.Add(new Parameters("PP_TVOpt_OptionNode","Noeuds","Nodes"));
			ListParameter.Add(new Parameters("PP_TVChild_OpNode_ID","ID","ID"));
			ListParameter.Add(new Parameters("PP_TVChild_OpNode_Name","Nom du noeud","Node name"));
			ListParameter.Add(new Parameters("PP_TVChild_OpNode_IP","Adresse IP","IP Adress"));
			ListParameter.Add(new Parameters("PP_TVChild_OpNode_GTWIP","Adresse IP Gateway","IP Gateway Adress"));
			ListParameter.Add(new Parameters("PP_TVChild_OpNode_DHCP","DHCP","DHCP"));
			ListParameter.Add(new Parameters("PP_TVChild_OpNode_MAC","Adresse Physique","Physical adress"));	
			ListParameter.Add(new Parameters("PP_TVChild_OpNode_CompileOK","Généré","Générate"));
			ListParameter.Add(new Parameters("PP_AddNode_Name_Button","Ajouter un noeud","Add a node"));
			ListParameter.Add(new Parameters("PP_DeleteNode_Name_Button","Supprimer un noeud","Delete a node"));	
			ListParameter.Add(new Parameters("PP_CompileOneNode_Name_Button","Générer un noeud","Generate one node"));
			ListParameter.Add(new Parameters("PP_LoadOneNode_Name_Button","Charger un noeud","Load node"));
			ListParameter.Add(new Parameters("PP_ErrorMessage_ChooseANode","Veuillez choisir un noeud","Choose a node"));
			ListParameter.Add(new Parameters("PP_ErrorMessage_ProjectNotSave","Votre projet n'est pas sauvegardé","Your project is not save"));	
			ListParameter.Add(new Parameters("PP_ErrorMessage_NodeNotCompile","Votre noeud n'est pas généré","Your node is not generate"));	
		}
		public void NodePropertieWidget()
		{
			//INT
			ListParameter.Add(new Parameters("NP_TVPro_TypePosition",0));
			ListParameter.Add(new Parameters("NP_TVPro_ValuePosition",1));
			ListParameter.Add(new Parameters("NP_TVPro_NamePostion",0));
			ListParameter.Add(new Parameters("NP_TVPro_IPPosition",1));
			ListParameter.Add(new Parameters("NP_TVPro_GTWIPPosition",2));
			ListParameter.Add(new Parameters("NP_TVPro_MacPosition",3));
			ListParameter.Add(new Parameters("NP_TVPro_DHCPPosition",4));
			ListParameter.Add(new Parameters("NP_TVPro_WebServerPosition",5));
			ListParameter.Add(new Parameters("NP_TVParam_PostionPixBuff",0));
			ListParameter.Add(new Parameters("NP_TVParam_PostionName",1));			
			ListParameter.Add(new Parameters("NP_TVChild_OpDebug_PositionType",0));
			ListParameter.Add(new Parameters("NP_TVChild_OpDebug_PositionValue",1));
			ListParameter.Add(new Parameters("NP_TVChild_OpDebug_PositionDesc",2));	
			ListParameter.Add(new Parameters("NP_TVChild_OpOption_PositionType",0)); //
			ListParameter.Add(new Parameters("NP_TVChild_OpOption_PositionClock",1)); //
			ListParameter.Add(new Parameters("NP_TVChild_OpOption_Position1Wire",2)); //
			ListParameter.Add(new Parameters("NP_TVOpt_TypePosition",0));
			ListParameter.Add(new Parameters("NP_TVOpt_ValuePosition",1));	
			ListParameter.Add(new Parameters("NP_TVChild_OpCustomer_PositionUse",0));
			ListParameter.Add(new Parameters("NP_TVChild_OpCustomer_PositionName",1)); //
			ListParameter.Add(new Parameters("NP_TVChild_OpCustomer_PositionScenario",2)); //
			ListParameter.Add(new Parameters("NP_TVChild_OpCustomer_PositionNote",3)); //
			ListParameter.Add(new Parameters("NP_TVChild_OpCustomer_PositionId",4)); //			
			ListParameter.Add(new Parameters("NP_TVChild_OpScenario_PositionName",0)); //
			ListParameter.Add(new Parameters("NP_TVChild_OpScenario_PositionNote",1)); //
			ListParameter.Add(new Parameters("NP_TVChild_OpScenario_PositionId",2)); //					
			
			
			//MULTI-LANGAGE
			ListParameter.Add(new Parameters("NP_SMBv00","V.0.0"));
			ListParameter.Add(new Parameters("NP_SMBv01","V.0.1"));
			ListParameter.Add(new Parameters("NP_CLockRTC","RTC"));
			ListParameter.Add(new Parameters("NP_CLockDCF77","DCF77"));
			
			//LANGAGE
			ListParameter.Add(new Parameters("NP_LabTVChild_Name_ScenarioList","Liste des fichiers scénarios :","Scenario files list :"));
			ListParameter.Add(new Parameters("NP_TVChild_OpScenario_Name","Nom","Name"));
			ListParameter.Add(new Parameters("NP_TVChild_OpScenario_Note","Notes","Notes"));			
			
			ListParameter.Add(new Parameters("NP_LabTVChild_Name_CustomerList","Liste des fichiers types :","Customers files list :"));
			ListParameter.Add(new Parameters("NP_TVChild_OpCustomer_Use","Utilisé","Use"));
			ListParameter.Add(new Parameters("NP_TVChild_OpCustomer_Name","Nom","Name"));
			ListParameter.Add(new Parameters("NP_TVChild_OpCustomer_Scenario","Scénario associé","Link scenario"));
			ListParameter.Add(new Parameters("NP_TVChild_OpCustomer_Note","Notes","Notes"));			
			
			ListParameter.Add(new Parameters("NP_LabTVChild_Name_Choose","Choisir une option :","Choose an option :"));
			ListParameter.Add(new Parameters("NP_LabTVChild_Name_DebugList","Liste des débogages :","Debug List"));
			ListParameter.Add(new Parameters("NP_LabTVChild_Name_Parameters","Propriété du noeud :","Node propertie :"));
			ListParameter.Add(new Parameters("NP_LabelProperties","Propriétés du noeud","Node properties"));
			ListParameter.Add(new Parameters("NP_NoteLabel","Notes personnelles :","Personal notes :"));
			ListParameter.Add(new Parameters("NP_TVPro_TypeLabel","Type","Type"));
			ListParameter.Add(new Parameters("NP_TVPro_ValueLabel","Valeur","Value"));
			ListParameter.Add(new Parameters("NP_TVPro_LabelLine1","Nom du noeud :","Node name :"));			
			ListParameter.Add(new Parameters("NP_TVPro_LabelLine2","Adresse IP :","IP Adress :"));
			ListParameter.Add(new Parameters("NP_TVPro_LabelLine3","Adresse IP gateway :","IP gateway Adress :"));			
			ListParameter.Add(new Parameters("NP_TVPro_LabelLine4","Adresse physique :","Physical Adress :"));
			ListParameter.Add(new Parameters("NP_TVPro_LabelLine5","DHCP :","DHCP :"));
			ListParameter.Add(new Parameters("NP_TVPro_LabelLine6","Serveur Web :","Web Serveur :"));
			ListParameter.Add(new Parameters("NP_TVParam_NameLabel","Paramétrage","Setting"));
			ListParameter.Add(new Parameters("NP_TVParam_ParameterParam","Propriétés","Properties"));
			ListParameter.Add(new Parameters("NP_TVParam_ParameterDebug","Débogage","Debug"));
			ListParameter.Add(new Parameters("NP_TVParam_ParameterOption","Options","Option"));
			ListParameter.Add(new Parameters("NP_TVParam_ParameterCustomer","Fichier type","Customer file"));
			ListParameter.Add(new Parameters("NP_TVParam_ParameterScenario","Fichier scénario","Scenario file"));
			ListParameter.Add(new Parameters("NP_TVChild_OpDebug_TypeDebug","Type de débogage","Debug type"));
			ListParameter.Add(new Parameters("NP_TVChild_OpDebug_ValueDebug","Valeur","Value"));
			ListParameter.Add(new Parameters("NP_TVChild_OpDebug_DescDebug","Description","Description"));
			ListParameter.Add(new Parameters("NP_TVOption_LabelLine1","Version de la SMB :","SMB Version :"));
			ListParameter.Add(new Parameters("NP_TVOption_LabelLine2","Horloge :","Clock :"));
			ListParameter.Add(new Parameters("NP_TVOption_LabelLine3","Puce 1-Wire :","1-Wire chi :"));
			ListParameter.Add(new Parameters("NP_YesValue","Oui","Yes"));
			ListParameter.Add(new Parameters("NP_NoValue","Non","No"));
			ListParameter.Add(new Parameters("NP_DataNoValid","Données non valide","Data is not valid"));
		}
		public void I2CPropertieWidget()
		{
			//INT
			ListParameter.Add(new Parameters("I2CP_TVPro_TypePosition",0));
			ListParameter.Add(new Parameters("I2CP_TVPro_ValuePosition",1));
			ListParameter.Add(new Parameters("I2CP_TVChild_OpBoard_PositionName",0));
			ListParameter.Add(new Parameters("I2CP_TVChild_OpBoard_PositionType",1));
			ListParameter.Add(new Parameters("I2CP_TVChild_OpBoard_PositionI2C0",2));
			ListParameter.Add(new Parameters("I2CP_TVChild_OpBoard_PositionI2C1",3));
			ListParameter.Add(new Parameters("I2CP_TVChild_OpBoard_PositionID",4));	
			
			//LANGAGE
			ListParameter.Add(new Parameters("I2CP_LabTVChild_Name_Choose","Choisir une option","Choose an option"));
			ListParameter.Add(new Parameters("I2CP_LabTVChild_Name_Properties","Propriétés du réseau :","Network properties :"));			
			ListParameter.Add(new Parameters("I2CP_LabTVChild_Name_BoardList","Liste des cartes :","Boards list :"));
			ListParameter.Add(new Parameters("I2CP_NoteLabel","Notes personnelles :","Personal notes :"));
			ListParameter.Add(new Parameters("I2CP_TVPro_TypeLabel","Type","Type"));
			ListParameter.Add(new Parameters("I2CP_TVPro_ValueLabel","Valeur","Value"));
			ListParameter.Add(new Parameters("I2CP_TVPro_LabelLine1","Type de réseau :","Network type :"));
			ListParameter.Add(new Parameters("I2CP_TVOpt_OptionProperties","Propriétés","Properties"));
			ListParameter.Add(new Parameters("I2CP_TVOpt_OptionBoard","Cartes","Boards"));
			ListParameter.Add(new Parameters("I2CP_TVChild_OpBoard_ID","ID","ID"));
			ListParameter.Add(new Parameters("I2CP_TVChild_OpBoard_Name","Nom de la carte","Board name"));
			ListParameter.Add(new Parameters("I2CP_TVChild_OpBoard_Type","Type de la carte","Board Type"));
			ListParameter.Add(new Parameters("I2CP_TVChild_OpBoard_I2C0","Addresse I2C 0","I2C 0 adress"));
			ListParameter.Add(new Parameters("I2CP_TVChild_OpBoard_I2C1","Addresse I2C 1","I2C 1 adress"));
			
			ListParameter.Add(new Parameters("I2CP_AddBoard_Name_Button","Ajouter carte","Add board"));

			
			ListParameter.Add(new Parameters("I2CP_AddBoardIn16_DefaultNameBoard","Carte16i","Board16i"));
			
			ListParameter.Add(new Parameters("I2CP_DeleteBoard_Name_Button","Supprimer une carte","Delete a board"));
			
		}
		public void OWPropertieWidget()
		{
			//INT 
			ListParameter.Add(new Parameters("OWP_TVPro_TypePosition",0));
			ListParameter.Add(new Parameters("OWP_TVPro_ValuePosition",1));
			ListParameter.Add(new Parameters("OWP_TVChild_OpBoard_PositionName",0));
			ListParameter.Add(new Parameters("OWP_TVChild_OpBoard_PositionType",1));
			ListParameter.Add(new Parameters("OWP_TVChild_OpBoard_PositionMac",2));
			ListParameter.Add(new Parameters("OWP_TVChild_OpBoard_PositionPrecision",3));
			ListParameter.Add(new Parameters("OWP_TVChild_OpBoard_PositionID",4));
			ListParameter.Add(new Parameters("OWP_TVOpt_PostionPixBuff",0));
			ListParameter.Add(new Parameters("OWP_TVOpt_PostionName",1));			
			
			//LANGAGE
			ListParameter.Add(new Parameters("OWP_LabTVChild_Name_Choose","Choisir une option","Choose an option"));
			ListParameter.Add(new Parameters("OWP_LabTVChild_Name_Properties","Propriété du réseau :","Network propertie"));
			ListParameter.Add(new Parameters("OWP_LabTVChild_Name_BoardList","Liste des sondes :","Sonde list"));
			ListParameter.Add(new Parameters("OWP_NoteLabel","Notes personnelles :","Personal notes :"));
			ListParameter.Add(new Parameters("OWP_TVPro_TypeLabel","Type","Type"));
			ListParameter.Add(new Parameters("OWP_TVPro_ValueLabel","Valeur","Value"));
			ListParameter.Add(new Parameters("OWP_TVPro_LabelLine1","Type de réseau :","Network type :"));
			ListParameter.Add(new Parameters("OWP_TVOpt_Label","Paramètre","Settings"));
			ListParameter.Add(new Parameters("OWP_TVOpt_OptionParameter","Propriétés","Properties"));
			ListParameter.Add(new Parameters("OWP_TVOpt_OptionBoard","Sondes","Sondes"));
			ListParameter.Add(new Parameters("OWP_TVChild_OpBoard_ID","ID","ID"));
			ListParameter.Add(new Parameters("OWP_TVChild_OpBoard_Name","Nom de la sonde","Sonde name"));
			ListParameter.Add(new Parameters("OWP_TVChild_OpBoard_Type","Type de sonde","Sonde type"));
			ListParameter.Add(new Parameters("OWP_TVChild_OpBoard_Mac","Addresse MAC","MAC adresse"));
			ListParameter.Add(new Parameters("OWP_TVChild_OpBoard_Precision","Precision","Precision"));			
			ListParameter.Add(new Parameters("OWP_ErrorMac","Addresse MAC n'est par correct","MAC Adress is not correct"));
			ListParameter.Add(new Parameters("OWP_QuestionMac","Avez-vous mis votre addresse en majuscule ?","Is that your address is in uppercase?"));
			ListParameter.Add(new Parameters("OWP_AddBoard_Name_Button","Ajouter une sonde","Add a probe"));
			ListParameter.Add(new Parameters("OWP_DeleteBoard_Name_Button","Supprimer une sonde","Delete a probe"));
			ListParameter.Add(new Parameters("OWP_DefaultProbeName","Sonde","Probe"));
			
		}
		public void I2CBoardPropertieWidget()
		{
			//INT
			ListParameter.Add(new Parameters("BI2CP_TVPro_TypePosition",0));
			ListParameter.Add(new Parameters("BI2CP_TVPro_ValuePosition",1));
			ListParameter.Add(new Parameters("BI2CP_TVPro_NamePostion",0));
			ListParameter.Add(new Parameters("BI2CP_TVPro_TypePostion",1));
			ListParameter.Add(new Parameters("BI2CP_TVPro_AdI2C0Postion",2));
			ListParameter.Add(new Parameters("BI2CP_TVPro_AdI2C1Postion",3));
			ListParameter.Add(new Parameters("BI2CP_TVChild_OpPin_PositionName",0));
			ListParameter.Add(new Parameters("BI2CP_TVChild_OpPin_PositionDirection",1));
			ListParameter.Add(new Parameters("BI2CP_TVChild_OpPin_PositionFallbackvalue",2));
			ListParameter.Add(new Parameters("BI2CP_TVChild_OpPin_PositionLinkInstance",3));
			ListParameter.Add(new Parameters("BI2CP_TVChild_OpPin_PositionID",4));
			
			//LANGAGE
			ListParameter.Add(new Parameters("BI2CP_LabTVChild_Name_Choose","Choisir une option","Choose an option"));
			ListParameter.Add(new Parameters("BI2CP_LabTVChild_Name_PinProperties","Propriétés de la carte :","Board properties :"));
			ListParameter.Add(new Parameters("BI2CP_LabTVChild_Name_PinList","Liste des broches de la carte :","Board pin list :"));
			ListParameter.Add(new Parameters("BI2CP_LabelProperties","Propriétés de la carte","Board properties"));
			ListParameter.Add(new Parameters("BI2CP_NoteLabel","Notes personnelles :","Personal notes :"));
			ListParameter.Add(new Parameters("BI2CP_TVPro_TypeLabel","Type","Type"));
			ListParameter.Add(new Parameters("BI2CP_TVPro_ValueLabel","Valeur","Value"));
			ListParameter.Add(new Parameters("BI2CP_TVPro_LabelLine1","Nom de la carte :","Board name :"));
			ListParameter.Add(new Parameters("BI2CP_TVPro_LabelLine2","Type de carte :","Board type :"));
			ListParameter.Add(new Parameters("BI2CP_TVPro_LabelLine3","Adresse I2C 0 :","I2C 0 Adress :"));
			ListParameter.Add(new Parameters("BI2CP_TVPro_LabelLine4","Adresse I2C 1 :","I2C 1 Adress :"));
			ListParameter.Add(new Parameters("BI2CP_TVOpt_Label","Paramètre","Settings"));			
			ListParameter.Add(new Parameters("BI2CP_TVOpt_OptionProperties","Propriétés","Properties"));
			ListParameter.Add(new Parameters("BI2CP_TVOpt_OptionPin","Broches","Pins"));
			ListParameter.Add(new Parameters("BI2CP_TVChild_OpPin_ID","ID","ID"));
			ListParameter.Add(new Parameters("BI2CP_TVChild_OpPin_Name","Nom de la broche","Pin name"));
			ListParameter.Add(new Parameters("BI2CP_TVChild_OpPin_Direction","Direction de la broche","Pin direction"));
			ListParameter.Add(new Parameters("BI2CP_TVChild_OpPin_Fallbackvalue","Valeur de repli","Fallback value"));
			ListParameter.Add(new Parameters("BI2CP_TVChild_OpPin_LinkInstance","Objet lié","Link device"));
			ListParameter.Add(new Parameters("BI2CP_EmptyLabel","Vide","Empty"));
			ListParameter.Add(new Parameters("BI2CP_TVChild_Up","Montée","Up"));
			ListParameter.Add(new Parameters("BI2CP_TVChild_Down","Descente","Down"));
			ListParameter.Add(new Parameters("BI2CP_TVChild_Stop","Arrêt","Stop"));			
			
			//MULTI-LANGAGE
			ListParameter.Add(new Parameters("BI2CP_BoardForbidderForShutter","4IN4DIM"));
			ListParameter.Add(new Parameters("BI2CP_TouchDelete","Delete"));			
		}
		public void InstancePropertieWidget()
		{
			//INT
			ListParameter.Add(new Parameters("IP_TVChild_Lighting_PositionName",0));
			ListParameter.Add(new Parameters("IP_TVChild_Lighting_PositionDefaultValue",1));
			ListParameter.Add(new Parameters("IP_TVChild_Lighting_PositionFade",2));
			ListParameter.Add(new Parameters("IP_TVChild_Lighting_PositionPath",3));
			ListParameter.Add(new Parameters("IP_TVChild_Lighting_PositionID",4));
			
			ListParameter.Add(new Parameters("IP_TVChild_Switch_PositionName",0));
			ListParameter.Add(new Parameters("IP_TVChild_Switch_PositionInverse",1));
			ListParameter.Add(new Parameters("IP_TVChild_Switch_PositionImpulsionTime",2));
			ListParameter.Add(new Parameters("IP_TVChild_Switch_PositionPath",3));
			ListParameter.Add(new Parameters("IP_TVChild_Switch_PositionID",4));	
			
			ListParameter.Add(new Parameters("IP_TVChild_Shutter_PositionName",0));
			ListParameter.Add(new Parameters("IP_TVChild_Shutter_PositionNumberOfOutput",1));
			ListParameter.Add(new Parameters("IP_TVChild_Shutter_PositionType",2));
			ListParameter.Add(new Parameters("IP_TVChild_Shutter_PositionTime",3));
			ListParameter.Add(new Parameters("IP_TVChild_Shutter_InitPositionTime",4));
			ListParameter.Add(new Parameters("IP_TVChild_Shutter_PositionPath1",5));
			ListParameter.Add(new Parameters("IP_TVChild_Shutter_PositionPath2",6));
			ListParameter.Add(new Parameters("IP_TVChild_Shutter_PositionPath3",7));
			ListParameter.Add(new Parameters("IP_TVChild_Shutter_PositionAppearenceOrder",8));
			ListParameter.Add(new Parameters("IP_TVChild_Shutter_PositionID",9));
			
			//LANGAGE
			ListParameter.Add(new Parameters("IP_LabTVChild_ButtonNewInstance","Ajout d'un objet ","Add device "));
			ListParameter.Add(new Parameters("IP_LabTVChild_ButtonDeleteInstance","Suppression d'un objet ","Delete device "));
			ListParameter.Add(new Parameters("IP_LabTVChild_Name_InstanceProperties","Propriétés de l'objet :","Device properties :"));
			ListParameter.Add(new Parameters("IP_NoteLabel","Notes personnelles :","Personal notes :"));
			ListParameter.Add(new Parameters("IP_LIG_Name","Nom de l'objet","Device name"));
			ListParameter.Add(new Parameters("IP_LIG_DefaultValue","Valeur par défaut","Default value"));
			ListParameter.Add(new Parameters("IP_LIG_Fade","Pente","Fade"));
			ListParameter.Add(new Parameters("IP_LIG_Path","Chemin de la broche","Pin path"));
			ListParameter.Add(new Parameters("IP_LIG_Id","Id","Id"));
			
			ListParameter.Add(new Parameters("IP_SelectInstance","Selectionner un objet","Select a device"));
			
			ListParameter.Add(new Parameters("IP_SWI_Name","Nom de l'objet","Device name"));
			ListParameter.Add(new Parameters("IP_SWI_Inverse","Inversion","Inverse"));
			ListParameter.Add(new Parameters("IP_SWI_ImpulsionTime","Temps d'impulsion","Impulsion Time"));
			ListParameter.Add(new Parameters("IP_SWI_Path","Chemin de la broche","Pin path"));
			ListParameter.Add(new Parameters("IP_SWI_Id","Id","Id"));	
			
			ListParameter.Add(new Parameters("IP_SHU_Name","Nom de l'objet","Device name"));
			ListParameter.Add(new Parameters("IP_SHU_NumberOfOutput","Nombre de sortie","Number of output"));
			ListParameter.Add(new Parameters("IP_SHU_Type","Type de volet","Shutter type"));
			ListParameter.Add(new Parameters("IP_SHU_Time","Temps de parcours","Travel Time"));
			ListParameter.Add(new Parameters("IP_SHU_InitTime","Offset mémoire position","Position memory offset"));
			ListParameter.Add(new Parameters("IP_SHU_PathUp","Chemin de la broche Montée","Up pin path"));
			ListParameter.Add(new Parameters("IP_SHU_PathDown","Chemin de la broche Descente","Down pin path"));
			ListParameter.Add(new Parameters("IP_SHU_PathStop","Chemin de la broche Arrêt","Stop pin path"));
			ListParameter.Add(new Parameters("IP_SHU_AppearenceOrder","Ordre d'apparition","Appearence order"));			
			ListParameter.Add(new Parameters("IP_SHU_Id","Id","Id"));	
			ListParameter.Add(new Parameters("IP_SHU_2_0","Montée / Descente","Up / Down"));
			ListParameter.Add(new Parameters("IP_SHU_2_1","Descente / Montée","Down / Up"));
			ListParameter.Add(new Parameters("IP_SHU_3_0","Montée / Descente / Arrêt","Up / Down / Stop"));
			ListParameter.Add(new Parameters("IP_SHU_3_1","Montée / Arrêt / Descente","Up / Stop / Down"));
			ListParameter.Add(new Parameters("IP_SHU_3_2","Descente / Arrêt / Montée","Down / Stop / Up"));
			ListParameter.Add(new Parameters("IP_SHU_3_3","Descente / Montée / Arrêt","Down / Up / Stop"));
			ListParameter.Add(new Parameters("IP_SHU_3_4","Arrêt / Descente / Montée","Stop / Down / Up"));
			ListParameter.Add(new Parameters("IP_SHU_3_5","Arrêt / Montée / Descente","Stop / Up / Down"));
			ListParameter.Add(new Parameters("IP_SHU_NotUsed","<--Non Utilisée-->","<--Not Used-->"));
		}
		public void CustomerEditWidget()
		{
			//INT
		
			//LANGAGE
			ListParameter.Add(new Parameters("CE_ItemMenuStandardCPP","Code standard c++","C++ standard code"));
			ListParameter.Add(new Parameters("CE_ItemMenuMacroProject","Macro project","Project macro"));
			ListParameter.Add(new Parameters("CE_ItemMenuMacroGlobal","Macro bloc","Bloc macro"));
			
			ListParameter.Add(new Parameters("CE_Header","En-tête","Header"));	
			ListParameter.Add(new Parameters("CE_VariableType","Type de variable","Variable type"));
			ListParameter.Add(new Parameters("CE_LoopAndCondition","Boucle et condition","Loop and condition"));
			ListParameter.Add(new Parameters("CE_Gate","Porte logique","Logical gate"));
			
			ListParameter.Add(new Parameters("CE_Project","Projet","Project"));	
				ListParameter.Add(new Parameters("CE_ProjectName","Nom du projet","Project name"));
				ListParameter.Add(new Parameters("CE_ProjectAuthor","Auteur du projet","Project author"));
				ListParameter.Add(new Parameters("CE_ProjectDateTimeCreation","D/H de création","D/T creation"));
				ListParameter.Add(new Parameters("CE_ProjectDateTimeModification","D/H de modification","D/T modification"));	
				ListParameter.Add(new Parameters("CE_ProjectVersion","Version du projet","Project version"));
				ListParameter.Add(new Parameters("CE_ProjectNote","Notes du projet","Project notes"));
			ListParameter.Add(new Parameters("CE_Node","Noeud","Node"));
				ListParameter.Add(new Parameters("CE_SMBName","Nom du noeud","Node name"));
				ListParameter.Add(new Parameters("CE_SMBType","Type du noeud","Node type"));
				ListParameter.Add(new Parameters("CE_SMBIP","IP du noeud","Node IP"));	
				ListParameter.Add(new Parameters("CE_SMBGTWIP","Passerelle IP du noeud","Node GatewayIP"));	
				ListParameter.Add(new Parameters("CE_SMBMAC","MAC du noeud","Node MAC"));
				ListParameter.Add(new Parameters("CE_BoardNumber","Nombre de ","Number of "));		
				ListParameter.Add(new Parameters("CE_LightingNumber","Nombre de lumière","Lighting number"));	
				ListParameter.Add(new Parameters("CE_SwitchNumber","Nombre de contact ","Switch number"));
				ListParameter.Add(new Parameters("CE_ShutterNumber","Nombre de volet","Shutter number"));			
				ListParameter.Add(new Parameters("CE_TempNumber","Nombre de sonde de tempétature","Temperature probe number"));	
			
			ListParameter.Add(new Parameters("CE_ConfigurationBoard","Configuration des cartes","Board configuration"));	
			ListParameter.Add(new Parameters("CE_ConfigurationSwitch","Configuration des commutateurs","Board configuration"));	
			ListParameter.Add(new Parameters("CE_ConfigurationShutter","Configuration des volets","Shutter configuration"));	
			ListParameter.Add(new Parameters("CE_ConfigurationLighting","Configuration des lumières","Lighting configuration"));	
			ListParameter.Add(new Parameters("CE_ConfigurationTemp","Configuration des sondes de température","Température probe configuration"));
			ListParameter.Add(new Parameters("CE_PreUpdate","Lecture des entrées","Input read"));
			ListParameter.Add(new Parameters("CE_PostUpdate","Ecriture des sorties","Output write"));
			
			ListParameter.Add(new Parameters("CE_Pulse","Impulsion","Pulse"));
			ListParameter.Add(new Parameters("CE_DoublePulse","DoubleImpulsion","DoublePulse"));
			ListParameter.Add(new Parameters("CE_On","Appuie","On"));
			ListParameter.Add(new Parameters("CE_Off","Relachement","Off"));
			ListParameter.Add(new Parameters("CE_OSR","FrontMontant","RisingEdge"));
			ListParameter.Add(new Parameters("CE_OSF","FrontDescendant","FallingEdge"));
			
			ListParameter.Add(new Parameters("CE_Toggle","Basculer","ToggleLight"));
			ListParameter.Add(new Parameters("CE_Tune","Regler","Tune"));
			ListParameter.Add(new Parameters("CE_StopTune","Figer","StopTune"));	
			
			ListParameter.Add(new Parameters("CE_OpenShutter","Ouvrir","Open"));
			ListParameter.Add(new Parameters("CE_CloseShutter","Fermer","Close"));
			ListParameter.Add(new Parameters("CE_StopShutter","Arreter","Stop"));	
			ListParameter.Add(new Parameters("CE_ToggleShutter","Basculer","ToggleShutter"));	
			
			ListParameter.Add(new Parameters("CE_ErrorScenario","/*##################### ATTENTION #####################\n\t\t\t\t\tChoisir un scénario\n#####################################################*/","/*##################### WARNING #####################\n\t\t\t\t\tChoose a scenario\n######################################################*/"));
		}
		public void ScenarioEditWidget()
		{
			//INT
			ListParameter.Add(new Parameters("SEW_TVVariable_PositionName",0));
			ListParameter.Add(new Parameters("SEW_TVVariable_PositionType",1));
			ListParameter.Add(new Parameters("SEW_TVVariable_PositionDefaultValue",2));
			ListParameter.Add(new Parameters("SEW_TVVariable_PositionNote",3));
			ListParameter.Add(new Parameters("SEW_TVVariable_PositionId",4));				
	
			ListParameter.Add(new Parameters("SEW_TVFunction_PositionName",0));
			ListParameter.Add(new Parameters("SEW_TVFunction_PositionType",1));
			ListParameter.Add(new Parameters("SEW_TVFunction_PositionNote",2));
			ListParameter.Add(new Parameters("SEW_TVFunction_PositionId",3));	
			
			ListParameter.Add(new Parameters("SEW_TVFunctionArgs_PositionNumber",0));
			ListParameter.Add(new Parameters("SEW_TVFunctionArgs_PositionType",1));
			ListParameter.Add(new Parameters("SEW_TVFunctionArgs_PositionName",2));			
			
			//LANGAGE
			ListParameter.Add(new Parameters("SEW_TVVariable_Name","Nom de la variable","Variable name"));
			ListParameter.Add(new Parameters("SEW_TVVariable_Type","Type de la variable","Variable Type"));
			ListParameter.Add(new Parameters("SEW_TVVariable_DefaultValue","Valeur par défaut","Default Value"));
			ListParameter.Add(new Parameters("SEW_TVVariable_Note","Commentaire","Note"));	
			ListParameter.Add(new Parameters("SEW_DeleteVariable","Supprimer","Delete"));	
			ListParameter.Add(new Parameters("SEW_DefaultNameVariable","NouvelleVariable","NewVariable"));	
			ListParameter.Add(new Parameters("SEW_MenuNewVariable","Nouvelle variable","New variable"));	
			ListParameter.Add(new Parameters("SEW_TooltipVariableTreeView","Clic droit pour ajouter une variable","Clic right to add a variable"));	
			ListParameter.Add(new Parameters("SEW_TooltipFunctionTreeView","Clic droit pour ajouter une fonction","Clic right to add a function"));	
			
			ListParameter.Add(new Parameters("SEW_TVFunction_Name","Nom de la fonction","Function name"));
			ListParameter.Add(new Parameters("SEW_TVFunction_Type","Type de la fonction","Function Type"));
			ListParameter.Add(new Parameters("SEW_TVFunction_Note","Commentaire","Note"));
			ListParameter.Add(new Parameters("SEW_DeleteFunction","Supprimer","Delete"));	
			ListParameter.Add(new Parameters("SEW_DefaultNameFunction","NouvelleFonction","NewFunction"));	
			ListParameter.Add(new Parameters("SEW_MenuNewFunction","Nouvelle fonction","New function"));
			
			ListParameter.Add(new Parameters("SEW_TVFunctionArgs_Number","N°","N°"));
			ListParameter.Add(new Parameters("SEW_TVFunctionArgs_Type","Type","Type"));
			ListParameter.Add(new Parameters("SEW_TVFunctionArgs_Name","Nom","Name"));	
			
			ListParameter.Add(new Parameters("SEW_SelectAFunction","Selectionner une fonction","Select a function"));	
			ListParameter.Add(new Parameters("SEW_VariableEmpty","ERREUR_VARIABLE","VARIABLE_ERROR"));	
			ListParameter.Add(new Parameters("SEW_FunctionEmpty","ERREUR_FONCTION","FUNCTION_ERROR"));
		}
		public void Question()
		{
			//LANGAGE
			ListParameter.Add(new Parameters("QuestionDeleteProject","Etes-vous sûre de vouloir fermer le projet ","Do you want to close the project "));
			ListParameter.Add(new Parameters("QuestionDeleteNode","Etes-vous sûre de vouloir supprimer le noeud ","Do you want to delete the node "));
			ListParameter.Add(new Parameters("QuestionDeleteNetwork","Etes-vous sûre de vouloir supprimer le réseau ","Do you want to delete the network "));
			ListParameter.Add(new Parameters("QuestionDeleteBoard","Etes-vous sûre de vouloir supprimer la carte ","Do you want to delete the board "));
			ListParameter.Add(new Parameters("QuestionDeleteInstance","Etes-vous sûre de vouloir supprimer l'objet ","Do you want to delete the device "));
			ListParameter.Add(new Parameters("QuestionErrorPreference","Il y a une erreur dans le fichier de préférences.\nLes valeurs par défauts seront appliquées\nVoulez-vous changer vos préférences ?","There are an error in the preferences file.\nDefault values ​​will be applied\nWould you like to change your preference?"));
			ListParameter.Add(new Parameters("QuestionDeleteCustomer","Etes-vous sûre de vouloir supprimer le fichier type ","Do you want to delete the customer file "));
			ListParameter.Add(new Parameters("QuestionDeleteScenario","Etes-vous sûre de vouloir supprimer le fichier scénario ","Do you want to delete the scenario file "));
			ListParameter.Add(new Parameters("QuestionDeleteVariable","Etes-vous sûre de vouloir supprimer la variable ","Do you want to delete the variable "));
			ListParameter.Add(new Parameters("QuestionDeleteFunction","Etes-vous sûre de vouloir supprimer la fonction ","Do you want to delete the function "));
			
			ListParameter.Add(new Parameters("ErrorPasswordOrInValidFile","Mot de passe Incorrect ou fichier invalide","Incorect password or invalid file"));
			ListParameter.Add(new Parameters("QuestionSaveProject","Voulez-vous sauvegader le projet ","Do you want to save the project "));
			ListParameter.Add(new Parameters("QuestionQuitProject","Voulez-vous quitter xPLduino - Manager ?","Do you want to quit xPLduino - Manager ?"));
		}
		public void AlertMessage()
		{
			//LANGAGE
			ListParameter.Add(new Parameters("NameProjectTooLong","Le nom de votre projet dépasse 16 caractères","The name of your project exceeds 16 characters"));
			ListParameter.Add(new Parameters("NameProjectEmpty","Le nom de votre projet n'est pas valide","The name of your project is not valid"));
			ListParameter.Add(new Parameters("AuthorProjectEmpty","Le nom de votre auteur n'est pas valide","The name of your author is not valid"));
			ListParameter.Add(new Parameters("NameNodeTooLong","Le nom de votre noeud dépasse 16 caractères","The name of your node exceeds 16 characters"));
			ListParameter.Add(new Parameters("NameNodeEmpty","Le nom de votre noeud n'est pas valide","The name of your node is not valid"));
			ListParameter.Add(new Parameters("IPNodeInvalid","Votre adresse IP n'est pas valide","Your IP adress is not valid"));
			ListParameter.Add(new Parameters("IdemProject","Un projet avec le même nom et le même chemin existe","A project with the same name and the same path exist"));
			ListParameter.Add(new Parameters("ProjectExistInPath","Un projet avec le même nom existe au chemin indiqué","A project with the same name exist in the indicate path"));
			ListParameter.Add(new Parameters("NodeExist","Ce noeud existe dans le projet","This node exist in the project"));
			ListParameter.Add(new Parameters("BoardExist","Cette carte existe dans ce réseau","This board exist in the network"));
			ListParameter.Add(new Parameters("InstanceExist","Cet objet existe dans ce noeud","This device exist in this node"));
			ListParameter.Add(new Parameters("I2CAdressNotPresent","Il n'y a pas deux adresses I2C pour ce type de carte","There are not two I2C adress for this board type"));
			ListParameter.Add(new Parameters("I2CAdressTooMuch","Adresse I2C trop grandre","I2C Adress too high"));
			ListParameter.Add(new Parameters("NameBoardEmpty","Le nom de votre carte n'est pas valide","The name of your board is not valid"));
			ListParameter.Add(new Parameters("NameCustomerEmpty","Le nom de votre fichier type n'est pas valide","The name of your customer file is not valid"));
			ListParameter.Add(new Parameters("NameBoardTooLong","Le nom de votre carte dépasse 16 caractères","The name of your board exceeds 16 characters"));
			ListParameter.Add(new Parameters("I2CAdressTooBig","Votre adresse I2C dépasse la limite","Your I2C adress exceeds the limite"));
			ListParameter.Add(new Parameters("I2CAdressNotCorrect","Le nom de votre adresse I2C n'est pas valide","The name of your I2C adress is not valid"));
			ListParameter.Add(new Parameters("NameInstanceTooLong","Le nom de votre objet dépasse 16 caractères","The name of your device exceeds 16 characters"));
			ListParameter.Add(new Parameters("NameInstanceEmpty","Le nom de votre objet n'est pas valide","The name of your device is not valid"));
			ListParameter.Add(new Parameters("DataNotValid","Votre donnée n'est pas valide","Your data is not valid"));
			ListParameter.Add(new Parameters("NameCustomerTooLong","Le nom de votre fichier type dépasse 16 caractères","The name of your customer file exceeds 16 characters"));			
			ListParameter.Add(new Parameters("OneCustomer","Il doit rester un fichier type","It must remains one customer file"));
			ListParameter.Add(new Parameters("OneScenario","Il doit rester un fichier scenario","It must remains one scenario file"));
			ListParameter.Add(new Parameters("NameScenarioEmpty","Le nom de votre scenario n'est pas valide","The name of your scenario file is not valid"));
			ListParameter.Add(new Parameters("NameVariableEmpty","Le nom de votre variable n'est pas valide","The name of your variable file is not valid"));
			ListParameter.Add(new Parameters("NameVariableExist","Le nom de votre variable éxiste","The name of your variable exist"));
			ListParameter.Add(new Parameters("NameVariableExistInFunction","Ce nom est utilisé pour une fonction","This name are using by a function"));
			ListParameter.Add(new Parameters("NameFunctionEmpty","Le nom de votre fonction n'est pas valide","The name of your function file is not valid"));
			ListParameter.Add(new Parameters("NameFunctionExist","Le nom de votre fonction éxiste","The name of your function exist"));
			ListParameter.Add(new Parameters("NameFunctionExistInVariable","Ce nom est utilisé pour une variable","This name are using by a variable"));		
			ListParameter.Add(new Parameters("NameArgsExistInArgs","Ce nom est utilisé pour un argument","This name are using by an argument"));	
			ListParameter.Add(new Parameters("NameExistInActionName","Ce nom est utilisé pour une action","This name are using by an action"));
			ListParameter.Add(new Parameters("NameExistInInstance","Ce nom est utilisé pour une instance","This name are using by an instance"));	
			ListParameter.Add(new Parameters("NameExistInFunctione","Ce nom est utilisé pour une function","This name are using by an function"));	
		}
		public void ModifyProject()
		{
			//INT
			ListParameter.Add(new Parameters("MoPo_ChoiceName",0));
			ListParameter.Add(new Parameters("MoPo_ChoiceAuthor",1));
			ListParameter.Add(new Parameters("MoPo_ChoicePath",2));
			ListParameter.Add(new Parameters("MoPo_ChoiceNote",3));
			ListParameter.Add(new Parameters("MoPo_ChoicePassword",4));
		}
		public void ModifyNode()
		{
			//INT
			ListParameter.Add(new Parameters("MoNo_ChoiceName",0));
			ListParameter.Add(new Parameters("MoNo_ChoiceIP",1));
			ListParameter.Add(new Parameters("MoNo_ChoiceGTWIP",2));
			ListParameter.Add(new Parameters("MoNo_ChoiceMac",3));
			ListParameter.Add(new Parameters("MoNo_ChoiceDHCP",4));	
			ListParameter.Add(new Parameters("MoNo_ChoiceNote",5));
			ListParameter.Add(new Parameters("MoNo_ChoiceType",6));
			ListParameter.Add(new Parameters("MoNo_ChoiceClock",7));
			ListParameter.Add(new Parameters("MoNo_Choice1Wire",8));
			ListParameter.Add(new Parameters("MoNo_ChoiceWebServer",9));
		}
		public void ModifyNetwork()
		{
			//INT
			ListParameter.Add(new Parameters("MoNet_ChoiceNote",0));
		}
		public void ModifyBoard()
		{
			//INT
			ListParameter.Add(new Parameters("MoBo_ChoiceName",0));
			ListParameter.Add(new Parameters("MoBo_ChoiceI2C0",1));
			ListParameter.Add(new Parameters("MoBo_ChoiceI2C1",2));
			ListParameter.Add(new Parameters("MoBo_Choice1WMac",3));	
			ListParameter.Add(new Parameters("MoBo_Choice1WPrecision",4));
			ListParameter.Add(new Parameters("MoBo_ChoiceNote",5));			
		}
		public void ModifyInstance()
		{
			//INT
			ListParameter.Add(new Parameters("MoIn_ChoiceName",0));
			ListParameter.Add(new Parameters("MoIn_ChoiceNote",1));
			ListParameter.Add(new Parameters("MoIn_ChoiceLIGDefaultValue",2));
			ListParameter.Add(new Parameters("MoIn_ChoiceLIGFade",3));	
			ListParameter.Add(new Parameters("MoIn_ChoiceSWIInverse",4));
			ListParameter.Add(new Parameters("MoIn_ChoiceSWIImpulsionTime",5));
			ListParameter.Add(new Parameters("MoIn_ChoiceSHUAppearenceOrder",6));
			ListParameter.Add(new Parameters("MoIn_ChoiceSHUNumberOfOutput",7));
			ListParameter.Add(new Parameters("MoIn_ChoiceSHUTravelTime",8));
			ListParameter.Add(new Parameters("MoIn_ChoiceSHUShutterType",9));
			ListParameter.Add(new Parameters("MoIn_ChoiceSHUInitTime",10));
			
			//LANGAGE
			ListParameter.Add(new Parameters("MoIn_InstanceEmpty","ERREUR_INSTANCE","INSTANCE_ERROR"));	
		}
		public void ModifyCustomer()
		{
			//INT
			ListParameter.Add(new Parameters("MoCu_ChoiceName",0));
			ListParameter.Add(new Parameters("MoCu_ChoiceData",1));		
			ListParameter.Add(new Parameters("MoCu_ChoiceNote",2));	
			ListParameter.Add(new Parameters("MoCu_ChoiceUse",3));	
			ListParameter.Add(new Parameters("MoCu_ChoiceScenario",4));	
		}
		public void ModifyScenario()
		{
			//INT
			ListParameter.Add(new Parameters("MoSc_ChoiceName",0));
			ListParameter.Add(new Parameters("MoSc_ChoiceData",1));		
			ListParameter.Add(new Parameters("MoSc_ChoiceNote",2));	
		}		
		public void ModifyPin()
		{
			//INT
			ListParameter.Add(new Parameters("MoPi_ChoiceFallbackValue",0));			
		}
		public void ModifyVariable()
		{
			//INT
			ListParameter.Add(new Parameters("MoVa_ChoiceName",0));
			ListParameter.Add(new Parameters("MoVa_ChoiceType",1));
			ListParameter.Add(new Parameters("MoVa_ChoiceValue",2));
			ListParameter.Add(new Parameters("MoVa_ChoiceNote",3));
		}
		public void ModifyFunction()
		{
			//INT
			ListParameter.Add(new Parameters("MoFu_ChoiceName",0));
			ListParameter.Add(new Parameters("MoFu_ChoiceType",1));
			ListParameter.Add(new Parameters("MoFu_ChoiceNote",2));	
			ListParameter.Add(new Parameters("MoFu_FunctionTypeArg1",3));		
			ListParameter.Add(new Parameters("MoFu_FunctionTypeArg2",4));		
			ListParameter.Add(new Parameters("MoFu_FunctionTypeArg3",5));
			ListParameter.Add(new Parameters("MoFu_FunctionTypeArg4",6));		
			ListParameter.Add(new Parameters("MoFu_FunctionTypeArg5",7));		
			ListParameter.Add(new Parameters("MoFu_FunctionTypeArg6",8));	
			ListParameter.Add(new Parameters("MoFu_FunctionNameArg1",9));		
			ListParameter.Add(new Parameters("MoFu_FunctionNameArg2",10));		
			ListParameter.Add(new Parameters("MoFu_FunctionNameArg3",11));
			ListParameter.Add(new Parameters("MoFu_FunctionNameArg4",12));		
			ListParameter.Add(new Parameters("MoFu_FunctionNameArg5",13));		
			ListParameter.Add(new Parameters("MoFu_FunctionNameArg6",14));		
			ListParameter.Add(new Parameters("MoFu_FunctionArgReorganisation",15));
			ListParameter.Add(new Parameters("MoFu_ChoiceData",16));
			
			//LANGAGE
			ListParameter.Add(new Parameters("Fun_BitlashComment","Action exécuté sur entrée de la commande \"custom\" dans le serial monitor","Action executed on receipt of the order \"custom\" in the serial monitor"));
		}
		public void XMLArgument()
		{
			//MULTI-LANGAGE
			ListParameter.Add(new Parameters("XMLBoards","Boards"));
			ListParameter.Add(new Parameters("XMLBoard","Board"));
			ListParameter.Add(new Parameters("XMLNetworks","Networks"));
			ListParameter.Add(new Parameters("XMLNetwork","Network"));
			ListParameter.Add(new Parameters("XMLDebug","Debug"));
			ListParameter.Add(new Parameters("XMLInstances","Instances"));
			ListParameter.Add(new Parameters("XMLInstance","Instance"));
		}
		public void ClicMouse()
		{
			//INT
			ListParameter.Add(new Parameters("LeftClic",1));
			ListParameter.Add(new Parameters("RightClic",3));
		}
		public void Instance()
		{
			//INT	
			ListParameter.Add(new Parameters("InstLightingDefaultValue",0));
			ListParameter.Add(new Parameters("InstLightingDefaultFade",0));
			ListParameter.Add(new Parameters("InstSwitchDefaultImpulsionTime",4));
			
			//LANGAGE
			ListParameter.Add(new Parameters("InstShutterType0","Impulsion", "Pulse"));
			ListParameter.Add(new Parameters("InstShutterType1","Permanent", "Permanent"));
			ListParameter.Add(new Parameters("InstShutterType2","Permanent Combiné", "Combined permanent"));
			
			//MULTI-LANGAGE
			ListParameter.Add(new Parameters("InstLightingName","LIGHTING"));
			ListParameter.Add(new Parameters("InstSwitchName","SWITCH"));
			ListParameter.Add(new Parameters("InstShutterName","SHUTTER"));
		}
		public void PinDirection()
		{
			//MULTI-LANGAGE
			ListParameter.Add(new Parameters("Direction1","IN"));
			ListParameter.Add(new Parameters("Direction2","OUT"));
		}
		public void PreferenceWindow()
		{
			//INT
			
			//LANGAGE
			ListParameter.Add(new Parameters("ErrorActionName","Un nom d'action est en double ou vide","An action name is empty or duplicate"));
			
			ListParameter.Add(new Parameters("PreferenceWindowTitle","Préférence","Preference"));
			ListParameter.Add(new Parameters("GeneralLabelTab","Général","General"));
			ListParameter.Add(new Parameters("StartTab","Démarrage","Start"));
			ListParameter.Add(new Parameters("VariousTab","Divers","Various"));
			ListParameter.Add(new Parameters("StartLabel","Démarrage","Start"));
			ListParameter.Add(new Parameters("EndLabel","Fermeture","Stop"));
			ListParameter.Add(new Parameters("DisplayWelcomeTab","Afficher la fenêtre de bienvenue","Display welcome window"));
			ListParameter.Add(new Parameters("ConfirmClose","Confirmer la fermeture","Close confirmation"));
			ListParameter.Add(new Parameters("VariousLabel","Divers","Various"));
			ListParameter.Add(new Parameters("BeepOnDelete","Emettre un bip sur les suppressions","Beep on deletions"));
			ListParameter.Add(new Parameters("BeepOnError","Emettre un bip sur les erreurs","Beep on errors"));
			ListParameter.Add(new Parameters("InterfaceLabelTab","Interface","Interface"));
			ListParameter.Add(new Parameters("InterfaceTab","Interface","Interface"));
			ListParameter.Add(new Parameters("OngletTab","Onglet","Tab"));
			ListParameter.Add(new Parameters("LangageLabel","Langue","Langage"));
			ListParameter.Add(new Parameters("LIGDefaultValueLabel","Valeur par défaut :","Default value :"));
			ListParameter.Add(new Parameters("DefaultValueLabel","Valeur par défaut d'une lumière :","Lighting default value :"));
			ListParameter.Add(new Parameters("FadeLabel","Pente :","Fade :"));
			ListParameter.Add(new Parameters("SWIDefaultValueLabel","Valeur par défaut :","Default value :"));
			ListParameter.Add(new Parameters("InverseLabel","Inversion :","Inverse :"));
			ListParameter.Add(new Parameters("ImpulstionTimeLabel","Temps d'impulstion :","Impulsion Time :"));
			ListParameter.Add(new Parameters("TemperatureLabelTab","Température","Temperature"));
			
			ListParameter.Add(new Parameters("LightingActionName","Nom des actions des lumières : ","Lighting actions name :"));
			ListParameter.Add(new Parameters("ToogleLabel","Basculer : ","Toogle :"));
			ListParameter.Add(new Parameters("TuneLabel","Régler : ","Tune :"));
			ListParameter.Add(new Parameters("StopTuneLabel","Figer : ","Stop :"));
			ListParameter.Add(new Parameters("SetValueLabel","Paramètrer : ","Set :"));
			
			ListParameter.Add(new Parameters("SwitchActionName","Nom des actions des commandes : ","Switch actions name :"));
			ListParameter.Add(new Parameters("ClicLabel","Impulsion : ","Clic :"));
			ListParameter.Add(new Parameters("DoubleClicLabel","Double Impulsion : ","Double clic :"));
			ListParameter.Add(new Parameters("OnLabel","Etat haut : ","On :"));		
			ListParameter.Add(new Parameters("OnFmLabel","Front montant : ","Rising edge :"));	
			ListParameter.Add(new Parameters("OffLabel","Etat bas : ","Off :"));		
			ListParameter.Add(new Parameters("OffFmLabel","Front descendant : ","Failing edge :"));				
			
			ListParameter.Add(new Parameters("SHUDefaultValueLabel","Valeur par défaut :","Default value :"));
			ListParameter.Add(new Parameters("SHUTypeLabel","Type de volet :","Shutter type :"));
			ListParameter.Add(new Parameters("SHUTravelTimeLabel","Temps de parcours :","Travel time :"));
			ListParameter.Add(new Parameters("SHUInitTimeLabel","Temps initial :","Initial time :"));
			ListParameter.Add(new Parameters("ShutterActionName","Nom des actions des volets : ","Shutter actions name :"));
			ListParameter.Add(new Parameters("SHUOpenLabel","Ouvrir : ","Open :"));
			ListParameter.Add(new Parameters("SHUCloseLabel","Fermer : ","Close :"));
			ListParameter.Add(new Parameters("SHUStopLabel","Arreter : ","Stop :"));		
			ListParameter.Add(new Parameters("SHUToggleLabel","Basculer : ","Toggle :"));	
			
			ListParameter.Add(new Parameters("TempActionName","Nom des actions des températures : ","Temperature actions name :"));
			ListParameter.Add(new Parameters("TEMPGetValue","Acquerir valeur : ","Get value :"));
		}
		public void HistoricText()
		{
			//LANGAGE
			ListParameter.Add(new Parameters("HTInit","Etat Initial","Initial State"));
			ListParameter.Add(new Parameters("HTNewProject","Nouveau projet : ","New project : "));
			ListParameter.Add(new Parameters("HTNewNode","Nouveau noeud : ","New node : "));
			ListParameter.Add(new Parameters("HTNewInstance","Nouvel objet : ","New device : "));
			ListParameter.Add(new Parameters("HTNewBoard","Nouvelle carte : ","New board : "));
			ListParameter.Add(new Parameters("HTNewNetwork","Nouveau réseau : ","New network : "));
			ListParameter.Add(new Parameters("HTNewCustomer","Nouveau fichier type : ","New customer file : "));
			ListParameter.Add(new Parameters("HTNewScenario","Nouvelle variable : ","New variable : "));
			ListParameter.Add(new Parameters("HTNewVariable","Nouveau fichier scenario : ","New scenario file : "));
			ListParameter.Add(new Parameters("HTNewFunction","Nouvelle fonction : ","New function : "));
			ListParameter.Add(new Parameters("HTDeleteProject","Fermeture projet : ","Close project : "));
			ListParameter.Add(new Parameters("HTOpenProject","Ouverture projet : ","Open project : "));
			ListParameter.Add(new Parameters("HTDeleteNode","Suppression noeud : ","Delete node : "));
			ListParameter.Add(new Parameters("HTDeleteNetwork","Suppression réseau : ","Delete network : "));
			ListParameter.Add(new Parameters("HTDeleteBoard","Suppression carte : ","Delete board : "));
			ListParameter.Add(new Parameters("HTDeleteInstance","Suppression objet : ","Delete device : "));
			ListParameter.Add(new Parameters("HTDeleteCustomer","Suppression du fichier customer : ","Delete customer file : "));
			ListParameter.Add(new Parameters("HTDeleteScenario","Suppression du fichier scenario : ","Delete scenario file : "));
			ListParameter.Add(new Parameters("HTDeleteVariable","Suppression de la variable : ","Delete variable : "));
			ListParameter.Add(new Parameters("HTDeleteFunction","Suppression de la fonction : ","Delete function : "));
			ListParameter.Add(new Parameters("HTModifyProjectName","Modification nom du projet : ","Modify project name : "));
			ListParameter.Add(new Parameters("HTModifyProjectAuthor","Modification auteur du projet : ","Modify project author : "));
			ListParameter.Add(new Parameters("HTModifyProjectPath","Modification chemin du projet : ","Modify project path : "));			
			ListParameter.Add(new Parameters("HTModifyProjectNote","Modification note du projet : ","Modify project note : "));	
			ListParameter.Add(new Parameters("HTModifyProjectPassword","Modification du mot de passe du projet : ","Modify project password : "));		
			ListParameter.Add(new Parameters("HTModifyNodeName","Modification nom du noeud : ","Modify node name : "));	
			ListParameter.Add(new Parameters("HTModifyNodeIP","Modification IP du noeud : ","Modify node IP : "));	
			ListParameter.Add(new Parameters("HTModifyNodeGTWIP","Modification IP gateway du noeud : ","Modify node gateway IP : "));	
			ListParameter.Add(new Parameters("HTModifyNodeDHCP","Modification DHCP du noeud : ","Modify node DHCP : "));
			ListParameter.Add(new Parameters("HTModifyNodeWebServer","Modification serveur web du noeud : ","Modify node web server : "));	
			ListParameter.Add(new Parameters("HTModifyNodeMac","Modification adresse mac du noeud : ","Modify node mac adress : "));
			ListParameter.Add(new Parameters("HTModifyNodeNote","Modification note du noeud : ","Modify node note : "));
			ListParameter.Add(new Parameters("HTModifyNodeType","Modification type du noeud : ","Modify node type : "));
			ListParameter.Add(new Parameters("HTModifyNodeClock","Modification type d'horloge du noeud : ","Modify node clock type : "));
			ListParameter.Add(new Parameters("HTModifyNode1Wire","Modification puce 1-Wire du noeud : ","Modify node clock type : "));
			ListParameter.Add(new Parameters("HTModifyNodeDebug","Modification debug du noeud : ","Modify node debug : "));		
			ListParameter.Add(new Parameters("HTModifyNetworkNote","Modification des notes du réseau : ","Modify network note :"));
			ListParameter.Add(new Parameters("HTModifyBoardName","Modification du nom de la carte : ","Modify board name : "));
			ListParameter.Add(new Parameters("HTModifyBoardI2C0","Modification de l'adresse I2C0 de la carte : ","Modify board I2C0 adress : "));
			ListParameter.Add(new Parameters("HTModifyBoardI2C1","Modification de l'adresse I2C1 de la carte : ","Modify board I2C1 adress : "));
			ListParameter.Add(new Parameters("HTModifyBoard1WireMac","Modification de l'adresse mac de la sonde de température : ","Modify probe mac adress : "));
			ListParameter.Add(new Parameters("HTModifyBoard1WirePrecision","Modification de la précsion de la sonde de température : ","Modify probe precision : "));
			ListParameter.Add(new Parameters("HTModifyBoardNote","Modification des notes de la carte : ","Modify board note : "));
			ListParameter.Add(new Parameters("HTModifyCustomerName","Modification du nom du fichier type : ","Modify customer file name : "));
			ListParameter.Add(new Parameters("HTModifyCustomerUse","Modification de l'utilisation du fichier type : ","Modify customer file use : "));
			ListParameter.Add(new Parameters("HTModifyCustomerData","Modification des données du fichier type : ","Modify customer data : "));
			ListParameter.Add(new Parameters("HTModifyCustomerNote","Modification des notes du fichier type : ","Modify customer file notes : "));
			ListParameter.Add(new Parameters("HTModifyScenarioName","Modification du nom du fichier scénario : ","Modify scenario file name : "));
			ListParameter.Add(new Parameters("HTModifyScenarioData","Modification des données du fichier scénario : ","Modify scenario data : "));
			ListParameter.Add(new Parameters("HTModifyScenarioNote","Modification des notes du fichier scénario : ","Modify scenario file notes : "));	
			ListParameter.Add(new Parameters("HTModifyVariableName","Modification du nom de la variable : ","Modify variable name : "));
			ListParameter.Add(new Parameters("HTModifyVariableType","Modification du type de la variable : ","Modify variable type : "));
			ListParameter.Add(new Parameters("HTModifyVariableValue","Modification de la valeur de la variable : ","Modify variable value : "));
			ListParameter.Add(new Parameters("HTModifyVariableNote","Modification des commentaires de la variable : ","Modify variable notes : "));
			
			ListParameter.Add(new Parameters("HTModifyInstanceName","Modification du nom de l'objet : ","Modify device name : "));
			ListParameter.Add(new Parameters("HTModifyInstanceNote","Modification des notes de l'objet : ","Modify device notes : "));
			ListParameter.Add(new Parameters("HTModifyInstanceLIGDefaultValue","Modification de la valeur par défaut d'une lumière de l'objet : ","Modify device lighting default value : "));
			ListParameter.Add(new Parameters("HTModifyInstanceLIGFade","Modification de la pente d'une lumière de l'objet : ","Modify device lighting fade : "));
			ListParameter.Add(new Parameters("HTModifyInstanceSWIInvese","Modification de l'invertion d'un bouton de l'objet : ","Modify device switch inverse : "));
			ListParameter.Add(new Parameters("HTModifyInstanceSWIImpulsionTime","Modification du temps d'impulsion d'un bouton de l'objet : ","Modify device switch impulsion time : "));
			ListParameter.Add(new Parameters("HTModifyInstanceSHUAppearenceOrder","Modification de l'ordre d'apparence d'un volet de l'objet : ","Modify device shutter appearence order : "));
			ListParameter.Add(new Parameters("HTModifyInstanceSHUNumberOfOutput","Modification du nombre de sortie d'un volet de l'objet : ","Modify device shutter number of output : "));
			ListParameter.Add(new Parameters("HTModifyInstanceSHUTravelTime","Modification du temps de parcours d'un volet de l'objet : ","Modify device shutter travel time : "));
			ListParameter.Add(new Parameters("HTModifyInstanceSHUInitTime","Modification du temps initial d'un volet de l'objet : ","Modify device shutter initial time : "));
			ListParameter.Add(new Parameters("HTModifyInstanceSHUShutterType","Modification du type d'un volet de l'objet : ","Modify device shutter type : "));
			ListParameter.Add(new Parameters("HTModifyInstancePinInBoard","Modification de l'objet associé de la broche : ","Modify link device of pin : "));
			ListParameter.Add(new Parameters("HTModifyPinFallBackValue","Modification de la valeur de repli : ","Modify fallback value : "));
			
			ListParameter.Add(new Parameters("HTModifyFunctionName","Modification du nom de la fonction : ","Modify function name : "));
			ListParameter.Add(new Parameters("HTModifyFunctionType","Modification du type de la fonction : ","Modify function type : "));
			ListParameter.Add(new Parameters("HTModifyFunctionNote","Modification des commentaires de la fonction : ","Modify function notes : "));	
			ListParameter.Add(new Parameters("HTModifyFunctionTypeArg1","Modification du type de l'argument 1 de la fonction : ","Modify argument 1 type of the function : "));	
			ListParameter.Add(new Parameters("HTModifyFunctionTypeArg2","Modification du type de l'argument 2 de la fonction : ","Modify argument 2 type of the function : "));	
			ListParameter.Add(new Parameters("HTModifyFunctionTypeArg3","Modification du type de l'argument 3 de la fonction : ","Modify argument 3 type of the function : "));	
			ListParameter.Add(new Parameters("HTModifyFunctionTypeArg4","Modification du type de l'argument 4 de la fonction : ","Modify argument 4 type of the function : "));	
			ListParameter.Add(new Parameters("HTModifyFunctionTypeArg5","Modification du type de l'argument 5 de la fonction : ","Modify argument 5 type of the function : "));	
			ListParameter.Add(new Parameters("HTModifyFunctionTypeArg6","Modification du type de l'argument 6 de la fonction : ","Modify argument 6 type of the function : "));	
		
			ListParameter.Add(new Parameters("HTModifyFunctionNameArg1","Modification du nom de l'argument 1 de la fonction : ","Modify argument 1 name of the function : "));
			ListParameter.Add(new Parameters("HTModifyFunctionNameArg2","Modification du nom de l'argument 2 de la fonction : ","Modify argument 2 name of the function : "));
			ListParameter.Add(new Parameters("HTModifyFunctionNameArg3","Modification du nom de l'argument 3 de la fonction : ","Modify argument 3 name of the function : "));
			ListParameter.Add(new Parameters("HTModifyFunctionNameArg4","Modification du nom de l'argument 4 de la fonction : ","Modify argument 4 name of the function : "));
			ListParameter.Add(new Parameters("HTModifyFunctionNameArg5","Modification du nom de l'argument 5 de la fonction : ","Modify argument 5 name of the function : "));
			ListParameter.Add(new Parameters("HTModifyFunctionNameArg6","Modification du nom de l'argument 6 de la fonction : ","Modify argument 6 name of the function : "));
			
			ListParameter.Add(new Parameters("HTModifyFunctionArgReorganisation1","Suppression de l'argument 1 de la fonction : ","Delete argument 1 name of the function : "));
			ListParameter.Add(new Parameters("HTModifyFunctionArgReorganisation2","Suppression de l'argument 2 de la fonction : ","Delete argument 2 name of the function : "));
			ListParameter.Add(new Parameters("HTModifyFunctionArgReorganisation3","Suppression de l'argument 3 de la fonction : ","Delete argument 3 name of the function : "));
			ListParameter.Add(new Parameters("HTModifyFunctionArgReorganisation4","Suppression de l'argument 4 de la fonction : ","Delete argument 4 name of the function : "));
			ListParameter.Add(new Parameters("HTModifyFunctionArgReorganisation5","Suppression de l'argument 5 de la fonction : ","Delete argument 5 name of the function : "));
			ListParameter.Add(new Parameters("HTModifyFunctionArgReorganisation6","Suppression de l'argument 6 de la fonction : ","Delete argument 6 name of the function : "));
			
			ListParameter.Add(new Parameters("HTModifyFunctionData","Modification des données de la fonction : ","Modify function data : "));
			
			ListParameter.Add(new Parameters("HTUpdateVersionAllProject","Mise à jour des versions de tous les projets","Updated versions of all projects"));
			ListParameter.Add(new Parameters("HTUpdateVersionOneProject","Mise à jour de la version du projet : ","Updated version of project :"));
		}
		public void ActionParameter()
		{
			//LANGAGE
			ListParameter.Add(new Parameters("AP_LIGToggle","Basculer","Toggle"));	
			ListParameter.Add(new Parameters("AP_LIGTune","Régler","Tune"));	
			ListParameter.Add(new Parameters("AP_LIGStop","Figer","Stop"));
			ListParameter.Add(new Parameters("AP_LIGSet","Paramétrer","Set"));	
			
			ListParameter.Add(new Parameters("AP_SWIPulse","Impulsion","Pulse "));	
			ListParameter.Add(new Parameters("AP_SWIDPulse","Double impulsion","Double pulse"));	
			ListParameter.Add(new Parameters("AP_SWIOn","Etat haut","On"));	
			ListParameter.Add(new Parameters("AP_SWIOnRe","Front montant","Rising edge"));	
			ListParameter.Add(new Parameters("AP_SWIOff","Etat bas","Off"));	
			ListParameter.Add(new Parameters("AP_SWIOffFe","Front descandant","Failing edge"));	
			
			ListParameter.Add(new Parameters("AP_SHUOpen","Ouvrir","Open"));	
			ListParameter.Add(new Parameters("AP_SHUClose","Fermer","Close"));	
			ListParameter.Add(new Parameters("AP_SHUStop","Arrêt","Stop"));		
			ListParameter.Add(new Parameters("AP_SHUToggle","Basculer","Toggle"));	
			
			ListParameter.Add(new Parameters("AP_TEMPName","Température","Temperature"));
			ListParameter.Add(new Parameters("AP_TEMPGet","Acquérir valeur","Get value"));
		}
		public void ClicRightMenu()
		{
			//LANGAGE
			ListParameter.Add(new Parameters("CRM_Variable","Variable","Variable"));			
			ListParameter.Add(new Parameters("CRM_Function","Fonction","Function"));
			ListParameter.Add(new Parameters("CRM_Argument","Argument","Argument"));	
		}
		
		
		public string ParamT(string _Name)
		{
			foreach(Parameters par in ListParameter)
			{
				if(par.Name == _Name)
				{
					if(Langage == "Français")
					{
						return par.FrenchValue;
					}
					else if(Langage == "English")
					{
						return par.EnglishValue;
					}
					else
					{
						return par.EnglishValue;
					}
				}
			}
			return _Name;
		}	
		
		public Int32 ParamI(string _Name)
		{
			foreach(Parameters par in ListParameter)
			{
				if(par.Name == _Name)
				{
					return par.Int32Value;
				}
			}
			return 0;			
		}
		
		public string ParamP(string _Name)
		{
			foreach(Parameters par in ListParameter)
			{
				if(par.Name == _Name)
				{				
					return par.MultiLangageValue;
				}
			}
			return "ERROR";			
		}
		
		public bool ChangeLangage(string _NewLangage)
		{
			if(_NewLangage != Langage)
			{
				Langage = _NewLangage;
				return true;
			}
			return false;
		}
	}
}