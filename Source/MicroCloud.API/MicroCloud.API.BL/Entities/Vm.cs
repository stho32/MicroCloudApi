using MicroCloud.API.BL.Interfaces;
using System;

namespace MicroCloud.API.BL.Entities
{
    public class Vm : IVm
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string BaseImage { get; set; }

        public int CreatedOnNode { get; set; }

        public string Alias { get; set; }

        public string Status { get; set; }

        public decimal RamInGb { get; set; }

        public string CloudInternalIP { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool ActivateThisVm { get; set; }

        public bool IsActivated { get; set; }

        public bool RemoveThisVm { get; set; }
    }
}
