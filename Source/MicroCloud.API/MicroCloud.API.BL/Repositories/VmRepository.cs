using System.Data.SqlClient;
using Dapper;
using MicroCloud.API.BL.Entities;
using MicroCloud.API.BL.Interfaces;

namespace MicroCloud.API.BL.Repositories
{
    public class VmRepository : IVmRepository
    {
        private readonly IConfigurationProvider _configurationProvider;

        public VmRepository(IConfigurationProvider configurationProvider)
        {
            this._configurationProvider = configurationProvider;
        }

        public IVm GetByName(string name)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(_configurationProvider.ConnectionString))
                {
                    var vm = sqlConnection.QuerySingle<Vm>("SELECT * FROM VirtualMachine WHERE Name=@name", new
                    {
                        name = name
                    });

                    return vm;
                }
            } 
            catch
            {
                return null;
            }
        }

        public void SetCloudInternalIP(int id, string userHostAddress)
        {
            using (var sqlConnection = new SqlConnection(_configurationProvider.ConnectionString))
            {
                sqlConnection.Execute("UPDATE dbo.VirtualMachine SET CloudInternalIP=@cloudInternalIP WHERE Id=@id", new
                {
                    id = id,
                    cloudInternalIp = userHostAddress
                });
            }
        }
    }
}
