using Pulumi;
using Pulumi.Azure.Core;

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

            var serviceBusNamespace = new Pulumi.Azure.ServiceBus.Namespace(
                "sbns-shahab-sample", new Pulumi.Azure.ServiceBus.NamespaceArgs
                {
                    // Location = "westeurope",
                    ResourceGroupName = resourceGroup.Name,
                    Sku = "Standard",
                    Tags =
                    {
                        {"project", "sample pulumi"},
                    },
                });

            var firstTopic = new Pulumi.Azure.ServiceBus.Topic("sbt-first",
                new Pulumi.Azure.ServiceBus.TopicArgs
                {
                    Name = "sbt-first",
                    ResourceGroupName = resourceGroup.Name,
                    NamespaceName = serviceBusNamespace.Name,
                    EnablePartitioning = false,
                });

            var secondTopic = new Pulumi.Azure.ServiceBus.Topic("sbt-second",
                new Pulumi.Azure.ServiceBus.TopicArgs
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
                new Pulumi.Azure.ServiceBus.Subscription("sbs-sample-first-topic",
                    new Pulumi.Azure.ServiceBus.SubscriptionArgs
                    {
                        Name = "sbs-sample",
                        ResourceGroupName = SampleResourceGroupName,
                        NamespaceName = ServiceBusNamespaceName,
                        TopicName = FirstTopicName,
                        MaxDeliveryCount = 5,
                    });

            var sampleSubscriptionOnSecondTopic =
                new Pulumi.Azure.ServiceBus.Subscription("sbs-sample-second-topic",
                    new Pulumi.Azure.ServiceBus.SubscriptionArgs
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
