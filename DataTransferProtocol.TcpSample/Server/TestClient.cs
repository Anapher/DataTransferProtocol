using System;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;
using DataTransferProtocol.TcpSample.Shared;

namespace DataTransferProtocol.TcpSample.Server
{
    public class TestClient : IDisposable
    {
        private readonly BinaryReader _binaryReader;
        private readonly BinaryWriter _binaryWriter;
        private readonly NetworkStream _networkStream;
        private readonly Func<byte> _readByteDelegate;
        private readonly TcpClient _tcpClient;
        private bool _isDisposed;
        private readonly DtpProcessor _dtpProcessor;
        private readonly object _sendLock = new object();

        public TestClient(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            _networkStream = tcpClient.GetStream();
            _binaryWriter = new BinaryWriter(_networkStream);
            _binaryReader = new BinaryReader(_networkStream);

            _dtpProcessor = new DtpProcessor();
            InitializeDataTransferProtocol();

            _readByteDelegate += _binaryReader.ReadByte;
            _readByteDelegate.BeginInvoke(EndRead, null);
        }

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

        private void InitializeDataTransferProtocol()
        {
            _dtpProcessor.RegisterMethod("ShowMessageBox", parameters => MessageBox.Show(parameters.GetString(0)));
            _dtpProcessor.RegisterFunction("GetServerInformation",
                parameters =>
                    new ServerInformation
                    {
                        MachineName = Environment.MachineName,
                        OperatingSystem = Environment.OSVersion.ToString(),
                        PageSize = Environment.SystemPageSize,
                        UserName = Environment.UserName
                    });
        }

        private void EndRead(IAsyncResult asyncResult)
        {
            try
            {
                var parameter = _readByteDelegate.EndInvoke(asyncResult);
                var size = _binaryReader.ReadInt32();
                var bytes = _binaryReader.ReadBytes(size);

                switch ((ClientPackageToken) parameter)
                {
                    case ClientPackageToken.DataTransferProtocol:
                        var result = _dtpProcessor.Receive(bytes);
                        lock (_sendLock)
                        {
                            _binaryWriter.Write((byte) ServerPackageToken.DataTransferProtocolResponse);
                            _binaryWriter.Write(result.Length);
                            _binaryWriter.Write(result);
                        }
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