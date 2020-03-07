using MicroCloud.API.BL.Interfaces;

namespace MicroCloud.API.BL.Entities
{
    public class ShortenedPortForwarding : IShortenedPortForwarding
    {
        public int Id { get; }

        public string Comment { get; }

        public int LocalPort { get; }

        public int PortOnEntranceRouter { get; }

        public bool IsEnabled { get; }

        public bool RemoveThis { get; }

        public ShortenedPortForwarding(int id, string comment, int localport, int portOnEntranceRouter, bool isEnabled, bool removeThis)
        {
            Id = id;
            Comment = comment;
            LocalPort = localport;
            PortOnEntranceRouter = portOnEntranceRouter;
            IsEnabled = isEnabled;
            RemoveThis = removeThis;
        }
    }
}