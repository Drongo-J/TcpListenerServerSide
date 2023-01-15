using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ServerSideTspListener
{
    public class Program
    {
        static TcpListener listener = null;
        static BinaryWriter bw = null;
        static BinaryReader br = null;


        static void Main(string[] args)
        {
            var ip = IPAddress.Parse("10.2.13.27");
            var port = 27001;

            var ep = new IPEndPoint(ip, port);
            listener = new TcpListener(ep);
            listener.Start();

            Console.WriteLine($"Listening on {listener.LocalEndpoint}");

            while (true)
            {
                var client = listener.AcceptTcpClient();
                Console.WriteLine($"{client.Client.RemoteEndPoint} connected . . .");

                Task.Run(() =>
                {
                    var reader = Task.Run(() =>
                    {
                        var stream = client.GetStream();
                        br = new BinaryReader(stream);
                        while (true)
                        {
                            var message = br.ReadString();
                            Console.WriteLine($"CLIENT : {client.Client.RemoteEndPoint} : {message}");
                        }
                    });

                    var writer = Task.Run(() =>
                    {
                        var stream = client.GetStream();
                        bw = new BinaryWriter(stream);
                        while (true)
                        {
                            var message = Console.ReadLine();
                            bw.Write(message);
                        }
                    });
                });
            }
            
        }
    }
}
