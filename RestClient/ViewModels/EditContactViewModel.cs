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


using RestSharp;

using CommonMVVM.ViewModel;
using CommonMVVM.Common;


using TestRestClient.Views;
using TestRestClient.Entities;

namespace TestRestClient.ViewModels
{
    class EditContactViewModel : BaseViewModel
    {
        #region Fields
        private DataTransferPersonCollection _dtpCollection;
        private DataTransferPerson _dtpPerson;
        private ICommand _saveCommand;
        private List<string> _contactType;
        private string _selectedType;
        private string _contactTxt;
        private CurrentContact _cContact;
        private Contact _contact;

        #endregion

        #region Properties

        public string ContactTxt
        {
            get
            {
                { return _contactTxt; }
            }
            set
            {
                _contactTxt = value;
            }
        }
        public List<string> ContactType
        {
            get
            {
                return new List<string>() { "Phone", "E-mail" };
            }
            set
            {
                _contactType = value;
            }
        }

        public string SelectedType
        {
            get { return _selectedType; }
            set
            {
                _selectedType = value;
                OnPropertyChanged("SelectedType");
            }
        }
        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand = new DelegateCommand(
                    param => this.SaveContacts());
            }
        }

        #endregion

        #region ClassConstructors

        public EditContactViewModel()
        {
            _dtpCollection = DataTransferPersonCollection.GetInstance();
            _dtpPerson = _dtpCollection.GetSelected();
            _cContact = CurrentContact.GetInstance();
            _contact = _cContact.GetContact();
            if (_contact != null)
            {
                SelectedType = TypeFromNumber(_contact.personContactId);
                ContactTxt = _contact.personContactTxt;
            }
        }

        #endregion


        //Convert type id to a txt
        string TypeFromNumber(int num)
        {
            if(num == 1)
            { 
                return "Phone"; 
            }
            if (num == 2)
            {
                return "E-mail";
            }
            return null;
        }

        //Convert txt to a type id
        Nullable<int> NumberFromType(string type)
        {
            if (type != null)
            {
                if (type.Equals("Phone"))
                {
                    return 1;
                }
                if (type.Equals("E-mail"))
                {
                    return 2;
                }
            }
            return null;
        }

        //Add new or edit a contact
        void SaveContacts()
        {
            if (NumberFromType(SelectedType) == null)
            {
                MessageBox.Show("Choose Type");
            }
            else
            {
                if (_contact != null)
                {
                    _dtpCollection.ReplaceContact(_dtpPerson.id, (int)NumberFromType(SelectedType), ContactTxt, _contact);
                }
                else
                {
                    _dtpCollection.AddContact(_dtpPerson.id, (int)NumberFromType(SelectedType), ContactTxt);
                }
            }
        }
    }
}
