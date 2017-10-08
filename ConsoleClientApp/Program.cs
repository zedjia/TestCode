using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using InterfacesLib;
using Orleans.Runtime.Configuration;

namespace ConsoleClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ClientConfiguration.LocalhostSilo();
            GrainClient.Initialize(config);
            while (true)
            {
                Console.WriteLine("请输入用户手机号：");
                var mobileNumber = Console.ReadLine();
                //这里由于我们采用的grain继承的是IGrainWithIntegerKey ，所以我们采用调用数值类型的key=10来创建这个grain，
                //可能有人会问key是干嘛的，他是唯一标识这个grain的，当你指定一个key的时候，Orleans 会创建一个，它首先到
                //你的存储介质中找(如果你配置了的话，默认采用内存存储，这种方式适合开发期，生产环境需要保持状态的，所以需要配置到能持久化存储的地方去，比如sqlserver等)
                //如果找到了就直接返回，如果没找到就根据你指定的这个key然后创建一个，这个就是grain的激活，具体详细的，可以看官方问的关于Grain一章。
                var userService = GrainClient.GrainFactory.GetGrain<IUserService>(0);
                
                //C#的一种新的表达式语法，这样就方便多了，省的我们拼接字符串。
                Console.WriteLine($"用户{mobileNumber},{(userService.Exist(mobileNumber).Result ? "已经存在" : "不存在")}");
            }


        }
    }
}
