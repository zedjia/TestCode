using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace GrainInterfaces
{

    public interface IMsgObserver : IGrainObserver
    {
        void ReceiveMessage(string message);

    }


    public interface IMsgHandler: IGrainWithIntegerKey
    {
        Task UnSubscribe(IMsgObserver observer);
        Task Subscribe(IMsgObserver observer);

        Task SendMsg(string message);
    }
}
