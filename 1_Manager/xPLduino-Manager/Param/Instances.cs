using System;

namespace xPLduinoManager
{
	public class Instances
	{
		public string InstanceType;
		public string InstanceFrenchName;
		public string InstanceEnglishName;
		
		public Instances (string _Type, string _FrenchName, string _EnglishName)
		{
			this.InstanceType = _Type;
			this.InstanceFrenchName = _FrenchName;
			this.InstanceEnglishName = _EnglishName;
		}
	}
}

