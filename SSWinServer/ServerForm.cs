using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SSWinServer.Handler;
using SuperSocket.SocketBase;
using Z.Lib.Controls;
using Z.Lib.Entity;
using Z.Lib.Model;

namespace SSWinServer
{
    public partial class ServerForm : Form
    {
        CommonAppServer appServer;
        public ServerForm()
        {
            InitializeComponent();
            Task.Factory.StartNew(() => InitServer());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var _all = appServer.GetAllSessions();
            if (!_all.Any())
            {
                ucLogMessageBox1.PrintShowLogMessage("没有找到已连接的客户端.");
                return;
            }
            _all.All(i =>
            {
                if (i.Connected)
                {
                    var data = textBox1.Text.ToSendData(CommonCommands.Text);
                    i.Send(data);

                }
                return true;
            });
        }

        #region socket server

        void InitServer()
        {
            appServer = new CommonAppServer
            (
                msg => ucLogMessageBox1.PrintShowLogMessage(msg)
            );

            appServer.NewSessionConnected += new SessionHandler<CommonBinarySession>(appServer_NewSessionConnected);
            appServer.SessionClosed += AppServer_SessionClosed;
            //appServer.NewRequestReceived += AppServer_NewRequestReceived;
            ConfigEntity conf = new ConfigEntity();
            //Setup the appServer
            if (!appServer.Setup(conf.SocketServerPort)) //Setup with listening port
            {
                MessageBox.Show("启动失败");
                return;
            }
            //Try to start the appServer
            if (!appServer.Start())
            {
                this.InvokeOnUiThreadIfRequired(()=> toolStripStatusLabel_ServiceStatus.Text = "服务启动失败");
                ucLogMessageBox1.PrintShowLogMessage("服务启动失败. ",MessageType.Error);
                    
            }
            else
            {
                this.InvokeOnUiThreadIfRequired(()=> toolStripStatusLabel_ServiceStatus.Text = "服务已启动");
                ucLogMessageBox1.PrintShowLogMessage("服务启动成功. ");
            }
        }

        private void appServer_NewSessionConnected(CommonBinarySession session)
        {
            //if(!CurrentSession.TryAdd(session.SessionID, session))
            //{
            //    listBox1.SetControlItem(string.Format("[error] {1}({0}) 连接上了服务器.但是当前用户列表已经发现相同的SessionId ", session.SessionID, session.RemoteEndPoint));
            //    session.Close(SuperSocket.SocketBase.CloseReason.Unknown);
            //    return;
            //}
            ucLogMessageBox1.PrintShowLogMessage($" {session.RemoteEndPoint}({session.SessionID}) 连接上了服务器. ");

            var data = "Welcome to Server".ToSendData(CommonCommands.Text);
            session.Send(data);
        }

        private void AppServer_SessionClosed(CommonBinarySession session, SuperSocket.SocketBase.CloseReason value)
        {
            ucLogMessageBox1.PrintShowLogMessage($" {session.RemoteEndPoint}({ session.SessionID}) 断开了连接({value}).");
        }

        #endregion

    }
}
