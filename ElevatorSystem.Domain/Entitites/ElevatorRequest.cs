using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Domain.Entitites
{
    /// <summary>
    /// A request to move the elevator.
    /// </summary>
    public class ElevatorRequest
    {
        private int _requestedFloor;
        private ElevatorStatus _requestStatus;

        /// <summary>
        /// The requested floor.
        /// </summary>
        /// <value>
        /// The requested floor.
        /// </value>
        public int RequestedFloor
        {
            get { return _requestedFloor; }
        }

        /// <summary>
        /// The request status.
        /// </summary>
        /// <value>
        /// The request status.
        /// </value>
        public ElevatorStatus RequestStatus
        {
            get { return _requestStatus; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevatorRequest"/> class.
        /// </summary>
        /// <param name="requestedFloor">The requested floor.</param>
        /// <param name="status">The status.</param>
        public ElevatorRequest(int requestedFloor, ElevatorStatus status)
        {
            _requestedFloor = requestedFloor;
            _requestStatus = status;
        }       
    }
}
