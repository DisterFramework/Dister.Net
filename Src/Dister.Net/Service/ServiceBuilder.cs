using System;
using System.Collections.Generic;
using System.Linq;
using Dister.Net.Communication;
using Dister.Net.Communication.Message;
using Dister.Net.Exceptions.ServiceBuilderExceptions;
using Dister.Net.Logs;
using Dister.Net.Serialization;
using Dister.Net.Variables.DiserVariables;

namespace Dister.Net.Service
{
    public class ServiceBuilder<T> where T : DisterService<T>
    {
        private readonly T service;
        private Communicator<T> communicator;
        private ISerializer serializer;
        private DisterVariablesController<T> disterVariablesController;

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
            if (disterVariablesController == null) throw new DisterVariableControllerNotSetException();

            disterVariablesController.SetDisterVariable(name, value);//TODO fix it
            return this;
        }
        public ServiceBuilder<T> WithDisterQueue<TV>(string name)
        {
            if (disterVariablesController == null) throw new DisterVariableControllerNotSetException();

            disterVariablesController.AddQueue(name);
            return this;
        }
        public ServiceBuilder<T> WithDisterQueue<TV>(string name, TV[] values)
        {
            if (disterVariablesController == null) throw new DisterVariableControllerNotSetException();

            var objects = values.Select(x => (object)x).ToArray();
            disterVariablesController.AddQueue(name, objects);
            return this;
        }
        public ServiceBuilder<T> WithDisterDictionary<TK, TV>(string name)
        {
            if (disterVariablesController == null) throw new DisterVariableControllerNotSetException();
            disterVariablesController.AddDictionary(name);
            return this;
        }
        public ServiceBuilder<T> WithDisterDictionary<TK, TV>(string name, Dictionary<TK, TV> values)
        {
            if (disterVariablesController == null) throw new DisterVariableControllerNotSetException();
            var dict = values.Select(x => new KeyValuePair<object, object>(x.Key, x.Value)).ToDictionary(x => x.Key, x => x.Value);
            disterVariablesController.AddDictionary(name, dict);
            return this;
        }
        public ServiceBuilder<T> WithExceptionHandler<TE>(Action<Exception, T> handler) where TE : Exception
        {
            service.ExceptionHanlders.Add(typeof(TE), handler);
            return this;
        }
        public ServiceBuilder<T> WithLogAggregator(LogAggregator<T> logAggregator)
        {
            service.LogAggregator = logAggregator;
            logAggregator.service = service;
            return this;
        }

        public void Run()
        {

            service.Serializer = serializer ?? throw new SerializerNotSetException();
            service.Communicator = communicator ?? throw new CommunicatorNotSetException();
            service.DisterVariablesController = disterVariablesController ?? throw new DisterVariableControllerNotSetException();
            service.MessageHandlers.Service = service;
            service.MessageHandlers.DService = service;
            service.MessageHandlers.Service = service;
            service.ExceptionHanlders.Service = service;

            communicator.service = service;
            disterVariablesController.service = service;

            communicator.Start();

            service.Start();
        }
    }
}
