using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RobotsInterview
{
	public class AttachMoveBehavior
	{
		public static readonly DependencyProperty MoveBehaviorProperty = DependencyProperty.RegisterAttached("MoveBehavior", typeof(MoveBehavior),
			typeof(AttachMoveBehavior), new PropertyMetadata(new PropertyChangedCallback(OnMoveBehaviorChanged)));

		public static MoveBehavior GetMoveBehavior(DependencyObject sender)
		{
			return (MoveBehavior)sender.GetValue(MoveBehaviorProperty);
		}

		public static void SetMoveBehavior(DependencyObject sender, object value)
		{
			sender.SetValue(MoveBehaviorProperty, value);
		}

		private static void OnMoveBehaviorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
		}
	}
}
