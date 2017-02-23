using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RobotsInterview
{
	public class BindingProxy : Freezable
	{
		public static readonly DependencyProperty DataContextProperty = DependencyProperty.Register("DataContext", typeof(object), typeof(BindingProxy),
			new PropertyMetadata(new PropertyChangedCallback(OnDataContextChanged)));

		private static void OnDataContextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
		}

		public static object GetDataContext(DependencyObject sender)
		{
			return sender.GetValue(DataContextProperty);
		}

		public static void SetDataContext(DependencyObject sender, object value)
		{
			sender.SetValue(DataContextProperty, value);
		}

		public static readonly DependencyProperty ElementProperty = DependencyProperty.Register("Element", typeof(FrameworkElement), typeof(BindingProxy),
			new PropertyMetadata(new PropertyChangedCallback(OnElementChanged)));

		public FrameworkElement Element
		{
			get { return (FrameworkElement)GetValue(ElementProperty); }
			set { SetValue(ElementProperty, value); }
		}

		private static void OnElementChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
		}

		protected override Freezable CreateInstanceCore()
		{
			return CreateInstance();
		}
	}
}
