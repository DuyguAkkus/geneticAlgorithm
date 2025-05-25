using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using geneticAlgorithm.Models;
using geneticAlgorithm.Services;

namespace geneticAlgorithm.Controllers;

public class HomeController : Controller
{
    private readonly GeneticAlgorithmService _gaService = new();

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult RunGA(GAParameters parameters)
    {
        var result = _gaService.RunGA(parameters);
        return View("Result", result); // Artık GAResult dönüyor
    }
    
    public IActionResult Topic()
    {
        return View();
    }


}