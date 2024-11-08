namespace DVLD.Application.Common.Queries
{
    public abstract class ItemsQueryBase : IPageableQueryBase, ISearchableQueryBase, ISortableQueryBase
    {
        protected virtual short MaxPageSize { get; set; } = 20;
        private int _minPageNumber = 1;
        private int _pageNumber = 1;
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = (value < _minPageNumber) ? _minPageNumber : value;
        }
        private short _pageSize = 10;
        public short PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public virtual string OrderBy { get; set; } = "Id";
        public virtual string OrderDirection { get; set; } = "Asc";
        public string? SearchQuery { get; set; } = null;

    }
}
