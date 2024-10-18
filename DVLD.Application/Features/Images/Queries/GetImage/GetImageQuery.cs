using DVLD.Application.Common.Response;
using MediatR;

namespace DVLD.Application.Features.Images.Queries.GetImage
{
    public class GetImageQuery : IRequest<Response<GetImageQueryResponse>>
    {
        public string FileName { get; set; }

        public GetImageQuery(string fileName)
        {
            FileName = fileName;
        }
    }
}
