using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

namespace ConsoleServerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ClusterConfiguration.LocalhostPrimarySilo();
            using (var host = new SiloHost("default", config))
            {
                host.InitializeOrleansSilo();
                host.StartOrleansSilo();
                
                Console.WriteLine("启动成功！");
                Console.ReadLine();
                host.StopOrleansSilo();
            }

        }
    }
}
