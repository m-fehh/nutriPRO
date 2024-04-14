namespace NutriPro.Application.Models
{
    public class PaginationResult<TEntity>
    {
        public List<TEntity> Entities { get; set; }
        public int TotalRecords { get; set; }
    }
}
