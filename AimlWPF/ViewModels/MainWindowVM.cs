using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AimlWPF.ViewModels
{
    class MainWindowVM
    {
        public void readAimlFile(string filename)
        {
            AimlParser aimlParser = new AimlParser(filename);
            if (!aimlParser.containCategory)
            {
                aimlParser.aimlObjectRead();
            }
            else
            {
                MessageBox.Show("Kategória už existuje", "Správa");
            }
        }
    }
}
