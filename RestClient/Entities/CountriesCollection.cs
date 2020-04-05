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
    class CountriesCollection
    {
        private static CountriesCollection _instance;
        private List<Country> _cCollection;

        private CountriesCollection()
        {
            _cCollection = new List<Country>();
            RestClient client = new RestClient(ViewModels.MainWindowViewModel.BaseUrl);
            RestRequest request = new RestRequest("/api/country", Method.GET);
            IRestResponse<List<Country>> response = client.Execute<List<Country>>(request);
            if (response.ErrorException == null)
            {
                foreach (var item in response.Data)
                {
                    _cCollection.Add(item);
                }
            }
        }

        public static CountriesCollection GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CountriesCollection();
            }
            else if(_instance._cCollection == null)
            {
                _instance = new CountriesCollection();
            }
            return _instance;
        }

        public List<Country> GetCollection()
        {
            return _cCollection;
        }

        public Country GetCountryByTextAndLanguage(string text, string lang)
        {
            Country country = new Country();
            switch (lang)
            {
                case "English":
                    foreach (var item in _cCollection)
                    {
                        if(item.txt4 == text)
                        {
                            country = item;
                        }
                    }
                    break;
                case "French":
                    foreach (var item in _cCollection)
                    {
                        if (item.txt2 == text)
                        {
                            country = item;
                        }
                    }
                    break;
                case "Italian":
                    foreach (var item in _cCollection)
                    {
                        if (item.txt3 == text)
                        {
                            country = item;
                        }
                    }
                    break;
                case "German":
                    foreach (var item in _cCollection)
                    {
                        if (item.txt1 == text)
                        {
                            country = item;
                        }
                    }
                    break;
            }
            return country;
        }
    }
}
