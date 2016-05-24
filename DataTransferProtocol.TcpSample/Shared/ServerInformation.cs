using System;

namespace DataTransferProtocol.TcpSample.Shared
{
    [Serializable]
    public class ServerInformation
    {
        public string MachineName { get; set; }
        public string OperatingSystem { get; set; }
        public string UserName { get; set; }
        public int PageSize { get; set; }
    }
}