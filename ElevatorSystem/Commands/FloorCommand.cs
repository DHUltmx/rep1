using ElevatorSystem.Domain.Entitites;
using ElevatorSystem.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using ElevatorSystem.UI.Views;

namespace ElevatorSystem.UI.Commands
{
    /// <summary>
    /// Class that commands the elevator to move following a floor button request. 
    /// </summary>
    public class FloorCommand
    {
        private ElevatorConsole _view;
        private ElevatorConsoleViewmodel _viewModel;

        /// <summary>
        /// The elevator console view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public ElevatorConsole View
        {
            get { return _view; }
            set { _view = value; }
        }       

        public DelegateCommand Command { get; private set; }

        /// <summary>
        /// The elevator view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public ElevatorConsoleViewmodel ViewModel
        {
            get { return _viewModel; }
            set
            {
                if (_viewModel != value)
                {
                    if (_viewModel != null)
                    {
                        _viewModel.PropertyChanged -= this.OnViewModelPropertyChanged;
                    }
                    _viewModel = value;
                    _viewModel.PropertyChanged += this.OnViewModelPropertyChanged;
                }

                _viewModel.Elevator.CurrentFloorChanged += Elevator_CurrentFloorChanged;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloorCommand"/> class.
        /// </summary>
        public FloorCommand()
        {
            this.Command = new DelegateCommand(ExecuteFloor, CanFloor);
        }

        /// <summary>
        /// Executes the floor command from the floor request button.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public void ExecuteFloor(object parameter)
        {
            int requestedFloor = Convert.ToInt32(parameter);
            ElevatorStatus status;

            if (requestedFloor < this._viewModel.Elevator.CurrentFloor)
            {
                status = ElevatorStatus.Down;
            }
            else
            {
                if (requestedFloor > this._viewModel.Elevator.CurrentFloor)
                {
                    status = ElevatorStatus.Up;
                }
                else
                {
                    status = ElevatorStatus.DoorsOpen;
                }
            }

            ElevatorRequest request = new ElevatorRequest(requestedFloor, status);
            this._viewModel.Elevator.CurrentRequests.Add(request);

            Action elevatorTravel = new Action(() =>
            {
                string arrow = status == ElevatorStatus.Up ? char.ConvertFromUtf32(8593) : char.ConvertFromUtf32(8595);
                
                _view.txtFloorDisplay.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                         new Action(() => { _view.txtFloorDisplay.Text = arrow; }));
                
               
                _view.txtFloorDisplay.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    new Action(() => _viewModel.Elevator.Travel()));
            });

            // Run this action in a separate thread...
            Task.Factory.StartNew(elevatorTravel);
        }

        /// <summary>
        /// Determines whether this instance can be used.
        /// </summary>
        /// <param name="unused">The unused.</param>
        /// <returns></returns>
        public bool CanFloor(object unused)
        {
            return true;
        }

        /// <summary>
        /// Called when [view model property changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.Command.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Updates the UI when the Elevator's current floor changed.
        /// </summary>
        /// <param name="currentFloor">The current floor.</param>
        private void Elevator_CurrentFloorChanged(int currentFloor)
        {
            _view.txtFloorDisplay.Dispatcher.Invoke(DispatcherPriority.Background,
                new Action(() => { _view.txtFloorDisplay.Text = currentFloor.ToString(); }));
        }
    }
}
