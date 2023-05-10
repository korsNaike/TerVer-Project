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

        protected List<object> values=new List<object>();
        
        public List<string> answers;

        public List<string> textList;
        protected string correct_answer;

        public string outCorrectAnswer
        {
            get
            {
                return letterByInd(this.answers.IndexOf(this.correct_answer)) + " " + this.correct_answer;
            }
        }

        public string fullTextOfTask
        {
            get
            {
                return this.outText + "\v" + this.createAnswerString();

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
                            t += " " + this.values[i].ToString();
                        }
                    }

                    return t;
                }
        }

        protected void remixAnswers()
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

        protected string createAnswerString()
        {
            string answersString = "";

            for (int i = 0; i < this.answers.Count; i++)
            {
                answersString += letterByInd(i) + " " + this.answers[i];
                if (i == this.answers.Count-1) answersString += ".";
                else answersString += ";\t";
            }

            return answersString;
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

        protected static int findNOD(int fav, int tot)
        {
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

                if (kol > 100) return 1;
                kol++;
            }

            return n;
        }
    }

   

    class Task1 : Task
    {
        
        public int kolFavComb;
        

        private const int kolTotalComb = 36;

        public Task1()
        {
            string textTask = "Игральная кость бросается два раза. Тогда вероятность того, что сумма выпавших очков – _, а разность – _, равна:";
            this.textList = splitText(textTask);
            this.values = new List<object>();
            this.answers = new List<string>();

            this.values.Add(rnd.Next(2, 12));
            this.values.Add(rnd.Next(0, 5));


            this.kolFavComb = CalcCorrectAnswer(Convert.ToInt32(this.values[0]), Convert.ToInt32(this.values[1]));
            while (this.kolFavComb == 0)
            {
                this.values[0] = rnd.Next(3, 11);
                this.values[1] = rnd.Next(1, 5);
                this.kolFavComb = CalcCorrectAnswer(Convert.ToInt32(this.values[0]), Convert.ToInt32(this.values[1]));
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
            int nod = findNOD(fav, tot);
            fav /= nod;
            tot /= nod;

            return (fav.ToString() + "/" + tot.ToString());
        }   
    }

    class Task2 :Task 
    {
        public Task2()
        {
            string textTask = "Внутрь круга радиуса _ наудачу брошена точка. Тогда вероятность того, что точка окажется _ вписанного в круг равностороннего треугольника, равна:";
            this.textList = splitText(textTask);
            this.values = new List<object>();
            this.answers = new List<string>();

            this.values.Add(rnd.Next(3,8));
            if (rnd.Next(0, 2) == 0) this.values.Add("снаружи");
            else this.values.Add("внутри");

            

            correct_answer = CalcAnswer(Convert.ToInt32(values[0]), values[1].ToString());

            this.CreatingAnswers();
            this.remixAnswers();
        }

        public string cor_answ
        {
            get
            {
                return correct_answer;
            }
        }

        private void CreatingAnswers()
        {
            this.answers.Add(correct_answer);

            string newAnsw;
            string value;

            if (values[1].ToString() == "снаружи") newAnsw = CalcAnswer(Convert.ToInt32(values[0]), "внутри");
            else newAnsw = CalcAnswer(Convert.ToInt32(values[0]), "снаружи");
            this.answers.Add(newAnsw);

            if (rnd.Next(0, 2) == 0) value = "снаружи";
            else value = "внутри";
            newAnsw = CalcAnswer(Convert.ToInt32(values[0]), value).Replace("\u03C0", "").Replace("( -","(1 -");
            this.answers.Add(newAnsw);


            if (rnd.Next(0, 2) == 0)  value= "внутри";
            else value="снаружи";         
            newAnsw = CalcAnswer(Convert.ToInt32(values[0]), value);

            int newValuewPi = rnd.Next(2, 9);
            while (newValuewPi == 4 || newValuewPi%2!=0)
            {
                newValuewPi = rnd.Next(2, 9);
            }
            newAnsw = newAnsw.Replace("(4\u03C0)", "("+newValuewPi+"\u03C0)");

            this.answers.Add(newAnsw);


        }
        
        private static string CalcAnswer(int radius, string place)
        {
            int numInDenominator=radius*radius*4;
            int numInNumerator_beforeSqrt=radius*radius*3;
            int nod;

            switch (place)
            {
                case "внутри":
                    nod = findNOD(numInNumerator_beforeSqrt,numInDenominator);
                    numInNumerator_beforeSqrt /= nod;
                    numInDenominator /= nod;
                    return stringInCorrectFormat(numInNumerator_beforeSqrt,numInDenominator);
                case "снаружи":
                    int numInNumerator_beforeMinus = numInDenominator/4;
                    int nodNumerators = findNOD(numInNumerator_beforeMinus, numInNumerator_beforeSqrt);
                    nod = findNOD(nodNumerators,numInDenominator);
                    numInNumerator_beforeMinus /= nod;
                    numInNumerator_beforeSqrt /= nod;
                    numInDenominator /= nod;
                    return stringInCorrectFormat(numInNumerator_beforeMinus, numInNumerator_beforeSqrt, numInDenominator);
                default:
                    return "что то пошло не так";
            }
        }

       private static string stringInCorrectFormat(int firstEl,int secondEl,int thirdEl=0)
        {
            string outFirst = firstEl.ToString();
            string outSecond = secondEl.ToString();
            string outThird = thirdEl.ToString();

            if (firstEl == 1)
            {
                outFirst = "";
            }

            if (secondEl == 1)
            {
                outSecond = "";
            }
            if (thirdEl == 1)
            {
                outThird = "";
            }
          
            if (thirdEl == 0)
            {
                return "(" + outFirst + "\u221a3)/(" + outSecond + "\u03C0)";
            }
            else
            {
                return "(" + outFirst + "\u03C0 - " + outSecond + "\u221a3)/(" + outThird + "\u03C0)";
            }
        }
    }
}
