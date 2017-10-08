using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace GrainInterfaces
{
    public interface ISubscriber : IGrainWithIntegerKey
    {
        /// <summary>Notification that a new Chirp has been received</summary>
        /// <param name="chirp">Chirp message entry</param>
        Task NewConnector(MessageInfo chirp);
    }
}
