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
	//Classe Network
	//Classe permettant de stocker l'ensemble des réseaux au sein d'un noeud
	//Eléments :
	//	Int32 Network_Id : Id du réseau
	//	string Network_Type : Type du réseaux
	//	List<Board> Board_ : Liste des board propre au réseaux
	//Fonctions :
	//	AddBoardInNetwork : Permet d'ajouter une carte à un réseau
	//	ReturnListBoard : Permet de retourner la liste des cartes
	public class Network
	{
		public Int32 Network_Id;
		public string Network_Type;
		public string Network_Note;
		
		public List<Board> Board_;
		
		//Constructeur de la classe (2 arguments)
		//	Int32 _Id : Id du réseau
		//	string _Type : Nom du réseau
		public Network (Int32 _Id ,string _Type)
		{
			Board_ = new List<Board>();
			this.Network_Id = _Id;
			this.Network_Type = _Type;	
			this.Network_Note = "";
		}
		
		public Network(Int32 _Id)
		{
			this.Network_Id = _Id;
			Board_ = new List<Board>();
		}		
		
		public Network()
		{
			Board_ = new List<Board>();
		}
			
		//Fonction permettant d'ajouter une carte à un réseau
		//Arguments :
		//	Int32 _Id : Id de la carte
		//	string _Type : Type de carte
		//	string _Name : Nom de la carte
		//	Int32 _I2C_0 : Addresse I2C 0 
		//	Int32 _I2C_1 : Addresse I2C 1 
		public List<Board> AddBoardInNetwork(Int32 _Id, string _Type, string _Name)
		{
			Board_.Add(new Board(_Id,_Type,_Name));
			return Board_;
		}
	
		//Fonction permettant d'ajouter une carte à un réseau
		//Arguments :
		//	Int32 _Id : Id de la carte
		//	string _Type : Type de carte
		//	string _Name : Nom de la carte
		//	Int32 _I2C_0 : Addresse I2C 0 
		//	Int32 _I2C_1 : Addresse I2C 1 
		public Board AddBoardInNetwork(Int32 _Id)
		{
			Board boa = new Board(_Id);
			Board_.Add(boa);
			return boa;
		}		
		
		//Fonction permetant de retourner la liste des cartes
		//Par d'argument
		public List<Board> ReturnListBoard()
		{
			return Board_;
		}
		
		public object Clone()
		{
			return this.MemberwiseClone();
		}		
	}
}