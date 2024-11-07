using DVLD.Application.Common.Response;
using DVLD.Application.Features.TestType.Common.Models;
using MediatR;

namespace DVLD.Application.Features.TestType.Commands.UpdateTestTypeCommand
{
    public class UpdateTestTypeCommand : IRequest<Response<TestTypeDTO>>
    {
        public int TestTypeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Fees { get; set; }
    }
}
