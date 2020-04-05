using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace TestRestClient.Converters
{
    class LanguageToGreetingConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string comboBoxValue = (string)values[0];
            var txt1 = values[1];
            var txt2 = values[2];
            var txt3 = values[3];
            var txt4 = values[4];
            if (comboBoxValue != null)
            {
                switch (comboBoxValue)
                {
                    case "English":
                        return txt4;
                    case "French":
                        return txt2;
                    case "Italian":
                        return txt3;
                    case "German":
                        return txt1;
                }
            }
            return txt1;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
