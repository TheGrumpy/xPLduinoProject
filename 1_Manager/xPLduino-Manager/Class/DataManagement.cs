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
using System.Diagnostics;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Threading;


using System.Collections;
using System.Text;
using System.Net;
using System.Net.Sockets;
using ICSharpCode.SharpZipLib.Zip;

namespace xPLduinoManager
{
	//Classe DataManagement
	//Classe permettant de gerer les autres classes entre elle
	//Elements :
	//	MainWindow mainwindow : permet d'acceder au fonction de mainwindows
	//
	//	List<Project> ListProject Liste des projets ouverts
	//  List<Node> ListNodes : Liste de noeud
	//	List<Network> ListNetwork : Liste des réseau pour un noeud choisit
	// 	List<Board> ListBoard : Liste de board pour un réseau choisit
	//	List<Pin> ListPin : Liste contenant l'ensemble des pin choisit
	//
	//	List<BoardsI2C> ListBoardsI2C : Liste contenant l'ensemble des cartes xPLduino
	//	List<Networks> ListNetworks : Liste contenant l'ensemble des réseaux
	//
	//	Int32 Project_Id : Id Project Maximum
	//	Int32 Node_Id : Id Node Maximum
	//	Int32 Network_Id : Id Network Maximum
	//	Int32 Board_Id : Id Board Maximum
	// 	Int32 Pin_Id : Id Pin Maximum
	//
	//Fonctions :
	//	ImportMainWindows : Importation de mainwindows
	//
	//	CreateNewProject : Fonction permettant de creer un nouveau projet
	//	AddNodeInProject : Fonction permettant d'ajouter un noeud à un projet
	//	AddNetworkInNode : Fonction permettant d'ajouter un réseau à un noeud
	//	AddBoardInNetwork : Fonctiom permettant d'ajoute une carte à un réseau
	//	AddPinsInBoard : Fonction permettant d'ajouter des pin à une carte
	//	AddLighting : Fonction permettant d'ajouter un lumière
	//	AddSwitch : Fonction permettant d'ajouter un bouton poussoir
	//	AddShutter : Fonction permettant d'ajouter un volet
	//
	//	DeleteProject : Fonction permettant de supprimer un projet
	//	DeleteNodeInProject : Fonction permettant la suppression d'un noeud dans un projet
	//	DeleteNetworkInNode : Fonction permettant la suppression d'un Réseau dans un noeud
	//	DeleteBoardInNetwork : Fonction permettant la suppression d'un carte dans un réseau
	//
	//	ModifyProject : Fonction permettant de modifier le nom et l'auteur du projet
	//	UpdateVesionProject : Fonction permettant de mettre à jour le numéro de version du projet
	// 	UpdateModificationDateAndTime : Fonction permettant de mettre à jour la date et l'heure de modification du projet
	//	ModifyNode : Fonction permettant de modifier un noeud
	//	ModifyNodeDebug : Fonction permettant de modifier les debug d'un noeud
	//	ModifyNetwork : Fonction permettant de modifier les paramètre d'un réseau
	//	UpdateInstanceUsed : Fonction permttant de savoir si une instance est utilisé
	//
	//	AddBoards : Fonction permettant de faire la liste des carte xPLduino à partir d'un fichier XML
	//	AddNetworks : Fonction permettant de faire la liste des réseaux
	//	ReadDebugXml : Fonction permettant de faire la liste des debbug
	//
	//	ReturnNewNameProject : Renomme un projet lors de la création d'un projet
	//	ReturnNewNameNode : Renomme un noeud lors de l'ajout n'un noeud à un projet
	//	ReturnNewNameBoard : Renomme une carte lors de l'ajout d'une carte à un réseau
	public class DataManagement
	{		
		public MainWindow mainwindow;
		public Param param;
		public Preference pref;
		
		public List<Project> ListProject;
		
		public List<List<Project>> CopyListProject;
		public int CountCopy;
		public int ViewCopy;		
		
		public List<Boards> ListBoards;
		public List<MenuTextEditor> ListMenuTextEditor;
		public List<MenuTextEditorScenario> ListMenuTextEditorScenario;
		public List<MenuTextEditorFunction> ListMenuTextEditorFunction;
		
		public static Int32 Project_Id = 1;
		public static Int32 Node_Id = 1;
		public static Int32 Network_Id = 1;
		public static Int32 Board_Id = 1;
		public static Int32 Pin_Id = 1;
		public static Int32 Instance_Id = 1;
		public static Int32 CustomerId = 1;
		public static Int32 ScenarioId = 1;
		public static Int32 Variable_Id = 1;
		public static Int32 Function_Id = 1;
		
		public Int32 CurrentProjectId = 0;
		
		public static int CountIdMenuCustomerEdit = 1;
		public static int IdParentColumn1 = 0;
		public static int IdParentColumn2 = 0;
		
		public static int CountIdMenuScenarioEdit = 1;		
		public static int IdParentColumn1Scenario = 0;
		public static int IdParentColumn2Scenario = 0;
		
		public static int CountIdMenuFunctionEdit = 1;		
		public static int IdParentColumn1Function = 0;
		public static int IdParentColumn2Function = 0;		
		
		private FileStream FileStream;
		private StreamWriter StreamWriter;
				
		private int _uptoFileCount;
		private int _totalFileCount;
		
		//Variable utile pour le remplacement des macros dans la création du Customer.ino
		public int CountIN4DIM4 = 0;
		public int Count8IN8R = 0;
		public int Count16IN = 0;
		public int Count8R = 0;
		public int CountLighting = 0;
		public int CountSwitch = 0;
		public int CountShutter = 0;
		public int CountTemp = 0;
		public string TextConfigBoard = "";
		public string TextConfigLighting = "";
		public string TextConfigSwitch = "";
		public string TextConfigShutter = "";
		public string TextConfigTemp = "";
		public string TextPreUpdate = "";
		public string TextPostUpdate = "";		
		public int[] CountBoard;
		public int[] InstanceTraity;
		public string[] TypeBoard;
		public int IndexCountBoard;
		
		//Variable pour le log
		public string CompilLogData = "";
		
		//Variable utilisé pour les commandes shell
	    private static ProcessStartInfo startInfo;
	    private static Process process;
	    private static StreamWriter writer;
	    private static StreamReader reader;
	    private static StreamReader errorReader;
		private static Thread OutputThread;				
		
	    public string lastReceivedUDPPacket="";
	    public string allReceivedUDPPackets="";
		UdpClient client; 
		
		
		
		public DataManagement (Param _param, Preference _pref)
		{
			this.param = _param;
			this.pref = _pref;
			ListProject = new List<Project>();	//Création de la liste des projets
			ListBoards = new List<Boards>(); //Création de la liste des cartes I2C
			ListMenuTextEditor = new List<MenuTextEditor>(); //Creation de la liste des élément du menu texteditor
			ListMenuTextEditorScenario = new List<MenuTextEditorScenario>(); //Création des éléments du menu texteditor
			ListMenuTextEditorFunction = new List<MenuTextEditorFunction>(); //Création des éléments du menu texteditor
			
			CopyListProject = new List<List<Project>>();
			
			CountCopy = 0;
			ViewCopy = 0;
			for(int i=0;i<pref.NumberOfProjectCopy;i++)
			{
				CopyListProject.Add(new List<Project>());
			}
			AddBoards(); 
			AddElementInListMenuTextEditor();
		}	
	
		public void CreateConfiguration()
		{
		/*
			CreateNewProject("Project_0","Ludovic","Path"); //PROJECT Id 1

			CreateNewProject("Project_1","Ludovic","Path"); //PROJECT Id 1
			CreateNewProject("Project_2","Ludovic","Path"); //PROJECT Id 1
			CreateNewProject("Project_3","Ludovic","Path"); //PROJECT Id 1
			CreateNewProject("Project_4","Ludovic","Path"); //PROJECT Id 1
			CreateNewProject("Project_5","Ludovic","Path"); //PROJECT Id 1
			CreateNewProject("Project_6","Ludovic","Path"); //PROJECT Id 1			
		*/
		/*	CreateNewProject("Project_0","Ludovic","Path"); //PROJECT Id 1
				AddNodeInProject("smb_0",Project_Id-1); //SMB Id 1
					AddNetworkInNode("I2C",Node_Id-1); //I2C Id 1
						AddBoardInNetwork("IN8R8","8in8r_0",Network_Id-1);
						AddBoardInNetwork("IN16","16in_0",Network_Id-1);
						AddBoardInNetwork("IN8R8","8in8r_1",Network_Id-1);
					AddNetworkInNode("1-Wire",Node_Id-1);
						AddBoardInNetwork("TEMPERATURE","Sonde_Cuisine",Network_Id-1);
						AddBoardInNetwork("TEMPERATURE","Sonde_SAM",Network_Id-1);
			
					
					AddInstanceInNode("LIGHTING","Lamp_Cuisine",Node_Id-1);
					AddInstanceInNode("LIGHTING","Lamp_Salon",Node_Id-1);
					AddInstanceInNode("LIGHTING","Lamp_Garage",Node_Id-1);
					
					AddInstanceInNode("SWITCH","Bp_Cuisine",Node_Id-1);
					AddInstanceInNode("SWITCH","Bp_Salon",Node_Id-1);
					AddInstanceInNode("SWITCH","Bp_Garage",Node_Id-1);
					
					AddInstanceInNode("SHUTTER","Volet_Cuisine",Node_Id-1);
					AddInstanceInNode("SHUTTER","Volet_Salon",Node_Id-1);
					AddInstanceInNode("SHUTTER","Volet_Garage",Node_Id-1);
			
				AddNodeInProject("smb_1",Project_Id-1); //SMB Id 2
			CreateNewProject("Project_1","Ludovic VOGEL","Path"); //Id 2
				AddNodeInProject("smb_0",Project_Id-1); //SMB Id 3
				AddNodeInProject("smb_1",Project_Id-1); //SMB Id 4	*/
			
			CreateNewProject("Project_0","Ludovic VOGEL","/home/ludovic/Bureau/TestPath"); //PROJECT Id 1			
				AddNodeInProject("smb_0",Project_Id-1); //SMB Id 1
					//AddNetworkInNode("I2C",Node_Id-1); //I2C Id 1
						AddBoardInNetwork("IN8R8","8in8r_0",Network_Id-2);
			
					AddInstanceInNode("LIGHTING","Lamp_Cuisine",Node_Id-1);
					AddInstanceInNode("LIGHTING","Lamp_Salon",Node_Id-1);
					AddInstanceInNode("LIGHTING","Lamp_Garage",Node_Id-1);
					
					AddInstanceInNode("SWITCH","Bp_Cuisine",Node_Id-1);
					AddInstanceInNode("SWITCH","Bp_Salon",Node_Id-1);
					AddInstanceInNode("SWITCH","Bp_Garage",Node_Id-1);			
			
					AddInstanceInNode("SHUTTER","Volet_Cuisine",Node_Id-1);
					AddInstanceInNode("SHUTTER","Volet_Salon",Node_Id-1);
					AddInstanceInNode("SHUTTER","Volet_Garage",Node_Id-1);			
		
					//AddVariableInScenario("Test","bool",1);
					//AddVariableInScenario("Test_1","bool",1);
			
					//AddFunctionInScenario("Test","void",1);
			
						mainwindow.InitPanedAndMouvementAuthor(); 
	
				
		}
		
//################Fonction d'import de class #################################################		

		//Fonction ImportMainWindows
		//Fonction permettant l'import de mainwindows
		public void ImportMainWindows(MainWindow _mainwindows)
		{
			mainwindow = _mainwindows; //Depuis la classe datamanagement, nous allons pouvoir lire et écrire dans la classe mainwindows
			CopyProject("HTInit","");
		}
		
//################Fonction permettant la création d'un nouveau projet##########################
		
		//Fonction CreateNewProject
		//Fonction permettant de creer un nouveau projet
		//Arguments :
		//	string _ProjectName : Nom du projet
		//	string _ProjectAuthor : Nom de l'auteur
		public bool CreateNewProject(string _ProjectName, string _ProjectAuthor,string _DirectoryPath)
		{	
			ListProject.Add(new Project(Project_Id,_ProjectName,_ProjectAuthor,_DirectoryPath,param,pref)); //Ajout d'un nouveau projet dans la liste
			CurrentProjectId = Project_Id;
			Project_Id++; //Incrémentation de l'id project
			mainwindow.UpdateEplorerTreeView(); //Mise à jour de l'explorer projet
			mainwindow.UpdateStatusBar();
			mainwindow.UpdateComboboxSelectUsb();			
			mainwindow.Sensitive = true; //On rend la main à la page principal
			CopyProject("HTNewProject",_ProjectName);
			return true;
		}
			
//################Fonction permettant la création des instances ################################	
		
		//Fonction AddNodeInProject
		//Fonction permettant l'ajout d'un nouveau noeud dans le projet
		//Argument : 
		//	string _Name : le nom du nouveau noeud
		//	Int32 _Project_Id : Id du projet où nous devons ajouter un noeud
		//Retour
		//	bool : Correct si la création est réussit
		public bool AddNodeInProject(string _Name, Int32 _Project_Id)
		{ 	
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{
				if(Pro.Project_Id == _Project_Id) //Si le projet porte l'id que nous avons passé en paramètre
				{			
					_Name = ReturnCorrectName(_Name);
					Pro.AddNodeInProject(_Name,Node_Id); //Nous ajoutons un nouveau noeud au projet
					
					AddDebudInNode(Node_Id); //Ajoute les debug au noeud a partir d'une liste définit
					
					//On ajoute les réseaux directement sans passé par une fenêtre d'ajout (Gain de temps à la conception)
					AddNetworkInNode("I2C",Node_Id);
					AddNetworkInNode("1-Wire",Node_Id);
					
					//On rajoute le fichier customer et scénario à la création d'un noeud
					AddCustomerInNode(param.ParamP("CustomerInoName"),Node_Id,false); //On ajouter le fichier customer de base à partir d'un fichier racine	
					AddScenarioInNode(param.ParamP("ScenarioName"),Node_Id,false); //On ajoute un fichier de scénarion de base	
					
					
					CopyProject("HTNewNode",Pro.Project_Name + "/" + _Name);
					Node_Id++;	//On incrémente l'Id noeud
					Pro.ProjectIsSave = false;
					mainwindow.UpdateStatusBar();							
					mainwindow.Sensitive = true; //Activation de la fenetre principale	
					mainwindow.UpdateComboboxSelectUsb();
					return true;
				}
			}
			return false;
		}		
		
		//Fonction AddNetworkInNode
		//Fonction permettant l'ajout d'un nouveau réseau dans un noeud
		//Argument : 
		//	string _Type : le type du réseau
		//	Int32 _Node_Id : l'id noeud où nous voulons ajouter le réseau	
		//Retour
		//	bool : Correct si la création est réussit		
		public bool AddNetworkInNode(string _Type,Int32 _Node_Id)
		{
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{				
				foreach (Node node in Pro.ReturnListNode())	//Pour chaque noeud présent
				{		
					if(node.Node_Id == _Node_Id)//Si l'id du noeud mis en paramètre = l'id trouvé alors on ajoute un réseau
					{
						foreach(Network Net in node.ReturnListNetwork())//Pour chaque réseau dans la liste des réseau pour ce noeud
						{
							if(Net.Network_Type == _Type)//Si le type de réseau et celui que nous avons passé en paramètre
							{
								return false;	
							}
						}
						node.AddNetworkInNode(Network_Id,_Type); //Nous ajoutons un réseau au noeud
						Network_Id++;	//On incrémente l'Id Network
						mainwindow.UpdateEplorerTreeView(); //Mise à jour de l'explorer treeview
						mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
						mainwindow.Sensitive = true; //Activation de la fenetre principale						
						//CopyProject("HTNewNetwork",Pro.Project_Name + "/" + node.Node_Name + "/" + _Type);
						Pro.ProjectIsSave = false;
						mainwindow.UpdateStatusBar();								
						return true;					
					}
				}
			}
			return false;
		}	
		
		//Fonction AddInstanceInNode
		//Fonction permettant l'ajout d'une instance à un noeud
		public bool AddInstanceInNode(string _Type,string _Name,Int32 _Node_Id)
		{
			foreach(Project Pro in ListProject) //Pour chaque projet de la liste
			{				
				foreach (Node node in Pro.ReturnListNode())	//Pour chaque noeud présent
				{		
					if(node.Node_Id == _Node_Id)	//Si l'id du noeud mis en paramètre = l'id trouvé alors on ajoute un réseau
					{		
						_Name = ReturnCorrectName(_Name);
						node.AddInstanceInNode(Instance_Id,_Type,_Name); //Nous ajoutons une instance à un noeud
						Instance_Id++; //Nous incrémentons l'instance ID
						
						mainwindow.UpdateEplorerTreeView(); //Mise à jour de l'explorer treeview
						mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
						mainwindow.Sensitive = true; //Activation de la fenetre principale			
						CopyProject("HTNewInstance",Pro.Project_Name + "/" + node.Node_Name + "/" + _Type +  "/" + _Name);
						Pro.ProjectIsSave = false;
						mainwindow.UpdateStatusBar();								
						return true;
					}
				}
			}
			return false;
		}
		
		//Fonction AddBoardInNetwork
		//Fontcion permettant l'ajout d'une nouvelle carte sur un réseau
		//Argument :	
		//	string _Type : le type de la carte
		//	string _Name : le nom de la carte
		//	Int32 _I2C_0 : Adresse I2C 0
		//	Int32 _I2C_1 : Adresse I2C 1
		//	Int32 _Network_Id : l'Id réseau où nous voulons ajouter
		//Retour
		//	bool : Correct si la création est réussit
		public bool AddBoardInNetwork(string _Type, string _Name,Int32 _Network_Id)
		{
			foreach(Project Pro in ListProject) //Pour chaque projet de la liste
			{			
				foreach (Node node in Pro.ReturnListNode())	//Pour chaque noeud présent						
				{	
					foreach (Network net in node.ReturnListNetwork())	//Pour chaque réseau présent de la liste chargé						
					{
						if(net.Network_Id == _Network_Id)	//Si l'Id du réseau == Id demandé
						{
							_Name = ReturnCorrectName(_Name);
							net.AddBoardInNetwork(Board_Id,_Type,_Name); //cf Network
							AddPinsInBoard(_Type,Board_Id); //On appele fonction permettant d'ajouter des broches à une carte
							Board_Id++;	//On incrémente l'id board
							mainwindow.UpdateEplorerTreeView(); //On fait la mise à jour de l'explorer treeview
							mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
							mainwindow.Sensitive = true; //Activation de la fenetre principale
							CopyProject("HTNewBoard",Pro.Project_Name + "/" + node.Node_Name + "/" + net.Network_Type + "/" + _Type + "/" + _Name);
							Pro.ProjectIsSave = false;
							mainwindow.UpdateStatusBar();									
							return true;
						}
					}
				}
			}
			return false;
		}
		
		//Fonction AddPinsInBoard
		//Fonction permettant d'ajouter des broches au carte
		//Argument :	
		//	string _Type : le type de la carte
		//	Int32 _Board_Id : Id de la carte où nous devons ajouter des broches
		public bool AddPinsInBoard(string _Type, Int32 _Board_Id)
		{
			foreach (Project Pro in ListProject)//Pour chaque projet de la liste
			{
				foreach (Node node in Pro.ReturnListNode()) //Pour chaque noeud de la liste des noeud
				{
					foreach (Network net in node.ReturnListNetwork())	//Pour chaque réseau présent de la liste chargé						
					{
						foreach(Board board in net.ReturnListBoard())	//Pour chaque carte présente dans la liste chargé
						{
							if(board.Board_Id == _Board_Id)	//Si le réseau trouvé à pour Id celui demandé
							{
								foreach(Boards boards in ListBoards) //Pour chaque carte de la liste des CarteI2C xPLduino
								{
									if(boards.Type == _Type)	//Si le type de la carte demandé est égale à une carte de la liste
									{
										for(int i=0;i<=boards.NumberOfInputs-1;i++) //on boucle sur le nombre d'entrée
										{
											board.AddPinInBoard(Pin_Id,boards.PrefixIN + i,param.ParamP("Direction1"),i); //On créer chaque broche avec le nom qui va bien
											Pin_Id++;	// On incrémente l'id pin
										}
										for(int i=0;i<=boards.NumberOfOutputs-1;i++)//on boucle sur le nombre de sortie
										{
											board.AddPinInBoard(Pin_Id,boards.PrefixOUT + i,param.ParamP("Direction2"),i); //On créer chaque broche avec le nom qui va bien
											Pin_Id++;	// On incrémente l'id pin
										}
										return true;	
									}
								}
							}
						}
					}
				}
			}
			return false;
		}
		
		//Fonction ReadDebugXml
		//Fonction permettant de lire les paramètres de debbugage dans le fichier xml
		public bool AddDebudInNode(Int32 _NodeId)
		{
			foreach(Project Pro in ListProject)
			{
				foreach(Node node in Pro.ReturnListNode())
				{
					if(node.Node_Id == _NodeId)
					{
				        node.AddDebugInNode("DEBUG_LIGHTING_XPL","Active l'affichage des messages xPL lighting dans la console","Empty");
				        node.AddDebugInNode("DEBUG_LIGHTING_CORE","Active l'affichage de détails de traitement des lighting dans la console","Empty");
				        node.AddDebugInNode("DEBUG_SHUTTER_XPL","Active l'affichage des messages xPL Shutter dans la console","Empty");
				        node.AddDebugInNode("DEBUG_SHUTTER_CORE","Active l'affichage de détails de traitement des shutter dans la console","Empty");
				        node.AddDebugInNode("DEBUG_SWITCH_XPL","Active l'affichage des messages xPL Switch dans la console","Empty");
				        node.AddDebugInNode("DEBUG_SWITCH_CORE","Active l'affichage de détails de traitement des switch dans la console","Empty");
				        node.AddDebugInNode("DEBUG_Temp_OW","Active l'affichage des messages xPL OneWire dans la console","Empty");
				        node.AddDebugInNode("DEBUG_ETHERNET","Active l'affichage de détails d'init de l'Ethernet dans la console","Empty");
				        node.AddDebugInNode("DEBUG_INCOMING","Active l'affichage de tous les messages xPL entrant dans la console","Empty");
				        node.AddDebugInNode("DEBUG_OUTCOMING","Active l'affichage de tous les messages xPL sortant dans la console","Empty");
				        node.AddDebugInNode("DEBUG_I2C_DIMMER","Active l'affichage de détails des échanges entre le contrôleur et la carte d'extension dans la console","Empty");
				        node.AddDebugInNode("DEBUG_BOARD_IN_16","Active l'affichage de détails des échanges entre le contrôleur et la carte d'extension dans la console","Empty");
				        node.AddDebugInNode("DEBUG_BOARD_R8","Active l'affichage de détails des échanges entre le contrôleur et la carte d'extension dans la console","Empty");   
				        node.AddDebugInNode("DEBUG_I2C_IN8R8","Active l'affichage de détails des échanges entre le contrôleur et la carte d'extension dans la console","Empty");
				        node.AddDebugInNode("DEBUG_TELEINFO_CORE","Active l'affichage de détails de traitement de la téléinformation dans la console","Empty");                    
				        node.AddDebugInNode("DEBUG_TELEINFO_SERIAL","Active l'affichage de détails de la communication série de la téléinformation dans la console","Empty");        
				        node.AddDebugInNode("DEBUG_TELEINFO_XPL","Active l'affichage des messages xPL de la téléinformation dans la console","Empty"); 
						//node.AddDebugInNode("DEBUG_DEVICERF","Active l'affichage des info sur les commande RF","Empty");
					}
				}
			}			
			return true;				
		}			
			
		//Fonction AddCustomerInNode
		//Fonction permettant d'ajouter le fichier customer de base dans un noeud
		public bool AddCustomerInNode(string _Name, Int32 _NodeId, bool ActivateCopyProject)
		{
			bool _DefaultUse = false;
			foreach(Project Pro in ListProject)
			{
				foreach(Node node in Pro.ReturnListNode())
				{
					if(node.Node_Id == _NodeId)
					{
						if(node.Customer_.Count == 0)
						{
							_DefaultUse = true;
						}
						else
						{
							_DefaultUse = false;
						}		
						node.AddCustomerInNode(CustomerId,_Name,ReturnCustomerByFileSource(),_DefaultUse);
						CustomerId++;
						
						if(ActivateCopyProject)
							CopyProject("HTNewCustomer",Pro.Project_Name + "/" + node.Node_Name + "/" + _Name);
						
						mainwindow.Sensitive = true;
						mainwindow.UpdateWidgetInTab();
						mainwindow.UpdateEplorerTreeView();
						Pro.ProjectIsSave = false;
						mainwindow.UpdateStatusBar();															
						return true;
					}
				}
			}
			return false;
		}
		
			//Fonction ReturnCustomerByFileSource
			//Permet de retourner un texte a partir du fichier customer sinon texte d'erreur
			private string ReturnCustomerByFileSource()
			{
				if(!File.Exists(Environment.CurrentDirectory + param.ParamP("PathCustomer"))) 
				{
				 return param.ParamT("ErrorPathCustomer");
				}				
				return System.IO.File.ReadAllText(Environment.CurrentDirectory + param.ParamP("PathCustomer"));
			}
		
		//Fonction AddScenarioInNode
		//Fonction permettant d'ajouter un scénario dans la liste des scénario d'un noeud
		public bool AddScenarioInNode(string _Name, Int32 _NodeId, bool ActivateCopyProject)
		{
			foreach(Project Pro in ListProject)
			{
				foreach(Node node in Pro.ReturnListNode())
				{
					if(node.Node_Id == _NodeId)
					{
						_Name = ReturnCorrectName(_Name);
						node.AddScenarioInNode(ScenarioId,_Name,"//\n");
						
						foreach(Scenario sce in node.ReturnListScenario())
						{
							if(sce.ScenarioId == ScenarioId)
							{
								sce.CreateBitlashCustomFunction(Function_Id).FunctionNote = param.ParamT("Fun_BitlashComment");
								
								Function_Id++;
							}
						}
						
						ScenarioId++;
						
						if(ActivateCopyProject)
							CopyProject("HTNewScenario",Pro.Project_Name + "/" + node.Node_Name + "/" + _Name);
						
						mainwindow.Sensitive = true;
						mainwindow.UpdateWidgetInTab();
						mainwindow.UpdateEplorerTreeView();
						Pro.ProjectIsSave = false;
						mainwindow.UpdateStatusBar();																					
						return true;						
					}
				}
			}
			return false;
		}
		
			//Fonction AddVariableInScenario
			//Fonction permettant d'ajouter une variable à un scenario
			public bool AddVariableInScenario(string _Name, string _Type, Int32 _ScenarioId)
			{
				foreach(Project Pro in ListProject)
				{
					foreach(Node node in Pro.ReturnListNode())
					{			
						foreach(Scenario sce in node.ReturnListScenario())
						{
							if(sce.ScenarioId == _ScenarioId)
							{
								_Name = ReturnCorrectName(_Name);
								sce.AddVariableInScenario(Variable_Id,_Name,_Type,0);
								Variable_Id++;
								CopyProject("HTNewScenario",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + _Name);
								mainwindow.UpdateWidgetInTab();
								mainwindow.UpdateEplorerTreeView();
								Pro.ProjectIsSave = false;
								mainwindow.UpdateStatusBar();																						
								return true;								
							}
						}
					}
				}
			return false;
			}
		
			//Fonction AddFunctionInScenario
			//Fonction permettant d'ajouter une fonction dans un scénario
			public bool AddFunctionInScenario(string _Name, string _TypeReturn, Int32 _ScenarioId)
			{
				foreach(Project Pro in ListProject)
				{
					foreach(Node node in Pro.ReturnListNode())
					{			
						foreach(Scenario sce in node.ReturnListScenario())
						{
							if(sce.ScenarioId == _ScenarioId)
							{
								_Name = ReturnCorrectName(_Name);
								sce.AddFunctionInScenario(Function_Id,_Name,_TypeReturn);
								Function_Id++;
								CopyProject("HTNewFunction",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + _Name);
								mainwindow.UpdateWidgetInTab();
								mainwindow.UpdateEplorerTreeView();
								Pro.ProjectIsSave = false;
								mainwindow.UpdateStatusBar();																						
								return true;								
							}
						}
					}
				}
			return false;
			}
		
//################Fonction permettant la destruction des instances #############################
		
		//Fonction DeleteProject
		//Arguments :
		//	Int32 _Project_Id : Id du projet à supprimer
		public bool DeleteProject(Int32 _Project_Id)
		{
			foreach(Project Pro in ListProject) //Pour chaque projet de la liste
			{
				if(Pro.Project_Id == _Project_Id) //Si le numéro du projet est celui que nous avons passé en paramètre
				{			
					string NameProject = Pro.Project_Name;
					ListProject.Remove(Pro);//Nous supprimons le projet de la liste
					mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
					mainwindow.CloseTabInCaseOfDelete();//Permet d'enlever le tab si celui-ci est ouvert							
					UpdateInstanceUsed();
					mainwindow.UpdateEplorerTreeView(); //On fait la mise à jour de l'explorer treeview
					mainwindow.Sensitive = true; //Activation de la fenetre principale	
					CopyProject("HTDeleteProject",NameProject);
					CurrentProjectId = 0;
					mainwindow.UpdateStatusBar();
					return true;
				}
			}
			return false;
		}
		
		//Fonction DeleteNodeInProject
		//Fonction permettant de supprimer un noeud et tous ses enfants
		//Arguments : 
		//	Int32 Node_Id : id du noeud à supprimer
		//Retour :
		//	bool : Vrai si réussit
		public bool DeleteNodeInProject(Int32 _Node_Id)
		{
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{		
				foreach (Node node in  Pro.ReturnListNode()) //Pour chaque noeud de la liste des noeud
				{		
					if(node.Node_Id == _Node_Id) //Si le noeud trouvé à pour Id celui demandé
					{		
						string NameNode = node.Node_Name;
						Pro.Node_.Remove(node);//On supprime le noeud
						mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
						mainwindow.CloseTabInCaseOfDelete();//Permet d'enlever le tab si celui-ci est ouvert								
						UpdateInstanceUsed();
						mainwindow.UpdateEplorerTreeView(); //On fait la mise à jour de l'explorer treeview
						mainwindow.Sensitive = true; //Activation de la fenetre principale							
						CopyProject("HTDeleteNode",Pro.Project_Name + "/" + NameNode);
						Pro.ProjectIsSave = false;
						mainwindow.UpdateStatusBar();
						return true;			
					}
				}	
			}
			return false;
		}	
		
		//Fonction DeleteNetworkInNode
		//Fonction permettant de supprimer un réseau et tous ses enfants
		//Arguments :
		//	Int32 _Network_Id : id de réseau à supprimer
		//Retour :
		//	bool : Vrai si réussit		
		public bool DeleteNetworkInNode(Int32 _Network_Id)
		{
			foreach(Project Pro in ListProject)
			{
				foreach (Node node in Pro.ReturnListNode()) //Pour chaque noeud de la liste des noeud
				{
					foreach (Network net in node.ReturnListNetwork())	//Pour chaque réseau dans la liste des réseau						
					{
						if(net.Network_Id == _Network_Id)	//Si le réseau trouvé à pour Id celui demandé
						{				
							string TypeNetwork = net.Network_Type;
							node.Network_.Remove(net);//on supprime le réseau
							mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
							mainwindow.CloseTabInCaseOfDelete();//Permet d'enlever le tab si celui-ci est ouvert								
							UpdateInstanceUsed();
							mainwindow.UpdateEplorerTreeView(); //On fait la mise à jour de l'explorer treeview
							mainwindow.Sensitive = true; //Activation de la fenetre principale								
							CopyProject("HTDeleteNetwork",Pro.Project_Name + "/" + node.Node_Name + "/" + TypeNetwork);		
							Pro.ProjectIsSave = false;
							mainwindow.UpdateStatusBar();																						
							return true;
						}
					}
				}
			}
			return false;
		}
		
		//Fonction DeleteBoardInNetwork
		//Fonction permettant de suprrimer une carte et tous ses enfants
		//Arguments :
		//	Int32 _Board_Id : id de la board à surpprimer
		//Retour :
		//	bool : Vrai si réussit		
		public bool DeleteBoardInNetwork(Int32 _Board_Id)
		{		
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{			
				foreach (Node node in Pro.ReturnListNode()) //Pour chaque noeud de la liste des noeud
				{
					foreach (Network net in node.ReturnListNetwork())	//Pour chaque réseau présent de la liste chargé						
					{
						foreach(Board board in net.ReturnListBoard()) //Dans la liste des cartes
						{
							if(board.Board_Id == _Board_Id)	//Si le réseau trouvé à pour Id celui demandé
							{		
								string BoardName = board.Board_Name;
								string BoardType = board.Board_Type;
								net.Board_.Remove(board);//On supprime la carte
								mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
								mainwindow.CloseTabInCaseOfDelete();//Permet d'enlever le tab si celui-ci est ouvert									
								UpdateInstanceUsed();
								mainwindow.UpdateEplorerTreeView(); //On fait la mise à jour de l'explorer treeview
								mainwindow.Sensitive = true; //Activation de la fenetre principale									
								CopyProject("HTDeleteBoard",Pro.Project_Name + "/" + node.Node_Name + "/" + net.Network_Type + "/" + BoardType + "/" + BoardName);
								Pro.ProjectIsSave = false;
								mainwindow.UpdateStatusBar();																							
								return true;
							}
						}
					}
				}
			}
			return false;					
		}
		
		//Fonction DeleteInstanceInNode
		//Fonction permettant de supprimer une instance dans un noeud
		public bool DeleteInstanceInNode(Int32 _Instance_Id)
		{
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{			
				foreach (Node node in Pro.ReturnListNode()) //Pour chaque noeud de la liste des noeud
				{
					foreach(Instance ins in node.ReturnListInstance())
					{
						if(ins.Instance_Id == _Instance_Id)
						{
							
							foreach(Scenario sce in node.ReturnListScenario())
							{
								sce.ScenarioData = sce.ScenarioData.Replace(" " + ins.Instance_Name + " "," " + param.ParamT("MoIn_InstanceEmpty") + " ");
								foreach(Function fun in sce.ReturnListFunction())
								{
									fun.FunctionData = fun.FunctionData.Replace(" " + ins.Instance_Name + " "," " + param.ParamT("MoIn_InstanceEmpty") + " ");
								}
							}
															
							
							string InstanceName = ins.Instance_Name;
							string InstanceType = ins.Instance_Type;							
							node.Instance_.Remove(ins);
							mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
							mainwindow.CloseTabInCaseOfDelete();//Permet d'enlever le tab si celui-ci est ouvert								
							UpdateInstanceUsed();	
							mainwindow.UpdateEplorerTreeView(); //On fait la mise à jour de l'explorer treeview
							mainwindow.Sensitive = true; //Activation de la fenetre principale								
							CopyProject("HTDeleteInstance",Pro.Project_Name + "/" + node.Node_Name + "/" + InstanceType + "/" + InstanceName);	
							Pro.ProjectIsSave = false;
							mainwindow.UpdateStatusBar();																						
							return true;							
						}
					}
				}
			}
			return false;
		}
		
		//Fonction DeleteCustomerInNode
		//Fonction permettant de supprimer un customer
		public bool DeleteCustomerInNode(Int32 _CustomerId)
		{
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{			
				foreach (Node node in Pro.ReturnListNode()) //Pour chaque noeud de la liste des noeud
				{
					foreach(Customer cus in node.ReturnListCustomer())
					{
						if(cus.CustomerId == _CustomerId)
						{
							if(node.ReturnListCustomer().Count == 1) //Dans le cas où nous avons un fichier customer
							{
								mainwindow.Sensitive = true; //Activation de la fenetre principale
								mainwindow.AddLineOutput(param.ParamI("OutputError"),"OneCustomer");
								return false;
							}							
							
							string CustomerName = cus.CustomerName;						
							node.Customer_.Remove(cus);
							mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
							mainwindow.CloseTabInCaseOfDelete();//Permet d'enlever le tab si celui-ci est ouvert								
							UpdateInstanceUsed();	
							mainwindow.UpdateEplorerTreeView(); //On fait la mise à jour de l'explorer treeview
							mainwindow.Sensitive = true; //Activation de la fenetre principale								
							CopyProject("HTDeleteCustomer",Pro.Project_Name + "/" + node.Node_Name + "/" + CustomerName);
							Pro.ProjectIsSave = false;
							mainwindow.UpdateStatusBar();																						
							return true;							
						}
					}
				}
			}
			return false;			
		}
		
		//Fonction DeleteScenarioInNode
		//Fonction permettant de supprimer un scenario dans un noeud
		public bool DeleteScenarioInNode(Int32 _ScenarioId)
		{
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{			
				foreach (Node node in Pro.ReturnListNode()) //Pour chaque noeud de la liste des noeud
				{
					foreach(Scenario sce in node.ReturnListScenario())
					{
						if(sce.ScenarioId == _ScenarioId)
						{
							if(node.ReturnListScenario().Count == 1)
							{
								mainwindow.Sensitive = true; //Activation de la fenetre principale
								mainwindow.AddLineOutput(param.ParamI("OutputError"),"OneScenario");
								return false;							
							}
							
							string ScenarioName = sce.ScenarioName;
							node.Scenario_.Remove(sce);
							mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
							mainwindow.CloseTabInCaseOfDelete();//Permet d'enlever le tab si celui-ci est ouvert								
							UpdateInstanceUsed();	
							mainwindow.UpdateEplorerTreeView(); //On fait la mise à jour de l'explorer treeview
							mainwindow.Sensitive = true; //Activation de la fenetre principale								
							CopyProject("HTDeleteCustomer",Pro.Project_Name + "/" + node.Node_Name + "/" + ScenarioName);
							Pro.ProjectIsSave = false;
							mainwindow.UpdateStatusBar();																						
							return true;		
						}
					}
				}
			}
			return false;
		}
		
