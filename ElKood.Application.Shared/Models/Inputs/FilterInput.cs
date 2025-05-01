namespace ElKood.Application.Shared.Models.Inputs
{
    public class FilterInput
    {
        public string Filter { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SortActive { get; set; }
        public string SortDirection { get; set; }
    }
}
