using AutoMapper;
using DVLD.Application.Common.Response;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.People.Queries.GetPersonQuery.ByNationalNumber;
using DVLD.Domain.Entities;
using MediatR;

namespace DVLD.Application.Features.People.Queries.GetPersonQuery
{
    internal class GetPersonQueryHandler : ResponseHandler
         , IRequestHandler<GetPersonByIdQuery, Response<PersonDTO>>
         , IRequestHandler<GetPersonByNationalNumberQuery, Response<PersonDTO>>
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public GetPersonQueryHandler(IPersonRepository personRepository, ICountryRepository countryRepository, IMapper mapper)
        {
            _personRepository = personRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }
        public async Task<Response<PersonDTO>> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
        {
            var person = await _personRepository.GetByIdAsync(request.PersonId);
            return await Handle(person);
        }

        public async Task<Response<PersonDTO>> Handle(GetPersonByNationalNumberQuery request, CancellationToken cancellationToken)
        {
            var person = await _personRepository.GetAsync(request.NationalNumber);
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
