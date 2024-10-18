using DVLD.Application.Common.Response;
using MediatR;

namespace DVLD.Application.Features.People.Commands.DeletePersonCommand
{
    public class DeletePersonCommand : IRequest<Response<int?>>
    {
        public int PersonId { get; set; }
        public DeletePersonCommand(int personId)
        {
            PersonId = personId;
        }
    }
}
