using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuizApp.Models;
using QuizApp.Data;
using Microsoft.EntityFrameworkCore;


namespace QuizApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly QuizAppContext _context;

        public HomeController(ILogger<HomeController> logger, QuizAppContext context)
        {
            _context = context;
            _logger = logger;
        }

        // GET quizzes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Quizzes.ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
