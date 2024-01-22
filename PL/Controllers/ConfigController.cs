using System.Diagnostics;
using BLL;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PL.Models;

namespace PL.Controllers;

public class ConfigController : Controller
{
    private readonly IConfigService _service;

    public ConfigController(IConfigService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult GetConfigName()
    {
        return View();
    }
    public IActionResult ImportJsonConfig()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> ImportJsonConfig(IFormFile? file)
    {
        if (file is not null && file.Length > 0)
        {
            using var reader = new StreamReader(file.OpenReadStream());
            string configFile = reader.ReadToEnd();
            string fileName = file.FileName;

            await _service.WriteConfigToDb(configFile, fileName);
            
            return RedirectToAction("GetConfigName");
        }
        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> GetHierarchy(string fileName)
    {
        try
        {
            Tree result = await _service.RetrieveConfigFromDb(fileName);
            return View(result);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}