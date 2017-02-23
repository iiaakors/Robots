using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotsInterview
{
	public class Arena
	{
		private ObservableCollection<Robot> robots;
		public ObservableCollection<Robot> Robots
		{
			get
			{
				return (robots ?? (robots = new ObservableCollection<Robot>(new Robot[4] 
			{ 
				new Robot(1,5, Images.Blue, RobotColors.Blue, "Optimus Prime", DirectionTypes.North),
				new Robot(2,6, Images.Green, RobotColors.Green, "Brawn", DirectionTypes.South),
				new Robot(3,7, Images.Orange, RobotColors.Orange, "JetFire", DirectionTypes.East),
				new Robot(4,8, Images.Purple, RobotColors.Purple, "LugNut", DirectionTypes.West)
			})));
			}
		}

		
	}
}
