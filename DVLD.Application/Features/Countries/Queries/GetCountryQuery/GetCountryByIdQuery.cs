using DVLD.Application.Common.Response;
using MediatR;

namespace DVLD.Application.Features.Countries.Queries.GetCountryQuery
{
    public class GetCountryByIdQuery : IRequest<Response<CountryDTO>>
    {
        public int CoutryId { get; set; }

        public GetCountryByIdQuery(int coutryId)
        {
            CoutryId = coutryId;
        }
    }
}
