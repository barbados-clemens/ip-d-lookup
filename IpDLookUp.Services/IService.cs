using System.Text.Json;
using System.Threading.Tasks;
using IpDLookUp.Services.Types;

namespace IpDLookUp.Services
{
    public abstract class Service
    {
        public abstract Task<IServiceResult> DoLookUp(string address, AddressType type);

       protected  TModel ParseBody<TModel>(string json) => JsonSerializer.Deserialize<TModel>(json);
    }
}