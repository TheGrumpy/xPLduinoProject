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
	public class Scenario
	{
		public Int32 ScenarioId;
		public string ScenarioName;
		public string ScenarioData;
		public string ScenarioNotes;
		public List<Variable> Variable_;
		public List<Function> Function_;
		public string ScenarioHashCode;
		
		public Scenario (Int32 _ScenarioId, string _ScenarioName, string _ScenarioData)
		{
			this.ScenarioId = _ScenarioId;
			this.ScenarioName = _ScenarioName;
			this.ScenarioData = _ScenarioData;
			this.ScenarioNotes = "";
			this.ScenarioHashCode = "";
			
			Variable_ = new List<Variable>();
			Function_ = new List<Function>();
		}
		
		public Scenario (Int32 _ScenarioId)
		{
			this.ScenarioId = _ScenarioId;
			Variable_ = new List<Variable>();
			Function_ = new List<Function>();			
		}
		
		public List<Variable> AddVariableInScenario(Int32 _Id, string _Name,string _Type,Int32 _DefaultValue)
		{
			Variable_.Add(new Variable(_Id,_Name,_Type,_DefaultValue));
			return Variable_;
		}
		
		public Variable AddVariableInScenario(Int32 _Id)
		{
			Variable vari = new Variable(_Id);
			Variable_.Add(vari);
			return vari;
		}		
		
		public List<Variable> ReturnListVariable()
		{
			return Variable_;
		}
		
		public List<Function> AddFunctionInScenario(Int32 _Id, string _Name, string _TypeReturn)
		{
			Function_.Add(new Function(_Id,_Name,_TypeReturn,false));
			return Function_;
		}
		
		public Function AddFunctionInScenario(Int32 _Id)
		{
			Function fun = new Function(_Id);
			Function_.Add(fun);
			return fun;
		}		
		
		
		public List<Function> ReturnListFunction()
		{
			return Function_;
		}
		
		public Scenario()
		{
			Variable_ = new List<Variable>();
			Function_ = new List<Function>();
		}
		
		public Function CreateBitlashCustomFunction(Int32 _Id)
		{
			Function BitlashCustomFunction = new Function(_Id,"bitlash_custom","void",true);
			Function_.Add(BitlashCustomFunction);	
			return BitlashCustomFunction;
		}
	}
}

