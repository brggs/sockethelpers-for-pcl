using SocketHelpers.Discovery;

namespace SocketHelpers.Test
{
    public class TestDiscoveryFormat : IDiscoveryPayload
    {
        public string RemoteAddress { get; set; }

        public int RemotePort { get; set; }

        public TestDiscoveryFormat(string remoteAddress, int remotePort)
        {
            RemoteAddress = remoteAddress;
            RemotePort = remotePort;
        }
    }
}