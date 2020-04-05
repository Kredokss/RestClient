using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TestRestClient.ViewModels;
using TestRestClient.Entities;

namespace TestRestClient.Views
{
    /// <summary>
    /// Логика взаимодействия для EditContactView.xaml
    /// </summary>
    public partial class EditContactView : Window
    {
        public EditContactView()
        {
            InitializeComponent();

            DataContext = new EditContactViewModel();
        }
    }
}
