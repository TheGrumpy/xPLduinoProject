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
	public class Function
	{
		public Int32 FunctionId;
		public string FunctionName;
		public string FunctionData;
		public string FunctionNote;
		
		public string FunctionTypeReturn;
		
		public string FunctionTypeArg1;
		public string FunctionTypeArg2;
		public string FunctionTypeArg3;
		public string FunctionTypeArg4;
		public string FunctionTypeArg5;
		public string FunctionTypeArg6;
		
		public string FunctionNameArg1;
		public string FunctionNameArg2;
		public string FunctionNameArg3;
		public string FunctionNameArg4;
		public string FunctionNameArg5;
		public string FunctionNameArg6;
		
		public bool InitFunction;
		
		public Function (Int32 _Id, string _Name, string _TypeReturn , bool _InitFunction)
		{
			this.FunctionId = _Id;
			this.FunctionName = _Name;
			this.FunctionTypeReturn = _TypeReturn;
			this.FunctionNote = "";
			this.FunctionData = "//\n";
			this.InitFunction = _InitFunction;
			
			FunctionTypeArg1 = "bool";
			FunctionTypeArg2 = "bool";
			FunctionTypeArg3 = "bool";
			FunctionTypeArg4 = "bool";
			FunctionTypeArg5 = "bool";
			FunctionTypeArg6 = "bool";			
			
			FunctionNameArg1 = "";
			FunctionNameArg2 = "";
			FunctionNameArg3 = "";
			FunctionNameArg4 = "";
			FunctionNameArg5 = "";
			FunctionNameArg6 = "";
			
			
		}
		
		public Function (Int32 _Id)
		{
			this.FunctionId = _Id;
		}
		
		
		public Function()
		{}
	}
}

