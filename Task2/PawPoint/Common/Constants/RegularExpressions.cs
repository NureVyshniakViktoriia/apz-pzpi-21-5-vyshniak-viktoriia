namespace Common.Constants;
public static class RegularExpressions
{
    public const string LOGIN = @"^[a-zA-Z0-9_-]{4,32}$";

    public const string PASSWORD = @"^[a-zA-Z0-9!@#$%^&*-_]{4,32}$";

    public const string PHONE_NUMBER = @"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$";

    public const string BASE64_REGEX = @"^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$";
}
