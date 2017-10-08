using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrainImps;
using GrainInterfaces;
using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

namespace WinFormsServer
{
    public partial class ServerForm : Form
    {
        static SiloHost siloHost;
        private IMsgHandler friend;
        private IMsgObserver observer;
        public ServerForm()
        {
            InitializeComponent();

            //ucLogMessageBox1.PrintShowLogMessage();
            Init();
            InitClient();
        }

        void Init()
        {
            // Orleans should run in its own AppDomain, we set it up like this
            AppDomain hostDomain = AppDomain.CreateDomain("OrleansHost", null,
                new AppDomainSetup()
                {
                    AppDomainInitializer = InitSilo
                });
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
        void ShutdownSilo()
        {
            if (siloHost != null)
            {
                siloHost.Dispose();
                GC.SuppressFinalize(siloHost);
                siloHost = null;
            }
        }

        void InitClient()
        {
            try
            {
                // Orleans comes with a rich XML and programmatic configuration. Here we're just going to set up with basic programmatic config
                var config = ClientConfiguration.LocalhostSilo(30000);
                GrainClient.Initialize(config);
                if (GrainClient.IsInitialized)
                {
                    friend = GrainClient.GrainFactory.GetGrain<IMsgHandler>(0);
                    observer = new MsgObserver((msg) => MsgReceiver(msg));
                    observer = GrainClient.GrainFactory.CreateObjectReference<IMsgObserver>(observer).Result;
                    friend.Subscribe(observer).Wait();
                }
            }
            catch (Exception ex)
            {
                ucLogMessageBox1.PrintShowLogMessage(ex.Message);
                return;
            }
            ucLogMessageBox1.PrintShowLogMessage("客户端初始化成功.");

        }

        void MsgReceiver(string msg)
        {
            ucLogMessageBox1.PrintShowLogMessage(msg);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            friend = GrainClient.GrainFactory.GetGrain<IMsgHandler>(0);
            await friend.SendMsg(textBox1.Text.Trim());
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ShutdownSilo();
        }

       

        private async void button4_Click(object sender, EventArgs e)
        {
            //var config = Orleans.Runtime.Configuration.ClientConfiguration.LocalhostSilo(30000);
            //GrainClient.Initialize(config);
            //friend = GrainClient.GrainFactory.GetGrain<IHello>((new Random().Next(1, 99999)));
            //Chat c = new Chat();//msg => ucLogMessageBox1.PrintShowLogMessage(msg)

            //var obj = await GrainClient.GrainFactory.CreateObjectReference<IChat>(c);
            //await friend.Subscribe(obj);
            //this.button4.Enabled = false;
        }
    }


}
