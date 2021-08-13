using AimlWPF.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AimlWPF.Views;
using CategoryView = AimlWPF.Views.CategoryView;
//using Test = AimlWPF.ViewModels.Test;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace AimlWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowVM mainWindow;

        public MainWindow()
        {
            mainWindow = new MainWindowVM();
            InitializeComponent();
            //mainWindow = this;
        }

        private void addAiml_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new AddAimlObject();
        }

        private void edit_Aiml_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new EditAimlObject();
        }

        //private void test_Click(object sender, RoutedEventArgs e)
        //{
        //    DataContext = new Test();
        //}

        private void addCategory_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new NewCategory();
        }

        private void deleteCategory_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new DeleteCategoryView();
        }

        private void exportAllToAimlFiles(object sender, RoutedEventArgs e)
        {
            getDatabase();
        }

        private void exportCategoryToAimlFile(object sender, RoutedEventArgs e)
        {
            CategoryView categoryView = new CategoryView();
            categoryView.Show();
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
                List<Aiml> aimls = convertAnonymousObjectToAiml(join);
                List<Category> categories = db.Categories.ToList();
                saveToFile(aimls, categories);
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

        private void saveToFile(List<Aiml> aimls, List<Category> categories)
        {
            List<Aiml> aiml = new List<Aiml>();
            string folderLocation = pickFolder();
            if (folderLocation == null)
            {
                return;
            }
            foreach (Category category in categories)
            {
                List<Aiml> tempAimls = aimls.Where(aiml => aiml.category.Name.Equals(category.Name)).ToList();
                Task task = ExampleAsync(tempAimls, category.Name.ToString(), folderLocation);
            }
        }

        private string pickFolder()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.ShowDialog();
            try
            {
                return dialog.FileName;
            }
            catch (Exception e)
            {
                return null;
            }
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

        private void importFromFile(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("Aiml", "*.aiml"));
            dialog.ShowDialog();
            try
            {
                mainWindow.readAimlFile(dialog.FileName);
            }
            catch (Exception exception)
            {
                return;
            }
        }
    }
}
