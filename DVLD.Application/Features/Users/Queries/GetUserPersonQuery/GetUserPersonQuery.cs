using DVLD.Application.Common.Response;
using DVLD.Application.Features.People.Common.Models;
using MediatR;

namespace DVLD.Application.Features.People.Queries.GetPersonQuery
{
    public class GetUserPersonQuery : IRequest<Response<PersonDTO>>
    {
        public int UserId { get; set; }

        public GetUserPersonQuery(int userId)
        {
            UserId = userId;
        }
    }
}
