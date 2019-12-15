using System;
using System.Collections.Generic;
using System.Text;
using Dister.Net.Communication;
using Dister.Net.Serialization;
using Dister.Net.Communication.Message;
using Dister.Net.Variables;
using System.Linq;

namespace Dister.Net.Service
{
    public class ServiceBuilder<T> where T : DisterService<T>
    {
        readonly T service;
        Communicator<T> communicator;
        ISerializer serializer;
        DisterVariablesController<T> disterVariablesController;

        public ServiceBuilder(T service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }
        public ServiceBuilder<T> WithSerializer(ISerializer serializer)
        {
            this.serializer = serializer;
            return this;
        }
        public ServiceBuilder<T> WithCommunicator(Communicator<T> communicator)
        {
            this.communicator = communicator;
            return this;

        }
        public ServiceBuilder<T> WithMessageHandler<TM>(string topic, Func<object, T, object> handler)
        {
            service.MessageHandlers.Add(topic, typeof(TM), handler);
            return this;
        }
        public ServiceBuilder<T> WithMessageHandler<TM>(string topic, Action<object, T> noResponseHandler)
        {
            service.MessageHandlers.Add(topic, typeof(TM), (o, master) => { noResponseHandler(o, master); return null; });
            return this;
        }
        public ServiceBuilder<T> InLoop(bool inLoop = true)
        {
            service.InLoop = inLoop;
            return this;
        }
        public ServiceBuilder<T> WithDisterVariableController(DisterVariablesController<T> disterVariablesController)
        {
            this.disterVariablesController = disterVariablesController;
            return this;
        }
        public ServiceBuilder<T> WithDisterVariable<TV>(string name, TV value = default)
        {
            disterVariablesController.SetDisterVariable(name, value);//TODO fix it
            return this;
        }
        public ServiceBuilder<T> WithDisterQueue<TV>(string name)
        {
            disterVariablesController.AddQueue(name);
            return this;
        }
        public ServiceBuilder<T> WithDisterQueue<TV>(string name, TV[] values)
        {
            var objects = values.Select(x => (object)x).ToArray();
            disterVariablesController.AddQueue(name, objects);
            return this;
        }
        public void Run()
        {
            service.Serializer = serializer;
            service.MessageHandlers = new MessageHandlers<T>(service, serializer);
            service.Communicator = communicator;
            service.DisterVariablesController = disterVariablesController;

            communicator.service = service;
            disterVariablesController.service = service;

            communicator.Start();

            service.Run();
            while (service.InLoop)
                service.Run();
        }
    }
}
