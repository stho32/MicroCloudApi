namespace MicroCloud.API.BL.Interfaces
{
    public interface IPortForwardingRepository
    {
        IPortForwarding GetByPort(int myPort);
        void Add(int virtualMachineId, int localPort);
    }
}