using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using DataTransferProtocol.TcpSample.Client;
using DataTransferProtocol.TcpSample.Server;
using DataTransferProtocol.TcpSample.Shared;

namespace DataTransferProtocol.TcpSample
{
    public partial class Form1 : Form
    {
        private TcpServerTest _tcpServerTest;
        private TcpClientTest _tcpClientTest;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ipComboBox.Items.Add("127.0.0.1");
            ipComboBox.Items.AddRange(
                Dns.GetHostAddresses(Dns.GetHostName())
                    .OrderByDescending(x => x.AddressFamily == AddressFamily.InterNetwork)
                    .Select(x => (object)x)
                    .ToArray());
            ipComboBox.SelectedIndex = 0;
            clientIpComboBox.Items.AddRange(ipComboBox.Items.Cast<object>().ToArray());
            clientIpComboBox.SelectedIndex = 0;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _tcpServerTest?.Dispose();
            _tcpClientTest?.Dispose();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                _tcpServerTest = new TcpServerTest(ipComboBox.Text, (int)portNumericUpDown.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to start the TCP server: " + ex.Message);
                return;
            }

            stopButton.Enabled = true;
            startButton.Enabled = false;
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            _tcpServerTest.Dispose();
            _tcpServerTest = null;

            stopButton.Enabled = false;
            startButton.Enabled = true;
        }

        private async void connectButton_Click(object sender, EventArgs e)
        {
            connectButton.Enabled = false;
            try
            {
                _tcpClientTest = await TcpClientTest.Connect(clientIpComboBox.Text, (int) clientPortNumericUpDown.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to the TCP server: " + ex.Message);
                connectButton.Enabled = true;
                return;
            }

            _tcpClientTest.Disconnected += (o, args) =>
            {
                BeginInvoke((MethodInvoker) delegate
                {
                    connectButton.Enabled = true;
                    disconnectButton.Enabled = false;
                    clientCommandsPanel.Enabled = false;
                });
            };

            disconnectButton.Enabled = true;
            clientCommandsPanel.Enabled = true;
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            _tcpClientTest.Dispose();
            _tcpClientTest = null;
        }

        private void showMessageBoxButton_Click(object sender, EventArgs e)
        {
            _tcpClientTest.DataTransferProtocolFactory.ExecuteMethod("ShowMessageBox", showMessageBoxTextBox.Text ?? "");
        }

        private void getServerInformationButton_Click(object sender, EventArgs e)
        {
            var information =
                _tcpClientTest.DataTransferProtocolFactory.ExecuteFunction<ServerInformation>("GetServerInformation");
            MessageBox.Show(
                $"MachineName: {information.MachineName}\r\nUserName: {information.UserName}\r\nOperatingSystem: {information.OperatingSystem}\r\nPageSize: {information.PageSize}");
        }
    }
}