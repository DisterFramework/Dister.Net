using System;
using System.Collections.Generic;
using System.Linq;
using Dister.Net.Communication;
using Dister.Net.Exceptions.ServiceBuilderExceptions;
using Dister.Net.Logs;
using Dister.Net.Modules;
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
        private readonly List<Module<T>> modules = new List<Module<T>>();

        /// <summary>
        /// Creates <see cref="DisterService{T}"/> of type T
        /// </summary>
        /// <param name="service">Service instance</param>
        public ServiceBuilder(T service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            modules.Add(service.ExceptionHanlders);
            modules.Add(service.MessageHandlers);
        }
        /// <summary>
        /// Adds Serializer to <see cref="DisterService{T}"/>
        /// </summary>
        /// <param name="serializer">Instance of serializer</param>
        /// <returns></returns>
        public ServiceBuilder<T> WithSerializer(ISerializer serializer)
        {
            this.serializer = serializer;
            return this;
        }
        /// <summary>
        /// Adds Communicator to <see cref="DisterService{T}"/>
        /// </summary>
        /// <param name="communicator">Communicator instance</param>
        /// <returns></returns>
        public ServiceBuilder<T> WithCommunicator(Communicator<T> communicator)
        {
            this.communicator = communicator;
            modules.Add(communicator);
            return this;

        }
        /// <summary>
        /// Adds MessageHandler to <see cref="DisterService{T}"/>
        /// </summary>
        /// <typeparam name="TM">Message type</typeparam>
        /// <param name="topic">Message topic</param>
        /// <param name="handler">Handler function</param>
        /// <returns></returns>
        public ServiceBuilder<T> WithMessageHandler<TM>(string topic, Func<object, T, object> handler)
        {
            service.MessageHandlers.Add(topic, typeof(TM), handler);
            return this;
        }
        /// <summary>
        /// Adds MessageHandler to <see cref="DisterService{T}"/>
        /// </summary>
        /// <typeparam name="TM">Message type</typeparam>
        /// <param name="topic">Message topic</param>
        /// <param name="noResponseHandler">Handler function</param>
        /// <returns></returns>
        public ServiceBuilder<T> WithMessageHandler<TM>(string topic, Action<object, T> noResponseHandler)
        {
            service.MessageHandlers.Add(topic, typeof(TM), (o, master) => { noResponseHandler(o, master); return null; });
            return this;
        }
        /// <summary>
        /// Makes <see cref="DisterService{T}"/> run in endless loop
        /// </summary>
        /// <param name="inLoop">Run in loop</param>
        /// <returns></returns>
        public ServiceBuilder<T> InLoop(bool inLoop = true)
        {
            service.InLoop = inLoop;
            return this;
        }
        /// <summary>
        /// Add DisterVariable Controller
        /// </summary>
        /// <param name="disterVariablesController">DisterVariablesController instance</param>
        /// <returns></returns>
        public ServiceBuilder<T> WithDisterVariableController(DisterVariablesController<T> disterVariablesController)
        {
            this.disterVariablesController = disterVariablesController;
            modules.Add(disterVariablesController);
            return this;
        }
        /// <summary>
        /// Creates DisterVariable on DisterService startup
        /// </summary>
        /// <typeparam name="TV">Variable type</typeparam>
        /// <param name="name">Variable name</param>
        /// <param name="value">Variable value</param>
        /// <returns></returns>
        public ServiceBuilder<T> WithDisterVariable<TV>(string name, TV value = default)
        {
            if (disterVariablesController == null) throw new DisterVariableControllerNotSetException();

            disterVariablesController.SetDisterVariable(name, value);
            return this;
        }
        /// <summary>
        /// Creates DisterQueue on DisterService startup
        /// </summary>
        /// <typeparam name="TV">Queue type</typeparam>
        /// <param name="name">Queue name</param>
        /// <returns></returns>
        public ServiceBuilder<T> WithDisterQueue<TV>(string name)
        {
            if (disterVariablesController == null) throw new DisterVariableControllerNotSetException();

            disterVariablesController.AddQueue(name);
            return this;
        }
        /// <summary>
        /// Creates DisterQueue with values on DisterService startup
        /// </summary>
        /// <typeparam name="TV">Queue type</typeparam>
        /// <param name="name">Queue name</param>
        /// <param name="values">Queue initial values</param>
        /// <returns></returns>
        public ServiceBuilder<T> WithDisterQueue<TV>(string name, TV[] values)
        {
            if (disterVariablesController == null) throw new DisterVariableControllerNotSetException();

            var objects = values.Select(x => (object)x).ToArray();
            disterVariablesController.AddQueue(name, objects);
            return this;
        }
        /// <summary>
        /// Creates DisterDictionary on DisterService startup
        /// </summary>
        /// <typeparam name="TV">Dictionary values type</typeparam>
        /// <typeparam name="TK">Dictionary keys type</typeparam>
        /// <param name="name">Dictionary name</param>
        /// <returns></returns>
        public ServiceBuilder<T> WithDisterDictionary<TK, TV>(string name)
        {
            if (disterVariablesController == null) throw new DisterVariableControllerNotSetException();
            disterVariablesController.AddDictionary(name);
            return this;
        }
        /// <summary>
        /// Creates DisterDictionary with values on DisterService startup
        /// </summary>
        /// <typeparam name="TV">Dictionary values type</typeparam>
        /// <typeparam name="TK">Dictionary keys type</typeparam>
        /// <param name="values">Dictionary initial values</param>
        /// <param name="name">Dictionary name</param>
        /// <returns></returns>
        public ServiceBuilder<T> WithDisterDictionary<TK, TV>(string name, Dictionary<TK, TV> values)
        {
            if (disterVariablesController == null) throw new DisterVariableControllerNotSetException();
            var dict = values.Select(x => new KeyValuePair<object, object>(x.Key, x.Value)).ToDictionary(x => x.Key, x => x.Value);
            disterVariablesController.AddDictionary(name, dict);
            return this;
        }
        /// <summary>
        /// Adds Exception Handler to DisterService
        /// </summary>
        /// <typeparam name="TE">Exception type</typeparam>
        /// <param name="handler">Handler function</param>
        /// <returns></returns>
        public ServiceBuilder<T> WithExceptionHandler<TE>(Action<Exception, T> handler) where TE : Exception
        {
            service.ExceptionHanlders.Add(typeof(TE), handler);
            return this;
        }
        /// <summary>
        /// Adds LogAggregator to DisterService
        /// </summary>
        /// <param name="logAggregator">LogAggregator instance</param>
        /// <returns></returns>
        public ServiceBuilder<T> WithLogAggregator(LogAggregator<T> logAggregator)
        {
            service.LogAggregator = logAggregator;
            modules.Add(logAggregator);
            return this;
        }
        /// <summary>
        /// Adds module to DisterService
        /// </summary>
        /// <param name="module">Module instance</param>
        /// <returns></returns>
        public ServiceBuilder<T> WithModule(Module<T> module)
        {
            modules.Add(module);
            return this;
        }
        /// <summary>
        /// Starts DisterService
        /// </summary>
        public void Run()
        {
            service.Serializer = serializer ?? throw new SerializerNotSetException();
            service.Communicator = communicator ?? throw new CommunicatorNotSetException();
            service.DisterVariablesController = disterVariablesController ?? throw new DisterVariableControllerNotSetException();

            foreach (var module in modules)
            {
                module.service = service;
                module.disterService = service;
                module.Start();
            }

            service.Start();
        }
    }
}
