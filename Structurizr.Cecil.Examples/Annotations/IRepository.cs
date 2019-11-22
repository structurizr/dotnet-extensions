using Structurizr.Annotations;

namespace Structurizr.Examples.Annotations
{
    [Component(Description = "Provides access to data stored in the database.", Technology = "C#")]
    public interface IRepository
    {
        string GetData(long id);
    }
}