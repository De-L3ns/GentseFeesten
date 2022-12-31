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
        private PlannerWindow _plannerWindow;

        public GentseFeestenApplication(DomainController domainController)
        {
            _domainController = domainController;
            _evenementenWindow= new EvenementenWindow();
            _plannerWindow = new PlannerWindow();

            _evenementenWindow.Show();
            _evenementenWindow.MainEvents = domainController.GetAllMainEvents();
            _evenementenWindow.EventSelected += EvenementenWindow_EvenementSelected;
            _evenementenWindow.GoToPlannerButtonClicked += EvenementenWindow_GoToPlanner;
            _evenementenWindow.AddEventToPlannerButtonClicked += EvenementenWindow_AddEventToPlanner;
            _evenementenWindow.EventSelected += EvenementenWindow_PopulateTreeView;

        }

        private void EvenementenWindow_EvenementSelected(object? sender, Evenement e)
        {
            List<Evenement> childevents = _domainController.GetChildsFromEvent(e);
            _evenementenWindow.ChildEvents = childevents;
            _domainController.GetEventDetails(e);
            _evenementenWindow.DescriptionBox.Text = e.ToString();
            _evenementenWindow.AddEventToPlannerButton.IsEnabled = true;
        }

        private void EvenementenWindow_GoToPlanner(object? sender, EventArgs e)
        {
            _evenementenWindow.Hide();
            _plannerWindow.Show();
            RefreshPlannerWindowData();
            _plannerWindow.PlannerEventSelected += PlannerWindow_EventSelected;
            _plannerWindow.RemoveEventButtonClicked += PlannerWindow_RemoveEvent;
            _plannerWindow.ReturnToEvenementenButtonClicked += PlannerWindow_ReturnToEvents;
        }

        private void EvenementenWindow_AddEventToPlanner(object? sender, Evenement e)
        {
            _domainController.AddEventToPlanner(e);
            MessageBox.Show($"{e.Name} werd aan uw planner toegevoegd");
        }

        private void PlannerWindow_EventSelected(object? sender, EventArgs e)
        {
            _plannerWindow.RemoveEventButton.IsEnabled = true;
        }

        private void PlannerWindow_RemoveEvent(object? sender, Evenement e) 
        { 
            _domainController.RemoveEventFromPlanner(e);
            MessageBox.Show($"{e.Name} werd verwijderd van uw planner");
            RefreshPlannerWindowData();
        }

        private void PlannerWindow_ReturnToEvents(object? sender, EventArgs e)
        {
            _plannerWindow.Hide();
            _evenementenWindow.Show();
        }

        private void RefreshPlannerWindowData()
        {
            _plannerWindow.PlannerEvents = _domainController.GetEventsFromPlanner();
            _plannerWindow.SummaryTextBox.Text = _domainController.GetPlannerSummary();
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
