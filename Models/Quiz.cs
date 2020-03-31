using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Models
{
    public class Quiz
    {
        public int QuizID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
