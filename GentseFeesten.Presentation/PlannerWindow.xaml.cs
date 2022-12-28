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
    /// <summary>
    /// Interaction logic for PlannerWindows.xaml
    /// </summary>
    public partial class PlannerWindow : Window
    {
        private List<Evenement> _plannerEvents;
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
    }
}
