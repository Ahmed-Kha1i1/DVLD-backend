using DVLD.Application.Common.Queries;

namespace DVLD.Application.Features.People.Queries.GetPeopleQuery
{
    public class GetPeopleQuery : ItemsQueryBase, IRequest<Response<GetPeopleQueryResponse>>
    {
        public int? Id { get; set; } = null;
        public string? NationalNumber { get; set; } = null;
        public bool? Gender { get; set; } = null;
    }
}
