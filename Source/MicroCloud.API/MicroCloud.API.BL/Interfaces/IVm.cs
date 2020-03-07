using System;

namespace MicroCloud.API.BL.Interfaces
{
    public interface IVm
    {
        int Id { get; }
        string Name { get; }
        string BaseImage { get; }
        int CreatedOnNode { get; }
        string Alias { get; }
        string Status { get; }
        decimal RamInGb { get; }
        string CloudInternalIP { get; }
        DateTime CreatedAt { get; }
        bool ActivateThisVm { get; }
        bool IsActivated { get; }
        bool RemoveThisVm { get; }
        string MacAddress { get; }
        bool StopThisVm { get; }
        bool StartThisVm { get; }

        IShortenedPortForwarding[] PortForwardings { get; }
    }
}
