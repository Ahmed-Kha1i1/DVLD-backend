using AutoMapper;
using DVLD.Application.Common.Enums;
using DVLD.Application.Common.Response;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Infrastracture;
using DVLD.Domain.Entities;
using MediatR;

namespace DVLD.Application.Features.People.Commands.UpdatePersonCommand
{
    public class UpdatePersonCommandHandler : ResponseHandler, IRequestHandler<UpdatePersonCommand, Response<PersonDTO>>
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        public UpdatePersonCommandHandler(IPersonRepository personRepository, ICountryRepository countryRepository, IImageService imageService, IMapper mapper)
        {
            _personRepository = personRepository;
            _countryRepository = countryRepository;
            _imageService = imageService;
            _mapper = mapper;

        }
        public async Task<Response<PersonDTO>> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            Person? person = await _personRepository.GetByIdAsync(request.PersonId);
            if (person == null)
            {
                return NotFound<PersonDTO>("Person not found");
            }
            _mapper.Map(request, person);
            (enUpdateImage Replace, string newIMagepath) = await _imageService.ReplaceImageAsync(request.ImageFile, person.ImageName, request.RemoveImage);

            if (Replace == enUpdateImage.UpdatedFail)
            {
                return Fail<PersonDTO>(null, "Image upload failed");
            }

            if (Replace == enUpdateImage.DeletedFail)
            {
                return Fail<PersonDTO>(null, "Image delete failed");
            }

            person.ImageName = Replace == enUpdateImage.Updated ? newIMagepath : Replace == enUpdateImage.DeletedWithoutUpdate ? null : person.ImageName;

            if (!await _personRepository.SaveAsync(person))
            {
                return Fail<PersonDTO>(null, "Error updating person");
            }
            person.CountryInfo = await _countryRepository.GetByIdAsync(person.NationalityCountryID);
            return Success(_mapper.Map<PersonDTO>(person));
        }
    }
}
