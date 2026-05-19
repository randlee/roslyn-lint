public static class EndpointFactory
{
    public static string Create()
    {
        const string Fallback = "https://fallback.example.com";
        return Fallback;
    }
}
