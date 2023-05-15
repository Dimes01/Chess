using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Chess.Converters
{
    public class HalfLength : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (double)value / 2;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (double)value * 2;
    }
}
