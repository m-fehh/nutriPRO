using NutriPro.Data;
using Microsoft.EntityFrameworkCore;

namespace NutriPro.Application.Extensions.Search
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class NoFreeSearchAttribute : Attribute
    {
    }

    public class SearchCollection
    {
        public SearchCollection() 
        {
            Itens = new List<ISearchItem>();
        }

        public List<ISearchItem> Itens { get; set; }
    }

    public class SearchItemRepository
    {

        public SearchItemRepository()
        {
        }

        public ISearchItem FindSearchItemByName(string searchName)
        {
            using (var dbContext = new NutriProContext(new DbContextOptions<NutriProContext>()))
            {
                return dbContext.Set<ISearchItem>()
                .FirstOrDefault(item => item.SearchName == searchName);
            }
        }
    }
}
