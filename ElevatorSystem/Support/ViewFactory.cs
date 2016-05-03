using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSystem.UI.ViewModel;
using ElevatorSystem.UI.Views;
using ElevatorSystem.UI.Support;
using ElevatorSystem.Domain.Entitites;
using ElevatorSystem.UI.Commands;

namespace ElevatorSystem.UI.Support
{
    /// <summary>
    /// Creates the components required to run the app.
    /// </summary>
    public class ViewFactory
    {
        /// <summary>
        /// Creates the infrastructure.
        /// </summary>
        /// <returns></returns>
        public ViewInfrastructure Create()
        {
            Elevator model = new Elevator();

            GoUpCommand goUpCommand = new GoUpCommand();
            GoDownCommand goDownCommand = new GoDownCommand();
            FloorCommand floorCommand = new FloorCommand();            
            
            ElevatorConsoleViewmodel viewModel = new ElevatorConsoleViewmodel(model, goUpCommand.Command, goDownCommand.Command, floorCommand.Command);
            
            ElevatorConsole view = new ElevatorConsole();
            view.ViewModel = viewModel;

            goUpCommand.ViewModel = viewModel;
            goUpCommand.View = view;
            goDownCommand.ViewModel = viewModel;
            goDownCommand.View = view;
            floorCommand.ViewModel = viewModel;
            floorCommand.View = view;
            
            return new ViewInfrastructure(view, viewModel, model);
        }
    }
}
