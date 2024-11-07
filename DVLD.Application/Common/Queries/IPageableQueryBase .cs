namespace DVLD.Application.Common.Queries
{
    public interface IPageableQueryBase
    {
        int PageNumber { get; set; }
        short PageSize { get; set; }

    }
}
