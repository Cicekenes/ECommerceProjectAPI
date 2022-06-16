using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Models.PaymentModels
{
    public class PaymentRequestModel
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
        public decimal TotalPrice { get; set; }
    }
}
