using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRestClient.Entities
{
    //Class that stores selected contact 
    class CurrentContact
    {
        private static CurrentContact _instance;
        private Contact _contact;
        private CurrentContact()
        {
            _contact = null;
        }

        public static CurrentContact GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CurrentContact();
            }
            return _instance;
        }

        public void SetContact(List<Contact> contacts)
        {
            if (contacts != null)
            {
                _contact = contacts[0];
            }
        }
        public Contact GetContact()
        {
            return _contact;
        }

        public void RemoveContact()
        {
            _contact = null;
        }
    }
}
