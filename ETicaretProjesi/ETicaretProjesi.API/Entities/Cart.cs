using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaretProjesi.API.Entities
{
    public class Cart
    {
        public Cart()
        {
            CartProducts = new List<CartProduct>();
        }
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsClosed { get; set; }
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }
        public List<CartProduct> CartProducts { get; set; }
    }
}
