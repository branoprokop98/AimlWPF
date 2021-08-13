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
    /// Interaction logic for NewCategory.xaml
    /// </summary>
    public partial class NewCategory : UserControl
    {
        public NewCategory()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new AimlContext())
            {
                Category category = new Category();
                if (!CategoryTextBox.Text.ToString().Equals(""))
                {
                    category.Name = CategoryTextBox.Text.ToString();
                    db.Categories.Add(category);
                    db.SaveChanges();
                    MessageBox.Show("Category has been added");
                    CategoryTextBox.Clear();
                }
                else
                {
                    MessageBox.Show("Name of category is empty");
                }
            }
        }
    }
}
