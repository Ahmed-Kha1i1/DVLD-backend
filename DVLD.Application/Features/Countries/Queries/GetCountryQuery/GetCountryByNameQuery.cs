using DVLD.Application.Common.Response;
using MediatR;

namespace DVLD.Application.Features.Countries.Queries.GetCountryQuery
{
    public class GetCountryByNameQuery : IRequest<Response<CountryDTO>>
    {
        public string CountryName { get; set; }

        public GetCountryByNameQuery(string countryName)
        {
            CountryName = countryName;
        }
    }
}
