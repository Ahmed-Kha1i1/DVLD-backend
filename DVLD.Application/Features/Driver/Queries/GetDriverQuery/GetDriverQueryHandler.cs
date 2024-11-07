using AutoMapper;
using DVLD.Application.Features.Driver.Common.Model;
using DVLD.Application.Features.People.Common.Models;

namespace DVLD.Application.Features.Driver.Queries.GetDriverQuery
{
    public class GetDriverQueryHandler(IDriverRepository driverRepository, IPersonRepository personRepository, ICountryRepository countryRepository, IMapper mapper) : ResponseHandler,
        IRequestHandler<GetDriverQuery, Response<DriverDTO>>,
        IRequestHandler<GetDriverByPersonIdQuery, Response<DriverDTO>>
    {
        public async Task<Response<DriverDTO>> Handle(GetDriverQuery request, CancellationToken cancellationToken)
        {
            var Driver = await driverRepository.GetFullAsync(request.DriverId);
            if (Driver == null)
            {
                return NotFound<DriverDTO>("Driver not found");
            }
            var person = await personRepository.GetByIdAsync(Driver.PersonID);
            if (person == null)
            {
                return NotFound<DriverDTO>("Person not found");
            }
            person.CountryInfo = await countryRepository.GetByIdAsync(person.NationalityCountryID);
            if (person.CountryInfo == null)
            {
                return NotFound<DriverDTO>("Country not found");
            }

            Driver.Person = mapper.Map<PersonDTO>(person);
            return Success(Driver);
        }

        public async Task<Response<DriverDTO>> Handle(GetDriverByPersonIdQuery request, CancellationToken cancellationToken)
        {
            var Driver = await driverRepository.GetBypersonId(request.PersonId);
            if (Driver == null)
            {
                return NotFound<DriverDTO>("Driver not found");
            }

            return Success(Driver);
        }
    }
}
