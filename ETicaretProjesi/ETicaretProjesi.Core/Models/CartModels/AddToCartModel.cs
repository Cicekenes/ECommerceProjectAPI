using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretProjesi.Core.Models.CartModels
{
    public class AddToCartModel
    {
        public int Quantity { get; set; }
        public int? ProductId { get; set; }
    }
}
