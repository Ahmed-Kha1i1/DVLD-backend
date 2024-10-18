using DVLD.Application.Common.Response;
using MediatR;

namespace DVLD.Application.Features.Users.Queries.GetUserQuery
{
    public class GetUserOverviewQuery : IRequest<Response<UserOverviewDTO>>
    {
        public int UserId { get; set; }

        public GetUserOverviewQuery(int userId)
        {
            UserId = userId;
        }
    }
}
