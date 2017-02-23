using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace RobotsInterview
{
	public class MoveBehavior : DependencyObject
	{

		public static readonly DependencyProperty InstructionProperty = DependencyProperty.Register("Instruction", typeof(Instruction), typeof(MoveBehavior),
			new PropertyMetadata(new PropertyChangedCallback(OnInstructionChanged)));

		public Instruction Instruction
		{
			get { return (Instruction)GetValue(InstructionProperty); }
			set { SetValue(InstructionProperty, value); }
		}

		private static Queue<Action> tasks = new Queue<Action>();

		[SecurityPermissionAttribute(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		static void DoEvents()
		{
			DispatcherFrame frame = new DispatcherFrame();
			AppDispatcher.Invoke(DispatcherPriority.ApplicationIdle,
				new DispatcherOperationCallback(ExitFrame), frame);
			Dispatcher.PushFrame(frame);
		}

		static object ExitFrame(object f)
		{
			((DispatcherFrame)f).Continue = false;

			return null;
		}

		private static Dispatcher AppDispatcher = Application.Current.Dispatcher;

		private static void ProcessNextTask(MoveBehavior behavior)
		{
			if (tasks.Count == 0)
			{
				behavior.Instruction.Robot.IsMoving = false;
				
			}
			else
				tasks.First()();
			//DoEvents();
			
		}

		private static void OnInstructionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue == null) return;
			MoveBehavior move = sender as MoveBehavior;
			if (move.Element == null) return;

			Instruction instruction = e.NewValue as Instruction;
			if (Canvas.GetLeft(move.Element) != (double)converter.Convert(instruction.Position.X, null, "L", null))
			{
				tasks.Enqueue(() => { MoveLeft(move, instruction, 0); });
			}

			if (Canvas.GetTop(move.Element) != (double)converter.Convert(instruction.Position.Y, null, "T", null))
			{
				tasks.Enqueue(() => { MoveTop(move, instruction, 0); });
			}
			tasks.Enqueue(() =>
			{
				Rotate(move, (double)instruction.InitialDirection);
			});

			foreach (var m in instruction.Moves)
			{
				tasks.Enqueue(() =>
				{
					switch (m.ToLower())
					{
						case "m":
							{
								switch (instruction.Robot.Direction)
								{
									case DirectionTypes.East:
										{
											var currenttop = Canvas.GetTop(move.Element);
											var result = (instruction.Position.X + 1);
											if (result > 10)
											{
												tasks.Dequeue();
												ProcessNextTask(move);
												break;
											}
											instruction.Position = new Point(result, instruction.Position.Y);
											MoveLeft(move, instruction);
										} break;
									case DirectionTypes.West:
										{
											var result = (instruction.Position.X - 1);
											if (result < 0)
											{
												tasks.Dequeue();
												ProcessNextTask(move);
												break;
											}
											instruction.Position = new Point(result, instruction.Position.Y);
											MoveLeft(move, instruction);
										} break;
									case DirectionTypes.North:
										{
											var result = (instruction.Position.Y + 1);
											if (result > 10)
											{
												tasks.Dequeue();
												ProcessNextTask(move);
												break;
											}
											instruction.Position = new Point(instruction.Position.X, result);
											MoveTop(move, instruction);
										} break;
									case DirectionTypes.South:
										{
											var result = (instruction.Position.Y - 1);
											if (result < 0)
											{
												tasks.Dequeue();
												ProcessNextTask(move);
												break;
											}
											instruction.Position = new Point(instruction.Position.X, result);
											MoveTop(move, instruction);
										} break;
								}
							} break;
						case "l":
							{
								switch (instruction.Robot.Direction)
								{
									case DirectionTypes.East:
										{
											Rotate(move, (double)DirectionTypes.North);
										} break;
									case DirectionTypes.South:
										{
											Rotate(move, (double)DirectionTypes.East);
										} break;
									case DirectionTypes.West:
										{
											Rotate(move, (double)DirectionTypes.South);
										} break;
									case DirectionTypes.North:
										{
											Rotate(move, (double)DirectionTypes.West);
										} break;
								}

							} break;
						case "r":
							{
								switch (instruction.Robot.Direction)
								{
									case DirectionTypes.East:
										{
											Rotate(move, (double)DirectionTypes.South);
										} break;
									case DirectionTypes.South:
										{
											Rotate(move, (double)DirectionTypes.West);
										} break;
									case DirectionTypes.West:
										{
											Rotate(move, (double)DirectionTypes.North);
										} break;
									case DirectionTypes.North:
										{
											Rotate(move, (double)DirectionTypes.East);
										} break;
								}
							} break;
					}
				});
			}

			ProcessNextTask(move);
		}

		private static LocationConverter converter = new LocationConverter();

		private static void MoveTop(MoveBehavior move, Instruction instruction, double seconds = .5)
		{
			AppDispatcher.BeginInvoke(new Action(() =>
			{
				DoubleAnimation anim = new DoubleAnimation();
				anim.From = (double)converter.Convert(instruction.Robot.Top, null, "T", null);
				anim.To = (double)converter.Convert(instruction.Position.Y, null, "T", null);
				anim.Duration = new Duration(TimeSpan.FromSeconds(seconds));
				anim.Completed += (oo, ee) =>
				{
					move.Instruction.Robot.SetCurrentTop(instruction.Position.Y);
					tasks.Dequeue();
					ProcessNextTask(move);
				};

				instruction.Robot.IsMoving = true;
				move.Element.BeginAnimation(Canvas.TopProperty, anim);

			}));
		}

		private static void MoveLeft(MoveBehavior move, Instruction instruction, double seconds = .5)
		{
			AppDispatcher.BeginInvoke(new Action(() =>
			{
			DoubleAnimation anim = new DoubleAnimation();
			anim.From = (double)converter.Convert(instruction.Robot.Left, null, "L", null);
			anim.To = (double)converter.Convert(instruction.Position.X, null, "L", null);
			anim.Duration = new Duration(TimeSpan.FromSeconds(seconds));
			anim.Completed += (oo, ee) =>
			{
				move.Instruction.Robot.SetCurrentLeft(instruction.Position.X);
				tasks.Dequeue();
				ProcessNextTask(move);
			};
			
				instruction.Robot.IsMoving = true;
				move.Element.BeginAnimation(Canvas.LeftProperty, anim);
			}));
		}


		private static void Rotate(MoveBehavior move, double desiredangle)
		{
			AppDispatcher.BeginInvoke(new Action(() =>
			{
			DoubleAnimation anim = new DoubleAnimation();
			var elem = move.Element;
			var angle = ((RotateTransform)elem.LayoutTransform).Angle;
			anim.Duration = new Duration(TimeSpan.FromSeconds(.2));
			anim.AutoReverse = false;
			anim.IsCumulative = false;
			anim.From = angle;
			anim.Completed += (oo, ee) =>
			{
				move.Instruction.Robot.SetCurrentDirection((DirectionTypes)desiredangle);
				tasks.Dequeue();
				ProcessNextTask(move);
			};
			switch ((DirectionTypes)desiredangle)
			{
				case DirectionTypes.East:
					{
						if (angle == 90)
							anim.To = angle - 90;
						else if (angle == 270)
							anim.To = angle + 90;
						else anim.To = 360;
					} break;
				case DirectionTypes.West:
					{
						if (angle == 90)
							anim.To = angle + 90;
						else if (angle == 270)
							anim.To = angle - 90;
						else anim.To = 180;
					} break;
				case DirectionTypes.North:
					{
						if (angle == 360)
							anim.To = angle - 90;
						else if (angle == 0)
						{
							anim.To = 270;
							anim.From = 360;
						}
						else if (angle == 180)
							anim.To = angle + 90;
						else anim.To = 270;
					} break;
				case DirectionTypes.South:
					{
						if (angle == 360)
						{
							anim.To = 90;
							anim.From = 0;
						}
						else if (angle == 180)
							anim.To = angle - 90;
						else anim.To = 90;
					} break;

			}

			
				move.Instruction.Robot.IsMoving = true;
				if (((RotateTransform)elem.LayoutTransform).IsFrozen)
					elem.LayoutTransform = elem.LayoutTransform.CloneCurrentValue();
				((RotateTransform)elem.LayoutTransform).BeginAnimation(RotateTransform.AngleProperty, anim);
			}));
		}

		public static readonly DependencyProperty ElementProperty = DependencyProperty.Register("Element", typeof(FrameworkElement), typeof(MoveBehavior),
			new PropertyMetadata(new PropertyChangedCallback(OnElementChanged)));

		public FrameworkElement Element
		{
			get { return (FrameworkElement)GetValue(ElementProperty); }
			set { SetValue(ElementProperty, value); }
		}

		public Image Image { get; set; }

		private static void OnElementChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
		}
	}
}
