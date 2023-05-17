using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Word = Microsoft.Office.Interop.Word;
using TerVer_Project_3_pop.Properties;
using System.Reflection;

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

        const int numberOfTaskWithImagesAnswers= 6;

        private void OffWarnings()
        {
            labelNoTasks.Visible = false;
            labelForException.Visible = false;
            labelWarning.Visible = false;
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            OffWarnings();
            
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

            if (countOfTheoryTasks==0 && !(practTaskList.Contains(true)))
            {
                labelNoTasks.Visible = true;
                return;
            }

            this.InitializeMainLists();

            int countVariants = Convert.ToInt32(numericKolVariants.Value);

            for (int i = 0; i < countVariants; i++)
            {
                TheoryTest theoryTest = getTheoryTestFromJson();

                this.getTheoryTasksInList(theoryTest, countOfTheoryTasks);
                

                if (practTaskList[0]) CreateTask1();
                if (practTaskList[1]) CreateTask2();

                Root practiceTestRoot = GetPracticeTestFromJson();

                for (int j = 0; j < 3; j++)
                {
                  if (practTaskList[2+j]) CreateTasks3_5WithoutPictures(practiceTestRoot, j);
                }

                for (int j = 3; j < practiceTestRoot.types.Count; j++)
                {
                    if (practTaskList[j + 2])
                    {
                        if (j != numberOfTaskWithImagesAnswers) CreateTaskWithTitleImage(practiceTestRoot, j);
                        else CreateTaskWithImages(practiceTestRoot);
                    }
                }

                this.AddTaskAndAnswerListInVariant();
            }




            this.InitialWorkWithWord();
            this.workWithTasksWordFile(countVariants);
            List<string> imagesPaths = imagesFileNames.Select(imgName => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", imgName)).ToList();
            ReplacePlaceholdersWithImages(imagesPaths);
            this.workWithAnswersWordFile(countVariants);




        }

        private static TheoryTest getTheoryTestFromJson()
        {
            string fileName = "TheoryTerVer.json";
            string jsonString = File.ReadAllText(fileName);

            return System.Text.Json.JsonSerializer.Deserialize<TheoryTest>(jsonString)!;
        }

        private static Root GetPracticeTestFromJson()
        {
            string fileName = "PracticeTerVer.json";
            string jsonString = File.ReadAllText(fileName);

            return JsonConvert.DeserializeObject<Root>(jsonString)!;
        }

        private List<List<string>> variantsTasksList;
        private List<List<string>> variantsAnswersList;
        private List<string> answersList;
        private List<string> theoryTaskList;
        private List<string> imagesFileNames;

        private void InitializeMainLists()
        {
            variantsTasksList = new List<List<string>>();
            variantsAnswersList = new List<List<string>>();
            imagesFileNames = new List<string>();
        }

        private void AddTaskAndAnswerListInVariant()
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
                int begRand;
                if (cntTheoryTasks == 6) begRand = i * 8;
                else begRand = 0;

                int endRand;
                if (cntTheoryTasks==6 && (begRand + 8 <= theoryTest.theoryTasks.Count)) endRand = begRand + 8;
                else endRand = theoryTest.theoryTasks.Count;

                int randIndexForTask = Task.rnd.Next(begRand, endRand);
                string newTheoryTaskString = theoryTest.getNewTheoryTaskString(i, randIndexForTask);

                List<string> imagesInTheoryTask = TheoryTest.GetImageFileNames(newTheoryTaskString);
                if (imagesInTheoryTask.Count == 0)
                {

                    answersList.Add(theoryTest.getNewTheoryAnswerString(i, randIndexForTask));
                }
                else
                {
                    newTheoryTaskString = TheoryTest.ReplaceImageFileNames(newTheoryTaskString);

                    foreach (string imageName in imagesInTheoryTask)
                    {
                        imagesFileNames.Add(imageName);
                    }

                    answersList.Add(theoryTest.getNewTheoryLetterString(i,randIndexForTask));
                }

                theoryTaskList.Add(newTheoryTaskString);

                theoryTest.theoryTasks.RemoveAt(randIndexForTask);
            }
        }

        private void buttonGenerateFull_Click(object sender, EventArgs e)
        {
            this.InitializeMainLists();

            OffWarnings();
           

            int countVariants = Convert.ToInt32(numericKolVariants.Value);

           

            for (int i = 0; i < countVariants; i++)
            {
                TheoryTest theoryTest = getTheoryTestFromJson();

            this.getTheoryTasksInList(theoryTest);
                

                CreateTask1();
                CreateTask2();

                Root practiceTestRoot = GetPracticeTestFromJson();

                for (int j = 0;j < 3; j++)
                {
                    CreateTasks3_5WithoutPictures(practiceTestRoot,j);
                }

                for (int j = 3; j < practiceTestRoot.types.Count; j++)
                {
                    if (j != numberOfTaskWithImagesAnswers) CreateTaskWithTitleImage(practiceTestRoot, j);
                    else CreateTaskWithImages(practiceTestRoot);
                }

                

                this.AddTaskAndAnswerListInVariant();
            }

            List<string> imagesPaths = imagesFileNames.Select(imgName => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", imgName)).ToList();



            this.InitialWorkWithWord();

            try
            {
                this.workWithTasksWordFile(countVariants);
                ReplacePlaceholdersWithImages(imagesPaths);
            }
            catch (Exception ex)
            {
                labelWarning.Visible = true;
               labelWarning.Text = "Что-то пошло не так с Word\n файлом теста!";
            }

            try
            {
                this.workWithAnswersWordFile(countVariants);
            }
            catch (Exception ex)
            {
                labelWarning.Visible = true;
                labelWarning.Text = "Что-то пошло не так с Word\n файлом ответов!";
            }
            



        }

        private void CreateTask1()
        {
            Task1 task1 = new Task1();
            theoryTaskList.Add((theoryTaskList.Count + 1).ToString() + "." + task1.fullTextOfTask);
            answersList.Add((answersList.Count + 1).ToString() + ".\t" + task1.outCorrectAnswer);
        }

        private void CreateTask2()
        {
            Task2 task2 = new Task2();
            theoryTaskList.Add((theoryTaskList.Count + 1).ToString() + "." + task2.fullTextOfTask);
            answersList.Add((answersList.Count + 1).ToString() + ".\t" + task2.outCorrectAnswer);

        }

        private void CreateTasks3_5WithoutPictures(Root data,int index)
        {
                PracticeTask task = data.types[index].getRandomTask();
                task.prepareToContinue();
                theoryTaskList.Add((theoryTaskList.Count + 1).ToString() + "." + task.fullTextOfTaskWithoutImages);
                answersList.Add((answersList.Count + 1).ToString() + ".\t" + task.outCorrectAnswerWithouImages);
        }

        private void CreateTaskWithTitleImage(Root data,int numberOfType)
        {
            PracticeTask task = data.types[numberOfType].getRandomTask();
            task.prepareToContinue();
            theoryTaskList.Add((theoryTaskList.Count + 1).ToString() + "." + task.textWithPlaceForImageInTitle);
            answersList.Add((answersList.Count + 1).ToString() + ".\t" + task.outCorrectAnswerWithouImages);
            imagesFileNames.Add(task.imagesSource.title);
            if (task.imagesSource.title2 != null) imagesFileNames.Add(task.imagesSource.title2);
        }

        private void CreateTaskWithImages(Root data,int numberOfType=numberOfTaskWithImagesAnswers)
        {
            PracticeTask task = data.types[numberOfType].getRandomTask();
            task.prepareToContinue();
            theoryTaskList.Add((theoryTaskList.Count + 1).ToString() + "." + task.textWithPlacesForImages);
            answersList.Add((answersList.Count + 1).ToString() + ".\t" + task.outCorrectAnswerOnlyLetter);
            imagesFileNames.Add(task.imagesSource.title);

            foreach(string imgName in task.answers)
            {
                imagesFileNames.Add(imgName);
            }
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

        private void ReplacePlaceholdersWithImages(List<string> imagesPaths)
        {

            Word.Document wordDoc = worddocument;

            
                int imgIndex = 0;
                while (imgIndex < imagesPaths.Count)
                {
                    object findText = "placeForImage";
                    object replaceWith = "^c";
                    object missing = Missing.Value;

                    Word.Range rng = wordDoc.Content;
                    rng.Find.ClearFormatting();
                    rng.Find.Replacement.ClearFormatting();
                object matchWholeWord = true;

                
                    
                        rng.Copy();
                 
                try
                {
                    Word.InlineShape inlineShape = wordDoc.InlineShapes.AddPicture(imagesPaths[imgIndex], LinkToFile: false, SaveWithDocument: true);
                
                    
                    inlineShape.Range.Copy();

                    rng.Find.Execute(
                            FindText: ref findText,
                            ReplaceWith: ref replaceWith,
                            MatchWholeWord: ref matchWholeWord,
                            Replace: Word.WdReplace.wdReplaceOne
                        );

                  
                        inlineShape.Delete();

                        imgIndex++;
                }
                catch (Exception ex)
                {
                    string thisImagePath = imagesPaths[imgIndex];
                    labelForException.Visible = true;
                    labelForException.Text = "Ошибка!\nВ пути :\n"+thisImagePath +"\nНе был найден файл:\n" + imagesFileNames[imgIndex];
                }
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
                Text = "Тест№2";

                wordapp = new Word.Application();

                wordapp.Visible = true;
            }
            catch (Exception ex)
            {
                Text = "Word не удалось открыть!";
            }
        }

        private void createNewDocumentWord()
        {
            Object template = System.Type.Missing;
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
                Text = "Тест№2";
                worddocument.Save();
                worddocumentAnswers.Save();
            }
            catch (Exception ex)
            {
                Text = "Файлы Word не были сохранены!";
            }
        }

        private void saveWordFileAs()
        {
            string curDate = DateTime.Now.ToString();
            curDate=curDate.Replace(":", " -");
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
            Object saveAsAOCELetter = System.Type.Missing;
            Object encoding = System.Type.Missing;
            Object insertLineBreaks = System.Type.Missing;
            Object allowSubstitutions = System.Type.Missing;
            Object lineEnding = System.Type.Missing;
            Object addBiDiMarks = System.Type.Missing;


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
            Object routeDocument = System.Type.Missing;
            wordapp.Quit(ref saveChanges,
                         ref originalFormat, ref routeDocument);
            wordapp = null;
        }

        private void FormTest2_Load(object sender, EventArgs e)
        {
            Task2 task2 = new Task2();
            Text = task2.cor_answ;

            numericTheoryTasks.ValueChanged += (s, a) => { if (numericTheoryTasks.Value != 0) OffWarnings(); };
            checkBoxTask1.CheckedChanged += (s, a) => { if (checkBoxTask1.Checked) OffWarnings(); };
            checkBoxTask2.CheckedChanged += (s, a) => { if (checkBoxTask2.Checked) OffWarnings(); };
            checkBoxTask3.CheckedChanged += (s, a) => { if (checkBoxTask3.Checked) OffWarnings(); };
            checkBoxTask4.CheckedChanged += (s, a) => { if (checkBoxTask4.Checked) OffWarnings(); };
            checkBoxTask5.CheckedChanged += (s, a) => { if (checkBoxTask5.Checked) OffWarnings(); };
            checkBoxTask6.CheckedChanged += (s, a) => { if (checkBoxTask6.Checked) OffWarnings(); };
            checkBoxTask7.CheckedChanged += (s, a) => { if (checkBoxTask7.Checked) OffWarnings(); };
            checkBoxTask8.CheckedChanged += (s, a) => { if (checkBoxTask8.Checked) OffWarnings(); };
            checkBoxTask9.CheckedChanged += (s, a) => { if (checkBoxTask9.Checked) OffWarnings(); };
            checkBoxTask10.CheckedChanged += (s, a) => { if (checkBoxTask10.Checked) OffWarnings(); };
            checkBoxTask11.CheckedChanged += (s, a) => { if (checkBoxTask11.Checked) OffWarnings(); };
            checkBoxTask12.CheckedChanged += (s, a) => { if (checkBoxTask12.Checked) OffWarnings(); };
            checkBoxTask13.CheckedChanged += (s, a) => { if (checkBoxTask13.Checked) OffWarnings(); };
            
        }
    }
}
