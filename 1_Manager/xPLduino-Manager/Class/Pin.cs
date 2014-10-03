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

namespace xPLduinoManager
{
	//Classe Pin
	//Cette classe contient l'ensemble des broches présent sur les cartes
	//Eléments :
	//	Int32 Pin_Id : Id de la broche
	//	string Pin_Name : Nom de la broche
	//	string Direction : Direction du courant dans la broche
	//	Int32 Instance_Id : Id de l'instance lié à la broche
	public class Pin
	{
		public Int32 Pin_Id;
		public string Pin_Name;
		public int Pin_Number;
		public string Pin_Direction;
		public Int32 Instance_Id;
		public bool Pin_FallbackValue;
		
			
		//Constructeur de la classe
		//Arguments :
		//	Int32 _Id : Id de la broche
		//	string _Name : Nom de la broche
		//	Int32 _Direction : Direction de la broche
		public Pin (Int32 _Id, string _Name, string _Direction, int _Number)
		{
			this.Pin_Id = _Id;
			this.Pin_Name = _Name;
			this.Pin_Number = _Number;
			this.Pin_Direction = _Direction;
			this.Instance_Id = 0;
		}
		
		public Pin (Int32 _Id)
		{
			this.Pin_Id = _Id;
			this.Instance_Id = 0;
		}
		
		public Pin()
		{}
	}
}