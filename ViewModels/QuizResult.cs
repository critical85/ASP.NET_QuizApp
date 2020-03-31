using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.ViewModels
{
    public class QuizResult
    {
        public QuizResult()
        {
            QuestionResults = new List<int>();
        }

        public List<int> QuestionResults { get; set; }
    }
}
