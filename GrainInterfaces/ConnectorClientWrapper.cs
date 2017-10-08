using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime.Configuration;

namespace GrainInterfaces
{
    public class ConnectorClientWrapper : IViewer
    {
        private IViewer viewer;
        private IConnector publisher;
        private Action<string> MsgPriter;

        public bool IsPublisher { get; private set; }
        public long UserId { get; private set; }


        public ConnectorClientWrapper(long UserId,Action<string> msgpriter)
        {
            MsgPriter = msgpriter;
            this.UserId = UserId; //todo: 需要处理

            if (UserId == 0) throw new ArgumentNullException("UserId", "No user UserId provided");
            PriteMsg($"Client UserId={UserId}");
            Init().Wait();
        }

        async Task Init()
        {
            try
            {
                //var config = ClientConfiguration.LocalhostSilo(30000);
                //GrainClient.Initialize(config);

                IConnector account = GrainClient.GrainFactory.GetGrain<IConnector>(UserId);
                publisher = account;

                string tmp = account.GetTest().Result;

                List<MessageInfo> chirps = await account.GetReceivedMessages(10);

                // Process the most recent chirps received
                foreach (MessageInfo c in chirps)
                {
                    this.NewChirpArrived(c);
                }
                // ... and then subscribe to receive any new chirps
                viewer = await GrainClient.GrainFactory.CreateObjectReference<IViewer>(this);
                if (!this.IsPublisher) PriteMsg($"Listening for new chirps...");
                await account.ViewerConnect(viewer);
                // Sleeps forwever, so Ctrl-C to exit
                Thread.Sleep(-1);
            }
            catch (Exception exc)
            {
                PriteMsg($"Error connecting Chirper client for user={UserId}. Exception:{exc}");
            }
        }


        public Task PublishMessage(string message)
        {
            return publisher.PublishMessage(message);
        }

        #region IChirperViewer interface methods

        public void NewChirpArrived(MessageInfo chirp)
        {
            if (!this.IsPublisher)
            {
                PriteMsg(
                    $"New chirp from @{chirp.PublisherAlias} at {chirp.Timestamp.ToShortTimeString()} on {chirp.Timestamp.ToShortDateString()}: {chirp.Message}");
            }
        }

        public void SubscriptionAdded(ConnectorInfo following)
        {
            PriteMsg(
                $"Added subscription to {following}");
        }

        public void SubscriptionRemoved(ConnectorInfo notFollowing)
        {
            PriteMsg(
                $"Removed subscription to {notFollowing}");
        }

        #endregion

        void PriteMsg(string msg)
        {
            if (MsgPriter != null)
            {
                MsgPriter(msg);
            }
        }


        //public bool ParseArgs(string[] args)
        //{
        //    IsPublisher = false;

        //    if (args.Length <= 0) return false;
        //    bool ok = true;
        //    int argPos = 0;
        //    for (int i = 0; i < args.Length; i++)
        //    {
        //        string a = args[i];
        //        if (a.StartsWith("-") || a.StartsWith("/"))
        //        {
        //            a = a.ToLowerInvariant().Substring(1);
        //            switch (a)
        //            {
        //                case "pub":
        //                    IsPublisher = true;
        //                    break;
        //                case "snap":
        //                case "snapshot":
        //                    this.Snapshot = true;
        //                    break;
        //                case "?":
        //                case "help":
        //                default:
        //                    ok = false;
        //                    break;
        //            }
        //        }
        //        // unqualified arguments below
        //        else if (argPos == 0)
        //        {
        //            long id = 0;
        //            ok = !string.IsNullOrWhiteSpace(a) && long.TryParse(a, out id);
        //            this.UserId = id;
        //            argPos++;
        //        }
        //        else
        //        {
        //            PriteMsg("ERROR: Unknown command line argument: " + a);
        //            ok = false;
        //        }

        //        if (!ok) break;
        //    }
        //    return ok;
        //}

        //public void PrintUsage()
        //{
        //    PriteMsg(Assembly.GetExecutingAssembly().GetName().Name + ".exe [/snapshot] <user ID> ");
        //}
    }
}
