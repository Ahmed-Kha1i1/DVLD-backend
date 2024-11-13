using DVLD.Application.Infrastracture;

namespace DVLD.Application.Features.Images.Queries.GetImage
{
    public class GetImageQueryHandler(IImageService imageService) : ResponseHandler, IRequestHandler<GetImageQuery, Response<GetImageQueryResponse>>
    {
        public async Task<Response<GetImageQueryResponse>> Handle(GetImageQuery request, CancellationToken cancellationToken)
        {
            var result = await imageService.GetImageAsync(request.FileName);
            if (result is null)
            {
                return NotFound<GetImageQueryResponse>("The specified key does not exist.");
            }
            return Success(result);
        }
    }
}
