using System.Collections.Generic;

namespace MicroCloud.API.BL.Interfaces
{
    public interface IImageRepository
    {
        IEnumerable<IImage> GetByApiKey(int apiKeyId);
    }
}
