using System;
using System.Collections.Generic;
using System.Text;

namespace MealVault.Entities.Custom
{
    public class AuthenticatedUser
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string MobileNumber { get; set; }
    }
}
