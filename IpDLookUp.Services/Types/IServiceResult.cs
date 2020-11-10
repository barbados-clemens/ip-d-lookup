using System.Text.Json.Serialization;

namespace IpDLookUp.Services.Types
{
    public interface IServiceResult<TModel>
    {
        /// <summary>
        /// There's gotta be a better way than typing "object"
        /// generics cause a lot of cascading, and using interfaces
        /// requires the consumer to "know" there will be other properties;
        /// therefore, still masking the problem
        /// Most straight forward way would be to force all "models" to use the same properties.
        /// would require a translation layer and depends on what the "consumer" what's to know from the service.
        /// If only there were DU's in C# :(
        /// Setting properties on the controller result requires the controller
        /// to know more about each service and it's types breaking the ability to abstract out into "worker machines"
        /// using generics that way "accomplishes" more strict typing, but look and feels all wrong.
        /// The controller shouldn't need to know about the specific methods needed for each service should
        /// just call the "process" method and get back what it wants.
        /// very interesting problem.
        /// specifics would probably come down what the consensus of the consumer wanted.
        /// </summary>
        public TModel Data { get; set; }

        public ServiceStatus Status { get; set; }

        public ServiceType Type { get; set; }

        public string? ErrorMessage { get; set; }

        public string WorkerId { get; set; }

        public long ElapsedMs { get; set; }
    }

    public class ServiceResult<TModel> : IServiceResult<TModel>
    {
        [JsonPropertyName("data")]
        public TModel Data { get; set; }

        [JsonPropertyName("status")]
        public ServiceStatus Status { get; set; }

        [JsonPropertyName("type")]
        public ServiceType Type { get; set; }

        [JsonPropertyName("errorMessage")]
        public string? ErrorMessage { get; set; }

        [JsonPropertyName("workerId")]
        public string WorkerId { get; set; }

        [JsonPropertyName("elapsedMs")]
        public long ElapsedMs { get; set; }
    }
}