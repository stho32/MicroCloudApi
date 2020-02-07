using System.Collections;
using System.Collections.Generic;

namespace MicroCloud.API.BL.Interfaces
{
    public interface IPortForwardingRepository
    {
        IPortForwarding GetByPort(int virtualMachineId, int myPort);
        void Add(int virtualMachineId, int localPort);
        void Remove(int virtualMachineId, int localPort);
        IEnumerable<IPortForwarding> GetByVm(int vmId);
    }
}