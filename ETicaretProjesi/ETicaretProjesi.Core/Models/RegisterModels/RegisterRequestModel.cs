using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretProjesi.Core.Models.RegisterModels
{
    public class RegisterRequestModel
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
    }
}
