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
using Gtk;
using Gdk;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace xPLduinoManager
{
	public class Preference
	{
		public bool DisplayWelcomeTab;
		public bool ConfirmClose;
		public bool BeepOnDelete;
		public bool BeepOnError;
		public string Langage;
		public int NumberOfProjectCopy;
		public Param param;
		public Int32 LIGDefaultValue;
		public Int32 LIGFade;
		public string LIGToggleActionName;
		public string LIGTuneActionName;
		public string LIGStopActionName;
		public string LIGSetActionName;
		
		public bool SWIInverse;
		public Int32 SWIImpusionTime;
		public string SWIClicActionName;
		public string SWIDoubleClicActionName;
		public string SWIOnActionName;
		public string SWIOnFmActionName;
		public string SWIOffActionName;
		public string SWIOffFmActionName;			
		
		public Int32 SHUType;
		public Int32 SHUTravelTime;
		public Int32 SHUInitTime;
		public string SHUOpenActionName;
		public string SHUCloseActionName;
		public string SHUStopActionName;
		public string SHUToggleActionName;
		
		public string TEMPGetValue;
		
		public Preference (Param _param)
		{
			this.param = _param;
			DisplayWelcomeTab = true;
			ConfirmClose = true;
			BeepOnDelete = true;
			BeepOnError = true;
			NumberOfProjectCopy = 100;
			LIGDefaultValue = 0;
			LIGFade = 0;
			LIGToggleActionName = "toggle";
			LIGTuneActionName = "tune";
			LIGStopActionName = "stop";
			LIGSetActionName = "set";
			
			SWIInverse = false;
			SWIImpusionTime = 0;
			SWIClicActionName = "pulse";
			SWIDoubleClicActionName = "dpulse";
			SWIOnActionName = "on";
			SWIOnFmActionName = "on_fm";
			SWIOffActionName = "off";
			SWIOffFmActionName = "off_fm";
					
			SHUType = 0;
			SHUTravelTime = 0;
			SHUInitTime = 0;
			SHUOpenActionName = "open";
			SHUCloseActionName = "close";
			SHUStopActionName = "stop";
			SHUToggleActionName = "toggle_shutter";
			
			TEMPGetValue = "get_value";
			
			InitPreference();
		}
		
		//Fonction InitPreference
		//Fonction permettant de lire le fichier de préférence et de mettre à jour les variables
		public bool InitPreference()
		{	
			//Nous allons vérifier la présence d'un fichier dans le dossier param
			string[] files;
			string CRC = ""; 
			
			//Permet d'enregistrer dans un fichier le xml généré
			if(!Directory.Exists(Environment.CurrentDirectory + param.ParamP("FolderPreference"))) 
			{
				 System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + param.ParamP("FolderPreference"));
			}		
			
			// pour avoir les noms des fichiers et sous-répertoires
			files = Directory.GetFiles(Environment.CurrentDirectory + param.ParamP("FolderPreference"));
			 
			int filecount = files.GetUpperBound(0) + 1;
			if(filecount > 1 || filecount==0)
			{
				return false;
			}
			
			for (int i = 0; i<filecount;  i++)
			{
				CRC = System.IO.Path.GetFileName(files[i]);
			    //Console.WriteLine(System.IO.Path.GetFileName(files[i]));
			}					
			
			//On recalcule le CRC du fichier
			string CRCCalcul = CalculHash(System.IO.File.ReadAllText(Environment.CurrentDirectory + param.ParamP("FolderPreference") + "/" + CRC));
			
			//On compare les deux
			for(int i=0;i<32;i++)
			{
				if(CRC[i] != CRCCalcul[i])
				{
					return false;
				}
			}
			
			using (XmlTextReader reader = new XmlTextReader(Environment.CurrentDirectory + param.ParamP("FolderPreference") + "/" + CRC)) //Ouverture du fichier Param.xml
			{	
			    while (reader.Read()) //Lecture total du fichier
			    {
					if (reader.IsStartElement())
					{
						if(reader.Name == param.ParamP("Preference"))
						{			
							if(reader["DisplayWelcomeTab"] != null)
								DisplayWelcomeTab = Convert.ToBoolean(reader["DisplayWelcomeTab"]);
							if(reader["ConfirmClose"] != null)
								ConfirmClose = Convert.ToBoolean(reader["ConfirmClose"]);
							if(reader["BeepOnDelete"] != null)
								BeepOnDelete = Convert.ToBoolean(reader["BeepOnDelete"]);
							if(reader["BeepOnError"] != null)
								BeepOnError = Convert.ToBoolean(reader["BeepOnError"]);
							if(reader["Langage"] != null)
								Langage = reader["Langage"];
							
							
							if(reader["LIGDefaultValue"] != null)
								LIGDefaultValue = Convert.ToInt32(reader["LIGDefaultValue"]);
							if(reader["LIGFade"] != null)
								LIGFade = Convert.ToInt32(reader["LIGFade"]);
							if(reader["LIGToggleActionName"] != null)
								LIGToggleActionName = reader["LIGToggleActionName"];	
							if(reader["LIGTuneActionName"] != null)
								LIGTuneActionName = reader["LIGTuneActionName"];	
							if(reader["LIGStopActionName"] != null)
								LIGStopActionName = reader["LIGStopActionName"];	
							if(reader["LIGSetActionName"] != null)
								LIGSetActionName = reader["LIGSetActionName"];								
							
							if(reader["SWIInverse"] != null)
								SWIInverse = Convert.ToBoolean(reader["SWIInverse"]);
							if(reader["SWIImpusionTime"] != null)
								SWIImpusionTime = Convert.ToInt32(reader["SWIImpusionTime"]);							
						
							if(reader["SWIClicActionName"] != null)
								SWIClicActionName = reader["SWIClicActionName"];
							if(reader["SWIDoubleClicActionName"] != null)
								SWIDoubleClicActionName = reader["SWIDoubleClicActionName"];
							if(reader["SWIOnActionName"] != null)
								SWIOnActionName = reader["SWIOnActionName"];
							if(reader["SWIOnFmActionName"] != null)
								SWIOnFmActionName = reader["SWIOnFmActionName"];
							if(reader["SWIOffActionName"] != null)
								SWIOffActionName = reader["SWIOffActionName"];
							if(reader["SWIOffFmActionName"] != null)
								SWIOffFmActionName = reader["SWIOffFmActionName"];	
							
							if(reader["SHUType"] != null)
								SHUType = Convert.ToInt32(reader["SHUType"]);									
							if(reader["SHUTravelTime"] != null)
								SHUTravelTime =  Convert.ToInt32(reader["SHUTravelTime"]);	
							if(reader["SHUInitTime"] != null)
								SHUInitTime =  Convert.ToInt32(reader["SHUInitTime"]);								
							if(reader["SHUOpenActionName"] != null)
								SHUOpenActionName = reader["SHUOpenActionName"];	
							if(reader["SHUCloseActionName"] != null)
								SHUCloseActionName = reader["SHUCloseActionName"];	
							if(reader["SHUStopActionName"] != null)
								SHUStopActionName = reader["SHUStopActionName"];	
							if(reader["SHUToggleActionName"] != null)
								SHUToggleActionName = reader["SHUToggleActionName"];
							
							if(reader["TEMPGetValue"] != null)
								TEMPGetValue = reader["TEMPGetValue"];
						}
					}
				}
			}
			
			param.Langage = Langage;
			return true;
		}
		
		//Fonction CalculHash
		//Fonction permettant de calculer le hash d'un fichier
		public string CalculHash(string _Text)
		{
			string hash;
			using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
			{
			  hash = BitConverter.ToString(md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(_Text))).Replace("-", String.Empty);
			}	
			return hash;
		}		
	}
}

