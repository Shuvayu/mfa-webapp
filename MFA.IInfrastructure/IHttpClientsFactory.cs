using System.Collections.Generic;
using System.Net.Http;

namespace MFA.IInfrastructure
{
    public interface IHttpClientsFactory
    {
        /// <summary>
        /// Gets all the http clients stored in the factory
        /// </summary>
        /// <returns></returns>
        Dictionary<string, HttpClient> Clients();

        /// <summary>
        /// Gets the httpclient corrosponding to that key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        HttpClient Client(string key);
    }
}
