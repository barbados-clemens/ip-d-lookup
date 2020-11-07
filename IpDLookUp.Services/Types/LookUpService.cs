namespace IpDLookUp.Services.Types
{
    public enum ServiceType
    {
        GeoIP,
        RDAP,
        ReverseDNS,
        SslLabs,
        Ping
    }

    public enum ServiceStatus
    {
        Unknown,
        Ok,
        Error,
    }
}