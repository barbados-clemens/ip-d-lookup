using System.Text.Json;
using System.Threading.Tasks;
using IpDLookUp.Services.Types;

namespace IpDLookUp.Services
{
    public abstract class Service<TModel>
    {
        public abstract Task<IServiceResult<TModel>> DoLookUp(string address, AddressType type);

        protected TModel ParseBody(string json) => JsonSerializer.Deserialize<TModel>(json);
    }
}