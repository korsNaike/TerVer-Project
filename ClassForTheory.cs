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

        public string getNewTheoryLetterString(int index,int randIndexForTask)
        {
            TheoryTask task = this.theoryTasks[randIndexForTask];
            return (index + 1).ToString() + ".\t" + task.true_answerLetterIndex;
        }

        public static List<string> GetImageFileNames(string input)
        {
            List<string> result = new List<string>();

            char[] separators = { ' ',';', ',', ':', '\t' };

            string[] imageExtensions = { ".gif", ".jpg", ".jpeg", ".png" };

            string[] words = input.Split(separators);


            foreach (string word in words)
            {
                foreach (string extension in imageExtensions)
                {
                    if (word.EndsWith(extension))
                    {
                        result.Add(word);
                        break;
                    }
                }
            }

            return result;
        }

        public static string ReplaceImageFileNames(string input)
        {

            char[] separators = { ' ', ';', ',', ':', '\t' };

            string[] imageExtensions = { ".gif", ".jpg", ".jpeg", ".png" };


            string[] words = input.Split(separators);

 
            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];

                foreach (string extension in imageExtensions)
                {
                    if (word.EndsWith(extension))
                    {
                        words[i] = "placeForImage";
                        break;
                    }
                }
            }

            string result = string.Join(" ", words);

            return result;
        }

    }
    public class TheoryTask : Task
    {
        public string question { get; set; }
        public new string correct_answer { get; set; }
        public List<string> all_answers { get; set; }

        protected internal string true_answer;
        protected internal string true_answerLetterIndex;
  

        public string getQuestionTheoryTaskString(int index)
        {
            return (index + 1).ToString() + "." + this.question + " \v";
        }

        public string getAnswerOptionTheoryTaskString(int index, int allAnswersCnt = 4)
        {
            string newTheoryTask = "";

            int rndIndexForAnswer = rnd.Next(0, this.all_answers.Count);
            newTheoryTask += letterByInd(index, true) +" "+ this.all_answers[rndIndexForAnswer]+" ";

            if (this.all_answers[rndIndexForAnswer] == correct_answer)
            {
                true_answerLetterIndex = letterByInd(index, true);
                true_answer = newTheoryTask+".";
            }

            if (index + 1 == allAnswersCnt) newTheoryTask += ".";
            else newTheoryTask += ";";

            

            this.all_answers.RemoveAt(rndIndexForAnswer);
            newTheoryTask += "\v";

            return newTheoryTask;
        }

        public string FindImageWord(string str)
        {
            string[] words = str.Split(' ');

            foreach (string word in words)
            {
                if (word.EndsWith(".gif") ||
                    word.EndsWith(".jpg") ||
                    word.EndsWith(".png"))
                {
                    return word;
                }
            }

            return null;
        }

    }
}
