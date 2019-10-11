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
using EngineeringThesis.Core.Models;
using EngineeringThesis.Core.Services;
using EngineeringThesis.Core.ViewModel;
using NavigationService = System.Windows.Navigation.NavigationService;

namespace EngineeringThesis.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Navigation.NavigationService _navigationService;
        public MainViewModel MainViewModel;
        public MainWindow(Navigation.NavigationService navigationService, MainViewModel mainViewModel)
        {
            InitializeComponent();

            this._navigationService = navigationService;
            MainViewModel = mainViewModel;

            MainViewModel.GetInvoices();

            DataGrid.ItemsSource = MainViewModel.Invoices;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.Invoices[0].Contractor.Name = "Bartek Prokop";

            DataGrid.Items.Refresh();
        }
    }
}
