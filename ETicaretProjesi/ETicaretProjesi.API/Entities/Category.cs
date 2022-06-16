using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaretProjesi.API.Entities
{
    [Table("Categories")]
    public class Category
    {
        public Category()
        {
            Products = new List<Product>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
