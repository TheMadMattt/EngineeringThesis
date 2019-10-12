using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EngineeringThesis.Core.Models;
using EngineeringThesis.Core.Services;
using EngineeringThesis.UI.Navigation;
using EngineeringThesis.UI.ViewModel;
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
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var navigationService = ServiceProvider.GetRequiredService<NavigationService>();

            var mainWindow = navigationService.ShowAsync<View.MainWindow>();
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<ApplicationContext>();
            serviceCollection.AddScoped<NavigationService>();

            //Services
            serviceCollection.AddScoped(typeof(InvoiceService));

            //ViewModels
            serviceCollection.AddTransient(typeof(MainViewModel));

            //Views
            serviceCollection.AddTransient(typeof(View.MainWindow));
        }
    }
}
