using Structurizr.Annotations;

namespace Structurizr.Examples.Annotations
{
    [Component(Description = "Serves HTML pages to users.", Technology = "ASP.NET MVC")]
    [UsedByPerson("User", Description = "Uses", Technology = "HTTPS")]
    class HtmlController
    {
        [UsesComponent("Gets data using")]
        private IRepository repository = new EfRepository();
    }
}