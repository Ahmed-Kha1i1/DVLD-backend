using DVLD.Application.Common.Response;
using DVLD.Application.Features.People.Common.Models;
using MediatR;

namespace DVLD.Application.Features.Driver.Queries.GetDriverPersonQuery
{
    public class GetDriverPersonQuery : IRequest<Response<PersonDTO>>
    {
        public int DriverId { get; set; }

        public GetDriverPersonQuery(int driverId)
        {
            DriverId = driverId;
        }
    }
}
