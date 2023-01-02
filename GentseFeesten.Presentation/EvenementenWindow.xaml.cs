using GentseFeesten.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GentseFeesten.Presentation
{
    public partial class EvenementenWindow : Window
    {
        public event EventHandler<Evenement> EventSelected;
        public event EventHandler GoToPlannerButtonClicked;
        public event EventHandler<Evenement> AddEventToPlannerButtonClicked;
        private List<Evenement> _mainEvents;
        private List<Evenement> _childEvents;

        public EvenementenWindow()
        {
            InitializeComponent();
        }


        public Evenement SelectedEvenement { get; set; }
        public Evenement PreviouslySelectedEvenement { get; set; }

        public List<Evenement> MainEvents
        {
            get => _mainEvents;
            set
            {
                _mainEvents = value;
                MainEventGrid.ItemsSource = _mainEvents;
            }
        }

        public List<Evenement> ChildEvents
        {
            get => _childEvents;
            set
            {
                _childEvents = value;
                ChildEventGrid.ItemsSource = _childEvents;
            }
        }

        // Methods
        private void MainEventGrid_SelectedCellsChanged(object? sender, SelectedCellsChangedEventArgs e)
        {
            SelectedCellsChangedHelper(MainEventGrid);
        }

        private void ChildEventGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            SelectedCellsChangedHelper(ChildEventGrid);
        }
        private void SearchMain_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchBoxHelper(sender, MainEvents, MainEventGrid);
        }
        private void SearchChild_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchBoxHelper(sender, ChildEvents, ChildEventGrid);
        }

        private void GoToPlannerButton_Click(object sender, RoutedEventArgs e)
        {
            GoToPlannerButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void AddEventToPlannerButton_Click(object sender, RoutedEventArgs e)
        {
            AddEventToPlannerButtonClicked?.Invoke(this, SelectedEvenement);
        }

        private void ReturnToPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            EventSelected?.Invoke(this, PreviouslySelectedEvenement);
        }

        // Helper Methods

        private void SelectedCellsChangedHelper(DataGrid grid)
        {
            if (grid.SelectedItem != null)
            {
                PreviouslySelectedEvenement = SelectedEvenement;
                Evenement evenement = (Evenement)grid.SelectedItem;
                SelectedEvenement = evenement;
                EventSelected?.Invoke(this, evenement);
            }
        }

        private void SearchBoxHelper(object sender, List<Evenement> gridSource, DataGrid grid)
        {
            var mainTextSearch = sender as TextBox;
            if (mainTextSearch.Text != null)
            {
                var filteredNames = gridSource.Where(n => n.Name.ToLower().Contains(mainTextSearch.Text.ToLower()));
                grid.ItemsSource = filteredNames;
            }
        }

        
    }
}
