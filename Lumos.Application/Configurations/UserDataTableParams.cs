namespace NutriPro.Application.Configurations
{
    public class UserDataTableParams
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public List<ColumnOrder> Order { get; set; }
        public SearchFilter Search { get; set; }
    }

    public class ColumnOrder
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }

    public class SearchFilter
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }

}
