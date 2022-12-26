using GentseFeesten.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GentseFeesten.Presentation
{
    public partial class EvenementenWindow : Window
    {
        public event EventHandler EventSelected;
        private List<Evenement> _mainEvents;
        private List<Evenement> _childEvents;

        public EvenementenWindow()
        {
            InitializeComponent();
        }

        public string IdOfSelectedEvent { get; set; }
        public string NameOfSelectedEvent { get; set; }
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

        // Helper Methods

        private void SelectedCellsChangedHelper(DataGrid grid)
        {
            if (grid.SelectedItem != null)
            {
                Evenement evenement = (Evenement)grid.SelectedItem;
                IdOfSelectedEvent = evenement.Id;
                NameOfSelectedEvent = evenement.Naam;

                EventSelected?.Invoke(this, EventArgs.Empty);
            }
        }

        private void SearchBoxHelper(object sender, List<Evenement> gridSource, DataGrid grid )
        {
            var mainTextSearch = sender as TextBox;
            if (mainTextSearch.Text != null)
            {
                var filteredNames = gridSource.Where(n => n.Naam.ToLower().Contains(mainTextSearch.Text.ToLower()));
                grid.ItemsSource = filteredNames;
            }
        }
    }
}
