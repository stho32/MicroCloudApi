namespace MicroCloud.API.BL.Interfaces
{
    public interface IPortForwarding
    {
        int Id { get; }
        int VirtualMachineId { get; }
        string Comment { get; }
        int LocalPort { get; }
        int PortOnEntranceRouter { get; }
        bool IsEnabled { get; }
        bool RemoveThis { get; }
    }
}