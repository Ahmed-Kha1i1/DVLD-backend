using DVLD.Application.Common.Response;
using DVLD.Application.Features.Driver.Common.Model;
using MediatR;

namespace DVLD.Application.Features.Driver.Queries.GetDriverQuery
{
    public class GetDriverQuery : IRequest<Response<DriverDTO>>
    {
        public int DriverId { get; set; }

        public GetDriverQuery(int driverId)
        {
            DriverId = driverId;
        }
    }
}
