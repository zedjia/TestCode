using System.Net;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using Z.Lib.Model;

namespace SSWinServer.Handler
{
    public class CommonReceiveFilterFactory : IReceiveFilterFactory<CommonRequestInfo>
    {
        public IReceiveFilter<CommonRequestInfo> CreateFilter(IAppServer appServer, IAppSession appSession, IPEndPoint remoteEndPoint)
        {
            return new CommonReceiveFilter();
        }
    }
}
