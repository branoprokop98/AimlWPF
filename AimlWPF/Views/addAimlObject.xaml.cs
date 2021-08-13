using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using AimlWPF.Models;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace AimlWPF.Views
{
    /// <summary>
    /// Interaction logic for addAimlObject.xaml
    /// </summary>
    public partial class addAimlObject : UserControl
    {
        private Aiml aiml;
        public addAimlObject()
        {
            InitializeComponent();
            aiml = new Aiml();
        }

        private void setListBox_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new AimlContext())
            {
                List<Category> categories = db.Categories.ToList();

                categoryComboBox.ItemsSource = categories;
                categoryComboBox.SelectedIndex = 0;

            }

            setupContentBoxOnLoad();
        }

        private void setupContentBoxOnLoad()
        {

            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.DocumentElement;

            //(2) string.Empty makes cleaner code
            XmlElement element1 = xml.CreateElement(string.Empty, "category", string.Empty);
            xml.AppendChild(element1);

            XmlElement element2 = xml.CreateElement(string.Empty, "pattern", string.Empty);
            element1.AppendChild(element2);

            XmlElement element3 = xml.CreateElement(string.Empty, "template", string.Empty);
            element1.AppendChild(element3);


            ContentTextBox.Text = xml.OuterXml;
        }

        private void updatePatternInContent(object sender, TextChangedEventArgs e)
        {
            aiml.Pattern = PatternBox.Text;
            updateContentFromSentenceAndAnswerTextBox();
            //updateDatabase(aiml);

        }

        private void updateTemplateInContent(object sender, TextChangedEventArgs e)
        {
            aiml.Template = TemplateBox.Text;
            updateContentFromSentenceAndAnswerTextBox();
            //updateDatabase(aiml);
        }

        private void content_Changed(object sender, TextChangedEventArgs e)
        {
            //position = ContentTextBox.SelectionStart;
            aiml.Content = ContentTextBox.Text;
            updateAnswerAndSentenceTextBoxFromContentTextBox();
            //ContentTextBox.SelectionStart = position;
            //updateDatabase(aiml);
        }

        public void updateContentFromSentenceAndAnswerTextBox()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("category"));
            el.AppendChild(doc.CreateElement("pattern")).InnerText = aiml.Pattern;
            el.AppendChild(doc.CreateElement("template")).InnerText = aiml.Template;
            XDocument xml = XDocument.Parse(doc.OuterXml);
            aiml.Content = xml.ToString();
            string content = HttpUtility.HtmlDecode(aiml.Content);
            int position = ContentTextBox.SelectionStart;
            ContentTextBox.Text = String.Empty;
            ContentTextBox.Text = content;
            ContentTextBox.SelectionStart = position;
            PatternBox.Text = aiml.Pattern;
            TemplateBox.Text = aiml.Template;
        }

        private int pos;
        public void updateAnswerAndSentenceTextBoxFromContentTextBox()
        {
            try
            {
                XmlDocument xmltest = new XmlDocument();
                xmltest.LoadXml(aiml.Content);
                PatternBox.Text = xmltest.GetElementsByTagName("pattern")[0].InnerXml;
                TemplateBox.Text = xmltest.GetElementsByTagName("template")[0].InnerXml;
                pos = ContentTextBox.SelectionStart;
                ContentTextBox.Select(pos, 0);
                //ContentTextBox.Text = aiml.Content;
            }
            catch (Exception)
            {
                return;
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                checkInput();
                readMood();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return;
            }

            saveNewAimlToDatabase();
            PatternBox.Text = string.Empty;
            TemplateBox.Text = string.Empty;
            MoodTextBox.Text = string.Empty;
            MessageBox.Show("Uložene");
        }

        private void checkInput()
        {
            if (categoryComboBox.SelectionBoxItem.Equals(""))
            {
                throw new Exception("Category is not selected");
            }

            if (PatternBox.Text.Equals(""))
            {
                throw new Exception("Sentence is not specified");
            }

            if (TemplateBox.Text.Equals(""))
            {
                throw new Exception("Answer is not specified");
            }
        }

        private void readMood()
        {
            try
            {
                XDocument doc = XDocument.Parse(ContentTextBox.Text.ToString());
                IEnumerable<XElement> xml = doc.Descendants("template");
                var test = xml.Where(x => x.Element("set").HasAttributes.Equals("name"));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void saveNewAimlToDatabase()
        {
            using (var db = new AimlContext())
            {
                int mood;
                List<Category> catogories = db.Categories.Where(category => category.Name.Equals(categoryComboBox.SelectedValue.ToString())).ToList();
                try
                {
                    mood = int.Parse(MoodTextBox.Text);
                }
                catch (Exception exception)
                {
                    mood = 0;
                }

                Aiml aiml = new Aiml
                {
                    Content = ContentTextBox.Text,
                    Pattern = PatternBox.Text,
                    Template = TemplateBox.Text,
                    Mood = mood,
                    CategoryId = catogories[0].CategoryId
                };

                db.Aimls.Add(aiml);
                db.SaveChanges();
            }
        }

        private void mood_TextChanged(object sender, TextChangedEventArgs e)
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
                        aimlMoodRead(aimlChildNode, contentDocument);
                        ContentTextBox.Text = contentDocument.ToString();
                        break;
                }
            }
        }

        private void aimlMoodRead(XElement aimlTemplate, XDocument contentDocument)
        {

            IEnumerable<XElement> aimlChildNodes = aimlTemplate.Descendants("think");
            IEnumerable<XElement> aimlObjects = from aimlObject in aimlTemplate.Descendants("think") select aimlObject;

            if (MoodTextBox.Text.Equals(""))
            {
                try
                {
                    aimlTemplate.Element("think").Elements("set").Where(x => x.Attribute("name").Value.Equals("mood")).Remove();
                    if (aimlTemplate.Element("think").IsEmpty)
                    {
                        aimlTemplate.Element("think").Remove();
                    }
                }
                catch (Exception e)
                {
                    return;
                }
                return;
            }

            foreach (XElement aimlChildNode in aimlChildNodes)
            {
                getMood(aimlChildNode);
                return;
            }

            if (!MoodTextBox.Text.ToString().Equals("-"))
            {
                aimlTemplate.Add(new XElement("think", new XElement("set", new XAttribute("name", "mood"), MoodTextBox.Text.ToString())));
                aiml.Mood = Int32.Parse(MoodTextBox.Text.ToString());
            }
            else
            {
                aimlTemplate.Add(new XElement("think", new XElement("set", new XAttribute("name", "mood"), "0")));
                aiml.Mood = 0;
            }
        }

        private void getMood(XElement aimlChildNode)
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
                            if (!MoodTextBox.Text.ToString().Equals("-"))
                            {
                                childNode.Value = MoodTextBox.Text.ToString();
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
