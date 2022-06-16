using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretProjesi.Core.Models.CartModels
{
    public class CartModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsClosed { get; set; }
        public int AccountId { get; set; }
        public List<CartProductModel> CartProducts { get; set; }

    }
}
