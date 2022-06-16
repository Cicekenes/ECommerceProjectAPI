using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretProjesi.Core.Models.PaymentModels
{
    public class PayModel
    {
        [Required]
        [CreditCard]
        public string CardNumber { get; set; }
        [StringLength(40)]
        [Required]
        public string CardName { get; set; }
        [Required]
        [StringLength(5)]
        [RegularExpression(@"^\d{2}\/\d{2}$")]
        public string ExpireDate { get; set; }
        [Required]
        [StringLength(3)]
        [RegularExpression(@"^\d{3}$")]
        public string CVV { get; set; }
        public decimal? TotalPriceOverride { get; set; } 
        [StringLength(25)]
        public string Type { get; set; }
        [Required]
        [StringLength(160)]
        public string InvoiceAddress { get; set; }
        [Required]
        [StringLength(160)]
        public string ShippedAddress { get; set; }
    }
}
