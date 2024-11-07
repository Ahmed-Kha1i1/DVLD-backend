using AutoMapper;

namespace DVLD.Application.Features.Test.Commands.AddTestCommand
{
    public class AddTestCommandHandler(IMapper mapper, ITestRepository testRepository) : ResponseHandler, IRequestHandler<AddTestCommand, Response<int?>>
    {
        public async Task<Response<int?>> Handle(AddTestCommand request, CancellationToken cancellationToken)
        {
            AllEntities.Test Test = mapper.Map<AllEntities.Test>(request);

            if (!await testRepository.SaveAsync(Test))
            {
                return Fail<int?>(null, "Error Adding test");
            }

            return Created<int?>(Test.Id);
        }
    }
}
