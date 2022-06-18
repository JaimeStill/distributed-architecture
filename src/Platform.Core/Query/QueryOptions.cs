namespace Platform.Core.Query;
public class QueryOptions
{
    private string sort;

    public int Page { get; set; } = 1;
    public int PageSize { get; set; }
    public string Search { get; set; }

    public string Sort
    {
        get => sort;
        set => sort = string.IsNullOrEmpty(value)
          ? "Id_asc"
          : value.Contains('_')
              ? value
              : $"{value}_asc";
    }

    public string SortProperty => Sort.Split('_')[0];

    public bool SortDescending =>
      Sort.Split('_')[1]
          .ToLower()
          .Equals("desc");

    static int GeneratePage(string page, int d) =>
        int.TryParse(page, out int _page)
          ? _page
          : d;

    public static QueryOptions FromQuery(QueryParams query, int pageSize = 20) => new()
    {
        Page = GeneratePage(query.Page, 1),
        PageSize = GeneratePage(query.PageSize, pageSize),
        Search = query.Search,
        Sort = query.Sort
    };
}