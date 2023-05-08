using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace TerVer_project
{



    public partial class FormTest2 : Form
    {

        public FormTest2()
        {
            InitializeComponent();
        }

        private void buttonSelectAllTasks_Click(object sender, EventArgs e)
        {
            numericTheoryTasks.Value = 6;
            checkBoxTask1.Checked = true;
            checkBoxTask2.Checked = true;
            checkBoxTask3.Checked = true;
            checkBoxTask4.Checked = true;
            checkBoxTask5.Checked = true;
            checkBoxTask6.Checked = true;
            checkBoxTask7.Checked = true;
            checkBoxTask8.Checked = true;
            checkBoxTask9.Checked = true;
            checkBoxTask10.Checked = true;
            checkBoxTask11.Checked = true;
            checkBoxTask12.Checked = true;
            checkBoxTask13.Checked = true;
        }

        private void buttonClearAllSelectedTasks_Click(object sender, EventArgs e)
        {
            numericTheoryTasks.Value = 0;
            checkBoxTask1.Checked = false;
            checkBoxTask2.Checked = false;
            checkBoxTask3.Checked = false;
            checkBoxTask4.Checked = false;
            checkBoxTask5.Checked = false;
            checkBoxTask6.Checked = false;
            checkBoxTask7.Checked = false;
            checkBoxTask8.Checked = false;
            checkBoxTask9.Checked = false;
            checkBoxTask10.Checked =false;
            checkBoxTask11.Checked =false;
            checkBoxTask12.Checked =false;
            checkBoxTask13.Checked =false;
        }

        private Word.Application wordapp;
        private Word.Documents worddocuments;

        private Word.Document worddocument;
        private Word.Document worddocumentAnswers;

        private Word.Paragraphs wordparagraphs;
        private Word.Paragraph wordparagraph;

        private Word.Style wordstyleForTitle;
        private Word.Style wordstyleForName;
        private Word.Style wordstyleForStudent;
        private Word.Style wordstyleForTask;

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            int countOfTheoryTasks= Convert.ToInt32(numericTheoryTasks.Value);

            List<bool> practTaskList = new List<bool>();
            practTaskList.Add(checkBoxTask1.Checked);
            practTaskList.Add(checkBoxTask2.Checked);
            practTaskList.Add(checkBoxTask3.Checked);
            practTaskList.Add(checkBoxTask4.Checked);
            practTaskList.Add(checkBoxTask5.Checked);
            practTaskList.Add(checkBoxTask6.Checked);
            practTaskList.Add(checkBoxTask7.Checked);
            practTaskList.Add(checkBoxTask8.Checked);
            practTaskList.Add(checkBoxTask9.Checked);
            practTaskList.Add(checkBoxTask10.Checked);
            practTaskList.Add(checkBoxTask11.Checked);
            practTaskList.Add(checkBoxTask12.Checked);
            practTaskList.Add(checkBoxTask13.Checked);


            variantsTasksList = new List<List<string>>();
            variantsAnswersList = new List<List<string>>();

            int countVariants = Convert.ToInt32(numericKolVariants.Value);

            for (int i = 0; i < countVariants; i++)
            {
                TheoryTest theoryTest = getTheoryTestFromJson();

                this.getTheoryTasksInList(theoryTest, countOfTheoryTasks);
                this.getTaskAndAnswerListInVariant();

                if (practTaskList[0]) createTask1();
            }




            this.InitialWorkWithWord();
            this.workWithTasksWordFile(countVariants);
            this.workWithAnswersWordFile(countVariants);




        }

        private static TheoryTest getTheoryTestFromJson()
        {
            string fileName = "TheoryTerVer.json";
            string jsonString = File.ReadAllText(fileName);

            return JsonSerializer.Deserialize<TheoryTest>(jsonString)!;
        }

        private List<List<string>> variantsTasksList;
        private List<List<string>> variantsAnswersList;
        private List<string> answersList;
        private List<string> theoryTaskList;

        private void getTaskAndAnswerListInVariant()
        {
            variantsTasksList.Add(theoryTaskList);
            variantsAnswersList.Add(answersList);
        }

        private void getTheoryTasksInList(TheoryTest theoryTest,int cntTheoryTasks=6)
        {
            this.theoryTaskList = new List<string>();
            this.answersList = new List<string>();

            for (int i = 0; i < cntTheoryTasks; i++)
            {
                int randIndexForTask = Task.rnd.Next(0, theoryTest.theoryTasks.Count);
                theoryTaskList.Add(theoryTest.getNewTheoryTaskString(i,randIndexForTask));
                answersList.Add(theoryTest.getNewTheoryAnswerString(i, randIndexForTask));
               theoryTest.theoryTasks.RemoveAt(randIndexForTask);
            }
        }

        private void buttonGenerateFull_Click(object sender, EventArgs e)
        {
            variantsTasksList = new List<List<string>>();
            variantsAnswersList = new List<List<string>>();

            int countVariants = Convert.ToInt32(numericKolVariants.Value);

            for (int i = 0; i < countVariants; i++)
            {
                TheoryTest theoryTest = getTheoryTestFromJson();

            this.getTheoryTasksInList(theoryTest);
                this.getTaskAndAnswerListInVariant();

                createTask1();
                
            }

            



            this.InitialWorkWithWord();
            this.workWithTasksWordFile(countVariants);
            this.workWithAnswersWordFile(countVariants);

            List<string> formuls = new List<string>();
            formuls.Add("f(x) = ((1)/(5\u221a(2\u03C0)))e^((-(x+6)^2)/(50))");
            formuls.Add("ㅤㅤㅤ \u23A7 0 \t при x \u2264 0" + "\v" +
                        "f(x) =\u23A8 (3x^2)/125 \t при 0 < x \u2264 5" + "\v" +
                        "ㅤㅤㅤ \u23A9 0 \t при x > 5");
            formuls.Add("\t(4-3\u221a3)/(4\u03C0)");

            
            for (int i = 0; i < 3; i++)
            {
                object oMissing = System.Reflection.Missing.Value;
                worddocumentAnswers.Paragraphs.Add(ref oMissing);
                wordparagraph = wordparagraphs[wordparagraphs.Count];

                wordparagraph.Range.Text = formuls[i];

                worddocumentAnswers.OMaths.Add(wordparagraph.Range).OMaths.BuildUp();
            }
            

        }

        private void createTask1()
        {
            Task1 task1 = new Task1();
            theoryTaskList.Add((theoryTaskList.Count + 1).ToString() + "." + task1.fullTextOfTask);
            answersList.Add((answersList.Count + 1).ToString() + ".\t" + task1.outCorrectAnswer);
        }

        private void InitialWorkWithWord()
        {
            this.openWord();

            this.createNewDocumentWord();

            this.createNewDocumentWord();

            this.StartWorkWithDocs();
        }

        private void workWithTasksWordFile(int countVariants)
        {

            wordparagraphs = worddocument.Paragraphs;

            this.setStylesForWord();

            this.PrintTitleInWord();

            int numOfParagraph = 1;

            for (int i = 0; i < countVariants; i++)
            {
                object oMis = System.Reflection.Missing.Value;
                worddocument.Paragraphs.Add(ref oMis);
                numOfParagraph++;
                OutputVariantInWord(i + 1, numOfParagraph);

                object oMiss = System.Reflection.Missing.Value;
                worddocument.Paragraphs.Add(ref oMiss);
                numOfParagraph++;
                OutputFieldForStudentInWord(numOfParagraph);

                for (int j = 0; j < theoryTaskList.Count; j++)
                {
                    object oMissing = System.Reflection.Missing.Value;
                    worddocument.Paragraphs.Add(ref oMissing);
                    numOfParagraph++;
                    outputTaskInWord(variantsTasksList[i][j], numOfParagraph);
                }
                
                numOfParagraph++;
                CreatePageBreak(numOfParagraph);
            }

            
        }

        private void workWithAnswersWordFile(int countVariants)
        {
            wordparagraphs = worddocumentAnswers.Paragraphs;

            this.setStyleForAnswersWord();

            this.PrintTitleInWord(true);

            int numOfParagraph = 1;

            for (int i=0;i<countVariants;i++)
            {
                object oMis = System.Reflection.Missing.Value;
                worddocumentAnswers.Paragraphs.Add(ref oMis);
                numOfParagraph++;
                OutputVariantInWord(i + 1, numOfParagraph);

                for (int j = 0; j < answersList.Count; j++)
                {
                    object oMissing = System.Reflection.Missing.Value;
                   worddocumentAnswers.Paragraphs.Add(ref oMissing);
                    numOfParagraph++;
                    outputTaskInWord(variantsAnswersList[i][j], numOfParagraph);
                }
            }
        }

        private void openWord()
        {
            try
            {

                wordapp = new Word.Application();

                wordapp.Visible = true;
            }
            catch (Exception ex)
            {
                Text = ex.Message;
            }
        }

        private void createNewDocumentWord()
        {
            Object template = Type.Missing;
            Object newTemplate = false;
            Object documentType = Word.WdNewDocumentType.wdNewBlankDocument;
            Object visible = true;

            wordapp.Documents.Add(ref template, ref newTemplate, ref documentType, ref visible);
        }

       

        private void StartWorkWithDocs()
        {
            worddocuments = wordapp.Documents;

            worddocument = worddocuments[2];
            worddocumentAnswers = worddocuments[1];
            worddocument.Activate();
        }

        private void saveWordFiles()
        {
            try
            {
                worddocument.Save();
                worddocumentAnswers.Save();
            }
            catch (Exception ex)
            {

            }
        }

        private void saveWordFileAs()
        {
            string curDate = DateTime.Now.ToString();
            curDate=curDate.Replace(":", "-");
            Object fileName = @"C:\Users\79186\Тест№2 " + curDate+ ".doc";
            Object fileFormat = Word.WdSaveFormat.wdFormatDocument;
            Object lockComments = false;
            Object password = "";
            Object addToRecentFiles = false;
            Object writePassword = "";
            Object readOnlyRecommended = false;
            Object embedTrueTypeFonts = false;
            Object saveNativePictureFormat = false;
            Object saveFormsData = false;
            Object saveAsAOCELetter = Type.Missing;
            Object encoding = Type.Missing;
            Object insertLineBreaks = Type.Missing;
            Object allowSubstitutions = Type.Missing;
            Object lineEnding = Type.Missing;
            Object addBiDiMarks = Type.Missing;

            worddocument.SaveAs(ref fileName, ref fileFormat, ref lockComments,
 ref password, ref addToRecentFiles, ref writePassword,
 ref readOnlyRecommended, ref embedTrueTypeFonts,
 ref saveNativePictureFormat, ref saveFormsData,
 ref saveAsAOCELetter, ref encoding, ref insertLineBreaks,
 ref allowSubstitutions, ref lineEnding, ref addBiDiMarks);
        }

        private void setStylesForWord()
        {
            object patternstyle = Word.WdStyleType.wdStyleTypeParagraph;

            wordstyleForTitle = worddocument.Styles.Add("myTitleStyle", ref patternstyle);
            wordstyleForTitle.Font.Size = 16;
            wordstyleForTitle.Font.Name = "Times New Roman";
            wordstyleForTitle.Font.Bold = 1;
            wordstyleForTitle.Font.Italic = 0;


            wordstyleForName = worddocument.Styles.Add("myStyle", ref patternstyle);
            
            wordstyleForName.Font.Size = 14;
            wordstyleForName.Font.Name = "Times New Roman";
            wordstyleForName.Font.Bold = 1;
            wordstyleForName.Font.Italic = 0;

            wordstyleForStudent = worddocument.Styles.Add("myStyleForStudent", ref patternstyle);

            wordstyleForStudent.Font.Size = 14;
            wordstyleForStudent.Font.Name = "Times New Roman";
            wordstyleForStudent.Font.Bold = 0;
            wordstyleForStudent.Font.Italic = 1;

            wordstyleForTask = worddocument.Styles.Add("styleForTask", ref patternstyle);
            wordstyleForTask.Font.Size = 12;
            wordstyleForTask.Font.Name = "Times New Roman";
            wordstyleForTask.Font.Bold = 0;
            wordstyleForTask.Font.Italic = 0;
        }

        private void setStyleForAnswersWord()
        {
            object patternstyle= Word.WdStyleType.wdStyleTypeParagraph; ;

            wordstyleForTitle = worddocumentAnswers.Styles.Add("myTitleStyle", ref patternstyle);
            wordstyleForTitle.Font.Size = 16;
            wordstyleForTitle.Font.Name = "Times New Roman";
            wordstyleForTitle.Font.Bold = 1;
            wordstyleForTitle.Font.Italic = 0;


            wordstyleForName = worddocumentAnswers.Styles.Add("myStyle", ref patternstyle);

            wordstyleForName.Font.Size = 14;
            wordstyleForName.Font.Name = "Times New Roman";
            wordstyleForName.Font.Bold = 1;
            wordstyleForName.Font.Italic = 0;

            wordstyleForTask = worddocumentAnswers.Styles.Add("styleForTask", ref patternstyle);
            wordstyleForTask.Font.Size = 12;
            wordstyleForTask.Font.Name = "Times New Roman";
            wordstyleForTask.Font.Bold = 0;
            wordstyleForTask.Font.Italic = 0;
        }

        private void CreatePageBreak(int numOfParagraph)
        {
            object Omissing = System.Reflection.Missing.Value;
            worddocument.Paragraphs.Add(ref Omissing);
            
            wordparagraph = wordparagraphs[numOfParagraph];
            wordparagraph.Range.Text = "\f";
        }

        private void PrintTitleInWord(bool itIsAnswers=false)
        {
            wordparagraph = wordparagraphs[1];
            object titleWordStyle = wordstyleForTitle;

            string curDate = DateTime.Now.ToString();
            curDate = curDate.Replace(":", "-");

            string title;
            if (itIsAnswers) title = "Ответы для теста№2 " + curDate;
            else title = "Тест№2 " + curDate;

            wordparagraph.Range.Text = title;
            wordparagraph.Range.set_Style(ref titleWordStyle);
            wordparagraph.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
        }

        private void OutputVariantInWord(int index,int indexOfParagraph)
        {
            wordparagraph = wordparagraphs[indexOfParagraph];
            object variantWordStyle = wordstyleForName;
            wordparagraph.Range.Text = "Вариант№" + (index).ToString();
            wordparagraph.Range.set_Style(ref variantWordStyle);
            
        }

        private void OutputFieldForStudentInWord(int indexOfParagraph)
        {
            wordparagraph = wordparagraphs[indexOfParagraph];
            object studentWordStyle = wordstyleForStudent;
            wordparagraph.Range.Text = "Фамилия ________________________ Группа __________";
            wordparagraph.Range.set_Style(ref studentWordStyle);
        }

        private void outputTaskInWord(string task,int indexOfParagraph)
        {
            wordparagraph = wordparagraphs[indexOfParagraph];
            object taskWordStyle = wordstyleForTask;
            wordparagraph.Range.Text = task;
            wordparagraph.Range.set_Style(ref taskWordStyle);
        }

        private void saveAndQuitWord()
        {
            Object saveChanges = Word.WdSaveOptions.wdSaveChanges;
            Object originalFormat = Word.WdOriginalFormat.wdWordDocument;
            Object routeDocument = Type.Missing;
            wordapp.Quit(ref saveChanges,
                         ref originalFormat, ref routeDocument);
            wordapp = null;
        }

        private void FormTest2_Load(object sender, EventArgs e)
        {

        }
    }
}
