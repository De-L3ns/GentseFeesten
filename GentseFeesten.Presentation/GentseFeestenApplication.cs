using System;
using GentseFeesten.Domain;

namespace GentseFeesten.Presentation
{
    public class GentseFeestenApplication
    {
        private readonly DomainController _domainController;
        private EvenementenWindow _evenementenWindow;

        public GentseFeestenApplication(DomainController domainController)
        {
            _domainController = domainController;
            // domainController.ShowAllParents();
            // domainController.ShowAllChildsFromEvent("3cf14c31-68fb-4636-8a47-e90cbe0188fa");
            _evenementenWindow= new EvenementenWindow();
            _evenementenWindow.Show();
            _evenementenWindow.MainEvents = domainController.GetAllMainEvents();
            
        }
    }
}
