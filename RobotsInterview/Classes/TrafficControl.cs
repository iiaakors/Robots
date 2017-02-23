using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotsInterview
{
	public class TrafficControl
	{
		private static Queue<Robot> instructionQueue;
		public static Queue<Robot> InstructionQueue
		{
			get { return (instructionQueue ?? (instructionQueue = new Queue<Robot>())); }
		}

		public static void AddToQueue(Robot robot)
		{
			InstructionQueue.Enqueue(robot);
			robot.IsAwaiting = true;
			robot.MoveComplete += robot_MoveComplete;
			ProcessNext();
		}

		static void ProcessNext()
		{
			if (InstructionQueue.Count == 0) return;
			if (InstructionQueue.Peek().IsMoving) return;

			try
			{
				InstructionQueue.Peek().ProcessInstruction();
				InstructionQueue.Peek().IsMoving = true;
			}
			catch (Exception ex)
			{
				InstructionQueue.Peek().IsMoving = false;
				InstructionQueue.Dequeue();
				throw ex;
			}
		}

		static void robot_MoveComplete(Robot robot)
		{
			robot.MoveComplete -= robot_MoveComplete;
			robot.IsMoving = false;
			InstructionQueue.Dequeue();
			ProcessNext();
		}
	}
}
