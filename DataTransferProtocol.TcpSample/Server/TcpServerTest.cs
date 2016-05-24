using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace DataTransferProtocol.TcpSample.Server
{
    public class TcpServerTest : IDisposable
    {
        private readonly List<TestClient> _clients;
        private readonly TcpListener _tcpListener;
        private bool _isDisposed;

        public TcpServerTest(string ipAddress, int port)
        {
            _tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
            _clients = new List<TestClient>();
            StartListen();
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;
            _isDisposed = true;
            _tcpListener.Stop();

            foreach (var testClient in _clients.ToList())
                testClient.Dispose();
        }

        private async void StartListen()
        {
            _tcpListener.Start();
            while (!_isDisposed)
            {
                try
                {
                    var client = new TestClient(await _tcpListener.AcceptTcpClientAsync());
                    client.Disconnected += ClientOnDisconnected;
                    _clients.Add(client);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        private void ClientOnDisconnected(object sender, EventArgs eventArgs)
        {
            _clients.Remove((TestClient) sender);
        }
    }
}