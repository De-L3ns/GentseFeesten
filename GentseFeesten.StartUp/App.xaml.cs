using GentseFeesten.Domain;
using GentseFeesten.Domain.Repository;
using GentseFeesten.Persistence;
using GentseFeesten.Presentation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GentseFeesten.StartUp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            IEvenementenRepository repository = new EvenementenMapper();
            DomainController domainController = new DomainController(repository);
            GentseFeestenApplication application = new GentseFeestenApplication(domainController);
        }
    }
}
