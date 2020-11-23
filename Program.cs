using System.Threading.Tasks;
using Pulumi;
using PulumiProvisioningSampleServiceBus;

class Program
{
    static Task<int> Main() => Deployment.RunAsync<MyStack>();
}
