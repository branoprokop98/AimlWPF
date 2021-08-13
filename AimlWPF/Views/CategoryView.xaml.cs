using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace AimlWPF.Views
{
    /// <summary>
    /// Interaction logic for CategoryView.xaml
    /// </summary>
    public partial class CategoryView : Window
    {
        private List<Aiml> aimls;
        private List<Category> categories;

        public CategoryView()
        {
            InitializeComponent();
        }

        private void initCategoriesComboList(object sender, RoutedEventArgs e)
        {
            getDatabase();
            CategoryComboBox.ItemsSource = categories;
        }

        private void getDatabase()
        {
            using (var db = new AimlContext())
            {
                var join = db.Aimls.Join(db.Categories, aiml => aiml.CategoryId, category => category.CategoryId,
                    (aiml, category) => new
                    {
                        AimlId = aiml.AimlId,
                        Content = aiml.Content,
                        Pattern = aiml.Pattern,
                        Template = aiml.Template,
                        Mood = aiml.Mood,
                        CategoryId = aiml.CategoryId,
                        CategoryName = category.Name
                    }).ToList();
                aimls = convertAnonymousObjectToAiml(join);
                categories = db.Categories.ToList();
            }
        }

        private List<Aiml> convertAnonymousObjectToAiml(IEnumerable<dynamic> join)
        {
            List<Aiml> findedAimls = new List<Aiml>();
            foreach (var finded in join)
            {
                Aiml a = new Aiml
                {
                    AimlId = finded.AimlId,
                    Content = finded.Content,
                    Pattern = finded.Pattern,
                    Template = finded.Template,
                    Mood = finded.Mood,
                    CategoryId = finded.CategoryId,
                    category = new Category { Name = finded.CategoryName, CategoryId = finded.CategoryId }
                };
                findedAimls.Add(a);
            }

            return findedAimls;
        }

        private void saveToFile()
        {
            List<Aiml> aiml = new List<Aiml>();
            string folderLocation = pickFolder();
            List<Aiml> tempAimls = aimls.Where(aiml => aiml.category.Name.Equals(CategoryComboBox.SelectedValue.ToString())).ToList();
            Task task = ExampleAsync(tempAimls, CategoryComboBox.SelectedValue.ToString(), folderLocation);
        }

        public static async Task ExampleAsync(List<Aiml> contentList, string name, string folderLocation)
        {
            using StreamWriter file = new(folderLocation + "\\" + name + ".aiml", append: true);
            await file.WriteLineAsync("<?xml version = \"1.0\" encoding = \"ISO-8859-1\" ?>");
            await file.WriteLineAsync("<aiml>");
            foreach (Aiml aiml in contentList)
            {
                await file.WriteLineAsync(aiml.Content);
            }
            await file.WriteLineAsync("</aiml>");
        }

        private string pickFolder()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.ShowDialog();
            return dialog.FileName;
        }

        private void exportToFile(object sender, RoutedEventArgs e)
        {
            if (!CategoryComboBox.SelectionBoxItem.Equals(""))
            {
                saveToFile();
            }
            else
            {
                MessageBox.Show("No category selected");
            }
        }
    }
}
