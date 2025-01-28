using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace ChatClient
{
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private NetworkStream stream;

        public MainWindow()
        {
            InitializeComponent();
            ClientConnect();
            Thread receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();
        }

        private void ClientConnect()
        {
            client = new TcpClient("127.0.0.1", 8888);
            stream = client.GetStream();
            string clientName = "Client_" + Guid.NewGuid();
            byte[] nameBytes = Encoding.UTF8.GetBytes(clientName);
            stream.Write(nameBytes, 0, nameBytes.Length);
        }

        private void ReceiveMessages()
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            while (true)
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead <= 0) break;

                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Dispatcher.Invoke(() => MessagesList.Text += message + "\n");
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string recipient = RecipientBox.Text;
            string message = MessageBox.Text;

            if (!string.IsNullOrWhiteSpace(message))
            {
                byte[] data = Encoding.UTF8.GetBytes($"{recipient}: {message}");
                stream.Write(data, 0, data.Length);
                MessageBox.Clear();
            }
        }
    }
}