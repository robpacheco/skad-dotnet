namespace Skad.Common.Api.Common
{
    public class PageInfo
    {
        public PageInfo(int totalItems, int currentPage, int pageSize)
        {
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }

        public int TotalItems { get; }
        public int CurrentPage { get; }
        public int PageSize { get; }
        
        public bool HasPrevPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage * PageSize < TotalItems;
        public string PrevPageQueryParameters => $"pageNum={CurrentPage - 1}&pageSize={PageSize}";
        public string NextPageQueryParameters => $"pageNum={CurrentPage + 1}&pageSize={PageSize}";
    }
}