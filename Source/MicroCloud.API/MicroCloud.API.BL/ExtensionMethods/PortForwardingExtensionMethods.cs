using MicroCloud.API.BL.Entities;
using MicroCloud.API.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroCloud.API.BL.ExtensionMethods
{
    public static class PortForwardingExtensionMethods
    {
        public static IShortenedPortForwarding[] ToShortenedPortForwardings(this IEnumerable<IPortForwarding> portForwardings)
        {
            var result = new List<IShortenedPortForwarding>();

            foreach (var portForwarding in portForwardings)
            {
                var shortenedPortForwarding = new ShortenedPortForwarding(
                    portForwarding.Id,
                    portForwarding.Comment,
                    portForwarding.LocalPort,
                    portForwarding.PortOnEntranceRouter,
                    portForwarding.IsEnabled,
                    portForwarding.RemoveThis);

                result.Add(shortenedPortForwarding);
            }

            return result.ToArray();
        }
    }
}
