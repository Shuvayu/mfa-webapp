using System.Collections.Generic;
using System.Net.Http;

namespace MFA.IInfrastructure
{
    public interface IHttpClientsFactory
    {
        Dictionary<string, HttpClient> Clients();
        HttpClient Client(string key);
    }
}
