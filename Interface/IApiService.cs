using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hestia_Maui.Interface
{
    public interface IApiService
    {
        Task<HttpResponseMessage> ApiPostAsync<TRequest>(string endpoint, TRequest data);
        Task<HttpResponseMessage> ApiPutAsync<TRequest>(string endpoint, TRequest data);
        Task<List<T>> ApiGetAll<T>(string endpoint);
    }
}
