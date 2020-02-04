using MicroCloud.API.BL.Interfaces;

namespace MicroCloud.API.BL.Entities
{
    public class PortForwarding : IPortForwarding
    {
        public int Id { get; }
        public int VirtualMachineId { get; }
        public string Comment { get; }
        public int LocalPort { get; }
        public int PortOnEntranceRouter { get; }
        public bool IsEnabled { get; }
        public bool RemoveThis { get; }
    }
}