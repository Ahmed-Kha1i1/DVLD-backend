namespace DVLD.Application.Common.Queries
{
    public interface ISortableQueryBase
    {
        string OrderBy { get; set; }
        string OrderDirection { get; set; }
    }
}
