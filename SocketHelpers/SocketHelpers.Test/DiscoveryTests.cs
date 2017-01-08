using System;
using System.Collections.Generic;
using System.Threading;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SocketHelpers.Discovery;

namespace SocketHelpers.Test
{
    [TestClass]
    public class DiscoveryTests
    {
        [TestMethod]
        public void PublishedServerIsVisibleToDiscoverer()
        {
            var discoveryRequests = new List<string>();
            var serviceDefinition = new FuncyJsonServiceDefinition<string, TestDiscoveryFormat>
            {
                ResponseForRequestFunc = s =>
                {
                    discoveryRequests.Add(s);
                    return new TestDiscoveryFormat("127.0.0.1", 9878);
                },
                DiscoveryRequestFunc = () => "Test",
                DiscoveryPort = 9876,
                ResponsePort = 9877,
            };

            var publisher = serviceDefinition.CreateServicePublisher();
            publisher.Publish().Wait();
            
            var discoverer = serviceDefinition.CreateServiceDiscoverer();
            var foundServices = new List<TestDiscoveryFormat>();

            using (discoverer.DiscoveredServices.Subscribe(format =>
            {
                foundServices.Add(format);
            }))
            {
                discoverer.Discover().Wait();

                // Wait for events to be processed
                Thread.Sleep(500);
            }

            discoveryRequests.Should().NotBeEmpty("a discovery request should have been recieved.");
            discoveryRequests.Should().Contain(s => s == "Test", "the incorrect discovery string was received.");

            foundServices.Should().NotBeEmpty("a service should have been found.");
        }
    }
}
