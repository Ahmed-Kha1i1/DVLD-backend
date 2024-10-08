public static class ValidationRegex
{
    public const string NationalNoPattern = @"^[A-Za-z0-9\-]+$";
    public const string NamePattern = @"^[A-Za-z0-9\-]+$";
    public const string PhonePattern = @"^[0-9\-\+]+$";
    public const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    public const string PasswordPattern = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$";
}