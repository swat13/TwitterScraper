using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Host
{
    public class CORSEnablingBehavior : BehaviorExtensionElement, IEndpointBehavior
    {
        public void AddBindingParameters(
            ServiceEndpoint endpoint,
            BindingParameterCollection bindingParameters)
        { }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime) { }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(
                new CorsHeaderInjectingMessageInspector()
            );
        }

        public void Validate(ServiceEndpoint endpoint) { }

        public override Type BehaviorType => typeof(CORSEnablingBehavior);

        protected override object CreateBehavior() { return new CORSEnablingBehavior(); }

        private class CorsHeaderInjectingMessageInspector : IDispatchMessageInspector
        {
            public object AfterReceiveRequest(
                ref Message request,
                IClientChannel channel,
                InstanceContext instanceContext)
            {
                return null;
            }

            private static readonly IDictionary<string, string> HeadersToInject = new Dictionary<string, string>
            {
                { "Access-Control-Allow-Origin", "*" },
                { "Access-Control-Request-Method", "POST,GET,PUT,DELETE,OPTIONS" },
                { "Access-Control-Allow-Headers", "X-Requested-With,Content-Type" }
            };

            public void BeforeSendReply(ref Message reply, object correlationState)
            {
                var httpHeader = reply.Properties["httpResponse"] as HttpResponseMessageProperty;
                foreach (var item in HeadersToInject)
                {
                    httpHeader?.Headers.Add(item.Key, item.Value);
                }
            }
        }
    }

}
