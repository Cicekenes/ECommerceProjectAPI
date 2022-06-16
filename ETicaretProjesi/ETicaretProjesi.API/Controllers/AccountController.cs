using ETicaretProjesi.API.DataAccess;
using ETicaretProjesi.API.Entities;
using ETicaretProjesi.API.Services;
using ETicaretProjesi.Core;
using ETicaretProjesi.Core.Models.ApplymentModels;
using ETicaretProjesi.Core.Models.AuthenticateModels;
using ETicaretProjesi.Core.Models.RegisterModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ETicaretProjesi.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AccountController : ControllerBase
    {
        private DatabaseContext _db;
        private IConfiguration _configuration;
        public AccountController(DatabaseContext databaseContext, IConfiguration configuration)
        {
            _db = databaseContext;
            _configuration = configuration;
        }

        // Applyment : Satıcı Başvuru
        // Register : Üye Kaydı
        // Authentication : Kimlik doğrulaması
        //Trim():başta ve sondaki boşlukları siler
        //Any bir propun varlığını sorgular        
        [HttpPost("merchant/applyment")]
        [ProducesResponseType(200, Type = typeof(ApplymentAccountResponseModel))]
        [ProducesResponseType(400, Type = typeof(Resp<ApplymentAccountResponseModel>))]
        public IActionResult Applyment([FromBody] ApplymentAccountRequestModel model)
        {

            //if (ModelState.IsValid)
            //{
            Resp<ApplymentAccountResponseModel> response = new Resp<ApplymentAccountResponseModel>();

            model.Username = model.Username?.Trim().ToLower();
            if (_db.Accounts.Any(x => x.Username.ToLower() == model.Username))
            {
                //ModelState.AddModelError(nameof(model.Username), "Bu kullanıcı adı zaten kullanılıyor");
                response.AddError(nameof(model.Username), "Bu kullanıcı adı zaten kullanılıyor");
                return BadRequest(response);
            }
            else
            {
                Account account = new Account
                {
                    Username = model.Username,
                    Password = model.Password,
                    CompanyName = model.CompanyName,
                    ContactEmail = model.ContactEmail,
                    ContactName = model.ContactName,
                    Type = AccountType.Merchant,
                    IsApplyment = true
                };
                _db.Accounts.Add(account);
                _db.SaveChanges();

                ApplymentAccountResponseModel applymentAccountResponseModel = new ApplymentAccountResponseModel
                {
                    Id = account.Id,
                    Username = account.Username,
                    CompanyName = account.CompanyName,
                    ContactName = account.ContactName,
                    ContactEmail = account.ContactEmail
                };

                response.Data = applymentAccountResponseModel;

                return Ok(response);

                //}
                //List<string> errors = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)).ToList();


                //return BadRequest(errors);

            }
        }

        [ProducesResponseType(200, Type = typeof(RegisterResponseModel))]
        [ProducesResponseType(400, Type = typeof(Resp<RegisterResponseModel>))]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequestModel model)
        {
            Resp<RegisterResponseModel> response = new Resp<RegisterResponseModel>();
            model.Username = model.Username?.Trim().ToLower();
            if (_db.Accounts.Any(x => x.Username == model.Username))
            {
                response.AddError(nameof(model.Username), "Bu kullanıcı adı zaten kullanılıyor.");
                return BadRequest();
            }
            else
            {
                Account account = new Account
                {
                    Username = model.Username,
                    Password = model.Password,
                    Type = AccountType.Member
                };
                _db.Accounts.Add(account);
                _db.SaveChanges();

                RegisterResponseModel data = new RegisterResponseModel
                {
                    Id = account.Id,
                    Username = account.Username
                };

                response.Data = data;

                return Ok(response);
            }
        }
        [HttpPost("authenticate")]
        [ProducesResponseType(200, Type = typeof(AuthenticateResponseModel))]
        [ProducesResponseType(400, Type = typeof(Resp<AuthenticateResponseModel>))]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] AuthenticateRequestModel model)
        {
            Resp<AuthenticateResponseModel> response = new Resp<AuthenticateResponseModel>();
            model.Username = model.Username?.Trim().ToLower();
            Account account = _db.Accounts.SingleOrDefault(x => x.Username.ToLower() == model.Username && x.Password == model.Password);

            if (account != null)
            {
                if (account.IsApplyment)
                {
                    response.AddError("*", "Henüz satıcı başvurusu tamamlanmamıştır");
                    return BadRequest(response);
                }
                else
                {
                    string key = _configuration["JwtOptions:Key"];

                    List<Claim> claims = new List<Claim>
                    {
                        new Claim("id",account.Id.ToString()),
                        new Claim(ClaimTypes.Name,account.Username),
                        new Claim("type",((int)account.Type).ToString()),
                        new Claim(ClaimTypes.Role,account.Type.ToString())
                    };

                    string token = TokenService.GenerateToken(key,DateTime.Now.AddDays(30),claims);
                    //Token oluşturma
                    //Response'a token'ı yüklemeliyiz.
                    AuthenticateResponseModel data = new AuthenticateResponseModel
                    {
                        Token = token
                    };
                    response.Data = data;
                    return Ok(response);
                };
            }
     
            else
            {
                response.AddError("*", "Kullanıcı adı yada şifre eşleşmiyor");
            }
            return BadRequest(response);
        }

    }
}

