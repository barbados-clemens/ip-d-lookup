// ReSharper disable InconsistentNaming

namespace IpDLookUp.Services.Types
{
    /// <summary>
    /// Service types available to query against. Note SSL Labs requests can take a while to process
    /// </summary>
    public enum ServiceType
    {
        GeoIP,
        RDAP,
        ReverseDNS,
        /// <summary>
        /// SSL Lab reports can take a while to process.
        /// </summary>
        SslLabs,
        Ping
    }

    /// <summary>
    /// Status of the service calls
    /// </summary>
    public enum ServiceStatus
    {
        Unknown,

        /// <summary>
        /// Evertyhing is good. Data property will be set in IServiceResult
        /// </summary>
        Ok,

        /// <summary>
        /// Error processing request. Data will be null
        /// </summary>
        Error,

        /// <summary>
        /// Error processing request because of invalid data provided. Data will be null
        /// </summary>
        Bad,
    }
}