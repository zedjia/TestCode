using System.Collections.Concurrent;
using System.Collections.Generic;
using SuperSocket.SocketBase;
using Z.Lib.Model;

namespace SSWinServer.Handler
{
    public class CommonBinarySession : AppSession<CommonBinarySession, CommonRequestInfo>
    {
        
        private CommonAppServer _server;

        private CommonAppServer Server
        {
            get
            {
                if (_server == null)
                {
                    _server = this.AppServer as CommonAppServer;
                }
                return _server;
            }
        }

        public ConcurrentDictionary<string, List<FileRequestInfo>> FileDataBuffer = new ConcurrentDictionary<string, List<FileRequestInfo>>();

        public virtual void Send(byte[] data)
        {
            Send(data, 0, data.Length);
        }

        protected override void OnSessionStarted()
        {
            base.OnSessionStarted();
            Server.PrintMsg("客户端连接了服务器...");
        }

        protected override void OnSessionClosed(CloseReason reason)
        {
            base.OnSessionClosed(reason);
            Server.PrintMsg($"客户端断开了链接-{reason.ToString()}...");

        }
    }
}
