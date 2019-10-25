using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using EngineeringThesis.Core.Models;
using EngineeringThesis.Core.Services;
using EngineeringThesis.Core.ViewModel;
using EngineeringThesis.UI.Navigation;
using EngineeringThesis.UI.View;
using Microsoft.Extensions.DependencyInjection;

namespace EngineeringThesis.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            InitializeCultures();
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var navigationService = ServiceProvider.GetRequiredService<NavigationService>();

            var mainWindow = navigationService.ShowAsync<MainWindow>();
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<ApplicationContext>();
            serviceCollection.AddScoped<NavigationService>();

            //Services
            serviceCollection.AddScoped(typeof(InvoiceService));
            serviceCollection.AddScoped(typeof(CustomerService));
            serviceCollection.AddScoped(typeof(PaymentTypeService));
            serviceCollection.AddScoped(typeof(InvoiceItemService));

            //ViewModels
            serviceCollection.AddTransient(typeof(MainViewModel));
            serviceCollection.AddTransient(typeof(InvoiceViewModel));
            serviceCollection.AddTransient(typeof(InvoiceItemViewModel));
            serviceCollection.AddTransient(typeof(AddCustomerViewModel));

            //Views
            serviceCollection.AddTransient(typeof(MainWindow));
            serviceCollection.AddTransient(typeof(InvoiceWindow));
            serviceCollection.AddTransient(typeof(InvoiceItemWindow));
            serviceCollection.AddTransient(typeof(AddCustomerWindow));
        }

        private void InitializeCultures()
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pl-PL");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pl-PL");
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage("pl-PL")));
        }
    }
}
