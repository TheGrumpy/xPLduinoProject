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
	//Class Node
	//Cette class permet de stocker l'ensemble des noeuds présents
	//Eléments :
	//	Int32 Node_Id : Id du noeud
	//	string Node_Type : Type de noeud (dans un premier temps NODE par défaut)
	//	string Node_Name : Nom du noeud
	//	string Node_IP : Adresse Ip du noeud, dans un second temps nous verrons la mise en place du choix DHCP
	//	strinf Node_MAC : Adresse MAC de la SMB
	//	bool Node_DHCP : Permet de déterminer si nous allons utiliser le DHCP pour ce noeud
	//	string Node_Note : Permet de mettre une note personnel pour un noeud
	//	List<Network> Network_ : Liste des réseaux propre à chaque noeud
	//	List<Debug> Debug_: Liste des debug propre à un noeud
	//	List<Instance> Instance_ : Liste des instances propre à un noeud
	//Fonctions :
	//	AddNetworkInNode : Permet d'ajouter un réseau à un noeud.
	//	AddNetworkInNode : Permet d'ajouter un noeud à un réseau
	//	AddInstanceInNode : Permet d'ajouter une instance à un noeud
	//  ReturnListNetwork : retourne la liste des réseaux
	//	ReturnAdressMac : Fonction permettant de retourner une adresse mac de façon aléatoire
	//	CountInstancePerType : Permet de retourner le nombre d'instance par type
	
	public class Node
	{
		public Int32 Node_Id;
		public string Node_Name;
		public string Node_IP;
		public string Node_GTWIP;
		public string Node_Mac;
		public bool Node_DHCP;
		public string Node_Note;
		public bool Node_WebServer;
		
		public string Node_Type;
		public string Node_Clock;
		public bool Node_1Wire;
		
		public List<Network> Network_;
		public List<Debug> Debug_;
		public List<Instance> Instance_;
		public List<Customer> Customer_;
		public List<Scenario> Scenario_;
		
		public Param param;
		public Preference pref;
		
		//Constructeur de la classe (2 arguments)
		//Attribut d'entrée :
		//	string _Name : Nom du Noeud
		//	Int32 _Id : Id du noeud
		//	Project Pro : permet la construction de la base
		public Node (string _Name, Int32 _Id, Param _param, Preference _pref)
		{
			Network_ = new List<Network>();
			Debug_ = new List<Debug>();
			Instance_ = new List<Instance>();
			Customer_ = new List<Customer>();
			Scenario_ = new List<Scenario>();
			
			this.param = _param;
			this.pref = _pref;
			
			this.Node_Id = _Id;
			this.Node_Name = _Name;
			this.Node_IP = "0.0.0.0";
			this.Node_GTWIP = "0.0.0.0";
			this.Node_Mac = ReturnAdressMac();
			this.Node_DHCP = true;
			this.Node_WebServer = true;
			
			this.Node_Note = "";
			
			this.Node_Type = param.ParamP("NP_SMBv01");
			this.Node_Clock = "0";
			this.Node_1Wire = false;
			
		}
			
		public Node()
		{	
			Network_ = new List<Network>();
			Debug_ = new List<Debug>();
			Instance_ = new List<Instance>();
			Customer_ = new List<Customer>();
			Scenario_ = new List<Scenario>();
		}
		
		
		public Node (Int32 _Id, Param _param, Preference _pref)
		{		
			Network_ = new List<Network>();
			Debug_ = new List<Debug>();
			Instance_ = new List<Instance>();
			Customer_ = new List<Customer>();
			Scenario_ = new List<Scenario>();
			
			this.Node_Id = _Id;
			this.param = _param;
			this.pref = _pref;		
		}
		
		
			
		//Fonction AddDebugInNode
		//Fonction permettant d'ajouter un debug dans un noeud
		public List<Debug> AddDebugInNode(string _Name, string _FrenchDescriptionDebug, string _EnglishDescriptionDebug)
		{
			Debug_.Add(new Debug(_Name,_FrenchDescriptionDebug,_EnglishDescriptionDebug));
			return Debug_;
		}
		
		//Fonction AddDebugInNode
		//Fonction permettant d'ajouter un debug dans un noeud
		public Debug  AddDebugInNode()
		{
			Debug deb = new Debug();
			Debug_.Add (deb);
			return deb;
		}
		
		//Fonction permettant d'ajouter un réseau à un noeud
		//Attribut d'entrée :
		//	string _Type : Type de réseaux
		//	Int32 _Id : Id du réseau
		public List<Network> AddNetworkInNode(Int32 _Id, string _Type)
		{
			Network_.Add(new Network(_Id,_Type));
			return Network_;
		}
		
		//Fonction permettant d'ajouter un réseau à un noeud
		public Network AddNetworkInNode(Int32 _Id)
		{
			Network net = new Network(_Id);
			Network_.Add(net);
			return net;
		}		
		
		//Fonction permettant d'ajouter une instance à un noeud
		//Attribut d'entrée :
		// 	Int32 _Id : Id de l'instance
		//	string _Type : Type de l'instance LIG, SWI, SHU ...
		//	string _Name : Nom de l'instance
		//	Int32 _Node_Id : Id du noeud
		//	Int32 _SHU_NumberOfOutput : Nombre du sortie dans le cas où nous avons un volet
		public List<Instance> AddInstanceInNode(Int32 _Id, string _Type,string _Name)
		{
			if(_Type == param.ParamP("InstLightingName")) //Si le type est de type lighting
			{
				Instance_.Add (new Instance(_Id,param.ParamP("InstLightingName"),_Name,pref.LIGDefaultValue,pref.LIGFade));//Ajout d'une nouvelle instance
			}
			else if(_Type == param.ParamP("InstSwitchName"))//Si le type est de type switch
			{
				Instance_.Add (new Instance(_Id,param.ParamP("InstSwitchName"),_Name,pref.SWIImpusionTime,pref.SWIInverse));//Ajout d'une nouvelle instance
			}
			else if(_Type == param.ParamP("InstShutterName"))//Si le type est de type shutter
			{
				Instance_.Add (new Instance(_Id,param.ParamP("InstShutterName"),_Name,pref.SHUType,pref.SHUTravelTime,pref.SHUInitTime,false));//Ajout d'une nouvelle instance
			}
			return Instance_;//Nous retournons l'instance
		}
		
		public Instance AddInstanceInNode(Int32 _Id)
		{
			Instance ins = new Instance(_Id);
			Instance_.Add (ins);
			return ins;
		}
		
		//Fonction AddCustomerInNode
		//Fonction permettant d'ajouter un fichier customer à un noeud
		public List<Customer> AddCustomerInNode(Int32 _Id, string _Name,string _Data,bool _Use)
		{
			Customer_.Add (new Customer(_Id,_Name,_Data,_Use));
			return Customer_;
		}
		
		//Fonction AddCustomerInNode
		//Fonction permettant d'ajouter un fichier customer à un noeud
		public Customer AddCustomerInNode(Int32 _Id)
		{
			Customer cus = new Customer(_Id);
			Customer_.Add (cus);
			return cus;
		}		
		
		//Fonction AddScenarioInNode
		//Fonction permettant d'ajouter un scénario à un noeud
		public List<Scenario> AddScenarioInNode(Int32 _Id, string _Name,string _Data)
		{
			Scenario_.Add(new Scenario(_Id,_Name,_Data));
			return Scenario_;          
		}
		
		public Scenario AddScenarioInNode(Int32 _Id)
		{
			Scenario sce = new Scenario(_Id);
			Scenario_.Add(sce);
			return sce;
		}
		
		//Fonction permettant de retourner la liste des réseaux
		//Pas d'argument
		public List<Network> ReturnListNetwork()
		{
			return Network_;
		}
		
		//Fonction ReturnListDebug
		//Fonction permettant de retourner la liste des debugs
		public List<Debug> ReturnListDebug()
		{
			return Debug_;
		}		
		
		//Fonction ReturnListCustomer
		//Fonction permettant de retourner la liste des fichier customer pour un noeud
		public List<Customer> ReturnListCustomer()
		{
			return Customer_;
		}
		
		//Fonction ReturnListScenarion
		//Fonction permettant de retourner la liste des scénario
		public List<Scenario> ReturnListScenario()
		{
			return Scenario_;
		}
		
		//Fonction permettant de retrourner la liste des instances propre au noeud
		//Pas d'argument
		public List<Instance> ReturnListInstance()
		{
			return Instance_;
		}		
		
		//Fonction permettant de retourner le nombre d'instance par type
		//Pas d'argument
		public int CountInstancePerType(string _Type)
		{
			int i = 0; //Initialisation d'une variable à 0
			foreach(Instance ins in Instance_) //Pour chaque instance
			{
				if(ins.Instance_Type == _Type)//Si l'instance est de type à celui que nous avons passé en paramètre
				{
					i++; //Nous incrémentons la variable
				}
			}
			return i;//Nous retournons le nombre d'instance par type
		}
		
		
		//Fonction ReturnAdressMac
		//Fcontion permettant de retourner une adresse mac de façon aléatoire
		public string ReturnAdressMac()
		{
			Random rnd1 = new Random(); 
			string _Mac = "";
			
			for(int i =0; i<= 11; i++)
			{
				int decValue = rnd1.Next(15);
				_Mac = _Mac + decValue.ToString("X");
				if(i==1 || i==3 || i==5 || i==7 || i==9)
				{
					_Mac = _Mac + "-";
				}
			}	                  
			return _Mac;
		}
	
		
	}
}