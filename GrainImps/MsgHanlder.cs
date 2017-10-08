using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrainInterfaces;
using Orleans;
using Orleans.Concurrency;
using Orleans.Providers;

namespace GrainImps
{
    public class MsgObserver: IMsgObserver
    {
        public Action<string> printMsg;

        public MsgObserver(Action<string> fn)
        {
            printMsg = fn;
        }

        public void ReceiveMessage(string message)
        {
            Console.WriteLine(message);
            printMsg?.Invoke(message);
        }



    }


    [Reentrant]
    [StorageProvider(ProviderName = "MemoryStore")]
    public class MsgHanlder : Grain, IMsgHandler
    {
        private ObserverSubscriptionManager<IMsgObserver> _subsManager;

        public override async Task OnActivateAsync()
        {
            // We created the utility at activation time.
            _subsManager = new ObserverSubscriptionManager<IMsgObserver>();
            await base.OnActivateAsync();
        }

        public override async Task OnDeactivateAsync()
        {


            //await base.OnDeactivateAsync();
        }

        // Clients call this to subscribe.
        public Task Subscribe(IMsgObserver observer)
        {
            _subsManager.Subscribe(observer);
            return Task.CompletedTask;
        }

        //Also clients use this to unsubscribe themselves to no longer receive the messages.
        public Task UnSubscribe(IMsgObserver observer)
        {
            _subsManager.Unsubscribe(observer);
            return Task.CompletedTask;
        }
        public Task SendMsg(string message)
        {
            var key = this.GetPrimaryKey();
            _subsManager.Notify(s => s.ReceiveMessage(message));
            return Task.CompletedTask;
        }

    }
}
