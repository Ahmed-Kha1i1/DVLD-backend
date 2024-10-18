using DVLD.Application.Common.Response;
using DVLD.Application.Infrastracture;
using MediatR;

namespace DVLD.Application.Features.Images.Queries.GetImage
{
    public class GetImageQueryHandler(IImageService imageService) : ResponseHandler, IRequestHandler<GetImageQuery, Response<GetImageQueryResponse>>
    {
        public Task<Response<GetImageQueryResponse>> Handle(GetImageQuery request, CancellationToken cancellationToken)
        {
            var result = imageService.GetImage(request.FileName);
            if (result is null)
            {
                return Task.FromResult(NotFound<GetImageQueryResponse>("The specified key does not exist."));
            }
            return Task.FromResult(Success(result));
        }
    }
}
