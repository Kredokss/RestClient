using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRestClient.Entities
{
    class CurrentLanguage
    {
        //Class that stores selected language for further usage
        private static CurrentLanguage _instance;
        private string _language;
        private CurrentLanguage()
        {
        }

        public static CurrentLanguage GetInstance()
        {
            if(_instance == null)
            {
                _instance = new CurrentLanguage();
            }
            return _instance;
        }

        public void SetLanguage(string lang)
        {
            _language = lang;
        }
        public string GetLanguage()
        {
            return _language;
        }
    }
}
