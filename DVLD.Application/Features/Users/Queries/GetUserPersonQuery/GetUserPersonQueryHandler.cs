using AutoMapper;
using DVLD.Application.Features.People.Common.Models;
using DVLD.Domain.Entities;

namespace DVLD.Application.Features.People.Queries.GetPersonQuery
{
    public class GetUserPersonQueryHandler : ResponseHandler
         , IRequestHandler<GetUserPersonQuery, Response<PersonDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public GetUserPersonQueryHandler(IUserRepository userRepository, ICountryRepository countryRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }
        public async Task<Response<PersonDTO>> Handle(GetUserPersonQuery request, CancellationToken cancellationToken)
        {
            var person = await _userRepository.GetPerson(request.UserId);
            return await Handle(person);
        }

        private async Task<Response<PersonDTO>> Handle(Person? person)
        {
            if (person == null)
            {
                return NotFound<PersonDTO>("Person not found");
            }
            person.CountryInfo = await _countryRepository.GetByIdAsync(person.NationalityCountryID);
            if (person.CountryInfo is null)
            {
                return NotFound<PersonDTO>("Country not found");
            }
            return Success(_mapper.Map<PersonDTO>(person));
        }
    }
}
