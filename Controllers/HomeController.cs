using System.Diagnostics;
using COMP2139_ICE.Areas.ProjectManagement.Controllers;
using Microsoft.AspNetCore.Mvc;
using COMP2139_ICE.Models;

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
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult GeneralSearch(string searchType, string searchString)
    {
        searchType = searchType.ToLower();
        if (string.IsNullOrEmpty(searchString) || string.IsNullOrWhiteSpace(searchType))
        {
            return RedirectToAction("Index", "Home");
        }

        if (searchType == "projects")
        {
            return RedirectToAction(nameof(ProjectController.Search), "Project", 
                new { area = "ProjectManagement", searchString = searchString });
        }

        if (searchType == "tasks")
        {
            return RedirectToAction(nameof(ProjectTaskController.Search), "ProjectTask",
                new { area = "ProjectManagement", searchString = searchString });
        }

        
        return RedirectToAction("Index", "Home");
    }
}