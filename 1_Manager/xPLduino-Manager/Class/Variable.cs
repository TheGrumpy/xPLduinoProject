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
	public class Variable
	{
		public Int32 VariableId;
		public string VariableName;
		public string VariableType;
		public Int32 VariableDefaultValue;
		public string VariableNote;
		
		public Variable (Int32 _Id, string _Name,string _Type,Int32 _DefaultValue)
		{
			this.VariableId = _Id;
			this.VariableName = _Name;
			this.VariableType = _Type;
			this.VariableDefaultValue = _DefaultValue;
			this.VariableNote = "";
		}
		
		public Variable (Int32 _Id)
		{
			this.VariableId = _Id;
		}
		
		public Variable()
		{}

	}
}

