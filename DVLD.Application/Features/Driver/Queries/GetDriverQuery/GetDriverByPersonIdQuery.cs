using DVLD.Application.Common.Response;
using DVLD.Application.Features.Driver.Common.Model;
using MediatR;

namespace DVLD.Application.Features.Driver.Queries.GetDriverQuery
{
    public class GetDriverByPersonIdQuery : IRequest<Response<DriverDTO>>
    {
        public int PersonId { get; set; }

        public GetDriverByPersonIdQuery(int personId)
        {
            PersonId = personId;
        }
    }
}
