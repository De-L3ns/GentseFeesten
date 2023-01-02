using GentseFeesten.Domain;
using GentseFeesten.Domain.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GentseFeesten.Presentation
{
    public class GentseFeestenApplication
    {
        private readonly DomainController _domainController;
        private readonly EvenementenWindow _evenementenWindow;
        private readonly PlannerWindow _plannerWindow;

        public GentseFeestenApplication(DomainController domainController)
        {
            _domainController = domainController;
            _evenementenWindow = new EvenementenWindow();
            _plannerWindow = new PlannerWindow();

            _evenementenWindow.Show();
            _evenementenWindow.MainEvents = domainController.GetAllMainEvents();
            _evenementenWindow.EventSelected += EvenementenWindow_EvenementSelected;
            _evenementenWindow.GoToPlannerButtonClicked += EvenementenWindow_GoToPlanner;
            _evenementenWindow.AddEventToPlannerButtonClicked += EvenementenWindow_AddEventToPlanner;

        }

        private void EvenementenWindow_EvenementSelected(object? sender, Evenement e)
        {
            List<Evenement> childevents = _domainController.GetChildsFromEvent(e);
            _evenementenWindow.ChildEvents = childevents;
            _domainController.GetEventDetails(e);
            _evenementenWindow.TitleLabel.Content = e.ToString();
            _evenementenWindow.DescriptionBox.Text = _domainController.GetEventInformation(e);
            _evenementenWindow.AddEventToPlannerButton.IsEnabled = true;
            
            if (e is ChildEvenement)
            {
                _evenementenWindow.ReturnToPreviousButton.IsEnabled = true;
            }
            
            FillInNavigationBar(e);

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
            try
            {
                _domainController.AddEventToPlanner(e);
                MessageBox.Show($"{e.Name} werd aan uw planner toegevoegd");
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
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

        private void FillInNavigationBar(Evenement e)
        {
            Label navigationItem = new Label();

            if (e is MainEvenement)
            {
                _evenementenWindow.NavigationBar.Children.Clear();
                navigationItem.Content = e.Name;

            }
            else
            {
                navigationItem.Content = "-> " + e.Name;
            }

            _evenementenWindow.NavigationBar.Children.Add(navigationItem);
        }

    }
}
