using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using RestSharp;

namespace TestRestClient.Entities
{
    //Class that stores collection of people obtained from database
    class DataTransferPersonCollection
    {
        private static DataTransferPersonCollection _instance;
        private List<DataTransferPerson> _pCollection;


        private DataTransferPersonCollection()
        {
            _pCollection = new List<DataTransferPerson>();
            RestClient client = new RestClient(ViewModels.MainWindowViewModel.BaseUrl);
            RestRequest request = new RestRequest("/api/person", Method.GET);
            IRestResponse<List<DataTransferPerson>> response = client.Execute<List<DataTransferPerson>>(request);
            if (response.ErrorException == null)
            {
                foreach (var item in response.Data)
                {
                    _pCollection.Add(item);
                }
            }
            else
            {
                MessageBox.Show("Connection error");
            }
        }

        public static DataTransferPersonCollection GetInstance()
        {
            if(_instance == null)
            {
                _instance = new DataTransferPersonCollection();
            }
            return _instance;
        }

        //Send request to database and renew collection
        public void Refresh()
        {
            _pCollection = new List<DataTransferPerson>();
            RestClient client = new RestClient(ViewModels.MainWindowViewModel.BaseUrl);
            RestRequest request = new RestRequest("/api/person", Method.GET);
            IRestResponse<List<DataTransferPerson>> response = client.Execute<List<DataTransferPerson>>(request);
            foreach (var item in response.Data)
            {
                _pCollection.Add(item);
            }
        }

        public void Add(DataTransferPerson person)
        {
            _pCollection.Add(person);
        }

        public List<DataTransferPerson> GetCollection()
        {
            return _pCollection;
        }

        //Find selected item in collection
        public DataTransferPerson GetSelected()
        {
            foreach(var item in _pCollection)
            {
                if(item.IsSelected == true)
                {
                    return item;
                }
            }
            return null;
        }

        public void Remove(DataTransferPerson person)
        {
            _pCollection.Remove(person);
        }

        //Find person by id and edit it
        public void Edit(DataTransferPerson person)
        {
            foreach(var item in _pCollection)
            {
                if(item.id == person.id)
                {
                    item.lName = person.lName;
                    item.fName = person.fName;
                    item.city = person.city;
                    if(item.city == null)
                    {
                        item.city = "-";
                    }
                    item.street = person.street;
                    item.zip = person.zip;
                    item.cpny = person.cpny;
                    item.title = person.title;
                }
            }
        }

        //Find person by id and add contact
        public void AddContact(int id, int type, string txt)
        {
            foreach(var item in _pCollection)
            {
                if(item.id == id)
                {
                    item.contacts.Add(new Contact() { personContactId = type, personContactTxt = txt });
                }
            }
        }

        //Replace contact that already exists
        public void ReplaceContact(int id, int type, string txt, Contact cContact)
        {
            foreach(var item in _pCollection)
            {
                if(item.id == id)
                {
                    foreach(var contact in item.contacts)
                    {
                        if(contact.Equals(cContact))
                        {
                            contact.personContactId = type;
                            contact.personContactTxt = txt;
                        }
                    }
                }
            }
        }

        //Forced selection of items in list
        public void SetSelected(List<DataTransferPerson> persons)
        {
            foreach(var item in persons)
            {
                foreach(var nItem in _pCollection)
                {
                    if(nItem.id == item.id)
                    {
                        nItem.IsSelected = true;
                    }
                }
            }
        }

        //Add new person to list
        public int AddNew(DataTransferPerson person)
        {
            person.id = _pCollection[_pCollection.Count - 1].id + 1;
            if (person.city == null)
            {
                person.city = "-";
            }
            _pCollection.Add(person);
            return person.id;
        }
    }
}
