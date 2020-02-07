using System.Collections;
using System.Collections.Generic;

namespace MicroCloud.API.BL.Interfaces
{
    public interface IVmRepository
    {
        IVm GetByName(string name);
        void SetCloudInternalIP(int id, string userHostAddress);

        IEnumerable<IVm> GetByApiKey(int apiKeyId);
    }
}
