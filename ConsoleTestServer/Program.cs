using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesLib;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Orleans;
using Orleans.Runtime.Host;

namespace ConsoleTestServer
{
    class Program
    {
        static SiloHost siloHost;
        static void Main(string[] args)
        {
            AppDomain hostDomain = AppDomain.CreateDomain("OrleansHost", null,
                new AppDomainSetup()
                {
                    AppDomainInitializer = InitSilo
                });
            //var heo = GrainClient.GrainFactory.GetGrain<IHello>(0);
            //var read= Console.ReadLine();
            //while (read!="q")
            //{
            //    heo.SendUpdateMessage(read);
            //    Console.WriteLine("\r\n 请输入内容:");
            //    read = Console.ReadLine();
            //}
            Console.ReadLine();
        }


        static void InitSilo(string[] args)
        {
            siloHost = new SiloHost(System.Net.Dns.GetHostName());
            // The Cluster config is quirky and weird to configure in code, so we're going to use a config file
            siloHost.ConfigFileName = "OrleansConfiguration.xml";

            siloHost.InitializeOrleansSilo();
            var startedok = siloHost.StartOrleansSilo();
            if (!startedok)
                throw new SystemException(String.Format("Failed to start Orleans silo '{0}' as a {1} node",
                    siloHost.Name, siloHost.Type));

        }
    }
}
