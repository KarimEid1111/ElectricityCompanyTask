namespace WebPortal.Models;

public class SearchPageViewModel
{
    public IEnumerable<WebPortal.Models.CuttingDownResultViewModel> Items { get; set; }
    public SearchFiltersViewModel Filters { get; set; }
}