internal static class SwitchArmLiteral
{
    public static string Resolve(int input)
    {
        return input switch
        {
            0 => "atm-core",
            _ => "safe",
        };
    }
}
