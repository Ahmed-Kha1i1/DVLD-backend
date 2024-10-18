using DVLD.Application.Common.Response;
using DVLD.Application.Features.People.Commands.ModificationPerson;
using MediatR;

namespace DVLD.Application.Features.People.Commands.AddPersonCommand
{
    public class AddPersonCommand : ModificationPersonCommand, IRequest<Response<int?>>
    {

    }
}
