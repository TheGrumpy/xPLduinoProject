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
using System.Collections.Generic;

using System.IO;

using System.Xml.Serialization;


namespace xPLduinoManager
{
	//Class Project
	//Cette classe permet de gérer un projet et de stocker l'ensemble des informations
	//Eléments : 
	//	string Project_Name : Nom du projet
	//	string Project_Author : Auteur du projet 
	//	DateTime Project_CreationDateAndTime : Date et heure à la création du projet
	//	DateTime Project_ModificationDateAndTime : Date et heure mise à jour à chaque modification du projet
	//	Int32 Project_Version : Numéro de version du projet	
	//	string Project_SavePath : Endroit où le projet est sauvegardé
	//	List<Node> Node_ : Liste de noeuds propre au projet
	//Fontions :
	//	AddNodeInproject : Permet de rajouter un nouveau noeud au projet
	//	ReturnListNode : Permet de retourner la liste des noeuds
	public class Project
	{
		public Int32 Project_Id;
		public string Project_Name;
		public string Project_Author;
		public string Project_CreationDateAndTime;
		public string Project_ModificationDateAndTime;
		public Int32 Project_Version;
		public string Project_SavePath;
		public string Project_Note;
		public string Project_Password;
		public bool ProjectIsSave;
		
		public List<Node> Node_;
		public List<Node> CopyNode_;
		
		public Param param;
		public Preference pref;
		
		//Consturcteur de la class (2 Arguments)
		//Attribut d'entrée :
		//	string _Name : Nom du projet
		//	string _Author : Auteur du projet
		//	Les autres attribut se mette à jour automatiquement
		public Project (Int32 _Id, string _Name, string _Author, string _SavePath, Param _param, Preference _pref)
		{
			Node_ = new List<Node>();
			//CopyNode_ = new List<Node>();
			this.Project_Id = _Id;
			this.Project_Name = _Name;
			this.Project_Author = _Author;
			this.Project_CreationDateAndTime =  DateTime.Now.ToString();
			this.Project_ModificationDateAndTime =  DateTime.Now.ToString();
			this.Project_Version = 0;
			this.Project_SavePath = _SavePath;
			this.Project_Note = "";
			this.Project_Password = "";
			this.param = _param;
			this.pref = _pref;
			this.ProjectIsSave = false;
		}
		
		public Project(Int32 _Id, string _Name, string _Author, string _SavePath, string _CreationDateAndTime, string _ModificationDateAndTime, string _Note, Int32 _Version, Param _param, Preference _pref)
		{
			Node_ = new List<Node>();
			this.Project_Id = _Id;
			this.Project_Name = _Name;
			this.Project_Author = _Author;
			this.Project_SavePath = _SavePath;
			
			this.Project_CreationDateAndTime =  _CreationDateAndTime;
			this.Project_ModificationDateAndTime =  _ModificationDateAndTime;
			
			this.Project_Version = _Version;
			
			this.Project_Note = _Note;
			
			this.param = _param;
			this.pref = _pref;	
			this.ProjectIsSave = false;
			this.Project_Password = "";
		}
		
		public Project ()
		{
			Node_ = new List<Node>();
			this.Project_Password = "";
		}
			
		public Project(Int32 _Id, Param _param, Preference _pref)
		{
			Node_ = new List<Node>();
			this.param = _param;
			this.pref = _pref;		
			this.Project_Id = _Id;
			this.Project_Password = "";
		}
		
		//Fonction permettant d'ajouter un noeud à un projet
		//Attribut d'entrée :
		//	string Node_Name : Nom du noeud
		//	Int32 _Id : Id du noeud
		//	Project _pro  : permettant la création de la base
		//Return :
		//	List<Node> : il nous retourne la liste de noeud
		public List<Node> AddNodeInProject(string Node_Name, Int32 _Id)
		{
			Node_.Add(new Node(Node_Name, _Id, param,pref));
			return Node_;
		}	
		
		//Fonction permettant d'ajouter un noeud à un projet en mode ouverture de fichier
		public Node AddNodeInProject(Int32 _Id)
		{
			Node ReturnNode = new Node(_Id,param,pref);
			Node_.Add(ReturnNode);
			return ReturnNode;
		}		
		
		//Fonction ReturnListNode
		//Fonction permettant de retourner la liste de noeud
		public List<Node> ReturnListNode()
		{
			return Node_;
		}	
	}
}