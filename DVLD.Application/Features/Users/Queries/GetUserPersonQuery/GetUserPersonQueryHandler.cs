using AutoMapper;
using DVLD.Application.Common.Response;
using DVLD.Application.Contracts.Persistence;
using DVLD.Domain.Entities;
using MediatR;

namespace DVLD.Application.Features.People.Queries.GetPersonQuery
{
    internal class GetUserPersonQueryHandler : ResponseHandler
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
            return Success(_mapper.Map<PersonDTO>(person));
        }
    }
}
