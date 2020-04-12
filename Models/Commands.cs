using System;
using System.Collections.Generic;

namespace MyStore.Models
{
    public partial class Commands
    {
        public int IdCom { get; set; }
        public int IdAcheteur { get; set; }
        public int IdProduit { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public string Livraison { get; set; }

        public virtual Users IdAcheteurNavigation { get; set; }
        public virtual Produits IdProduitNavigation { get; set; }
    }
}
