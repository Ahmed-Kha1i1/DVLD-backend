using DVLD.Application.Common.Response;
using DVLD.Application.Contracts.Persistence;
using MediatR;
using System.Net;

namespace DVLD.Application.Features.People.Commands.DeletePersonCommand
{
    public class DeletePersonCommandHandler(IPersonRepository personRepository) : ResponseHandler, IRequestHandler<DeletePersonCommand, Response<int?>>
    {
        public async Task<Response<int?>> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
            var personExists = await personRepository.IsPersonExists(request.PersonId);

            if (!personExists)
            {
                return NotFound<int?>("Person not found");
            }

            if (await personRepository.DeleteAsync(request.PersonId))
            {
                return Success<int?>(request.PersonId, $"Person with id {request.PersonId} has been deleted.");
            }
            else
            {
                return Custom<int?>(HttpStatusCode.Conflict, null, $"Cannot delete person with id {request.PersonId}");
            }
        }
    }
}
