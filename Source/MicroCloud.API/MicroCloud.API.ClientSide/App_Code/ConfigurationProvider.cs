using System.Web.Configuration;
using MicroCloud.API.BL.Interfaces;

namespace MicroCloud.API.ClientSide
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public string ConnectionString => WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    }
}