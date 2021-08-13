using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using AimlWPF.ViewModels;

namespace AimlWPF.Views
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        private Aiml aiml;
        private List<Category> categories;
        private ReadMood readMood;
        private WriteMood writeMood;

        public TestWindow(Aiml aiml)
        {
            this.aiml = aiml;
            InitializeComponent();
            readMood = new ReadMood(editContent, MoodTextBox, aiml);

        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            editSentence.Text = aiml.Pattern;
            editAnswer.Text = aiml.Template;
            editContent.Text = aiml.Content;

            XDocument contentDocument = XDocument.Parse(editContent.Text);
            IEnumerable<XElement> aimlObjects = from aimlObject in contentDocument.Descendants("category") select aimlObject;
            IEnumerable<XElement> aimlChildNodes = aimlObjects.Elements();
            readMood.aimlObjectRead();
            setCategoriesComboBox();
        }


        private void setCategoriesComboBox()
        {
            using (var db = new AimlContext())
            {
                categories = db.Categories.ToList();

                categoryComboBox.ItemsSource = categories;
                int index = categories.FindIndex(x => x.Name == aiml.category.Name);
                categoryComboBox.SelectedIndex = index;

            }
        }

        private void sentence_Changed(object sender, TextChangedEventArgs e)
        {
            aiml.Pattern = editSentence.Text;
            updateContentFromSentenceAndAnswerTextBox();
            //updateDatabase(aiml);
        }

        private void answer_Changed(object sender, TextChangedEventArgs e)
        {
            aiml.Template = editAnswer.Text;
            updateContentFromSentenceAndAnswerTextBox();
            //updateDatabase(aiml);
        }

        private void content_Changed(object sender, TextChangedEventArgs e)
        {
            aiml.Content = editContent.Text;
            updateAnswerAndSentenceTextBoxFromContentTextBox();
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
            int position = editContent.SelectionStart;
            editContent.Text = String.Empty;
            editContent.Text = content;
            editContent.SelectionStart = position;
            editSentence.Text = aiml.Pattern;
            editAnswer.Text = aiml.Template;
            //updateDatabase(aiml);
        }

        public void updateDatabase()
        {
            using (var db = new AimlContext())
            {
                db.Update(aiml);
                db.SaveChanges();
            }
        }

        public void updateAnswerAndSentenceTextBoxFromContentTextBox()
        {
            try
            {
                XmlDocument xmltest = new XmlDocument();
                xmltest.LoadXml(aiml.Content);
                editSentence.Text = xmltest.GetElementsByTagName("pattern")[0].InnerXml;
                editAnswer.Text = xmltest.GetElementsByTagName("template")[0].InnerXml;
                editContent.Text = aiml.Content;
            }
            catch (Exception)
            {
                return;
            }
        }

        private void sentence_Changed(object sender, TextCompositionEventArgs e)
        {
            aiml.Pattern = editSentence.Text;
            updateContentFromSentenceAndAnswerTextBox();
            //updateDatabase(aiml);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                XDocument.Parse(editContent.Text.ToString());
                checkInput();
                string value = categoryComboBox.SelectedValue.ToString();
                Category category = categories.Find(x => x.Name == value);
                aiml.category = category;
                aiml.CategoryId = category.CategoryId;
                updateDatabase();
                this.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return;
            }

            //MainWindow.mainWindow.Show();
            //this.Close();
        }

        private void checkInput()
        {
            if (editSentence.Text.Equals(""))
            {
                throw new Exception("Veta nie je zadaná");
            }

            if (editAnswer.Text.Equals(""))
            {
                throw new Exception("Odpoveď nie je zadaná");
            }
        }

        private void setMood_TextChanged(object sender, TextChangedEventArgs e)
        {
            writeMood = new WriteMood(editContent, MoodTextBox, aiml);
            writeMood.readAiml();
        }

        private void deleteObject_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Zmazanie objektu", "Naozaj chcete zmazať tento objekt?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                using (var db = new AimlContext())
                {
                    db.Aimls.Remove(aiml);
                    db.SaveChanges();
                    this.Close();
                }
            }
        }
    }
}
