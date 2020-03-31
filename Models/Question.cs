using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Models
{
    public enum AnswerEnum
    {
        A, B, C, D
    }
    public class Question
    {
        public int QuestionID { get; set; }
        public string QusetionRead { get; set; }
        public string ChoiceA { get; set; }
        public string ChoiceB { get; set; }
        public string ChoiceC { get; set; }
        public string ChoiceD { get; set; }
        public AnswerEnum CorrectAnswer { get; set; }

        public int QuizID { get; set; }
        public virtual Quiz Quiz { get; set; }

    }
}
