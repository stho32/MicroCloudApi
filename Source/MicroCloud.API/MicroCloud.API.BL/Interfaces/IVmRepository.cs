﻿using System.Collections;
using System.Collections.Generic;

namespace MicroCloud.API.BL.Interfaces
{
    public interface IVmRepository
    {
        IVm GetByName(string name, int apiKeyId = -1);
        void SetCloudInternalIP(int id, string userHostAddress);

        IEnumerable<IVm> GetByApiKey(int apiKeyId);
        IVm CreateForApiKey(int apiKeyId, string baseImage, string parametersJson);
        void RemoveVm(int apiKeyId, string vmname);
    }
}
