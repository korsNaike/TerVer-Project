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

    }
    public class TheoryTask : Task
    {
        public string question { get; set; }
        public string correct_answer { get; set; }
        public List<string> other_answers { get; set; }


    }
}
