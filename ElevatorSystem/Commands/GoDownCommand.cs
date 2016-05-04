using ElevatorSystem.UI.ViewModel;
using ElevatorSystem.UI.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ElevatorSystem.UI.Commands
{
    /// <summary>
    /// Command class bound to Down button
    /// </summary>
    public class GoDownCommand
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
            get { return _view; }
            set { _view = value; }
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
        /// Initializes a new instance of the <see cref="GoDownCommand"/> class.
        /// </summary>
        public GoDownCommand()
        {
            this.Command = new DelegateCommand(ExecuteGoDown, CanGoDown);
        }

        /// <summary>
        /// Executes the go down request called from the UI Down button.
        /// </summary>
        /// <param name="unused">The unused.</param>
        public void ExecuteGoDown(object unused)
        {
            _viewModel.GoDown();

            Action elevatorTravel = new Action(() =>
            {
                _view.txtFloorDisplay.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    new Action(() => { _view.txtFloorDisplay.Text = char.ConvertFromUtf32(8595); }));

                _view.txtFloorDisplay.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    new Action(() => _viewModel.Elevator.Travel()));
            });

            // Run this action in a separate thread...
            Task.Factory.StartNew(elevatorTravel);           
        }

        /// <summary>
        /// Determines whether this instance [can go down].
        /// </summary>
        /// <param name="unused">The unused.</param>
        /// <returns></returns>
        public bool CanGoDown(object unused)
        {
            return true;
            //TODO: fix 
            //return _viewModel.Elevator.CurrentFloor > 0;
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
        /// Updates the UI when the Elevator's the current floor changed.
        /// </summary>
        /// <param name="currentFloor">The current floor.</param>
        private void Elevator_CurrentFloorChanged(int currentFloor)
        {
            _view.txtFloorDisplay.Dispatcher.Invoke(DispatcherPriority.Background,
                new Action(() => { _view.txtFloorDisplay.Text = currentFloor.ToString(); }));
        }
    }
}
