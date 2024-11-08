using DVLD.Application.Common.Queries;

namespace DVLD.Application.Features.Users.Queries.GetUsersQuery
{
    public class GetUsersQuery : ItemsQueryBase, IRequest<Response<GetUsersQueryResponse>>
    {
        public int? Id { get; set; } = null;
        public int? PersonId { get; set; } = null;
        public string? Username { get; set; } = null;
        public bool? IsActive { get; set; } = null;
    }
}
