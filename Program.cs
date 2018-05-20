using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Choose Tcp Or Udp Protocol.");
            byte[] buffer = new byte[2048];
            Random rnd = new Random();
            int port = rnd.Next(3002, 4000);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, port);
                var tcpOrudp = Console.ReadLine();

            if (tcpOrudp.ToUpper() == "TCP")
            {
                TcpClient client = new TcpClient(endPoint);

                client.Connect(IPAddress.Loopback, 3000);
                var stream = client.GetStream();
                while (true)
                {
                    var bytes = Reading();
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Read(buffer, 0, buffer.Length);

                    Console.WriteLine(Encoding.Unicode.GetString(buffer));
                }

            }
            else if (tcpOrudp.ToUpper() == "UDP")
            {
                UdpClient client = new UdpClient();

                client.Connect(IPAddress.Loopback, 3001);
                while (true)
                {
                    var bytes = Reading();
                    client.Send(bytes, bytes.Length);
                    buffer = client.Receive(ref endPoint);
                    Console.WriteLine(Encoding.Unicode.GetString(buffer));
                }
            }
            else
            {
                Console.WriteLine("You must choose either TCP or UDP.");
            }

        }
        
        static byte[] Reading()
        {
            Console.WriteLine("Write the  operation.");
            var operation = Console.ReadLine();
            return  Encoding.Unicode.GetBytes(operation);
        }
    }
}
