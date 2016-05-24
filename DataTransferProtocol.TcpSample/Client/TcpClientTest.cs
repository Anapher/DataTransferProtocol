using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using DataTransferProtocol.TcpSample.Shared;

namespace DataTransferProtocol.TcpSample.Client
{
    public class TcpClientTest : IDisposable
    {
        private readonly BinaryReader _binaryReader;
        private readonly BinaryWriter _binaryWriter;
        private readonly NetworkStream _networkStream;
        private readonly Func<byte> _readByteDelegate;
        private readonly TcpClient _tcpClient;
        private readonly object _writeLock = new object();
        private bool _isDisposed;

        private TcpClientTest(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            _networkStream = tcpClient.GetStream();
            _binaryWriter = new BinaryWriter(_networkStream);
            _binaryReader = new BinaryReader(_networkStream);

            DataTransferProtocolFactory = new DtpFactory(SendData);

            _readByteDelegate += _binaryReader.ReadByte;
            _readByteDelegate.BeginInvoke(EndRead, null);
        }

        public DtpFactory DataTransferProtocolFactory { get; }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            using (_binaryReader)
            using (_binaryWriter)
            using (_networkStream)
                _tcpClient.Close();

            Disconnected?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Disconnected;

        public static async Task<TcpClientTest> Connect(string ipAddress, int port)
        {
            var client = new TcpClient();
            var result = client.BeginConnect(ipAddress, port, null, null);
            var success = await Task.Run(() => result.AsyncWaitHandle.WaitOne(3000, false));
            if (!success)
                throw new TimeoutException();

            client.EndConnect(result);
            return new TcpClientTest(client);
        }

        private void SendData(byte[] data)
        {
            if (_isDisposed)
                return;

            lock (_writeLock)
            {
                _binaryWriter.Write((byte)ClientPackageToken.DataTransferProtocol);
                _binaryWriter.Write(data.Length);
                _binaryWriter.Write(data);
            }
        }

        private void EndRead(IAsyncResult asyncResult)
        {
            try
            {
                var parameter = _readByteDelegate.EndInvoke(asyncResult);
                var size = _binaryReader.ReadInt32();
                var bytes = _binaryReader.ReadBytes(size);

                switch ((ServerPackageToken) parameter)
                {
                    case ServerPackageToken.DataTransferProtocolResponse:
                        DataTransferProtocolFactory.Receive(bytes);
                        break;
                }

                _readByteDelegate.BeginInvoke(EndRead, null);
            }
            catch (Exception)
            {
                Dispose();
            }
        }
    }
}