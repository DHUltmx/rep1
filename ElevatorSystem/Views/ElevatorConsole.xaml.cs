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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ElevatorSystem.UI.ViewModel;

namespace ElevatorSystem.UI.Views
{
    /// <summary>
    /// Interaction logic for ElevatorConsole.xaml
    /// </summary>
    public partial class ElevatorConsole : Window
    {
        public ElevatorConsoleViewmodel ViewModel { get; set; }

        public ElevatorConsole()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the SelectionChanged event of the Floor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void Floor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Floor.SelectedItem.ToString() == "Ground")
            {
                ViewModel.SelectedFloor = 0;
            }
            else
            {
                ViewModel.SelectedFloor = Convert.ToInt32(Floor.SelectedItem.ToString());
            }
        }        
    }
}
