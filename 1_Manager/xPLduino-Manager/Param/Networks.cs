using System;

namespace xPLduinoManager
{
	//Classe Networks
	//Cette classe permet de répertorier l'ensemble des réseaux
	//Elements :
	//	string Type : type du réseau de la carte
	public class Networks
	{
		public string Type;
		
		//Constructeur de la classe Networks
		//Arguments :
		//	string _Type : type du réseau
		public Networks (string _Type)
		{
			this.Type = _Type;
		}
	}
}