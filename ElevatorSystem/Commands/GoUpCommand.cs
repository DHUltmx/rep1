using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSystem.UI.ViewModel;
using ElevatorSystem.UI.Views;
using ElevatorSystem.Domain.Entitites;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Input;

namespace ElevatorSystem.UI.Commands
{
    /// <summary>
    /// Command class bound to Up button
    /// </summary>
    public class GoUpCommand
    {
        private ElevatorConsole _view;
        private ElevatorConsoleViewmodel _viewModel;

        /// <summary>
        /// The view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public ElevatorConsole View
        {
            get { return _view;}
            set {_view = value; }             
        }       

        public DelegateCommand Command { get; private set; }

        /// <summary>
        /// The view model.
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
        /// Initializes a new instance of the <see cref="GoUpCommand"/> class.
        /// </summary>
        public GoUpCommand()
        {
            this.Command = new DelegateCommand(ExecuteGoUp, CanGoUp);            
        }
        
        /// <summary>
        /// Executes the go up request called from the UI Up button.
        /// </summary>
        /// <param name="unused">The unused.</param>
        public void ExecuteGoUp(object unused)
        {
            //create up request...
            ElevatorRequest request = new ElevatorRequest(this.ViewModel.SelectedFloor, ElevatorStatus.Up);
            _viewModel.Elevator.CurrentRequests.Add(request);
           
            Action elevatorTravel = new Action(() =>
            {
                _view.txtFloorDisplay.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    new Action(() => { _view.txtFloorDisplay.Text = char.ConvertFromUtf32(8593); }));

                _view.txtFloorDisplay.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    new Action(() => _viewModel.Elevator.Travel()));
            });

            // Run this action in a separate thread...
            Task.Factory.StartNew(elevatorTravel);
        }

        /// <summary>
        /// Determines whether this instance [can go up].
        /// </summary>
        /// <param name="unused">The unused.</param>
        /// <returns></returns>
        public bool CanGoUp(object unused)
        {
            return true;
            //TODO fix
            //return _viewModel.Elevator.CurrentFloor < _viewModel.Elevator.MaxFloors;
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
        /// Updates the UI when the elevator's the current floor changed.
        /// </summary>
        /// <param name="currentFloor">The current floor.</param>
        private void Elevator_CurrentFloorChanged(int currentFloor)
        {
            _view.txtFloorDisplay.Dispatcher.Invoke(DispatcherPriority.Background,
                    new Action(() => { _view.txtFloorDisplay.Text = currentFloor.ToString(); }));
            
            //TODO: neither comment forces execution of method 
            _viewModel.GoDownCommand.CanExecute(null);
            CommandManager.InvalidateRequerySuggested();      
        }
    }
}
