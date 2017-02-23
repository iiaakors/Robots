using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotsInterview
{
	public class Images
	{
		public const string Blue = "/Resources/BlueArrow.png";
		public const string Green = "/Resources/GreenArrow.png"	 ;
		public const string Orange = "/Resources/OrangeArrow.png";
		public const string Purple = "/Resources/PurpleArrow.png";
	}

	public class RobotColors
	{
		public const string Orange = "#ff6f37";
		public const string Blue = "#008eff";
		public const string Green = "#439c4a";
		public const string Purple = "#ce37ff";
	}

	public enum DirectionTypes
	{
		North = 270,
		South = 90,
		East = 360,
		West = 180
	}
}
