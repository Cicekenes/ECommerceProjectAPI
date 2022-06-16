using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaretProjesi.API.Entities
{
    [Table("Payments")]
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }
        [StringLength(25)]
        public string Type { get; set; }
        [Required]
        [StringLength(160)]
        public string InvoiceAddress { get; set; }
        [Required]
        [StringLength(160)]
        public string ShippedAddress { get; set; }
        public bool IsCompleted { get; set; }
        [StringLength(50)]
        public string TransactionId { get; set; }
        public int? CartId { get; set; }
        public int? AccountId { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual Account Account { get; set; }
    }
}
