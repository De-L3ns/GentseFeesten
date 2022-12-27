using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
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
            _evenementenWindow= new EvenementenWindow();
            _evenementenWindow.Show();
            _evenementenWindow.MainEvents = domainController.GetAllMainEvents();
            _evenementenWindow.EventSelected += EvenementenWindow_EvenementSelected;
            _evenementenWindow.EventSelected += EvenementenWindow_PopulateTreeView;
            
            
        }

        private void EvenementenWindow_EvenementSelected(object? sender, Evenement e)
        {
            List<Evenement> childevents = _domainController.GetChildsFromEvent(_evenementenWindow.IdOfSelectedEvent);
            _evenementenWindow.ChildEvents = childevents;
            _evenementenWindow.DescriptionBox.Text = _domainController.GetEventDetails(e.Id);
        }

        private void EvenementenWindow_PopulateTreeView(object? sender, Evenement e)
        {
            if (e is MainEvenement)
            {
                _evenementenWindow.EventTreeView.Items.Clear();
                TreeViewItem mainEvent = new TreeViewItem();
                mainEvent.Header = _evenementenWindow.NameOfSelectedEvent;
                _evenementenWindow.EventTreeView.Items.Add(mainEvent);
            } else
            {
                TreeViewItem childEvent = new TreeViewItem();
                childEvent.Header = _evenementenWindow.NameOfSelectedEvent;
                TreeViewItem mainEvent = (TreeViewItem)_evenementenWindow.EventTreeView.Items[0];
                mainEvent.Items.Add(childEvent);
            }
            
        }
    }
}
