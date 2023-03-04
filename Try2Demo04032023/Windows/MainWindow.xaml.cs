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
using Try2Demo04032023.Model;
using Try2Demo04032023.Services;
using Try2Demo04032023.Windows;

namespace Try2Demo04032023
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Product> Products { get; set; } = DataBaseManager.GetProducts();
        public List<ProductType> ProductTypes { get; set; } = DataBaseManager.GetProductTypes();

        public MainWindow()
        {
            InitializeComponent();

            ProductLV.ItemsSource = Products;
            
            foreach (var type in ProductTypes)
            {
                FiltherCB.Items.Add(type.Title);
            }

            RefreshButtons();
        }

        public int PageSize { get; set; } = 20;
        public int PageNumber { get; set; } = 0;
        private void RefreshPagination()
        {
            ProductLV.ItemsSource = null;
            ProductLV.ItemsSource = Products.Skip(PageNumber * PageSize).Take(PageSize).ToList();
        }

        private void NextPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Products.Count % PageSize == 0)
            {
                if (PageNumber == (Products.Count / PageSize) - 1)
                    return;
            }
            else
            {
                if (PageNumber == (Products.Count / PageSize))
                    return;
            }
            PageNumber++;
            RefreshPagination();
        }

        private void BackPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PageNumber == 0)
                return;
            PageNumber--;
            RefreshPagination();
        }

        private void RefreshButtons()
        {
            ButtonsWP.Children.Clear();
            if (Products.Count % PageSize == 0)
            {
                for (int i = 0; i < Products.Count / PageSize; i++)
                {
                    Button button = new Button();
                    button.Style = this.FindResource("MaterialDesignIconButton") as Style;
                    button.Content = i + 1;
                    button.Click += Button_Click;
                    ButtonsWP.Children.Add(button);
                }
            }
            else
            {
                for (int i = 0; i < Products.Count / PageSize + 1; i++)
                {
                    Button button = new Button();
                    button.Style = this.FindResource("MaterialDesignIconButton") as Style;
                    button.Content = i + 1;
                    button.Click += Button_Click;
                    ButtonsWP.Children.Add(button);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            PageNumber = Convert.ToInt32(button.Content) - 1;
            RefreshPagination();
        }

        private void CreateProductBtn_Click(object sender, RoutedEventArgs e)
        {
            new EditProductWindow().ShowDialog();

            Products = DataBaseManager.GetProducts();
            ProductLV.ItemsSource = Products;
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            var currentLst = Products;

            if (SearchTB.Text != string.Empty)
            {
                currentLst = Products.Where(z => (z.Title.Contains(SearchTB.Text))).ToList();
            }

            ProductLV.ItemsSource = currentLst;
        }

        private void NoneSortItm_Selected(object sender, RoutedEventArgs e)
        {
            if(ProductLV != null)
                ProductLV.ItemsSource = Products;
        }

        private void NameUpItm_Selected(object sender, RoutedEventArgs e)
        {
            ProductLV.ItemsSource = Products.OrderByDescending(x => x.Title);
        }

        private void NameDownItm_Selected(object sender, RoutedEventArgs e)
        {
            ProductLV.ItemsSource = Products.OrderBy(x => x.Title);
        }

        private void ManufactorUpItm_Selected(object sender, RoutedEventArgs e)
        {
            ProductLV.ItemsSource = Products.OrderByDescending(x => x.ProductionWorkshopNumber);
        }

        private void ManufactorDownItm_Selected(object sender, RoutedEventArgs e)
        {
            ProductLV.ItemsSource = Products.OrderBy(x => x.ProductionWorkshopNumber);
        }

        private void PriceUpItm_Selected(object sender, RoutedEventArgs e)
        {
            ProductLV.ItemsSource = Products.OrderByDescending(x => x.MinCostForAgent);
        }

        private void PriceDownItm_Selected(object sender, RoutedEventArgs e)
        {
            ProductLV.ItemsSource = Products.OrderBy(x => x.MinCostForAgent);
        }

        private void FiltherCB_DropDownClosed(object sender, EventArgs e)
        {
            var type = FiltherCB.SelectedItem as string;

            if (ProductLV == null && type != null)
                return;

            if(type == null)
            {
                ProductLV.ItemsSource = Products;
                return;
            }

            ProductLV.ItemsSource = Products.Where(x => x.ProductType.Title == type);
        }

        private void EditProductBtn_Click(object sender, object e)
        {
            var product = ProductLV.SelectedItem as Product;

            if (product == null)
            {
                MessageBox.Show("Выберите нужный продукт из списка!");
                return;
            }

            new EditProductWindow(product).ShowDialog();
            Products = DataBaseManager.GetProducts();
            ProductLV.ItemsSource = Products;
        }
    }
}
