namespace MicroCloud.API.BL.Interfaces
{
    public interface IShortenedPortForwarding
    {
        int Id { get; }
        string Comment { get; }
        int LocalPort { get; }
        int PortOnEntranceRouter { get; }
        bool IsEnabled { get; }
        bool RemoveThis { get; }

    }
}