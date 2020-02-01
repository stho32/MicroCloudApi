using MicroCloud.API.BL.Interfaces;

namespace MicroCloud.API.BL.Classes
{
    public class RepositoryFactory
    {
        public IVmRepository VmRepository() => new VmRepository();
    }
}
