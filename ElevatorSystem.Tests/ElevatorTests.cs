using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using NUnit.Framework;
using ElevatorSystem.Domain.Entitites;

namespace ElevatorSystem.Tests
{
    [TestFixture]
    public class ElevatorTests
    {
        Elevator e;

        [OneTimeSetUp]
        public void Initialise()
        {
            e = new Elevator();
            e.CurrentFloorChanged += E_CurrentFloorChanged;
        }
       

        [Test]
        public void Can_ascend()
        {                    
            ElevatorRequest r = new ElevatorRequest(3, ElevatorStatus.Up);
            e.CurrentRequests.Add(r);
            e.Travel();
            //e.CloseLog();
            Assert.True(e.CurrentFloor == r.RequestedFloor);
        }

        [Test]
        public void Can_descend()
        {   
            e.CurrentRequests.Add(new ElevatorRequest(5, ElevatorStatus.Up));           
            e.Travel();
            e.CurrentRequests.Add(new ElevatorRequest(2, ElevatorStatus.Down));
            e.Travel();
            //e.CloseLog();
            Assert.True(e.CurrentFloor == 2);
        }

        // Simulates:
        //  1. Called from floor 8;
        //  2. Called from floor 5 before floor 8 reached.
        //  3. Stops at floor 5, user requests floor 10.
        //  4. Stops at floor 8, user requests floor 12.
        //  Stop order should be 5, 8, 10, 12.
        [Test]
        public void Can_stop_when_multiple_up_requests()
        {
            Initialise();
            e.CurrentRequests.Add(new ElevatorRequest(8, ElevatorStatus.Up));
            Thread t = new Thread(() => e.Travel());
            t.Start();
            Thread.Sleep(2000);
            e.CurrentRequests.Add(new ElevatorRequest(5, ElevatorStatus.Up));
            t.Join();
            //e.CloseLog();
            Assert.IsTrue(e.CurrentFloor == 5);
            Assert.IsTrue(e.CurrentStatus == ElevatorStatus.DoorsOpen);

            e.CurrentRequests.Add(new ElevatorRequest(10, ElevatorStatus.Up));
            t = new Thread(() => e.Travel());
            t.Start();
            Thread.Sleep(2000);
            t.Join();
            Assert.IsTrue(e.CurrentFloor == 8);
            Assert.IsTrue(e.CurrentStatus == ElevatorStatus.DoorsOpen);

            e.CurrentRequests.Add(new ElevatorRequest(12, ElevatorStatus.Up));
            t = new Thread(() => e.Travel());
            t.Start();
            Thread.Sleep(1000);
            t.Join();
            Assert.IsTrue(e.CurrentFloor == 10);
            Assert.IsTrue(e.CurrentStatus == ElevatorStatus.DoorsOpen);

            t = new Thread(() => e.Travel());
            t.Start();
            Thread.Sleep(1000);
            t.Join();
            Assert.IsTrue(e.CurrentFloor == 12);
            Assert.IsTrue(e.CurrentStatus == ElevatorStatus.DoorsOpen);
        }

        private void E_CurrentFloorChanged(int currentFloor)
        {
            //throw new NotImplementedException();
        }

        [Test]
        public void Can_stop_when_multiple_down_requests()
        {
            e.CurrentRequests.Add(new ElevatorRequest(11, ElevatorStatus.Up));
            e.Travel();
            e.CurrentRequests.Add(new ElevatorRequest(2, ElevatorStatus.Down));
            Thread t = new Thread(() => e.Travel());
            t.Start();
            Thread.Sleep(5000);
            e.CurrentRequests.Add(new ElevatorRequest(4, ElevatorStatus.Down));
            t.Join();
            //e.CloseLog();
            Assert.IsTrue(e.CurrentFloor == 4);
            Assert.IsTrue(e.CurrentStatus == ElevatorStatus.DoorsOpen);

        }
    }
}
