using AutoMapper;
using DVLD.Application.Common.Response;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Infrastracture;
using DVLD.Domain.Entities;
using MediatR;

namespace DVLD.Application.Features.People.Commands.AddPersonCommand
{
    public class AddPersonCommandHandler : ResponseHandler, IRequestHandler<AddPersonCommand, Response<int?>>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        public AddPersonCommandHandler(IPersonRepository personRepository, IImageService imageService, IMapper mapper)
        {
            _personRepository = personRepository;
            _imageService = imageService;
            _mapper = mapper;

        }
        public async Task<Response<int?>> Handle(AddPersonCommand request, CancellationToken cancellationToken)
        {
            var person = _mapper.Map<Person>(request);

            if (!await _personRepository.SaveAsync(person))
            {
                return Fail<int?>(null, "Error adding person");
            }

            if (request.ImageFile is null)
            {
                return Created<int?>(person.Id);
            }

            string ImageName = await _imageService.UploadImageAsync(request.ImageFile);

            if (string.IsNullOrEmpty(ImageName))
            {
                return Custom<int?>(System.Net.HttpStatusCode.MultiStatus, person.Id, "Person was added successfully, but image upload failed"); ;
            }

            person.ImageName = ImageName;

            if (await _personRepository.SaveAsync(person))
            {
                return Created<int?>(person.Id);
            }
            else
            {
                return Custom<int?>(System.Net.HttpStatusCode.MultiStatus, person.Id, "Person was added successfully, but fail updating person with image name"); ;
            }
        }
    }
}
