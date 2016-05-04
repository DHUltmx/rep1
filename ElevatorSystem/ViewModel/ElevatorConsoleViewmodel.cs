using ElevatorSystem.Domain.Entitites;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

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

        internal void GoUp()
        {
            //create up request...
            ElevatorRequest request = new ElevatorRequest(this.SelectedFloor, ElevatorStatus.Up);
            this.Elevator.CurrentRequests.Add(request);
        }

        internal void GoDown()
        {
            ElevatorRequest request = new ElevatorRequest(this.SelectedFloor, ElevatorStatus.Down);
            this.Elevator.CurrentRequests.Add(request);
        }

        internal ElevatorStatus FloorRequest(int requestedFloor)
        {
            ElevatorStatus status;

            if (requestedFloor < this.Elevator.CurrentFloor)
            {
                status = ElevatorStatus.Down;
            }
            else
            {
                if (requestedFloor > this.Elevator.CurrentFloor)
                {
                    status = ElevatorStatus.Up;
                }
                else
                {
                    status = ElevatorStatus.DoorsOpen;
                }
            }

            ElevatorRequest request = new ElevatorRequest(requestedFloor, status);
            this.Elevator.CurrentRequests.Add(request);
            return status;
        }

        /// <summary>
        /// To force the CanExecute methods.
        /// </summary>
        internal void Requery()
        {
            //TODO: this run on UI thread but neither working.
            CommandManager.InvalidateRequerySuggested();
            GoDownCommand.CanExecute(null);
        }
    }
}
