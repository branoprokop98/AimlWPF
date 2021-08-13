using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AimlWPF.ViewModels
{
    public class AimlParser
    {
        private XDocument aimlFile;
        private List<Aiml> listOfAimls;
        private Aiml aiml;
        private Category category;
        private string nameOfFile;
        public bool containCategory { get; set; }

        public AimlParser(string pathToFile)
        {
            this.nameOfFile = pathToFile;
            aimlFile = XDocument.Load(pathToFile);
            listOfAimls = new List<Aiml>();
            category = new Category();
            category.Name = Path.GetFileNameWithoutExtension(pathToFile);
            saveCategory();
            getCategoryId();
        }

        private void saveCategory()
        {
            using (var db = new AimlContext())
            {
                List<Category> categories = db.Categories.Where(category => category.Name.Equals(this.category.Name)).ToList();
                if (categories.Count == 0)
                {
                    db.Categories.Add(category);
                    db.SaveChanges();
                    containCategory = false;
                }
                else
                {
                    containCategory = true;
                }
            }
        }

        private void getCategoryId()
        {
            using (var db = new AimlContext())
            {
                List<Category> category = db.Categories.Where(category => category.Name.Equals(this.category.Name)).ToList();
                this.category.CategoryId =  category[0].CategoryId;
            }
        }

        public void aimlObjectRead()
        {
            IEnumerable<XElement> aimlObjects = from aimlObject in aimlFile.Descendants("category") select aimlObject;
            int id = 55;
            foreach (XElement aimlObject in aimlObjects)
            {
                aiml = new Aiml();
                aiml.Content = aimlObject.ToString().Replace("'", "''");
                aimlCategoryRead(aimlObject);
                aiml.CategoryId = this.category.CategoryId;
                addToList(aiml);
                Console.WriteLine(aiml);
            }
            addAimlFileToDatabase();
        }

        private void addToList(Aiml aiml)
        {
            listOfAimls.Add(aiml);
        }

        private void aimlCategoryRead(XElement aimlObject)
        {
            IEnumerable<XElement> aimlChildNodes = aimlObject.Elements();

            foreach (XElement aimlChildNode in aimlChildNodes)
            {
                string tmp;
                switch (aimlChildNode.Name.ToString())
                {
                    case "pattern":
                        tmp = aimlChildNode.ToString().Replace("<pattern>", "");
                        aiml.Pattern = tmp.Replace("</pattern>", "");
                        aiml.Pattern = aiml.Pattern.Replace("'", "''");
                        break;
                    case "template":
                        tmp = aimlChildNode.ToString().Replace("<template>", "");
                        aiml.Template = tmp.Replace("</template>", "");
                        aiml.Template = aiml.Template.Replace("'", "''");
                        aimlMoodRead(aimlChildNode);
                        break;
                }
            }
        }

        private void aimlMoodRead(XElement aimlMood)
        {
            IEnumerable<XElement> aimlChildNodes = aimlMood.Descendants("think");
            IEnumerable<XElement> aimlObjects = from aimlObject in aimlFile.Descendants("think") select aimlObject;

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
                            aiml.Mood = Int32.Parse(childNode.Value);
                            break;
                    }
                }
            }
        }

        private void addAimlFileToDatabase()
        {
            using (var db = new AimlContext())
            {
                //db.Categories.Add(category);
                //db.SaveChanges();

                db.Aimls.AddRange(listOfAimls);
                db.SaveChanges();
            }
        }

    }
}
