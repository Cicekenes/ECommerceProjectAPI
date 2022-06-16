using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaretProjesi.Core.Models.ApplymentModels
{
    public class ApplymentAccountResponseModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
    }
}
