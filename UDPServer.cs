using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ClientServerApp
{
    class UDPServer : IMathService
    {
        public delegate void MathOperation(byte[] buffer, ref byte[] results, IMathService mathService);
        UdpClient client;
        IPEndPoint endPoint;
        private byte[] buffer;
        public UDPServer()
        {
            client = new UdpClient(3001);
            endPoint = null;
            buffer = new byte[2048];
        }
        public void Run(MathOperation operation)
        {
            byte[] results = null;
            while (true)
            {
                buffer=client.Receive(ref endPoint);
                Task task = new Task(()=> { var endPointClient=endPoint;
                                            operation(buffer, ref results, this);
                                            client.Send(results, results.Length,endPointClient);
                                          });
                task.Start();
                
            }
        }
        public double Add(double firstValue, double secondValue)
        {
            return firstValue + secondValue;
        }

        public double Div(double firstValue, double secondValue)
        {
            if (secondValue != 0)
            {
                return firstValue / secondValue;
            }
            throw new Exception("Numbers are not divided into 0.");
        }

        public double Mult(double firstValue, double secondValue)
        {
            return firstValue * secondValue;
        }

        public double Sub(double firstValue, double secondValue)
        {
            return firstValue - secondValue;
        }
    }
}
