using GentseFeesten.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for EvenementenWindow.xaml
    /// </summary>
    public partial class EvenementenWindow : Window
    {
        public event EventHandler EventSelected;
        private List<Evenement> _mainEvents;
        public List<Evenement> MainEvents {
            get => _mainEvents;
            set {

                _mainEvents = value;
                MainEventGrid.ItemsSource = _mainEvents;

            }
        }
        public EvenementenWindow()
        {
            
            InitializeComponent();

        }

        private void MainEventGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Evenement evenement = (Evenement)MainEventGrid.SelectedItem;
            string id = evenement.Id;
            

        }
    }
}
