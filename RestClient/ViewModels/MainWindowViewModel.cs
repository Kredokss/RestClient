using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Net;


using RestSharp;

using CommonMVVM.ViewModel;
using CommonMVVM.Common;

using TestRestClient.Views;
using TestRestClient.Entities;

namespace TestRestClient.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        #region Fields
        public static string BaseUrl = "https://localhost:5001";


        private CountCommand _edShowCommand;
        private ICommand _deleteCommand;
        private ICommand _refreshCommand;
        private ICommand _addCommand;
        private string _selectedLanguage;
        private string _srchValue;
        private List<string> _property;
        private DataTransferPersonCollection _dtpCollection;
        private CurrentLanguage _currentLanguage;

        private ObservableCollection<DataTransferPerson> _persons;

        #endregion

        #region Properties
        public List<string> Property
        {
            get
            {
                return new List<string>() { "English", "French", "Italian", "German" };
            }
            set
            {
                _property = value;
            }
        }


        public ObservableCollection<DataTransferPerson> Persons
        {
            get { return _persons; }
            set { _persons = value; OnPropertyChanged("Persons"); }
        }

        public string SrchValue
        {
            get { return _srchValue; }
            set
            {
                _srchValue = value;
                SearchByText(_srchValue);
                OnPropertyChanged("SrchValue");
            }
        }
        public string SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                _currentLanguage.SetLanguage(value);
                _selectedLanguage = value;
                OnPropertyChanged("SelectedLanguage");
            }
        }


        public CountCommand EdShowCommand
        {
            get
            {
                return _edShowCommand ??
            (_edShowCommand = new CountCommand(obj =>
            {
                this.ShowEditWindow();
            },
            (obj) => {
                System.Collections.IList persons = (System.Collections.IList)obj;
                if (persons != null && persons.Count == 1)
                {
                    List<DataTransferPerson> lst = persons.Cast<DataTransferPerson>().ToList();
                    _dtpCollection.SetSelected(lst);
                    return true;
                }
                return false;
            }));
            }
        }
        public ICommand DeleteCommand
        {
            get
            {
                return _deleteCommand = new DelegateCommand(
                    param => this.RemovePersons());
            }
        }
        public ICommand AddCommand
        {
            get
            {
                return _addCommand = new DelegateCommand(
                    param => this.ShowAddWindow());
            }
        }
        public ICommand RefreshCommand
        {
            get
            {
                return _refreshCommand = new DelegateCommand(
                    param => this.RefreshPersons());
            }
        }

        #endregion


        #region ClassConstructors
        public MainWindowViewModel()
        {
            _currentLanguage = CurrentLanguage.GetInstance();
            SelectedLanguage = "English";
            _dtpCollection = DataTransferPersonCollection.GetInstance();
            Persons = GetPersons() ;
        }
        #endregion

        
        //Searches for matches between manually entered text and values from collection
        void SearchByText(string text)
        {
            Persons = GetPersons();
            if (text != null)
            {
                ObservableCollection<DataTransferPerson> newPersons = new ObservableCollection<DataTransferPerson>();
                foreach (var item in Persons)
                {
                    if (item.fName != null && item.fName.Contains(text) || item.lName != null && item.lName.Contains(text) || item.cpny != null && item.cpny.Contains(text) ||
                        item.cpny != null && item.cpny.Contains(text))
                    {
                        newPersons.Add(item);
                    }
                }
                Persons = newPersons;
            }
        }

        void ShowEditWindow()
        {
            var win = new EditWindowView();
            win.ShowDialog();
            UpdatePersonDb();
            foreach (var item in Persons)
            {
                item.IsSelected = false;
            }
            SearchByText(SrchValue);
        }

        void ShowAddWindow()
        {
            foreach (var item in Persons)
            {
                item.IsSelected = false;
            }
            var win = new EditWindowView();
            win.ShowDialog();
            AddPersonDb();
            SearchByText(SrchValue);
        }

        //Send a reques for a new list to a database
        void RefreshPersons()
        {
            _dtpCollection.Refresh();
            SearchByText(SrchValue);
        }

        //Remove selected persons
        void RemovePersons()
        {
            var selectedItems = Persons.Where(i => i.IsSelected).ToList();
            foreach (var item in selectedItems)
            {
                Persons.Remove(item);
                DeletePerson(item.id);
                _dtpCollection.Remove(item);
            }
            foreach (var item in Persons)
            {
                item.IsSelected = false;
            }
            SearchByText(SrchValue);
        }

        //Add a single person to the project database
        void AddPersonDb()
        {
            RestClient client = new RestClient(BaseUrl);
            var request = new RestRequest("/api/person", Method.POST);
            request.RequestFormat = RestSharp.DataFormat.Json;
            if (_dtpCollection.GetSelected() != null)
            {
                request.AddJsonBody(_dtpCollection.GetSelected());
                try
                {
                    client.ExecuteAsync(request, response =>
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                        }
                        else
                        {
                            MessageBox.Show("Error");
                        }
                    });
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.ToString());
                }
            }
        }

        //Update a single person in the project database
        void UpdatePersonDb()
        {
            RestClient client = new RestClient(BaseUrl);
            var request = new RestRequest("/api/person", Method.PUT);
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddJsonBody(_dtpCollection.GetSelected());
            try
            {
                client.ExecuteAsync(request, response =>
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                    }
                    else
                    {
                        MessageBox.Show("Error");
                    }
                });
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            }
        }
        //Get currently stored objects from DataTransferPersonCollection
        ObservableCollection<DataTransferPerson> GetPersons()
        {
            var persons = new ObservableCollection<DataTransferPerson>();
            var tmp = _dtpCollection.GetCollection();
            foreach (var item in tmp)
            {
                persons.Add(item);
            }
            return persons;
        }


        //Delete person/people from the project database
        void DeletePerson(int id)
        {
            RestClient client = new RestClient(BaseUrl);
            var request = new RestRequest("/api/person/{id}", Method.DELETE)
                .AddParameter("id", id, ParameterType.UrlSegment);
            try
            {
                client.ExecuteAsync(request, response =>
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                    }
                    else
                    {
                        MessageBox.Show("Error");
                    }
                });
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            }
        }
    }
}
