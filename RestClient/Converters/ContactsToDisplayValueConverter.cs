using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using TestRestClient.Entities;
using System.Collections.Generic;

namespace TestRestClient.Converters
{
    class ContactsToDisplayValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var contacts = (List<Contact>)values[0];
            foreach(var item in contacts)
            {
                if(item.personContactId == 1)
                {
                    return item.personContactTxt;
                }
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
