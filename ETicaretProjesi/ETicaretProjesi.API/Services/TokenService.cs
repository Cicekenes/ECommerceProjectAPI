using ETicaretProjesi.API.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretProjesi.API.Services
{
    //public class TokenService
    //{
    //    private IConfiguration _configuration;
    //    public TokenService(IConfiguration configuration)
    //    {
    //        _configuration = configuration;
    //    }
    //    public static string GenerateToken(Account account)
    //    {
    //        byte[] jwtKey = Encoding.UTF8.GetBytes(_configuration["JwtOptions:Key"]);
    //        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(jwtKey);
    //        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    //        string issure = "site.com";
    //        string audience = "site.com";

    //        List<Claim> claims = new List<Claim>
    //                {
    //                    new Claim("id",account.Id.ToString()),
    //                    new Claim(ClaimTypes.Name,account.Username),
    //                    new Claim("type",((int)account.Type).ToString()),
    //                    new Claim(ClaimTypes.Role,account.Type.ToString())
    //                };

    //        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(issure, audience, claims, expires: DateTime.Now.AddDays(30), signingCredentials: credentials);
    //        string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    //        return token;
    //    }

    //}
}
