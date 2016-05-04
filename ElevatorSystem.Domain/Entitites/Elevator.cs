using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ElevatorSystem.Domain.Entitites
{
    /// <summary>
    /// An Elevator.
    /// </summary>
    public class Elevator
    {
        private const int MAXFLOORS = 12;
        private int _currentFloor;
        private ElevatorStatus _currentStatus;        
        private IList<ElevatorRequest> _currentRequests;
     
        /// <summary>
        /// A collection of all current requests created from elevator calls or requests from within the elevator.
        /// </summary>
        /// <value>
        /// The current requests.
        /// </value>
        public IList<ElevatorRequest> CurrentRequests
        {
            get { return _currentRequests; }
        }

        /// <summary>
        /// The current floor.
        /// </summary>
        /// <value>
        /// The current floor.
        /// </value>
        public int CurrentFloor
        {
            get { return _currentFloor; }
            set { _currentFloor = value; }
        }

        /// <summary>
        ///The current status.
        /// </summary>
        /// <value>
        /// The current status.
        /// </value>
        public ElevatorStatus CurrentStatus
        {
            get { return _currentStatus; }
        }

        /// <summary>
        /// The maximum number of floors.
        /// </summary>
        /// <value>
        /// The maximum floors.
        /// </value>
        public int MaxFloors
        {
            get { return MAXFLOORS; }
        }

        public event FloorChanged CurrentFloorChanged;
        public delegate void FloorChanged(int currentFloor);

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Elevator"/> class.
        /// </summary>
        public Elevator()
        {
            _currentRequests = new List<ElevatorRequest>();
            _currentStatus = ElevatorStatus.DoorsOpen;
            _currentFloor = 0;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Moves the elevator up or down dependent on the current requests.
        /// </summary>
        public void Travel()
        {
            ElevatorRequest request;
            switch(_currentStatus)
            {                
                case ElevatorStatus.DoorsOpen:                    

                    switch(_currentFloor)
                    {
                        case 0:
                            request = GetNextUpRequest();
                            
                            if (request != null)
                            {
                                Ascend(request);
                            }
                            
                            break;
                        case (MAXFLOORS):
                            request = GetNextDownRequest();

                            if (request != null)
                            {
                                Descend(request);
                            }
                           
                            break;
                        default:
                            request = _currentRequests.Where(x => x.RequestStatus != ElevatorStatus.DoorsOpen)
                                               .FirstOrDefault(); ;

                            if (request != null)
                            {
                                if (request.RequestStatus == ElevatorStatus.Up)
                                {
                                    Ascend(request);
                                }
                                else
                                {
                                    Descend(request);
                                }
                            }
                                              
                            break;
                    }
                    break;

                case ElevatorStatus.Up:
                    request = GetNextUpRequest();

                    if (request != null)
                    {
                        Ascend(request);
                    }
                    
                    break;
                case ElevatorStatus.Down:
                    request = GetNextDownRequest();

                    if (request != null)
                    {
                        Descend(request);
                    }
                   
                    break;
            }
        }

        #endregion

        #region private methods

        /// <summary>
        /// Moves the elevator up.
        /// </summary>
        /// <param name="request">The request.</param>
        private void Ascend(ElevatorRequest request)
        {
            do
            {
                Thread.Sleep(1000);
                CurrentFloor++;
                CurrentFloorChanged(_currentFloor);
                ElevatorRequest interimRequest = GetNextUpRequest();
                
                if (interimRequest != null && interimRequest.RequestedFloor == _currentFloor)
                {
                    request = interimRequest;
                    break;
                }
            }
            while (_currentFloor < request.RequestedFloor);

            _currentRequests.Remove(request);
            _currentStatus = ElevatorStatus.DoorsOpen;
        }

        /// <summary>
        /// Moves the elevator down.
        /// </summary>
        /// <param name="request">The request.</param>
        private void Descend(ElevatorRequest request)
        {
            do
            {
                Thread.Sleep(1000);
                CurrentFloor--;
                CurrentFloorChanged(_currentFloor);
                ElevatorRequest interimRequest = GetNextDownRequest();
  
                if (interimRequest != null && interimRequest.RequestedFloor == _currentFloor)
                {
                    request = interimRequest;
                    break;
                }
            }
            while (_currentFloor > request.RequestedFloor);

            _currentRequests.Remove(request);
            _currentStatus = ElevatorStatus.DoorsOpen;
        }

        /// <summary>
        /// Gets the next 'up' request.
        /// </summary>
        /// <returns></returns>
        private ElevatorRequest GetNextUpRequest()
        {
            return _currentRequests.Where(x => x.RequestStatus == ElevatorStatus.Up && x.RequestedFloor >= _currentFloor)
                                                   .OrderBy(x => x.RequestedFloor)
                                                   .FirstOrDefault();
        }

        /// <summary>
        /// Gets the next 'down' request.
        /// </summary>
        /// <returns></returns>
        private ElevatorRequest GetNextDownRequest()
        {
            return _currentRequests.Where(x => x.RequestStatus == ElevatorStatus.Down && x.RequestedFloor <= _currentFloor)
                                                    .OrderByDescending(x => x.RequestedFloor)
                                                    .FirstOrDefault();
        }

        #endregion
    }


    /// <summary>
    /// Describes the status of the elevator.
    /// </summary>
    public enum ElevatorStatus
    {
        DoorsOpen,
        Up,
        Down
    }
}
