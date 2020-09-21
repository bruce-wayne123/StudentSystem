using SQLite;
using System;

namespace StudXam.Models
{
    public class Student : BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int RollNo { get; set; }

        private string _name;

        public string Name
        {
            get => _name; set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _gender;

        public string Gender
        {
            get => _gender; set
            {
                _gender = value;
                OnPropertyChanged();
            }
        }

        private string _class;

        public string Class
        {
            get => _class;
            set
            {
                _class = value;
                OnPropertyChanged();
            }
        }

        private DateTime _birthDate { get; set; }

        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                OnPropertyChanged();
            }
        }

        private string _address { get; set; }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        private string _emailID { get; set; }

        public string EmailID
        {
            get => _emailID;
            set
            {
                _emailID = value;
                OnPropertyChanged();
            }
        }

        private bool isAudited { get; set; }

        public bool IsAudited
        {
            get => isAudited;
            set
            {
                isAudited = value;
                OnPropertyChanged();
            }
        }
    }
}