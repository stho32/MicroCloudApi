using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using MicroCloud.API.BL.Entities;
using MicroCloud.API.BL.Interfaces;

namespace MicroCloud.API.BL.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly IConfigurationProvider _configurationProvider;

        public ImageRepository(IConfigurationProvider configurationProvider)
        {
            this._configurationProvider = configurationProvider;
        }

        public IEnumerable<IImage> GetByApiKey(int apiKeyId)
        {
            using (var sqlConnection = new SqlConnection(_configurationProvider.ConnectionString))
            {
                IEnumerable<Image> images = sqlConnection.Query<Image>("SELECT * FROM VirtualMachineImage WHERE IsActive = 1 ORDER BY Name");
                return images;
            }
        }
    }
}
