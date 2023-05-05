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


            string fileName = "TheoryTerVer.json";
            string jsonString = File.ReadAllText(fileName);
            TheoryTest theoryTest = JsonSerializer.Deserialize<TheoryTest>(jsonString);
        }

        private void buttonGenerateFull_Click(object sender, EventArgs e)
        {
            string fileName = "TheoryTerVer.json";
            string jsonString = File.ReadAllText(fileName);
            
            TheoryTest theoryTest = JsonSerializer.Deserialize<TheoryTest>(jsonString)!;

            checkBox1.Text = theoryTest.theoryTasks[0].question;
        }
    }
}
