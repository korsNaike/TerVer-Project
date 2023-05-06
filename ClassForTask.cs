using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerVer_project
{
    abstract public class Task
    {
        public string text;

        public static Random rnd = new Random();

        public Task()
        {

        }

        static protected List<string> splitText(string text)
        {
            string[] splittedText = text.Split('_');
            return new List<string>(splittedText);
        }

        static public string letterByInd(int i,bool upperCase=false)
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

   

    class Task1 : Task
    {
        public List<string> textList;
        protected List<int> values;
        private string correct_answer;
        public int kolFavComb;
        public List<string> answers;

        private const int kolTotalComb = 36;

        public Task1()
        {
            string textTask = "Игральная кость бросается два раза. Тогда вероятность того, что сумма выпавших очков – _, а разность – _, равна:";
            this.textList = splitText(textTask);
            this.values = new List<int>();
            this.answers = new List<string>();

            this.values.Add(rnd.Next(2, 12));
            this.values.Add(rnd.Next(0, 5));


            this.kolFavComb = CalcCorrectAnswer(this.values[0], this.values[1]);
            while (this.kolFavComb == 0)
            {
                this.values[0] = rnd.Next(3, 11);
                this.values[1] = rnd.Next(1, 5);
                this.kolFavComb = CalcCorrectAnswer(this.values[0], this.values[1]);
            }
            this.correct_answer = convertToFraction(kolFavComb, kolTotalComb);

            this.creatingAnswers();
            this.remixAnswers();
        }

        private void creatingAnswers()
        {
            this.answers.Add(this.correct_answer);
            string newAnsw;

            if ((rnd.Next(1, 2)) == 1)
            {
                newAnsw = convertToFraction(Math.Abs(kolFavComb - 1), kolTotalComb);
                if (!answers.Contains(newAnsw)) answers.Add(newAnsw);
            }
            else
            {
                newAnsw = convertToFraction(Math.Abs(kolFavComb + 1), kolTotalComb);
                if (!answers.Contains(newAnsw)) answers.Add(newAnsw);
            }

            int firstRand = rnd.Next(-1, 1);
            int secondRand = rnd.Next(-1, 1);

            while (firstRand == 0 && secondRand == 0)
            {
                firstRand = rnd.Next(-1, 1);
                secondRand = rnd.Next(-1, 1);
            }


            while (answers.Count < 4)
            {
                newAnsw = convertToFraction(CalcCorrectAnswer(rnd.Next(3, 11), rnd.Next(1, 4)) + rnd.Next(2, 6), kolTotalComb);
                if (!answers.Contains(newAnsw)) answers.Add(newAnsw);
            }



        }


        private static int CalcCorrectAnswer(int sum, int razn)
        {
            int n = 0;
            for (int i = 1; i <= 6; i++)
            {
                for (int j = 1; j <= 6; j++)
                {
                    if (i + j == sum && Math.Abs(i - j) == razn)
                    {
                        n++;
                    }
                }

            }

            return n;
        }

        private static string convertToFraction(int fav, int tot)
        {
            int nod;
            int m = fav;
            int n = tot;
            int kol = 0;
            while (m != n)
            {
                if (m > n)
                {
                    m = m - n;
                }
                else
                {
                    n = n - m;
                }

                if (kol > 100) return (fav.ToString() + "/" + tot.ToString());
                kol++;
            }

            nod = n;
            fav /= nod;
            tot /= nod;

            return (fav.ToString() + "/" + tot.ToString());
        }

        public string fullTextOfTask
        {
            get
            {
                return this.outText+ "\v"+this.createAnswerString();

            }
        }

        public string outText
        {
            get
            {
                string t = "";
                for (int i = 0; i < this.textList.Count; i++)
                {
                    t += this.textList[i];
                    if (i < this.values.Count)
                    {
                        t += " " + this.values[i];
                    }
                }

                return t;
            }
        }

        public string outCorrectAnswer
        {
            get
            {
                return letterByInd(this.answers.IndexOf(this.correct_answer))+" "+this.correct_answer;
            }
        }

        private void remixAnswers()
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

        private string createAnswerString()
        {
                string answersString = "";
                
                for (int i = 0; i < this.answers.Count; i++)
                {
                    answersString += letterByInd(i) +" " +this.answers[i];
                    if (i == 3) answersString += ".";
                    else answersString += ";\t";
                }

                return answersString;
        }

        
    }
}
