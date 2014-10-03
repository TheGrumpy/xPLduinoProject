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
	//Classe Board
	//Classe permettant de stocker l'ensemble des cartes sur un réseau
	//Eléments :
	//	Int32 Board_Id : Id de la carte
	//	string Board_Type : Type de carte
	//	string Board_Name : Nom de la carte
	//	Int32 Board_I2C_0 : Addresse I2C 0 
	//	Int32 Board_I2C_1 : Addresse I2C 1 	
	//	string Board_1Wire_Mac : Adresse Mac dans le cas ou la carte est de type 1-wire
	//	string Board_Note : Note propre à une carte
	//	public List<Pin> Pin_ : Liste contenant l'ensemble de pin pour chaque board
	//Fonction : 
	//	AddPinInBoard : Fonction permettant d'ajouter une broche à une carte
	//	ReturnListPin : Fonction permettant de retourner la liste des broches
	
	public class Board
	{
		public Int32 Board_Id;
		public string Board_Type;
		public string Board_Name;
		public Int32 Board_I2C_0;
		public Int32 Board_I2C_1;
		public string Board_1Wire_Mac;
		public string Board_1Wire_Precision;
		public string Board_Note;
		public List<Pin> Pin_;
		
		//Constructeur de la classe (2 arguments)
		//Int32 _Id : Id de la carte
		//string _Type : Type de la carte
		//string _Name : Nom de la carte
		//Int32 _I2C_0 : Addresse I2C 0 
		//Int32 _I2C_1 : Addresse I2C 1 
		public Board (Int32 _Id, string _Type, string _Name)
		{
			Pin_ = new List<Pin>();
			
			this.Board_Id = _Id;
			this.Board_Type = _Type;
			this.Board_Name = _Name;
			this.Board_I2C_0 = 0;
			this.Board_I2C_1 = 0;	
			this.Board_1Wire_Mac = "00-00-00-00-00-00-00-00";
			this.Board_1Wire_Precision = "0.1";
			this.Board_Note = "";
		}
		
		public Board (Int32 _Id)
		{
			Pin_ = new List<Pin>();
			this.Board_Id = _Id;
		}
		
		public Board()
		{
			Pin_ = new List<Pin>();
		}
		
		//Fontion permettant d'ajouter une broche à une carte
		//Arguments :
		//	Int32 _Id : Id de la broche
		//	string _Name : Nom de la broche
		//	string _Direction : Direction de la broche
		public List<Pin> AddPinInBoard(Int32 _Id, string _Name, string _Direction, int _Number)
		{
			Pin_.Add(new Pin(_Id, _Name, _Direction, _Number));
			return Pin_;
		}
		
		//Fontion permettant d'ajouter une broche à une carte
		//Arguments :
		//	Int32 _Id : Id de la broche
		//	string _Name : Nom de la broche
		//	string _Direction : Direction de la broche
		public Pin AddPinInBoard(Int32 _Id)
		{
			Pin pin = new Pin(_Id);
			Pin_.Add(pin);
			return pin;
		}		
		
		//Fcontion permettant de retourner la liste des pin
		//Pas d'argument
		public List<Pin> ReturnListPin()
		{
			return 	Pin_;
		}
	}
}