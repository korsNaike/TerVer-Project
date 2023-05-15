using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerVer_project
{
    public class PracticeTask
    {
        public string text { get; set; }
        public string correct_answer { get; set; }
        public List<string> answers { get; set; }
        public ImagesSource imagesSource { get; set; }

        public string outCorrectAnswerWithouImages
        {
            get
            {
                return letterByInd(this.answers.IndexOf(this.correct_answer)) + " " + this.correct_answer;
            }
        }

        public string outCorrectAnswerOnlyLetter
        {
            get
            {
                return letterByInd(this.answers.IndexOf(this.correct_answer));
            }
        }


        public string fullTextOfTaskWithoutImages
        {
            get
            {
                return this.text + "\v" + this.createAnswerString();

            }
        }

        public string textWithPlaceForImageInTitle
        {
            get
            {
                string[] splitted;
                if (imagesSource.title2 != null)
                {

                    splitted = text.Split(':');
                    if (splitted.Length==3) return splitted[0] + "\v" + " placeForImage \v" + splitted[1] + "\v placeForImage \v"+splitted[2]+"\v" + this.createAnswerString();
                }
                splitted = text.Split('.');
                if (splitted.Length == 2) return splitted[0] + "\v" + " placeForImage \v" + splitted[1] + "\v" + this.createAnswerString();

                return this.text + "\v" + " placeForImage" + "\v" + this.createAnswerString();
            }
        }

        public string textWithPlacesForImages
        {
            get
            {
                string[] splitted = text.Split('.');
                if (splitted.Length == 2) return splitted[0] + "\v" + " placeForImage \v" + splitted[1] + "\v" + this.createAnswerStringWithPlaceForImages();
                
                return this.text + "\v" + " placeForImage" + "\v" + this.createAnswerStringWithPlaceForImages();
            }
        }

        public void prepareToContinue()
        {
            if (imagesSource == null)
            {
                remixAnswers();
                return;
            }

            if (imagesSource.answer == null)
            {
                remixAnswers();
                return;
            }

            imagesSource.remixImages();
            this.answers = imagesSource.answer;
            
        }

        

        protected void remixAnswers()
        {
            List<string> copyAnswers = new List<string>(this.answers);
            List<string> mixAnswers = new List<string>();

            for (int i = 0; i < this.answers.Count; i++)
            {
                int randInd = Task.rnd.Next(0, copyAnswers.Count);
                mixAnswers.Add(copyAnswers[randInd]);
                copyAnswers.RemoveAt(randInd);
            }

            this.answers = new List<string>(mixAnswers);

        }

        protected string createAnswerString()
        {
            string answersString = "";

            for (int i = 0; i < this.answers.Count; i++)
            {
                answersString += letterByInd(i) + " " + this.answers[i];
                if (i == this.answers.Count - 1) answersString += ".";
                else answersString += ";\t";
            }

            if (answersString.Length>80)
            {
                answersString = "";
                for (int i = 0; i < this.answers.Count; i++)
                {
                    answersString += letterByInd(i) + " " + this.answers[i];
                    if (i == this.answers.Count - 1) answersString += ".";
                    else answersString += ";";

                    if (i == 1) answersString += "\v";
                    else answersString += "\t";
                }
            }

            return answersString;
        }

        protected string createAnswerStringWithPlaceForImages()
        {
            string answersString = "";

            for (int i = 0; i < this.answers.Count; i++)
            {
                answersString += letterByInd(i) + " placeForImage";
                if (i == 1) answersString += "\v";
                else answersString += "\t";
                
            }

            return answersString;
        }


        static public string letterByInd(int i, bool upperCase = false)
        {
            if (!upperCase)
            {
                switch (i)
                {
                    case 0: return "a)";
                    case 1: return "б)";
                    case 2: return "в)";
                    case 3: return "г)";
                }
            }
            else
            {
                switch (i)
                {
                    case 0: return "А.";
                    case 1: return "Б.";
                    case 2: return "В.";
                    case 3: return "Г.";
                }
            }

            return "нет такой буквы";
        }
    }

    public class ImagesSource
    {
        public string title { get; set; }

        public string title2 { get; set; }
        public List<string> answer { get; set; }

        protected internal void remixImages()
        {
            List<string> copyAnswers = new List<string>(this.answer);
            List<string> mixAnswers = new List<string>();

            for (int i = 0; i < this.answer.Count; i++)
            {
                int randInd = Task.rnd.Next(0, copyAnswers.Count);
                mixAnswers.Add(copyAnswers[randInd]);
                copyAnswers.RemoveAt(randInd);
            }

            this.answer = new List<string>(mixAnswers);

        }
    }

    public class Type
    {
        public List<PracticeTask> tasks { get; set; }

        public PracticeTask getRandomTask()
        {
            int rndInd = Task.rnd.Next(0, tasks.Count);

            return tasks[rndInd];
        }
    }

    public class Root
    {
        public List<Type> types { get; set; }
    }
}
