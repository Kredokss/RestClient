using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using RestSharp;

namespace TestRestClient.Entities
{
    //Class that storer collection of countries obtained by request
    class GreetingsCollection
    {
        private static GreetingsCollection _instance;
        private List<Greeting> _gCollection;

        private GreetingsCollection()
        {
            _gCollection = new List<Greeting>();
            RestClient client = new RestClient(ViewModels.MainWindowViewModel.BaseUrl);
            RestRequest request = new RestRequest("/api/greeting", Method.GET);
            IRestResponse<List<Greeting>> response = client.Execute<List<Greeting>>(request);
            if (response.ErrorException == null)
            {
                foreach (var item in response.Data)
                {
                    _gCollection.Add(item);
                }
            }
        }

        public static GreetingsCollection GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GreetingsCollection();
            }
            return _instance;
        }

        public List<Greeting> GetCollection()
        {
            return _gCollection;
        }

        public Greeting GetGreetingByTextAndLanguage(string text, string lang)
        {
            Greeting greeting = new Greeting();
            switch (lang)
            {
                case "English":
                    foreach (var item in _gCollection)
                    {
                        if (item.txt4 == text)
                        {
                            greeting = item;
                        }
                    }
                    break;
                case "French":
                    foreach (var item in _gCollection)
                    {
                        if (item.txt2 == text)
                        {
                            greeting = item;
                        }
                    }
                    break;
                case "Italian":
                    foreach (var item in _gCollection)
                    {
                        if (item.txt3 == text)
                        {
                            greeting = item;
                        }
                    }
                    break;
                case "German":
                    foreach (var item in _gCollection)
                    {
                        if (item.txt1 == text)
                        {
                            greeting = item;
                        }
                    }
                    break;
            }
            return greeting;
        }
    }
}
