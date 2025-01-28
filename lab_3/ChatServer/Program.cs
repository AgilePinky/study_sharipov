using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Program
{
    // Хранилище для подключенных клиентов, где ключом является имя клиента, а значением - TcpClient
    private static readonly ConcurrentDictionary<string, TcpClient> clients = new ConcurrentDictionary<string, TcpClient>();

    static void Main(string[] args)
    // Создание TcpListener для прослушивания на всех IP-адресах на порту 8888
    {
        TcpListener server = new TcpListener(IPAddress.Any, 8888);
        server.Start();
        Console.WriteLine("Сервер запущен на порту 8888...");

        while (true)
        {
            var client = server.AcceptTcpClient();
            ThreadPool.QueueUserWorkItem(HandleClient, client);
        }
    }

    private static void HandleClient(object obj)
    {
        var client = (TcpClient)obj;
        string clientName = string.Empty;

        using (var stream = client.GetStream())
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            // Получение имени клиента
            if ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                clientName = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                clients.TryAdd(clientName, client);
                Console.WriteLine($"{clientName} подключен.");
            }

            // Цикл для обработки входящих сообщений от клиента
            while (true)
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead <= 0) break;

                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"{clientName}: {message}");

                SendMessageToClients(clientName, message);
            }

            clients.TryRemove(clientName, out _);
            Console.WriteLine($"{clientName} отключен.");
        }
    }

    private static void SendMessageToClients(string sender, string message)
    // Проход по всем подключенным клиентам и отправка им полученного сообщения
    {
        foreach (var client in clients)
        {
            var stream = client.Value.GetStream();
            string msgToSend = $"{sender}: {message}";
            byte[] data = Encoding.UTF8.GetBytes(msgToSend);
            stream.Write(data, 0, data.Length);
        }
    }
}