public class ApiConstants
{
    private const string HiddenEndpoint = "https://private.example.com";
    protected const string BaseRoute = "/api";
    private protected const string InternalRoute = "/internal";
    // v1 keeps non-public-or-internal visibilities out of scope, including protected internal.
    protected internal const string SharedRoute = "/shared";
}
