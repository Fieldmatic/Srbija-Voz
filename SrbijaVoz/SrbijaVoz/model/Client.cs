using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrbijaVoz.model
{
    public class Client : Person
    {
        public Client(string Username, string Password, string Name, string Surname) : base(Username, Password, Name, Surname)
        {
        }
    }
}
