using DVLD.Application.Common.Response;
using DVLD.Application.Features.TestType.Common.Models;
using MediatR;

namespace DVLD.Application.Features.TestType.Queries.GetTestTypeGuery
{
    public class GetTestTypeGuery : IRequest<Response<TestTypeDTO>>
    {
        public int TestTypeId { get; set; }

        public GetTestTypeGuery(int testTypeId)
        {
            TestTypeId = testTypeId;
        }
    }
}
