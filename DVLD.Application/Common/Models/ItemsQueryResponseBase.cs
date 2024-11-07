namespace DVLD.Application.Common.Models
{
    public abstract class ItemsQueryResponseBase<T> where T : class
    {
        public IReadOnlyList<T> Items { get; set; }
        public PaginationMetadata Metadata { get; set; }
    }
}
