using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PastePodWebApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PastePodWebApp.Data;

namespace PastePodWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TextDocumentDbContext _context;

        public HomeController(ILogger<HomeController> logger, TextDocumentDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (Request.Path.HasValue)
            {
                string[] path = Request.Path.Value.Split("/");
                string fileName = path[path.Count() - 1];
                Guid test;
                if (fileName != null && Guid.TryParse(fileName, out test)) 
                {
                    Debug.WriteLine(fileName);
                    string fileContent = await DataAccess.GetDocument(fileName);
                    Debug.WriteLine(fileContent);
                    ViewBag.DocumentContent = fileContent;
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(TextDocumentViewModel model)
        {
            string fileName = await DataAccess.SaveDocument(model, _context);
            return Redirect("/Home/Index/" + fileName);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
