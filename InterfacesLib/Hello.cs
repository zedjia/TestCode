using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace InterfacesLib
{
    public interface IChat : IGrainObserver
    {
        void ReceiveMessage(string message);
    }

    public class Chat : IChat
    {
        private Action<string> printMsg;

        //public Chat(Action<string> fn)
        //{
        //    printMsg = fn;
        //}

        public void ReceiveMessage(string message)
        {
            Console.WriteLine(message);
            if (printMsg != null)
            {
                printMsg(message);
            }
        }
    }
    public interface IHello : IGrainWithIntegerKey
    {
        Task UnSubscribe(IChat observer);
        Task Subscribe(IChat observer);

        Task SendUpdateMessage(string message);
    }

    public class HelloGrain : Grain, IHello
    {
        private ObserverSubscriptionManager<IChat> _subsManager;

        public override async Task OnActivateAsync()
        {
            // We created the utility at activation time.
            _subsManager = new ObserverSubscriptionManager<IChat>();
            await base.OnActivateAsync();
        }

        // Clients call this to subscribe.
        public Task Subscribe(IChat observer)
        {
            _subsManager.Subscribe(observer);
            return Task.CompletedTask;
        }

        //Also clients use this to unsubscribe themselves to no longer receive the messages.
        public Task UnSubscribe(IChat observer)
        {
            _subsManager.Unsubscribe(observer);
            return Task.CompletedTask;
        }
        public Task SendUpdateMessage(string message)
        {
            var key = this.GetPrimaryKey();
            _subsManager.Notify(s => s.ReceiveMessage(message));
            return Task.CompletedTask;
        }
    }


    ////下面就是Grain发送消息给Client的代码
    //var friend = GrainClient.GrainFactory.GetGrain<IHello>(0);
    //Chat c = new Chat();

    //var obj = await GrainClient.GrainFactory.CreateObjectReference<IChat>(c);
    //await friend.Subscribe(obj);

}
