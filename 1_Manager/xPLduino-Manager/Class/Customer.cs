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
	public class Customer
	{
		public Int32 CustomerId;
		public string CustomerName;
		public string CustomerData;
		public string CustomerNote;
		public bool CustomerUse;
		public Int32 ScenarioId;
		
		public Customer (Int32 _CustomerId, string _CustomerName, string _CustomerData, bool _CustomerUse)
		{
			this.CustomerId = _CustomerId;
			this.CustomerName = _CustomerName;
			this.CustomerData = _CustomerData;
			this.CustomerUse = _CustomerUse;
			this.CustomerNote = "";
			this.ScenarioId = 0;
		}
		
		public Customer (Int32 _CustomerId)
		{
			this.CustomerId = _CustomerId;
			this.ScenarioId = 0;
		}
		
		
		public Customer()
		{}
	}
}

