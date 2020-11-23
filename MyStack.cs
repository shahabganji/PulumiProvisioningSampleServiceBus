using Pulumi;
using Pulumi.Azure.Core;
using Pulumi.Azure.ServiceBus;

namespace PulumiProvisioningSampleServiceBus
{
    internal  class MyStack : Stack
    {
        public Output<string> FirstTopicName { get; private set; }
        public Output<string> SecondTopicName { get; private set; }
        public Output<string> ServiceBusNamespaceName { get; private set; }
        public Output<string> SampleResourceGroupName { get; private set; }

        public MyStack()
        {
            ProvisioningServiceBus();
            ProvisioningSubscriptions();
        }

        private void ProvisioningServiceBus()
        {
            var resourceGroup = new ResourceGroup("shahab");

            var serviceBusNamespace = new Namespace(
                "sbns-shahab-sample", new NamespaceArgs
                {
                    // Location = "westeurope",
                    ResourceGroupName = resourceGroup.Name,
                    Sku = "Standard",
                    Tags =
                    {
                        {"project", "sample pulumi"},
                    },
                });

            var firstTopic = new Topic("sbt-first",
                new TopicArgs
                {
                    Name = "sbt-first",
                    ResourceGroupName = resourceGroup.Name,
                    NamespaceName = serviceBusNamespace.Name,
                    EnablePartitioning = false,
                });

            var secondTopic = new Topic("sbt-second",
                new TopicArgs
                {
                    Name = "sbt-second",
                    ResourceGroupName = resourceGroup.Name,
                    NamespaceName = serviceBusNamespace.Name,
                    EnablePartitioning = false,
                });

            SampleResourceGroupName = resourceGroup.Name;
            ServiceBusNamespaceName = serviceBusNamespace.Name;
            FirstTopicName = firstTopic.Name;
            SecondTopicName = secondTopic.Name;
        }
        private void ProvisioningSubscriptions()
        {
            var sampleSubscriptionOnFirstTopic =
                new Subscription("sbs-sample-first-topic",
                    new SubscriptionArgs
                    {
                        Name = "sbs-sample",
                        ResourceGroupName = SampleResourceGroupName,
                        NamespaceName = ServiceBusNamespaceName,
                        TopicName = FirstTopicName,
                        MaxDeliveryCount = 5,
                    });

            var sampleSubscriptionOnSecondTopic =
                new Subscription("sbs-sample-second-topic",
                    new SubscriptionArgs
                    {
                        Name = "sbs-sample",
                        ResourceGroupName = SampleResourceGroupName,
                        NamespaceName = ServiceBusNamespaceName,
                        TopicName = SecondTopicName,
                        MaxDeliveryCount = 5,
                    });
        }
    }
}
