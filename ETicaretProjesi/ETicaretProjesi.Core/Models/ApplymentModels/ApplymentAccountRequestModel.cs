using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaretProjesi.Core.Models.ApplymentModels
{
    public class ApplymentAccountRequestModel
    {
        [Required]
        [StringLength(25)]
        public string Username { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 6)]
        public string Password { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 6)]
        [Compare(nameof(Password))]
        public string RePassword { get; set; }
        [StringLength(50)]
        public string CompanyName { get; set; }
        [StringLength(50)]
        public string ContactName { get; set; }
        [StringLength(50)]
        [EmailAddress]
        public string ContactEmail { get; set; }
    }
}
