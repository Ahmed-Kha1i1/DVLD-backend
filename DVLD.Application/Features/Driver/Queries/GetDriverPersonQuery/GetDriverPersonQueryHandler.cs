using AutoMapper;
using DVLD.Application.Features.People.Common.Models;
using DVLD.Domain.Entities;

namespace DVLD.Application.Features.Driver.Queries.GetDriverPersonQuery
{
    public class GetDriverPersonQueryHandler : ResponseHandler
         , IRequestHandler<GetDriverPersonQuery, Response<PersonDTO>>
    {
        private readonly IDriverRepository _driverRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public GetDriverPersonQueryHandler(IDriverRepository driverRepository, ICountryRepository countryRepository, IMapper mapper)
        {
            _driverRepository = driverRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        public async Task<Response<PersonDTO>> Handle(GetDriverPersonQuery request, CancellationToken cancellationToken)
        {
            var person = await _driverRepository.GetPerson(request.DriverId);
            return await Handle(person);
        }

        private async Task<Response<PersonDTO>> Handle(Person? person)
        {
            if (person == null)
            {
                return NotFound<PersonDTO>("Person not found");
            }
            person.CountryInfo = await _countryRepository.GetByIdAsync(person.NationalityCountryID);
            if (person.CountryInfo == null)
            {
                return NotFound<PersonDTO>("Country not found");
            }
            return Success(_mapper.Map<PersonDTO>(person));
        }
    }
}
