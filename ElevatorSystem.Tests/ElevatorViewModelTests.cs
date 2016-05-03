using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using NUnit.Framework;
using ElevatorSystem.UI.ViewModel;
using ElevatorSystem.Domain.Entitites;

namespace ElevatorSystem.Tests
{
    [TestFixture]
    public class ElevatorViewModelTests
    {
        Elevator e;
        ElevatorConsoleViewmodel vm;

        [OneTimeSetUp]
        public void Initialise()
        {
            e = new Elevator();
            e.CurrentFloorChanged += E_CurrentFloorChanged;
            vm = new ElevatorConsoleViewmodel(e, null, null, null);
        }
        
        [Test]
        public void Can_update_current_floor()
        {
            vm.Elevator.CurrentRequests.Add(new ElevatorRequest(4, ElevatorStatus.Up));
            vm.Elevator.Travel();
            Assert.IsTrue(vm.Elevator.CurrentFloor == 4);
            Assert.IsTrue(vm.FloorDisplay == 4);
        }

        private void E_CurrentFloorChanged(int currentFloor)
        {
            vm.FloorDisplay = currentFloor;
        }
    }
}
