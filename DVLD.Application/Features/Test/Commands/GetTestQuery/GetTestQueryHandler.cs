
using AutoMapper;

namespace DVLD.Application.Features.Test.Commands.GetTestQuery
{
    public class GetTestQueryHandler(ITestRepository testRepository, IMapper mapper) : ResponseHandler, IRequestHandler<GetTestQuery, Response<GetTestQueryResponse>>
    {
        public async Task<Response<GetTestQueryResponse>> Handle(GetTestQuery request, CancellationToken cancellationToken)
        {
            var Test = await testRepository.GetByIdAsync(request.TestId);
            if (Test == null)
            {
                return NotFound<GetTestQueryResponse>("Test not found");
            }

            return Success(mapper.Map<GetTestQueryResponse>(Test));
        }
    }
}
