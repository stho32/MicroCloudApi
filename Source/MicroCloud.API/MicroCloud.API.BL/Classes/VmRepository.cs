using Dapper;
using MicroCloud.API.BL.Entities;
using MicroCloud.API.BL.Interfaces;
using System;
using System.Data.SqlClient;

namespace MicroCloud.API.BL.Classes
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public string ConnectionString => throw new NotImplementedException();
    }

    public class VmRepository : IVmRepository
    {
        private readonly IConfigurationProvider configurationProvider;

        public VmRepository(IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        public IVm GetByName(string name)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(configurationProvider.ConnectionString))
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
            using (var sqlConnection = new SqlConnection(configurationProvider.ConnectionString))
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
