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
using Microsoft.AspNetCore.Session;
using QuizApp.ViewModels;

namespace QuizApp.Controllers
{
    public class QuizzesController : Controller
    {
        private readonly QuizAppContext _context;

        public QuizzesController(QuizAppContext context)
        {
            _context = context;
        }

        //*************************************************************************************
        // GET: Quizzes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Quizzes.ToListAsync());
        }

        //*************************************************************************************
        // GET: Quizzes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quizzes
                .FirstOrDefaultAsync(m => m.QuizID == id);
            if (quiz == null)
            {
                return NotFound();
            }

            foreach (var question in _context.Questions.Where(x => x.QuizID == id))
            {
                quiz.Questions.Add(question);
            }

            return View(quiz);
        }

        //*************************************************************************************
        // GET: Quizzes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Quizzes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuizID,Title,Description")] Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quiz);
                await _context.SaveChangesAsync();

                HttpContext.Session.SetInt32("QuizID", quiz.QuizID);

                return RedirectToAction("Create", "Questions", new { id = quiz.QuizID});
            }
            return View(quiz);
        }

        //*************************************************************************************
        // GET: Quizzes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            foreach (var question in _context.Questions.Where(q => q.QuizID == id))
            {
                quiz.Questions.Add(question);
            }

            HttpContext.Session.SetInt32("QuizID", quiz.QuizID);

            return View(quiz);
        }

        // POST: Quizzes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuizID,Title,Description,NumberOfQuestions")] Quiz quiz)
        {
            if (id != quiz.QuizID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quiz);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuizExists(quiz.QuizID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(quiz);
        }

        //*************************************************************************************
        // GET: Quizzes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quizzes
                .FirstOrDefaultAsync(m => m.QuizID == id);
            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        // POST: Quizzes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuizExists(int id)
        {
            return _context.Quizzes.Any(e => e.QuizID == id);
        }

        public IActionResult Timeout()
        {
            return View();
        }

        // Get: Quizzes/Solve/1?questionIndex=0
        public IActionResult Solve(int id, int questionIndex)
        {
            // get quiz obj by id from URL and fill it with correcponding questions
            var quiz = _context.Quizzes.Find(id);
            foreach (var question in _context.Questions.Where(q => q.QuizID == id))
            {
                quiz.Questions.Add(question);
            }

            // pass questions to a list
            var questionsList = quiz.Questions.ToList();

            // prepare question a title for a view
            var model = questionsList[questionIndex];
            ViewData["QuizTitle"] = quiz.Title;

            // set question index for post method
            HttpContext.Session.SetInt32("QuestionIndex", questionIndex);
            //TempData["QuestionIndex"] = questionIndex;

            // check if its last question then set flag for a view
            if(questionsList.Count == questionIndex + 1)
            {
                ViewData["LastQuestion"] = "true";
            }

            return View(model);
        }

        // Post: Quizzes/Solve/1?questionIndex=0
        [HttpPost]
        public IActionResult Solve(int id, AnswerEnum answer)
        {
            int questionIndex = (int)HttpContext.Session.GetInt32("QuestionIndex");
            //int questionIndex = (int)TempData["QuestionIndex"];

            // get quiz obj by id from URL and fill it with correcponding questions
            var quiz = _context.Quizzes.Find(id);
            foreach (var question in _context.Questions.Where(q => q.QuizID == id))
            {
                quiz.Questions.Add(question);
            }

            // pass questions to a list
            var questionsList = quiz.Questions.ToList();

            // check answers and set result in tempdata
            if(questionsList[questionIndex].CorrectAnswer == answer)
            {
                HttpContext.Session.SetInt32(questionIndex.ToString(), 1);
                //TempData[questionIndex.ToString()] = 1;
            }
            else
            {
                HttpContext.Session.SetInt32(questionIndex.ToString(), 0);
                //TempData[questionIndex.ToString()] = 0;
            }

            // check for last question and then set number of questions and redirect to summary
            if (questionsList.Count == questionIndex + 1)
            {
                HttpContext.Session.SetInt32("NumberOfQuestions", questionsList.Count);
                //TempData["NumberOfQuestions"] = questionsList.Count;
                return RedirectToAction("Summary");
            }

            // redirect to newxt question
            return RedirectToAction("Solve", "Quizzes", new { questionIndex = questionIndex + 1 });
        }

        public IActionResult Summary()
        {
            QuizResult quizResult = new QuizResult
            {
                QuestionResults = new List<int>()
            };
            for (int x = 0; x < (int)HttpContext.Session.GetInt32("NumberOfQuestions"); x++)
            //for (int x = 0; x < (int)TempData["NumberOfQuestions"]; x++)
            {
                int result = (int)HttpContext.Session.GetInt32(x.ToString());
                quizResult.QuestionResults.Add(result);
                //quizResult.QuestionResults.Add((int)TempData[x.ToString()]);
            }

            return View(quizResult);
        }
    }
}
