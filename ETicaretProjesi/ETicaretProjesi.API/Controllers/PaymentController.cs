using ETicaretProjesi.API.DataAccess;
using ETicaretProjesi.API.Entities;
using ETicaretProjesi.Core;
using ETicaretProjesi.Core.Models.PaymentModels;
using ETicaretProjesi.Core.Models.PayModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PaymentAPI.Models.PaymentModels;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ETicaretProjesi.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class PaymentController : ControllerBase
    {
        private DatabaseContext _db;
        private IConfiguration _configuration;
        public PaymentController(DatabaseContext context, IConfiguration configuration)
        {
            _db = context;
            _configuration = configuration;
        }

        //[ProducesResponseType(200, Type = typeof(Resp<CartModel>))]

        [HttpPost("Pay/{cartid}")]
        [ProducesResponseType(200,Type =typeof(Resp<PaymentModel>))]
        [ProducesResponseType(400, Type = typeof(Resp<string>))]
        public IActionResult Pay([FromRoute] int cartid, [FromBody] PayModel model)
        {
            Resp<PaymentModel> result = new Resp<PaymentModel>();
            Cart cart = _db.Carts.Include(x => x.CartProducts).SingleOrDefault(x => x.Id == cartid);

            string paymentApiEndpoint = _configuration["PaymentAPI:Endpoint"];
            if (!cart.IsClosed)
            {
                decimal totalPrice = model.TotalPriceOverride ?? cart.CartProducts.Sum(x => x.Quantity * x.DiscountedPrice);
                //Api
                HttpClient client = new HttpClient();
                //Postman->Body->Raw->JSON
                AuthRequestModel authRequestModel = new AuthRequestModel { Username = _configuration["PaymentAPI:Username"], Password = _configuration["PaymentAPI:Password"] };
                StringContent content = new StringContent(JsonSerializer.Serialize(authRequestModel), Encoding.UTF8, "application/json");
                HttpResponseMessage authResponse = client.PostAsync($"{paymentApiEndpoint}/Pay/authenticate", content).Result;

                if (authResponse.StatusCode == HttpStatusCode.OK)
                {
                    string authJsonContent = authResponse.Content.ReadAsStringAsync().Result;
                    AuthResponseModel authResponseModel = JsonSerializer.Deserialize<AuthResponseModel>(authJsonContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    string token = authResponseModel.Token;

                    PaymentRequestModel paymentRequestModel = new PaymentRequestModel
                    {
                        CardNumber = model.CardNumber,
                        CardName = model.CardName,
                        ExpireDate = model.ExpireDate,
                        CVV = model.CVV,
                        TotalPrice = totalPrice
                    };

                    StringContent paymentContent = new StringContent(JsonSerializer.Serialize(paymentRequestModel), Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

                    HttpResponseMessage paymentResponse = client.PostAsync($"{paymentApiEndpoint}/Pay/Payment", paymentContent).Result;
                    if (paymentResponse.StatusCode == HttpStatusCode.OK)
                    {
                        string paymentJsonContent = paymentResponse.Content.ReadAsStringAsync().Result; PaymentResponseModel paymentResponseModel = JsonSerializer.Deserialize<PaymentResponseModel>(paymentJsonContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        if (paymentResponseModel.Result == "ok")
                        {
                            string transactionId = paymentResponseModel.TransactionId;
                            Payment payment = new Payment
                            {
                                CartId = cartid,
                                AccountId = cart.AccountId,
                                InvoiceAddress = model.InvoiceAddress,
                                ShippedAddress = model.ShippedAddress,
                                Type = model.Type,
                                TransactionId = transactionId,
                                Date = DateTime.Now,
                                TotalPrice = totalPrice
                            };

                            cart.IsClosed = true;
                            _db.Payments.Add(payment);
                            _db.SaveChanges();

                            PaymentModel data = new PaymentModel
                            {
                                Id = payment.Id,
                                AccountId=payment.AccountId,
                                CartId = payment.CartId,
                                Date=payment.Date,
                                InvoiceAddress=payment.InvoiceAddress,
                                IsCompleted=payment.IsCompleted,
                                ShippedAddress=payment.ShippedAddress,
                                TotalPrice=payment.TotalPrice,
                                Type=payment.Type
                            };

                            result.Data = data;
                            return Ok(result);

                        }
                        else
                        {
                            Resp<string> paymentOkResult = new Resp<string>();
                            paymentOkResult.AddError("payment", authResponse.Content.ReadAsStringAsync().Result);
                            return BadRequest(paymentOkResult);
                        }

                    }
                    else
                    {
                        Resp<string> paymentResult = new Resp<string>();
                        paymentResult.AddError("payment", "Ödeme alınamadı");
                        return BadRequest(paymentResult);
                    }


                }
                else
                {
                    Resp<string> authResult = new Resp<string>();
                    authResult.AddError("auth", authResponse.Content.ReadAsStringAsync().Result);
                    return BadRequest(authResult);
                }

            }
            else
            {
                Payment payment = _db.Payments.SingleOrDefault(x => x.CartId == cartid);

                if (payment==null)
                {
                    result.AddError("cart", $"The card appears to be closed but not paid. CartId : {cartid}");

                    return BadRequest(result);
                }
                PaymentModel data = new PaymentModel
                {
                    Id = payment.Id,
                    AccountId = payment.AccountId,
                    CartId = payment.CartId,
                    Date = payment.Date,
                    InvoiceAddress = payment.InvoiceAddress,
                    IsCompleted = payment.IsCompleted,
                    ShippedAddress = payment.ShippedAddress,
                    TotalPrice = payment.TotalPrice,
                    Type = payment.Type
                };

                result.Data = data;
                return Ok(result);

            }

        }

    }
}
