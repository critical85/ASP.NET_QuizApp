using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Models;
using Microsoft.AspNetCore.Http;

namespace QuizApp.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly QuizAppContext _context;

        public QuestionsController(QuizAppContext context)
        {
            _context = context;
        }

        //*************************************************************************************
        // GET: Questions
        public async Task<IActionResult> Index()
        {
            var quizAppContext = _context.Questions.Include(q => q.Quiz);
            return View(await quizAppContext.ToListAsync());
        }

        //*************************************************************************************
        // GET: Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Quiz)
                .FirstOrDefaultAsync(m => m.QuestionID == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        //*************************************************************************************
        // GET: Questions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Questions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestionID,QusetionRead,ChoiceA,ChoiceB,ChoiceC,ChoiceD,CorrectAnswer,Answer,QuizID")] Question question)
        {
            if(HttpContext.Session.GetInt32("QuizID") == null)
            {
                return RedirectToAction("Timeout", "Quizzes");
            }

            if (ModelState.IsValid)
            {
                question.QuizID = (int)HttpContext.Session.GetInt32("QuizID");

                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
            }
            return View(question);
        }

        //*************************************************************************************
        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        // POST: Questions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestionID,QusetionRead,ChoiceA,ChoiceB,ChoiceC,ChoiceD,CorrectAnswer,Answer,QuizID")] Question question)
        {
            if (HttpContext.Session.GetInt32("QuizID") == null)
            {
                return RedirectToAction("Index", "Quizzes");
            }

            if (id != question.QuestionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.QuestionID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Quizzes");
            }
            return View(question);
        }

        //*************************************************************************************
        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Quiz)
                .FirstOrDefaultAsync(m => m.QuestionID == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.QuestionID == id);
        }
    }
}
