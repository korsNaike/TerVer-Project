using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerVer_project
{
    public class PracticeTest 
    {
        public List<VariantsOfTasks> types { get; set; }
    }

    public class VariantsOfTasks 
    {
        public List<VariantOfTask> tasks { get; set; }
    }

    public class VariantOfTask : Task
    {
        public new string text { get; set; }
        public new string correct_answer { get; set; }

        public new List<string> answers { get; set; }


        public ImagesSources imagesSources { get; set; }

        protected new void remixAnswers()
        {
            List<string> copyAnswers = new List<string>(this.answers);
            List<string> mixAnswers = new List<string>();

            for (int i = 0; i < this.answers.Count; i++)
            {
                int randInd = rnd.Next(0, copyAnswers.Count);
                mixAnswers.Add(copyAnswers[randInd]);
                copyAnswers.RemoveAt(randInd);
            }

            this.answers = new List<string>(mixAnswers);

        }


        public void prepareTask()
        {
            
            if (imagesSources == null)
            {
                this.remixAnswers();
                return;
            }

            if (imagesSources.answer == null && imagesSources.title!=null)
            {
                this.remixAnswers();

                return;
            }

            if (imagesSources.answer != null)
            {
                this.answers = imagesSources.answer;
                this.remixAnswers();

                return;
            }

        }

        public string OutputTaskWithIndex(int index)
        {
            return (index).ToString() +"\t"+ OutputTask;
        }

        protected string OutputTask
        {
            get
            {
                return this.text + "\v" + this.createAnswerString();
            }
        }
    }

    public class ImagesSources 
    {
        public string title { get; set; }
        public List<string> answer { get; set; }

        Random rnd = new Random();

        public static string getPathToImage(string nameOfFile)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", nameOfFile);
        }

        
    }
}
