using AutoMapper;
using DVLD.Application.Common.Response;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.TestType.Common.Models;
using MediatR;

namespace DVLD.Application.Features.TestType.Queries.GetTestTypeGuery
{
    public class GetTestTypeGueryHandler(ITestTypeRepository testTypeRepository, IMapper mapper) : ResponseHandler, IRequestHandler<GetTestTypeGuery, Response<TestTypeDTO>>
    {
        public async Task<Response<TestTypeDTO>> Handle(GetTestTypeGuery request, CancellationToken cancellationToken)
        {
            var TestType = await testTypeRepository.GetByIdAsync(request.TestTypeId);
            if (TestType == null)
            {
                return NotFound<TestTypeDTO>("Test type not found");
            }

            return Success(mapper.Map<TestTypeDTO>(TestType));
        }
    }
}
