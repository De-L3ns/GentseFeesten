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
            IEvenementenRepository evenementenRepository = new EvenementenMapper();
            IPlannerRepository plannerRepository = new PlannerMapper();
            DomainController domainController = new DomainController(evenementenRepository, plannerRepository);
            GentseFeestenApplication application = new GentseFeestenApplication(domainController);
        }
    }
}
