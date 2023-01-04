using GentseFeesten.Domain;
using GentseFeesten.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
            // Used to initialise the Date structure in DataTable for WPF.
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            
            try
            {
                // Initialisation
                _domainController = domainController;
                _evenementenWindow = new EvenementenWindow();
                _plannerWindow = new PlannerWindow();

                // Window Logic
                _evenementenWindow.Show();
                _evenementenWindow.MainEvents = domainController.GetAllMainEvents();
                _evenementenWindow.EventSelected += EvenementenWindow_EvenementSelected;
                _evenementenWindow.GoToPlannerButtonClicked += EvenementenWindow_GoToPlanner;
                _evenementenWindow.AddEventToPlannerButtonClicked += EvenementenWindow_AddEventToPlanner;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        // EvenementenWindow Methods
        private void EvenementenWindow_EvenementSelected(object? sender, Evenement e)
        {
            List<Evenement> childevents = _domainController.GetChildsFromEvent(e);
            _evenementenWindow.ChildEvents = childevents;
            _domainController.GetEventDetails(e);
            _evenementenWindow.TitleLabel.Content = e.ToString();
            _evenementenWindow.DescriptionBox.Text = _domainController.GetEventInformation(e);
            _evenementenWindow.AddEventToPlannerButton.IsEnabled = true;
            
            if (e is MainEvenement)
            {
                _evenementenWindow.ReturnToPreviousButton.IsEnabled = false;

            } else
            {
                _evenementenWindow.ReturnToPreviousButton.IsEnabled = true;
            }

            FillInNavigationBar();
        }

        private void EvenementenWindow_GoToPlanner(object? sender, EventArgs e)
        {
            _evenementenWindow.Hide();
            _plannerWindow.Show();
            RefreshPlannerWindowData();
            _plannerWindow.PlannerEventSelected += PlannerWindow_EventSelected;
            _plannerWindow.RemoveEventButtonClicked += PlannerWindow_RemoveEvent;
            _plannerWindow.ReturnToEvenementenButtonClicked += PlannerWindow_ReturnToEvents;
            _plannerWindow.WindowClosing += PlannerWindow_Closing;
        }

        private void EvenementenWindow_AddEventToPlanner(object? sender, Evenement e)
        {
            try
            {
                MessageBox.Show(_domainController.AddEventToPlanner(e), "Gelukt!");

            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // PlannerWindow Methods
        private void PlannerWindow_EventSelected(object? sender, EventArgs e)
        {
            _plannerWindow.RemoveEventButton.IsEnabled = true;
        }

        private void PlannerWindow_RemoveEvent(object? sender, Evenement e)
        {
            MessageBox.Show(_domainController.RemoveEventFromPlanner(e), "Gelukt!");
            RefreshPlannerWindowData();
        }

        private void PlannerWindow_ReturnToEvents(object? sender, EventArgs e)
        {
            _plannerWindow.Hide();
            _evenementenWindow.Show();
        }

        private void PlannerWindow_Closing(object? sender, CancelEventArgs e)
        {
            if (!_evenementenWindow.IsClosing)
            {
                e.Cancel = true;
                _plannerWindow.Hide();
                _evenementenWindow.Show();
            }
        }

        // Helper Methods
        private void RefreshPlannerWindowData()
        {
            _plannerWindow.PlannerEvents = _domainController.GetEventsFromPlanner();
            _plannerWindow.SummaryTextBox.Text = _domainController.GetPlannerSummary();
        }

        private void FillInNavigationBar()
        {
            _evenementenWindow.NavigationBar.Children.Clear();
            foreach (Evenement e in _evenementenWindow.EventsInTree)
            {
                Label l = new Label();
                if (e is MainEvenement)
                {
                    l.Content = e.Name;
                }
                else
                {
                    l.Content = "->  " + e.Name;
                }
                _evenementenWindow.NavigationBar.Children.Add(l);
            }
        }

    }
}
