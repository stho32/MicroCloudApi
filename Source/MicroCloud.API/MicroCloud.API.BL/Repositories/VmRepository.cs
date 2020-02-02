using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
            using (var sqlConnection = new SqlConnection(_configurationProvider.ConnectionString))
            {
                List<Vm> vm = sqlConnection.Query<Vm>("SELECT * FROM VirtualMachine WHERE Name=@name", new
                {
                    name = name
                }).ToList();

                if (vm.Count > 0)
                    return vm[0];

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
