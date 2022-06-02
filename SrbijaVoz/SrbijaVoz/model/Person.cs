using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrbijaVoz.model
{
    public class Person
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public Person(string Username, string Password, string Name, string Surname)
        {
            this.Username = Username;
            this.Password = Password;
            this.Name = Name;
            this.Surname = Surname;
        }

    }
}