		//Fonction DeleteVariableInScenario
		//Fonction permettant de supprimer une variable dans un scénario
		public bool DeleteVariableInScenario(Int32 _VariableId)
		{
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{			
				foreach (Node node in Pro.ReturnListNode()) //Pour chaque noeud de la liste des noeud
				{
					foreach(Scenario sce in node.ReturnListScenario())
					{
						foreach(Variable vari in sce.ReturnListVariable())
						{
							if(vari.VariableId == _VariableId)
							{
								string NameVariable = vari.VariableName;
								sce.Variable_.Remove(vari);
								mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
								mainwindow.CloseTabInCaseOfDelete();//Permet d'enlever le tab si celui-ci est ouvert								
								UpdateInstanceUsed();
								mainwindow.UpdateEplorerTreeView(); //On fait la mise à jour de l'explorer treeview
								mainwindow.Sensitive = true; //Activation de la fenetre principale								
								CopyProject("HTDeleteVariable",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + NameVariable);		
								Pro.ProjectIsSave = false;
								mainwindow.UpdateStatusBar();																							
								return true;
							}
						}
					}
				}
			}
			return false;			
		}
		
		//Fonction DeleteFunctionInScenario
		//Fonction permettant de supprimer une fonction
		public bool DeleteFunctionInScenario(Int32 _FunctionId)
		{
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{			
				foreach (Node node in Pro.ReturnListNode()) //Pour chaque noeud de la liste des noeud
				{
					foreach(Scenario sce in node.ReturnListScenario())
					{
						foreach(Function fun in sce.ReturnListFunction())
						{
							if(fun.FunctionId == _FunctionId && !fun.InitFunction)
							{
								string NameFunction = fun.FunctionName;
								sce.Function_.Remove(fun);
								mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
								mainwindow.CloseTabInCaseOfDelete();//Permet d'enlever le tab si celui-ci est ouvert								
								UpdateInstanceUsed();
								mainwindow.UpdateEplorerTreeView(); //On fait la mise à jour de l'explorer treeview
								mainwindow.Sensitive = true; //Activation de la fenetre principale								
								CopyProject("HTDeleteFunction",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + NameFunction);	
								Pro.ProjectIsSave = false;
								mainwindow.UpdateStatusBar();																							
								return true;
							}
						}
					}
				}
			}
			return false;				
		}
		
		
//################Fonction permettant la modification des instances ############################
		
		//Fonction ModifyProject
		//Fonction permettant de modifier le nom et l'auteur du projet
		//Attributs :
		//	string _Name : Nom du projet
		//	string _Author : Auteur du projet
		//  Int16 _Choice : 0 : Modification Nom et auteur, 1 : Modification Nom, 2 : Modification Auteur
		//	Int32 _Project_Id : Id du projet où nous devons faire des modifications
		//Retour :
		//	bool : Vrai si réussit	
		public bool ModifyProject(string _Text, int _Choice, Int32 _Project_Id)
		{
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{
				if(Pro.Project_Id == _Project_Id) //Si un id de projet est égale au Project ID que nous avons passé en paramètre
				{
					if(_Choice == param.ParamI("MoPo_ChoiceName")) //Dans le cas où nous mettons à jour le nom du projet, nous verifions qu'il n'éxiste pas un projet du même nom avec le même chemin de sauvegarde
					{
						if(Pro.Project_Name != _Text) //Dans le cas d'un double clic involotaire, on filtre
						{
							foreach(Project Pro_ in ListProject) //dans cette boucle nous allons verifier que nous avons pas de doublons entre le nom et le chemin des projets
							{
								if(Pro_.Project_Name == _Text.Replace(" ","_") && Pro_.Project_SavePath == Pro.Project_SavePath) //Dans le cas où le nom et le chemin du projet sont identique 
								{
									mainwindow.AddLineOutput(param.ParamI("OutputError"),"IdemProject");//Nous affichons un petit message d'erreur
									return false;
								}
								else if(File.Exists(Pro.Project_SavePath +  "/" + _Text.Replace(" ","_") + param.ParamP("ExtensionFile")))//Nous vérifions que le fichier existe pas
								{
									mainwindow.AddLineOutput(param.ParamI("OutputError"),"ProjectExistInPath");//Nous affichons un petit message d'erreur
									return false;
								}
							}
							string OldProjectName = Pro.Project_Name;
							Pro.Project_Name = _Text.Replace(" ","_"); //On fait la mise à jour du nom du projet	
							mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
							mainwindow.UpdateMainNoteBook();
							CopyProject("HTModifyProjectName",OldProjectName + " => " + Pro.Project_Name);
							Pro.ProjectIsSave = false;
							mainwindow.UpdateStatusBar();	
							return true;
						}						
					}
					else if(_Choice == param.ParamI("MoPo_ChoiceAuthor")) //Dans le cas où nous voulons modifier l'auteur
					{
						string OldProjectAuthor = Pro.Project_Author;
						if(OldProjectAuthor != _Text)
						{
							Pro.Project_Author = _Text;
							CopyProject("HTModifyProjectAuthor",Pro.Project_Name + " (" + OldProjectAuthor + " => " + Pro.Project_Author + ")");
							mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
							Pro.ProjectIsSave = false;
							mainwindow.UpdateStatusBar();																						
							return true;
						}
					}
					else if(_Choice == param.ParamI("MoPo_ChoicePath")) //Dans le cas où nous mettons à jour le chemin du projet
					{
						if(Pro.Project_SavePath != _Text) //Dans le cas d'un double clic involotaire, on filtre
						{
							foreach(Project Pro_ in ListProject) //dans cette boucle nous allons verifier que nous avons pas de doublons entre le nom et le chemin des projets
							{
								if(Pro_.Project_Name == Pro.Project_Name && Pro_.Project_SavePath == _Text) //Dans le cas où le nom et le chemin du projet sont identique 
								{
									mainwindow.AddLineOutput(param.ParamI("OutputError"),"IdemProject");//Nous affichons un petit message d'erreur
									return false;
								}
								else if(File.Exists(_Text +  "/" + Pro.Project_Name + param.ParamP("ExtensionFile"))) //Nous vérifions que le fichier existe pas
								{
									mainwindow.AddLineOutput(param.ParamI("OutputError"),"ProjectExistInPath"); //Nous affichons un petit message d'erreur
									return false;
								}								
							}
							string OldProjectPath = Pro.Project_SavePath;
							Pro.Project_SavePath = _Text; //Nous sauvegardons la nouvelle valeur du chemin
							CopyProject("HTModifyProjectPath",Pro.Project_Name + " (" + OldProjectPath + " => " + Pro.Project_SavePath + ")");	
							Pro.ProjectIsSave = false;
							mainwindow.UpdateStatusBar();																						
							return true;
						}						
					}
					else if(_Choice == param.ParamI("MoPo_ChoiceNote")) //Dans le cas de la mise à jour des commentaires
					{
						if(Pro.Project_Note != _Text)
						{
							Pro.Project_Note = _Text; //On fait la mise à jour des commentaires
							CopyProject("HTModifyProjectNote",Pro.Project_Name);
							mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
							Pro.ProjectIsSave = false;
							mainwindow.UpdateStatusBar();																						
							return true;
						}
					}
					else if(_Choice == param.ParamI("MoPo_ChoicePassword")) //Dans le cas de la mise à jour des commentaires
					{
						if(Pro.Project_Password != _Text)
						{
							Pro.Project_Password = _Text; //On fait la mise à jour du mot de passe
							CopyProject("HTModifyProjectPassword",Pro.Project_Name);
							mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab		
							Pro.ProjectIsSave = false;
							mainwindow.UpdateStatusBar();																						
							return true;
						}
					}					
				}
			}
			return false;
		}
				
