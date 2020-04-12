using System;
using System.Collections.Generic;

namespace MyStore.Models
{
    public partial class Produits
    {
        public Produits()
        {
            Commands = new HashSet<Commands>();
        }

        public int Id { get; set; }
        public byte[] Improd { get; set; }
        public string Namprod { get; set; }
        public string Descprod { get; set; }
        public decimal Priprod { get; set; }
        public decimal? Discprod { get; set; }
        public decimal? Stock { get; set; }

        public virtual ICollection<Commands> Commands { get; set; }
    }
}
