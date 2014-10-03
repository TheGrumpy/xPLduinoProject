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
	public class Parameters
	{
		//Classe Parameters
		//Class permettant de stocker l'ensemble des paramètres dans différente langues ou valeurs
		public string Name;
		public Int32 Int32Value;
		public string FrenchValue;
		public string EnglishValue;
		public string MultiLangageValue;
		
		public Parameters (string _Name, Int32 _Int32Value)
		{
			this.Name = _Name;
			this.Int32Value = _Int32Value;
		}
		public Parameters(string _Name, string _FrenchValue, string _EnglishValue)
		{
			this.Name = _Name;
			this.FrenchValue = _FrenchValue;
			this.EnglishValue = _EnglishValue;
		}
		public Parameters(string _Name, string _MultiLangageValue)
		{
			this.Name = _Name;
			this.MultiLangageValue = _MultiLangageValue;
		}		
		
	}
}

