﻿#region Copyright
//=======================================================================================
// Microsoft Azure Customer Advisory Team 
//
// This sample is supplemental to the technical guidance published on my personal
// blog at http://blogs.msdn.com/b/paolos/. 
// 
// Author: Paolo Salvatori
//=======================================================================================
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// LICENSED UNDER THE APACHE LICENSE, VERSION 2.0 (THE "LICENSE"); YOU MAY NOT USE THESE 
// FILES EXCEPT IN COMPLIANCE WITH THE LICENSE. YOU MAY OBTAIN A COPY OF THE LICENSE AT 
// http://www.apache.org/licenses/LICENSE-2.0
// UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING, SOFTWARE DISTRIBUTED UNDER THE 
// LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY 
// KIND, EITHER EXPRESS OR IMPLIED. SEE THE LICENSE FOR THE SPECIFIC LANGUAGE GOVERNING 
// PERMISSIONS AND LIMITATIONS UNDER THE LICENSE.
//=======================================================================================
#endregion

using System.Threading.Tasks;

using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

using ServiceBusExplorer.Utilities.Helpers;

// ReSharper disable CheckNamespace
namespace ServiceBusExplorer.ServiceBus.Helpers
// ReSharper restore CheckNamespace
{
    using Azure.Core;
    using System;
    using Azure.Identity;

    public class ServiceBusHelper2
    {
        private const string AadScope = "https://management.core.windows.net";
        private const string AuthorityHostUri = "https://login.microsoftonline.com/";
        readonly WriteToLogDelegate writeToLog;

        public string ConnectionString { get; set; }
        public ServiceBusTransportType TransportType { get; set; }

        public WriteToLogDelegate WriteToLog
        {
            get
            {
                return writeToLog;
            }
        }

        public ServiceBusHelper2(WriteToLogDelegate writeToLog)
        {
            this.writeToLog = writeToLog;
        }

        public InteractiveBrowserCredential GetTokenCredential(string tenantId)
        {
            var credentialOption = new InteractiveBrowserCredentialOptions()
            {
                TenantId = tenantId,
                AuthorityHost = new Uri(AuthorityHostUri)
            };

            InteractiveBrowserCredential credential = new InteractiveBrowserCredential(credentialOption);
            return credential;
        }

        public void Connect(string fqn, string tenantId)
        {
            var client = new ServiceBusClient(fqn, GetTokenCredential(tenantId));
            
        }

        public bool ConnectionStringContainsEntityPath()
        {
            var connectionStringProperties = ServiceBusConnectionStringProperties.Parse(ConnectionString);

            return connectionStringProperties?.EntityPath != null;
        }

        /// <summary>
        ///  Dispose of the returned ServiceBusClient object by calling DisposeAsync().
        /// </summary>
        /// <returns>An Azure.Messaging.ServiceBus.ServiceBusClient</returns>
        public ServiceBusClient CreateServiceBusClient()
        {
            return new ServiceBusClient(
                ConnectionString,
                new ServiceBusClientOptions { TransportType = this.TransportType });
        }

        public ServiceBusClient CreateServiceBusClient(string fqns, TokenCredential credential)
        {
            return new ServiceBusClient(fqns, credential);
        }

        public ServiceBusAdministrationClient CreateServiceBusAdministrationClient(string fqns, TokenCredential credential)
        {
            return new ServiceBusAdministrationClient(fqns, credential);
        }

        public async Task<bool> IsPremiumNamespace()
        {
            var administrationClient = new ServiceBusAdministrationClient(ConnectionString);
            NamespaceProperties namespaceProperties = await administrationClient.GetNamespacePropertiesAsync().ConfigureAwait(false);

            return namespaceProperties.MessagingSku == MessagingSku.Premium;
        }

        public async Task<bool> IsQueue(string name)
        {
            var administrationClient = new ServiceBusAdministrationClient(ConnectionString);
            return await administrationClient.QueueExistsAsync(name).ConfigureAwait(false);
        }

        public async Task<bool> IsTopic(string name)
        {
            var administrationClient = new ServiceBusAdministrationClient(ConnectionString);
            return await administrationClient.TopicExistsAsync(name).ConfigureAwait(false);
        }
    }
}
