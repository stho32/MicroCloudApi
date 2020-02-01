using MicroCloud.API.BL.Interfaces;
using System;

namespace MicroCloud.API.BL.Classes
{

    public class VmRepository : IVmRepository
    {
        public IVm GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public void SetCloudInternalIP(int id, string userHostAddress)
        {
            throw new NotImplementedException();
        }
    }
}
