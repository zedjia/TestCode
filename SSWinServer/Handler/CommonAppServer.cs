using System;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using Z.Lib.Model;

namespace SSWinServer.Handler
{
    public class CommonAppServer : AppServer<CommonBinarySession, CommonRequestInfo>
    {
        public CommonAppServer(Action<string> action)
            : base(new CommonReceiveFilterFactory())
        {
            PrintAction = action;
        }

        Action<string> PrintAction;
        
        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            return base.Setup(rootConfig, config);
        }

        protected override void OnStarted()
        {
            base.OnStarted();
        }

        protected override void OnStopped()
        {
            base.OnStopped();
        }

        protected override void OnSystemMessageReceived(string messageType, object messageData)
        {
            base.OnSystemMessageReceived(messageType, messageData);
        }

        protected override void ExecuteCommand(CommonBinarySession session, CommonRequestInfo requestInfo)
        {
            
            base.ExecuteCommand(session, requestInfo);
        }

        public void PrintMsg(string msg)
        {
            PrintAction(msg);
        }
        
    }
}
