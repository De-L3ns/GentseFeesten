using System;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using GentseFeesten.Domain;
using GentseFeesten.Domain.Model;

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

            _evenementenWindow.EventSelected += EvenementenWindow_EvenementSelected;
            
        }

        private void EvenementenWindow_EvenementSelected(object? sender, EventArgs e)
        {
          
            _domainController.ShowAllChildsFromEvent(_evenementenWindow.IdOfSelectedEvent);
            _evenementenWindow.DescriptionBox.Text = _domainController.ShowEventDetails(_evenementenWindow.IdOfSelectedEvent);
        }
    }
}
