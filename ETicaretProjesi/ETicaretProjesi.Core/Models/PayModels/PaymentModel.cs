using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretProjesi.Core.Models.PayModels
{
    public class PaymentModel
    {
       
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }
      
        public string Type { get; set; }
     
        public string InvoiceAddress { get; set; }
     
        public string ShippedAddress { get; set; }
        public bool IsCompleted { get; set; }
        public int? CartId { get; set; }
        public int? AccountId { get; set; }
    }
}
