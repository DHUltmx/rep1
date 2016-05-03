using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSystem.UI.Views;
using ElevatorSystem.UI.ViewModel;
using ElevatorSystem.Domain.Entitites;

namespace ElevatorSystem.UI.Support
{
    /// <summary>
    /// Initiates basic infrastructure.
    /// </summary>
    public class ViewInfrastructure
    {
        public ElevatorConsole View { get; private set; }

        public ElevatorConsoleViewmodel ViewModel { get; private set; }

        public Elevator Model { get; private set; }

        public ViewInfrastructure(ElevatorConsole view, ElevatorConsoleViewmodel viewModel, Elevator model)
        {
            this.View = view;
            this.ViewModel = viewModel;
            this.Model = model;
        }
    }
}
