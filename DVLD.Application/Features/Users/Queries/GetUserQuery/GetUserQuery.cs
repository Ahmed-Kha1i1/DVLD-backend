using DVLD.Application.Common.Response;
using MediatR;

namespace DVLD.Application.Features.Users.Queries.GetUserQuery
{
    public class GetUserQuery : IRequest<Response<UserDTO>>
    {
        public int UserId { get; set; }

        public GetUserQuery(int userId)
        {
            UserId = userId;
        }
    }
}
