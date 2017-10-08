using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuperSocket.ClientEngine;
using SuperSocket.ProtoBase;
using Z.Lib.Controls;
using Z.Lib.Entity;
using Z.Lib.Model;
using Timer = System.Timers.Timer;

namespace SSWinClient
{
    public partial class ClientForm : Form
    {
        CommonSocketClient client;
        private ConfigEntity Conf;
        private Timer Reconnecttimer;
        public ClientForm()
        {
            InitializeComponent();
            InitSocket();
            Reconnecttimer = new Timer(5000);
            Reconnecttimer.Elapsed += Timer_Elapsed;
            //Reconnecttimer.Enabled = true;
            //Reconnecttimer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ReconnectServer();
        }

        #region socket
        async void InitSocket()
        {
            client = new CommonSocketClient();
            client.Error += Client_Error;
            client.Closed += Client_Closed;
            client.Connected += Client_Connected;
            // Initialize the client with the receive filter and request handler
            client.Initialize(new MyReceiveFilter(), (request) =>
            {
                ucLogMessageBox1.PrintShowLogMessage(Encoding.UTF8.GetString(request.Body));
            });
            Conf = new ConfigEntity();
            await client.ConnectAsync(new IPEndPoint(IPAddress.Parse(Conf.SocketServerIp), Conf.SocketServerPort));
        }

        private void Client_Connected(object sender, EventArgs e)
        {
            Reconnecttimer.Stop();
            this.InvokeOnUiThreadIfRequired(() => toolStripStatusLabel_ServerStatus.Text = "服务已连接");
            ucLogMessageBox1.PrintShowLogMessage("服务器连接成功.", MessageType.Error);
        }

        private void Client_Closed(object sender, EventArgs e)
        {
            this.InvokeOnUiThreadIfRequired(() => toolStripStatusLabel_ServerStatus.Text = "服务未连接");
            ucLogMessageBox1.PrintShowLogMessage("断开了连接.", MessageType.Error);
            Reconnecttimer.Start();
            ucLogMessageBox1.PrintShowLogMessage("等待重新连接服务器.");

        }

        private void Client_Error(object sender, ErrorEventArgs e)
        {
            this.InvokeOnUiThreadIfRequired(() => toolStripStatusLabel_ServerStatus.Text = "服务未连接");
            ucLogMessageBox1.PrintShowLogMessage(e.Exception.Message,MessageType.Error);
            Reconnecttimer.Start();
            ucLogMessageBox1.PrintShowLogMessage("等待重新连接服务器.");
        }

        async Task ReconnectServer()
        {
            try
            {
                await client.ConnectAsync(new IPEndPoint(IPAddress.Parse(Conf.SocketServerIp), Conf.SocketServerPort));
            }
            catch (Exception e)
            {
                Reconnecttimer.Stop();
                ucLogMessageBox1.PrintShowLogMessage($"重新连接服务器出错,请重新启动程序.\r\n {e.Message}",MessageType.Error);
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            if (client != null)
            {
                if (client.IsConnected)
                {
                    var sendData = textBox1.Text.ToSendData(CommonCommands.Text);
                    client.Send(sendData);
                }
            }
        }
        
        
    }

    public class MyReceiveFilter : FixedHeaderReceiveFilter<CommonRequestInfo>
    {
        private int HeaderSize { get; set; }

        public MyReceiveFilter()
            : base(14) // two vertical bars as package terminator
        {
            HeaderSize = 14;
        }


        //public override BinaryRequestInfo ResolvePackage(IBufferStream bufferStream)
        //{
        //    //bufferStream.
        //    //bufferStream.

        //    byte[] b = new byte[bufferStream.Length];
        //    bufferStream.Read(b, 0, b.Length);
        //    //string content = Encoding.UTF8.GetString(b);
        //    var model = b.BytesToObject<DataPackage>();
        //    return new BinaryRequestInfo(model.Name, model.Other, null);
        //    //return base.ResolvePackage(bufferStream);
        //}

        // other code you need implement according yoru protocol details
        public override CommonRequestInfo ResolvePackage(IBufferStream bufferStream)
        {
            //var info = new CommonRequestInfo();
            var stream = bufferStream.GetCurrentStream();
            byte[] content = new byte[stream.Length];
            stream.Read(content, 0, (int)stream.Length);

            byte[] ary = new byte[stream.Length - HeaderSize];
            Array.Copy(content, HeaderSize, ary, 0, ary.Length);
            //stream.Read(ary, 0, (int)stream.Length - 8);
            //var package= ary.BytesToStruct<DataPackage>();
            //info.Body = package.Body;
            //info.Key = package.Name;
            //info.Length = package.Length;
            //info.Total = package.Total;
            var info = new CommonRequestInfo(content);
            return info.IsValid ? info : null;

            //return new CommonRequestInfo(Encoding.UTF8.GetString(header.Array, header.Offset, 4), bodyBuffer.CloneRange(offset, length));
        }

        protected override int GetBodyLengthFromHeader(IBufferStream bufferStream, int length)
        {
            byte[] header = new byte[bufferStream.Length];
            bufferStream.Read(header, 0, (int)bufferStream.Length);
            var headerData = new byte[4];
            Array.Copy(header, 6, headerData, 0, 4);
            return BitConverter.ToInt32(headerData, 0) + 2;
        }
    }

}
