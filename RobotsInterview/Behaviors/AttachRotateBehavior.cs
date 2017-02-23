using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace RobotsInterview
{
	public class AttachRotateBehavior
	{
		public static readonly DependencyProperty AngleProperty = DependencyProperty.RegisterAttached("Angle", typeof(double), typeof(AttachRotateBehavior),
			new PropertyMetadata(new PropertyChangedCallback(OnAngleChanged)));

		public static double GetAngle(DependencyObject sender)
		{
			return (double)sender.GetValue(AngleProperty);
		}

		public static void SetAngle(DependencyObject sender, object value)
		{
			sender.SetValue(AngleProperty, value);
		}

		private static bool isanimationcomplete = true;

		private static void OnAngleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (!isanimationcomplete) return;
			DoubleAnimation anim = new DoubleAnimation();
			FrameworkElement elem = sender as FrameworkElement;
			var angle = ((RotateTransform)elem.LayoutTransform).Angle;
			anim.Duration = new Duration(TimeSpan.FromSeconds(.5));
			anim.AutoReverse = false;
			anim.IsCumulative = false;
			anim.From = angle;
			anim.Completed += (o, ee) =>
			{
				isanimationcomplete = true;
			};
			switch ((DirectionTypes)(double)e.NewValue)
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

			Application.Current.Dispatcher.BeginInvoke(new Action(() =>
			{
				if (((RotateTransform)elem.LayoutTransform).IsFrozen)
					elem.LayoutTransform = elem.LayoutTransform.CloneCurrentValue();
				((RotateTransform)elem.LayoutTransform).BeginAnimation(RotateTransform.AngleProperty, anim);
				isanimationcomplete = false;
			}));
		}
	}
}
