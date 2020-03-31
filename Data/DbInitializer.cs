using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizApp.Models;

namespace QuizApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(QuizAppContext context)
        {
            context.Database.EnsureCreated();

            // test any quiz
            if(context.Quizzes.Any())
            {
                return;
            }

            // if no quizzes seed database by these
            var quizes = new Quiz[]
            {
                new Quiz{Title="Math",Description="Basic mathematical quiz"},
                new Quiz{Title="Atrology",Description="Basic astrological quiz"}
            };

            foreach(Quiz q in quizes)
            {
                context.Quizzes.Add(q);
            }
            context.SaveChanges();

            // and seed with these questions
            var questions = new Question[]
            {
                new Question{QusetionRead="5+5?",ChoiceA="0",ChoiceB="5",ChoiceC="10",ChoiceD="15",CorrectAnswer=AnswerEnum.C,QuizID=1},
                new Question{QusetionRead="5*5?",ChoiceA="5",ChoiceB="25",ChoiceC="35",ChoiceD="50",CorrectAnswer=AnswerEnum.B,QuizID=1},
                new Question{QusetionRead="5/5?",ChoiceA="1",ChoiceB="5",ChoiceC="15",ChoiceD="2",CorrectAnswer=AnswerEnum.A,QuizID=1},
                new Question{QusetionRead="What is Sun?",ChoiceA="Star",ChoiceB="Planet",ChoiceC="Galaxy",ChoiceD="Comet",CorrectAnswer=AnswerEnum.A,QuizID=2},
                new Question{QusetionRead="What is Earth?",ChoiceA="Star",ChoiceB="Galaxy",ChoiceC="Comet",ChoiceD="Planet",CorrectAnswer=AnswerEnum.D,QuizID=2},
                new Question{QusetionRead="What is Milkey Way?",ChoiceA="Star",ChoiceB="Galaxy",ChoiceC="Comet",ChoiceD="Planet",CorrectAnswer=AnswerEnum.B,QuizID=2}
            };

            foreach(Question q in questions)
            {
                context.Questions.Add(q);
            }
            context.SaveChanges();

        }
    }
}
