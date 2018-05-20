using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ClientServerApp
{
    public class TCPServer : IMathService
    {
        public delegate void MathOperation(byte[] buffer,  ref byte[] results, IMathService mathService);
        TcpListener tcpListener;
        private byte[] buffer;
        public TCPServer()
        {
            this.tcpListener = new TcpListener(IPAddress.Any, 3000);
            this.buffer = new byte[2048];
            this.tcpListener.Start(100);
        }
        public void Run(MathOperation operation)
        {
            byte[] results = null;
            while (true)
            {
                var clinet = tcpListener.AcceptTcpClient();
                
                
                Task task = new Task(() => {
                    var data = clinet.GetStream();
                    while (true)
                    {
                        var el = data.Read(buffer, 0, buffer.Length);
                        operation(buffer, ref results, this);
                        data.Write(results, 0, results.Length);
                    }
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
