using Structurizr.Annotations;

namespace Structurizr.Examples.Annotations
{
    [CodeElement(nameof(IRepository), Description = "Implementation")]
    [UsesContainer("Database", Description = "Reads from", Technology = "Entity Framework")]
    class EfRepository : IRepository
    {
        public string GetData(long id)
        {
            return "...";
        }
    }
}