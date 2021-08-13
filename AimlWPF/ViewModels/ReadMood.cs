using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace AimlWPF.ViewModels
{
    class ReadMood
    {
        Aiml aiml;
        private TextBox MoodTextBox;
        private TextBox ContentTextBox;

        public ReadMood(TextBox editContenTextBox, TextBox MoodTextBox, Aiml aiml)
        {
            this.ContentTextBox = editContenTextBox;
            this.MoodTextBox = MoodTextBox;
            this.aiml = aiml;
        }

        public void aimlObjectRead()
        {
            XDocument contentDocument = XDocument.Parse(ContentTextBox.Text);
            IEnumerable<XElement> aimlObjects = from aimlObject in contentDocument.Descendants("category") select aimlObject;
            IEnumerable<XElement> aimlChildNodes = aimlObjects.Elements();

            foreach (XElement aimlChildNode in aimlChildNodes)
            {
                string tmp;
                switch (aimlChildNode.Name.ToString())
                {
                    case "pattern":
                        break;
                    case "template":
                        aimlMoodRead(aimlChildNode);
                        //ContentTextBox.Text = contentDocument.ToString();
                        break;
                }
            }
        }

        private void aimlMoodRead(XElement aimlMood)
        {
            IEnumerable<XElement> aimlChildNodes = aimlMood.Descendants("think");
            IEnumerable<XElement> aimlObjects = from aimlObject in aimlMood.Descendants("think") select aimlObject;

            foreach (XElement aimlChildNode in aimlChildNodes)
            {
                getMood(aimlChildNode);
            }

        }

        private void getMood(XElement child)
        {
            IEnumerable<XElement> aimlChildNodes = child.Elements();

            foreach (XElement childNode in aimlChildNodes)
            {
                foreach (XAttribute attribute in childNode.Attributes())
                {
                    switch (attribute.Value.ToString())
                    {
                        case "mood":
                            if (!MoodTextBox.Text.ToString().Equals("-"))
                            {
                                MoodTextBox.Text = childNode.Value;
                                aiml.Mood = Int32.Parse(childNode.Value);
                            }
                            else
                            {
                                childNode.Value = "0";
                                aiml.Mood = 0;
                            }
                            break;
                    }
                }
            }
        }
    }
}
