using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Models.PaymentModels
{
    public class PaymentResponseModel
    {
        public string Result { get; set; }
        public string TransactionId { get; set; }
    }
}
