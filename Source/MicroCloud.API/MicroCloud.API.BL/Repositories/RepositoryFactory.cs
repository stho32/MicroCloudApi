using MicroCloud.API.BL.Interfaces;

namespace MicroCloud.API.BL.Repositories
{
    public class RepositoryFactory
    {
        private readonly IConfigurationProvider configurationProvider;

        public RepositoryFactory(IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        public IVmRepository VmRepository() => new VmRepository(configurationProvider);
    }
}
