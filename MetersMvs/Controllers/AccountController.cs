using MetersMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Core.Logging;
using NuGet.Protocol.Plugins;

using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Core.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.AspNetCore.Authorization;

namespace MetersMVC.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7202/api/User/");
        }

        /// <summary>
        /// Displays the login form.
        /// </summary>
     
        
        
        [HttpGet]
       
        public IActionResult Login()
        {
            LoggerHelper.LogInfo("AccountController Info: Login GET action started.");
            return View();
        }



        [HttpPost]
       
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            LoggerHelper.LogInfo("Login POST action started.");

            // Validate the model
            if (!ModelState.IsValid)
            {
                LoggerHelper.LogError(new Exception("Invalid model state in Login POST action."));
                return View(model);
            }

            try
            {
               
                var jsonData = JsonSerializer.Serialize(model);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

               
                var response = await _httpClient.PostAsync("Login", content);

               
                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();
                    LoggerHelper.LogInfo("Token received successfully.");
                    //     var oo = ((dynamic)JsonConvert.DeserializeObject(token));
                    TokenResponse tokenResponse = JsonSerializer.Deserialize<TokenResponse>(token);

                    HttpContext.Session.SetString("JwtToken", tokenResponse.Token);

                    LoggerHelper.LogInfo("Token saved to session. Redirecting to Index.");
                    return RedirectToAction("Index", "Meters");
                }
                else
                { 
                    
                    LoggerHelper.LogError(new Exception($"Login failed with status code: {response.StatusCode}"));
                    ModelState.AddModelError(string.Empty, $"Login failed. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(new Exception($"Error during Login POST action: {ex.Message}", ex));
                ModelState.AddModelError(string.Empty, "An error occurred while processing your login.");
            }

            return View(model);
        }

        //private List<System.Security.Claims.Claim> ParseClaimsFromJwt(string jwt)
        //{
        //    var claims = new List<System.Security.Claims.Claim>();
        //    var jwtHandler = new JwtSecurityTokenHandler();

        //    try
        //    {
        //        if (!jwtHandler.CanReadToken(jwt))
        //        {
        //            var token = jwtHandler.ReadToken(jwt) as JwtSecurityToken;

        //            if (token != null)
        //            {
        //                claims = token.Claims.ToList();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerHelper.LogError(new Exception($"Error parsing JWT token: {ex.Message}", ex));
        //    }

        //    return claims;
        //}

        private List<System.Security.Claims.Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<System.Security.Claims.Claim>();

            try
            {
                
                if (string.IsNullOrWhiteSpace(jwt) || jwt.Split('.').Length != 3)
                {
                    throw new SecurityTokenMalformedException("The JWT is not well-formed. It must have exactly three parts.");
                }

               
                var parts = jwt.Split('.');

                
                var header = DecodeBase64Url(parts[0]);
                var payload = DecodeBase64Url(parts[1]);

               
                var payloadData = JsonSerializer.Deserialize<Dictionary<string, object>>(payload);
                if (payloadData != null)
                {
                    foreach (var kvp in payloadData)
                    {
                        claims.Add(new System.Security.Claims.Claim(kvp.Key, kvp.Value.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(new Exception($"Error parsing JWT token: {ex.Message}", ex));
            }

            return claims;
        }


        private string DecodeBase64Url(string base64Url)
        {
           
            var base64 = base64Url.Replace('-', '+').Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            
            var bytes = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(bytes);
        }



        /// <summary>
        /// Handles the POST request for login.
        /// </summary>
        //[httppost]
        //public async task<iactionresult> login(loginviewmodel model)
        //{
        //    loggerhelper.loginfo("accountcontroller info: login post action started.");
        //    if (!modelstate.isvalid)
        //    {
        //        loggerhelper.logerror(new exception("accountcontroller error: invalid model state in login post action."));
        //        return view(model);
        //    }

        //    try
        //    {
        //        var jsondata = jsonserializer.serialize(model);
        //        var content = new stringcontent(jsondata, encoding.utf8, "application/json");
        //        var response = await _httpclient.postasync("login", content);

        //        if (response.issuccessstatuscode)
        //        {
        //           var token = await response.content.readasstringasync();
        //            tempdata["token"] = token;
        //            loggerhelper.loginfo("accountcontroller info: login successful.");
        //            return redirecttoaction("index", "meters");
        //        }

        //        loggerhelper.logerror(new exception("accountcontroller error: login failed."));
        //        modelstate.addmodelerror(string.empty, "login failed. please check your credentials.");
        //    }
        //    catch (exception ex)
        //    {
        //        loggerhelper.logerror(new exception($"accountcontroller error: an exception occurred during login post action. details: {ex.message}"));
        //    }

        //    return view(model);
        //}





        /// <summary>
        /// Displays the registration form.
        /// </summary>
        [HttpGet]
        public IActionResult Register()
        {
            LoggerHelper.LogInfo("AccountController Info: Register GET action started.");
            return View();
        }

        /// <summary>
        /// Handles the POST request for user registration.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            LoggerHelper.LogInfo("AccountController Info: Register POST action started.");
            if (!ModelState.IsValid)
            {
                LoggerHelper.LogError(new Exception("AccountController Error: Invalid model state in Register POST action."));
                return View(model);
            }

            try
            {
                var jsonData = JsonSerializer.Serialize(model);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Register", content);

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();
                    TempData["Token"] = token;
                    LoggerHelper.LogInfo("AccountController Info: Registration successful.");
                    return RedirectToAction("Login");
                }

                LoggerHelper.LogError(new Exception("AccountController Error: Registration failed."));
                ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(new Exception($"AccountController Error: An exception occurred during Register POST action. Details: {ex.Message}"));
            }

            return View(model);
        }

        /// <summary>
        /// Displays an error page with a custom error message.
        /// </summary>
        /// <param name="message">The error message to display.</param>
        [HttpGet]
        public IActionResult Error(string message)
        {
            LoggerHelper.LogError(new Exception($"AccountController Error: {message}"));
            ViewBag.ErrorMessage = message;
            return View();
        }
    }
}
