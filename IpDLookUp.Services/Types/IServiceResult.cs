namespace IpDLookUp.Services.Types
{
    public interface IServiceResult
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
        public object Data { get; set; }

        public ServiceStatus Status { get; set; }

        public ServiceType Type { get; set; }

        public string? ErrorMessage { get; set; }
    }

    public struct ServiceResult : IServiceResult
    {
        public object Data { get; set; }

        public ServiceStatus Status { get; set; }

        public ServiceType Type { get; set; }

        public string? ErrorMessage { get; set; }
    }
}