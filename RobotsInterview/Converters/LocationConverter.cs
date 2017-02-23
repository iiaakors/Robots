using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RobotsInterview
{
	public class LocationConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if(parameter.ToString() == "T")
				return ((10-(double)value) * 60)-10;
			else return (((double)value) * 60) - 10;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return System.Convert.ChangeType(value, targetType);
		}
	}
}