		//Fonction ModifyNode
		//Fonction permettan de modifier les paramètre d'un noeud
		public bool ModifyNode(string _Text, int _Choice, Int32 _Node_Id)
		{
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{
				foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
				{
					if(node.Node_Id == _Node_Id) //Si l'Id d'un noeud est égale au noeud demandé
					{
						if(_Choice == param.ParamI("MoNo_ChoiceName")) //Dans le cas où nous voulons modifier le nom d'un noeud
						{
							_Text = ReturnCorrectName(_Text);	
							foreach(Project _Pro in ListProject) //Nous allons vérifier que le nom exite pas déjà dans la liste des projet
							{		
								foreach(Node _node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
								{	
									if(_node.Node_Name == _Text.Replace(" ","_")) //On regarde si un noeud portant le même nom existe
									{
										mainwindow.AddLineOutput(param.ParamI("OutputError"),"NodeExist"); //dans ce cas nous affichons un message d'erreur
										return false;
									}
								}
									
							}
							string OldNodeName = node.Node_Name;
							node.Node_Name = _Text.Replace(" ","_"); //On met à jour le nom	
							mainwindow.UpdateMainNoteBook();
							mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
							CopyProject("HTModifyNodeName",Pro.Project_Name + " (" + OldNodeName + " => " + node.Node_Name + ")");	
							Pro.ProjectIsSave = false;
							mainwindow.UpdateStatusBar();	
							return true;
						}
						else if(_Choice == param.ParamI("MoNo_ChoiceIP")) //Dans le cas où nous voulons modifier l'ip d'un noeud
						{
							string OldNodeIP = node.Node_IP;
							if(OldNodeIP != _Text)
							{
								node.Node_IP  = _Text;	//On met à jour l'IP
								mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
								CopyProject("HTModifyNodeIP",Pro.Project_Name + "/" + node.Node_Name + " (" + OldNodeIP + " => " + node.Node_IP + ")");
								mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab	
								Pro.ProjectIsSave = false;
								mainwindow.UpdateStatusBar();																							
								return true;
							}
						}
						else if(_Choice == param.ParamI("MoNo_ChoiceGTWIP")) //Dans le cas où nous voulons modifier la gtw ip d'un noeud
						{
							string OldNodeGTWIP = node.Node_GTWIP;
							if(OldNodeGTWIP != _Text)
							{							
								node.Node_GTWIP  = _Text;	//On met à jour l'IP
								mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
								CopyProject("HTModifyNodeGTWIP",Pro.Project_Name + "/" + node.Node_Name + " (" + OldNodeGTWIP + " => " + node.Node_GTWIP + ")");
								mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab;	
								Pro.ProjectIsSave = false;
								mainwindow.UpdateStatusBar();																							
								return true;
							}
						}						
						else if(_Choice == param.ParamI("MoNo_ChoiceDHCP"))//Dans le cas où nous voulons modifier le dhcp d'un noeud
						{
							if(_Text == "True")//Si le texte est True
							{
								string OldNodeDHCP = node.Node_DHCP.ToString();
								if(OldNodeDHCP != _Text)
								{	
									node.Node_DHCP  = true;//On met le booleen à vrai
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyNodeDHCP",Pro.Project_Name + "/" + node.Node_Name + " (" + OldNodeDHCP + " => " + node.Node_DHCP.ToString() + ")");
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab	
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																								
									return true;
								}
							}
							else
							{
								string OldNodeDHCP = node.Node_DHCP.ToString();
								if(OldNodeDHCP != _Text)
								{									
									node.Node_DHCP  = false;//Sinon nous le mettons à faux
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyNodeDHCP",Pro.Project_Name + "/" + node.Node_Name + " (" + OldNodeDHCP + " => " + node.Node_DHCP.ToString() + ")");
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab	
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																								
									return true;
								}
							}
						}
						else if(_Choice == param.ParamI("MoNo_ChoiceWebServer"))//Dans le cas où nous voulons modifier le dhcp d'un noeud
						{
							if(_Text == "True")//Si le texte est True
							{
								string OldNodeWebServer = node.Node_WebServer.ToString();
								if(OldNodeWebServer != _Text)
								{	
									node.Node_WebServer  = true;//On met le booleen à vrai
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyNodeWebServer",Pro.Project_Name + "/" + node.Node_Name + " (" + OldNodeWebServer + " => " + node.Node_WebServer.ToString() + ")");
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab	
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																								
									return true;
								}
							}
							else
							{
								string OldNodeWebServer = node.Node_WebServer.ToString();
								if(OldNodeWebServer != _Text)
								{									
									node.Node_WebServer  = false;//Sinon nous le mettons à faux
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyNodeWebServer",Pro.Project_Name + "/" + node.Node_Name + " (" + OldNodeWebServer + " => " + node.Node_WebServer.ToString() + ")");
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab	
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																								
									return true;
								}
							}
						}						
						else if(_Choice == param.ParamI("MoNo_ChoiceMac"))//Dans le cas où nous voulons modifier le mac d'un noeud
						{
							string OldNodeMac = node.Node_Mac;
							if(OldNodeMac != _Text)
							{								
								node.Node_Mac = node.ReturnAdressMac();//On appel la fonction générant un mac automatiquement
								mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
								CopyProject("HTModifyNodeMac",Pro.Project_Name + "/" + node.Node_Name + " (" + OldNodeMac + " => " + node.Node_Mac + ")");
								mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab	
								Pro.ProjectIsSave = false;
								mainwindow.UpdateStatusBar();																																
								return true;
							}
						}
						else if(_Choice == param.ParamI("MoNo_ChoiceNote"))//Dans la cas où nous voulons modifier les notes d'un noeud
						{
							if(node.Node_Note != _Text)
							{							
								node.Node_Note = _Text;//on met à jour les notes
								mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
								CopyProject("HTModifyNodeNote",Pro.Project_Name + "/" + node.Node_Name);
								mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab	
								Pro.ProjectIsSave = false;
								mainwindow.UpdateStatusBar();																																
								return true;
							}
						}
						else if(_Choice == param.ParamI("MoNo_ChoiceType"))//Dans la cas où nous voulons modifier le type d'un noeud
						{
							string OldNodeType = node.Node_Type;
							if(OldNodeType != _Text)
							{							
								node.Node_Type = _Text;//on met à jour le type
								mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
								CopyProject("HTModifyNodeType",Pro.Project_Name + "/" + node.Node_Name + " (" + OldNodeType + " => " + node.Node_Type + ")");
								mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab	
								Pro.ProjectIsSave = false;
								mainwindow.UpdateStatusBar();																																								
								return true;
							}
						}
						else if(_Choice == param.ParamI("MoNo_ChoiceClock"))//Dans la cas où nous voulons modifier le type d'horloge
						{
							string OldNodeClock = node.Node_Clock;
							string NewNodeClock = _Text;
							if(node.Node_Clock == "0")
							{
								OldNodeClock = param.ParamT("NP_NoValue");
							}
							if(_Text == param.ParamT("NP_NoValue"))
							{
								_Text = "0";
							}
				
							if(node.Node_Clock != _Text)
							{								
								node.Node_Clock = _Text;//on met à jour le type d'horloge
								CopyProject("HTModifyNodeClock",Pro.Project_Name + "/" + node.Node_Name + " (" + OldNodeClock + " => " + NewNodeClock + ")");
								mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab	
								Pro.ProjectIsSave = false;
								mainwindow.UpdateStatusBar();																																								
								return true;
							}
						}
						else if(_Choice == param.ParamI("MoNo_Choice1Wire"))//Dans la cas où nous voulons modifier la présence d'une puce 1-Wire
						{
							if(_Text == "True")//Si le texte est True
							{		
								string OldNode1Wire = node.Node_1Wire.ToString();
								if(OldNode1Wire != _Text)
								{									
									node.Node_1Wire = true;//on met à jour la puce 1-wire à vrai
									CopyProject("HTModifyNode1Wire",Pro.Project_Name + "/" + node.Node_Name + " (" + OldNode1Wire + " => " + node.Node_1Wire.ToString() + ")");
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab	
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																									
									return true;
								}
							}
							else
							{
								string OldNode1Wire = node.Node_1Wire.ToString();
								if(OldNode1Wire != _Text)
								{								
									node.Node_1Wire = false;//on met à jour la puce 1-wire à faux
									CopyProject("HTModifyNode1Wire",Pro.Project_Name + "/" + node.Node_Name + " (" + OldNode1Wire + " => " + node.Node_1Wire.ToString() + ")");
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																									
									return true;
								}
							}
						}							
					}
				}
				
			}
			return false;
			}
		
		//Fonction ModifyNodeDebug
		//Fonction permettant de modifier un  debug dans un node
		public bool ModifyNodeDebug(bool _DebugValue, string _DebugName, Int32 _Node_Id)
		{
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{
				foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
				{
					if(node.Node_Id == _Node_Id) //Si l'Id d'un noeud est égale au noeud demandé
					{
						foreach(Debug deb in node.Debug_) //Dans la liste des debug
						{
							if(deb.Name == _DebugName)//Si le nom du debug est celui que nous avons mis en paramètre
							{
								string OldNodeDebug = deb.Value.ToString();
								if(deb.Value != _DebugValue)
								{
									deb.Value = _DebugValue;//On fait la mise à jour de la valeur
									CopyProject("HTModifyNodeDebug",Pro.Project_Name + "/" + node.Node_Name + "/" + deb.Name + " (" + OldNodeDebug + " => " + deb.Value.ToString() + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																									
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}
		
		//Fonction ModifyNetwork
		//Fonction permettant de modifier un réseau
		public bool ModifyNetwork(string _Text, int _Choice, Int32 _Network_Id)
		{
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{
				foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
				{
					foreach(Network net in node.ReturnListNetwork()) //Dans la liste des réseau de chaque noeud
					{
						if(net.Network_Id == _Network_Id)//Si l'id du réseau est celui que nous avons passé en paramètre
						{
							if(_Choice == param.ParamI("MoNet_ChoiceNote"))//Dans la cas où nous voulons modifier les note
							{
								if(net.Network_Note != _Text)
								{
									net.Network_Note = _Text;//On met à jours les notes
									CopyProject("HTModifyNetworkNote",Pro.Project_Name + "/" + node.Node_Name + "/" + net.Network_Type);
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																									
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}
		
		//Fonction ModifyBoard
		//Fonction permettant de faire la mise à jour des cartes
		public bool ModifyBoard(string _Text, int _Choice, Int32 _Board_Id)
		{
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{
				foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
				{
					foreach(Network net in node.ReturnListNetwork()) //Dans la liste des réseau de chaque noeud
					{
						foreach(Board boa in net.ReturnListBoard()) //Dans la liste des carte de chaque réseau
						{
							if(boa.Board_Id == _Board_Id) //Si l'id de la carte est celle que nous avons passé en paramètre
							{
								if(_Choice == param.ParamI("MoBo_ChoiceName")) //Dans le cas où nous voulons modifier le nom de la carte
								{
									_Text = ReturnCorrectName(_Text);	
									foreach(Project _Pro in ListProject) //Nous allons vérifier que le nom exite pas déjà dans la liste des projet
									{		
										foreach(Node _node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
										{	
											foreach(Network _net in _node.ReturnListNetwork()) //Pour chaque réseau de la liste des réseau propre au noeud
											{
												foreach(Board _boa in _net.ReturnListBoard()) //Pour chaque carte de la liste des cartes propre au réseau
												{
													if(_boa.Board_Name == _Text.Replace(" ","_"))//Si le nom de la carte est égale au texte que nous avons mis en paramètre
													{
														mainwindow.AddLineOutput(param.ParamI("OutputError"),"BoardExist"); //dans ce cas nous affichons un message d'erreur
														return false;	
													}
												}
											}
										}
									}
									string OldBoardName = boa.Board_Name;
									boa.Board_Name = _Text.Replace(" ","_"); //on modifie le nom de la carte
									mainwindow.UpdateMainNoteBook();
									mainwindow.UpdateEplorerTreeView(); //On met à jour ExplorerTreeView
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyBoardName",Pro.Project_Name + "/" + node.Node_Name + "/" + net.Network_Type + " (" + OldBoardName + " => " + boa.Board_Name + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																
									return true;
								}
								else if(_Choice == param.ParamI("MoBo_ChoiceI2C0")) //Dans la cas où nous voulons modifier la premiere adresser I2C
								{
									foreach(Boards boas in ListBoards)
									{
										if(boas.Type == boa.Board_Type)
										{									
											if(Convert.ToInt32(_Text) > boas.MaxI2CAdress-1)
											{
												mainwindow.AddLineOutput(param.ParamI("OutputError"),"I2CAdressTooMuch"); //on affiche un message d'erreur
												return false;
											}
											else
											{
												string OldBoardI2C0 = boa.Board_I2C_0.ToString();
												if(boa.Board_I2C_0 != Convert.ToInt32(_Text))
												{
													boa.Board_I2C_0 = Convert.ToInt32(_Text);//On fait la mise à jour de l'adresse
													mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
													mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
													CopyProject("HTModifyBoardI2C0",Pro.Project_Name + "/" + node.Node_Name + "/" + net.Network_Type + "/" + boa.Board_Name + " (" + OldBoardI2C0 + " => " + boa.Board_I2C_0.ToString() + ")");
													return true;
												}
											}
										}
									}
								}
								else if(_Choice == param.ParamI("MoBo_ChoiceI2C1"))//Dans la cas où nous voulons modifier la seconde adresser I2C
								{
									foreach(Boards boas in ListBoards)
									{
										if(boas.Type == boa.Board_Type)
										{
											if(boas.NumberI2CAdress == 2)
											{	
												if(Convert.ToInt32(_Text) > boas.MaxI2CAdress-1)
												{
													mainwindow.AddLineOutput(param.ParamI("OutputError"),"I2CAdressTooMuch"); //on affiche un message d'erreur
													return false;
												}
												else
												{
													string OldBoardI2C1 = boa.Board_I2C_1.ToString();
													if(boa.Board_I2C_1 != Convert.ToInt32(_Text))
													{
														boa.Board_I2C_1 = Convert.ToInt32(_Text);//On fait la mise à jour de l'adresse
														mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
														mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
														CopyProject("HTModifyBoardI2C1",Pro.Project_Name + "/" + node.Node_Name + "/" + net.Network_Type + "/" + boa.Board_Name + " (" + OldBoardI2C1 + " => " + boa.Board_I2C_1.ToString() + ")");
														Pro.ProjectIsSave = false;
														mainwindow.UpdateStatusBar();	
														return true;
													}
												}
											}
											else
											{
												mainwindow.AddLineOutput(param.ParamI("OutputError"),"I2CAdressNotPresent"); //on affiche un message d'erreur
												return false;
											}
										}
									}

								}	
								else if(_Choice == param.ParamI("MoBo_Choice1WMac"))//Dans la cas où nous voulons modifier l'adresse mac dans la cas d'une carte 1-wire
								{
									string OldBoard1WireMac = boa.Board_1Wire_Mac;
									boa.Board_1Wire_Mac = _Text;//On fait la mise à jour de l'adresse
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyBoard1Wire",Pro.Project_Name + "/" + node.Node_Name + "/" + net.Network_Type + "/" + boa.Board_Name + " (" + OldBoard1WireMac + " => " + boa.Board_1Wire_Mac + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();
									return true;
								}
								else if(_Choice == param.ParamI("MoBo_Choice1WPrecision"))//Dans la cas où nous voulons modifier l'adresse mac dans la cas d'une carte 1-wire
								{
									string OldBoard1WirePrecision = boa.Board_1Wire_Precision;
									boa.Board_1Wire_Precision = _Text;//On fait la mise à jour de l'adresse
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyBoard1WireMac",Pro.Project_Name + "/" + node.Node_Name + "/" + net.Network_Type + "/" + boa.Board_Name + " (" + OldBoard1WirePrecision + " => " + boa.Board_1Wire_Precision + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();									
									return true;
								}								
								else if(_Choice == param.ParamI("MoBo_ChoiceNote"))//Dans la cas où nous voulons modifier les notes de la cartes
								{
									if(boa.Board_Note != _Text)
									{
										boa.Board_Note = _Text;//On fait la mise à jour des notes
										CopyProject("HTModifyBoardNote",Pro.Project_Name + "/" + node.Node_Name + "/" + net.Network_Type + "/" + boa.Board_Name);
										Pro.ProjectIsSave = false;
										mainwindow.UpdateStatusBar();										
										return true;
									}
								}
							}
						}
					}
				}
			}
			return false;
		}
		
		//Fonction ModifyInstance
		//Fcontion permettant de modifier une instance
		public bool ModifyInstance(string _Text, int _Choice, Int32 _Instance_Id)
		{
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{
				foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
				{
					foreach(Instance ins in node.ReturnListInstance())
					{
						if(ins.Instance_Id == _Instance_Id)
						{
							if(_Choice == param.ParamI("MoIn_ChoiceName")) //Dans le cas où nous voulons modifier le nom de la carte
							{		
								_Text = ReturnCorrectName(_Text);								
								foreach(Project _Pro in ListProject) //Dans la liste des projets
								{
									foreach(Node _node in _Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
									{
										if(_node.Node_Id == node.Node_Id)
										{
											foreach(Instance _ins in _node.ReturnListInstance())
											{		
												if(_ins.Instance_Name == _Text.Replace(" ","_"))
												{
													mainwindow.AddLineOutput(param.ParamI("OutputError"),"InstanceExist");
													return false;
												}
											}
										}
									}
								}
								
								foreach(Scenario sce in node.ReturnListScenario())
								{
									foreach(Variable vari in sce.ReturnListVariable())
									{
										if(vari.VariableName == _Text.Replace(" ","_"))
										{
											mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameFunctionExistInVariable");
											return false;
										}
									}
									foreach(Function fun in sce.ReturnListFunction())
									{
										if(fun.FunctionName ==  _Text.Replace(" ","_"))
										{
											mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameExistInFunctione");
											return false;
										}
										if(fun.FunctionNameArg1 == _Text.Replace(" ","_") || fun.FunctionNameArg2 == _Text.Replace(" ","_") || fun.FunctionNameArg3 == _Text.Replace(" ","_") || fun.FunctionNameArg4 == _Text.Replace(" ","_") || fun.FunctionNameArg5 == _Text.Replace(" ","_") || fun.FunctionNameArg6 == _Text.Replace(" ","_"))
										{
											mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameArgsExistInArgs");
											return false;
										}
									}
								}
								
								foreach(Scenario sce in node.ReturnListScenario())
								{
									sce.ScenarioData = sce.ScenarioData.Replace(" " + ins.Instance_Name + " "," " + _Text.Replace(" ","_") + " ");
									foreach(Function fun in sce.ReturnListFunction())
									{
										fun.FunctionData = fun.FunctionData.Replace(" " + ins.Instance_Name + " "," " + _Text.Replace(" ","_") + " ");
									}
								}
								
								
								string OldInstanceName = ins.Instance_Name;
								ins.Instance_Name = _Text.Replace(" ","_");
								mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
								CopyProject("HTModifyInstanceName",Pro.Project_Name + "/" + node.Node_Name + "/" + ins.Instance_Type + " (" + OldInstanceName + " => " + ins.Instance_Name + ")");
								Pro.ProjectIsSave = false;
								mainwindow.UpdateStatusBar();								
								return true;
							}
							else if(_Choice == param.ParamI("MoIn_ChoiceNote")) //Dans le cas où nous voulons modifier le nom de la carte
							{		
								if(ins.Instance_Note != _Text)
								{
									ins.Instance_Note = _Text;
									CopyProject("HTModifyInstanceNote",Pro.Project_Name + "/" + node.Node_Name + "/" + ins.Instance_Type + "/" + ins.Instance_Name);
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();									
									return true;
								}
							}		
							else if(_Choice == param.ParamI("MoIn_ChoiceLIGDefaultValue")) //Dans le cas où nous voulons modifier le nom de la carte
							{		
								if(ins.Instance_LIG_DefaultValue != Convert.ToInt32(_Text))
								{
									string OldInstanceLIGDefaultValue = ins.Instance_LIG_DefaultValue.ToString();
									ins.Instance_LIG_DefaultValue = Convert.ToInt32(_Text);
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyInstanceLIGDefaultValue",Pro.Project_Name + "/" + node.Node_Name + "/" + ins.Instance_Type + "/" + ins.Instance_Name + " (" + OldInstanceLIGDefaultValue + " => " + ins.Instance_LIG_DefaultValue.ToString() + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();									
									return true;
								}
							}	
							else if(_Choice == param.ParamI("MoIn_ChoiceLIGFade"))
							{
								if(ins.Instance_LIG_Fade != Convert.ToInt32(_Text))
								{
									string OldInstanceLIGFade = ins.Instance_LIG_Fade.ToString();
									ins.Instance_LIG_Fade = Convert.ToInt32(_Text);
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyInstanceLIGFade",Pro.Project_Name + "/" + node.Node_Name + "/" + ins.Instance_Type + "/" + ins.Instance_Name + " (" + OldInstanceLIGFade + " => " + ins.Instance_LIG_Fade.ToString() + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();						
									return true;			
								}
							}		
							else if(_Choice == param.ParamI("MoIn_ChoiceSWIInverse"))
							{
								string OldInstanceSWIInverse = ins.Instance_SWI_Inverse.ToString();
								if(_Text == "True")
								{
									if(ins.Instance_SWI_Inverse == false)
									{
										ins.Instance_SWI_Inverse = true;
										mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
										CopyProject("HTModifyInstanceSWIInvese",Pro.Project_Name + "/" + node.Node_Name + "/" + ins.Instance_Type + "/" + ins.Instance_Name + " (" + OldInstanceSWIInverse + " => " + ins.Instance_SWI_Inverse.ToString() + ")");
										Pro.ProjectIsSave = false;
										mainwindow.UpdateStatusBar();										
										return true;
									}
								}
								else
								{
									if(ins.Instance_SWI_Inverse == true)
									{									
										ins.Instance_SWI_Inverse = false;
										mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
										CopyProject("HTModifyInstanceSWIInvese",Pro.Project_Name + "/" + node.Node_Name + "/" + ins.Instance_Type + "/" + ins.Instance_Name + " (" + OldInstanceSWIInverse + " => " + ins.Instance_SWI_Inverse.ToString() + ")");
										Pro.ProjectIsSave = false;
										mainwindow.UpdateStatusBar();										
										return true;
									}
								}
															
							}	
							else if(_Choice == param.ParamI("MoIn_ChoiceSWIImpulsionTime"))
							{
								string OldInstanceSWIImpulsionTime = ins.Instance_SWI_ImpulsionTime.ToString();
								if(ins.Instance_SWI_ImpulsionTime != Convert.ToInt32(_Text))
								{
									ins.Instance_SWI_ImpulsionTime = Convert.ToInt32(_Text);
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyInstanceSWIImpulsionTime",Pro.Project_Name + "/" + node.Node_Name + "/" + ins.Instance_Type + "/" + ins.Instance_Name + " (" + OldInstanceSWIImpulsionTime + " => " + ins.Instance_SWI_ImpulsionTime.ToString() + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();									
									return true;
								}
							}
							else if(_Choice == param.ParamI("MoIn_ChoiceSHUAppearenceOrder"))
							{
								if(ins.Instance_Up_Down_Stop != Convert.ToInt32(_Text))
								{
									ins.Instance_Up_Down_Stop = Convert.ToInt32(_Text);
									UpdateInstanceUsed();
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyInstanceSHUAppearenceOrder",Pro.Project_Name + "/" + node.Node_Name + "/" + ins.Instance_Type + "/" + ins.Instance_Name);
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();									
									return true;
								}
							}
							else if(_Choice == param.ParamI("MoIn_ChoiceSHUNumberOfOutput"))
							{
								if(ins.Instance_SHU_NumberOfOutput != Convert.ToInt32(_Text))
								{
									string OldInstanceSHUNumberOfOuput = ins.Instance_SHU_NumberOfOutput.ToString();
									ins.Instance_SHU_NumberOfOutput = Convert.ToInt32(_Text);
									UpdateInstanceUsed();
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyInstanceSHUNumberOfOutput",Pro.Project_Name + "/" + node.Node_Name + "/" + ins.Instance_Type + "/" + ins.Instance_Name + " (" + OldInstanceSHUNumberOfOuput + " => " + ins.Instance_SHU_NumberOfOutput.ToString() + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();									
									return true;
								}
							}	
							else if(_Choice == param.ParamI("MoIn_ChoiceSHUTravelTime"))
							{
								if(ins.Instance_SHU_Time != Convert.ToInt32(_Text))
								{
									string OldInstanceSHUTravelTime = ins.Instance_SHU_Time.ToString();
									ins.Instance_SHU_Time = Convert.ToInt32(_Text);
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyInstanceSHUTravelTime",Pro.Project_Name + "/" + node.Node_Name + "/" + ins.Instance_Type + "/" + ins.Instance_Name + " (" + OldInstanceSHUTravelTime + " => " + ins.Instance_SHU_Time.ToString() + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();									
									return true;
								}
							}
							else if(_Choice == param.ParamI("MoIn_ChoiceSHUInitTime"))
							{
								if(ins.Instance_SHU_InitTime != Convert.ToInt32(_Text))
								{
									string OldInstanceSHUInitTime = ins.Instance_SHU_InitTime.ToString();
									ins.Instance_SHU_InitTime = Convert.ToInt32(_Text);
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyInstanceSHUInitTime",Pro.Project_Name + "/" + node.Node_Name + "/" + ins.Instance_Type + "/" + ins.Instance_Name + " (" + OldInstanceSHUInitTime + " => " + ins.Instance_SHU_InitTime.ToString() + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();									
									return true;
								}
							}							
							else if(_Choice == param.ParamI("MoIn_ChoiceSHUShutterType"))
							{
								if(ins.Instance_SHU_Type != Convert.ToInt32(_Text))
								{
									ins.Instance_SHU_Type = Convert.ToInt32(_Text);
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyInstanceSHUShutterType",Pro.Project_Name + "/" + node.Node_Name + "/" + ins.Instance_Type + "/" + ins.Instance_Name);
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();									
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}
		
		//Fonction ModifyCustomer
		//Fonction permettant de modifier les fichier customer
		public bool ModifyCustomer(string _Text, int _Choice, Int32 _Customer_Id)
		{
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{
				foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
				{
					foreach(Customer cus in node.ReturnListCustomer())
					{
						if(cus.CustomerId == _Customer_Id)
						{
							if(_Choice == param.ParamI("MoCu_ChoiceName"))
							{	
								string OldCustomerName = cus.CustomerName;
								cus.CustomerName = _Text;
								mainwindow.UpdateMainNoteBook();
								mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
								CopyProject("HTModifyCustomerName",Pro.Project_Name + "/" + node.Node_Name + " (" + OldCustomerName + " => " + cus.CustomerName + ")");
								Pro.ProjectIsSave = false;
								mainwindow.UpdateStatusBar();								
								return true;
							}
							else if(_Choice == param.ParamI("MoCu_ChoiceData"))
							{
								if(cus.CustomerData != _Text && _Text != "")
								{
									cus.CustomerData = _Text;
									CopyProject("HTModifyCustomerData",Pro.Project_Name + "/" + node.Node_Name + "/"  + cus.CustomerName);
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																	
									return true;
								}
								else
								{
									return false;
								}
							}	
							else if(_Choice == param.ParamI("MoCu_ChoiceNote"))
							{
								string OldCustomerNote = cus.CustomerNote;
								string NewCustomerNote = _Text;
								if(OldCustomerNote == "")
								{
									OldCustomerNote = param.ParamT("BI2CP_EmptyLabel");
								}
								cus.CustomerNote = NewCustomerNote;
								if(NewCustomerNote == "")
								{
									NewCustomerNote = param.ParamT("BI2CP_EmptyLabel");
								}
								CopyProject("HTModifyCustomerNote",Pro.Project_Name + "/" + node.Node_Name + "/" + cus.CustomerName + " (" + OldCustomerNote + " => " + NewCustomerNote + ")");
								Pro.ProjectIsSave = false;
								mainwindow.UpdateStatusBar();																									
								return true;
							}
							else if(_Choice == param.ParamI("MoCu_ChoiceUse"))
							{
								if(_Text == "True")//Si le texte est True
								{		
									string OldCustomerUse = cus.CustomerUse.ToString();
									if(OldCustomerUse != _Text)
									{									
										cus.CustomerUse = true;//on met à jour la puce 1-wire à vrai
										CopyProject("HTModifyCustomerUse",Pro.Project_Name + "/" + node.Node_Name + "/" + cus.CustomerName + " (" + OldCustomerUse + " => " + cus.CustomerUse.ToString() + ")");
										mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
										mainwindow.UpdateEplorerTreeView();
										Pro.ProjectIsSave = false;
										mainwindow.UpdateStatusBar();																											
										return true;
									}
								}
								else
								{
									string OldCustomerUse = cus.CustomerUse.ToString();
									if(OldCustomerUse != _Text)
									{								
										cus.CustomerUse = false;//on met à jour la puce 1-wire à faux
										CopyProject("HTModifyCustomerUse",Pro.Project_Name + "/" + node.Node_Name + "/" + cus.CustomerName + " (" + OldCustomerUse + " => " + cus.CustomerUse.ToString() + ")");
										mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
										mainwindow.UpdateEplorerTreeView();
										Pro.ProjectIsSave = false;
										mainwindow.UpdateStatusBar();																											
										return true;
									}
								}
							}
							else if(_Choice == param.ParamI("MoCu_ChoiceScenario"))
							{
								string OldCustomerScenario = "";
								foreach(Scenario sce in node.ReturnListScenario())
								{
									if(sce.ScenarioId == cus.ScenarioId)
									{
										OldCustomerScenario = sce.ScenarioName;
									}
									if(cus.ScenarioId == 0)
									{
										OldCustomerScenario = param.ParamT("BI2CP_EmptyLabel");
									}									
									if(_Text == sce.ScenarioName)
									{
										cus.ScenarioId = sce.ScenarioId;
										CopyProject("HTModifyCustomerUse",Pro.Project_Name + "/" + node.Node_Name + "/" + cus.CustomerName + " (" + OldCustomerScenario + " => " + _Text + ")");
										mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
										mainwindow.UpdateEplorerTreeView();
										Pro.ProjectIsSave = false;
										mainwindow.UpdateStatusBar();																											
										return true;										
									}
									if(_Text == param.ParamT("BI2CP_EmptyLabel"))
									{
										cus.ScenarioId = 0;
										CopyProject("HTModifyCustomerUse",Pro.Project_Name + "/" + node.Node_Name + "/" + cus.CustomerName + " (" + OldCustomerScenario + " => " + _Text + ")");
										mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
										mainwindow.UpdateEplorerTreeView();
										Pro.ProjectIsSave = false;
										mainwindow.UpdateStatusBar();																											
										return true;											
									}
									
								}
							}
						}
					}
				}
			}
			return false;
		}
		
		//Fonction ModifyScenario
		//Fonction permettant de modifier un scénario
		public bool ModifyScenario(string _Text, int _Choice, Int32 _Scenario_Id)
		{
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{
				foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
				{
					foreach(Scenario sce in node.ReturnListScenario())
					{
						if(sce.ScenarioId == _Scenario_Id)
						{
							if(_Choice == param.ParamI("MoSc_ChoiceName"))
							{	
								_Text = ReturnCorrectName(_Text);	
								string OldScenarioName = sce.ScenarioName;
								sce.ScenarioName = _Text;
								mainwindow.UpdateMainNoteBook();
								mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
								CopyProject("HTModifyScenarioName",Pro.Project_Name + "/" + node.Node_Name + " (" + OldScenarioName + " => " + sce.ScenarioName + ")");
								Pro.ProjectIsSave = false;
								mainwindow.UpdateStatusBar();																									
								return true;								
							}
							else if(_Choice == param.ParamI("MoSc_ChoiceData"))
							{	
								if(sce.ScenarioData != _Text)
								{
									sce.ScenarioData = _Text;
									CopyProject("HTModifyScenarioData",Pro.Project_Name + "/" + node.Node_Name + "/"  + sce.ScenarioName);
									mainwindow.UpdateMainNoteBook();
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab		
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																		
									return true;	
								}
								else
								{
									return false;
								}
							}
							else if(_Choice == param.ParamI("MoSc_ChoiceNote"))
							{	
								string OldScenarioNote = sce.ScenarioNotes;
								string NewScenarioNote = _Text;
								if(OldScenarioNote == "")
								{
									OldScenarioNote = param.ParamT("BI2CP_EmptyLabel");
								}
								sce.ScenarioNotes = NewScenarioNote;
								if(NewScenarioNote == "")
								{
									NewScenarioNote = param.ParamT("BI2CP_EmptyLabel");
								}
								CopyProject("HTModifyScenarioNote",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioNotes + " (" + OldScenarioNote + " => " + NewScenarioNote + ")");
								Pro.ProjectIsSave = false;
								mainwindow.UpdateStatusBar();																																										
								return true;								
							}
						}						
					}
				}
			}
			return false;
		}
		
		//Fonction ModifyVariable
		//Fonction permettant de modifier les variables
		public bool ModifyVariable(string _Text, int _Choice, Int32 _Variable_Id)
		{
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{
				foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
				{
					foreach(Scenario sce in node.ReturnListScenario())
					{
						foreach(Variable vari in sce.ReturnListVariable())
						{
							if(vari.VariableId == _Variable_Id)
							{
								if(_Choice == param.ParamI("MoVa_ChoiceName"))
								{	
									_Text = ReturnCorrectName(_Text);
									sce.ScenarioData = sce.ScenarioData.Replace(vari.VariableName,_Text);
									foreach(Function fun in sce.ReturnListFunction())
									{
										fun.FunctionData = fun.FunctionData.Replace(vari.VariableName,_Text);
									}
									
									string OldVariableName = vari.VariableName;
									vari.VariableName = _Text;
									mainwindow.UpdateMainNoteBook();
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyVariableName",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + " (" + OldVariableName + " => " + vari.VariableName + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																											
									return true;										
								}
								else if(_Choice == param.ParamI("MoVa_ChoiceType"))
								{	
									string OldVariableType = vari.VariableType;
									if(vari.VariableType == _Text)
									{	
										return false;	
									}
									vari.VariableType = _Text;
									vari.VariableDefaultValue = 0;
									mainwindow.UpdateMainNoteBook();
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyVariableType",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + vari.VariableName + " (" + OldVariableType + " => " + vari.VariableType + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																											
									return true;										
								}	
								else if(_Choice == param.ParamI("MoVa_ChoiceValue"))
								{	
									string OldVariableValue = vari.VariableDefaultValue.ToString();
									if(vari.VariableType == "bool")
									{
										if(_Text.Length == 1 && ((Convert.ToInt32(_Text[0]) == 48) || (Convert.ToInt32(_Text[0]) == 49)) && vari.VariableDefaultValue != Convert.ToInt32(_Text))
										{
											vari.VariableDefaultValue = Convert.ToInt32(_Text);
											mainwindow.UpdateMainNoteBook();
											mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
											CopyProject("HTModifyVariableValue",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + vari.VariableName + " (" + OldVariableValue + " => " + vari.VariableDefaultValue + ")");											
											Pro.ProjectIsSave = false;
											mainwindow.UpdateStatusBar();																																													
											return true;
										}
										else
										{
											
											return false;
										}
									}
									else if(vari.VariableType == "byte")
									{
										if(_Text.Length > 3)
										{
											return false;
										}
										for(int i=0;i<_Text.Length;i++)
										{
											if(Convert.ToInt32(_Text[i]) < 48 || Convert.ToInt32(_Text[i]) > 57)
											{
												return false;
											}						
										}
										if(Convert.ToInt32(_Text) > 255)
										{
											_Text = "255";
										}
										
										if(vari.VariableDefaultValue == Convert.ToInt32(_Text))
										{
											return false;
										}
										
										vari.VariableDefaultValue = Convert.ToInt32(_Text);
										mainwindow.UpdateMainNoteBook();
										mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab		
										CopyProject("HTModifyVariableValue",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + vari.VariableName + " (" + OldVariableValue + " => " + vari.VariableDefaultValue + ")");											
										Pro.ProjectIsSave = false;
										mainwindow.UpdateStatusBar();																																												
										return true;										
									}
									else if(vari.VariableType == "uint8_t")	
									{
										if(_Text.Length > 3)
										{
											return false;
										}
										for(int i=0;i<_Text.Length;i++)
										{
											if(Convert.ToInt32(_Text[i]) < 48 || Convert.ToInt32(_Text[i]) > 57)
											{
												return false;
											}						
										}
										if(Convert.ToInt32(_Text) > 255)
										{
											_Text = "255";
										}
										
										if(vari.VariableDefaultValue == Convert.ToInt32(_Text))
										{
											return false;
										}
										
										vari.VariableDefaultValue = Convert.ToInt32(_Text);
										mainwindow.UpdateMainNoteBook();
										mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab		
										CopyProject("HTModifyVariableValue",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + vari.VariableName + " (" + OldVariableValue + " => " + vari.VariableDefaultValue + ")");											
										Pro.ProjectIsSave = false;
										mainwindow.UpdateStatusBar();																																												
										return true;	
									}
									else if(vari.VariableType == "uint16_t")	
									{
										if(_Text.Length > 5)
										{
											return false;
										}
										for(int i=0;i<_Text.Length;i++)
										{
											if(Convert.ToInt32(_Text[i]) < 48 || Convert.ToInt32(_Text[i]) > 57)
											{
												return false;
											}						
										}
										if(Convert.ToInt32(_Text) > 65535)
										{
											_Text = "65535";
										}
										
										if(vari.VariableDefaultValue == Convert.ToInt32(_Text))
										{
											return false;
										}
										
										vari.VariableDefaultValue = Convert.ToInt32(_Text);
										mainwindow.UpdateMainNoteBook();
										mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab		
										CopyProject("HTModifyVariableValue",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + vari.VariableName + " (" + OldVariableValue + " => " + vari.VariableDefaultValue + ")");											
										Pro.ProjectIsSave = false;
										mainwindow.UpdateStatusBar();																																												
										return true;										
									}									
								}	
								else if(_Choice == param.ParamI("MoVa_ChoiceNote"))
								{	
									string OldVariableNote = vari.VariableNote;
									vari.VariableNote = _Text;
									mainwindow.UpdateMainNoteBook();
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyVariableNote",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + vari.VariableName + " (" + OldVariableNote + " => " + vari.VariableNote + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																											
									return true;	
								}									
							}
						}
					}
				}
			}
			return false;
		}
		
		//Fonction ModifyFunction
		//Fonction permettant de modifier les fonctions
		public bool ModifyFunction(string _Text, int _Choice, Int32 _Function_Id)
		{
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{
				foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
				{
					foreach(Scenario sce in node.ReturnListScenario())
					{
						foreach(Function fun in sce.ReturnListFunction())
						{
							if(fun.FunctionId == _Function_Id)
							{
								if(_Choice == param.ParamI("MoFu_ChoiceName") && !fun.InitFunction)
								{	
									_Text = ReturnCorrectName(_Text);	
									sce.ScenarioData = sce.ScenarioData.Replace(fun.FunctionName,_Text);
									string OldFunctionName = fun.FunctionName;
									fun.FunctionName = _Text;
									mainwindow.UpdateMainNoteBook();
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyFunctionName",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + " (" + OldFunctionName + " => " + fun.FunctionName + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																											
									return true;									
								}
								else if(_Choice == param.ParamI("MoFu_ChoiceType") && !fun.InitFunction)
								{
									string OldFunctionType = fun.FunctionTypeReturn;
									fun.FunctionTypeReturn = _Text;
									mainwindow.UpdateMainNoteBook();
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyFunctionType",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + fun.FunctionName + " (" + OldFunctionType + " => " + fun.FunctionTypeReturn + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																											
									return true;										
								}
								else if(_Choice == param.ParamI("MoFu_ChoiceNote") && !fun.InitFunction)
								{
									string OldFunctionNote = fun.FunctionNote;
									fun.FunctionNote = _Text;
									mainwindow.UpdateMainNoteBook();
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyFunctionNote",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + fun.FunctionName + " (" + OldFunctionNote + " => " + fun.FunctionNote + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																											
									return true;										
								}	
								else if(_Choice == param.ParamI("MoFu_ChoiceData"))
								{
									if(fun.FunctionData != _Text && _Text != "")
									{
										fun.FunctionData = _Text;
										CopyProject("HTModifyFunctionData",Pro.Project_Name + "/" + node.Node_Name + "/"  + fun.FunctionName);
										mainwindow.UpdateMainNoteBook();
										mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab								
										Pro.ProjectIsSave = false;
										mainwindow.UpdateStatusBar();																																												
										return true;	
									}
									else
									{
										return false;
									}
								}
								else if(_Choice == param.ParamI("MoFu_FunctionTypeArg1") && !fun.InitFunction)
								{
									string OldFunctionTypeArg1 = fun.FunctionTypeArg1;
									fun.FunctionTypeArg1 = _Text;
									mainwindow.UpdateMainNoteBook();
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyFunctionTypeArg1",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + fun.FunctionName + " (" + OldFunctionTypeArg1 + " => " + fun.FunctionTypeArg1 + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																											
									return true;
								}
								else if(_Choice == param.ParamI("MoFu_FunctionTypeArg2"))
								{
									string OldFunctionTypeArg2 = fun.FunctionTypeArg2;
									fun.FunctionTypeArg2 = _Text;
									mainwindow.UpdateMainNoteBook();
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyFunctionTypeArg1",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + fun.FunctionName + " (" + OldFunctionTypeArg2 + " => " + fun.FunctionTypeArg2 + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																											
									return true;									
								}
								else if(_Choice == param.ParamI("MoFu_FunctionTypeArg3"))
								{
									string OldFunctionTypeArg3 = fun.FunctionTypeArg3;
									fun.FunctionTypeArg3 = _Text;
									mainwindow.UpdateMainNoteBook();
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyFunctionTypeArg3",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + fun.FunctionName + " (" + OldFunctionTypeArg3 + " => " + fun.FunctionTypeArg3 + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																											
									return true;									
								}
								else if(_Choice == param.ParamI("MoFu_FunctionTypeArg4"))
								{
									string OldFunctionTypeArg4 = fun.FunctionTypeArg4;
									fun.FunctionTypeArg4 = _Text;
									mainwindow.UpdateMainNoteBook();
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyFunctionTypeArg4",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + fun.FunctionName + " (" + OldFunctionTypeArg4 + " => " + fun.FunctionTypeArg4 + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																											
									return true;										
								}
								else if(_Choice == param.ParamI("MoFu_FunctionTypeArg5"))
								{
									string OldFunctionTypeArg5 = fun.FunctionTypeArg5;
									fun.FunctionTypeArg5 = _Text;
									mainwindow.UpdateMainNoteBook();
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyFunctionTypeArg5",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + fun.FunctionName + " (" + OldFunctionTypeArg5 + " => " + fun.FunctionTypeArg5 + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																											
									return true;										
								}
								else if(_Choice == param.ParamI("MoFu_FunctionTypeArg6"))
								{
									string OldFunctionTypeArg6 = fun.FunctionTypeArg6;
									fun.FunctionTypeArg6 = _Text;
									mainwindow.UpdateMainNoteBook();
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyFunctionTypeArg6",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + fun.FunctionName + " (" + OldFunctionTypeArg6 + " => " + fun.FunctionTypeArg6 + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																											
									return true;										
								}	
								else if(_Choice == param.ParamI("MoFu_FunctionNameArg1") && !fun.InitFunction )
								{
									if(fun.FunctionNameArg1 != "")
									{
										fun.FunctionData = fun.FunctionData.Replace(fun.FunctionNameArg1,_Text);
									}
									string OldFunctionNameArg1 = fun.FunctionNameArg1;
									fun.FunctionNameArg1 = _Text;
									mainwindow.UpdateMainNoteBook();
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyFunctionNameArg1",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + fun.FunctionName + " (" + OldFunctionNameArg1 + " => " + fun.FunctionNameArg1 + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																											
									return true;									
								}
								else if(_Choice == param.ParamI("MoFu_FunctionNameArg2"))
								{
									if(fun.FunctionNameArg2 != "")
									{									
										fun.FunctionData = fun.FunctionData.Replace(fun.FunctionNameArg2,_Text);
									}
									string OldFunctionNameArg2 = fun.FunctionNameArg2;
									fun.FunctionNameArg2 = _Text;
									mainwindow.UpdateMainNoteBook();
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyFunctionNameArg2",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + fun.FunctionName + " (" + OldFunctionNameArg2 + " => " + fun.FunctionNameArg2 + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																											
									return true;									
								}
								else if(_Choice == param.ParamI("MoFu_FunctionNameArg3"))
								{
									if(fun.FunctionNameArg3 != "")
									{									
										fun.FunctionData = fun.FunctionData.Replace(fun.FunctionNameArg3,_Text);
									}
									string OldFunctionNameArg3 = fun.FunctionNameArg3;
									fun.FunctionNameArg3 = _Text;
									mainwindow.UpdateMainNoteBook();
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyFunctionNameArg3",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + fun.FunctionName + " (" + OldFunctionNameArg3 + " => " + fun.FunctionNameArg3 + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																											
									return true;									
								}
								else if(_Choice == param.ParamI("MoFu_FunctionNameArg4"))
								{
									if(fun.FunctionNameArg4 != "")
									{									
										fun.FunctionData = fun.FunctionData.Replace(fun.FunctionNameArg4,_Text);
									}
									string OldFunctionNameArg4 = fun.FunctionNameArg4;
									fun.FunctionNameArg4 = _Text;
									mainwindow.UpdateMainNoteBook();
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyFunctionNameArg4",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + fun.FunctionName + " (" + OldFunctionNameArg4 + " => " + fun.FunctionNameArg4 + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																											
									return true;										
								}
								else if(_Choice == param.ParamI("MoFu_FunctionNameArg5"))
								{
									if(fun.FunctionNameArg5 != "")
									{									
										fun.FunctionData = fun.FunctionData.Replace(fun.FunctionNameArg5,_Text);
									}
									string OldFunctionNameArg5 = fun.FunctionNameArg5;
									fun.FunctionNameArg5 = _Text;
									mainwindow.UpdateMainNoteBook();
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyFunctionNameArg5",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + fun.FunctionName + " (" + OldFunctionNameArg5 + " => " + fun.FunctionNameArg5 + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																											
									return true;
								}
								else if(_Choice == param.ParamI("MoFu_FunctionNameArg6"))
								{
									if(fun.FunctionNameArg6 != "")
									{									
										fun.FunctionData = fun.FunctionData.Replace(fun.FunctionNameArg6,_Text);
									}
									string OldFunctionNameArg6 = fun.FunctionNameArg6;
									fun.FunctionNameArg6 = _Text;
									mainwindow.UpdateMainNoteBook();
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyFunctionNameArg6",Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + fun.FunctionName + " (" + OldFunctionNameArg6 + " => " + fun.FunctionNameArg6 + ")");
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																											
									return true;									
								}
								else if(_Choice == param.ParamI("MoFu_FunctionArgReorganisation"))
								{
									if(Convert.ToInt32(_Text) == 1)
									{
										fun.FunctionNameArg1 = fun.FunctionNameArg2;
										fun.FunctionTypeArg1 = fun.FunctionTypeArg2;

										fun.FunctionNameArg2 = fun.FunctionNameArg3;
										fun.FunctionTypeArg2 = fun.FunctionTypeArg3;

										fun.FunctionNameArg3 = fun.FunctionNameArg4;
										fun.FunctionTypeArg3 = fun.FunctionTypeArg4;

										fun.FunctionNameArg4 = fun.FunctionNameArg5;
										fun.FunctionTypeArg4 = fun.FunctionTypeArg5;
										
										fun.FunctionNameArg5 = fun.FunctionNameArg6;
										fun.FunctionTypeArg5 = fun.FunctionTypeArg6;	
										
										fun.FunctionNameArg6 = "";
										fun.FunctionTypeArg6 = "bool";
									}
									else if(Convert.ToInt32(_Text) == 2)
									{
										fun.FunctionNameArg2 = fun.FunctionNameArg3;
										fun.FunctionTypeArg2 = fun.FunctionTypeArg3;

										fun.FunctionNameArg3 = fun.FunctionNameArg4;
										fun.FunctionTypeArg3 = fun.FunctionTypeArg4;

										fun.FunctionNameArg4 = fun.FunctionNameArg5;
										fun.FunctionTypeArg4 = fun.FunctionTypeArg5;
										
										fun.FunctionNameArg5 = fun.FunctionNameArg6;
										fun.FunctionTypeArg5 = fun.FunctionTypeArg6;	
										
										fun.FunctionNameArg6 = "";
										fun.FunctionTypeArg6 = "bool";										
									}
									else if(Convert.ToInt32(_Text) == 3)
									{
										fun.FunctionNameArg3 = fun.FunctionNameArg4;
										fun.FunctionTypeArg3 = fun.FunctionTypeArg4;

										fun.FunctionNameArg4 = fun.FunctionNameArg5;
										fun.FunctionTypeArg4 = fun.FunctionTypeArg5;
										
										fun.FunctionNameArg5 = fun.FunctionNameArg6;
										fun.FunctionTypeArg5 = fun.FunctionTypeArg6;	
										
										fun.FunctionNameArg6 = "";
										fun.FunctionTypeArg6 = "bool";										
									}									
									else if(Convert.ToInt32(_Text) == 4)
									{
										fun.FunctionNameArg4 = fun.FunctionNameArg5;
										fun.FunctionTypeArg4 = fun.FunctionTypeArg5;
										
										fun.FunctionNameArg5 = fun.FunctionNameArg6;
										fun.FunctionTypeArg5 = fun.FunctionTypeArg6;	
										
										fun.FunctionNameArg6 = "";
										fun.FunctionTypeArg6 = "bool";										
									}	
									else if(Convert.ToInt32(_Text) == 5)
									{	
										fun.FunctionNameArg5 = fun.FunctionNameArg6;
										fun.FunctionTypeArg5 = fun.FunctionTypeArg6;	
										
										fun.FunctionNameArg6 = "";
										fun.FunctionTypeArg6 = "bool";
									}	
									else if(Convert.ToInt32(_Text) == 6)
									{											
										fun.FunctionNameArg6 = "";
										fun.FunctionTypeArg6 = "bool";
									}										
									mainwindow.UpdateMainNoteBook();
									mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
									CopyProject("HTModifyFunctionArgReorganisation" + _Text,Pro.Project_Name + "/" + node.Node_Name + "/" + sce.ScenarioName + "/" + fun.FunctionName);
									Pro.ProjectIsSave = false;
									mainwindow.UpdateStatusBar();																																											
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}
		
		//Fonction ModifyPin
		//Fonction permettant de modifier les pin d'une carte
		public bool ModifyInstancePinInBoard(Int32 _Texte, Int32 _Pin_Id)
		{
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{
				foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
				{
					foreach(Network net in node.ReturnListNetwork()) //Dans la liste des réseau de chaque noeud
					{
						foreach(Board boa in net.ReturnListBoard()) //Dans la liste des carte de chaque réseau
						{
							foreach(Pin pin in boa.ReturnListPin()) //Dans la liste des pin de chaque carte
							{
								if(pin.Pin_Id == _Pin_Id) //Si l'id de la broche est celle que nous avons rentré en paramètre
								{
									string OldPinInstanceId = param.ParamT("BI2CP_EmptyLabel");
									string NewPinInstanceId = param.ParamT("BI2CP_EmptyLabel");									
									foreach(Instance ins in node.ReturnListInstance())
									{
										if(ins.Instance_Id == pin.Instance_Id)
										{
											OldPinInstanceId = ins.Instance_Name;
										}
										if(ins.Instance_Id ==_Texte)
										{
											NewPinInstanceId = ins.Instance_Name;
										}										
									}
									if(pin.Instance_Id != _Texte)
									{
										pin.Instance_Id = _Texte; //Nous modifion l'instance id
										CopyProject("HTModifyInstancePinInBoard",Pro.Project_Name + "/" + node.Node_Name + "/" + net.Network_Type + "/" + boa.Board_Name + "/" + pin.Pin_Name + " (" + OldPinInstanceId + " => " + NewPinInstanceId + ")");
										Pro.ProjectIsSave = false;
										mainwindow.UpdateStatusBar();																																												
										return true;
									}
								}
							}
						}
					}
				}
			}
			return false;
		}
				
		// Fonction ModifyPin
		//Fonction permettant de modifier les broches d'une carte I2C
		public bool ModifyPin(string _Text, int _Choice, Int32 _Pin_Id)
		{
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{
				foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
				{
					foreach(Network net in node.ReturnListNetwork()) //Dans la liste des réseau de chaque noeud
					{
						foreach(Board boa in net.ReturnListBoard()) //Dans la liste des carte de chaque réseau
						{
							foreach(Pin pin in boa.ReturnListPin()) //Dans la liste des pin de chaque carte
							{
								if(pin.Pin_Id == _Pin_Id) //Si l'id de la broche est celle que nous avons rentré en paramètre
								{
									if(_Choice == param.ParamI("MoPi_ChoiceFallbackValue"))
									{
										if(_Text == "True")//Si le texte est True
										{
											string OldFallBackValue = pin.Pin_FallbackValue.ToString();
											if(OldFallBackValue != _Text)
											{	
												pin.Pin_FallbackValue  = true;//On met le booleen à vrai
												mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
												CopyProject("HTModifyPinFallBackValue",Pro.Project_Name + "/" + node.Node_Name + "/" + net.Network_Type + "/" + boa.Board_Name + "/" + pin.Pin_Name + " (" + OldFallBackValue + " => " + pin.Pin_FallbackValue.ToString() + ")");
												mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
												Pro.ProjectIsSave = false;
												mainwindow.UpdateStatusBar();																																														
												return true;
											}
										}
										else
										{
											string OldFallBackValue = pin.Pin_FallbackValue.ToString();
											if(OldFallBackValue != _Text)
											{									
												pin.Pin_FallbackValue  = false;//Sinon nous le mettons à faux
												mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
												CopyProject("HTModifyPinFallBackValue",Pro.Project_Name + "/" + node.Node_Name + "/" + net.Network_Type + "/" + boa.Board_Name + "/" + pin.Pin_Name + " (" + OldFallBackValue + " => " + pin.Pin_FallbackValue.ToString() + ")");
												mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
												Pro.ProjectIsSave = false;
												mainwindow.UpdateStatusBar();																																														
												return true;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return false;
		}
		
		//Fonction UpdateInstanceUsed
		//Fonction permettant de savoir si une instance est posé sur une carte
		public void UpdateInstanceUsed()
		{
			foreach(Project pro in ListProject) //Dans la liste des projets
			{
				foreach(Node node in pro.ReturnListNode()) //Dans la liste des noeud propre à un projet
				{
					foreach(Instance ins in node.ReturnListInstance()) //Dans la liste des instances propre à un noeud
					{
						ins.Instance_Used = false; //Nous mettons la valeur permettant de dire que l'instance n'est pas utilisé
						ins.Instance_Used_0 = 0; //Pous les shutter à deux broches : on met cette valeur à 0
						ins.Instance_Used_1 = 0; //Pous les shutter à trois broches : on met cette valeur à 0
						ins.Pin_Id_0 = 0;
						ins.Pin_Id_1 = 0;
						ins.Pin_Id_2 = 0;
						foreach(Project _pro in ListProject) //Dans la liste des projet
						{
							foreach(Node _node in _pro.ReturnListNode()) //Dans la liste des noeud propre au projet
							{
								foreach(Network _net in _node.ReturnListNetwork()) //Dans la liste des réseaux propre au noeud
								{
									foreach(Board _boa in _net.ReturnListBoard()) //Dans la liste des carte propre au réseau
									{
										foreach(Pin _pin in _boa.ReturnListPin()) //Dans la liste des broches propre à la carte
										{
											if(ins.Instance_Type != param.ParamP("InstShutterName")) //Si l'instance est différent de type shutter
											{
												if(ins.Instance_Id == _pin.Instance_Id) //Si l'id de la pin est égale à l'instance
												{
													ins.Instance_Used = true; //Veut dire que l'instance est utilisé
													ins.Pin_Id_0 = _pin.Pin_Id;
												}
											}
											else //Dans le cas ou l'instance est de type shutter
											{
												if(ins.Instance_SHU_NumberOfOutput == 2)//Si le nombre de sortie du volet est de deux
												{
													if((ins.Instance_Id == _pin.Instance_Id) && (ins.Instance_Used_0 == 0))//Premier fois utilisé
													{
														ins.Instance_Used_0 = _boa.Board_Id; //on met Instance_Used_0 aavec la valeur de la carte qui est rattaché
														if(ins.Instance_Up_Down_Stop == 0)
														{
															ins.Pin_Id_0 = _pin.Pin_Id;
														}
														else if(ins.Instance_Up_Down_Stop == 1)
														{
															ins.Pin_Id_1 = _pin.Pin_Id;
														}														
													}
													else if((ins.Instance_Id == _pin.Instance_Id) && (ins.Instance_Used_0 > 0)) //Seconde fois utilisé
													{
														ins.Instance_Used = true; //On met la variable d'utilisation à vrai
														ins.Instance_Used_0 = _boa.Board_Id;//on met Instance_Used_0 avec la valeur de la carte qui est rattaché
														if(ins.Instance_Up_Down_Stop == 0)
														{
															ins.Pin_Id_1 = _pin.Pin_Id;
														}
														else if(ins.Instance_Up_Down_Stop == 1)
														{
															ins.Pin_Id_0 = _pin.Pin_Id;
														}	
													}
												}
												else if(ins.Instance_SHU_NumberOfOutput == 3)//Si le nombre de sortie du volet est de trois
												{
													if((ins.Instance_Id == _pin.Instance_Id) && (ins.Instance_Used_0 == 0)) //Premier fois utilisé
													{
														ins.Instance_Used_0 = _boa.Board_Id;//on met Instance_Used_0 aavec la valeur de la carte qui est rattaché
														if(ins.Instance_Up_Down_Stop == 0)
														{
															ins.Pin_Id_0 = _pin.Pin_Id;
														}
														else if(ins.Instance_Up_Down_Stop == 1)
														{
															ins.Pin_Id_0 = _pin.Pin_Id;
														}	
														else if(ins.Instance_Up_Down_Stop == 2)
														{
															ins.Pin_Id_1 = _pin.Pin_Id;
														}	
														else if(ins.Instance_Up_Down_Stop == 3)
														{
															ins.Pin_Id_1 = _pin.Pin_Id;
														}	
														else if(ins.Instance_Up_Down_Stop == 4)
														{
															ins.Pin_Id_2 = _pin.Pin_Id;
														}	
														else if(ins.Instance_Up_Down_Stop == 5)
														{
															ins.Pin_Id_2 = _pin.Pin_Id;
														}															
													}
													else if((ins.Instance_Id == _pin.Instance_Id) && (ins.Instance_Used_0 > 0) && (ins.Instance_Used_1 == 0))//Seconde fois utilisé
													{
														ins.Instance_Used_0 = _boa.Board_Id;//on met Instance_Used_0 aavec la valeur de la carte qui est rattaché
														ins.Instance_Used_1 = _boa.Board_Id;//on met Instance_Used_1 aavec la valeur de la carte qui est rattaché
														if(ins.Instance_Up_Down_Stop == 0)
														{
															ins.Pin_Id_1 = _pin.Pin_Id;
														}
														else if(ins.Instance_Up_Down_Stop == 1)
														{
															ins.Pin_Id_2 = _pin.Pin_Id;
														}	
														else if(ins.Instance_Up_Down_Stop == 2)
														{
															ins.Pin_Id_2 = _pin.Pin_Id;
														}	
														else if(ins.Instance_Up_Down_Stop == 3)
														{
															ins.Pin_Id_0 = _pin.Pin_Id;
														}	
														else if(ins.Instance_Up_Down_Stop == 4)
														{
															ins.Pin_Id_1 = _pin.Pin_Id;
														}	
														else if(ins.Instance_Up_Down_Stop == 5)
														{
															ins.Pin_Id_0 = _pin.Pin_Id;
														}
													}
													else if((ins.Instance_Id == _pin.Instance_Id) && (ins.Instance_Used_0 > 0) && (ins.Instance_Used_1 > 0))//Troisième fois utilisé												
													{
														ins.Instance_Used = true;
														ins.Instance_Used_0 = _boa.Board_Id;//on met Instance_Used_0 aavec la valeur de la carte qui est rattaché
														ins.Instance_Used_1 = _boa.Board_Id;//on met Instance_Used_1 aavec la valeur de la carte qui est rattaché
														if(ins.Instance_Up_Down_Stop == 0)
														{
															ins.Pin_Id_2 = _pin.Pin_Id;
														}
														else if(ins.Instance_Up_Down_Stop == 1)
														{
															ins.Pin_Id_1 = _pin.Pin_Id;
														}	
														else if(ins.Instance_Up_Down_Stop == 2)
														{
															ins.Pin_Id_0 = _pin.Pin_Id;
														}	
														else if(ins.Instance_Up_Down_Stop == 3)
														{
															ins.Pin_Id_2 = _pin.Pin_Id;
														}	
														else if(ins.Instance_Up_Down_Stop == 4)
														{
															ins.Pin_Id_0 = _pin.Pin_Id;
														}	
														else if(ins.Instance_Up_Down_Stop == 5)
														{
															ins.Pin_Id_1 = _pin.Pin_Id;
														}
													}												
												}
											}
										}
									}
								}
							}
						}
					}		
				}
			}
			mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
		}
		
		//Fonction UpdateActionInScenarioAndFunction
		//Fonction permettant de modifier les actions dans les scénarios et les fonctions 
		public void UpdateActionInScenarioAndFunction(string _OldTexte, string _NewText)
		{
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{
				foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
				{
					foreach(Scenario sce in node.ReturnListScenario())
					{
						sce.ScenarioData = sce.ScenarioData.Replace(" " + _OldTexte + " ", " " + _NewText + " ");
						foreach(Function fun in sce.ReturnListFunction())
						{
							fun.FunctionData = fun.FunctionData.Replace(" " + _OldTexte + " ", " " + _NewText + " ");
						}
					}
				}
			}
		}
		
//################Fonction permettant d'ajouter des cartes avec leur spécificité #######

		//Fonction AddBoards
		//Fonction permettant de faire la liste des carte xPLduino
		//Argument :
		//	Auncun
		//Retour :
		//	Vrai si réussit
		public bool AddBoards()
		{					
			ListBoards.Add(new Boards("IN8R8",8,"IN",8,"OUT",1,"I2C",16,"in8R8_to_switch","in8R8_to_lighting","lighting_to_in8R8","shutter_to_in8R8"));
			ListBoards.Add(new Boards("IN4DIM4",4,"IN",4,"DIM",1,"I2C",16,"dimmer_to_switch","dimmer_to_lighting","lighting_to_dimmer",""));
			ListBoards.Add(new Boards("IN16",16,"IN",0,"OUT",2,"I2C",8,"in16_to_switch","","",""));
			ListBoards.Add(new Boards("R8",0,"IN",8,"OUT",1,"I2C",8,"","R8_to_lighting","lighting_to_R8","shutter_to_R8"));
			ListBoards.Add(new Boards("TEMPERATURE",0,"",0,"",0,"1-Wire",0,"","","",""));
			ListBoards.Add(new Boards("EDF",0,"",0,"",0,"RS485",0,"","","",""));
			return true;
		}
		
//################Fonction permettant d'ajouter des Elements dans la liste du menu dans le TextEditor Customer #######
		
		public void AddElementInListMenuTextEditor()
		{
			ListMenuTextEditor.Clear();
			CountIdMenuCustomerEdit = 1;
			IdParentColumn1 = 0;
			IdParentColumn2 = 0;			
			
			IdParentColumn1 = AddOneElementInLMTE(1,0,false,param.ParamT("CE_ItemMenuStandardCPP"),"","");
				IdParentColumn2 = AddOneElementInLMTE(2,IdParentColumn1,false,param.ParamT("CE_Header"),"","");
					AddOneElementInLMTE(3,IdParentColumn2,true,"#define","#define","");
					AddOneElementInLMTE(3,IdParentColumn2,true,"#include","#include","");
				IdParentColumn2 = AddOneElementInLMTE(2,IdParentColumn1,false,param.ParamT("CE_VariableType"),"","");
					AddOneElementInLMTE(3,IdParentColumn2,true,"bool","bool","");
					AddOneElementInLMTE(3,IdParentColumn2,true,"byte","byte","");
					AddOneElementInLMTE(3,IdParentColumn2,true,"uint-8","uint_8","");
					AddOneElementInLMTE(3,IdParentColumn2,true,"uint-16","uint_16","");			
				IdParentColumn2 = AddOneElementInLMTE(2,IdParentColumn1,false,param.ParamT("CE_LoopAndCondition"),"","");
					AddOneElementInLMTE(3,IdParentColumn2,true,"if","if()\n{\n\n}","");
					AddOneElementInLMTE(3,IdParentColumn2,true,"if else","if()\n{\n\n}\nelse\n{\n\n}","");	
					AddOneElementInLMTE(3,IdParentColumn2,true,"if else if","if()\n{\n\n}\nelse if()\n{\n\n}","");	
				IdParentColumn2 = AddOneElementInLMTE(2,IdParentColumn1,false,param.ParamT("CE_Gate"),"","");
					AddOneElementInLMTE(3,IdParentColumn2,true,"and","&&","");
					AddOneElementInLMTE(3,IdParentColumn2,true,"or","||","");
					AddOneElementInLMTE(3,IdParentColumn2,true,"xor","^","");	
			IdParentColumn1 = AddOneElementInLMTE(1,0,false,param.ParamT("CE_ItemMenuMacroProject"),"","");
				IdParentColumn2 = AddOneElementInLMTE(2,IdParentColumn1,false,param.ParamT("CE_Project"),"","");
					AddOneElementInLMTE(3,IdParentColumn2,true,param.ParamT("CE_ProjectName"),"[PROJECTNAME]","[PROJECTNAME]");
					AddOneElementInLMTE(3,IdParentColumn2,true,param.ParamT("CE_ProjectAuthor"),"[PROJECTAUTHOR]","[PROJECTAUTHOR]");
					AddOneElementInLMTE(3,IdParentColumn2,true,param.ParamT("CE_ProjectDateTimeCreation"),"[PROJECTDATETIMECREATION]","[PROJECTDATETIMECREATION]");						
					AddOneElementInLMTE(3,IdParentColumn2,true,param.ParamT("CE_ProjectDateTimeModification"),"[PROJECTDATETIMEMODIFICATION]","[PROJECTDATETIMEMODIFICATION]");	
					AddOneElementInLMTE(3,IdParentColumn2,true,param.ParamT("CE_ProjectVersion"),"[PROJECTVERSION]","[PROJECTVERSION]");	
					AddOneElementInLMTE(3,IdParentColumn2,true,param.ParamT("CE_ProjectNote"),"[PROJECTNOTE]","[PROJECTNOTE]");	
				IdParentColumn2 = AddOneElementInLMTE(2,IdParentColumn1,false,param.ParamT("CE_Node"),"","");
					AddOneElementInLMTE(3,IdParentColumn2,true,param.ParamT("CE_SMBName"),"[NODENAME]","[NODENAME]");
					AddOneElementInLMTE(3,IdParentColumn2,true,param.ParamT("CE_SMBType"),"[NODETYPE]","[NODETYPE]");	
					AddOneElementInLMTE(3,IdParentColumn2,true,param.ParamT("CE_SMBIP"),"[NODEIP]","[NODEIP]");
					AddOneElementInLMTE(3,IdParentColumn2,true,param.ParamT("CE_SMBGTWIP"),"[NODEGWIP]","[NODEGWIP]");
					AddOneElementInLMTE(3,IdParentColumn2,true,param.ParamT("CE_SMBMAC"),"[NODEMAC]","[NODEMAC]");
					AddOneElementInLMTE(3,IdParentColumn2,false,"SeparatorMenuItem","","");
					foreach(Boards boas in ListBoards)
					{
						if(boas.NetworkType == "I2C")
						{
							AddOneElementInLMTE(3,IdParentColumn2,true,param.ParamT("CE_BoardNumber") + boas.Type,"[COUNT" + boas.Type + "]","[COUNT" + boas.Type + "]");
						}
					}
					AddOneElementInLMTE(3,IdParentColumn2,false,"SeparatorMenuItem","","");			
					AddOneElementInLMTE(3,IdParentColumn2,true,param.ParamT("CE_LightingNumber"),"[COUNTLIGHTING]","[COUNTLIGHTING]");	
					AddOneElementInLMTE(3,IdParentColumn2,true,param.ParamT("CE_SwitchNumber"),"[COUNTSWITCH]","[COUNTSWITCH]");	
					AddOneElementInLMTE(3,IdParentColumn2,true,param.ParamT("CE_ShutterNumber"),"[COUNTSHUTTER]","[COUNTSHUTTER]");	
					AddOneElementInLMTE(3,IdParentColumn2,false,"SeparatorMenuItem","","");			
					AddOneElementInLMTE(3,IdParentColumn2,true,param.ParamT("CE_TempNumber"),"[COUNTTEMP]","[COUNTTEMP]");
			IdParentColumn1 = AddOneElementInLMTE(1,0,false,param.ParamT("CE_ItemMenuMacroGlobal"),"","");
				IdParentColumn2 = AddOneElementInLMTE(2,IdParentColumn1,true,param.ParamT("CE_ConfigurationBoard"),"[CONFIGBOARD]","[CONFIGBOARD]");
				IdParentColumn2 = AddOneElementInLMTE(2,IdParentColumn1,false,"SeparatorMenuItem","","");
				IdParentColumn2 = AddOneElementInLMTE(2,IdParentColumn1,true,param.ParamT("CE_ConfigurationSwitch"),"[CONFIGSWITCH]","[CONFIGSWITCH]");
				IdParentColumn2 = AddOneElementInLMTE(2,IdParentColumn1,true,param.ParamT("CE_ConfigurationShutter"),"[CONFIGSHUTTER]","[CONFIGSHUTTER]");
				IdParentColumn2 = AddOneElementInLMTE(2,IdParentColumn1,true,param.ParamT("CE_ConfigurationLighting"),"[CONFIGLIGHTING]","[CONFIGLIGHTING]");
				IdParentColumn2 = AddOneElementInLMTE(2,IdParentColumn1,false,"SeparatorMenuItem","","");
				IdParentColumn2 = AddOneElementInLMTE(2,IdParentColumn1,true,param.ParamT("CE_ConfigurationTemp"),"[CONFIGTEMPERATURE]","[CONFIGTEMPERATURE]");
				IdParentColumn2 = AddOneElementInLMTE(2,IdParentColumn1,false,"SeparatorMenuItem","","");
				IdParentColumn2 = AddOneElementInLMTE(2,IdParentColumn1,true,param.ParamT("CE_PreUpdate"),"[PREUPDATE]","[PREUPDATE]");
				IdParentColumn2 = AddOneElementInLMTE(2,IdParentColumn1,true,param.ParamT("CE_PostUpdate"),"[POSTUPDATE]","[POSTUPDATE]");			
		}
		
		public int AddOneElementInLMTE(int Column, int IdParent, bool Active, string Name, string Text, string ToolTip)
		{
			ListMenuTextEditor.Add(new MenuTextEditor(Column,CountIdMenuCustomerEdit,IdParent,Active,Name,Text,ToolTip));
			CountIdMenuCustomerEdit++;
			return CountIdMenuCustomerEdit-1;
		}
		
//################Fonction permettant d'ajouter des Elements dans la liste du menu dans le TextEditor Scenario #######
		
		public void AddElementInListMenuTextEditorScenario(Int32 _Node_Id, Int32 _ScenarioId)
		{
			ListMenuTextEditorScenario.Clear();
			CountIdMenuScenarioEdit = 1;		
			IdParentColumn1Scenario = 0;
			IdParentColumn2Scenario = 0;
			
			IdParentColumn1Scenario = AddOneElementInLMTES(1,0,false,param.ParamT("CE_ItemMenuStandardCPP"),"","");
				IdParentColumn2Scenario = AddOneElementInLMTES(2,IdParentColumn1Scenario,false,param.ParamT("CE_VariableType"),"","");
					AddOneElementInLMTES(3,IdParentColumn2Scenario,true,"bool","bool","");
					AddOneElementInLMTES(3,IdParentColumn2Scenario,true,"byte","byte","");
					AddOneElementInLMTES(3,IdParentColumn2Scenario,true,"uint-8","uint_8","");
					AddOneElementInLMTES(3,IdParentColumn2Scenario,true,"uint-16","uint_16","");	
				IdParentColumn2Scenario = AddOneElementInLMTES(2,IdParentColumn1Scenario,false,param.ParamT("CE_LoopAndCondition"),"","");
					AddOneElementInLMTES(3,IdParentColumn2Scenario,true,"if","if()\n{\n\n}","");
					AddOneElementInLMTES(3,IdParentColumn2Scenario,true,"if else","if()\n{\n\n}\nelse\n{\n\n}","");	
					AddOneElementInLMTES(3,IdParentColumn2Scenario,true,"if else if","if()\n{\n\n}\nelse if()\n{\n\n}","");	
				IdParentColumn2Scenario = AddOneElementInLMTES(2,IdParentColumn1Scenario,false,param.ParamT("CE_Gate"),"","");
					AddOneElementInLMTES(3,IdParentColumn2Scenario,true,"and","&&","");
					AddOneElementInLMTES(3,IdParentColumn2Scenario,true,"or","||","");
					AddOneElementInLMTES(3,IdParentColumn2Scenario,true,"xor","^","");
			
			
				bool AddSeparatorLineOneWire = false;
				foreach(Project Pro in ListProject) //Dans la liste des projets
				{
					foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
					{
						if(node.Node_Id == _Node_Id)
						{						
							foreach(Network net in node.ReturnListNetwork())
							{
								foreach(Board boa in net.ReturnListBoard())
								{
									if(boa.Board_Type == "TEMPERATURE")
									{
										if(!AddSeparatorLineOneWire)
										{
											AddOneElementInLMTES(1,0,false,"SeparatorMenuItem","","");
												IdParentColumn1Scenario = AddOneElementInLMTES(1,0,false,param.ParamT("AP_TEMPName"),"","");
											AddSeparatorLineOneWire = true;
										}
											IdParentColumn2Scenario = AddOneElementInLMTES(2,IdParentColumn1Scenario,false,boa.Board_Name.Replace("_","-"),"","");
												AddOneElementInLMTES(3,IdParentColumn2Scenario,true,param.ParamT("AP_TEMPGet"), pref.TEMPGetValue + "( " + boa.Board_Name + " );", pref.TEMPGetValue + "( " + boa.Board_Name + " )");
									}
								}
							}
						}
					}
				}
			
			
			
			AddOneElementInLMTES(1,0,false,"SeparatorMenuItem","","");
						
				bool LihgtingOK = false;
				bool SwitchOK = false;
				bool ShutterOK = false;
			
				foreach(Project Pro in ListProject) //Dans la liste des projets
				{
					foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
					{
						if(node.Node_Id == _Node_Id)
						{		
							foreach(Instance ins in node.ReturnListInstance())
							{
								if(ins.Instance_Type == param.ParamP("InstLightingName"))
								{
									LihgtingOK = true;
								}
								if(ins.Instance_Type == param.ParamP("InstSwitchName"))
								{
									SwitchOK = true;
								}	
								if(ins.Instance_Type == param.ParamP("InstShutterName"))
								{
									ShutterOK = true;
								}							
							}
						
							if(SwitchOK)
							{
								IdParentColumn1Scenario = AddOneElementInLMTES(1,0,false,param.ParamT("ExTVNameSwitch"),"","");
								foreach(Instance ins in node.ReturnListInstance())
								{							
									if(ins.Instance_Type == param.ParamP("InstSwitchName"))
									{
										IdParentColumn2Scenario = AddOneElementInLMTES(2,IdParentColumn1Scenario,false,ins.Instance_Name.Replace("_","-"),"","");
											AddOneElementInLMTES(3,IdParentColumn2Scenario,true,param.ParamT("AP_SWIPulse"),pref.SWIClicActionName + "( " + ins.Instance_Name + " );",pref.SWIClicActionName + "( " + ins.Instance_Name + " )");
											AddOneElementInLMTES(3,IdParentColumn2Scenario,true,param.ParamT("AP_SWIDPulse"),pref.SWIDoubleClicActionName + "( " + ins.Instance_Name + " );",pref.SWIDoubleClicActionName + "( " + ins.Instance_Name + " )");
											AddOneElementInLMTES(3,IdParentColumn2Scenario,true,param.ParamT("AP_SWIOn"),pref.SWIOnActionName + "( " + ins.Instance_Name + " );",pref.SWIOnActionName + "( " + ins.Instance_Name + " )");	
											AddOneElementInLMTES(3,IdParentColumn2Scenario,true,param.ParamT("AP_SWIOnRe"),pref.SWIOnFmActionName + "( " + ins.Instance_Name + " );",pref.SWIOnFmActionName + "( " + ins.Instance_Name + " )");	
											AddOneElementInLMTES(3,IdParentColumn2Scenario,true,param.ParamT("AP_SWIOff"),pref.SWIOffActionName + "( " + ins.Instance_Name + " );",pref.SWIOffActionName + "( " + ins.Instance_Name + " )");	
											AddOneElementInLMTES(3,IdParentColumn2Scenario,true,param.ParamT("AP_SWIOffFe"),pref.SWIOffFmActionName + "( " + ins.Instance_Name + " );",pref.SWIOffFmActionName + "( " + ins.Instance_Name + " )");	
										
									}			
								}
							}
							if(LihgtingOK)
							{
								IdParentColumn1Scenario = AddOneElementInLMTES(1,0,false,param.ParamT("ExTVNameLighting"),"","");
								foreach(Instance ins in node.ReturnListInstance())
								{							
									if(ins.Instance_Type == param.ParamP("InstLightingName"))
									{
										IdParentColumn2Scenario = AddOneElementInLMTES(2,IdParentColumn1Scenario,false,ins.Instance_Name.Replace("_","-"),"","");
											AddOneElementInLMTES(3,IdParentColumn2Scenario,true,param.ParamT("AP_LIGToggle"), pref.LIGToggleActionName + "( " + ins.Instance_Name + " );", pref.LIGToggleActionName + "( " + ins.Instance_Name + " )");
											AddOneElementInLMTES(3,IdParentColumn2Scenario,true,param.ParamT("AP_LIGTune"), pref.LIGTuneActionName + "( " + ins.Instance_Name + " );", pref.LIGTuneActionName + "( " + ins.Instance_Name + " )");
											AddOneElementInLMTES(3,IdParentColumn2Scenario,true,param.ParamT("AP_LIGStop"), pref.LIGStopActionName + "( " + ins.Instance_Name + " );", pref.LIGStopActionName + "( " + ins.Instance_Name + " )");
											AddOneElementInLMTES(3,IdParentColumn2Scenario,true,param.ParamT("AP_LIGSet"), pref.LIGSetActionName + "( " + ins.Instance_Name + " ,/*value*/);", pref.LIGStopActionName + "( " + ins.Instance_Name + " ,/*value*/)");
									}	
								}
							}					  
							if(ShutterOK)
							{
								IdParentColumn1Scenario = AddOneElementInLMTES(1,0,false,param.ParamT("ExTVNameShutter"),"","");
								foreach(Instance ins in node.ReturnListInstance())
								{								
									if(ins.Instance_Type == param.ParamP("InstShutterName"))
									{
										IdParentColumn2Scenario = AddOneElementInLMTES(2,IdParentColumn1Scenario,false,ins.Instance_Name.Replace("_","-"),"","");
											AddOneElementInLMTES(3,IdParentColumn2Scenario,true,param.ParamT("AP_SHUOpen"),pref.SHUOpenActionName + "( " + ins.Instance_Name + " );",pref.SHUOpenActionName + "( " + ins.Instance_Name + " )");
											AddOneElementInLMTES(3,IdParentColumn2Scenario,true,param.ParamT("AP_SHUClose"),pref.SHUCloseActionName + "( " + ins.Instance_Name + " );",pref.SHUCloseActionName + "( " + ins.Instance_Name + " )");
											AddOneElementInLMTES(3,IdParentColumn2Scenario,true,param.ParamT("AP_SHUStop"),pref.SHUStopActionName + "( " + ins.Instance_Name + " );",pref.SHUStopActionName + "( " + ins.Instance_Name + " )");									
											AddOneElementInLMTES(3,IdParentColumn2Scenario,true,param.ParamT("AP_SHUToggle"),pref.SHUToggleActionName + "( " + ins.Instance_Name + " );",pref.SHUToggleActionName + "( " + ins.Instance_Name + " )");
									}
								}
							}					
							
						}
					}
				}	
			
			AddOneElementInLMTES(1,0,false,"SeparatorMenuItem","","");
			bool CountVariableOK = true;
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{
				foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
				{
					foreach(Scenario sce in node.ReturnListScenario())
					{
						if(sce.ScenarioId == _ScenarioId)
						{
							if(sce.ReturnListVariable().Count > 0)
							{
								CountVariableOK = false;
							}
						}
					}
				}
			}								
			
			IdParentColumn1Scenario = AddOneElementInLMTES(1,0,CountVariableOK,param.ParamT("CRM_Variable"),"","");
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{
				foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
				{
					foreach(Scenario sce in node.ReturnListScenario())
					{
						if(sce.ScenarioId == _ScenarioId)
						{
							foreach(Variable vari in sce.ReturnListVariable())
							{
								IdParentColumn2Scenario = AddOneElementInLMTES(2,IdParentColumn1Scenario,true,vari.VariableName.Replace("_","-"),vari.VariableName,"");
							}
						}
					}
				}
			}
			
			
			bool CountFunctionOK = true;
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{
				foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
				{
					foreach(Scenario sce in node.ReturnListScenario())
					{
						if(sce.ScenarioId == _ScenarioId)
						{
							if(sce.ReturnListFunction().Count > 1)
							{
								CountFunctionOK = false;
							}
						}
					}
				}
			}								
			
			IdParentColumn1Scenario = AddOneElementInLMTES(1,0,CountFunctionOK,param.ParamT("CRM_Function"),"","");
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{
				foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
				{
					foreach(Scenario sce in node.ReturnListScenario())
					{
						if(sce.ScenarioId == _ScenarioId)
						{
							foreach(Function fun in sce.ReturnListFunction())
							{
								if(!fun.InitFunction)
								{
									
									string Args1 = "";
									string Args2 = "";
									string Args3 = "";
									string Args4 = "";
									string Args5 = "";
									string Args6 = "";
								
									if(fun.FunctionNameArg1 != "")
									{
										Args1 = "/*" + fun.FunctionTypeArg1 + " " + fun.FunctionNameArg1 + "*/";
									}
									if(fun.FunctionNameArg2 != "")
									{
										Args2 = ",/*" + fun.FunctionTypeArg2 + " " + fun.FunctionNameArg2 + "*/";
									}
									if(fun.FunctionNameArg3 != "")
									{
										Args3 = ",/*" + fun.FunctionTypeArg3 + " " + fun.FunctionNameArg3 + "*/";
									}
									if(fun.FunctionNameArg4 != "")
									{
										Args4 = ",/*" + fun.FunctionTypeArg4 + " " + fun.FunctionNameArg4 + "*/";
									}
									if(fun.FunctionNameArg5 != "")
									{
										Args5 = ",/*" + fun.FunctionTypeArg5 + " " + fun.FunctionNameArg5 + "*/";
									}
									if(fun.FunctionNameArg6 != "")
									{
										Args6 = ",/*" + fun.FunctionTypeArg6 + " " + fun.FunctionNameArg6 + "*/";
									}													
								
									//TextEditorScenario.TooltipText = fun.FunctionName + "(" + Args1 + Args2 + Args3 + Args4 + Args5 + Args6 + ")";									
									
									IdParentColumn2Scenario = AddOneElementInLMTES(2,IdParentColumn1Scenario,true,fun.FunctionName.Replace("_","-"),fun.FunctionName + "(" + Args1 + Args2 + Args3 + Args4 + Args5 + Args6 + ");","");
								}
							}
						}
					}
				}
			}			
			
		}
		
		public int AddOneElementInLMTES(int Column, int IdParent, bool Active, string Name, string Text, string ToolTip)
		{
			ListMenuTextEditorScenario.Add (new MenuTextEditorScenario(Column,CountIdMenuScenarioEdit,IdParent,Active,Name,Text,ToolTip));
			CountIdMenuScenarioEdit++;
			return CountIdMenuScenarioEdit-1;
		}
	
//################Fonction permettant d'ajouter des Elements dans la liste du menu dans le TextEditor Function #######	
		
		public void AddElementInListMenuTextEditorFunction(Int32 _Node_Id, Int32 _ScenarioId, Int32 _FunctionId)
		{
			ListMenuTextEditorFunction.Clear();
			CountIdMenuFunctionEdit = 1;		
			IdParentColumn1Function = 0;
			IdParentColumn2Function = 0;
			
			IdParentColumn1Function = AddOneElementInLMTEF(1,0,false,param.ParamT("CE_ItemMenuStandardCPP"),"","");
				IdParentColumn2Function = AddOneElementInLMTEF(2,IdParentColumn1Function,false,param.ParamT("CE_VariableType"),"","");
					AddOneElementInLMTEF(3,IdParentColumn2Function,true,"bool","bool","");
					AddOneElementInLMTEF(3,IdParentColumn2Function,true,"byte","byte","");
					AddOneElementInLMTEF(3,IdParentColumn2Function,true,"uint-8","uint_8","");
					AddOneElementInLMTEF(3,IdParentColumn2Function,true,"uint-16","uint_16","");	
				IdParentColumn2Function = AddOneElementInLMTEF(2,IdParentColumn1Function,false,param.ParamT("CE_LoopAndCondition"),"","");
					AddOneElementInLMTEF(3,IdParentColumn2Function,true,"if","if()\n{\n\n}","");
					AddOneElementInLMTEF(3,IdParentColumn2Function,true,"if else","if()\n{\n\n}\nelse\n{\n\n}","");	
					AddOneElementInLMTEF(3,IdParentColumn2Function,true,"if else if","if()\n{\n\n}\nelse if()\n{\n\n}","");	
				IdParentColumn2Function = AddOneElementInLMTEF(2,IdParentColumn1Function,false,param.ParamT("CE_Gate"),"","");
					AddOneElementInLMTEF(3,IdParentColumn2Function,true,"and","&&","");
					AddOneElementInLMTEF(3,IdParentColumn2Function,true,"or","||","");
					AddOneElementInLMTEF(3,IdParentColumn2Function,true,"xor","^","");
			
				bool AddSeparatorLineOneWire = false;
				foreach(Project Pro in ListProject) //Dans la liste des projets
				{
					foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
					{
						if(node.Node_Id == _Node_Id)
						{						
							foreach(Network net in node.ReturnListNetwork())
							{
								foreach(Board boa in net.ReturnListBoard())
								{
									if(boa.Board_Type == "TEMPERATURE")
									{
										if(!AddSeparatorLineOneWire)
										{
											AddOneElementInLMTEF(1,0,false,"SeparatorMenuItem","","");
												IdParentColumn1Function = AddOneElementInLMTEF(1,0,false,param.ParamT("AP_TEMPName"),"","");
											AddSeparatorLineOneWire = true;
										}
											IdParentColumn2Function = AddOneElementInLMTEF(2,IdParentColumn1Function,false,boa.Board_Name.Replace("_","-"),"","");
												AddOneElementInLMTEF(3,IdParentColumn2Function,true,param.ParamT("AP_TEMPGet"), pref.TEMPGetValue + "( " + boa.Board_Name + " );", pref.TEMPGetValue + "( " + boa.Board_Name + " )");
									}
								}
							}
						}
					}
				}			
			
			AddOneElementInLMTEF(1,0,false,"SeparatorMenuItem","","");
						
				bool LihgtingOK = false;
				bool SwitchOK = false;
				bool ShutterOK = false;
			
				foreach(Project Pro in ListProject) //Dans la liste des projets
				{
					foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
					{
						if(node.Node_Id == _Node_Id)
						{		
							foreach(Instance ins in node.ReturnListInstance())
							{
								if(ins.Instance_Type == param.ParamP("InstLightingName"))
								{
									LihgtingOK = true;
								}
								if(ins.Instance_Type == param.ParamP("InstSwitchName"))
								{
									SwitchOK = true;
								}	
								if(ins.Instance_Type == param.ParamP("InstShutterName"))
								{
									ShutterOK = true;
								}							
							}
						
							if(SwitchOK)
							{
								IdParentColumn1Function = AddOneElementInLMTEF(1,0,false,param.ParamT("ExTVNameSwitch"),"","");
								foreach(Instance ins in node.ReturnListInstance())
								{							
									if(ins.Instance_Type == param.ParamP("InstSwitchName"))
									{
										IdParentColumn2Function = AddOneElementInLMTEF(2,IdParentColumn1Function,false,ins.Instance_Name.Replace("_","-"),"","");
											AddOneElementInLMTEF(3,IdParentColumn2Function,true,param.ParamT("AP_SWIPulse"),pref.SWIClicActionName + "( " + ins.Instance_Name + " );",pref.SWIClicActionName + "( " + ins.Instance_Name + " )");
											AddOneElementInLMTEF(3,IdParentColumn2Function,true,param.ParamT("AP_SWIDPulse"),pref.SWIDoubleClicActionName + "( " + ins.Instance_Name + " );",pref.SWIDoubleClicActionName + "( " + ins.Instance_Name + " )");
											AddOneElementInLMTEF(3,IdParentColumn2Function,true,param.ParamT("AP_SWIOn"),pref.SWIOnActionName + "( " + ins.Instance_Name + " );",pref.SWIOnActionName + "( " + ins.Instance_Name + " )");	
											AddOneElementInLMTEF(3,IdParentColumn2Function,true,param.ParamT("AP_SWIOnRe"),pref.SWIOnFmActionName + "( " + ins.Instance_Name + " );",pref.SWIOnFmActionName + "( " + ins.Instance_Name + " )");	
											AddOneElementInLMTEF(3,IdParentColumn2Function,true,param.ParamT("AP_SWIOff"),pref.SWIOffActionName + "( " + ins.Instance_Name + " );",pref.SWIOffActionName + "( " + ins.Instance_Name + " )");	
											AddOneElementInLMTEF(3,IdParentColumn2Function,true,param.ParamT("AP_SWIOffFe"),pref.SWIOffFmActionName + "( " + ins.Instance_Name + " );",pref.SWIOffFmActionName + "( " + ins.Instance_Name + " )");	
										
									}			
								}
							}
							if(LihgtingOK)
							{
								IdParentColumn1Function = AddOneElementInLMTEF(1,0,false,param.ParamT("ExTVNameLighting"),"","");
								foreach(Instance ins in node.ReturnListInstance())
								{							
									if(ins.Instance_Type == param.ParamP("InstLightingName"))
									{
										IdParentColumn2Function = AddOneElementInLMTEF(2,IdParentColumn1Function,false,ins.Instance_Name.Replace("_","-"),"","");
											AddOneElementInLMTEF(3,IdParentColumn2Function,true,param.ParamT("AP_LIGToggle"), pref.LIGToggleActionName + "( " + ins.Instance_Name + " );", pref.LIGToggleActionName + "( " + ins.Instance_Name + " )");
											AddOneElementInLMTEF(3,IdParentColumn2Function,true,param.ParamT("AP_LIGTune"), pref.LIGTuneActionName + "( " + ins.Instance_Name + " );", pref.LIGTuneActionName + "( " + ins.Instance_Name + " )");
											AddOneElementInLMTEF(3,IdParentColumn2Function,true,param.ParamT("AP_LIGStop"), pref.LIGStopActionName + "( " + ins.Instance_Name + " );", pref.LIGStopActionName + "( " + ins.Instance_Name + " )");
											AddOneElementInLMTES(3,IdParentColumn2Scenario,true,param.ParamT("AP_LIGSet"), pref.LIGSetActionName + "( " + ins.Instance_Name + " ,/*value*/);", pref.LIGStopActionName + "( " + ins.Instance_Name + " ,/*value*/)");									
									}	
								}
							}					  
							if(ShutterOK)
							{
								IdParentColumn1Function = AddOneElementInLMTEF(1,0,false,param.ParamT("ExTVNameShutter"),"","");
								foreach(Instance ins in node.ReturnListInstance())
								{								
									if(ins.Instance_Type == param.ParamP("InstShutterName"))
									{
										IdParentColumn2Function = AddOneElementInLMTEF(2,IdParentColumn1Function,false,ins.Instance_Name.Replace("_","-"),"","");
											AddOneElementInLMTEF(3,IdParentColumn2Function,true,param.ParamT("AP_SHUOpen"),pref.SHUOpenActionName + "( " + ins.Instance_Name + " );",pref.SHUOpenActionName + "( " + ins.Instance_Name + " )");
											AddOneElementInLMTEF(3,IdParentColumn2Function,true,param.ParamT("AP_SHUClose"),pref.SHUCloseActionName + "( " + ins.Instance_Name + " );",pref.SHUCloseActionName + "( " + ins.Instance_Name + " )");
											AddOneElementInLMTEF(3,IdParentColumn2Function,true,param.ParamT("AP_SHUStop"),pref.SHUStopActionName + "( " + ins.Instance_Name + " );",pref.SHUStopActionName + "( " + ins.Instance_Name + " )");									
											AddOneElementInLMTES(3,IdParentColumn2Scenario,true,param.ParamT("AP_SHUToggle"),pref.SHUToggleActionName + "( " + ins.Instance_Name + " );",pref.SHUToggleActionName + "( " + ins.Instance_Name + " )");
									}
								}
							}					
							
						}
					}
				}	
			
			AddOneElementInLMTEF(1,0,false,"SeparatorMenuItem","","");							
			
			foreach(Project Pro in ListProject) //Dans la liste des projets
			{
				foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
				{
					foreach(Scenario sce in node.ReturnListScenario())
					{
						if(sce.ScenarioId == _ScenarioId)
						{

							IdParentColumn1Function = AddOneElementInLMTEF(1,0,!(sce.ReturnListVariable().Count > 0),param.ParamT("CRM_Variable"),"","");
							
							foreach(Variable vari in sce.ReturnListVariable())
							{
								IdParentColumn2Function = AddOneElementInLMTEF(2,IdParentColumn1Function,true,vari.VariableName.Replace("_","-"),vari.VariableName,"");
							}
							
							foreach(Function fun in sce.ReturnListFunction())
							{
								if(fun.FunctionId == _FunctionId)
								{
									IdParentColumn1Function = AddOneElementInLMTEF(1,0,!(fun.FunctionNameArg1 != ""),param.ParamT("CRM_Argument"),"","");		
									if(fun.FunctionNameArg1 != "")
									{								
										IdParentColumn2Function = AddOneElementInLMTEF(2,IdParentColumn1Function,true,fun.FunctionNameArg1.Replace("_","-"),fun.FunctionNameArg1.Replace("_","-"),"");
									}
									if(fun.FunctionNameArg2 != "")
									{
										IdParentColumn2Function = AddOneElementInLMTEF(2,IdParentColumn1Function,true,fun.FunctionNameArg2.Replace("_","-"),fun.FunctionNameArg2.Replace("_","-"),"");
									}
									if(fun.FunctionNameArg3 != "")
									{
										IdParentColumn2Function = AddOneElementInLMTEF(2,IdParentColumn1Function,true,fun.FunctionNameArg3.Replace("_","-"),fun.FunctionNameArg3.Replace("_","-"),"");
									}
									if(fun.FunctionNameArg4 != "")
									{
										IdParentColumn2Function = AddOneElementInLMTEF(2,IdParentColumn1Function,true,fun.FunctionNameArg4.Replace("_","-"),fun.FunctionNameArg4.Replace("_","-"),"");
									}
									if(fun.FunctionNameArg5 != "")
									{
										IdParentColumn2Function = AddOneElementInLMTEF(2,IdParentColumn1Function,true,fun.FunctionNameArg5.Replace("_","-"),fun.FunctionNameArg5.Replace("_","-"),"");
									}
									if(fun.FunctionNameArg6 != "")
									{
										IdParentColumn2Function = AddOneElementInLMTEF(2,IdParentColumn1Function,true,fun.FunctionNameArg6.Replace("_","-"),fun.FunctionNameArg6.Replace("_","-"),"");
									}	
								}
							}
						}
					}
				}
			}
		}
		
		public int AddOneElementInLMTEF(int Column, int IdParent, bool Active, string Name, string Text, string ToolTip)
		{
			ListMenuTextEditorFunction.Add (new MenuTextEditorFunction(Column,CountIdMenuFunctionEdit,IdParent,Active,Name,Text,ToolTip));
			CountIdMenuFunctionEdit++;
			return CountIdMenuFunctionEdit-1;
		}		
		
//################Fonction permettant de faire du renommage #####################################		
		
		//Fonction ReturnNewName
		//Fonction permettant de retourner un nouveaux nom si celui-ci existe
		//Argument :
		//	string _ProjectName : Nom à vérifier
		//Return :
		//	Retourne le nouveau nom
		public string ReturnNewNameProject(string _ProjectName)
		{
			Int32 Count = 0;	//On initialise un compteur
			bool NameOK = true; //NameOK est un booléan nous retournant si le nom existe
			bool FlagOK = true; //Flag permettant de sortir d'une boucle
				
			foreach(Project Pro in ListProject) //Pour chaque projet de la liste
			{			
				if(Pro.Project_Name == _ProjectName) //Dans la liste des projet on verifie que le nom existe
				{
					NameOK = false; //Si oui, on remet NameOK à faux
				}
			}			
			
			if(!NameOK) //Si le nom existe
			{
				while (!NameOK) //Tant que le nom existe
	        	{
					FlagOK = true; //A chaque tour on met le flag à true
					foreach(Project Pro in ListProject)
					{			
						if(Pro.Project_Name == _ProjectName + Count) //On recherche un nouveau nom et on verifie si il existe
						{
							FlagOK = false; //Si le nouveau nom existe, on remet le flag en bas
							Count++; //On incremente le compteur du nouveau nom
						}
					}
					
					if(FlagOK) //si le nouveau nom existe pas 
					{
						_ProjectName = _ProjectName + Count; //on le renomme
						NameOK=true; //on met NameOK à vrai permettant de sortir de la boucle
					}
				}
			}
			
			return _ProjectName; //on retourne le nom du projet
		}
		
		//Fonction ReturnNewNameNode
		//Fonction permettant de retourner un nouveau nom de noeud dans le cas où celui-ci existe
		//Argments :
		//	string _NodeName : Nom du noeud
		//	Int32 _IdProject : Id du projet
		//Retour : le nouveau nom
		public string ReturnNewNameNode(string _NodeName, Int32 _Project_Id)
		{
			Int32 Count = 0;	//On initialise un compteur
			bool NameOK = true; //NameOK est un booléan nous retournant si le nom existe
			bool FlagOK = true; //Flag permettant de sortir d'une boucle
				
			foreach(Project Pro in ListProject) //Pour chaque projet de la liste
			{	
				if(Pro.Project_Id == _Project_Id) //On cherche celui qui a l'id mis en paramètre
				{
					foreach(Node Nod in Pro.ReturnListNode()) //On boucle sur les noeuds
					{
						if(Nod.Node_Name == _NodeName) // on recherche si il existe un noeud avec le meme nom
						{
							NameOK = false; //Dans la liste des projet on verifie que le nom existe
						}
					}
				}
			}			
			
			if(!NameOK) //Si le nom existe
			{
				while (!NameOK) //Tant que le nom existe
	        	{
					FlagOK = true; //A chaque tour on met le flag à true
					foreach(Project Pro in ListProject)	//Pour chaque projet de la liste
					{		
						if(Pro.Project_Id == _Project_Id) //On cherche celui qui a l'id mis en paramètre
						{						
							foreach(Node Nod in Pro.ReturnListNode())	//On boucle sur les noeuds
							{						
								if(Nod.Node_Name == _NodeName + Count) //On recherche un nouveau nom et on verifie si il existe
								{
									FlagOK = false; //Si le nouveau nom existe, on remet le flag en bas
									Count++; //On incremente le compteur du nouveau nom
								}
							}
						}
					}
					
					if(FlagOK) //si le nouveau nom existe pas 
					{
						_NodeName = _NodeName + Count; //on le renomme
						NameOK=true; //on met NameOK à vrai permettant de sortir de la boucle
					}
				}
			}
			
			return _NodeName; //on retourne le nom du projet		
		}
		
		//Fonction ReturnNewNameBoard
		//Fonction permettant de retourner un nouveau nom de carte dans le cas où celui-ci existe
		//Argments :	
		//	string _BoardName : Nom de la carte à verifier
		//	Int32 _NetworkId : Id du réseau
		//Retour :
		//	string : le nouveau nom
		public string ReturnNewNameBoard(string _BoardName, Int32 _NetworkId)
		{
			Int32 Count = 0;	//On initialise un compteur
			bool NameOK = true; //NameOK est un booléan nous retournant si le nom existe
			bool FlagOK = true; //Flag permettant de sortir d'une boucle
				
			foreach(Project Pro in ListProject) //Pour chaque projet de la liste
			{	
				foreach(Node Nod in Pro.ReturnListNode()) //On boucle sur les noeuds
				{
					foreach(Network Net in Nod.ReturnListNetwork())
					{
						if(Net.Network_Id == _NetworkId) // on recherche si il existe un noeud avec le meme nom
						{
							foreach(Board board in Net.ReturnListBoard()) //On boucle sur la liste des cartes
							{
								if(board.Board_Name == _BoardName) //On regarde si une carte porte le meme nom
								{
									NameOK = false; //Dans la liste des projet on verifie que le nom existe
								}
							}
						}
					}
				}
			}			
			
			if(!NameOK) //Si le nom existe
			{
				while (!NameOK) //Tant que le nom existe
	        	{
					FlagOK = true; //A chaque tour on met le flag à true
					foreach(Project Pro in ListProject) //Pour chaque projet de la liste
					{	
						foreach(Node Nod in Pro.ReturnListNode()) //On boucle sur les noeuds
						{
							foreach(Network Net in Nod.ReturnListNetwork()) //On boucle sur les réseau
							{
								if(Net.Network_Id == _NetworkId) // on recherche si il existe un noeud avec le meme id
								{
									foreach(Board board in Net.ReturnListBoard()) //On boucle sur les cartes
									{
										if(board.Board_Name == _BoardName + Count)
										{					
											FlagOK = false; //Si le nouveau nom existe, on remet le flag en bas
											Count++; //On incremente le compteur du nouveau nom
										}
									}
								}
							}
						}
					}
					
					if(FlagOK) //si le nouveau nom existe pas 
					{
						_BoardName = _BoardName + Count; //on le renomme
						NameOK=true; //on met NameOK à vrai permettant de sortir de la boucle
					}
				}
			}
			
			return _BoardName; //on retourne le nom du projet				
		}
		
		//Fonction ReturnNewNameInstance
		//Fonction permettant de retourner un nouveau non d'instance dans le cas où celui-ci existe
		public string ReturnNewNameInstance(string _InstanceName, Int32 _Node_Id)
		{
			Int32 Count = 0;	//On initialise un compteur
			bool NameOK = true; //NameOK est un booléan nous retournant si le nom existe
			bool FlagOK = true; //Flag permettant de sortir d'une boucle
				
			foreach(Project Pro in ListProject) //Pour chaque projet de la liste
			{	
				foreach(Node Nod in Pro.ReturnListNode()) //On boucle sur les noeuds
				{
					if(Nod.Node_Id == _Node_Id)
					{
						foreach(Instance ins in Nod.ReturnListInstance())
						{
							if(ins.Instance_Name == _InstanceName)
							{
								NameOK = false; //Dans la liste des projet on verifie que le nom existe
							}
						}
					}
				}
			}			
			
			if(!NameOK) //Si le nom existe
			{
				while (!NameOK) //Tant que le nom existe
	        	{
					FlagOK = true; //A chaque tour on met le flag à true
					foreach(Project Pro in ListProject) //Pour chaque projet de la liste
					{	
						foreach(Node Nod in Pro.ReturnListNode()) //On boucle sur les noeuds
						{
							foreach(Instance ins in Nod.ReturnListInstance())
							{
								if(ins.Instance_Name == _InstanceName + Count)
								{
									FlagOK = false; //Si le nouveau nom existe, on remet le flag en bas
									Count++; //On incremente le compteur du nouveau nom
								}
							}							
						}
					}
					
					if(FlagOK) //si le nouveau nom existe pas 
					{
						_InstanceName = _InstanceName + Count; //on le renomme
						NameOK=true; //on met NameOK à vrai permettant de sortir de la boucle
					}
				}
			}
			
			return _InstanceName; //on retourne le nom du projet					
		}
		
		//Fonction ReturnNewNameCustomer
		//Fonction permettant de retourner nouveau nom pour un fichier customer
		public string ReturnNewNameCustomer(string _CustomerName, Int32 _Node_Id)
		{
			int SizeString =  _CustomerName.Length;
			Int32 Count = 0;	//On initialise un compteur
			bool NameOK = true; //NameOK est un booléan nous retournant si le nom existe
			bool FlagOK = true; //Flag permettant de sortir d'une boucle			
			
			if(_CustomerName.Substring(SizeString - 4,4) != ".ino")
			{
				_CustomerName = _CustomerName.Replace(".","_");
				_CustomerName = _CustomerName.Replace(" ","_");
				_CustomerName = _CustomerName + ".ino";
			}
			else
			{
				_CustomerName = _CustomerName.Substring(0,SizeString - 4);
				_CustomerName = _CustomerName.Replace(".","_");
				_CustomerName = _CustomerName.Replace(" ","_");
				_CustomerName = _CustomerName + ".ino";
			}	
		
			SizeString =  _CustomerName.Length;
			
			foreach(Project Pro in ListProject) //Pour chaque projet de la liste
			{	
				foreach(Node Nod in Pro.ReturnListNode()) //On boucle sur les noeuds
				{
					if(Nod.Node_Id == _Node_Id)
					{
						foreach(Customer cus in Nod.ReturnListCustomer())
						{
							if(cus.CustomerName == _CustomerName)
							{
								NameOK = false; //Dans la liste des projet on verifie que le nom existe
							}
						}
					}
				}
			}			
			
			if(!NameOK) //Si le nom existe
			{
				while (!NameOK) //Tant que le nom existe
	        	{
					FlagOK = true; //A chaque tour on met le flag à true
					foreach(Project Pro in ListProject) //Pour chaque projet de la liste
					{	
						foreach(Node Nod in Pro.ReturnListNode()) //On boucle sur les noeuds
						{
							foreach(Customer cus in Nod.ReturnListCustomer())
							{
								if(cus.CustomerName == _CustomerName.Substring(0,SizeString - 4) + "_" + Count + ".ino")
								{
									FlagOK = false; //Si le nouveau nom existe, on remet le flag en bas
									Count++; //On incremente le compteur du nouveau nom
								}
							}							
						}
					}
					
					if(FlagOK) //si le nouveau nom existe pas 
					{
						_CustomerName = _CustomerName.Substring(0,SizeString - 4) + "_" + Count + ".ino"; //on le renomme
						NameOK=true; //on met NameOK à vrai permettant de sortir de la boucle
					}
				}
			}
			
			return _CustomerName; //on retourne le nom du projet				
		}

		//Fonction ReturnNewNameCustomer
		//Fonction permettant de retourner nouveau nom pour un fichier customer
		public string ReturnNewNameScenario(string _ScenarioName, Int32 _Node_Id)
		{
			Int32 Count = 0;	//On initialise un compteur
			bool NameOK = true; //NameOK est un booléan nous retournant si le nom existe
			bool FlagOK = true; //Flag permettant de sortir d'une boucle			
					
			foreach(Project Pro in ListProject) //Pour chaque projet de la liste
			{	
				foreach(Node Nod in Pro.ReturnListNode()) //On boucle sur les noeuds
				{
					if(Nod.Node_Id == _Node_Id)
					{
						foreach(Scenario sce in Nod.ReturnListScenario())
						{
							if(sce.ScenarioName == _ScenarioName)
							{
								NameOK = false; //Dans la liste des projet on verifie que le nom existe
							}
						}
					}
				}
			}			
			
			if(!NameOK) //Si le nom existe
			{
				while (!NameOK) //Tant que le nom existe
	        	{
					FlagOK = true; //A chaque tour on met le flag à true
					foreach(Project Pro in ListProject) //Pour chaque projet de la liste
					{	
						foreach(Node Nod in Pro.ReturnListNode()) //On boucle sur les noeuds
						{
							foreach(Scenario sce in Nod.ReturnListScenario())
							{
								if(sce.ScenarioName == _ScenarioName + "_" + Count)
								{
									FlagOK = false; //Si le nouveau nom existe, on remet le flag en bas
									Count++; //On incremente le compteur du nouveau nom
								}
							}							
						}
					}
					
					if(FlagOK) //si le nouveau nom existe pas 
					{
						_ScenarioName = _ScenarioName + "_" + Count; //on le renomme
						NameOK=true; //on met NameOK à vrai permettant de sortir de la boucle
					}
				}
			}
			
			return _ScenarioName.Replace(" ","_"); //on retourne le nom du projet				
		}
		
		//Fonction ReturnNewNameVariable 
		//Fonction permettant de retourner un nouveau nom de variable
		public string ReturnNewNameVariable(string _VariableName, Int32 _ScenarioId)
		{
			Int32 Count = 0;	//On initialise un compteur
			bool NameOK = true; //NameOK est un booléan nous retournant si le nom existe
			bool FlagOK = true; //Flag permettant de sortir d'une boucle			
					
			foreach(Project Pro in ListProject) //Pour chaque projet de la liste
			{	
				foreach(Node Nod in Pro.ReturnListNode()) //On boucle sur les noeuds
				{
					foreach(Scenario sce in Nod.ReturnListScenario())
					{
						if(sce.ScenarioId == _ScenarioId)
						{
							foreach(Variable vari in sce.ReturnListVariable())
							{
								if(vari.VariableName == _VariableName)
								{
									NameOK = false; //Dans la liste des projet on verifie que le nom existe
								}
							}
						}
					}
				}
			}			
			
			if(!NameOK) //Si le nom existe
			{
				while (!NameOK) //Tant que le nom existe
	        	{
					FlagOK = true; //A chaque tour on met le flag à true
					foreach(Project Pro in ListProject) //Pour chaque projet de la liste
					{	
						foreach(Node Nod in Pro.ReturnListNode()) //On boucle sur les noeuds
						{
							foreach(Scenario sce in Nod.ReturnListScenario())
							{
								foreach(Variable vari in sce.ReturnListVariable())
								{
									if(vari.VariableName == _VariableName + Count)
									{
										FlagOK = false; //Si le nouveau nom existe, on remet le flag en bas
										Count++; //On incremente le compteur du nouveau nom
									}
								}
							}							
						}
					}
					
					if(FlagOK) //si le nouveau nom existe pas 
					{
						_VariableName = _VariableName + Count; //on le renomme
						NameOK=true; //on met NameOK à vrai permettant de sortir de la boucle
					}
				}
			}
			
			return _VariableName.Replace(" ","_"); //on retourne le nom du projet	
		}
		
		//Fonction ReturnNewNameFunction
		//Fonction permettant de retourner un nouveau nom de fonction
		public string ReturnNewNameFunction(string _FunctionName, Int32 _ScenarioId)
		{
			Int32 Count = 0;	//On initialise un compteur
			bool NameOK = true; //NameOK est un booléan nous retournant si le nom existe
			bool FlagOK = true; //Flag permettant de sortir d'une boucle			
					
			foreach(Project Pro in ListProject) //Pour chaque projet de la liste
			{	
				foreach(Node Nod in Pro.ReturnListNode()) //On boucle sur les noeuds
				{
					foreach(Scenario sce in Nod.ReturnListScenario())
					{
						if(sce.ScenarioId == _ScenarioId)
						{
							foreach(Function fun in sce.ReturnListFunction())
							{
								if(fun.FunctionName == _FunctionName)
								{
									NameOK = false; //Dans la liste des projet on verifie que le nom existe
								}
							}
						}
					}
				}
			}			
			
			if(!NameOK) //Si le nom existe
			{
				while (!NameOK) //Tant que le nom existe
	        	{
					FlagOK = true; //A chaque tour on met le flag à true
					foreach(Project Pro in ListProject) //Pour chaque projet de la liste
					{	
						foreach(Node Nod in Pro.ReturnListNode()) //On boucle sur les noeuds
						{
							foreach(Scenario sce in Nod.ReturnListScenario())
							{
								foreach(Function fun in sce.ReturnListFunction())
								{
									if(fun.FunctionName == _FunctionName + Count)
									{
										FlagOK = false; //Si le nouveau nom existe, on remet le flag en bas
										Count++; //On incremente le compteur du nouveau nom
									}
								}
							}							
						}
					}
					
					if(FlagOK) //si le nouveau nom existe pas 
					{
						_FunctionName = _FunctionName + Count; //on le renomme
						NameOK=true; //on met NameOK à vrai permettant de sortir de la boucle
					}
				}
			}
			
			return _FunctionName.Replace(" ","_"); //on retourne le nom du projet				
		}
		
		//Fonction ReturnCorrectName
		//Fonction permettant de retourner un nom correct
		public string ReturnCorrectName(string _Name)
		{
			for(int i=0; i<_Name.Length; i++)
			{
				if(_Name[i] < 48 || (_Name[i] > 57 && _Name[i] < 65) || (_Name[i] > 90 && _Name[i] < 97) || _Name[i] > 122)
				{
					_Name = _Name.Replace(_Name[i].ToString()," ");
				}
			}
			return _Name.Trim();
		}
		
//#################Fonction Divers#################################################################
		
		//Fonction AnalyseIP
		//Fonction permettant de savoir si une IP est bonne
		public bool AnalyseIP(string _Adress)
		{
			string[] StringByte = new string[10];
			string Byte0 = "";
			string Byte1 = "";
			string Byte2 = "";
			string Byte3 = "";
			
			string [] split = _Adress.Split(new Char [] {'.'});
			
			int i = 0;
			foreach (string s in split) 
			{
             	if (s.Trim() != "")
				{
					StringByte[i] = StringByte[i] + s;
				}
				i++;
			}
			
			if(i>4)
			{
				return false;
			}
			
			if(StringByte[0] == null || StringByte[1] == null || StringByte[2] == null || StringByte[3] == null)
			{
				return false;
			}
			else
			{
				Byte0 = StringByte[0];
				Byte1 = StringByte[1];
				Byte2 = StringByte[2];		
				Byte3 = StringByte[3];		
			}
			
			if(Byte0.Length > 3 || Byte1.Length > 3 || Byte2.Length > 3 || Byte3.Length > 3)
			{
				return false;
			}			
			
			for(i=1;i<=3;i++)
			{
				if(Byte0.Length == i)
				{
					for(int j=0;j<=i-1;j++)
					{
						if(Byte0[j] < 48 || Byte0[j] > 57)
						{
							return false;
						}
					}
				}
				if(Byte1.Length == i)
				{
					for(int j=0;j<=i-1;j++)
					{
						if(Byte1[j] < 48 || Byte1[j] > 57)
						{
							return false;
						}
					}
				}	
				if(Byte2.Length == i)
				{
					for(int j=0;j<=i-1;j++)
					{
						if(Byte2[j] < 48 || Byte2[j] > 57)
						{
							return false;
						}
					}
				}	
				if(Byte3.Length == i)
				{
					for(int j=0;j<=i-1;j++)
					{
						if(Byte3[j] < 48 || Byte3[j] > 57)
						{
							return false;
						}
					}
				}					
			}
			if(Convert.ToInt16(Byte0) > 255 || Convert.ToInt16(Byte1) > 255 || Convert.ToInt16(Byte2) > 255 || Convert.ToInt16(Byte3) > 255)
			{
				return false;
			}
			return true;
		}
		
		//Fonction Analyse MAC
		//Fcontion permettant de savoir si une adresse MAC est correct
		public bool AnalyseMac(string _Adress)
		{
			string[] StringByte = new string[10];
			string Byte0 = "";
			string Byte1 = "";
			string Byte2 = "";
			string Byte3 = "";
			string Byte4 = "";
			string Byte5 = "";
			string Byte6 = "";
			string Byte7 = "";
			
			string [] split = _Adress.Split(new Char [] {'-'});
			
			int i = 0;
			
			foreach (string s in split) 
			{
             	if (s.Trim() != "")
				{
					StringByte[i] = StringByte[i] + s;
				}
				i++;
			}
			
			if(i>8)
			{
				return false;
			}
			
			if(StringByte[0] == null || StringByte[1] == null || StringByte[2] == null || StringByte[3] == null || StringByte[4] == null || StringByte[5] == null || StringByte[6] == null || StringByte[7] == null)
			{
				return false;
			}
			else
			{
				Byte0 = StringByte[0];
				Byte1 = StringByte[1];
				Byte2 = StringByte[2];		
				Byte3 = StringByte[3];	
				Byte4 = StringByte[4];		
				Byte5 = StringByte[5];
				Byte6 = StringByte[6];		
				Byte7 = StringByte[7];					
				
			}
			
			if(Byte0.Length != 2 || Byte1.Length != 2 || Byte2.Length != 2 || Byte3.Length != 2 || Byte4.Length != 2 || Byte5.Length != 2 || Byte6.Length != 2 || Byte7.Length != 2)
			{
				return false;
			}			

			for(int j=0;j<=1;j++)
			{
				if(Byte0[j] > 57 && (Byte0[j] < 65 || Byte0[j] > 70))
				{
					return false;
				}
				if(Byte0[j] < 48)
				{
					return false;
				}	
				
				if(Byte1[j] > 57 && (Byte1[j] < 65 || Byte1[j] > 70))
				{
					return false;
				}
				if(Byte1[j] < 48)
				{
					return false;
				}		
				
				if(Byte2[j] > 57 && (Byte2[j] < 65 || Byte2[j] > 70))
				{
					return false;
				}
				if(Byte2[j] < 48)
				{
					return false;
				}	
				
				if(Byte3[j] > 57 && (Byte3[j] < 65 || Byte3[j] > 70))
				{
					return false;
				}
				if(Byte3[j] < 48)
				{
					return false;
				}	
				
				if(Byte4[j] > 57 && (Byte4[j] < 65 || Byte4[j] > 70))
				{
					return false;
				}
				if(Byte4[j] < 48)
				{
					return false;
				}						
				
				if(Byte5[j] > 57 && (Byte5[j] < 65 || Byte5[j] > 70))
				{
					return false;
				}
				if(Byte5[j] < 48)
				{
					return false;
				}	
				if(Byte6[j] > 57 && (Byte6[j] < 65 || Byte6[j] > 70))
				{
					return false;
				}
				if(Byte6[j] < 48)
				{
					return false;
				}
				if(Byte7[j] > 57 && (Byte7[j] < 65 || Byte7[j] > 70))
				{
					return false;
				}
				if(Byte7[j] < 48)
				{
					return false;
				}				
			}
			return true;
		}
		
//#################Fonction Copy##################################################################
		
		//Fonction CopyProject
		//Fonction permettant de copier la liste des projet courant dans la liste des projets copiés
		//Les attribut de cette fonction sont les texte que nous pourrons voir dans l'arbre
		public void CopyProject(string _TextParam, string _TextExt)
		{
			int i = 0;
			int j = 0;
			int k = 0;
			int l = 0;
			int m = 0;
			int n = 0;
			int o = 0;
			int p = 0;
			int q = 0;
			int r = 0;
			int s = 0;
			
			if(ViewCopy == CountCopy - 1 || ViewCopy == CountCopy)
			{
				if(CountCopy!=pref.NumberOfProjectCopy)
				{
					i = 0;
					foreach(Project Pro in ListProject)
					{
						CopyListProject[CountCopy].Add(new Project());
						CopyProject(Pro,CopyListProject[CountCopy][i]);
						
						j = 0;
						foreach(Node nod in Pro.ReturnListNode())
						{
							CopyListProject[CountCopy][i].Node_.Add(new Node());
							CopyNode(nod,CopyListProject[CountCopy][i].Node_[j]);
							k = 0;
							foreach(Debug deb in nod.ReturnListDebug())
							{
								CopyListProject[CountCopy][i].Node_[j].Debug_.Add(new Debug());
								CopyDegub(deb,CopyListProject[CountCopy][i].Node_[j].Debug_[k]);
								k++;
							}
							l = 0;
							foreach(Instance ins in nod.ReturnListInstance())
							{
								CopyListProject[CountCopy][i].Node_[j].Instance_.Add(new Instance());
								CopyInstance(ins,CopyListProject[CountCopy][i].Node_[j].Instance_[l]);
								l++;
							}
							p = 0;
							foreach(Customer cus in nod.ReturnListCustomer())
							{
								CopyListProject[CountCopy][i].Node_[j].Customer_.Add(new Customer());
								CopyCustomer(cus,CopyListProject[CountCopy][i].Node_[j].Customer_[p]);
								p++;
							}
							q = 0;
							foreach(Scenario sce in nod.ReturnListScenario())
							{
								CopyListProject[CountCopy][i].Node_[j].Scenario_.Add(new Scenario());
								CopyScenario(sce,CopyListProject[CountCopy][i].Node_[j].Scenario_[q]);
								
								r = 0;
								foreach(Variable vari in sce.ReturnListVariable())
								{
									CopyListProject[CountCopy][i].Node_[j].Scenario_[q].Variable_.Add(new Variable());
									CopyVariable(vari,CopyListProject[CountCopy][i].Node_[j].Scenario_[q].Variable_[r]);
									r++;
								}
								
								s = 0;
								foreach(Function fun in sce.ReturnListFunction())
								{
									CopyListProject[CountCopy][i].Node_[j].Scenario_[q].Function_.Add(new Function());
									CopyFunction(fun,CopyListProject[CountCopy][i].Node_[j].Scenario_[q].Function_[s]);
									s++;
								}								
								
								q++;								
							}
							m = 0;
							foreach(Network net in nod.ReturnListNetwork())
							{
								CopyListProject[CountCopy][i].Node_[j].Network_.Add(new Network());
								CopyNetwork(net,CopyListProject[CountCopy][i].Node_[j].Network_[m]);
								
								n = 0;
								foreach(Board boa in net.ReturnListBoard())
								{
									CopyListProject[CountCopy][i].Node_[j].Network_[m].Board_.Add(new Board());
									CopyBoard(boa,CopyListProject[CountCopy][i].Node_[j].Network_[m].Board_[n]);
									
									o = 0;
									foreach(Pin pin in boa.ReturnListPin())
									{
										CopyListProject[CountCopy][i].Node_[j].Network_[m].Board_[n].Pin_.Add(new Pin());
										CopyPin(pin,CopyListProject[CountCopy][i].Node_[j].Network_[m].Board_[n].Pin_[o]);
										o++;
									}
									n++;
								}
								m++;
							}
							j++;
						}
						i++;
					}
				CountCopy ++;
				ViewCopy = CountCopy-1;
				mainwindow.UndoRedoInactif("undo",true);
				mainwindow.UndoRedoInactif("redo",false);
				mainwindow.ListHistoricTreeview.Add(new HistoricTreeView(ViewCopy,_TextParam,_TextExt,DateTime.Now));
				}
				else
				{
					
					for(i=0;i<pref.NumberOfProjectCopy-1;i++)
					{
						//On décale les texte d'une case vers la bas
						mainwindow.ListHistoricTreeview[i].HistoricTreeviewTextExt = mainwindow.ListHistoricTreeview[i+1].HistoricTreeviewTextExt;
						mainwindow.ListHistoricTreeview[i].HistoricTreeviewTextParam = mainwindow.ListHistoricTreeview[i+1].HistoricTreeviewTextParam;
						
						CopyListProject[i].Clear();
						foreach(Project Pro in CopyListProject[i+1])
						{					
							CopyListProject[i].Add(Pro);
						}
					}
					mainwindow.ListHistoricTreeview[pref.NumberOfProjectCopy-1].HistoricTreeviewTextExt = _TextExt;
					mainwindow.ListHistoricTreeview[pref.NumberOfProjectCopy-1].HistoricTreeviewTextParam = _TextParam;
					mainwindow.ListHistoricTreeview[pref.NumberOfProjectCopy-1].HistoricTreeviewDateAndTime = DateTime.Now;
					
					CopyListProject[CountCopy-1].Clear();
					i = 0;
					foreach(Project Pro in ListProject)
					{
						CopyListProject[CountCopy-1].Add(new Project());
						CopyProject(Pro,CopyListProject[CountCopy-1][i]);
						
						j = 0;
						foreach(Node nod in Pro.ReturnListNode())
						{
							CopyListProject[CountCopy-1][i].Node_.Add(new Node());
							CopyNode(nod,CopyListProject[CountCopy-1][i].Node_[j]);
							k = 0;
							foreach(Debug deb in nod.ReturnListDebug())
							{
								CopyListProject[CountCopy-1][i].Node_[j].Debug_.Add(new Debug());
								CopyDegub(deb,CopyListProject[CountCopy-1][i].Node_[j].Debug_[k]);
								k++;
							}
							l = 0;
							foreach(Instance ins in nod.ReturnListInstance())
							{
								CopyListProject[CountCopy-1][i].Node_[j].Instance_.Add(new Instance());
								CopyInstance(ins,CopyListProject[CountCopy-1][i].Node_[j].Instance_[l]);
								l++;
							}
							p = 0;
							foreach(Customer cus in nod.ReturnListCustomer())
							{
								CopyListProject[CountCopy-1][i].Node_[j].Customer_.Add(new Customer());
								CopyCustomer(cus,CopyListProject[CountCopy-1][i].Node_[j].Customer_[p]);
								p++;
							}		
							q = 0;
							foreach(Scenario sce in nod.ReturnListScenario())
							{
								CopyListProject[CountCopy-1][i].Node_[j].Scenario_.Add(new Scenario());
								CopyScenario(sce,CopyListProject[CountCopy-1][i].Node_[j].Scenario_[q]);
								
								r = 0;
								foreach(Variable vari in sce.ReturnListVariable())
								{
									CopyListProject[CountCopy-1][i].Node_[j].Scenario_[q].Variable_.Add(new Variable());
									CopyVariable(vari,CopyListProject[CountCopy-1][i].Node_[j].Scenario_[q].Variable_[r]);
									r++;
								}								
								
								s = 0;
								foreach(Function fun in sce.ReturnListFunction())
								{
									CopyListProject[CountCopy-1][i].Node_[j].Scenario_[q].Function_.Add(new Function());
									CopyFunction(fun,CopyListProject[CountCopy-1][i].Node_[j].Scenario_[q].Function_[s]);
									s++;
								}									
								
								q++;
							}							
							m = 0;
							foreach(Network net in nod.ReturnListNetwork())
							{
								CopyListProject[CountCopy-1][i].Node_[j].Network_.Add(new Network());
								CopyNetwork(net,CopyListProject[CountCopy-1][i].Node_[j].Network_[m]);
								
								n = 0;
								foreach(Board boa in net.ReturnListBoard())
								{
									CopyListProject[CountCopy-1][i].Node_[j].Network_[m].Board_.Add(new Board());
									CopyBoard(boa,CopyListProject[CountCopy-1][i].Node_[j].Network_[m].Board_[n]);
									
									o = 0;
									foreach(Pin pin in boa.ReturnListPin())
									{
										CopyListProject[CountCopy-1][i].Node_[j].Network_[m].Board_[n].Pin_.Add(new Pin());
										CopyPin(pin,CopyListProject[CountCopy-1][i].Node_[j].Network_[m].Board_[n].Pin_[o]);
										o++;
									}
									n++;
								}
								m++;
							}
							j++;
						}
						i++;
					}
				mainwindow.UndoRedoInactif("undo",true);
				mainwindow.UndoRedoInactif("redo",false);
				}
			}
			else if(ViewCopy < CountCopy - 1)
			{
				for(i=CountCopy-1;i>ViewCopy;i--)
				{
					CopyListProject[i].Clear();
					mainwindow.ListHistoricTreeview.RemoveAt(i);
				}
				CountCopy = ViewCopy+1;
				
					i = 0;
					foreach(Project Pro in ListProject)
					{
						CopyListProject[CountCopy].Add(new Project());
						CopyProject(Pro,CopyListProject[CountCopy][i]);
						
						j = 0;
						foreach(Node nod in Pro.ReturnListNode())
						{
							CopyListProject[CountCopy][i].Node_.Add(new Node());
							CopyNode(nod,CopyListProject[CountCopy][i].Node_[j]);
							k = 0;
							foreach(Debug deb in nod.ReturnListDebug())
							{
								CopyListProject[CountCopy][i].Node_[j].Debug_.Add(new Debug());
								CopyDegub(deb,CopyListProject[CountCopy][i].Node_[j].Debug_[k]);
								k++;
							}
							l = 0;
							foreach(Instance ins in nod.ReturnListInstance())
							{
								CopyListProject[CountCopy][i].Node_[j].Instance_.Add(new Instance());
								CopyInstance(ins,CopyListProject[CountCopy][i].Node_[j].Instance_[l]);
								l++;
							}
							p = 0;
							foreach(Customer cus in nod.ReturnListCustomer())
							{
								CopyListProject[CountCopy][i].Node_[j].Customer_.Add(new Customer());
								CopyCustomer(cus,CopyListProject[CountCopy][i].Node_[j].Customer_[p]);
								p++;
							}	
							q = 0;
							foreach(Scenario sce in nod.ReturnListScenario())
							{
								CopyListProject[CountCopy][i].Node_[j].Scenario_.Add(new Scenario());
								CopyScenario(sce,CopyListProject[CountCopy][i].Node_[j].Scenario_[q]);
							
								r = 0;
								foreach(Variable vari in sce.ReturnListVariable())
								{
									CopyListProject[CountCopy][i].Node_[j].Scenario_[q].Variable_.Add(new Variable());
									CopyVariable(vari,CopyListProject[CountCopy][i].Node_[j].Scenario_[q].Variable_[r]);
									r++;
								}	
							
								s = 0;
								foreach(Function fun in sce.ReturnListFunction())
								{
									CopyListProject[CountCopy][i].Node_[j].Scenario_[q].Function_.Add(new Function());
									CopyFunction(fun,CopyListProject[CountCopy][i].Node_[j].Scenario_[q].Function_[s]);
									s++;
								}							
								q++;
							}							
							m = 0;
							foreach(Network net in nod.ReturnListNetwork())
							{
								CopyListProject[CountCopy][i].Node_[j].Network_.Add(new Network());
								CopyNetwork(net,CopyListProject[CountCopy][i].Node_[j].Network_[m]);
								
								n = 0;
								foreach(Board boa in net.ReturnListBoard())
								{
									CopyListProject[CountCopy][i].Node_[j].Network_[m].Board_.Add(new Board());
									CopyBoard(boa,CopyListProject[CountCopy][i].Node_[j].Network_[m].Board_[n]);
									
									o = 0;
									foreach(Pin pin in boa.ReturnListPin())
									{
										CopyListProject[CountCopy][i].Node_[j].Network_[m].Board_[n].Pin_.Add(new Pin());
										CopyPin(pin,CopyListProject[CountCopy][i].Node_[j].Network_[m].Board_[n].Pin_[o]);
										o++;
									}
									n++;
								}
								m++;
							}
							j++;
						}
						i++;
					}
				CountCopy ++;
				ViewCopy = CountCopy-1;	
				mainwindow.ListHistoricTreeview.Add(new HistoricTreeView(ViewCopy,_TextParam,_TextExt,DateTime.Now));
			}
			mainwindow.UpdateHistoricTreeView();
			mainwindow.UpdateStatusBar();
		}
		
			public void CopyProject(Project Source, Project Destination)
			{
				Destination.Project_Id = Source.Project_Id;
				Destination.Project_Name = Source.Project_Name;
				Destination.Project_Author = Source.Project_Author;
				Destination.Project_CreationDateAndTime = Source.Project_CreationDateAndTime;
				Destination.Project_ModificationDateAndTime = Source.Project_ModificationDateAndTime;
				Destination.Project_Version = Source.Project_Version;	
				Destination.Project_SavePath = Source.Project_SavePath;
				Destination.Project_Note = Source.Project_Note;	
				Destination.param = Source.param;
				Destination.pref = Source.pref;
				Destination.Project_Password = Source.Project_Password;
				Destination.ProjectIsSave = Source.ProjectIsSave;			
			}
		
			public void CopyNode(Node Source, Node Destination)
			{
				Destination.Node_Id = Source.Node_Id;
				Destination.Node_Name = Source.Node_Name;
				Destination.Node_IP = Source.Node_IP;
				Destination.Node_GTWIP = Source.Node_GTWIP;
				Destination.Node_Mac = Source.Node_Mac;
				Destination.Node_WebServer = Source.Node_WebServer;
				Destination.Node_DHCP = Source.Node_DHCP;
				Destination.Node_Note = Source.Node_Note;
				Destination.Node_Type = Source.Node_Type;
				Destination.Node_Clock = Source.Node_Clock;
				Destination.Node_1Wire = Source.Node_1Wire;		
				Destination.param = Source.param;
				Destination.pref = Source.pref;
			}
		
			public void CopyDegub(Debug Source, Debug Destination)
			{
				Destination.Name = Source.Name;
				Destination.Value = Source.Value;
				Destination.FrenchDescriptionDebug = Source.FrenchDescriptionDebug;
				Destination.EnglishDescriptionDebug = Source.EnglishDescriptionDebug;			
			}
		
			public void CopyNetwork(Network Source, Network Destination)
		{
			Destination.Network_Id = Source.Network_Id;
			Destination.Network_Type = Source.Network_Type;	
			Destination.Network_Note = Source.Network_Note;			
		}
			
			public void CopyInstance(Instance Source, Instance Destination)
			{
				Destination.Instance_Id = Source.Instance_Id;
				Destination.Instance_Type = Source.Instance_Type;
				Destination.Instance_Name = Source.Instance_Name;
				Destination.Instance_Direction = Source.Instance_Direction;
				Destination.Instance_Note = Source.Instance_Note;
				Destination.Instance_Up_Down_Stop = Source.Instance_Up_Down_Stop;
				Destination.Instance_Used = Source.Instance_Used;
				Destination.Instance_Used_0 = Source.Instance_Used_0;
				Destination.Instance_Used_1 = Source.Instance_Used_1;
				Destination.Pin_Id_0 = Source.Pin_Id_0;
				Destination.Pin_Id_1 = Source.Pin_Id_1;
				Destination.Pin_Id_2 = Source.Pin_Id_2;
				Destination.Instance_LIG_DefaultValue = Source.Instance_LIG_DefaultValue;
				Destination.Instance_LIG_Fade = Source.Instance_LIG_Fade;	
				Destination.Instance_SWI_Inverse = Source.Instance_SWI_Inverse;
				Destination.Instance_SWI_ImpulsionTime = Source.Instance_SWI_ImpulsionTime;	
				Destination.Instance_SHU_InitTime = Source.Instance_SHU_InitTime;
				Destination.Instance_SHU_Type = Source.Instance_SHU_Type; 
				Destination.Instance_SHU_Time = Source.Instance_SHU_Time;
				Destination.Instance_SHU_NumberOfOutput = Source.Instance_SHU_NumberOfOutput;			
			}
		
			public void CopyBoard(Board Source, Board Destination)
			{
				Destination.Board_Id = Source.Board_Id;
				Destination.Board_Type = Source.Board_Type;
				Destination.Board_Name = Source.Board_Name;
				Destination.Board_I2C_0 = Source.Board_I2C_0;
				Destination.Board_I2C_1 = Source.Board_I2C_1;	
				Destination.Board_1Wire_Mac = Source.Board_1Wire_Mac;
				Destination.Board_1Wire_Precision = Source.Board_1Wire_Precision;
				Destination.Board_Note = Source.Board_Note;			
			}
			
			public void CopyPin(Pin Source, Pin Destination)
			{
				Destination.Pin_Id = Source.Pin_Id;
				Destination.Pin_Name = Source.Pin_Name;
				Destination.Pin_Direction = Source.Pin_Direction;
				Destination.Instance_Id = Source.Instance_Id;	
				Destination.Pin_FallbackValue = Source.Pin_FallbackValue;
			}
		
			public void CopyCustomer(Customer Source, Customer Destination)
			{
				Destination.CustomerId = Source.CustomerId;
				Destination.CustomerName = Source.CustomerName;
				Destination.CustomerData = Source.CustomerData;
				Destination.CustomerNote = Source.CustomerNote;
				Destination.CustomerUse = Source.CustomerUse;
				Destination.ScenarioId = Source.ScenarioId;
			}
		
			public void CopyScenario(Scenario Source, Scenario Destination)
			{
				Destination.ScenarioId = Source.ScenarioId;
				Destination.ScenarioName = Source.ScenarioName;
				Destination.ScenarioData = Source.ScenarioData;
				Destination.ScenarioNotes = Source.ScenarioNotes;
			}
		
			public void CopyVariable(Variable Source, Variable Destination)
			{
				Destination.VariableId = Source.VariableId;
				Destination.VariableName = Source.VariableName;
				Destination.VariableType = Source.VariableType;
				Destination.VariableDefaultValue = Source.VariableDefaultValue;
				Destination.VariableNote = Source.VariableNote;
			}
		
			public void CopyFunction(Function Source, Function Destination)
			{
				Destination.FunctionId = Source.FunctionId;
				Destination.FunctionName = Source.FunctionName;
				Destination.FunctionNote = Source.FunctionNote;
				Destination.FunctionData = Source.FunctionData;
				Destination.FunctionTypeArg1 = Source.FunctionTypeArg1;
				Destination.FunctionTypeArg2 = Source.FunctionTypeArg2;
				Destination.FunctionTypeArg3 = Source.FunctionTypeArg3;
				Destination.FunctionTypeArg4 = Source.FunctionTypeArg4;
				Destination.FunctionTypeArg5 = Source.FunctionTypeArg5;
				Destination.FunctionTypeArg6 = Source.FunctionTypeArg6;
				Destination.FunctionTypeReturn = Source.FunctionTypeReturn;
				Destination.FunctionNameArg1 = Source.FunctionNameArg1;
				Destination.FunctionNameArg2 = Source.FunctionNameArg2;
				Destination.FunctionNameArg3 = Source.FunctionNameArg3;
				Destination.FunctionNameArg4 = Source.FunctionNameArg4;
				Destination.FunctionNameArg5 = Source.FunctionNameArg5;
				Destination.FunctionNameArg6 = Source.FunctionNameArg6;
			}
		
		//Fonction UndoProject
		//Fonction permettant de charger les projets -1 (en faisant ctrl + z)
		public void UndoProject()
		{
			if(ViewCopy>=1)
			{
				ViewCopy--;
				ListProject.Clear ();
				
				int i=0;
				foreach(Project Pro in CopyListProject[ViewCopy])
				{
					ListProject.Add(new Project());
					CopyProject(Pro,ListProject[i]);
					
					int j = 0;
					foreach(Node nod in Pro.ReturnListNode())
					{
						ListProject[i].Node_.Add(new Node());
						CopyNode(nod,ListProject[i].Node_[j]);		
						int k = 0;
						foreach(Debug deb in nod.ReturnListDebug())
						{
							ListProject[i].Node_[j].Debug_.Add(new Debug());
							CopyDegub(deb,ListProject[i].Node_[j].Debug_[k]);
							k++;
						}
						int l = 0;
						foreach(Instance ins in nod.ReturnListInstance())
						{
							ListProject[i].Node_[j].Instance_.Add(new Instance());
							CopyInstance(ins,ListProject[i].Node_[j].Instance_[l]);
							l++;
						}
						int p = 0;
						foreach(Customer cus in nod.ReturnListCustomer())
						{
							ListProject[i].Node_[j].Customer_.Add(new Customer());
							CopyCustomer(cus,ListProject[i].Node_[j].Customer_[p]);
							p++;
						}	
						int q = 0;
						foreach(Scenario sce in nod.ReturnListScenario())
						{
							ListProject[i].Node_[j].Scenario_.Add(new Scenario());
							CopyScenario(sce,ListProject[i].Node_[j].Scenario_[q]);
							
								int r = 0;
								foreach(Variable vari in sce.ReturnListVariable())
								{
									ListProject[i].Node_[j].Scenario_[q].Variable_.Add(new Variable());
									CopyVariable(vari,ListProject[i].Node_[j].Scenario_[q].Variable_[r]);
									r++;
								}		
							
								int s = 0;
								foreach(Function fun in sce.ReturnListFunction())
								{
									ListProject[i].Node_[j].Scenario_[q].Function_.Add(new Function());
									CopyFunction(fun,ListProject[i].Node_[j].Scenario_[q].Function_[s]);
									s++;
								}							
							
							q++;
						}							
						int m = 0;
						foreach(Network net in nod.ReturnListNetwork())
						{
							ListProject[i].Node_[j].Network_.Add(new Network());
							CopyNetwork(net,ListProject[i].Node_[j].Network_[m]);
							
							int n = 0;
							foreach(Board boa in net.ReturnListBoard())
							{
								ListProject[i].Node_[j].Network_[m].Board_.Add(new Board());
								CopyBoard(boa,ListProject[i].Node_[j].Network_[m].Board_[n]);
								
								int o = 0;
								foreach(Pin pin in boa.ReturnListPin())
								{
									ListProject[i].Node_[j].Network_[m].Board_[n].Pin_.Add(new Pin());
									CopyPin(pin,ListProject[i].Node_[j].Network_[m].Board_[n].Pin_[o]);
									o++;
								}
								n++;
							}
							m++;
						}
						j++;
					}
					i++;						
					}
							
				UpdateInstanceUsed();
				mainwindow.UpdateMainNoteBook();
				mainwindow.UpdateWidgetInTab();
				mainwindow.UpdateMainWindow();
				mainwindow.RenameTooltipTextButtonOutput();
				mainwindow.UpdateOutputTreeview();		
				mainwindow.CloseTabInCaseOfDelete();
				mainwindow.UpdateStatusBar();
				if(ViewCopy>=1)
				{
					mainwindow.UndoRedoInactif("undo",true);
					mainwindow.UndoRedoInactif("redo",true);		
				}
				else
				{
					mainwindow.UndoRedoInactif("undo",false);
					mainwindow.UndoRedoInactif("redo",true);					
				}
				}
				mainwindow.UpdateHistoricTreeView();
			}

		//Fonction RedoProject
		//Fonction permettant de charger les projets +1 (en faisant ctrl + y)
		public void RedoProject()
		{
			if(ViewCopy < CountCopy - 1)
			{
				ViewCopy++;
				ListProject.Clear ();
				
				int i=0;
				foreach(Project Pro in CopyListProject[ViewCopy])
				{
					ListProject.Add(new Project());
					CopyProject(Pro,ListProject[i]);
					
					int j = 0;
					foreach(Node nod in Pro.ReturnListNode())
					{
						ListProject[i].Node_.Add(new Node());
						CopyNode(nod,ListProject[i].Node_[j]);		
						int k = 0;
						foreach(Debug deb in nod.ReturnListDebug())
						{
							ListProject[i].Node_[j].Debug_.Add(new Debug());
							CopyDegub(deb,ListProject[i].Node_[j].Debug_[k]);
							k++;
						}
						int l = 0;
						foreach(Instance ins in nod.ReturnListInstance())
						{
							ListProject[i].Node_[j].Instance_.Add(new Instance());
							CopyInstance(ins,ListProject[i].Node_[j].Instance_[l]);
							l++;
						}
						int p = 0;
						foreach(Customer cus in nod.ReturnListCustomer())
						{
							ListProject[i].Node_[j].Customer_.Add(new Customer());
							CopyCustomer(cus,ListProject[i].Node_[j].Customer_[p]);
							p++;
						}	
						int q = 0;
						foreach(Scenario sce in nod.ReturnListScenario())
						{
							ListProject[i].Node_[j].Scenario_.Add(new Scenario());
							CopyScenario(sce,ListProject[i].Node_[j].Scenario_[q]);
							
								int r = 0;
								foreach(Variable vari in sce.ReturnListVariable())
								{
									ListProject[i].Node_[j].Scenario_[q].Variable_.Add(new Variable());
									CopyVariable(vari,ListProject[i].Node_[j].Scenario_[q].Variable_[r]);
									r++;
								}	
							
								int s = 0;
								foreach(Function fun in sce.ReturnListFunction())
								{
									ListProject[i].Node_[j].Scenario_[q].Function_.Add(new Function());
									CopyFunction(fun,ListProject[i].Node_[j].Scenario_[q].Function_[s]);
									s++;
								}								
							
							q++;
						}							
						int m = 0;
						foreach(Network net in nod.ReturnListNetwork())
						{
							ListProject[i].Node_[j].Network_.Add(new Network());
							CopyNetwork(net,ListProject[i].Node_[j].Network_[m]);
							
							int n = 0;
							foreach(Board boa in net.ReturnListBoard())
							{
								ListProject[i].Node_[j].Network_[m].Board_.Add(new Board());
								CopyBoard(boa,ListProject[i].Node_[j].Network_[m].Board_[n]);
								
								int o = 0;
								foreach(Pin pin in boa.ReturnListPin())
								{
									ListProject[i].Node_[j].Network_[m].Board_[n].Pin_.Add(new Pin());
									CopyPin(pin,ListProject[i].Node_[j].Network_[m].Board_[n].Pin_[o]);
									o++;
								}
								n++;
							}
							m++;
						}
						j++;
					}
					i++;						
					
				UpdateInstanceUsed();
				mainwindow.UpdateMainNoteBook();
				mainwindow.UpdateWidgetInTab();
				mainwindow.UpdateMainWindow();
				mainwindow.RenameTooltipTextButtonOutput();
				mainwindow.UpdateOutputTreeview();		
				mainwindow.CloseTabInCaseOfDelete();	
				mainwindow.UpdateHistoricTreeView();
				mainwindow.UpdateStatusBar();
				
				if(ViewCopy < CountCopy - 1)
				{
					mainwindow.UndoRedoInactif("undo",true);
					mainwindow.UndoRedoInactif("redo",true);		
				}
				else
				{
					mainwindow.UndoRedoInactif("undo",true);
					mainwindow.UndoRedoInactif("redo",false);					
				}
				mainwindow.UpdateHistoricTreeView();
				}
			}
		}
		
		//Fonction ViewProject
		//Fonction permettant de charger un projet à partir d'un numéro de vu
		public void ViewProject(int _ViewCopy)
		{
			ListProject.Clear ();
			ViewCopy = _ViewCopy;
			int i=0;
			foreach(Project Pro in CopyListProject[ViewCopy])
			{
				ListProject.Add(new Project());
				CopyProject(Pro,ListProject[i]);
				
				int j = 0;
				foreach(Node nod in Pro.ReturnListNode())
				{
					ListProject[i].Node_.Add(new Node());
					CopyNode(nod,ListProject[i].Node_[j]);		
					int k = 0;
					foreach(Debug deb in nod.ReturnListDebug())
					{
						ListProject[i].Node_[j].Debug_.Add(new Debug());
						CopyDegub(deb,ListProject[i].Node_[j].Debug_[k]);
						k++;
					}
					int l = 0;
					foreach(Instance ins in nod.ReturnListInstance())
					{
						ListProject[i].Node_[j].Instance_.Add(new Instance());
						CopyInstance(ins,ListProject[i].Node_[j].Instance_[l]);
						l++;
					}
					int p = 0;
					foreach(Customer cus in nod.ReturnListCustomer())
					{
						ListProject[i].Node_[j].Customer_.Add(new Customer());
						CopyCustomer(cus,ListProject[i].Node_[j].Customer_[p]);
						p++;
					}	
					int q = 0;
					foreach(Scenario sce in nod.ReturnListScenario())
					{
						ListProject[i].Node_[j].Scenario_.Add(new Scenario());
						CopyScenario(sce,ListProject[i].Node_[j].Scenario_[q]);
						
							int r = 0;
							foreach(Variable vari in sce.ReturnListVariable())
							{
								ListProject[i].Node_[j].Scenario_[q].Variable_.Add(new Variable());
								CopyVariable(vari,ListProject[i].Node_[j].Scenario_[q].Variable_[r]);
								r++;
							}	
						
							int s = 0;
							foreach(Function fun in sce.ReturnListFunction())
							{
								ListProject[i].Node_[j].Scenario_[q].Function_.Add(new Function());
								CopyFunction(fun,ListProject[i].Node_[j].Scenario_[q].Function_[s]);
								s++;
							}							
						
						q++;
					}						
					int m = 0;
					foreach(Network net in nod.ReturnListNetwork())
					{
						ListProject[i].Node_[j].Network_.Add(new Network());
						CopyNetwork(net,ListProject[i].Node_[j].Network_[m]);
						
						int n = 0;
						foreach(Board boa in net.ReturnListBoard())
						{
							ListProject[i].Node_[j].Network_[m].Board_.Add(new Board());
							CopyBoard(boa,ListProject[i].Node_[j].Network_[m].Board_[n]);
							
							int o = 0;
							foreach(Pin pin in boa.ReturnListPin())
							{
								ListProject[i].Node_[j].Network_[m].Board_[n].Pin_.Add(new Pin());
								CopyPin(pin,ListProject[i].Node_[j].Network_[m].Board_[n].Pin_[o]);
								o++;
							}
							n++;
						}
						m++;
					}
					j++;
				}
				i++;						
				}
						
			UpdateInstanceUsed();
			mainwindow.UpdateMainNoteBook();
			mainwindow.UpdateWidgetInTab();
			mainwindow.UpdateMainWindow();
			mainwindow.RenameTooltipTextButtonOutput();
			mainwindow.UpdateOutputTreeview();		
			mainwindow.CloseTabInCaseOfDelete();
			mainwindow.UpdateStatusBar();
			
			if(ViewCopy>=1)
			{
				mainwindow.UndoRedoInactif("undo",true);
				mainwindow.UndoRedoInactif("redo",false);		
			}
			else if(ViewCopy < CountCopy - 1)
			{
				mainwindow.UndoRedoInactif("undo",false);
				mainwindow.UndoRedoInactif("redo",true);					
			}
			mainwindow.UpdateHistoricTreeView();
		}
		
//#################Fonction Save Project ##################################################################
		
		//Fonction SaveProject
		//Fonction permettant de sauver tous les projets ou un projet(A faire)
		public bool SaveProject(bool SaveAll, Int32 _Project_Id)
		{			
			bool SaveOneOk = false;		
			foreach(Project pro in ListProject)
			{
				SaveOneOk = false;
				string TempUnzip = pro.Project_SavePath + "/UnZip";
				string BasePath = pro.Project_SavePath + "/Temp";
				string CompressPath = pro.Project_SavePath;		
				
				if(Directory.Exists(pro.Project_SavePath))
				{
					if((pro.Project_Id == CurrentProjectId && !pro.ProjectIsSave) || (SaveAll && !pro.ProjectIsSave) || (pro.Project_Id == _Project_Id && !pro.ProjectIsSave) )
					{
						SaveOneOk = true;

						string NameFile = "";
						//Dans le cas ou le dossier existe, nous le supprimons
						if(Directory.Exists(BasePath)) 
						{				
							DirectoryInfo directory = new DirectoryInfo(BasePath);
							directory.Delete(true);
						}
						
						//On fait un .bak.version de la précédente sauvegarde
						if(File.Exists(pro.Project_SavePath + "/" + pro.Project_Name + ".dom"))
							System.IO.File.Copy(pro.Project_SavePath + "/" + pro.Project_Name + ".dom", pro.Project_SavePath + "/" + pro.Project_Name + ".dom.bak(Version" + pro.Project_Version + ")", true);
						
						//On met à jour la date du projet et la version
						pro.Project_ModificationDateAndTime = DateTime.Now.ToString();
						pro.Project_Version = pro.Project_Version + 1;
						mainwindow.UpdateWidgetInTab();
						
						ExtractZipFile(pro.Project_SavePath + "/" + pro.Project_Name + ".dom",TempUnzip,pro.Project_Password);
						
//##########################  PROJECT #################################################################			
						
						//On va écrire un nouveau fichier xml pour le projet
						using (StringWriter StringWriterProject = new StringWriter())
						using (XmlTextWriter xmlProject = new XmlTextWriter(StringWriterProject))
						{
							//En tête du fichier xml Project
							HeaderXmlFile(xmlProject);
							
							//On ajoute les éléments pour le projet
							xmlProject.WriteStartElement("Projects");
								AddElement(xmlProject,"Project","Name",pro.Project_Name,1);
								AddElement(xmlProject,"Project","Author",pro.Project_Author,1);
								AddElement(xmlProject,"Project","Note",pro.Project_Note.ToString(),1);
								AddElement(xmlProject,"Project","CreationDateAndTime",pro.Project_CreationDateAndTime.ToString(),1);
								AddElement(xmlProject,"Project","ModificationDateAndTime",pro.Project_ModificationDateAndTime.ToString(),1);
								AddElement(xmlProject,"Project","Version",pro.Project_Version.ToString(),1);
							
		//##########################  NODE #################################################################							
							
							//Même travail pour les noeud
							foreach(Node node in pro.ReturnListNode())
							{
								using (StringWriter StringWriterNode = new StringWriter())
								using (XmlTextWriter xmlNode = new XmlTextWriter(StringWriterNode))
								{
									
									//En tête du fichier xml Project
									HeaderXmlFile(xmlNode);
									
									xmlNode.WriteStartElement("Nodes");
										AddElement(xmlNode,"Node","Name",node.Node_Name,1);	
										AddElement(xmlNode,"Node","IP",node.Node_IP,1);	
										AddElement(xmlNode,"Node","GTWIP",node.Node_GTWIP,1);	
										AddElement(xmlNode,"Node","Mac",node.Node_Mac,1);	
										AddElement(xmlNode,"Node","DHCP",node.Node_DHCP.ToString(),1);	
										AddElement(xmlNode,"Node","Type",node.Node_Type,1);	
										AddElement(xmlNode,"Node","Clock",node.Node_Clock,1);	
										AddElement(xmlNode,"Node","OneWire",node.Node_1Wire.ToString(),1);
										AddElement(xmlNode,"Node","Note",node.Node_Note,1);
										AddElement(xmlNode,"Node","WebServer",node.Node_WebServer.ToString(),1);	
									
		//##########################  DEBUG #################################################################									
									
									//Même travail pour les debug
									foreach(Debug deb in node.ReturnListDebug())
									{	
										using (StringWriter StringWriterDebug = new StringWriter())
										using (XmlTextWriter xmlDebug = new XmlTextWriter(StringWriterDebug))
										{	
											//En tête du fichier xml Debug
											HeaderXmlFile(xmlDebug);		
											
											xmlDebug.WriteStartElement("Debugs");
												AddElement(xmlDebug,"Debug","Name",deb.Name,1);	
												AddElement(xmlDebug,"Debug","Value",deb.Value.ToString(),1);	
												AddElement(xmlDebug,"Debug","FrenchDescription",deb.FrenchDescriptionDebug,1);
												AddElement(xmlDebug,"Debug","EnglishDescription",deb.EnglishDescriptionDebug,1);	
											
											NameFile = deb.Name;
											//On sauvegarde le xml du réseau dans un fichier
											SaveXmlInFile(xmlDebug,StringWriterDebug, BasePath + "/Debugs","/" + NameFile);
											//On lit le fichier que nous venons d'enregistrer
											string FileXmlDebug = System.IO.File.ReadAllText(BasePath + "/Debugs/" + NameFile);	
											//On renomme le fichier node avec son HashCode
											if(!File.Exists(BasePath + "/Debugs/" + CalculHash(FileXmlDebug))) 
											{
												File.Move(BasePath + "/Debugs/" + NameFile, BasePath + "/Debugs/" + CalculHash(FileXmlDebug));									
											}
											else
											{
												File.Delete(BasePath + "/Debugs/" + NameFile);
											}
											
											//On ajoute un réseau dans le xml du noeud
											AddElement(xmlNode,"Node","Debugs",CalculHash(FileXmlDebug),2);									
										}
									}							
									
		//##########################  INSTANCE #################################################################									
									
									//Même travail pour les instances
									foreach(Instance ins in node.ReturnListInstance())
									{			
										using (StringWriter StringWriterInstance = new StringWriter())
										using (XmlTextWriter xmlInstance = new XmlTextWriter(StringWriterInstance))
										{
											//En tête du fichier xml Instance
											HeaderXmlFile(xmlInstance);								
											
											xmlInstance.WriteStartElement("Instances");
												AddElement(xmlInstance,"Instance","Name",ins.Instance_Name,1);	
												AddElement(xmlInstance,"Instance","Type",ins.Instance_Type,1);
												AddElement(xmlInstance,"Instance","Direction",ins.Instance_Direction,1);	
												AddElement(xmlInstance,"Instance","Note",ins.Instance_Note,1);	
												AddElement(xmlInstance,"Instance","UpDownStop",ins.Instance_Up_Down_Stop.ToString(),1);	
												AddElement(xmlInstance,"Instance","LIGDefaultValue",ins.Instance_LIG_DefaultValue.ToString(),1);
												AddElement(xmlInstance,"Instance","LIGFade",ins.Instance_LIG_Fade.ToString(),1);	
												AddElement(xmlInstance,"Instance","SWIInverse",ins.Instance_SWI_Inverse.ToString(),1);
												AddElement(xmlInstance,"Instance","SWIImpulstionTime",ins.Instance_SWI_ImpulsionTime.ToString(),1);
												AddElement(xmlInstance,"Instance","SHUType",ins.Instance_SHU_Type.ToString(),1);
												AddElement(xmlInstance,"Instance","SHUTime",ins.Instance_SHU_Time.ToString(),1);
												AddElement(xmlInstance,"Instance","SHUInitTime",ins.Instance_SHU_InitTime.ToString(),1);
												AddElement(xmlInstance,"Instance","SHUNumberOfOutput",ins.Instance_SHU_NumberOfOutput.ToString(),1);
											
											NameFile = ins.Instance_Name;
											//On sauvegarde le xml du réseau dans un fichier
											SaveXmlInFile(xmlInstance,StringWriterInstance, BasePath + "/Instances","/" + NameFile);
											//On lit le fichier que nous venons d'enregistrer
											string FileXmlInstance = System.IO.File.ReadAllText(BasePath + "/Instances/" + NameFile);	
											//On renomme le fichier node avec son HashCode
											if(!File.Exists(BasePath + "/Instances/" + CalculHash(FileXmlInstance))) 
											{
												File.Move(BasePath + "/Instances/" + NameFile, BasePath + "/Instances/" + CalculHash(FileXmlInstance));
											}
											else
											{
												File.Delete(BasePath + "/Instances/" + NameFile);
											}									
											
											ins.Instance_HashCode = CalculHash(FileXmlInstance);
											
											//On ajoute un réseau dans le xml du noeud
											AddElement(xmlNode,"Node","Instances",CalculHash(FileXmlInstance),2);										
										}
									}							
									
		//##########################  SCENARIO ##############################################################	
									
									//Même travail pour les fichiers scénarios
									foreach(Scenario sce in node.ReturnListScenario())
									{			
										using (StringWriter StringWriterScenario = new StringWriter())
										using (XmlTextWriter xmlScenario = new XmlTextWriter(StringWriterScenario))
										{
											//En tête du fichier xml Scenario
											HeaderXmlFile(xmlScenario);	
											
											xmlScenario.WriteStartElement("Scenarios");
												AddElement(xmlScenario,"Scenario","Name",sce.ScenarioName,1);
												AddElement(xmlScenario,"Scenario","Data",sce.ScenarioData,1);
												AddElement(xmlScenario,"Scenario","Note",sce.ScenarioNotes,1);
										
		//##########################  VARIABLE ##############################################################										
											
											//Même travail pour les fichiers Variables
											foreach(Variable vari in sce.ReturnListVariable())
											{			
												using (StringWriter StringWriterVariable = new StringWriter())
												using (XmlTextWriter xmlVariable = new XmlTextWriter(StringWriterVariable))
												{
													//En tête du fichier xml Variable
													HeaderXmlFile(xmlVariable);
													
													xmlVariable.WriteStartElement("Variables");
														AddElement(xmlVariable,"Variable","Name",vari.VariableName,1);
														AddElement(xmlVariable,"Variable","Type",vari.VariableType,1);
														AddElement(xmlVariable,"Variable","DefaultValue",vari.VariableDefaultValue.ToString(),1);	
														AddElement(xmlVariable,"Variable","Note",vari.VariableNote,1);
													
														NameFile = vari.VariableName;
														//On sauvegarde le xml du réseau dans un fichier
														SaveXmlInFile(xmlVariable,StringWriterVariable, BasePath + "/Variables","/" + NameFile);
														//On lit le fichier que nous venons d'enregistrer
														string FileXmlVariable = System.IO.File.ReadAllText(BasePath + "/Variables/" + NameFile);	
														//On renomme le fichier node avec son HashCode
														if(!File.Exists(BasePath + "/Variables/" + CalculHash(FileXmlVariable))) 
														{
															File.Move(BasePath + "/Variables/" + NameFile, BasePath + "/Variables/" + CalculHash(FileXmlVariable));		
														}
														else
														{
															File.Delete(BasePath + "/Variables/" + NameFile);
														}
													
														//On ajoute un réseau dans le xml du noeud
														AddElement(xmlScenario,"Scenario","Variables",CalculHash(FileXmlVariable),2);												
												}
											}
											
		//##########################  FUNCTION ##############################################################										
											
											//Même travail pour les fichiers Fonctions
											foreach(Function fun in sce.ReturnListFunction())
											{			
												using (StringWriter StringWriterFunction = new StringWriter())
												using (XmlTextWriter xmlFunction = new XmlTextWriter(StringWriterFunction))
												{
													//En tête du fichier xml Variable
													HeaderXmlFile(xmlFunction);
													
													xmlFunction.WriteStartElement("Functions");
														AddElement(xmlFunction,"Function","Name",fun.FunctionName,1);
														AddElement(xmlFunction,"Function","Type",fun.FunctionTypeReturn,1);
														AddElement(xmlFunction,"Function","Data",fun.FunctionData,1);
														AddElement(xmlFunction,"Function","Note",fun.FunctionNote,1);	
														AddElement(xmlFunction,"Function","InitFunction",fun.InitFunction.ToString(),1);
													
														AddElement(xmlFunction,"Function","TypeArgs1",fun.FunctionTypeArg1,1);
														AddElement(xmlFunction,"Function","TypeArgs2",fun.FunctionTypeArg2,1);
														AddElement(xmlFunction,"Function","TypeArgs3",fun.FunctionTypeArg3,1);	
														AddElement(xmlFunction,"Function","TypeArgs4",fun.FunctionTypeArg4,1);
														AddElement(xmlFunction,"Function","TypeArgs5",fun.FunctionTypeArg5,1);
														AddElement(xmlFunction,"Function","TypeArgs6",fun.FunctionTypeArg6,1);	
													
														AddElement(xmlFunction,"Function","NameArgs1",fun.FunctionNameArg1,1);
														AddElement(xmlFunction,"Function","NameArgs2",fun.FunctionNameArg2,1);
														AddElement(xmlFunction,"Function","NameArgs3",fun.FunctionNameArg3,1);	
														AddElement(xmlFunction,"Function","NameArgs4",fun.FunctionNameArg4,1);
														AddElement(xmlFunction,"Function","NameArgs5",fun.FunctionNameArg5,1);
														AddElement(xmlFunction,"Function","NameArgs6",fun.FunctionNameArg6,1);		
													
														NameFile = fun.FunctionName;
														//On sauvegarde le xml du réseau dans un fichier
														SaveXmlInFile(xmlFunction,StringWriterFunction, BasePath + "/Functions","/" + NameFile);
														//On lit le fichier que nous venons d'enregistrer
														string FileXmlFunction = System.IO.File.ReadAllText(BasePath + "/Functions/" + NameFile);	
														//On renomme le fichier node avec son HashCode
														if(!File.Exists(BasePath + "/Functions/" + CalculHash(FileXmlFunction))) 
														{
															File.Move(BasePath + "/Functions/" + NameFile, BasePath + "/Functions/" + CalculHash(FileXmlFunction));	
														}
														else
														{
															File.Delete(BasePath + "/Functions/" + NameFile);
														}
																									
														//On ajoute un réseau dans le xml du noeud
														AddElement(xmlScenario,"Scenario","Functions",CalculHash(FileXmlFunction),2);											
												}
											}
											
											sce.ScenarioHashCode = "";
											NameFile = sce.ScenarioName;
											//On sauvegarde le xml du réseau dans un fichier
											SaveXmlInFile(xmlScenario,StringWriterScenario, BasePath + "/Scenarios","/" + NameFile);
											//On lit le fichier que nous venons d'enregistrer
											string FileXmlScenario = System.IO.File.ReadAllText(BasePath + "/Scenarios/" + NameFile);	
											//On renomme le fichier node avec son HashCode
											if(!File.Exists(BasePath + "/Scenarios/" + CalculHash(FileXmlScenario))) 
											{
												File.Move(BasePath + "/Scenarios/" + NameFile, BasePath + "/Scenarios/" + CalculHash(FileXmlScenario));	
											}
											else
											{
												File.Delete(BasePath + "/Scenarios/" + NameFile);
											}									
											
											sce.ScenarioHashCode = CalculHash(FileXmlScenario);
											
											//On ajoute un réseau dans le xml du noeud
											AddElement(xmlNode,"Node","Scenarios",CalculHash(FileXmlScenario),2);										
										}
									}
									
		//##########################  CUSTOMER ##############################################################								
									
									//Même travail pour les fichiers customer
									foreach(Customer cus in node.ReturnListCustomer())
									{			
										using (StringWriter StringWriterCustomer = new StringWriter())
										using (XmlTextWriter xmlCustomer = new XmlTextWriter(StringWriterCustomer))
										{
											//En tête du fichier xml Customer
											HeaderXmlFile(xmlCustomer);		
											
											xmlCustomer.WriteStartElement("Customers");
												AddElement(xmlCustomer,"Customer","Name",cus.CustomerName,1);
												AddElement(xmlCustomer,"Customer","Data",cus.CustomerData,1);
												AddElement(xmlCustomer,"Customer","Note",cus.CustomerNote,1);
												AddElement(xmlCustomer,"Customer","Use",cus.CustomerUse.ToString(),1);
											
											bool flag = false;
											foreach(Scenario sce in node.ReturnListScenario())
											{	
												if(sce.ScenarioId == cus.ScenarioId)
												{
													AddElement(xmlCustomer,"Customer","ScenarioId",sce.ScenarioHashCode,1);
													flag = true;
												}
											}
											
											if(!flag)
												AddElement(xmlCustomer,"Customer","ScenarioId","",1);
											
											NameFile = cus.CustomerName;
											//On sauvegarde le xml du réseau dans un fichier
											SaveXmlInFile(xmlCustomer,StringWriterCustomer, BasePath + "/Customers","/" + NameFile);
											//On lit le fichier que nous venons d'enregistrer
											string FileXmlCustomer = System.IO.File.ReadAllText(BasePath + "/Customers/" + NameFile);	
											//On renomme le fichier node avec son HashCode
											if(!File.Exists(BasePath + "/Customers/" + CalculHash(FileXmlCustomer))) 
											{
												File.Move(BasePath + "/Customers/" + NameFile, BasePath + "/Customers/" + CalculHash(FileXmlCustomer));		
											}
											else
											{
												File.Delete(BasePath + "/Customers/" + NameFile);
											}											
											
											//On ajoute un réseau dans le xml du noeud
											AddElement(xmlNode,"Node","Customers",CalculHash(FileXmlCustomer),2);										
										}
									}
									
		//##########################  NETWORK ##############################################################									
									
									//Même travail pour les réseaux
									foreach(Network net in node.ReturnListNetwork())
									{	
										using (StringWriter StringWriterNetwork = new StringWriter())
										using (XmlTextWriter xmlNetwork = new XmlTextWriter(StringWriterNetwork))
										{	
											//En tête du fichier xml Network
											HeaderXmlFile(xmlNetwork);									
											
											xmlNetwork.WriteStartElement("Networks");
												AddElement(xmlNetwork,"Network","Type",net.Network_Type,1);	
												AddElement(xmlNetwork,"Network","Note",net.Network_Note,1);	
											
											
		//##########################  BOARD ##############################################################									
											
											
											//Même travail pour les cartes
											foreach(Board boa in net.ReturnListBoard())
											{	
												using (StringWriter StringWriterBoard = new StringWriter())
												using (XmlTextWriter xmlBoard = new XmlTextWriter(StringWriterBoard))
												{	
													//En tête du fichier xml Board
													HeaderXmlFile(xmlBoard);	
													
													xmlBoard.WriteStartElement("Boards");
														AddElement(xmlBoard,"Board","Name",boa.Board_Name,1);	
														AddElement(xmlBoard,"Board","Type",boa.Board_Type,1);
														AddElement(xmlBoard,"Board","I2C0",boa.Board_I2C_0.ToString(),1);
														AddElement(xmlBoard,"Board","I2C1",boa.Board_I2C_1.ToString(),1);
														AddElement(xmlBoard,"Board","OneWireMac",boa.Board_1Wire_Mac,1);
														AddElement(xmlBoard,"Board","OneWirePrecision",boa.Board_1Wire_Precision,1);											
														AddElement(xmlBoard,"Board","Note",boa.Board_Note,1);	
														
		//##########################  PIN ##############################################################												
													
													
														//Même travail pour les cartes
														foreach(Pin pin in boa.ReturnListPin())
														{	
															using (StringWriter StringWriterPin = new StringWriter())
															using (XmlTextWriter xmlPin = new XmlTextWriter(StringWriterPin))
															{	
																//En tête du fichier xml Pin
																HeaderXmlFile(xmlPin);
															
																xmlPin.WriteStartElement("Pins");
																	AddElement(xmlPin,"Pin","Name",pin.Pin_Name,1);	
																	AddElement(xmlPin,"Pin","Number",pin.Pin_Number.ToString(),1);
																	AddElement(xmlPin,"Pin","Direction",pin.Pin_Direction,1);
																	AddElement(xmlPin,"Pin","FallBack",pin.Pin_FallbackValue.ToString(),1);
															
																bool Flag = false;
																foreach(Instance ins in node.ReturnListInstance())
																{
																	if(ins.Instance_Id == pin.Instance_Id)
																	{
																		AddElement(xmlPin,"Pin","InstanceId",ins.Instance_HashCode,1);
																		Flag = true;
																	}
																}
																if(!Flag)
																	AddElement(xmlPin,"Pin","InstanceId","",1);
																
																NameFile = pin.Pin_Name;
																//On sauvegarde le xml du réseau dans un fichier
																SaveXmlInFile(xmlPin,StringWriterPin, BasePath + "/Pins","/" +  NameFile);
																//On lit le fichier que nous venons d'enregistrer
																string FileXmlPin = System.IO.File.ReadAllText(BasePath + "/Pins/" + NameFile);	
																//On renomme le fichier node avec son HashCode
																if(!File.Exists(BasePath + "/Pins/" + CalculHash(FileXmlPin))) 
																{
																	File.Move(BasePath + "/Pins/" + NameFile, BasePath + "/Pins/" + CalculHash(FileXmlPin));	
																}
																else
																{
																	File.Delete(BasePath + "/Pins/" + NameFile);
																}										
																
																//On ajoute un réseau dans le xml du noeud
																AddElement(xmlBoard,"Board","Pins",CalculHash(FileXmlPin),2);	
															
															
															}	
														}
														
													
														NameFile = boa.Board_Name;
														//On sauvegarde le xml du réseau dans un fichier
														SaveXmlInFile(xmlBoard,StringWriterBoard, BasePath + "/Boards","/" +  NameFile);
														//On lit le fichier que nous venons d'enregistrer
														string FileXmlBoard = System.IO.File.ReadAllText(BasePath + "/Boards/" + NameFile);	
														//On renomme le fichier node avec son HashCode
														if(!File.Exists(BasePath + "/Boards/" + CalculHash(FileXmlBoard))) 
														{
															File.Move(BasePath + "/Boards/" + NameFile, BasePath + "/Boards/" + CalculHash(FileXmlBoard));	
														}
														else
														{
															File.Delete(BasePath + "/Boards/" + NameFile);
														}										
														
														//On ajoute un réseau dans le xml du noeud
														AddElement(xmlNetwork,"Network","Boards",CalculHash(FileXmlBoard),2);											
												}
											}
											
											NameFile = net.Network_Type;
											//On sauvegarde le xml du réseau dans un fichier
											SaveXmlInFile(xmlNetwork,StringWriterNetwork, BasePath + "/Networks","/" +  NameFile);
											//On lit le fichier que nous venons d'enregistrer
											string FileXmlNetwork = System.IO.File.ReadAllText(BasePath + "/Networks/" + NameFile);	
											//On renomme le fichier node avec son HashCode
											if(!File.Exists(BasePath + "/Networks/" + CalculHash(FileXmlNetwork))) 
											{
												File.Move(BasePath + "/Networks/" + NameFile, BasePath + "/Networks/" + CalculHash(FileXmlNetwork));	
											}
											else
											{
												File.Delete(BasePath + "/Networks/" + NameFile);
											}										
											
											//On ajoute un réseau dans le xml du noeud
											AddElement(xmlNode,"Node","Networks",CalculHash(FileXmlNetwork),2);
										}
									}		
									
									//On sauvegarde le xml du noeud dans un fichier
									SaveXmlInFile(xmlNode,StringWriterNode, BasePath + "/Nodes","/" + node.Node_Name);
									//On lit le fichier que nous venons d'enregistrer
									string FileXmlNode = System.IO.File.ReadAllText(BasePath + "/Nodes/" + node.Node_Name);	
									//On renomme le fichier node avec son HashCode
									if(!File.Exists(BasePath + "/Nodes/" + CalculHash(FileXmlNode)))
									{
										File.Move(BasePath + "/Nodes/" + node.Node_Name, BasePath + "/Nodes/" + CalculHash(FileXmlNode));
									}
									else
									{
										File.Delete(BasePath + "/Nodes/" + NameFile);
									}								
									
									//On ajoute un noeud dans le xml du projet
									AddElement(xmlProject,"Project","Nodes",CalculHash(FileXmlNode),2);
									node.Node_CRC = CalculHash(FileXmlNode);
									
									//Permet de créer un dossier pour ajouter les fichiers compilés
									if(!Directory.Exists(BasePath + "/Hex")) 
									{
										 System.IO.Directory.CreateDirectory(BasePath + "/Hex");
									}									
									
									//Si le fichier compilé existait déjà dans le dossier compréssé, on le remet dans le dossier final
									if(File.Exists(TempUnzip + "/Hex/" + node.Node_CRC + ".hex"))
									{
										File.Move(TempUnzip + "/Hex/" + node.Node_CRC + ".hex",BasePath + "/Hex/" + node.Node_CRC + ".hex");
										node.Node_Compile = true;
									}
									else
									{
										node.Node_Compile = false;
									}
																	
								}
							}
							//On sauvegarde le xml du projet dans un fichier
							SaveXmlInFile(xmlProject,StringWriterProject, BasePath, "/" + pro.Project_Name + ".dom");
						}
						
						pro.ProjectIsSave = true;
						mainwindow.UpdateStatusBar();

						//On appel la fonction permettant de créer le nouveau fichier .dom
						CompressProject(CompressPath,pro.Project_Password,BasePath,pro.Project_Name);
						
									
						CopyProject("HTUpdateVersionOneProject",pro.Project_Name);					
					}
					else if(pro.ProjectIsSave)
					{
						//return true;
					}
				}
				
				//On supprime le dossier TempUnzip
				if(Directory.Exists(TempUnzip)) 
				{				
					DirectoryInfo directory = new DirectoryInfo(TempUnzip);
					directory.Delete(true);
				}	
			}
			
			if(SaveOneOk)
			{
				mainwindow.AddLineOutput(param.ParamI("OutputInformation"),"SaveSucceful");
			}
			
			mainwindow.UpdateEplorerTreeView(); //Mise à jour de l'explorer projet
			mainwindow.UpdateMainNoteBook();
			mainwindow.UpdateWidgetInTab();
			return SaveOneOk;
		}
		
		//Fonction HeaderXmlFile
		//Fonction permettant de faire le Header dans le xml
		public void HeaderXmlFile(XmlTextWriter _xml)
		{
		    _xml.WriteStartDocument();
			_xml.WriteWhitespace("\n");			
		}
		
		//Fonction SaveXmlInFile
		//Fonction permettant de mettre un xml dans un fichier)
		public void SaveXmlInFile(XmlTextWriter _xml, StringWriter _StringWriter, string _Path, string _File)
		{
			
			if(!Directory.Exists(_Path)) 
			{
				 System.IO.Directory.CreateDirectory(_Path);
			}			
			
			if(!File.Exists(_Path + _File)) 
			{			
				_xml.WriteWhitespace("\n");
				_xml.WriteEndElement();
				_xml.WriteEndDocument();	
				FileStream = new FileStream(_Path + _File,FileMode.Create);
				StreamWriter = new StreamWriter(this.FileStream);
				StreamWriter.WriteLine(_StringWriter.ToString().Replace("encoding=\"utf-16\"","encoding=\"utf-8\""));	
				StreamWriter.Close();
				FileStream.Close();	
			}
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
		
		//Fonction CompressFolder
		//Fonction permettant de compresser un dossier
		public void CompressProject(string outPathname, string password, string folderName, string _ProjectName)
		{
			
			
			
			ICSharpCode.SharpZipLib.Zip.FastZip fastZip = new ICSharpCode.SharpZipLib.Zip.FastZip();
			fastZip.Password = password;
			fastZip.CreateEmptyDirectories = true;
			string zipFileName = outPathname + "/" + _ProjectName + ".dom";

			//On supprime l'ancien .dom avant de créer le nouveau
			if(File.Exists(zipFileName))
			{
				File.Delete(zipFileName);
			}	
			
			//On fait la création du zip
			fastZip.CreateZip(zipFileName, folderName, true, "");
			
			//on supprime le dossier source
			if(Directory.Exists(folderName)) 
			{				
				DirectoryInfo directory = new DirectoryInfo(folderName);
				directory.Delete(true);
			}				
		}	
		
		//Fonction FileIsCrypte
		//Fonction permettant de savoir si un fichier et crypté
		public bool FileIsCrypte(string archiveFilenameIn)
		{
		    ICSharpCode.SharpZipLib.Zip.ZipFile zf = null;
		    try 
			{
		        FileStream fs = File.OpenRead(archiveFilenameIn);
		        zf = new ICSharpCode.SharpZipLib.Zip.ZipFile(fs);
				
		        foreach (ICSharpCode.SharpZipLib.Zip.ZipEntry zipEntry in zf) 
				{
					if (zipEntry.IsCrypted) 
					{	
						return true;
					}
				}
			}
			catch 
			{
		        if (zf != null) 
				{
		            zf.IsStreamOwner = true; // Makes close also shut the underlying stream
		            zf.Close(); // Ensure we release resources
		        }
				return false;				
			}
			return false;
		}
		
		//Fonction ExtractZipFile
		//Fonction permettant d'extraire les fichiers zip dans un dossier
		public bool ExtractZipFile(string archiveFilenameIn, string outFolder, string password)
		{
		    ICSharpCode.SharpZipLib.Zip.ZipFile zf = null;
		    try 
			{
		        FileStream fs = File.OpenRead(archiveFilenameIn);
		        zf = new ICSharpCode.SharpZipLib.Zip.ZipFile(fs);
				
		        if (!String.IsNullOrEmpty(password)) 
				{
		            zf.Password = password;
		        }
		        foreach (ICSharpCode.SharpZipLib.Zip.ZipEntry zipEntry in zf) 
				{
			        if (String.IsNullOrEmpty(password) && zipEntry.IsCrypted) 
					{	
						return false;
					}
					
		            if (!zipEntry.IsFile) 
					{
		                continue;           // Ignore directories
		            }
		            String entryFileName = zipEntry.Name;
		
		            byte[] buffer = new byte[4096];     // 4K is optimum
					
		            Stream zipStream = zf.GetInputStream(zipEntry);
					
					
		            String fullZipToPath = Path.Combine(outFolder, entryFileName);
					
					
		            string directoryName = Path.GetDirectoryName(fullZipToPath);
		            if (directoryName.Length > 0)
		                Directory.CreateDirectory(directoryName);

					
		            using (FileStream streamWriter = File.Create(fullZipToPath)) {
		                ICSharpCode.SharpZipLib.Core.StreamUtils.Copy(zipStream, streamWriter, buffer);
		            }
		        }
		    } 
			catch 
			{
		        if (zf != null) {
		            zf.IsStreamOwner = true; // Makes close also shut the underlying stream
		            zf.Close(); // Ensure we release resources
		        }
				return false;
		    }
			return true;
		}		
		
//################# Fonction Open Project ##################################################################
		
		//Fonction OpenProject
		//Fonction permettant d'ouvrir un projet
		public int OpenProject(string _Path, string _password)
		{	
			string _DirectoryPath = Path.GetDirectoryName(_Path) + "/temp";
			string _FilePath  = "";
			
			
			foreach(Project pro in ListProject)
			{
				if(pro.Project_SavePath + "/" + pro.Project_Name + ".dom" == _Path)
				{
					return 0;
				}
			}
			
			//UnCompressProject(_Path,"",_DirectoryPath);
			if(!ExtractZipFile(_Path,_DirectoryPath,_password))
			{				
				return 0;
			}
			
			DirectoryInfo dir = new DirectoryInfo(_DirectoryPath);
			FileInfo[] fichiers = dir.GetFiles();
			
			foreach ( FileInfo fichier in fichiers)
			{
				if(fichier.Extension == ".dom")
				{
					_FilePath = fichier.Name.ToString();
				}
			} 			
			
			if(_FilePath == "")
			{
				mainwindow.AddLineOutput(param.ParamI("OutputError"),"ProjectNotCorrect");
				return 0;
			}
			
			using (XmlTextReader ReaderProject = new XmlTextReader(_DirectoryPath + "/" + _FilePath)) //Ouverture du fichier Param.xml
			{	
				Project Pro;		
				Node node;
				Debug deb;
				Instance ins;
				Scenario sce;
				Variable vari;
				Function fun;
				Customer cus;
				Network net;
				Board boa;
				Pin pin;
				
//####################################################################  PROJECT #################################################################				
				
				Pro = new Project(Project_Id,param,pref);
				ListProject.Add(Pro); //Ajout d'un nouveau projet dans la liste
				CurrentProjectId = Project_Id;
				Project_Id++; //Incrémentation de l'id project	
			    while (ReaderProject.Read()) //Lecture total du fichier
			    {
					if (ReaderProject.IsStartElement())
					{
						if(ReaderProject.Name == "Project")
						{		
							Pro.Project_SavePath = Path.GetDirectoryName(_Path);
							Pro.Project_Password = _password;
							Pro.ProjectIsSave = true;
 							
							if(ReaderProject["Name"] != null)
								 Pro.Project_Name = ReaderProject["Name"];
							if(ReaderProject["Author"] != null)
								 Pro.Project_Author = ReaderProject["Author"];	
							if(ReaderProject["Note"] != null)
								 Pro.Project_Note = ReaderProject["Note"];								
							if(ReaderProject["CreationDateAndTime"] != null)
								 Pro.Project_CreationDateAndTime = ReaderProject["CreationDateAndTime"];	
							if(ReaderProject["ModificationDateAndTime"] != null)
								 Pro.Project_ModificationDateAndTime = ReaderProject["ModificationDateAndTime"];		
							if(ReaderProject["Version"] != null)
								 Pro.Project_Version = Convert.ToInt32(ReaderProject["Version"]);		
							
//#######################################################################  NODE #################################################################								
							
							if(ReaderProject["Nodes"] != null)
							{								
								//On vérifie si le fichier existe
								if(!File.Exists(_DirectoryPath + "/Nodes/" + ReaderProject["Nodes"])) 
								{	
									return 0;
								}
								//On vérifie qu'il n'a pas subit de modification
								if(CalculHash(System.IO.File.ReadAllText(_DirectoryPath + "/Nodes/" + ReaderProject["Nodes"])) != ReaderProject["Nodes"])
								{
									return 0;
								}
								//On vient lire le fichier Node
								using (XmlTextReader ReaderNode = new XmlTextReader(_DirectoryPath + "/Nodes/" + ReaderProject["Nodes"])) //Ouverture du fichier Param.xml
								{	
									node = Pro.AddNodeInProject(Node_Id);
									Node_Id++;
									
								    while (ReaderNode.Read()) //Lecture total du fichier
								    {
										if (ReaderNode.IsStartElement())
										{
											if(ReaderNode.Name == "Node")
											{	
												if(ReaderNode["Name"] != null)
													 node.Node_Name = ReaderNode["Name"];	
												if(ReaderNode["IP"] != null)
													 node.Node_IP = ReaderNode["IP"];
												if(ReaderNode["GTWIP"] != null)
													 node.Node_GTWIP = ReaderNode["GTWIP"];	
												if(ReaderNode["Mac"] != null)
													 node.Node_Mac = ReaderNode["Mac"];
												if(ReaderNode["DHCP"] != null)
													 node.Node_DHCP = Convert.ToBoolean(ReaderNode["DHCP"]);	
												if(ReaderNode["Type"] != null)
													 node.Node_Type = ReaderNode["Type"];
												if(ReaderNode["Clock"] != null)
													 node.Node_Clock = ReaderNode["Clock"];
												if(ReaderNode["OneWire"] != null)
													 node.Node_1Wire = Convert.ToBoolean(ReaderNode["OneWire"]);	
												if(ReaderNode["Note"] != null)
													 node.Node_Note = ReaderNode["Note"];
												if(ReaderNode["WebServer"] != null)
													 node.Node_WebServer = Convert.ToBoolean(ReaderNode["WebServer"]);													

												
												node.Node_CRC = ReaderProject["Nodes"]; //On vient mettre le CRC du noeud dans une variable
												
												//Nous allons vérifier si nous avons un .hex pour un noeud et si oui nous venons complété l'information compilé
												if(File.Exists(_DirectoryPath + "/Hex/" + node.Node_CRC + ".hex"))
												{
													node.Node_Compile = true;
												}
												else
												{
													node.Node_Compile = false;
												}
												
//#######################################################################  DEBUG #################################################################	
												
												if(ReaderNode["Debugs"] != null)
												{										
													//On vérifie si le fichier existe
													if(!File.Exists(_DirectoryPath + "/Debugs/" + ReaderNode["Debugs"])) 
													{	
														return 0;
													}
													//On vérifie qu'il n'a pas subit de modification
													if(CalculHash(System.IO.File.ReadAllText(_DirectoryPath + "/Debugs/" + ReaderNode["Debugs"])) != ReaderNode["Debugs"])
													{
														return 0;
													}												
													
													//On vient lire le fichier Debug
													using (XmlTextReader ReaderDebug = new XmlTextReader(_DirectoryPath + "/Debugs/" + ReaderNode["Debugs"])) //Ouverture du fichier Param.xml
													{	
														deb = node.AddDebugInNode();
													    while (ReaderDebug.Read()) //Lecture total du fichier
													    {
															if (ReaderDebug.IsStartElement())
															{
																if(ReaderDebug.Name == "Debug")
																{	
																	if(ReaderDebug["Name"] != null)
																		 deb.Name = ReaderDebug["Name"];	
																	if(ReaderDebug["Value"] != null)
																		 deb.Value = Convert.ToBoolean(ReaderDebug["Value"]);
																	if(ReaderDebug["FrenchDescription"] != null)
																		 deb.FrenchDescriptionDebug = ReaderDebug["FrenchDescription"];	
																	if(ReaderDebug["EnglishDescription"] != null)
																		 deb.EnglishDescriptionDebug = ReaderDebug["EnglishDescription"];
																}
															}
														}
														ReaderDebug.Close ();
													}
												}
												
//#############################################################  INSTANCE #####################################################################################	
												
												if(ReaderNode["Instances"] != null)
												{										
													//On vérifie si le fichier existe
													if(!File.Exists(_DirectoryPath + "/Instances/" + ReaderNode["Instances"])) 
													{	
														return 0;
													}
													//On vérifie qu'il n'a pas subit de modification
													if(CalculHash(System.IO.File.ReadAllText(_DirectoryPath + "/Instances/" + ReaderNode["Instances"])) != ReaderNode["Instances"])
													{
														return 0;
													}	
													
													//On vient lire le fichier Instance
													using (XmlTextReader ReaderInstance = new XmlTextReader(_DirectoryPath + "/Instances/" + ReaderNode["Instances"])) //Ouverture du fichier Param.xml
													{
														ins = node.AddInstanceInNode(Instance_Id);
														Instance_Id++;
														ins.Instance_HashCode = ReaderNode["Instances"];
 														
													    while (ReaderInstance.Read()) //Lecture total du fichier
													    {
															if (ReaderInstance.IsStartElement())
															{
																if(ReaderInstance.Name == "Instance")
																{	
																	if(ReaderInstance["Name"] != null)
																		 ins.Instance_Name = ReaderInstance["Name"];	
																	if(ReaderInstance["Type"] != null)
																		 ins.Instance_Type = ReaderInstance["Type"];
																	if(ReaderInstance["Direction"] != null)
																		 ins.Instance_Direction = ReaderInstance["Direction"];	
																	if(ReaderInstance["Note"] != null)
																		 ins.Instance_Note = ReaderInstance["Note"];
																	if(ReaderInstance["UpDownStop"] != null)
																		 ins.Instance_Up_Down_Stop = Convert.ToInt32(ReaderInstance["UpDownStop"]);	
																	if(ReaderInstance["LIGDefaultValue"] != null)
																		 ins.Instance_LIG_DefaultValue = Convert.ToInt32(ReaderInstance["LIGDefaultValue"]);
																	if(ReaderInstance["LIGFade"] != null)
																		 ins.Instance_LIG_Fade = Convert.ToInt32(ReaderInstance["LIGFade"]);	
																	if(ReaderInstance["SWIInverse"] != null)
																		 ins.Instance_SWI_Inverse = Convert.ToBoolean(ReaderInstance["SWIInverse"]);	
																	if(ReaderInstance["SWIImpulstionTime"] != null)
																		 ins.Instance_SWI_ImpulsionTime = Convert.ToInt32(ReaderInstance["SWIImpulstionTime"]);
																	if(ReaderInstance["SHUType"] != null)
																		 ins.Instance_SHU_Type = Convert.ToInt32(ReaderInstance["SHUType"]);	
																	if(ReaderInstance["SHUTime"] != null)
																		 ins.Instance_SHU_Time = Convert.ToInt32(ReaderInstance["SHUTime"]);
																	if(ReaderInstance["SHUInitTime"] != null)
																		 ins.Instance_SHU_InitTime = Convert.ToInt32(ReaderInstance["SHUInitTime"]);
																	if(ReaderInstance["SHUNumberOfOutput"] != null)
																		ins.Instance_SHU_NumberOfOutput = Convert.ToInt32(ReaderInstance["SHUNumberOfOutput"]);																	
																}
															}
														}
														ReaderInstance.Close ();
													}
												}
												
//#############################################################  SCENARIO #####################################################################################	
												
												if(ReaderNode["Scenarios"] != null)
												{										
													//On vérifie si le fichier existe
													if(!File.Exists(_DirectoryPath + "/Scenarios/" + ReaderNode["Scenarios"])) 
													{	
														return 0;
													}
													//On vérifie qu'il n'a pas subit de modification
													if(CalculHash(System.IO.File.ReadAllText(_DirectoryPath + "/Scenarios/" + ReaderNode["Scenarios"])) != ReaderNode["Scenarios"])
													{
														return 0;
													}	
													
													//On vient lire le fichier Scenario
													using (XmlTextReader ReaderScenario = new XmlTextReader(_DirectoryPath + "/Scenarios/" + ReaderNode["Scenarios"])) //Ouverture du fichier Param.xml
													{
														sce = node.AddScenarioInNode(ScenarioId);
														ScenarioId++;	
														sce.ScenarioHashCode = ReaderNode["Scenarios"];
														
													    while (ReaderScenario.Read()) //Lecture total du fichier
													    {
															if (ReaderScenario.IsStartElement())
															{
																if(ReaderScenario.Name == "Scenario")
																{															
																	if(ReaderScenario["Name"] != null)
																		 sce.ScenarioName = ReaderScenario["Name"];	
																	if(ReaderScenario["Data"] != null)
																		 sce.ScenarioData = ReaderScenario["Data"];
																	if(ReaderScenario["Note"] != null)
																		 sce.ScenarioNotes = ReaderScenario["Note"];	
																	
//#############################################################  VARIABLE #####################################################################################																		
																	
																	if(ReaderScenario["Variables"] != null)
																	{
																		//On vérifie si le fichier existe
																		if(!File.Exists(_DirectoryPath + "/Variables/" + ReaderScenario["Variables"])) 
																		{	
																			return 0;
																		}
																		//On vérifie qu'il n'a pas subit de modification
																		if(CalculHash(System.IO.File.ReadAllText(_DirectoryPath + "/Variables/" + ReaderScenario["Variables"])) != ReaderScenario["Variables"])
																		{
																			return 0;
																		}
																		
																		//On vient lire le fichier Scenario
																		using (XmlTextReader ReaderVariable = new XmlTextReader(_DirectoryPath + "/Variables/" + ReaderScenario["Variables"])) //Ouverture du fichier Param.xml
																		{
																			vari = sce.AddVariableInScenario(Variable_Id);
																			Variable_Id++;
																			
																		    while (ReaderVariable.Read()) //Lecture total du fichier
																		    {
																				if (ReaderVariable.IsStartElement())
																				{
																					if(ReaderVariable.Name == "Variable")
																					{	
																						if(ReaderVariable["Name"] != null)
																							 vari.VariableName = ReaderVariable["Name"];	
																						if(ReaderVariable["Type"] != null)
																							 vari.VariableType = ReaderVariable["Type"];
																						if(ReaderVariable["DefaultValue"] != null)
																							 vari.VariableDefaultValue = Convert.ToInt32(ReaderVariable["DefaultValue"]);
																						if(ReaderVariable["Note"] != null)
																							 sce.ScenarioNotes = ReaderVariable["Note"];																						
																					}
																				}
																			}
																			ReaderVariable.Close ();
																		}
																	}
																	
//#############################################################  FUNCTION #####################################################################################	
																	
																	if(ReaderScenario["Functions"] != null)
																	{
																		//On vérifie si le fichier existe
																		if(!File.Exists(_DirectoryPath + "/Functions/" + ReaderScenario["Functions"])) 
																		{	
																			return 0;
																		}
																		//On vérifie qu'il n'a pas subit de modification
																		if(CalculHash(System.IO.File.ReadAllText(_DirectoryPath + "/Functions/" + ReaderScenario["Functions"])) != ReaderScenario["Functions"])
																		{
																			return 0;
																		}		
																		
																		//On vient lire le fichier Function
																		using (XmlTextReader ReaderFunction = new XmlTextReader(_DirectoryPath + "/Functions/" + ReaderScenario["Functions"])) 
																		{
																			fun = sce.AddFunctionInScenario(Function_Id);
																			Function_Id ++;
																			
																		    while (ReaderFunction.Read()) //Lecture total du fichier
																		    {
																				if (ReaderFunction.IsStartElement())
																				{
																					if(ReaderFunction.Name == "Function")
																					{																						 
																						if(ReaderFunction["Name"] != null)
																							 fun.FunctionName = ReaderFunction["Name"];	
																						if(ReaderFunction["Type"] != null)
																							 fun.FunctionTypeReturn = ReaderFunction["Type"];																						
																						if(ReaderFunction["Data"] != null)
																							 fun.FunctionData = ReaderFunction["Data"];
																						if(ReaderFunction["Note"] != null)
																							 fun.FunctionNote = ReaderFunction["Note"];	
																						if(ReaderFunction["InitFunction"] != null)
																							 fun.InitFunction = Convert.ToBoolean(ReaderFunction["InitFunction"]);		
																						if(ReaderFunction["TypeArgs1"] != null)
																							 fun.FunctionTypeArg1 = ReaderFunction["TypeArgs1"];	
																						if(ReaderFunction["TypeArgs2"] != null)
																							 fun.FunctionTypeArg2 = ReaderFunction["TypeArgs2"];
																						if(ReaderFunction["TypeArgs3"] != null)
																							fun.FunctionTypeArg3 = ReaderFunction["TypeArgs3"];	
																						if(ReaderFunction["TypeArgs4"] != null)
																							 fun.FunctionTypeArg4 = ReaderFunction["TypeArgs4"];	
																						if(ReaderFunction["TypeArgs5"] != null)
																							 fun.FunctionTypeArg5 = ReaderFunction["TypeArgs5"];	
																						if(ReaderFunction["TypeArgs6"] != null)
																							 fun.FunctionTypeArg6 = ReaderFunction["TypeArgs6"];
																						if(ReaderFunction["NameArgs1"] != null)
																							 fun.FunctionNameArg1 = ReaderFunction["NameArgs1"];	
																						if(ReaderFunction["NameArgs2"] != null)
																							 fun.FunctionNameArg2 = ReaderFunction["NameArgs2"];		
																						if(ReaderFunction["NameArgs3"] != null)
																							 fun.FunctionNameArg3 = ReaderFunction["NameArgs3"];	
																						if(ReaderFunction["NameArgs4"] != null)
																							 fun.FunctionNameArg4 = ReaderFunction["NameArgs4"];
																						if(ReaderFunction["NameArgs5"] != null)
																							 fun.FunctionNameArg5 = ReaderFunction["NameArgs5"];	
																						if(ReaderFunction["NameArgs6"] != null)
																							 fun.FunctionNameArg6 = ReaderFunction["NameArgs6"];																									
																					}
																				}
																			}	
																			ReaderFunction.Close();
																		}
																	}
																}
															}
														}
														ReaderScenario.Close();
													}
												}
												
//#############################################################  CUSTOMER #####################################################################################													
												
												if(ReaderNode["Customers"] != null)
												{
													//On vérifie si le fichier existe
													if(!File.Exists(_DirectoryPath + "/Customers/" + ReaderNode["Customers"])) 
													{	
														return 0;
													}
													//On vérifie qu'il n'a pas subit de modification
													if(CalculHash(System.IO.File.ReadAllText(_DirectoryPath + "/Customers/" + ReaderNode["Customers"])) != ReaderNode["Customers"])
													{
														return 0;
													}
													
													//On vient lire le fichier Node
													using (XmlTextReader ReaderCustomer = new XmlTextReader(_DirectoryPath + "/Customers/" + ReaderNode["Customers"])) //Ouverture du fichier Param.xml
													{	
														cus = node.AddCustomerInNode(CustomerId);
														CustomerId++;
														
													    while (ReaderCustomer.Read()) //Lecture total du fichier
													    {
															if (ReaderCustomer.IsStartElement())
															{
																if(ReaderCustomer.Name == "Customer")
																{	
																	if(ReaderCustomer["Name"] != null)
																		 cus.CustomerName = ReaderCustomer["Name"];	
																	if(ReaderCustomer["Data"] != null)
																		 cus.CustomerData = ReaderCustomer["Data"];																						
																	if(ReaderCustomer["Note"] != null)
																		 cus.CustomerNote = ReaderCustomer["Note"];
																	if(ReaderCustomer["Use"] != null)
																		 cus.CustomerUse = Convert.ToBoolean(ReaderCustomer["Use"]);	
																	if(ReaderCustomer["ScenarioId"] != null)
																	{
																		 //fun.FunctionNote = ReaderCustomer["ScenarioId"];	
																		foreach(Scenario sce_ in node.ReturnListScenario())
																		{
																			if(sce_.ScenarioHashCode == ReaderCustomer["ScenarioId"])
																			{
																				cus.ScenarioId = sce_.ScenarioId;
																			}
																		}
																	}
																}
															}
														}
														ReaderCustomer.Close();
													}
												}
												
//#############################################################  NETWORK #####################################################################################	
												
												if(ReaderNode["Networks"] != null)
												{
													//On vérifie si le fichier existe
													if(!File.Exists(_DirectoryPath + "/Networks/" + ReaderNode["Networks"])) 
													{	
														return 0;
													}
													//On vérifie qu'il n'a pas subit de modification
													if(CalculHash(System.IO.File.ReadAllText(_DirectoryPath + "/Networks/" + ReaderNode["Networks"])) != ReaderNode["Networks"])
													{
														return 0;
													}
													
													//On vient lire le fichier Node
													using (XmlTextReader ReaderNetwork = new XmlTextReader(_DirectoryPath + "/Networks/" + ReaderNode["Networks"])) //Ouverture du fichier Param.xml
													{	
														net = node.AddNetworkInNode(Network_Id);
														Network_Id++;
														
													    while (ReaderNetwork.Read()) //Lecture total du fichier
													    {
															if (ReaderNetwork.IsStartElement())
															{
																if(ReaderNetwork.Name == "Network")
																{	
																	if(ReaderNetwork["Type"] != null)
																		 net.Network_Type = ReaderNetwork["Type"];	
																	if(ReaderNetwork["Note"] != null)
																		 net.Network_Note = ReaderNetwork["Note"];		
																	
//#############################################################  BOARDS #####################################################################################																		
																	
																	if(ReaderNetwork["Boards"] != null)
																	{
																		//On vérifie si le fichier existe
																		if(!File.Exists(_DirectoryPath + "/Boards/" + ReaderNetwork["Boards"])) 
																		{	
																			return 0;
																		}
																		//On vérifie qu'il n'a pas subit de modification
																		if(CalculHash(System.IO.File.ReadAllText(_DirectoryPath + "/Boards/" + ReaderNetwork["Boards"])) != ReaderNetwork["Boards"])
																		{
																			return 0;
																		}	
																		
																		//On vient lire le fichier Node
																		using (XmlTextReader ReaderBoard = new XmlTextReader(_DirectoryPath + "/Boards/" + ReaderNetwork["Boards"])) //Ouverture du fichier Param.xml
																		{
																			boa = net.AddBoardInNetwork(Board_Id);
																			Board_Id++;
																			
																		    while (ReaderBoard.Read()) //Lecture total du fichier
																		    {
																				if (ReaderBoard.IsStartElement())
																				{
																					if(ReaderBoard.Name == "Board")
																					{																				
																						if(ReaderBoard["Name"] != null)
																							 boa.Board_Name = ReaderBoard["Name"];	
																						if(ReaderBoard["Type"] != null)
																							 boa.Board_Type = ReaderBoard["Type"];	
																						if(ReaderBoard["I2C0"] != null)
																							 boa.Board_I2C_0 = Convert.ToInt32(ReaderBoard["I2C0"]);	
																						if(ReaderBoard["I2C1"] != null)
																							 boa.Board_I2C_1 = Convert.ToInt32(ReaderBoard["I2C1"]);
																						if(ReaderBoard["OneWireMac"] != null)
																							 boa.Board_1Wire_Mac = ReaderBoard["OneWireMac"];	
																						if(ReaderBoard["OneWirePrecision"] != null)
																							 boa.Board_1Wire_Precision = ReaderBoard["OneWirePrecision"];	
																						if(ReaderBoard["Note"] != null)
																							 boa.Board_Note = ReaderBoard["I2NoteC1"];	
																						
//#############################################################  PIN #####################################################################################	
																						
																						if(ReaderBoard["Pins"] != null)
																						{
																							//On vérifie si le fichier existe
																							if(!File.Exists(_DirectoryPath + "/Pins/" + ReaderBoard["Pins"])) 
																							{	
																								return 0;
																							}
																							//On vérifie qu'il n'a pas subit de modification
																							if(CalculHash(System.IO.File.ReadAllText(_DirectoryPath + "/Pins/" + ReaderBoard["Pins"])) != ReaderBoard["Pins"])
																							{
																								return 0;
																							}	
																							
																							//On vient lire le fichier Node
																							using (XmlTextReader ReaderPin = new XmlTextReader(_DirectoryPath + "/Pins/" + ReaderBoard["Pins"])) //Ouverture du fichier Param.xml
																							{
																								pin = boa.AddPinInBoard(Pin_Id);
																								Pin_Id++;	
																								
																							    while (ReaderPin.Read()) //Lecture total du fichier
																							    {
																									if (ReaderPin.IsStartElement())
																									{
																										if(ReaderPin.Name == "Pin")
																										{	
																											if(ReaderPin["Name"] != null)
																												 pin.Pin_Name = ReaderPin["Name"];	
																											if(ReaderPin["Number"] != null)
																												 pin.Pin_Number = Convert.ToInt16(ReaderPin["Number"]);	
																											if(ReaderPin["Direction"] != null)
																												 pin.Pin_Direction = ReaderPin["Direction"];	
																											if(ReaderPin["FallBack"] != null)
																												 pin.Pin_FallbackValue = Convert.ToBoolean(ReaderPin["FallBack"]);
																											
																											foreach(Instance ins_ in node.ReturnListInstance())
																											{
																												if(ins_.Instance_HashCode == ReaderPin["InstanceId"])
																												{
																													pin.Instance_Id = ins_.Instance_Id;
																												}
																											}
																										}
																									}
																								}
																								ReaderPin.Close ();
																							}
																						}
																					}
																				}
																			}
																			ReaderBoard.Close ();
																		}																			
																	}
																}
															}
														}
														ReaderNetwork.Close ();
													}
												}	
											}
										}
									}
									ReaderNode.Close ();
								}
							}				
						}
					}
				}
				ReaderProject.Close ();	
				mainwindow.UpdateStatusBar();
				mainwindow.UpdateEplorerTreeView(); //Mise à jour de l'explorer projet
				mainwindow.InitPanedAndMouvementAuthor(); 	
				mainwindow.UpdateComboboxSelectUsb();
				UpdateInstanceUsed();
			}
			if(Directory.Exists(_DirectoryPath)) 
			{				
				DirectoryInfo directory = new DirectoryInfo(_DirectoryPath);
				directory.Delete(true);
			}		
			
			
			CopyProject("HTOpenProject",_Path);
			return 1;
		}
		
//################# Return Customer.ino ###############################################
		
		//Fonction ReplaceData
		//Fonction permettant de construire le fichier customer.ino
		public string ReplaceData(string NewText, Int32 ProjectId, Int32 NodeId, Int32 _CustomerId)
		{
			string NodeType = "";
			string StringScenario = param.ParamT("CE_ErrorScenario");
			string StringVariable = "";
			string StringFunction = "";
			
			foreach(Project Pro in ListProject)
			{
				if(Pro.Project_Id == ProjectId)
				{
					NewText = NewText.Replace("[PROJECTNAME]",Pro.Project_Name);
					NewText = NewText.Replace("[PROJECTAUTHOR]",Pro.Project_Author);
					NewText = NewText.Replace("[PROJECTDATETIMECREATION]",Pro.Project_CreationDateAndTime.ToString());
					NewText = NewText.Replace("[PROJECTDATETIMEMODIFICATION]",Pro.Project_ModificationDateAndTime.ToString());
					NewText = NewText.Replace("[PROJECTVERSION]",Pro.Project_Version.ToString());
					NewText = NewText.Replace("[PROJECTNOTE]",Pro.Project_Note.Replace("\n","\n//"));
					//CountNewLine(Pro.Project_Note.Replace("\n","\n//"));
					
				}
				foreach(Node nod in Pro.ReturnListNode())
				{
					if(nod.Node_Id == NodeId)
					{
						//Type de SMB
						if(nod.Node_Type == param.ParamP("NP_SMBv00"))
						{
							NodeType = "2";
						}
						else if(nod.Node_Type == param.ParamP("NP_SMBv01"))
						{
							NodeType = "6";
						}
						NewText = NewText.Replace("[NODETYPE]",NodeType);
						NewText = NewText.Replace("[NODENAME]",nod.Node_Name);
						
						ReturnConfBoard(nod);
						/*
						NewText = NewText.Replace("[COUNTIN4DIM4]",CountIN4DIM4.ToString());
						NewText = NewText.Replace("[COUNTIN8R8]",Count8IN8R.ToString());
						NewText = NewText.Replace("[COUNT16IN]",Count16IN.ToString());
						NewText = NewText.Replace("[COUNT8R]",Count8R.ToString());
						*/
						for(int i=0;i<IndexCountBoard;i++)
						{
							if(TypeBoard[i] != null && TypeBoard[i] != "")
							{
								NewText = NewText.Replace("[COUNT" + TypeBoard[i] + "]",CountBoard[i].ToString());
							}
						}
						
						ReturnConfInstance(nod);
						NewText = NewText.Replace("[COUNTLIGHTING]",CountLighting.ToString());
						NewText = NewText.Replace("[COUNTSWITCH]",CountSwitch.ToString());
						NewText = NewText.Replace("[COUNTSHUTTER]",CountShutter.ToString());
						ReturnConfTemp(nod);
						NewText = NewText.Replace("[COUNTTEMP]",CountTemp.ToString());
						
						NewText = NewText.Replace("[NODEIP]",nod.Node_IP.Replace(".",","));
						NewText = NewText.Replace("[NODEGWIP]",nod.Node_GTWIP.Replace(".",","));
						NewText = NewText.Replace("[NODEMAC]","0x" + nod.Node_Mac.Replace("-",",0x"));
						
						NewText = NewText.Replace("[CONFIGBOARD]",TextConfigBoard);

						
						NewText = NewText.Replace("[CONFIGSWITCH]",TextConfigSwitch);
						NewText = NewText.Replace("[CONFIGLIGHTING]",TextConfigLighting);
						NewText = NewText.Replace("[CONFIGSHUTTER]",TextConfigShutter);
						NewText = NewText.Replace("[CONFIGTEMPERATURE]",TextConfigTemp);
						
						NewText = NewText.Replace("[PREUPDATE]",TextPreUpdate);
						NewText = NewText.Replace("[POSTUPDATE]",TextPostUpdate);
												
						foreach(Customer cus in nod.ReturnListCustomer())
						{	
							if(cus.CustomerId == _CustomerId)
							{
								foreach(Scenario sce in nod.ReturnListScenario())
								{
									if(sce.ScenarioId == cus.ScenarioId)
									{
										StringScenario = sce.ScenarioData;
										foreach(Variable vari in sce.ReturnListVariable())
										{
											if(vari.VariableType == "bool" && vari.VariableDefaultValue == 0)
											{
												StringVariable = StringVariable + vari.VariableType + " " + vari.VariableName + " = " + "false;" + " //" + vari.VariableNote + "\n";
											}
											else if(vari.VariableType == "bool" && vari.VariableDefaultValue == 1)
											{
												StringVariable = StringVariable + vari.VariableType + " " + vari.VariableName + " = " + "true;" + " //" + vari.VariableNote + "\n";
											}	
											else
											{
												StringVariable = StringVariable + vari.VariableType + " " + vari.VariableName + " = " + vari.VariableDefaultValue + "; //" + vari.VariableNote + "\n";
											}
										}
										foreach(Function fun in sce.ReturnListFunction())
										{
											string Args1 = "";
											string Args2 = "";
											string Args3 = "";
											string Args4 = "";
											string Args5 = "";
											string Args6 = "";
											
											if(fun.FunctionNameArg1 != "")
											{
												Args1 = fun.FunctionTypeArg1 + " " + fun.FunctionNameArg1;
											}
											if(fun.FunctionNameArg2 != "")
											{
												Args2 = "," + fun.FunctionTypeArg2 + " " + fun.FunctionNameArg2;
											}
											if(fun.FunctionNameArg3 != "")
											{
												Args3 = "," + fun.FunctionTypeArg3 + " " + fun.FunctionNameArg3;
											}
											if(fun.FunctionNameArg4 != "")
											{
												Args4 = "," + fun.FunctionTypeArg4 + " " + fun.FunctionNameArg4;
											}
											if(fun.FunctionNameArg5 != "")
											{
												Args5 = "," + fun.FunctionTypeArg5 + " " + fun.FunctionNameArg5;
											}
											if(fun.FunctionNameArg6 != "")
											{
												Args6 = "," + fun.FunctionTypeArg6 + " " + fun.FunctionNameArg6;
											}											
											
											StringFunction = StringFunction + "//" + fun.FunctionNote + "\n";
											StringFunction = StringFunction + fun.FunctionTypeReturn + " " + fun.FunctionName + "(" + Args1 + Args2 + Args3 + Args4 + Args5 + Args6 + ")" + "\n";
											StringFunction = StringFunction + "{" + "\n";
											StringFunction = StringFunction + fun.FunctionData + "\n";
											StringFunction = StringFunction + "}" + "\n\n";
										}										
									}
								}
							}
						}
						NewText = NewText.Replace("[SCENARIO]",StringScenario);	
						NewText = NewText.Replace("[VARIABLE]",StringVariable);
						NewText = NewText.Replace("[FUNCTION]",StringFunction);
						
					}
				}
			}   
			return NewText;
		}
		
			//Fonction ReturnConfBoard
			//Fonction permettant de compter les cartes de chaque type
			public void ReturnConfBoard(Node nod)
			{		
				CountIN4DIM4 = 0;
				Count8IN8R = 0;
				Count16IN = 0;
				Count8R = 0;	
				TextConfigBoard = "";
				TextPreUpdate = "";
				TextPostUpdate = "";
			
				CountBoard = new int[ListBoards.Count];
				TypeBoard = new string[ListBoards.Count];
			
				//Permet de savoir quel instance on était traité pour les volets
				int CountInstanceTraity = 0;
				InstanceTraity = new int[nod.ReturnListInstance().Count];
				
				IndexCountBoard = 0;
				foreach(Boards boas in ListBoards)
				{
					if(boas.NetworkType == "I2C")
					{
						TypeBoard[IndexCountBoard] = boas.Type;
					}
					IndexCountBoard++;
				}			
			
				bool TreateInstance = true;
				string PRESWI = "";
				string PRELIG = "";
				string POSTLIG = "";
				string POSTSHU = "";	
				int FallBackValue = 0;
				
				foreach(Network net in nod.ReturnListNetwork())
				{
					foreach(Board boa in net.ReturnListBoard())
					{
						for(int i=0;i<IndexCountBoard;i++)
						{
							if(boa.Board_Type == TypeBoard[i])
							{
								//##### MISE A JOUR TEXTE CONFIGBOARD #####							
								foreach(Boards boas in ListBoards)
								{
									if(boas.Type == boa.Board_Type)
									{
										PRESWI = boas.FunctionNamePRESWI;
										PRELIG = boas.FunctionNamePRELIG;
										POSTLIG = boas.FunctionNamePOSTLIG;
										POSTSHU = boas.FunctionNamePOSTSHU;										
									}
								
									if(boas.Type == boa.Board_Type)
									{
										for(int j=0;j<boas.NumberOfInputs;j++)
										{
											if(boa.Pin_[j].Pin_FallbackValue == true)
											{
												FallBackValue = FallBackValue + (int)Math.Pow((double)2,(double)j);
											}
										}
									}
								}
							
								if(boa.Board_Type == "IN8R8")
								{
									TextConfigBoard = TextConfigBoard + "" + boa.Board_Type + "[" + CountBoard[i] + "].init(" + boa.Board_I2C_0 + "," + FallBackValue + ");\n\t";
								}
								else if(boa.Board_Type == "IN4DIM4")
								{
									TextConfigBoard = TextConfigBoard + "" + boa.Board_Type + "[" + CountBoard[i] + "].init(" + boa.Board_I2C_0 + ");\n\t";
								}
								else if(boa.Board_Type == "IN16")
								{
									TextConfigBoard = TextConfigBoard + "" + boa.Board_Type + "[" + CountBoard[i] + "].init(" + boa.Board_I2C_0 + "," + boa.Board_I2C_1 + ");\n\t";
								}
								else if(boa.Board_Type == "R8")
								{
									TextConfigBoard = TextConfigBoard + "" + boa.Board_Type + "[" + CountBoard[i] + "].init(" + boa.Board_I2C_0 + ");\n\t";
								}								
							
								//##### MISE A JOUR TEXTE PREUPDATE #####
								foreach(Pin pin in boa.ReturnListPin())
								{
									foreach(Instance ins in nod.ReturnListInstance())
									{
										if(pin.Instance_Id == ins.Instance_Id)
										{
											TreateInstance = true;
											for(int j=0;j<CountInstanceTraity;j++)
											{
												if(InstanceTraity[j]==ins.Instance_Id)
												{
													TreateInstance = false;
												}
											}										
											
											if(TreateInstance)
											{
												InstanceTraity[CountInstanceTraity] = ins.Instance_Id;
												CountInstanceTraity++;
											
												if(PRESWI != "" && pin.Pin_Direction == param.ParamP("Direction1"))
												{
													TextPreUpdate = TextPreUpdate + PRESWI +"(" + ins.Instance_Name + "," + CountBoard[i] + "," + pin.Pin_Number + ");\n\t";
												}
												if(PRELIG != "" && pin.Pin_Direction == param.ParamP("Direction2") && ins.Instance_Type != param.ParamP("InstShutterName"))
												{
													TextPreUpdate = TextPreUpdate + PRELIG +"(" + ins.Instance_Name + "," + CountBoard[i] + "," + pin.Pin_Number + ");\n\t";
												}
											
												if(POSTLIG != "" && pin.Pin_Direction == param.ParamP("Direction2") && ins.Instance_Type != param.ParamP("InstShutterName"))
												{
													TextPostUpdate = TextPostUpdate + POSTLIG +"(" + ins.Instance_Name + "," + CountBoard[i] + "," + pin.Pin_Number + ");\n\t";
												}										
											
												string Pin_0 = "ERROR";
												string Pin_1 = "ERROR";
												string Pin_2 = "ERROR";
												string PinDirection = "";	
											
												if(ins.Instance_Type == param.ParamP("InstShutterName"))
												{
													foreach(Pin pin_1 in boa.ReturnListPin())
													{	
														PinDirection = pin_1.Pin_Direction;
														if(ins.Pin_Id_0 == pin_1.Pin_Id)
														{
															Pin_0 = pin_1.Pin_Number.ToString();
														}	
														if(ins.Pin_Id_1 == pin_1.Pin_Id)
														{
															Pin_1 = pin_1.Pin_Number.ToString();
														}												
														if(ins.Pin_Id_2 == pin_1.Pin_Id)
														{
															Pin_2 = pin_1.Pin_Number.ToString();
														}														
													}
												}
			
												if(POSTSHU != "" && PinDirection == param.ParamP("Direction2") && ins.Instance_SHU_NumberOfOutput == 2)
												{
													TextPostUpdate = TextPostUpdate + POSTSHU +"(" + ins.Instance_Name + "," + CountBoard[i] + "," + Pin_0 + "," + Pin_1 + ");\n\t";
												}
												if(POSTSHU != "" && PinDirection == param.ParamP("Direction2") && ins.Instance_SHU_NumberOfOutput == 3)
												{
													TextPostUpdate = TextPostUpdate + POSTSHU +"(" + ins.Instance_Name + "," + CountBoard[i] + "," + Pin_0 + "," + Pin_1 + "," + Pin_2 + ");\n\t";
												}
											}
										}
									}
								}
								CountBoard[i]++;
							}
						}
					}	
				}
			}		
		
			//Fonction ReturnConfInstance
			//Fonction permettant de compter les instances de chaque type
			public void ReturnConfInstance(Node nod)
			{
				CountLighting = 0;
				CountSwitch = 0;
				CountShutter = 0;	
				TextConfigLighting = "";
				TextConfigSwitch = "";
				TextConfigShutter = "";			
				
				foreach(Instance ins in nod.ReturnListInstance())
				{	
					if(ins.Instance_Type == param.ParamP("InstShutterName") && ins.Instance_SHU_InitTime > 0)
				  	{		
						TextConfigShutter = TextConfigShutter + "#define " + "POSITION_" + ins.Instance_Name.ToUpper() + " " + ins.Instance_SHU_InitTime + "\n\t";
					}
				}
				TextConfigShutter = TextConfigShutter + "\n\t";
			
				foreach(Instance ins in nod.ReturnListInstance())
				{
					if(ins.Instance_Type == param.ParamP("InstLightingName"))
				  	{
						TextConfigLighting = TextConfigLighting + "LIGHTING[" + CountLighting + "].init(\"" + ins.Instance_Name + "\"," + ins.Instance_LIG_DefaultValue + ", " + ins.Instance_LIG_Fade + ");\n\t";
						TextConfigLighting = TextConfigLighting + "#define " + ins.Instance_Name + "\t" + CountLighting + "\n\t";

						CountLighting++;
					}
					else if(ins.Instance_Type == param.ParamP("InstSwitchName"))
				  	{
						string Inverse = "";
						if(ins.Instance_SWI_Inverse)
						{
							Inverse = "LOW";
						}
						else
						{
							Inverse = "HIGH";
						}
						TextConfigSwitch = TextConfigSwitch + "SWITCH[" + CountSwitch + "].init(\"" + ins.Instance_Name + "\"," + Inverse + "," + ins.Instance_SWI_ImpulsionTime.ToString() + ");\n\t";
						TextConfigSwitch = TextConfigSwitch + "#define " + ins.Instance_Name + "\t" + CountSwitch + "\n\t";
							//TextConfigSwitch = TextConfigSwitch + "#define " + param.ParamT("CE_Pulse") + "(" + ins.Instance_Name + ")\tSWITCH[" + ins.Instance_Name + "].isPulse()\n\t\t";
							//TextConfigSwitch = TextConfigSwitch + "#define " + param.ParamT("CE_DoublePulse") + "(" + ins.Instance_Name + ")\tSWITCH[" + ins.Instance_Name + "].isDoublePulse()\n\t\t";
							//TextConfigSwitch = TextConfigSwitch + "#define " + param.ParamT("CE_On") + "(" + ins.Instance_Name + ")\tSWITCH[" + ins.Instance_Name + "].isOn()\n\t\t";
							//TextConfigSwitch = TextConfigSwitch + "#define " + param.ParamT("CE_OSR") + "(" + ins.Instance_Name + ")\tSWITCH[" + ins.Instance_Name + "].isOnOSR()\n\t\t";					
							//TextConfigSwitch = TextConfigSwitch + "#define " + param.ParamT("CE_Off") + "(" + ins.Instance_Name + ")\tSWITCH[" + ins.Instance_Name + "].isOff()\n\t\t";
							//TextConfigSwitch = TextConfigSwitch + "#define " + param.ParamT("CE_OSF") + "(" + ins.Instance_Name + ")\tSWITCH[" + ins.Instance_Name + "].isOnOSF()\n\n\t";						
						CountSwitch++;
					}
					else if(ins.Instance_Type == param.ParamP("InstShutterName"))
				  	{
						string ShutterType = "";
						if(ins.Instance_SHU_Type == 0)
						{
							ShutterType = "TYPE_PULSE";
						}
						else if(ins.Instance_SHU_Type == 1)
						{
							ShutterType = "TYPE_PERMANENT";
						}
						else if(ins.Instance_SHU_Type == 2)
						{
							ShutterType = "TYPE_PERMANENT_COMB";
						}					
						
						TextConfigShutter = TextConfigShutter + "SHUTTER[" + CountShutter + "].init(\"" + ins.Instance_Name + "\"," + ShutterType + "," + ins.Instance_SHU_Time.ToString() + ");\n\t";
						TextConfigShutter = TextConfigShutter + "#define " + ins.Instance_Name + "\t" + CountShutter + "\n\t";
					
						if(ins.Instance_SHU_InitTime > 0)
						{
							TextConfigShutter = TextConfigShutter + "SHUTTER[" + CountShutter + "].postCurrent(read16(POSITION_" + ins.Instance_Name.ToUpper() + "));\n\t";
							TextConfigShutter = TextConfigShutter + "Serial.println(read16(POSITION_" + ins.Instance_Name.ToUpper() + "));\n\n\t";
						}
						else
						{
							TextConfigShutter = TextConfigShutter + "\n\t";
						}
						
					
						CountShutter++;
					}				
				}
			}
		
			//Fonction ReturnConfTemp(Node nod)
			//Fonction permettant de retourner le nombre de sonde de température
			public void ReturnConfTemp(Node nod)
			{
				CountTemp = 0;
				TextConfigTemp = "";
				string PrecisionTemp = "";
				
				foreach(Network net in nod.ReturnListNetwork())
				{
					foreach(Board boa in net.ReturnListBoard())
					{
						if(boa.Board_Type == "TEMPERATURE")
						{
							if(boa.Board_1Wire_Precision == "0.1")
							{
								PrecisionTemp = "false";
							}
							else if(boa.Board_1Wire_Precision == "0.5")
							{
								PrecisionTemp = "true";
							}
						
							TextConfigTemp = TextConfigTemp + "TEMPERATURE[" + CountTemp + "].init(\"" + boa.Board_Name + "\",0x" + boa.Board_1Wire_Mac.Replace("-",",0x") + "," + PrecisionTemp + ");\n\t";
							TextConfigTemp = TextConfigTemp + "#define\t" + boa.Board_Name + "\t" + CountTemp + "\n\t";
							CountTemp++;
						}
					}
				}
			}		
			
//################# Copie des fichiers sources #############################################		
		
		//Fonction CopyFiles
		//Fonction permettant de copier tous les fichiers code source dans un dossier de destination
		public bool CopyFiles(Int32 _ProjectID, Int32 _NodeId)
		{	
			string fileName = "";
			string destFile = "";
			string ListFunction = "";
			string CustomerName = "";
			
			List<string> ProjectLibraries = new List<string>();
			ProjectLibraries.Add("/tinyFAT");
			ProjectLibraries.Add("/ethercard");
			ProjectLibraries.Add("/xPL");
			ProjectLibraries.Add("/Wire");
			ProjectLibraries.Add("/bitlash");
			ProjectLibraries.Add("/DS1307new");
			ProjectLibraries.Add("/DS2482");	
			
			foreach(Project pro in ListProject)
			{
				foreach(Node node in pro.ReturnListNode())
				{
					foreach(Customer cus in node.ReturnListCustomer())
					{
						if(cus.CustomerUse)
						{	
							if(cus.ScenarioId == 0)
							{
								mainwindow.AddLineOutput(param.ParamI("OutputError"),"ChooseAScenario");
								mainwindow.ActiveCompileAndLoadButtonCompilation(false);							
								return false;
							}							
						}
					}
					
					if(pro.Project_Id == _ProjectID && node.Node_Id == _NodeId && Directory.Exists(pro.Project_SavePath))
					{
						string TargetPath = pro.Project_SavePath + param.ParamP("FolderTargetFirmware");
						
						if(Directory.Exists(TargetPath))
						{
							DirectoryInfo DirectoryFirmwareNode = new DirectoryInfo(TargetPath);
							DirectoryFirmwareNode.Delete(true);							
						}
						
						System.IO.Directory.CreateDirectory(TargetPath);
						
				        if (System.IO.Directory.Exists(Environment.CurrentDirectory + param.ParamP("FolderSourceFirmware")))
				        {
				            string[] files = System.IO.Directory.GetFiles(Environment.CurrentDirectory + param.ParamP("FolderSourceFirmware"));
				
				            // Copy the files and overwrite destination files if they already exist.
				            foreach (string s in files)
				            {
				                // Use static Path methods to extract only the file name from the path.
				                fileName = System.IO.Path.GetFileName(s);
								if(fileName != "xplduino_controller.cpp")
								{
					                destFile = System.IO.Path.Combine(TargetPath, fileName);
					                System.IO.File.Copy(s, destFile, true);
								}
				            }
							
							//On vient mettre à jour le fichier "xplduino_controller.cpp" avec les fonctions
							foreach(Customer cus in node.ReturnListCustomer())
							{
								if(cus.CustomerUse)
								{
									foreach(Scenario sce in node.ReturnListScenario())
									{
										if(cus.ScenarioId == sce.ScenarioId)
										{
											foreach(Function fun in sce.ReturnListFunction())
											{
												if(!fun.InitFunction)
												{
													string Args1 = "";
													string Args2 = "";
													string Args3 = "";
													string Args4 = "";
													string Args5 = "";
													string Args6 = "";
												
													if(fun.FunctionNameArg1 != "")
													{
														Args1 = fun.FunctionTypeArg1;
													}
													if(fun.FunctionNameArg2 != "")
													{
														Args2 = "," + fun.FunctionTypeArg2;
													}
													if(fun.FunctionNameArg3 != "")
													{
														Args3 = "," + fun.FunctionTypeArg3;
													}
													if(fun.FunctionNameArg4 != "")
													{
														Args4 = "," + fun.FunctionTypeArg4;
													}
													if(fun.FunctionNameArg5 != "")
													{
														Args5 = "," + fun.FunctionTypeArg5;
													}
													if(fun.FunctionNameArg6 != "")
													{
														Args6 = "," + fun.FunctionTypeArg6;
													}													
													
													ListFunction = ListFunction + fun.FunctionTypeReturn + " " + fun.FunctionName + "(" + Args1 + Args2 + Args3 + Args4 + Args5 + Args6 + ");\n";
													CustomerName = cus.CustomerName;
												}
											}
										}
									}
								}
							}
							
							StreamWriter ControllerFile = new StreamWriter(TargetPath + "/xplduino_controller.cpp", true, System.Text.Encoding.ASCII); 
								ControllerFile.WriteLine(System.IO.File.ReadAllText(Environment.CurrentDirectory + param.ParamP("FolderSourceFirmware") + "/xplduino_controller.cpp").Replace("[PERSONNALFUNCTION]",ListFunction).Replace("[CUSTOMERNAME]",CustomerName));
							ControllerFile.Close(); 
							
//################################# ECRITURE DU FICHIER DEFINE.H ##########################################		
							
						    StreamWriter DefineFile = new StreamWriter(TargetPath + "/define.h", true, System.Text.Encoding.ASCII); 
							
						    DefineFile.WriteLine("#define " + pref.SWIClicActionName+"(name)\t\tSWITCH[name].isPulse()"); 
							DefineFile.WriteLine("#define " + pref.SWIDoubleClicActionName+"(name)\t\tSWITCH[name].isDoublePulse()"); 
							DefineFile.WriteLine("#define " + pref.SWIOnActionName+"(name)\t\tSWITCH[name].isOn()"); 
							DefineFile.WriteLine("#define " + pref.SWIOnFmActionName+"(name)\t\tSWITCH[name].isOnOSR()"); 
							DefineFile.WriteLine("#define " + pref.SWIOffActionName+"(name)\t\tSWITCH[name].isOff()"); 
							DefineFile.WriteLine("#define " + pref.SWIOffFmActionName+"(name)\t\tSWITCH[name].isOnOSF()\n"); 

							DefineFile.WriteLine("#define " + pref.SHUOpenActionName+"(name)\t\tSHUTTER[name].opening()"); 
							DefineFile.WriteLine("#define " + pref.SHUCloseActionName+"(name)\t\tSHUTTER[name].closing()"); 
							DefineFile.WriteLine("#define " + pref.SHUStopActionName+"(name)\t\tSHUTTER[name].stop()"); 	
							DefineFile.WriteLine("#define " + pref.SHUToggleActionName+"(name)\t\tSHUTTER[name].toggle()\n"); 	

							DefineFile.WriteLine("#define " + pref.LIGToggleActionName+"(name)\t\tLIGHTING[name].toggle()"); 
							DefineFile.WriteLine("#define " + pref.LIGTuneActionName+"(name)\t\tLIGHTING[name].tune()"); 
							DefineFile.WriteLine("#define " + pref.LIGStopActionName+"(name)\t\tLIGHTING[name].stop()\n"); 	
							DefineFile.WriteLine("#define " + pref.LIGSetActionName+"(name,value)\t\tLIGHTING[name].new_setpoint(value)\n"); 	
							
							DefineFile.WriteLine("#define " + pref.TEMPGetValue+"(name)\t\tTEMPERATURE[name].getValue()\n"); 	
							
						    DefineFile.Close(); 
							
//################################# ECRITURE DU FICHIER CONFIG.H ##########################################								
							
							StreamWriter ConfigFile = new StreamWriter(TargetPath + "/config.h", true, System.Text.Encoding.ASCII); 
							
							ConfigFile.WriteLine("#ifndef config_h"); 
							ConfigFile.WriteLine("#define config_h\n"); 
							
							if(node.Node_DHCP)
							{
								ConfigFile.WriteLine("\t #define STATIC " + 0 + "  // set to 1 to disable DHCP (adjust myip/gwip values below)\n");
							}
							else
							{
								ConfigFile.WriteLine("\t #define STATIC " + 1 + "  // set to 1 to disable DHCP (adjust myip/gwip values below)\n");
							}
							
							if(node.Node_WebServer)
							{
								ConfigFile.WriteLine("\t #define WEBSERVER " + 1 + "  // set to 1 to enable webserver (need a uSD in SMB)\n");
							}
							else
							{
								ConfigFile.WriteLine("\t #define WEBSERVER " + 0 + " // set to 1 to enable webserver (need a uSD in SMB)\n");
							}							

							
							foreach(Debug deb in node.ReturnListDebug())
							{
								if(deb.Value)
								{
									ConfigFile.WriteLine("\t #define " + deb.Name + "");
								}
							}
							
							ConfigFile.WriteLine("");
						    ConfigFile.WriteLine("\t #define XPL_PORT 3865");
						    ConfigFile.WriteLine("\t #define T_CMND 1");
						    ConfigFile.WriteLine("\t #define T_STAT 2");
						    ConfigFile.WriteLine("\t #define T_TRIG 3");
						
						
						    ConfigFile.WriteLine("\t #define STATUS_LED      26");
						    ConfigFile.WriteLine("\t #define FAULT_LED       30");							
							
							
							ConfigFile.WriteLine("#endif"); 
							ConfigFile.Close();
							
//################################# ECRITURE DU FICHIER CUSTOMER.INO ##########################################		
							
							foreach(Customer cus in node.ReturnListCustomer())
							{
								if(cus.CustomerUse && cus.ScenarioId != 0)
								{
									StreamWriter CustomerFile = new StreamWriter(TargetPath + "/xplduino_controller.cpp", true, System.Text.Encoding.ASCII);
									CustomerFile.WriteLine(ReplaceData(cus.CustomerData,pro.Project_Id,node.Node_Id,cus.CustomerId));
									CustomerFile.Close(); 
								}
							}					
				        }
				        else
				        {
				            return false;
				        }
											
						return true;
					}
				}
			}
			return true;	
		}		
		
//################# Compilation des fichiers #################################
		
		//Fonction CompileFile
		//Fonction permettant de compiler les fichier
		public bool CompileFile(Int32 _ProjectID, Int32 _NodeId)
		{		
			bool CompilationIsCorrect = false;
			string SeparateStar = "\n\n*************************************************************************************************************************************************************";
			string SeparateStarEnd = "*************************************************************************************************************************************************************\n";
			
			mainwindow.UpdateProgressBar(0);
			
			if(!CopyFiles(_ProjectID,_NodeId))
			{
				return false;
			}

			
			mainwindow.UpdateProgressBar(10);
			
			List<string> ListLibraries = new List<string>();
			ListLibraries.Add("/hardware/arduino/cores/arduino -I");
			ListLibraries.Add("/hardware/arduino/variants/xplduino -I");
			ListLibraries.Add("/libraries/tinyFAT -I");
			ListLibraries.Add("/libraries/ethercard -I");
			ListLibraries.Add("/libraries/xPL -I");
			ListLibraries.Add("/libraries/Wire -I");
			ListLibraries.Add("/libraries/bitlash -I");
			ListLibraries.Add("/libraries/DS1307new -I");
			ListLibraries.Add("/libraries/DS2482 ");
			  	
			List<string> ProjectLibraries = new List<string>();
			ProjectLibraries.Add("/tinyFAT");
				ProjectLibraries.Add("/tinyFAT/utility");
			ProjectLibraries.Add("/ethercard");
				ProjectLibraries.Add("/ethercard/utility");
			ProjectLibraries.Add("/xPL");
				ProjectLibraries.Add("/xPL/utility");
			ProjectLibraries.Add("/Wire");
				ProjectLibraries.Add("/Wire/utility");
			ProjectLibraries.Add("/bitlash");
				ProjectLibraries.Add("/bitlash/utility");
			ProjectLibraries.Add("/DS1307new");
				ProjectLibraries.Add("/DS1307new/utility");
			ProjectLibraries.Add("/DS2482");			
				ProjectLibraries.Add("/DS2482/utility");
					
	        if (System.IO.Directory.Exists(Environment.CurrentDirectory + param.ParamP("FolderHardware")))
	        {	
				string SavePath = "";
				string ShellCommande = "";
				string ShellArgument = "";
				string Filename = "";
				string ExtensionFile = "";
				
				foreach(Project pro in ListProject)
				{
					foreach(Node node in pro.ReturnListNode())
					{
						if(pro.Project_Id == _ProjectID && node.Node_Id == _NodeId && Directory.Exists(pro.Project_SavePath))
						{		
							CompilLogData = SeparateStar + "\n" + " Generation " + pro.Project_Name + " " + node.Node_Name + "\n" + SeparateStarEnd;
							
							SavePath = pro.Project_SavePath + "/xplduino_controller";
																	
//######################################### On vient compiler les fichier propre au projet ######################################################	
							
				            string[] files = System.IO.Directory.GetFiles(SavePath);
				            foreach (string s in files)
				            {
				                Filename = System.IO.Path.GetFileName(s);								
								ExtensionFile = System.IO.Path.GetExtension(s);
																
								if(ExtensionFile == ".cpp" || ExtensionFile == ".c")
								{
									if(ExtensionFile == ".cpp")
									{
										ShellCommande = Environment.CurrentDirectory + param.ParamP("FolderHardware") + "avr-g++";
										ShellArgument = "-c -g -Os -Wall -fno-exceptions -ffunction-sections -fdata-sections -mmcu=atmega1284p -DF_CPU=20000000L -MMD -DUSB_VID=null -DUSB_PID=null -DARDUINO=102 -I";
									}
									else if(ExtensionFile == ".c")
									{
										ShellCommande = Environment.CurrentDirectory + param.ParamP("FolderHardware") + "avr-gcc";
										ShellArgument = "-c -g -Os -Wall -ffunction-sections -fdata-sections -mmcu=atmega1284p -DF_CPU=20000000L -MMD -DUSB_VID=null -DUSB_PID=null -DARDUINO=102 -I";
									}
									
									foreach(string Library in ListLibraries)
									{
										ShellArgument = ShellArgument + Environment.CurrentDirectory + Library;
									}
									
									ShellArgument = ShellArgument + SavePath + "/" + Filename + " -o " + SavePath + "/" + Filename + ".o";
									
									CompilLogData = CompilLogData + SeparateStar + "\n" + " Generation " + Filename + " : " + ShellCommande + " " + ShellArgument + "\n" + SeparateStarEnd;																						
									
									if(Filename == "xplduino_controller.cpp")
									{
										ShellTransfert(ShellCommande,ShellArgument,true,Filename);
									}
									else
									{
										ShellTransfert(ShellCommande,ShellArgument,false,Filename);
									}
								}
							}
							
							mainwindow.UpdateProgressBar(30);
						
//######################################### On vient compiler les éléments des libraries propre au projet ######################################################	
						
							foreach(string FolderLibrary in ProjectLibraries)
							{
								if(Directory.Exists(Environment.CurrentDirectory + param.ParamP("FolderLibrary") + FolderLibrary))
								{		
									System.IO.Directory.CreateDirectory(SavePath + FolderLibrary);	
									
									string[] FilesLibrary = System.IO.Directory.GetFiles(Environment.CurrentDirectory + param.ParamP("FolderLibrary") + FolderLibrary);
						            foreach (string s in FilesLibrary)
						            {
						                Filename = System.IO.Path.GetFileName(s);								
										ExtensionFile = System.IO.Path.GetExtension(s);
										
										if(ExtensionFile == ".cpp" || ExtensionFile == ".c")
										{	
											if(ExtensionFile == ".cpp")
											{
												ShellCommande = Environment.CurrentDirectory + param.ParamP("FolderHardware") + "avr-g++";
												ShellArgument = "-c -g -Os -Wall -fno-exceptions -ffunction-sections -fdata-sections -mmcu=atmega1284p -DF_CPU=20000000L -MMD -DUSB_VID=null -DUSB_PID=null -DARDUINO=102 -I";
											}
											else if(ExtensionFile == ".c")
											{
												ShellCommande = Environment.CurrentDirectory + param.ParamP("FolderHardware") + "avr-gcc";
												ShellArgument = "-c -g -Os -Wall -ffunction-sections -fdata-sections -mmcu=atmega1284p -DF_CPU=20000000L -MMD -DUSB_VID=null -DUSB_PID=null -DARDUINO=102 -I";
											}
											
											foreach(string Library in ListLibraries)
											{
												ShellArgument = ShellArgument + Environment.CurrentDirectory + Library;
											}
											
											if(FolderLibrary.IndexOf("utility") == -1)
											{
												ShellArgument = ShellArgument + "-I" + Environment.CurrentDirectory + param.ParamP("FolderLibrary") + FolderLibrary + "/utility ";
											}
											else
											{
												ShellArgument = ShellArgument + "-I" + Environment.CurrentDirectory + param.ParamP("FolderLibrary") + FolderLibrary + " ";
											}
											
											ShellArgument = ShellArgument + Environment.CurrentDirectory + param.ParamP("FolderLibrary") + FolderLibrary + "/" + Filename + " -o " + SavePath + FolderLibrary + "/" + Filename + ".o";
											
											CompilLogData = CompilLogData + SeparateStar + "\n" + " Generation " + Filename + " : " + ShellCommande + " " + ShellArgument + "\n" + SeparateStarEnd;																						
																
											ShellTransfert(ShellCommande,ShellArgument,false,Filename);										
										}
									}
								}
							}
							
							mainwindow.UpdateProgressBar(50);

//######################################### On vient compiler les éléments du core ######################################################	
							
							if(Directory.Exists(Environment.CurrentDirectory + param.ParamP("FolderCore")))
							{	
								string[] FilesCore = System.IO.Directory.GetFiles(Environment.CurrentDirectory + param.ParamP("FolderCore"));
					            foreach (string s in FilesCore)
					            {	
					                Filename = System.IO.Path.GetFileName(s);								
									ExtensionFile = System.IO.Path.GetExtension(s);
																	
									if(ExtensionFile == ".cpp" || ExtensionFile == ".c")
									{	
										if(ExtensionFile == ".cpp")
										{
											ShellCommande = Environment.CurrentDirectory + param.ParamP("FolderHardware") + "avr-g++";
											ShellArgument = "-c -g -Os -Wall -fno-exceptions -ffunction-sections -fdata-sections -mmcu=atmega1284p -DF_CPU=20000000L -MMD -DUSB_VID=null -DUSB_PID=null -DARDUINO=102 -I";
										}
										else if(ExtensionFile == ".c")
										{
											ShellCommande = Environment.CurrentDirectory + param.ParamP("FolderHardware") + "avr-gcc";
											ShellArgument = "-c -g -Os -Wall -ffunction-sections -fdata-sections -mmcu=atmega1284p -DF_CPU=20000000L -MMD -DUSB_VID=null -DUSB_PID=null -DARDUINO=102 -I";
										}
										
										ShellArgument = ShellArgument + Environment.CurrentDirectory + param.ParamP("FolderCore") + " -I" + Environment.CurrentDirectory + param.ParamP("FolderXplduinoVariant") + " ";
										ShellArgument = ShellArgument + Environment.CurrentDirectory + param.ParamP("FolderCore") + "/" + Filename + " -o " + SavePath + "/" + Filename + ".o";
										
										CompilLogData = CompilLogData + SeparateStar + "\n" + " Generation " + Filename + " : " + ShellCommande + " " + ShellArgument + "\n" + SeparateStarEnd;																																
										
										ShellTransfert(ShellCommande,ShellArgument,false,Filename);											
									}									
								}
							}
							
							mainwindow.UpdateProgressBar(60);
	
//######################################### On les zip dans le core.a ######################################################								
							
							if(Directory.Exists(Environment.CurrentDirectory + param.ParamP("FolderCore")))
							{	
								string[] FilesCore = System.IO.Directory.GetFiles(Environment.CurrentDirectory + param.ParamP("FolderCore"));
					            foreach (string s in FilesCore)
					            {	
					                Filename = System.IO.Path.GetFileName(s);	
									ExtensionFile = System.IO.Path.GetExtension(s);
																		
									if(ExtensionFile == ".cpp")
									{
										ShellCommande = Environment.CurrentDirectory + param.ParamP("FolderHardware") + "avr-ar";
										ShellArgument = "rcs ";
										ShellArgument = ShellArgument + SavePath + "/core.a " + SavePath + "/" + Filename + ".o";
										
										CompilLogData = CompilLogData + SeparateStar + "\n" + " Generation " + Filename + " : " + ShellCommande + " " + ShellArgument + "\n" + SeparateStarEnd;																					
										
										ShellTransfert(ShellCommande,ShellArgument,false,Filename);	
									}
								}
							}
							
							mainwindow.UpdateProgressBar(70);
							
//######################################### On créer le .elf du projet ######################################################							
							
							ShellCommande = Environment.CurrentDirectory + param.ParamP("FolderHardware") + "avr-gcc";
							ShellArgument = "-Os -Wl,--gc-sections -mmcu=atmega1284p -o ";
							ShellArgument = ShellArgument + SavePath + "/xplduino_controller.cpp.elf ";
							
				            string[] FilesProject = System.IO.Directory.GetFiles(SavePath);
				            foreach (string s in FilesProject)
				            {
				                Filename = System.IO.Path.GetFileName(s);								
								ExtensionFile = System.IO.Path.GetExtension(s);
																
								if(ExtensionFile == ".o")
								{
									ShellArgument = ShellArgument + SavePath + "/" + Filename + " ";
									CompilLogData = CompilLogData + SeparateStar + "\n" + " Generation " + Filename + " : " + ShellCommande + " " + ShellArgument + "\n" + SeparateStarEnd;																					
								}
							}
							
							foreach(string FolderLibrary in ProjectLibraries)
							{
								if(Directory.Exists(SavePath + FolderLibrary))
								{
						            string[] FilesLibrary = System.IO.Directory.GetFiles(SavePath + FolderLibrary);
						            foreach (string s in FilesLibrary)
						            {
						                Filename = System.IO.Path.GetFileName(s);								
										ExtensionFile = System.IO.Path.GetExtension(s);									
																										
										if(ExtensionFile == ".o")
										{											
											ShellArgument = ShellArgument + SavePath + FolderLibrary + "/" + Filename + " ";
											CompilLogData = CompilLogData + SeparateStar + "\n" + " Generation " + Filename + " : " + ShellCommande + " " + ShellArgument + "\n" + SeparateStarEnd;			
										}
									}
								}
							}
							
							ShellArgument = ShellArgument + SavePath + "/core.a -L" + SavePath + " -lm";
							
							ShellTransfert(ShellCommande,ShellArgument,false,Filename);
							
							mainwindow.UpdateProgressBar(80);
							
//######################################### On créer du .eep du projet ######################################################	
							
							CompilLogData = CompilLogData + SeparateStar + "\n" + " Generation " + "File .eep" + " : " + ShellCommande + " " + ShellArgument + "\n" + SeparateStarEnd;
							
							ShellCommande = Environment.CurrentDirectory + param.ParamP("FolderHardware") + "avr-objcopy";
							ShellArgument = " -O ihex -j .eeprom --set-section-flags=.eeprom=alloc,load --no-change-warnings --change-section-lma .eeprom=0 ";
							ShellArgument = ShellArgument + SavePath + "/xplduino_controller.cpp.elf ";
							ShellArgument = ShellArgument + SavePath + "/xplduino_controller.cpp.eep";
							
							ShellTransfert(ShellCommande,ShellArgument,false,"");	
							
							mainwindow.UpdateProgressBar(90);
							
//######################################### On créer du .hex du projet ######################################################								
							
							CompilLogData = CompilLogData + SeparateStar + "\n" + " Generation " + "File .hex" + " : " + ShellCommande + " " + ShellArgument + "\n" + SeparateStarEnd;
							
							ShellCommande = Environment.CurrentDirectory + param.ParamP("FolderHardware") + "avr-objcopy";
							ShellArgument = " -O ihex -R .eeprom ";
							ShellArgument = ShellArgument + SavePath + "/xplduino_controller.cpp.elf ";
							ShellArgument = ShellArgument + SavePath + "/xplduino_controller.cpp.hex";
							
							ShellTransfert(ShellCommande,ShellArgument,false,"");	
							
							mainwindow.UpdateProgressBar(100);
							
							if(File.Exists(SavePath + "/xplduino_controller.cpp.hex"))
							{		
								CompilationIsCorrect = true;
								System.IO.Directory.Move(SavePath + "/xplduino_controller.cpp.hex" , SavePath + "/" + node.Node_CRC + ".hex"); //On renomme le fichier avec le  CRC du noeud
								
								//On ajoute le .hex dans le projet
								ZipFile zipFile = new ZipFile(pro.Project_SavePath + "/" + pro.Project_Name + ".dom");
								zipFile.BeginUpdate();
								zipFile.Password = pro.Project_Password;
								zipFile.Add(SavePath + "/" + node.Node_CRC + ".hex","Hex/" + node.Node_CRC + ".hex");
							    zipFile.CommitUpdate();
							    zipFile.Close();
								
								//On supprimer le dossier de fabrication
								string TargetPath = pro.Project_SavePath + param.ParamP("FolderTargetFirmware");
								if(Directory.Exists(TargetPath))
								{
									DirectoryInfo DirectoryFirmwareNode = new DirectoryInfo(TargetPath);
									DirectoryFirmwareNode.Delete(true);							
								}	
								
								node.Node_Compile = true;
							}							
						}
					}
				}
			}

			Gtk.Application.Invoke(delegate {
		        mainwindow.ActiveCompileAndLoadButtonCompilation(CompilationIsCorrect);
				mainwindow.UpdateWidgetInTab(); //Mise à jour des widget dans les tab
				mainwindow.UpdateLogCompilation(CompilLogData);
		    });		
			
			return CompilationIsCorrect;
		}
		
			//Fonction ShellTransfert
			//Fonction permettant d'envoyer des commandes shell
			public bool ShellTransfert(string _Commande, string _Argument, bool _DispayMessage, string _FileName)
			{
				bool ShellTransfertWithoutError = true;
	            startInfo = new ProcessStartInfo(_Commande, _Argument);
	
		        startInfo.UseShellExecute = false;
		        startInfo.CreateNoWindow = true;
		        startInfo.ErrorDialog = false;
		
		        startInfo.RedirectStandardError = true;
		        startInfo.RedirectStandardInput = true;
		        startInfo.RedirectStandardOutput = true;
		
		        process = new Process();
		        process.StartInfo = startInfo;
		        process.Start();
		
		        writer = process.StandardInput;
		        reader = process.StandardOutput;
		        errorReader = process.StandardError;
				
				string StandardError = "";
			
				
					string PreviousLine = "";
					while(!errorReader.EndOfStream)
					{
						StandardError = errorReader.ReadLine();
						Console.WriteLine(StandardError);
						
						//On va enregistrer le log dans un string pour l'afficher 
			    		CompilLogData = CompilLogData + "\n" + _FileName + " : " + StandardError;	
						
						if(_DispayMessage)
						{
					
							if(StandardError.IndexOf("error") > -1)
							{
								Gtk.Application.Invoke(delegate {
							        mainwindow.AddLineOutput(param.ParamI("OutputError"),_FileName + " : " + PreviousLine + " => " + StandardError);	
							    });	
								
								ShellTransfertWithoutError = false;
							}
							else if(StandardError.IndexOf("warning") > -1)
							{
								Gtk.Application.Invoke(delegate {
							        mainwindow.AddLineOutput(param.ParamI("OutputWarning"),_FileName + " : " + StandardError);	
							    });	
							}					
							PreviousLine = StandardError;		
						}
						
					}

				process.WaitForExit();
				return ShellTransfertWithoutError;
			}			
		
//################# Chargement des fichiers #################################		
		
		//Fonction LoadBoard
		//Fonction permettant de charger une carte
		public void LoadBoard(string USBPort, Int32 _ProjectID, Int32 _NodeId)
		{
	        if (System.IO.Directory.Exists(Environment.CurrentDirectory + param.ParamP("FolderHardware")))
	        {	
				string ShellCommande = "";
				string ShellArgument = "";
				
				foreach(Project pro in ListProject)
				{
					foreach(Node node in pro.ReturnListNode())
					{
						if(pro.Project_Id == _ProjectID && node.Node_Id == _NodeId && Directory.Exists(pro.Project_SavePath))
						{		
							string TempUnzip = pro.Project_SavePath + "/UnZip";
							
							ExtractZipFile(pro.Project_SavePath + "/" + pro.Project_Name + ".dom",TempUnzip,pro.Project_Password);
						
							if(File.Exists(TempUnzip + "/Hex/" + node.Node_CRC + ".hex"))
							{							
								ShellCommande = Environment.CurrentDirectory + param.ParamP("FolderTools") + "avrdude";
								ShellArgument = " -C " + Environment.CurrentDirectory + param.ParamP("FolderTools") + "avrdude.conf  -v -v -v -v -patmega1284p -carduino -P/dev/tty" + USBPort + " -b115200 -D -Uflash:w:";
								ShellArgument = ShellArgument + TempUnzip + "/Hex/" + node.Node_CRC + ".hex:i";

								if(ShellTransfertForUpload(ShellCommande,ShellArgument,true,ReturnNumberOfLineInHexFile(TempUnzip + "/Hex/" + node.Node_CRC + ".hex")))
								{
									Gtk.Application.Invoke(delegate {
							        	mainwindow.ActiveCompileAndLoadButtonLoad(true);	
							    	});	
								}
								else
								{
									Gtk.Application.Invoke(delegate {
							        	mainwindow.ActiveCompileAndLoadButtonLoad(false);	
							    	});	
								}		
							}
							else
							{
								
							}
						}
					}
				}	

			}
		}		
		
			//Fonction ShellTransfertForUpload
			//Fonction permettant d'envoyer des commandes shell pour la partie upload
			public bool ShellTransfertForUpload(string _Commande, string _Argument, bool _DispayMessage, Int32 _NumberOfLineInHexFile)
			{
				bool ShellTransfertWithoutError = true;
	            startInfo = new ProcessStartInfo(_Commande, _Argument);
	
		        startInfo.UseShellExecute = false;
		        startInfo.CreateNoWindow = true;
		        startInfo.ErrorDialog = false;
		
		        startInfo.RedirectStandardError = true;
		        startInfo.RedirectStandardInput = true;
		        startInfo.RedirectStandardOutput = true;
		
		        process = new Process();
		        process.StartInfo = startInfo;
		        process.Start();
		
		        writer = process.StandardInput;
		        reader = process.StandardOutput;
		        errorReader = process.StandardError;
				
			
				Int32 NumberOfLine = 0;
				
				string PreviousLine = "";
				while(!errorReader.EndOfStream)
				{
					string StandardError = errorReader.ReadLine();
					Console.WriteLine(StandardError);
				
					CompilLogData = CompilLogData + "\n" + StandardError;
				
				
					if(StandardError.IndexOf("Connection timed out") > -1 || StandardError.IndexOf("programmer is not responding") > -1 && _DispayMessage)
					{
						Gtk.Application.Invoke(delegate {
					        mainwindow.AddLineOutput(param.ParamI("OutputError"),PreviousLine + " => " + StandardError);	
					    });	
						
						ShellTransfertWithoutError = false;
					}

					PreviousLine = StandardError;	
				
					NumberOfLine++;
				
					Gtk.Application.Invoke(delegate {
						mainwindow.UpdateProgressBar(((NumberOfLine*1.1750)/_NumberOfLineInHexFile)*100);
					 });
				}
					
				return ShellTransfertWithoutError;
			}		
		
			//Fonction ReturnNumberOfLineInHexFile
			//Permet de retourner le nombre de ligne du fichier hex
			public Int32 ReturnNumberOfLineInHexFile(string _NameFile)
			{
				System.IO.StreamReader sr = new System.IO.StreamReader(_NameFile);
				Int32 i = 0;
				string buff = "";
				while (buff != null)
				{
				     buff = sr.ReadLine();
				     if (buff == null) { break; }
				     else { i++; }
				}
				return i;
			}
		
//################# Lecture des trame ethernet #########################
		
		//Fonction ReadEthernetData
		//Fonction permettant de lire les données UDP
		public void ReadEthernetData()
		{
		int Port = 3865;
		var server = new UdpClient(Port);
			
			
			server.BeginReceive(result => {
					IPEndPoint sender1 = null;
			        var data = server.EndReceive(result, ref sender1);
			        var value1 = Encoding.UTF8.GetString(data);
					Console.WriteLine(value1);
			        server.Close ();
			}, null);	
					
		ReadEthernetData();	
		}
	}
}