using System.Net.Http;
using System.Threading.Tasks;

namespace MFA.IInfrastructure
{
    public interface IHttpProvider
    {
        Task<HttpResponseMessage> GetAsync(string requestUri);
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
        Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content);
        Task<HttpResponseMessage> DeleteAsync(string requestUri);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestObject);
    }
}
