using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using TestRestClient.Entities;
using System.Collections.Generic;

namespace TestRestClient.Converters
{
    class NumberToContactTypeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (int)values[0];
            if(val == 1)
            {
                return "Phone";
            }
            if(val == 2)
            {
                return "E-mail";
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
