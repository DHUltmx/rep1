using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ElevatorSystem.Domain.Entitites;
using System.Windows.Input;
using System.Threading;
using System.Windows.Threading;

namespace ElevatorSystem.UI.ViewModel
{
    /// <summary>
    /// Elevator console viewmodel.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class ElevatorConsoleViewmodel : INotifyPropertyChanged
    {
        private Elevator _elevator;
        private ObservableCollection<int> _calledFloor;
        private int _floorDisplay;

        /// <summary>
        /// The elevator.
        /// </summary>
        /// <value>
        /// The elevator.
        /// </value>
        public Elevator Elevator
        {
            get { return _elevator; }
            set
            {
                _elevator = value;
                OnPropertyChanged("Elevator");
            }
        }

        /// <summary>
        /// The floor display bound to UI.
        /// </summary>
        /// <value>
        /// The floor display.
        /// </value>
        public int FloorDisplay
        {
            get
            {                
                return _floorDisplay;
            }
            set
            {
                _floorDisplay = value;
                OnPropertyChanged("FloorDisplay");
            }
        }

        /// <summary>
        /// The possible floors to be selected.
        /// </summary>
        /// <value>
        /// The called floor.
        /// </value>
        public ObservableCollection<int> CalledFloor
        {
            get { return _calledFloor; }
            set
            {
                _calledFloor = value;
                OnPropertyChanged("CalledFloor");
            }
        }

        /// <summary>
        /// The selected floor.
        /// </summary>
        /// <value>
        /// The selected floor.
        /// </value>
        public int SelectedFloor { get; set; }

        public ICommand GoUpCommand { get; private set; }
        public ICommand GoDownCommand { get; private set; }
        public ICommand FloorCommand { get; private set; }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevatorConsoleViewmodel"/> class.
        /// </summary>
        /// <param name="elevator">The elevator.</param>
        /// <param name="goUpCommand">The go up command.</param>
        /// <param name="goDownCommand">The go down command.</param>
        /// <param name="floorCommand">The floor command.</param>
        public ElevatorConsoleViewmodel(Elevator elevator, ICommand goUpCommand, ICommand goDownCommand, ICommand floorCommand)
        {
            _elevator = elevator;           
            _calledFloor = new ObservableCollection<int>();
            FloorDisplay = _elevator.CurrentFloor;
            GoUpCommand = goUpCommand;
            GoDownCommand = goDownCommand;
            FloorCommand = floorCommand;

            for (int i=0; i <= _elevator.MaxFloors; i++)
            {
                _calledFloor.Add(i);
            }
        }
        
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
