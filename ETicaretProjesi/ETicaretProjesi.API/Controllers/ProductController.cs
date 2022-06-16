using ETicaretProjesi.API.DataAccess;
using ETicaretProjesi.API.Entities;
using ETicaretProjesi.Core;
using ETicaretProjesi.Core.Models.ProductsModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ETicaretProjesi.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Merchant")]
    public class ProductController : ControllerBase
    {
        private DatabaseContext _db;
        private IConfiguration _configuration;
        public ProductController(DatabaseContext context, IConfiguration configuration)
        {
            _db = context;
            _configuration = configuration;
        }
        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(Resp<ProductModel>))]
        [ProducesResponseType(400, Type = typeof(Resp<ProductModel>))]
        public IActionResult Create([FromBody] ProductCreateModel model)
        {
            Resp<ProductModel> response = new Resp<ProductModel>();
            string productName = model.Name?.Trim().ToLower();
            if (_db.Products.Any(x => x.Name.ToLower() == productName))
            {
                response.AddError(nameof(model.Name), "Bu ürün adı zaten mevcuttur");
                return BadRequest(response);

            }
            else
            {
                int accountId = int.Parse(HttpContext.User.FindFirst("id").Value);

                Product product = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    UnitPrice = model.UnitPrice,
                    DiscountedPrice = model.DiscountedPrice,
                    Discontinued = model.Discontinued,
                    CategoryId = model.CategoryId,
                    AccountId = accountId
                };
                _db.Products.Add(product);
                _db.SaveChanges();

                product = _db.Products.Include(x => x.Category).Include(x => x.Account).SingleOrDefault(x => x.Id == product.Id);

                ProductModel data = new ProductModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    UnitPrice = product.UnitPrice,
                    DiscountedPrice = product.DiscountedPrice,
                    Discontinued = product.Discontinued,
                    CategoryId = product.CategoryId,
                    AccountId = product.AccountId,
                    CategoryName = product.Category.Name,
                    AccountCompanyName = product.Account.CompanyName
                };

                response.Data = data;

                return Ok(response);
            }
        }

        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(Resp<List<ProductModel>>))]
        [ProducesResponseType(400, Type = typeof(Resp<List<ProductModel>>))]
        public IActionResult List()
        {
            Resp<List<ProductModel>> response = new Resp<List<ProductModel>>();
            //int accountId = int.Parse(HttpContext.User.FindFirst("id").Value);
            List<ProductModel> list = _db.Products.Include(x => x.Category).Include(x => x.Account)/*.Where(x => x.AccountId == accountId)*/.Select(x => new ProductModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                UnitPrice = x.UnitPrice,
                DiscountedPrice = x.DiscountedPrice,
                Discontinued = x.Discontinued,
                CategoryId = x.CategoryId,
                AccountId = x.AccountId,
                CategoryName = x.Category.Name,
                AccountCompanyName = x.Account.CompanyName

            }).ToList();
            response.Data = list;

            return Ok(response);
        }

        [HttpGet("list/{accountId}")]
        [ProducesResponseType(200, Type = typeof(Resp<List<ProductModel>>))]
        [ProducesResponseType(400, Type = typeof(Resp<List<ProductModel>>))]
        public IActionResult ListByAccountId([FromRoute] int accountId)
        {
            Resp<List<ProductModel>> response = new Resp<List<ProductModel>>();
            //int accountId = int.Parse(HttpContext.User.FindFirst("id").Value);
            List<ProductModel> list = _db.Products.Include(x => x.Category).Include(x => x.Account).Where(x => x.AccountId == accountId).Select(x => new ProductModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                UnitPrice = x.UnitPrice,
                DiscountedPrice = x.DiscountedPrice,
                Discontinued = x.Discontinued,
                CategoryId = x.CategoryId,
                AccountId = x.AccountId,
                CategoryName = x.Category.Name,
                AccountCompanyName = x.Account.CompanyName

            }).ToList();
            response.Data = list;

            return Ok(response);
        }


        [HttpGet("get/{productId}")]
        [ProducesResponseType(200, Type = typeof(Resp<ProductModel>))]
        [ProducesResponseType(404, Type = typeof(Resp<ProductModel>))]
        public IActionResult GetById([FromRoute] int productId)
        {
            Resp<ProductModel> response = new Resp<ProductModel>();
            int accountId = int.Parse(HttpContext.User.FindFirst("id").Value);
            Product product = _db.Products.Include(x => x.Category).Include(x => x.Account).SingleOrDefault(x => x.Id == productId);

            ProductModel data = null;

            if (product != null)
                return NotFound(response);

            data = new ProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description
            };

            response.Data = data;
            return Ok(response);
        }

        [ProducesResponseType(200, Type = typeof(Resp<ProductModel>))]
        [ProducesResponseType(400, Type = typeof(Resp<ProductModel>))]
        [ProducesResponseType(404, Type = typeof(Resp<ProductModel>))]
        [HttpPut("update/{productId}")]
        public IActionResult Update([FromRoute] int id, [FromBody] ProductUpdateModel model)
        {
            Resp<ProductModel> response = new Resp<ProductModel>();
            Product product = _db.Products.Find(id);

            int accountId = int.Parse(HttpContext.User.FindFirst("id").Value);
            string role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            if (product == null)
                return NotFound();

            string productName = model.Name.Trim().ToLower();

            if (_db.Products.Any(x => x.Name.ToLower() == productName && x.Id != id && (role != "Admin" && x.AccountId == accountId)))
            {
                response.AddError(nameof(model.Name), "Bu ürün adı zaten mevcuttur");
                return BadRequest(response);
            }

            product = _db.Products.Include(x => x.Category).Include(x => x.Account).SingleOrDefault(x => x.Id == id);

            ProductModel data = new ProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                UnitPrice = product.UnitPrice,
                DiscountedPrice = product.DiscountedPrice,
                Discontinued = product.Discontinued,
                CategoryId = product.CategoryId,
                AccountId = product.AccountId,
                CategoryName = product.Category.Name,
                AccountCompanyName = product.Account.CompanyName
            };
            response.Data = data;

            return Ok(response);
        }


        [ProducesResponseType(200, Type = typeof(Resp<object>))]
        [ProducesResponseType(404, Type = typeof(Resp<object>))]
        [HttpDelete("delete/{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            Resp<object> response = new Resp<object>();
            int accountId = int.Parse(HttpContext.User.FindFirst("id").Value);
            string role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            Product product = _db.Products.SingleOrDefault(x => x.Id == id && (role == "Admin" || (role != "Admin" && x.AccountId == accountId)));
            if (product == null)
                return NotFound(response);

            _db.Products.Remove(product);
            _db.SaveChanges();


            return Ok(response);
        }
    }
}
