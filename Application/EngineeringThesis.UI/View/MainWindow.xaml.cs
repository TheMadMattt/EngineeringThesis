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

namespace EngineeringThesis.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly InvoiceService InvoiceService;
        public List<Invoice> Invoices;
        public MainWindow()
        {
            InitializeComponent();

            InvoiceService = new InvoiceService();

            Invoices = InvoiceService.TestAdd();

            DataGrid.ItemsSource = Invoices;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Invoices[0].Contractor.Name = "Bartek Prokop";

            DataGrid.Items.Refresh();
        }
    }
}
