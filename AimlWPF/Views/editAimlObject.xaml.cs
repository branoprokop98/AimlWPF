using AimlWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace AimlWPF.Views
{
    /// <summary>
    /// Interaction logic for editAimlObject.xaml
    /// </summary>
    public partial class editAimlObject : UserControl
    {
        public editAimlObject()
        {
            InitializeComponent();
        }


        private void loadDatabase(object sender, RoutedEventArgs e)
        {

            //List<Aiml> aimls = db.Aimls.ToList();
            //databaseView.ItemsSource = null;
            //databaseView.Items.Clear();
            //databaseView.Items.Refresh();
            //databaseView.ItemsSource = aimls;
            //databaseView.Columns[6].Visibility = Visibility.Hidden;
            getListOfAllAimls();

        }

        public void getListOfAllAimls()
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
                setDatabseView(aimls);
                //List<Aiml> aimls = new List<Aiml>();
                //foreach (var aiml in join)
                //{
                //    Aiml a = new Aiml
                //    {
                //        AimlId = aiml.AimlId,
                //        Content = aiml.Content,
                //        Pattern = aiml.Pattern,
                //        Template = aiml.Template,
                //        Mood = aiml.Mood,
                //        CategoryId = aiml.CategoryId,
                //        category = new Category { Name = aiml.CategoryName, CategoryId = aiml.CategoryId }
                //    };
                //    aimls.Add(a);
                //}
                //databaseView.ItemsSource = null;
                //databaseView.Items.Clear();
                //databaseView.Items.Refresh();
                //databaseView.ItemsSource = aimls;
                //databaseView.Columns[5].Visibility = Visibility.Hidden;
            }
        }

        //private void GetRow(object sender, SelectionChangedEventArgs e)
        //{
        //    Aiml aiml = databaseView.SelectedItem as Aiml;
        //    DataContext = new AddAimlObject();
        //}

        private void button_Clicked(object sender, RoutedEventArgs e)
        {
            Aiml aiml = databaseView.SelectedItem as Aiml;
            if (aiml != null)
            {
                TestWindow test = new TestWindow(aiml);
                test.Show();
            }
            //MainWindow.mainWindow.Hide();
        }

        private void findByPattern(object sender, TextChangedEventArgs e)
        {
            SearchBoxTemplate.Text = String.Empty;
            string searchText = SearchBoxPattern.Text.ToLower();
            findPattern(searchText);
        }

        private void findByTemplate(object sender, TextChangedEventArgs e)
        {
            SearchBoxPattern.Text = String.Empty;
            string searchText = SearchBoxTemplate.Text.ToLower();
            findTemplate(searchText);
        }

        private void findPattern(string searchText)
        {
            if (SearchBoxPattern.Text == string.Empty && SearchBoxTemplate.Text == string.Empty)
            {
                getListOfAllAimls();
                return;
            }
            using (var context = new AimlContext())
            {
                // Query for all blogs with names starting with B
                var join = context.Aimls.Join(context.Categories, aiml => aiml.CategoryId,
                    category => category.CategoryId,
                    (aiml, category) => new
                    {
                        AimlId = aiml.AimlId,
                        Content = aiml.Content,
                        Pattern = aiml.Pattern,
                        Template = aiml.Template,
                        Mood = aiml.Mood,
                        CategoryId = aiml.CategoryId,
                        CategoryName = category.Name
                    }).Where(aiml => aiml.Pattern.ToLower().Contains(searchText.ToLower())).ToList();


                List<Aiml> findedAimls = convertAnonymousObjectToAiml(join);
                setDatabseView(findedAimls);

                //databaseView.ItemsSource = null;
                //databaseView.Items.Clear();
                //databaseView.Items.Refresh();
                //databaseView.ItemsSource = findedAimls;
                //databaseView.Columns[5].Visibility = Visibility.Hidden;

                //var aimlPatterns = from b in context.Aimls
                //                   where b.Pattern.ToLower().Contains(searchText)
                //                   select b;

                //List<Aiml> aimls = aimlPatterns.ToList();
                //databaseView.ItemsSource = null;
                //databaseView.Items.Clear();
                //databaseView.Items.Refresh();
                //databaseView.ItemsSource = aimls;
            }
        }

        private void findTemplate(string searchText)
        {
            if (SearchBoxPattern.Text == string.Empty && SearchBoxTemplate.Text == string.Empty)
            {
                getListOfAllAimls();
                return;
            }
            using (var context = new AimlContext())
            {

                var join = context.Aimls.Join(context.Categories, aiml => aiml.CategoryId,
                    category => category.CategoryId,
                    (aiml, category) => new
                    {
                        AimlId = aiml.AimlId,
                        Content = aiml.Content,
                        Pattern = aiml.Pattern,
                        Template = aiml.Template,
                        Mood = aiml.Mood,
                        CategoryId = aiml.CategoryId,
                        CategoryName = category.Name
                    }).Where(aiml => aiml.Template.ToLower().Contains(searchText.ToLower())).ToList();


                List<Aiml> findedAimls = convertAnonymousObjectToAiml(join);
                setDatabseView(findedAimls);

                //databaseView.ItemsSource = null;
                //databaseView.Items.Clear();
                //databaseView.Items.Refresh();
                //databaseView.ItemsSource = findedAimls;
                //databaseView.Columns[5].Visibility = Visibility.Hidden;


                // Query for all blogs with names starting with B
                //var aimlPatterns = from b in context.Aimls
                //                   where b.Template.ToLower().Contains(searchText)
                //                   select b;

                //List<Aiml> aimls = aimlPatterns.ToList();
                //databaseView.ItemsSource = null;
                //databaseView.Items.Clear();
                //databaseView.Items.Refresh();
                //databaseView.ItemsSource = aimls;
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

        private void setDatabseView(List<Aiml> aimls)
        {
            databaseView.ItemsSource = null;
            databaseView.Items.Clear();
            databaseView.Items.Refresh();
            databaseView.ItemsSource = aimls;
            databaseView.Columns[5].Visibility = Visibility.Hidden;
            databaseView.Columns[1].Visibility = Visibility.Hidden;
        }
    }
}
