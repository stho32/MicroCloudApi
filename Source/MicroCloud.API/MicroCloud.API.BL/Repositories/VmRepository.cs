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

        public IVm GetByName(string name, int apiKeyId = -1)
        {
            using (var sqlConnection = new SqlConnection(_configurationProvider.ConnectionString))
            {
                List<Vm> vm = sqlConnection.Query<Vm>("SELECT * FROM VirtualMachine WHERE Name=@name AND (ApiKeyId=@apiKeyId OR @apiKeyId=-1)", new
                {
                    name,
                    apiKeyId
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
                    id,
                    cloudInternalIp = userHostAddress
                });
            }
        }

        public IEnumerable<IVm> GetByApiKey(int apiKeyId)
        {
            using (var sqlConnection = new SqlConnection(_configurationProvider.ConnectionString))
            {
                IEnumerable<Vm> vm = 
                    sqlConnection.Query<Vm>("SELECT * FROM VirtualMachine WHERE ApiKeyId=@id", 
                        new { id = apiKeyId });

                return vm;
            }
        }

        public IVm CreateForApiKey(int apiKeyId, string baseImage, int ramInGb, string parametersJson)
        {
            using (var sqlConnection = new SqlConnection(_configurationProvider.ConnectionString))
            {
                List<Vm> vm = sqlConnection.Query<Vm>("EXEC AddMicroVMClientSide @BaseImage, @RamInGb, @ApiKeyId", new
                {
                    BaseImage = baseImage,
                    RamInGb = ramInGb,
                    ApiKeyId = apiKeyId
                }).ToList();

                if (vm.Count > 0)
                    return vm[0];

                return null;
            }
        }

        public void RemoveVm(int apiKeyId, string vmname)
        {
            using (var sqlConnection = new SqlConnection(_configurationProvider.ConnectionString))
            {
                sqlConnection.Execute("UPDATE dbo.VirtualMachine SET RemoveThisVm=1 WHERE ApiKeyId=@id AND name=@vmname", new
                {
                    id = apiKeyId,
                    vmname
                });
            }
        }

        public void SetVmAlias(int apiKeyId, string vmname, string alias)
        {
            var vm = GetByName(vmname, apiKeyId);

            if ( vm != null )
            {
                using (var sqlConnection = new SqlConnection(_configurationProvider.ConnectionString))
                {
                    sqlConnection.Execute("EXEC dbo.SetVmAlias @Id, @Alias", new
                    {
                        id = vm.Id,
                        alias
                    });
                }
            }
        }

        public void StopVm(int apiKeyId, string vmname)
        {
            var vm = GetByName(vmname, apiKeyId);

            if (vm != null)
            {
                using (var sqlConnection = new SqlConnection(_configurationProvider.ConnectionString))
                {
                    sqlConnection.Execute("UPDATE dbo.VirtualMachine SET StopThisVm = 1 WHERE Id=@Id", new
                    {
                        id = vm.Id
                    });
                }
            }
        }

        public void StartVm(int apiKeyId, string vmname)
        {
            var vm = GetByName(vmname, apiKeyId);

            if (vm != null)
            {
                using (var sqlConnection = new SqlConnection(_configurationProvider.ConnectionString))
                {
                    sqlConnection.Execute("UPDATE dbo.VirtualMachine SET StartThisVm = 1 WHERE Id=@Id", new
                    {
                        id = vm.Id
                    });
                }
            }
        }
    }
}
