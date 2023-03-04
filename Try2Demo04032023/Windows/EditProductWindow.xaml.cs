using Microsoft.Win32;
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
using System.Windows.Shapes;
using Try2Demo04032023.Model;
using Try2Demo04032023.Services;

namespace Try2Demo04032023.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        public Product Product { get; set; }
        public List<ProductType> ProductTypes { get; set; } = DataBaseManager.GetProductTypes();
        public EditProductWindow()
        {
            InitializeComponent();

            Product = new Product();
            ProductSP.DataContext = Product;
            TypeCB.ItemsSource = ProductTypes;
            _isNew = true;
        }

        public EditProductWindow(Product product)
        {
            InitializeComponent();

            Product = product;
            ProductSP.DataContext = Product;
            TypeCB.ItemsSource = ProductTypes;
            _isNew = false;
        }

        private bool _isNew;

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if( IdTB.Text == string.Empty || NameTB.Text == string.Empty || TypeCB.SelectedItem == null || 
                CountPersonTB.Text == string.Empty || ManufactorTB.Text == string.Empty || PriceTB.Text == string.Empty)
            {
                MessageBox.Show("Все поля должны быть заполнены!");
                return;
            }

            if(!_priceCheck)
            {
                MessageBox.Show("Некорректные данные!");
                return;
            }

            if(_isNew)
            {
                try
                {
                    DataBaseManager.AddProduct(Product);
                }
                catch
                {
                    MessageBox.Show("Проверьте заполненные данные!");
                    return;
                }
                MessageBox.Show("Товар успешно добавлен!");
                this.Close();
            }
            else
            {
                this.Close();
            }

        }

        bool _priceCheck;
        private void PriceTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            try 
            {
                Convert.ToDecimal(PriceTB.Text.Replace('.', ','));
                _priceCheck = true;
                PriceTB.BorderBrush = Brushes.Black;
            }
            catch 
            {
                _priceCheck = false;
                PriceTB.BorderBrush = Brushes.Red;
            }
        }

        private void LoadImageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            DataBaseManager.Try2SaveChanges();
        }
    }
}
