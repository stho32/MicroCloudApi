namespace MicroCloud.API.BL.Interfaces
{
    public interface IVmRepository
    {
        IVm GetByName(string name);
        void SetCloudInternalIP(int id, string userHostAddress);
    }
}
