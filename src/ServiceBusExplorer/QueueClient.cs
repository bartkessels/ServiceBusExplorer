using Azure.Messaging.ServiceBus;
using Microsoft.Azure.NotificationHubs;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace ServiceBusExplorer
{
    public class QueueClient
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly string _path;
        private readonly ServiceBusReceiveMode _serviceBusReceiveMode;
        public QueueClient(ServiceBusClient serviceBusClient, string path, ServiceBusReceiveMode serviceBusReceiveMode)
        {
            _serviceBusClient = serviceBusClient;
            _path = path;
            _serviceBusReceiveMode = serviceBusReceiveMode;
        }
    }
}
