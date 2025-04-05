using System.Diagnostics;
using COMP2139_ICE.Areas.ProjectManagement.Controllers;
using COMP2139_ICE.Models;
using Microsoft.AspNetCore.Mvc;

namespace COMP2139_ICE.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        _logger.LogInformation("Accessed HomeController Index at {Time}", DateTime.Now);
        return View();
    }

    public IActionResult About()
    {
        _logger.LogInformation("Accessed HomeController About at {Time}", DateTime.Now);
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        _logger.LogInformation("Accessed HomeController Error at {Time}", DateTime.Now);
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    // Lab 6 - General Search for Projects or ProjectTasks
    // Redirects users to the appropriate search function

    [HttpGet]
    public IActionResult GeneralSearch(string searchType, string searchString)
    {
        _logger.LogInformation("Accessed HomeController GeneralSearch at {Time}", DateTime.Now);
        // Ensure searchType is not null and handle case-insensitivity
        searchType = searchType?.Trim().ToLower();
        
        // Ensure the search string is not empty
        if (string.IsNullOrWhiteSpace(searchType) || string.IsNullOrWhiteSpace(searchString))
        {
            // Redirect back to home if the search is empty
            return RedirectToAction(nameof(Index), "Home");
        }
        
        // Determine where to redirect based on search type
        if (searchType == "projects")
        {
            // Redirects to Project search
            return RedirectToAction(nameof(ProjectController.Search), 
                "Project",
                new {area = "ProjectManagement", searchString = searchString});
        }
        else if (searchType == "tasks")
        {
            // Redirects to ProjectTask search
            return RedirectToAction(nameof(ProjectTaskController.Search),
                "ProjectTask",
                new { area = "ProjectManagement", searchString = searchString });
        }
        
        // If searchType is invalid, redirect to Home page
        return RedirectToAction(nameof(Index), "Home");
    }

    [HttpGet]
    public IActionResult NotFound(int statusCode)
    {
        _logger.LogWarning("Not invoked HomeController Index at {Time}", DateTime.Now);
        if (statusCode == 404)
        {
            return NotFound();
        }
        
        return View("Error");
    }
}