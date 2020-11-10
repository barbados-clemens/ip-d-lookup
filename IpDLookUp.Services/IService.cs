using System.Text.Json;
using System.Threading.Tasks;
using IpDLookUp.Services.Types;

namespace IpDLookUp.Services
{
    /// <summary>
    /// Base class for each service. Includes helper JSON parser.
    /// </summary>
    /// <typeparam name="TModel">Object type that will be set in IServiceResult<TModel></typeparam>
    public abstract class Service<TModel>
    {
        public abstract Task<IServiceResult<TModel>> DoLookUp(string address, AddressType type);

        protected TModel ParseBody(string json) => JsonSerializer.Deserialize<TModel>(json);
    }
}