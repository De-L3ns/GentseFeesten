using System;
using System.Collections.Generic;
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
            _evenementenWindow.EventSelected += EvenementenWindow_PopulateTreeView;
            
        }

        private void EvenementenWindow_EvenementSelected(object? sender, EventArgs e)
        {

            _evenementenWindow.DescriptionBox.Text = _domainController.ShowEventDetails(_evenementenWindow.IdOfSelectedEvent);
        }

        private void EvenementenWindow_PopulateTreeView(object? sender, EventArgs e) {
            _evenementenWindow.EventTreeView.Items.Clear();
            
            List<Evenement> childsToDisplay = _domainController.GetChildsFromEvent(_evenementenWindow.IdOfSelectedEvent);
            TreeViewItem treeItem = new TreeViewItem();
            treeItem.Header = _evenementenWindow.NameOfSelectedEvent;

            foreach (Evenement child in childsToDisplay)
            {
                treeItem.Items.Add(child.Naam);
                if (_domainController.GetChildsFromEvent(child.Id).Count > 0)
                {
                    TreeViewItem treeItem2 = new TreeViewItem();
                    foreach(Evenement c in _domainController.GetChildsFromEvent(child.Id))
                    {
                        treeItem2.Items.Add(child.Naam);
                    }

                    treeItem.Items.Add(treeItem2);
                }
            }

            _evenementenWindow.EventTreeView.Items.Add(treeItem);
            

        }
    }
}
