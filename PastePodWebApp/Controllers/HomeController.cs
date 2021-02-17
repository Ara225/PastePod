using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PastePodWebApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PastePodWebApp.Data;
using Microsoft.AspNetCore.Identity;

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
                    string fileContent = await DataAccess.GetDocumentContents(fileName);
                    ViewBag.DocumentContent = fileContent;
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(TextDocumentViewModel model)
        {
            UserManager<IdentityUser> _UserManager = (UserManager<IdentityUser>)HttpContext.RequestServices.GetService(typeof(UserManager<IdentityUser>));
            IdentityUser User = await _UserManager.GetUserAsync(HttpContext.User);
            string fileName = await DataAccess.SaveDocument(model, User, _context);
            return Redirect("/Home/Index/" + fileName);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Delete()
        {
            TextDocumentViewModel model = new Models.TextDocumentViewModel();
            if (Request.Path.HasValue)
            {
                string[] path = Request.Path.Value.Split("/");
                string fileName = path[path.Count() - 1];
                Guid test;
                if (fileName != null && Guid.TryParse(fileName, out test))
                {
                    string fileContent = await DataAccess.GetDocumentContents(fileName);
                    model.TextContent = fileContent;
                    model.FileName = fileName;
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(TextDocumentViewModel model)
        {
            UserManager<IdentityUser> _UserManager = (UserManager<IdentityUser>)HttpContext.RequestServices.GetService(typeof(UserManager<IdentityUser>));
            IdentityUser User = await _UserManager.GetUserAsync(HttpContext.User);
            TextDocumentModel document = await DataAccess.GetDocumentDbRecord(_context, model.FileName);
            if (document.OwnerId == User.Id)
            {
                await DataAccess.DeleteDocument(_context, document.FileName);
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
