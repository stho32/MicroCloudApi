using System;
using System.Web.Configuration;
using MicroCloud.API.BL.Interfaces;

namespace MicroCloud.API.CloudSide.App_Code
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public string ConnectionString => WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    }
}