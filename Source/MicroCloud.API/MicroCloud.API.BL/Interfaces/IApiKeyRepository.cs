namespace MicroCloud.API.BL.Interfaces
{
    public interface IApiKeyRepository
    {
        int GetApiKeyIdByCode(string code);
    }
}