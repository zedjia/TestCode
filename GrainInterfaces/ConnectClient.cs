using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfacesLib
{
    public class ConnectClientState
    {
        /// <summary>The list of publishers who this user is following</summary>
        public Dictionary<ChirperUserInfo, IChirperPublisher> Subscriptions { get; set; }

        /// <summary>The list of subscribers who are following this user</summary>
        public Dictionary<ChirperUserInfo, IChirperSubscriber> Followers { get; set; }

        /// <summary>Chirp messages recently received by this user</summary>
        public Queue<ChirperMessage> RecentReceivedMessages { get; set; }

        /// <summary>Chirp messages recently published by this user</summary>
        public Queue<ChirperMessage> MyPublishedMessages { get; set; }

        public long UserId { get; set; }

        /// <summary>Alias / username for this actor / user</summary>
        public string UserAlias { get; set; }
    }



    public class ConnectClient
    {
    }
}
