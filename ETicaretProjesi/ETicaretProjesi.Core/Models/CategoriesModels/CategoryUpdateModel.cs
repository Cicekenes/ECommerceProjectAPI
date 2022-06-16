using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretProjesi.Core.Models.CategoriesModels
{
    public class CategoryUpdateModel
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
    }
}
