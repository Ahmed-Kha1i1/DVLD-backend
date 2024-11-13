namespace DVLD.Application.Features.Images.Queries.GetImage
{
    public class GetImageQueryResponse
    {
        public Stream Image { get; set; }
        public string ContentType { get; set; }

        public GetImageQueryResponse(Stream image, string contentType)
        {
            Image = image;
            ContentType = contentType;
        }
    }
}
