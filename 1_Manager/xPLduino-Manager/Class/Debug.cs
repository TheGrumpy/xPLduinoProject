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
	//Classe Debug
	//Classe permettant de lister les debug que nous avons besoin dans une node
	//Eléments :
	//	string Name : Nom du debug 
	//	bool Value : Valeur du debug
	//	string DescriptionDebug : description du debug
	//Fonction :
	public class Debug
	{
		public string Name;
		public bool Value;
		public string FrenchDescriptionDebug;
		public string EnglishDescriptionDebug;
		
		//Constructeur de la classe debug (2 argument)
		//string _Name : Nom du debug
		//string _DescriptionDebug : description du debug
		public Debug (string _Name, string _FrenchDescriptionDebug, string _EnglishDescriptionDebug)
		{
			this.Name = _Name;
			this.Value = false;
			this.FrenchDescriptionDebug = _FrenchDescriptionDebug;
			this.EnglishDescriptionDebug = _EnglishDescriptionDebug;
		}		
		
		public Debug()
		{}
	}
}