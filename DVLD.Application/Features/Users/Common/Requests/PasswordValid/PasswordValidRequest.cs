namespace DVLD.Application.Features.Users.Common.Requests.PasswordValid
{
    public class PasswordValidRequest
    {
        public int UserId { get; set; }
        public string CurrentPassword { get; set; }
    }
}
