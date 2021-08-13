using System;
using System.Collections.Generic;
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
    /// Interaction logic for DeleteCategoryView.xaml
    /// </summary>
    public partial class DeleteCategoryView : UserControl
    {
        private List<Category> categories;

        public DeleteCategoryView()
        {
            InitializeComponent();
        }

        private void initComboBox_OnLoad(object sender, RoutedEventArgs e)
        {
            using (var db = new AimlContext())
            {
                categories = db.Categories.ToList();

                categoryComboBox.ItemsSource = categories;
            }
        }

        private void deleteCategory_Clicked(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Category deletion", "Are you sure you want to delete this category?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                using (var db = new AimlContext())
                {
                    Category category = categories.Find(x => x.Name == categoryComboBox.SelectedItem.ToString());
                    db.Categories.Remove(category);
                    db.SaveChanges();
                    categories = db.Categories.ToList();
                    categoryComboBox.ItemsSource = categories;
                }
            }
        }
    }
}
