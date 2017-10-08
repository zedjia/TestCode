using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesLib;
using Orleans;
using Orleans.Runtime.Configuration;

namespace ConsoleTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ClientConfiguration.LocalhostSilo(30000);
            GrainClient.Initialize(config);

            //var  friend = GrainClient.GrainFactory.GetGrain<IChat>((new Random().Next(1, 99999)));

            var  friend = GrainClient.GrainFactory.GetGrain<IHello>(0);
            Chat c = new Chat();

            var obj = GrainClient.GrainFactory.CreateObjectReference<IChat>(c).Result;
            friend.Subscribe(obj).Wait();

            string line = Console.ReadLine();
            while (line != null && line.ToLower().Trim() != "quit")
            {
                friend.SendUpdateMessage(line).Wait();
                Console.Write("\r\n Enter a comment: ");
                line = Console.ReadLine();
            }

        }
    }
}
