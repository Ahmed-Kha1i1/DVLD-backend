public static class ValidationRegex
{
    public const string NamePattern = @"^[A-Za-z0-9\-]+$";
    public const string PhonePattern = @"^01(0|1|2|5)\d{8}$";
    public const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    public const string PasswordPattern = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$";
}