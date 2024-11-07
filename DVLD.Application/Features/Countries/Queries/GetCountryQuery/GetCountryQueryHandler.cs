using AutoMapper;
using DVLD.Domain.Entities;

namespace DVLD.Application.Features.Countries.Queries.GetCountryQuery
{
    public class GetCountryQueryHandler : ResponseHandler,
        IRequestHandler<GetCountryByIdQuery, Response<CountryDTO>>,
        IRequestHandler<GetCountryByNameQuery, Response<CountryDTO>>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public GetCountryQueryHandler(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }
        public async Task<Response<CountryDTO>> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
        {
            var country = await _countryRepository.GetByIdAsync(request.CoutryId);
            return Handle(country);
        }

        public async Task<Response<CountryDTO>> Handle(GetCountryByNameQuery request, CancellationToken cancellationToken)
        {
            var country = await _countryRepository.GetByNameAsync(request.CountryName);
            return Handle(country);
        }

        private Response<CountryDTO> Handle(Country? country)
        {
            if (country == null)
            {
                return NotFound<CountryDTO>("Country not found");
            }

            return Success(_mapper.Map<CountryDTO>(country));
        }
    }
}
