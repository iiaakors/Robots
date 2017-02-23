using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RobotsInterview
{
	public class Robot : INotifyPropertyChanged
	{
		public delegate void MoveCompletedEventHandler(Robot robot);
		public event MoveCompletedEventHandler MoveComplete;

		public Robot(double top, double left, string image, string color, string name, DirectionTypes direction)
			: base()
		{
			ID = Guid.NewGuid().ToString();
			Top = top;
			Left = left;
			Image = image;
			Color = color;
			Name = name;
			Direction = direction;
		}

		public override bool Equals(object obj)
		{
			return (obj is Robot ? ((obj as Robot).ID == ID) : false);
		}

		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}

		private string directionString;
		public string DirectionString
		{
			get { return (directionString ?? (directionString = Enum.GetName(typeof(DirectionTypes), Direction).Substring(0, 1))); }
			set
			{
				directionString = value;
				FireChange("DirectionString");
				if (value == null) return;
				switch (value.ToLower())
				{
					case "n": { Direction = DirectionTypes.North; } break;
					case "s": { Direction = DirectionTypes.South; } break;
					case "e": { Direction = DirectionTypes.East; } break;
					case "w": { Direction = DirectionTypes.West; } break;
				}
			}
		}

		private DirectionTypes direction;
		public DirectionTypes Direction
		{
			get { return direction; }
			set
			{
				direction = value;
				FireChange("Direction");
				Angle = (double)value;
				directionString = null;
				FireChange("DirectionString");
			}
		}

		public string ID { get; private set; }

		public string Image { get; private set; }

		public string Name { get; private set; }

		private string statusText = "Stopped";
		public string StatusText
		{
			get { return statusText; }
			set
			{
				statusText = value;
				FireChange("StatusText");
			}
		}

		private double angle;
		public double Angle
		{
			get { return angle; }
			set
			{
				angle = value;
				FireChange("Angle");
			}
		}

		private double left;
		public double Left
		{
			get { return left; }
			set
			{
				left = value;
				FireChange("Left");
			}
		}

		private double top;
		public double Top
		{
			get { return top; }
			set
			{
				top = value;
				FireChange("Top");
			}
		}

		private bool isawaiting;
		public bool IsAwaiting
		{
			get { return isawaiting; }
			set
			{
				isawaiting = value;
				FireChange("IsAwaiting");
				if(value)
					StatusText = "Awaiting Turn";
				ProcessInstructionsCommand.RaiseCanExecuteChanged();
			}
		}

		private bool ismoving;
		public bool IsMoving
		{
			get { return ismoving; }
			set
			{
				ismoving = value;
				StatusText = value ? "Moving" : "Stopped";
				if (value)
					IsAwaiting = false;
				FireChange("IsMoving");
				if (!value)
					if (MoveComplete != null)
						MoveComplete(this);
				ProcessInstructionsCommand.RaiseCanExecuteChanged();
			}
		}

		public string Color { get; private set; }

		private DelegateCommand processInstructionsCommand;
		public DelegateCommand ProcessInstructionsCommand
		{
			get
			{
				return (processInstructionsCommand ?? (processInstructionsCommand = new DelegateCommand(
					() =>
					{
						if (!ProcessInstructionsCommand.CanExecute(null)) return;
						TrafficControl.AddToQueue(this);
						ProcessInstructionsCommand.RaiseCanExecuteChanged();
					},
					() => { return (!string.IsNullOrEmpty(XYInstruction) || !string.IsNullOrEmpty(RouteInstruction)) && !IsMoving && !IsAwaiting; })));
			}
		}

		private Instruction instruction;
		public Instruction Instruction
		{
			get { return instruction; }
			set
			{
				instruction = value;
				FireChange("Instruction");
			}
		}

		public void SetCurrentLeft(double left)
		{
			this.left = left;
			FireChange("Left");
		}

		public void SetCurrentTop(double top)
		{
			this.top = top;
			FireChange("Top");
		}

		public void SetCurrentDirection(DirectionTypes direction)
		{
			this.direction = direction;
			directionString = null;
			FireChange("Direction");
			FireChange("DirectionString");
		}

		public void ProcessInstruction()
		{
			Instruction instruction = new Instruction(this);
			if (!string.IsNullOrEmpty(XYInstruction) && !string.IsNullOrWhiteSpace(XYInstruction))
			{
				double x = double.Parse(XYInstruction[0].ToString());
				double y = double.Parse(XYInstruction[2].ToString());
				string dir = XYInstruction.Length < 5 ? "n" : XYInstruction[4].ToString();
				instruction.Position = new System.Windows.Point(x, y);
				switch (dir.ToLower())
				{
					case "n": { instruction.InitialDirection = DirectionTypes.North; } break;
					case "s": { instruction.InitialDirection = DirectionTypes.South; } break;
					case "e": { instruction.InitialDirection = DirectionTypes.East; } break;
					case "w": { instruction.InitialDirection = DirectionTypes.West; } break;
				}
			}
			else
			{
				string dir = DirectionString;
				instruction.Position = new System.Windows.Point(Left, Top);
				instruction.InitialDirection = Direction;
			}
			instruction.Moves = string.IsNullOrWhiteSpace(RouteInstruction) || string.IsNullOrEmpty(RouteInstruction) ? new string[0] :
				RouteInstruction.Where(c => !char.IsWhiteSpace(c) && char.IsLetter(c) && new string[] { "m", "l", "r" }.Contains(c.ToString().ToLower())).Select(c => c.ToString()).ToArray();

			Instruction = instruction;
		}

		private string xyInstruction;
		public string XYInstruction
		{
			get { return xyInstruction; }
			set
			{
				xyInstruction = value;
				FireChange("XYInstruction");
				ProcessInstructionsCommand.RaiseCanExecuteChanged();
			}
		}

		private string routeInstruction;
		public string RouteInstruction
		{
			get { return routeInstruction; }
			set
			{
				routeInstruction = value;
				FireChange("RouteInstruction");
				ProcessInstructionsCommand.RaiseCanExecuteChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void FireChange(string propertyname)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
		}
	}
}
