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

namespace WindowsFormsClient
{
    public partial class ClientForm : Form
    {
        private ConnectorClientWrapper ClientWrapper;
        public bool IsPublisher { get; private set; }
        public long UserId { get; private set; }

        private IMsgHandler friend;
        private IMsgObserver observer;
        public ClientForm()
        {
            InitializeComponent();
            InitOrleans();

        }

        void InitOrleans()
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
                    //var friend = GrainClient.GrainFactory.GetGrain<IHello>(0);
                    //var result = friend.SayHello(textBox1.Text).Result;
                    //ClientWrapper = new ConnectorClientWrapper(1,(msg) => { ucLogMessageBox1.PrintShowLogMessage(msg); });
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

            //ClientWrapper.PublishMessage(textBox1.Text).Wait();
            //var friend = GrainClient.GrainFactory.GetGrain<IHello>(0);
            //var result = friend.SayHello(textBox1.Text).Result;
            //ucLogMessageBox1.PrintShowLogMessage(result);
            //friend.SendUpdateMessage(textBox1.Text);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            //ClientWrapper.
            //friend = GrainClient.GrainFactory.GetGrain<IHello>((new Random().Next(1, 99999)));
            //Chat c = new Chat();//msg => ucLogMessageBox1.PrintShowLogMessage(msg)

            //var obj = await GrainClient.GrainFactory.CreateObjectReference<IChat>(c);
            //await friend.Subscribe(obj);
            //this.button3.Enabled = false;
        }




    }
}
