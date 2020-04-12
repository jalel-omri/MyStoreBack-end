using System;
using System.Collections.Generic;

namespace MyStore.Models
{
    public partial class Users
    {
        public Users()
        {
            Commands = new HashSet<Commands>();
        }

        public int Id { get; set; }
        public string Admin { get; set; }
        public string Name { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Commands> Commands { get; set; }
    }
}
