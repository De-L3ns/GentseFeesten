using GentseFeesten.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
    
    public partial class PlannerWindow : Window
    {
        public event EventHandler<Evenement> RemoveEventButtonClicked;
        public event EventHandler PlannerEventSelected;
        public event EventHandler ReturnToEvenementenButtonClicked;
        private List<Evenement> _plannerEvents;
        public Evenement SelectedPlannerEvent { get; private set; }
        public List<Evenement> PlannerEvents
        {
            get => _plannerEvents;
            set
            {
                _plannerEvents = value;
                PlannerGrid.ItemsSource = _plannerEvents;
            }
        }
        public PlannerWindow()
        {
            InitializeComponent();
        }

        private void PlannerGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (PlannerGrid.SelectedItem != null)
            {
                Evenement evenement = (Evenement)PlannerGrid.SelectedItem;
                SelectedPlannerEvent = evenement;
                PlannerEventSelected?.Invoke(this, EventArgs.Empty);
            }
        }

        private void DeleteEventButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveEventButtonClicked?.Invoke(this, SelectedPlannerEvent);
        }

        private void ReturnToEventsButton_Click(object sender, RoutedEventArgs e)
        {
            ReturnToEvenementenButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        

        
    }
}
