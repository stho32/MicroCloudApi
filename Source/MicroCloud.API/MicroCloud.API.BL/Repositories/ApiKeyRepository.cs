using System.Data.SqlClient;
using Dapper;
using MicroCloud.API.BL.Interfaces;

namespace MicroCloud.API.BL.Repositories
{
    public class ApiKeyRepository : IApiKeyRepository
    {
        private readonly IConfigurationProvider _configurationProvider;

        public ApiKeyRepository(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public int GetApiKeyIdByCode(string code)
        {
            if (code == null)
            {
                return -1;
            }

            using (var sqlConnection = new SqlConnection(_configurationProvider.ConnectionString))
            {
                return sqlConnection.QueryFirstOrDefault<int>(@"
SELECT Id
  FROM ApiKey 
 WHERE Code = @code
   AND IsActive = 1
", new
                {
                    code
                });
            }
        }
    }
}