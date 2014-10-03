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
	//Classe Instances
	//Classe permettant de gerer l'ensemble des instances
	//Eléments :
	//	Int32 Instance_Id : Id de l'instance
	//	string Instance_Type : Type de l'instance
	//  string Instance_Name : Nom de l'instance
	//		Lighting :
	//			Int32 Instance_LIG_DefaultValue : Valeur par défaut
	//			Int32 Instance_LIG_Fade : Pente pour la luminosité
	//			Int32 Pin_Id_0 : Permet de connaitre l'id de la pin connecté
	//		Switch :
	//			bool Instance_SWI_Inverse : Permet d'inversé l'entrée
	//			Int32 Instance_SWI_ImpulsionTime : Permet de mettre une valeur d'impulsion
	//			Int32 Pin_Id_0 : Permet de connaitre l'id de la pin connecté	
	//		Shutter : 
	//			string Instance_SHU_Type : Permet de définir un type de volet
	//			Int32 Instance_SHU_Time : Permet de définir un temps de montée et de descnte du volet
	//			Int32 Pin_Id_0 : Permet de connaitre l'id de la pin connecté	
	//			Int32 Pin_Id_1 : Permet de connaitre l'id de la pin connecté		
	public class Instance
	{
		//Variable propre à tous les type d'instance
		public Int32 Instance_Id;
		public string Instance_Type;
		public string Instance_Name;
		public string Instance_Direction;
		public string Instance_Note;
		public Int32 Instance_Up_Down_Stop;
		public string Instance_HashCode;
		
		//Variable permettant de savoir si l'instance est utilisé
		public bool Instance_Used;
		public Int32 Instance_Used_0;
		public Int32 Instance_Used_1;
		
		//Id des broches des pin associé
		public Int32 Pin_Id_0;
		public Int32 Pin_Id_1;
		public Int32 Pin_Id_2;
		
		//Variable propre à LIGHTING
		public Int32 Instance_LIG_DefaultValue;
		public Int32 Instance_LIG_Fade;
		
		//Variable propre à SWITCH		
		public bool Instance_SWI_Inverse;
		public Int32 Instance_SWI_ImpulsionTime;
		
		//Variable propre à SHUTTER		
		public Int32 Instance_SHU_Type; 
		public Int32 Instance_SHU_Time;
		public Int32 Instance_SHU_InitTime;
		public Int32 Instance_SHU_NumberOfOutput;
		
		//Dans le cas des instances, nous allons quatre type de constructeur pour chacune des instance
		//	ceci nous permet un certaine facilité pour boucler sur la totalité des instances
		//Le premier constructeur sera pour l'instance LIGHTING
		//Eléments : 
		//	Int32 _Id : Id de l'instance
		//	string _Type : Type de l'instance
		//	string _Name : Nom de l'instance
		//  Int32 _LIG_DefaultValue : valeur par défaut de l'instance
		//	Int32 _LIG_Fade : Pente pour la luminosité
		//	Int32 _Pin_Id_0 : Id de la pin associé
		public Instance (Int32 _Id, string _Type, string _Name, Int32 _LIG_DefaultValue, Int32 _LIG_Fade)
		{
			this.Instance_Id = _Id;
			this.Instance_Type = _Type;
			this.Instance_Name = _Name;
			this.Instance_LIG_DefaultValue = _LIG_DefaultValue;
			this.Instance_LIG_Fade = _LIG_Fade;
			this.Pin_Id_0 = 0;
			this.Instance_Direction = "OUT";
			this.Instance_Used = false;
			this.Instance_Note = "";

		}
		
		//Le second constructeur sera pour l'instance SWITCH
		//Eléments : 
		//	Int32 _Id : Id de l'instance
		//	string _Type : Type de l'instance
		//	string _Name : Nom de l'instance	
		//	bool _SWI_Inverse : Entrée inversé
		//	Int32 _SWI_ImpulsionTime : Temps d'impulsion
		//	Int32 _Pin_Id_0 : Id de la pin associé
		public Instance (Int32 _Id, string _Type, string _Name, Int32 _SWI_ImpulsionTime, bool _SWI_Inverse)
		{
			this.Instance_Id = _Id;
			this.Instance_Type = _Type;
			this.Instance_Name = _Name;
			this.Instance_SWI_Inverse = _SWI_Inverse;
			this.Instance_SWI_ImpulsionTime = _SWI_ImpulsionTime;
			this.Pin_Id_0 = 0;		
			this.Instance_Direction = "IN";
			this.Instance_Used = false;
			this.Instance_Note = "";
		}
		
		//Le troisième constructeur sera pour l'instance SHUTTER
		//Eléments : 
		//	Int32 _Id : Id de l'instance
		//	string _Type : Type de l'instance
		//	string _Name : Nom de l'instance
		//	string _SHU_Type : Type de volet roulant
		//	Int32 _SHU_Time : Temps de descente et de monté du volet roulant
		//	Int32 _Pin_Id_0 : Id de la premiere pin associé
		//	Int32 _Pin_Id_1 : Id de la seconde pin associé
		//	Int32 _Pin_Id_2 : Id de la troisième pin associé
		//	Int32 _SHU_NumberOfOutput : Nombre de sortie pour un volet
		public Instance(Int32 _Id, string _Type, string _Name, Int32 _SHU_Type, Int32 _SHU_Time, Int32 _SHU_InitTime, bool _Polymorphisme)
		{
			this.Instance_Id = _Id;
			this.Instance_Type = _Type;
			this.Instance_Name = _Name;
			this.Instance_SHU_Type = _SHU_Type;
			this.Instance_SHU_Time = _SHU_Time;
			this.Instance_SHU_InitTime = _SHU_InitTime;
			this.Pin_Id_0 = 0;
			this.Pin_Id_1 = 0;
			this.Pin_Id_2 = 0;
			this.Instance_Direction = "OUT";
			this.Instance_SHU_NumberOfOutput = 2;
			this.Instance_Used_0 = 0;
			this.Instance_Used_1 = 0;	
			this.Instance_Note = "";
			this.Instance_Up_Down_Stop = 0;
		}
		
		public Instance(Int32 _Id)
		{
			this.Instance_Id = _Id;
		}		
		
		public Instance()
		{}
	}
} 