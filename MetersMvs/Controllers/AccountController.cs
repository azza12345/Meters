using MetersMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Core.Logging;

namespace MetersMVC.Controllers
{
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

        /// <summary>
        /// Handles the POST request for login.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            LoggerHelper.LogInfo("AccountController Info: Login POST action started.");
            if (!ModelState.IsValid)
            {
                LoggerHelper.LogError(new Exception("AccountController Error: Invalid model state in Login POST action."));
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
                    TempData["Token"] = token;
                    LoggerHelper.LogInfo("AccountController Info: Login successful.");
                    return RedirectToAction("Index", "Meters");
                }

                LoggerHelper.LogError(new Exception("AccountController Error: Login failed."));
                ModelState.AddModelError(string.Empty, "Login failed. Please check your credentials.");
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(new Exception($"AccountController Error: An exception occurred during Login POST action. Details: {ex.Message}"));
            }

            return View(model);
        }

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
