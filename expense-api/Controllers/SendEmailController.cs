using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using expense_api.Models;
using expense_api.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace expense_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendEmailController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SendEmailController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // POST: api/SendEmail
        [HttpPost]
        // public async void Post([FromBody] RequestSendEmail requestSendEmail)
        public async Task<ActionResult<string>> Post([FromBody] RequestSendEmail requestSendEmail)
        {
            // Fire and forget
            try
            {
                string url = _configuration.GetSection("Api").GetSection("SendGridEmail").Value;
                // _config.GetSection("api").GetSection("postSendEmail").Value;
                // string url = $"{ _config.GetSection("api").GetSection("baseUrl").Value}{resourceUri}";
                //RequestSendEmail reqSendEmail = new RequestSendEmail(); // { UserId = userId, ExpenseId = expenseId };

                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                using (var httpContent = new UtilHttpContent().CreateHttpContent(requestSendEmail))
                {
                    //client.DefaultRequestHeaders
                    //  .Accept
                    //.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    // request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    request.Content = httpContent;

                    using (var response = await client
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            // var content = await response.Content.ReadAsStringAsync();
                            // return StatusCode((int)response.StatusCode, content);
                            // LOG Success
                            
                        }
                        return response.Content.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                // LOG ERROR
                string error = ex.Message.ToString();
                return null;
            }
        }

    }
}
