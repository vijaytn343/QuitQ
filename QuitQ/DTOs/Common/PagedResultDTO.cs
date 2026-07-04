namespace QuitQ.DTOs.Common
{
    public class PagedResultDTO<T>
    {
        public List<T> Items { get; set; } = new();

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages { get; set; }
    }
}