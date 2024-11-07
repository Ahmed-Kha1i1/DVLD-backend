using DVLD.Application.Common.Response;
using DVLD.Application.Features.People.Commands.ModificationPerson;
using DVLD.Application.Features.People.Common.Models;
using MediatR;

namespace DVLD.Application.Features.People.Commands.UpdatePersonCommand
{
    public class UpdatePersonCommand : ModificationPersonCommand, IRequest<Response<PersonDTO>>
    {
        public int PersonId { get; set; }
        public bool RemoveImage { get; set; } = false;
    }
}
