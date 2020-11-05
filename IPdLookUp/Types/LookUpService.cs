namespace IPdLookUp.Types
{
    public enum LookUpService
    {
        GeoIP,
        RDAP,
        ReverseDNS,
    }

    public enum LookUpStatus
    {
        Unknown,
        Ok,
        Error,
    }
}