namespace DVLD.Application.Features.Images.Queries.GetImage
{
    public class GetImageQueryResponse
    {
        public FileStream Image { get; set; }
        public string ContentType { get; set; }

        public GetImageQueryResponse(FileStream image, string contentType)
        {
            Image = image;
            ContentType = contentType;
        }
    }
}
