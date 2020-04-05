using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Globalization;
using System.Net;



using RestSharp;

using CommonMVVM.ViewModel;
using CommonMVVM.Common;

using TestRestClient.Views;
using TestRestClient.Entities;


namespace TestRestClient.ViewModels
{
    class EditWindowViewModel : BaseViewModel
    {
        #region Fields
        private ICommand _saveCommand;
        private ICommand _addShowCommand;
        private CountCommand _edShowCommand;
        private CountCommand _deleteCommand;
        private DataTransferPersonCollection _dtpCollection;
        private CountriesCollection _cCollection;
        private GreetingsCollection _gCollection;
        private CurrentLanguage _currentLanguage;
        private DataTransferPerson _dtpPerson;
        private CurrentContact _cContact;
        public List<string> _countries;
        public List<string> _greetings;
        private string _selectedCountry;
        private string _selectedGreeting;
        private string _lName;
        private string _fName;
        private string _city;
        private string _street;
        private string _zip;
        private string _cpny;
        private string _title;
        private ObservableCollection<Contact> _contacts;
        #endregion

        #region Properties
        public string LName
        {
            get { return _lName; }
            set
            {
                _lName = value;
                OnPropertyChanged();
            }
        }
        public string FName
        {
            get { return _fName; }
            set
            {
                _fName = value;
                OnPropertyChanged();
            }
        }
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged();
            }
        }
        public string Street
        {
            get { return _street; }
            set
            {
                _street = value;
                OnPropertyChanged();
            }
        }
        public string Zip
        {
            get { return _zip; }
            set
            {
                _zip = value;
                OnPropertyChanged();
            }
        }
        public string Cpny
        {
            get { return _cpny; }
            set
            {
                _cpny = value;
                OnPropertyChanged();
            }
        }
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Contact> Contacts
        {
            get { return _contacts; }
            set { _contacts = value; OnPropertyChanged("Contacts"); }
        }

        public List<string> Countries
        {
            get { return _countries; }
            set { _countries = value; OnPropertyChanged("Countries"); }
        }

        public List<string> Greetings
        {
            get { return _greetings; }
            set { _greetings = value; OnPropertyChanged("Greetings"); }
        }

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand = new DelegateCommand(
                    param => this.SavePerson());
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
                System.Collections.IList contacts = (System.Collections.IList)obj;
                if (contacts != null && contacts.Count == 1)
                {
                    List<Contact> lst = contacts.Cast<Contact>().ToList();
                    _cContact.SetContact(lst);
                    return true;
                }
                return false;
            }));
            }
        }

        public CountCommand DeleteCommand
        {
            get
            {
                return _deleteCommand ??
            (_deleteCommand = new CountCommand(obj =>
            {
                //Transforms command parameter to list and passes it to a method
                System.Collections.IList contacts = (System.Collections.IList)obj;
                List<Contact> lst = contacts.Cast<Contact>().ToList();
                this.DeleteSelectedContacts(lst);
            },
            (obj) => {
                //IsExecutable 
                System.Collections.IList contacts = (System.Collections.IList)obj;
                if (contacts != null)
                {
                    return true;
                }
                return false;
            }));
            }
        }

        public ICommand AddShowCommand
        {
            get
            {
                return _addShowCommand = new DelegateCommand(
                    param => this.ShowAddWindow());
            }
        }

        public string SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                _selectedCountry = value;
                OnPropertyChanged("SelectedCountry");
            }
        }




        public string SelectedGreeting
        {
            get { return _selectedGreeting; }
            set
            {
                _selectedGreeting = value;
                OnPropertyChanged("SelectedGreeting");
            }
        }
        #endregion

        #region ClassConstructors
        public EditWindowViewModel()
        {
            _cContact = CurrentContact.GetInstance();
            _currentLanguage = CurrentLanguage.GetInstance();
            _cCollection = CountriesCollection.GetInstance();
            _gCollection = GreetingsCollection.GetInstance();
            _dtpCollection = DataTransferPersonCollection.GetInstance();
            _dtpPerson = _dtpCollection.GetSelected();
            Countries = GetCountries(_currentLanguage.GetLanguage());
            Greetings = GetGreetings(_currentLanguage.GetLanguage());
            if (_dtpPerson != null)
            {
                Contacts = new ObservableCollection<Contact>(_dtpPerson.contacts);
                LName = _dtpPerson.lName;
                FName = _dtpPerson.fName;
                City = _dtpPerson.city;
                Street = _dtpPerson.street;
                Zip = _dtpPerson.zip;
                Cpny = _dtpPerson.cpny;
                Title = _dtpPerson.title;
                SelectedCountry = CountryIdentifierByLang(_dtpPerson, _currentLanguage.GetLanguage());
                SelectedGreeting = GreetingIdentifierByLang(_dtpPerson, _currentLanguage.GetLanguage());
            }
        }
        #endregion

        //Opens new instance of window for contact editing 
        void ShowEditWindow()
        {
            var win = new EditContactView();
            win.ShowDialog();
            Contacts = new ObservableCollection<Contact>(_dtpPerson.contacts);
            _cContact.RemoveContact();
        }

        //Deleted selected contacrs from datagrid
        void DeleteSelectedContacts(List<Contact> contacts)
        {
            foreach(var item in contacts)
            {
                _dtpPerson.contacts.Remove(item);
            }
            Contacts = new ObservableCollection<Contact>(_dtpPerson.contacts);
        }

        //Opens new instance of window for contact adding
        void ShowAddWindow()
        {
            if(_dtpCollection.GetSelected() == null)
            {
                SavePerson();
            }
            _cContact.RemoveContact();
            var win = new EditContactView();
            win.ShowDialog();
            Contacts = new ObservableCollection<Contact>(_dtpPerson.contacts);
        }

        //Saves created of edited person in collection
        void SavePerson()
        {
            var country = _cCollection.GetCountryByTextAndLanguage(SelectedCountry, _currentLanguage.GetLanguage());
            var greeting = _gCollection.GetGreetingByTextAndLanguage(SelectedGreeting, _currentLanguage.GetLanguage());
            if (_dtpPerson != null)
            {
                _dtpPerson.lName = LName;
                _dtpPerson.fName = FName;
                _dtpPerson.city = City;
                _dtpPerson.street = Street;
                _dtpPerson.zip = Zip;
                _dtpPerson.cpny = Cpny;
                _dtpPerson.title = Title;
                _dtpPerson.txt1 = country.txt1;
                _dtpPerson.txt2 = country.txt2;
                _dtpPerson.txt3 = country.txt3;
                _dtpPerson.txt4 = country.txt4;
                _dtpPerson.greetingTxt1 = greeting.txt1;
                _dtpPerson.greetingTxt2 = greeting.txt2;
                _dtpPerson.greetingTxt3 = greeting.txt3;
                _dtpPerson.greetingTxt4 = greeting.txt4;
                _dtpCollection.Edit(_dtpPerson);
            }
            else
            {
                _dtpPerson = new DataTransferPerson();
                _dtpPerson.lName = LName;
                _dtpPerson.fName = FName;
                _dtpPerson.city = City;
                _dtpPerson.street = Street;
                _dtpPerson.zip = Zip;
                _dtpPerson.cpny = Cpny;
                _dtpPerson.title = Title;
                _dtpPerson.txt1 = country.txt1;
                _dtpPerson.txt2 = country.txt2;
                _dtpPerson.txt3 = country.txt3;
                _dtpPerson.txt4 = country.txt4;
                _dtpPerson.firstContact = DateTime.Now;
                _dtpPerson.dateOfBirth = null;
                _dtpPerson.greetingTxt1 = greeting.txt1;
                _dtpPerson.greetingTxt2 = greeting.txt2;
                _dtpPerson.greetingTxt3 = greeting.txt3;
                _dtpPerson.greetingTxt4 = greeting.txt4;
                _dtpPerson.contacts = new List<Contact>();
                _dtpPerson.IsSelected = true;
                _dtpPerson.id = _dtpCollection.AddNew(_dtpPerson);
            }
        }

        //Identifies selected country
        string CountryIdentifierByLang(DataTransferPerson person, string lang)
        {
            switch (lang)
            {
                case "English":
                    return person.txt4;
                case "French":
                    return person.txt2;
                case "Italian":
                    return person.txt3;
                case "German":
                    return person.txt1;
            }
            return person.txt4;
        }

        //Idenifies greeting by selected language
        string GreetingIdentifierByLang(DataTransferPerson person, string lang)
        {
            switch (lang)
            {
                case "English":
                    return person.greetingTxt4;
                case "French":
                    return person.greetingTxt2;
                case "Italian":
                    return person.greetingTxt3;
                case "German":
                    return person.greetingTxt1;
            }
            return person.greetingTxt4;
        }

        //Creates a collection of countries names depending on selected language 
        List<string> GetCountries(string lang)
        {
            List<string> clst = new List<string>();
            var lst = _cCollection.GetCollection();
            switch (lang)
            {
                case "English":
                    foreach (var item in lst)
                    {
                        if (item.txt4 != null)
                            clst.Add(item.txt4);
                    }
                    break;
                case "French":
                    foreach (var item in lst)
                    {
                        if (item.txt2 != null)
                            clst.Add(item.txt2);
                    }
                    break;
                case "Italian":
                    foreach (var item in lst)
                    {
                        if (item.txt3 != null)
                            clst.Add(item.txt3);
                    }
                    break;
                case "German":
                    foreach (var item in lst)
                    {
                        if (item.txt1 != null)
                            clst.Add(item.txt1);
                    }
                    break;
            }
            return clst;
        }

        //Creates a collection of greetings depending on selected language 
        List<string> GetGreetings(string lang)
        {
            List<string> glst = new List<string>();
            var lst = _gCollection.GetCollection();
            switch (lang)
            {
                case "English":
                    foreach (var item in lst)
                    {
                        if (item.txt4 != null)
                            glst.Add(item.txt4);
                    }
                    break;
                case "French":
                    foreach (var item in lst)
                    {
                        if (item.txt2 != null)
                            glst.Add(item.txt2);
                    }
                    break;
                case "Italian":
                    foreach (var item in lst)
                    {
                        if (item.txt3 != null)
                            glst.Add(item.txt3);
                    }
                    break;
                case "German":
                    foreach (var item in lst)
                    {
                        if (item.txt1 != null)
                            glst.Add(item.txt1);
                    }
                    break;
            }
            return glst;
        }
    }
}
