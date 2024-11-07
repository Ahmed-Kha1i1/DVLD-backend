using AutoMapper;
using DVLD.Application.Features.TestType.Common.Models;

namespace DVLD.Application.Features.TestType.Commands.UpdateTestTypeCommand
{
    public class UpdateTestTypeCommandHandler(ITestTypeRepository testTypeRepository, IMapper mapper) : ResponseHandler, IRequestHandler<UpdateTestTypeCommand, Response<TestTypeDTO>>
    {
        public async Task<Response<TestTypeDTO>> Handle(UpdateTestTypeCommand request, CancellationToken cancellationToken)
        {
            var testType = await testTypeRepository.GetByIdAsync(request.TestTypeId);

            if (testType == null)
            {
                return NotFound<TestTypeDTO>("Test type not found");
            }
            mapper.Map(request, testType);

            if (!await testTypeRepository.SaveAsync(testType))
            {
                return Fail<TestTypeDTO>(null, "Error updating test type");
            }

            return Success(mapper.Map<TestTypeDTO>(testType));
        }
    }
}
