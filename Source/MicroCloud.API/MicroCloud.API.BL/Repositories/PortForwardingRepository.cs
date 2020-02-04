using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using MicroCloud.API.BL.Entities;
using MicroCloud.API.BL.Interfaces;

namespace MicroCloud.API.BL.Repositories
{
    public class PortForwardingRepository : IPortForwardingRepository
    {
        private readonly IConfigurationProvider _configurationProvider;

        public PortForwardingRepository(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public IPortForwarding GetByPort(int virtualMachineId, int myPort)
        {
            using (var sqlConnection = new SqlConnection(_configurationProvider.ConnectionString))
            {
                List<PortForwarding> portForwardings = 
                    sqlConnection.Query<PortForwarding>(@"
                        SELECT * 
                          FROM VirtualMachinePortForwarding 
                         WHERE VirtualMachineId = @virtualMachineId
                           AND LocalPort = @port", 
                        new
                            {
                                virtualMachineId,
                                port = myPort
                            }).ToList();

                if (portForwardings.Count > 0)
                    return portForwardings[0];

                return null;
            }
        }

        public void Add(int virtualMachineId, int localPort)
        {
            using (var sqlConnection = new SqlConnection(_configurationProvider.ConnectionString))
            {
                sqlConnection.Execute(@"
                    INSERT INTO VirtualMachinePortForwarding 
                        (VirtualMachineId, LocalPort)
                    VALUES 
                        (@virtualMachineId, @localPort)
                    ", new
                {
                    virtualMachineId,
                    localPort
                });
            }
        }

        public void Remove(int virtualMachineId, int localPort)
        {
            using (var sqlConnection = new SqlConnection(_configurationProvider.ConnectionString))
            {
                sqlConnection.Execute(@"
                    UPDATE VirtualMachinePortForwarding 
                       SET RemoveThis = 1
                     WHERE VirtualMachineId = @virtualMachineId
                       AND LocalPort = @localPort
                    ", new {
                    virtualMachineId,
                    localPort
                });
            }
        }
    }
}