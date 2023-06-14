using System;
using System.Globalization;
using System.Windows.Data;

namespace Chess.Converters
{
	public class RadioButtonConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int integer = (int)value;
			if (integer == int.Parse(parameter.ToString()))
				return true;
			else
				return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value == true)
				return parameter;
			else
				return null;
		}
	}
}
