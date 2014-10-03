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
	//Classe Boards
	//Cette classe permet de stocker l'ensemble des carte I2C de la gamme xPLduino
	//Eléments :	
	//	string Type : Type de la carte
	//	Int32 NumberOfInputs : Nombre d'entrée de la carte
	//	string PrefixIN : Permet de determiner le préfix dans le nom de la borche
	//	Int32 NumberOfOutputs : Nombre de sortie de la carte
	//	string PrefixOUT : Permet de determiner le préfix dans le nom de la borche	
	//	Int32 NumberI2CAdress : Nombre d'adresse I2C présent sur la carte
	public class Boards
	{
		public string Type;
		public Int32 NumberOfInputs;
		public string PrefixIN;
		public Int32 NumberOfOutputs;
		public string PrefixOUT;
		public Int32 NumberI2CAdress;	
		public string NetworkType;
		public Int32 MaxI2CAdress;
		
		public string FunctionNamePRESWI;
		public string FunctionNamePRELIG;
		
		public string FunctionNamePOSTLIG;
		public string FunctionNamePOSTSHU;
		
		//Constructeur de la classe (4 arguments)
		//string _Type = Type de carte
		//NumberOfInputs = _NumberOfInputs = Nombre d'entrée de la carte
		//NumberOfOutputs = _NumberOfOutputs = Nombre de sortie de la carte
		//NumberI2CAdress = _NumberI2CAdress = Nombre d'adresse I2C présent sur la carte
		public Boards (string _Type, Int32 _NumberOfInputs, string _PrefixIN, Int32 _NumberOfOutputs, string _PrefixOUT, Int32 _NumberI2CAdress, string _NetworkType, Int32 _MaxI2CAdress, string _FunctionNamePRESWI, string _FunctionNamePRELIG, string _FunctionNamePOSTLIG, string _FunctionNamePOSTSHU)
		{
			this.Type = _Type;
			this.NumberOfInputs = _NumberOfInputs;
			this.PrefixIN = _PrefixIN;
			this.NumberOfOutputs = _NumberOfOutputs;
			this.PrefixOUT = _PrefixOUT;			
			this.NumberI2CAdress = _NumberI2CAdress;
			this.NetworkType = _NetworkType;
			this.MaxI2CAdress = _MaxI2CAdress;
			
			this.FunctionNamePRESWI = _FunctionNamePRESWI;
			this.FunctionNamePRELIG = _FunctionNamePRELIG;	
			
			this.FunctionNamePOSTLIG = _FunctionNamePOSTLIG;
			this.FunctionNamePOSTSHU = _FunctionNamePOSTSHU;
		}
	}
}