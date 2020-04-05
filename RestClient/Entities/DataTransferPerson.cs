using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommonMVVM.Common;
using System.Web.Script.Serialization;

namespace TestRestClient.Entities
{
    public class DataTransferPerson : BasePropertyChangedNotifier
    {
        public int id { get; set; }
        public string cpny { get; set; }
        public string title { get; set; }
        public string fName { get; set; }
        public string lName { get; set; }
        public string street { get; set; }
        public string txt1 { get; set; }
        public string txt2 { get; set; }
        public string txt3 { get; set; }
        public string txt4 { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public DateTime firstContact { get; set; }
        public string greetingTxt1 { get; set; }
        public string greetingTxt2 { get; set; }
        public string greetingTxt3 { get; set; }
        public string greetingTxt4 { get; set; }
        public List<Contact> contacts { get; set; }

        [ScriptIgnore]
        private bool _isSelected { get; set; }

        [ScriptIgnore]
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
    }
}
