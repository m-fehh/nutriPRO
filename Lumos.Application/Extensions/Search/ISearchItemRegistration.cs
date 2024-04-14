namespace NutriPro.Application.Extensions.Search
{
    public interface ISearchItemRegistration
    {
        string RegistrationName { get; }

        string RegistrationAddress { get; }
    }

    public interface ISearchItem
    {
        string SearchName { get; }
        List<ISearchResult> Get(string searchTerm, int pageSize, int pageNum, string extraCondition = null);
        SearchResult GetById(long id);
        long GetCount(string searchTerm, string extraCondition = null);
    }

    public interface ISearchResult
    {
        public long? id { get; set; }
        string text { get; set; }
        string CustomHtmlFormat { get; set; }
    }

    /// <summary>
    /// Todo resultado de um buscador (item gráfico normalmente utilizado para foreign keys) deverá ser convertido para esse objeto
    /// </summary>
    public class SearchResult : ISearchResult
    {
        public long? id { get; set; }
        public string text { get; set; }
        public string CustomHtmlFormat { get; set; }
        public bool Inactive { get; set; }
        public bool Block { get; set; }
        public int? AccessLimitNumber { get; set; }
    }

    public class GroupSearchResult : ISearchResult
    {
        public long? id { get; set; }
        public string text { get; set; }
        public string CustomHtmlFormat { get; set; }
        public List<ISearchResult> children { get; set; }
    }
}
