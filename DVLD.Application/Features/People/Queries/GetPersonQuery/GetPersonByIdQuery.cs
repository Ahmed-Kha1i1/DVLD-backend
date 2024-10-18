using DVLD.Application.Common.Response;
using MediatR;

namespace DVLD.Application.Features.People.Queries.GetPersonQuery
{
    public class GetPersonByIdQuery : IRequest<Response<PersonDTO>>
    {
        public int PersonId { get; set; }

        public GetPersonByIdQuery(int personId)
        {
            PersonId = personId;
        }
    }
}
