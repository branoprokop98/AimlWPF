using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace AimlWPF.ViewModels
{
    class WriteMood
    {
        private TextBox editContentBox;
        private TextBox moodTextBox;
        private Aiml aiml;


        public WriteMood(TextBox editContentBox, TextBox moodTextBox, Aiml aiml)
        {
            this.editContentBox = editContentBox;
            this.moodTextBox = moodTextBox;
            this.aiml = aiml;
        }

        public void readAiml()
        {
            XDocument contentDocument = XDocument.Parse(editContentBox.Text);
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
                        aimlMoodRead(aimlChildNode, contentDocument);
                        editContentBox.Text = contentDocument.ToString();
                        break;
                }
            }
        }

        private void aimlMoodRead(XElement aimlTemplate, XDocument contentDocument)
        {

            IEnumerable<XElement> aimlChildNodes = aimlTemplate.Descendants("think");
            IEnumerable<XElement> aimlObjects = from aimlObject in aimlTemplate.Descendants("think") select aimlObject;

            if (moodTextBox.Text.Equals(""))
            {
                aimlTemplate.Elements("think").Elements("set").Where(x => x.Attribute("name").Value.Equals("mood")).Remove();
                aiml.Mood = 0;
                aimlTemplate.Elements("think").Where(x => !x.HasElements).Remove();
                return;
            }
            //foreach (XElement aimlChildNode in aimlChildNodes)
            //{


            //    //if (getMood(aimlChildNode))
            //    //{
            //    //    return;
            //    //}
            //}
            if (getMood2(aimlChildNodes))
            {
                return;
            }

            if (!moodTextBox.Text.ToString().Equals("-"))
            {
                aimlTemplate.Add(new XElement("think", new XElement("set", new XAttribute("name", "mood"), moodTextBox.Text.ToString())));
                aiml.Mood = Int32.Parse(moodTextBox.Text.ToString());
            }
            else
            {
                aimlTemplate.Add(new XElement("think", new XElement("set", new XAttribute("name", "mood"), "0")));
                aiml.Mood = 0;
            }
        }

        private bool getMood(XElement aimlChildNode)
        {
            IEnumerable<XElement> aimlChildNodes = aimlChildNode.Elements();

            foreach (XElement childNode in aimlChildNodes)
            {
                foreach (XAttribute attribute in childNode.Attributes())
                {
                    switch (attribute.Value.ToString())
                    {
                        case "mood":
                            childNode.Value = "";
                            if (!moodTextBox.Text.ToString().Equals("-") || !moodTextBox.Text.ToString().Equals(""))
                            {
                                childNode.Value = moodTextBox.Text.ToString();
                                aiml.Mood = Int32.Parse(childNode.Value);
                            }
                            else
                            {
                                childNode.Value = "0";
                                aiml.Mood = 0;
                            }
                            return true;
                    }
                }
            }
            return false;
        }

        private bool getMood2(IEnumerable<XElement> aimlChildNodes)
        {
            foreach (XElement aimlChildNode in aimlChildNodes)
            {
                IEnumerable<XElement> thinkNodes = aimlChildNode.Elements();
                foreach (XElement childNode in thinkNodes)
                {
                    foreach (XAttribute attribute in childNode.Attributes())
                    {
                        switch (attribute.Value.ToString())
                        {
                            case "mood":
                                childNode.Value = "";
                                if (!moodTextBox.Text.ToString().Equals("-") || !moodTextBox.Text.ToString().Equals(""))
                                {
                                    childNode.Value = moodTextBox.Text.ToString();
                                    aiml.Mood = Int32.Parse(childNode.Value);
                                }
                                else
                                {
                                    childNode.Value = "0";
                                    aiml.Mood = 0;
                                }
                                return true;
                        }
                    }
                }
            }
            return false;
        }

    }
}
