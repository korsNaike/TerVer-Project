using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace TerVer_project
{
    public class TheoryTest : Task
    {
        public List<TheoryTask> theoryTasks { get; set; }

        public string getNewTheoryTaskString(int index,int randIndexForTask)
        {
            string newTheoryTask = "";

            

            TheoryTask task = this.theoryTasks[randIndexForTask];
            newTheoryTask += task.getQuestionTheoryTaskString(index);

            const int allAnswersCnt = 4;

            for (int j = 0; j < allAnswersCnt; j++)
            {
                newTheoryTask += task.getAnswerOptionTheoryTaskString(j);
            }

            return newTheoryTask;
        }

        public string getNewTheoryAnswerString(int index,int randIndexForTask)
        {
            TheoryTask task = this.theoryTasks[randIndexForTask];
            return (index+1).ToString() + ".\t" + task.true_answer;
        }

    }
    public class TheoryTask : Task
    {
        public string question { get; set; }
        public string correct_answer { get; set; }
        public List<string> all_answers { get; set; }

        protected internal string true_answer;
  

        public string getQuestionTheoryTaskString(int index)
        {
            return (index + 1).ToString() + "." + this.question + "\v";
        }

        public string getAnswerOptionTheoryTaskString(int index, int allAnswersCnt = 4)
        {
            string newTheoryTask = "";
            int rndIndexForAnswer = rnd.Next(0, this.all_answers.Count);
            newTheoryTask += letterByInd(index, true) +" "+ this.all_answers[rndIndexForAnswer];

            if (this.all_answers[rndIndexForAnswer] == correct_answer)
            {
                true_answer = newTheoryTask+".";
            }

            if (index + 1 == allAnswersCnt) newTheoryTask += ".";
            else newTheoryTask += ";";

            

            this.all_answers.RemoveAt(rndIndexForAnswer);
            newTheoryTask += "\v";

            return newTheoryTask;
        }

    }
}
