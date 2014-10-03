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
	//Classe MenuTextEditorFunction
	//Classe permettant de stocker l'ensemble des items dans le menu ListMenuTextEditor
	public class MenuTextEditorFunction
	{
		public int MenuTextEditor_Column;
		public int MenuTextEditor_Id;
		public int MenuTextEditor_IdParent;
		public bool MenuTextEditor_Active;
		public string MenuTextEditor_Name;
		public string MenuTextEditor_Text;
		public string MenuTextEditor_ToolTip;
		
		public MenuTextEditorFunction (int _Column, int _Id, int _IdParent, bool _Active,  string _Name, string _Text, string _Tooltip)
		{
			this.MenuTextEditor_Column = _Column;
			this.MenuTextEditor_Id = _Id;
			this.MenuTextEditor_IdParent = _IdParent;
			this.MenuTextEditor_Active = _Active;
			this.MenuTextEditor_Name = _Name;
			this.MenuTextEditor_Text = _Text;
			this.MenuTextEditor_ToolTip = _Tooltip;
		}
	}
}

