using AutoMapper;
using Core.Interfaces;
using Core.Logging;
using Core.Models;
using MetersMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

using MetersMVC.ViewModels;
using Infrastructure.Data;

namespace MetersMVC.Controllers
{
public class MetersController : Controller
{
        private readonly IMeterService _meterService;
    private readonly IMapper _mapper;
        // private readonly ILogger<MetersController> _logger;
        private readonly HttpClient _httpClient;

        public MetersController(IMeterService meterService, IMapper mapper, HttpClient httpClient)
          //  ILogger<MetersController> logger)
    {
            _meterService = meterService;
        _mapper = mapper;
            _httpClient = httpClient;
            // _logger = logger;
    }

    // GET: Meters
    public async Task<IActionResult> Index()
    {
            try
            {
                _httpClient.BaseAddress = new Uri("https://localhost:7182/api/");
                _httpClient.DefaultRequestHeaders.Accept.Add(
              new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.GetAsync("Meter");
                                
                if (response.IsSuccessStatusCode)
                {
                   
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    //var meters = JsonSerializer.Deserialize<List<Meter>>(jsonResponse, new JsonSerializerOptions());
                    var meters = JsonSerializer.Deserialize<List<Meter>>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

        var meterViewModels = _mapper.Map<List<MeterViewModel>>(meters);
                    LoggerHelper.LogInfo("MetersControllersMVC Info: Retrieved meters successfully.");

        return View(meterViewModels);
    }
                else
                {
                    LoggerHelper.LogError(new Exception("MetersControllersMVC Error: API request failed."));
                    return RedirectToAction("Error", new { message = "Failed to retrieve meters from the API." });
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(new Exception($"MetersControllersMVC Error in Index action: {ex.Message}"));
                return RedirectToAction("Error", new { message = "An error occurred while retrieving the list of meters." });
            }
        }


        /// <summary>
        /// Retrieves and displays the details of a specific meter.
        /// </summary>
        /// <param name="id">The ID of the meter.</param>
        /// <returns>A view displaying the details of the meter.</returns>
    public async Task<IActionResult> Details(int id)
    {
            LoggerHelper.LogInfo("MetersControllersMVC Info : Details action   {MeterId} :" + id);

          
            try
            {
                var meter = await _meterService.GetMeterById(id);
        if (meter == null)
        {
                  
            return NotFound();
        }

        var meterViewModel = _mapper.Map<MeterViewModel>(meter);
        return View(meterViewModel);
    }
            catch (Exception ex)
            {
                LoggerHelper.LogError(new Exception("MetersControllersMVC Error in details action {ex}  :" + ex+ "for {MeterId} " +id));

                return RedirectToAction("Error", new { message = "An error occurred while retrieving meter details." });
            }
        }

        /// <summary>
        /// Displays the meter creation form.
        /// </summary>
        /// <returns>A view displaying the form to create a new meter.</returns>
    public IActionResult Create()
    {
            LoggerHelper.LogInfo("MetersControllersMVC Info : Create Get action started");
           
        return View();
    }

        /// <summary>
        /// Handles the POST request to create a new meter.
        /// </summary>
        /// <param name="meterViewModel">The meter view model containing the meter details.</param>
        /// <returns>Redirects to the Index view if successful; otherwise, returns to the Create view with validation errors.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MeterViewModel meterViewModel)
    {
            LoggerHelper.LogInfo("MetersControllersMVC Info : Create Post action started");
            try
            {
        if (ModelState.IsValid)
        {
                    var meter = _mapper.Map<Meter>(meterViewModel);
                    await _meterService.AddMeter(meter);
                    LoggerHelper.LogInfo("MetersControllersMVC Info : Successfully created meter with Id {MeterId} " +meter.Id);
                   
            return RedirectToAction(nameof(Index));
        }

          
        return View(meterViewModel);
    }
            catch (Exception ex)
            {
                LoggerHelper.LogError(new Exception("MetersControllersMVC Error : An error occurred while creating a meter "+ex));

                return RedirectToAction("Error", new { message = "An error occurred while creating a meter." });
            }
        }

        /// <summary>
        /// Displays the meter editing form.
        /// </summary>
        /// <param name="id">The ID of the meter to edit.</param>
        /// <returns>A view displaying the form to edit a meter.</returns>
    public async Task<IActionResult> Edit(int id)
    {
            LoggerHelper.LogInfo("MetersControllersMVC Info : Edit Get action for Meter Id {MeterId}" + id);
          
            try
            {
                var meter = await _meterService.GetMeterById(id);
        if (meter == null)
        {
                  
            return NotFound();
        }

        var meterViewModel = _mapper.Map<MeterViewModel>(meter);
        return View(meterViewModel);
    }
            catch (Exception ex)
            {
                LoggerHelper.LogError(new Exception("MetersControllersMVC Error :An error occurred while retrieving meter for editing {MeterId}"+id));
             
                return RedirectToAction("Error", new { message = "An error occurred while retrieving meter for editing." });
            }
        }

        /// <summary>
        /// Handles the POST request to update an existing meter.
        /// </summary>
        /// <param name="id">The ID of the meter to edit.</param>
        /// <param name="meterViewModel">The meter view model containing the updated meter details.</param>
        /// <returns>Redirects to the Index view if successful; otherwise, returns to the Edit view with validation errors.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MeterViewModel meterViewModel)
    {
            LoggerHelper.LogInfo("MetersControllersMVC Info Edit POST action started for {MeterId}:"+id);
            
            try
            {
        if (id != meterViewModel.Id)
        {
             
            return NotFound();
        }

        if (ModelState.IsValid)
        {
                    var meter = _mapper.Map<Meter>(meterViewModel);
                    await _meterService.UpdateMeter(meter);
                    LoggerHelper.LogInfo("MetersControllersMVC Info : Successfully updated meter with  {MeterId} "+meter.Id);
                 
                    return RedirectToAction(nameof(Index));
                }

             
                return View(meterViewModel);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(new Exception("MetersControllersMVC Error :An error occurred while Updating meter for editing {MeterId}" + id));

                
                return RedirectToAction("Error", new { message = "An error occurred while updating the meter." });
        }
        return View(meterViewModel);
    }

        /// <summary>
        /// Displays the meter deletion confirmation form.
        /// </summary>
        /// <param name="id">The ID of the meter to delete.</param>
        /// <returns>A view displaying the form to delete a meter.</returns>
    public async Task<IActionResult> Delete(int id)
    {
            LoggerHelper.LogInfo("MetersControllersMVC Info Delet GET action started for {MeterId}:" + id);
            
            try
            {
                var meter = await _meterService.GetMeterById(id);
        if (meter == null)
        {
                
            return NotFound();
        }

        var meterViewModel = _mapper.Map<MeterViewModel>(meter);
        return View(meterViewModel);
    }
            catch (Exception ex)
            {
                LoggerHelper.LogError(new Exception("MetersControllersMVC Error :An error occurred in Delete GET meter  {MeterId}" + id));

                
                return RedirectToAction("Error", new { message = "An error occurred while retrieving meter for deletion." });
            }
        }

        /// <summary>
        /// Handles the POST request to confirm meter deletion.
        /// </summary>
        /// <param name="id">The ID of the meter to delete.</param>
        /// <returns>Redirects to the Index view if successful.</returns>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
            LoggerHelper.LogInfo("MetersControllersMVC Info DElete POST action started for {MeterId}:" + id);
           
            try
            {
                await _meterService.DeleteMeter(id);
                LoggerHelper.LogInfo("MetersControllersMVC Info : Deleted Successfully {MeterId}:" + id);
               
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
        {
                LoggerHelper.LogError(new Exception("MetersControllersMVC Error :An error occurr in Delete POST{MeterId}" + id));

                
                return RedirectToAction("Error", new { message = "An error occurred while deleting the meter." });
        }
        return RedirectToAction(nameof(Index));
    }

        /// <summary>
        /// Displays an error page with a custom error message.
        /// </summary>
        /// <param name="message">The error message to display.</param>
        /// <returns>A view displaying the error message.</returns>
        public IActionResult Error(string message)
    {
            LoggerHelper.LogError(new Exception("MetersControllersMVC Error :  Error action triggered with message: {ErrorMessage}"+message));

            
            ViewBag.ErrorMessage = message;
            return View();
        }
    }
}
