using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RobotsInterview
{
	public class Instruction
	{
		public Instruction(Robot robot)
		{
			Robot = robot;
		}

		public Robot Robot { get; private set; }

		public Point Position { get; set; }

		public DirectionTypes InitialDirection { get; set; }

		public string[] Moves { get; set; }
		
	}
}
