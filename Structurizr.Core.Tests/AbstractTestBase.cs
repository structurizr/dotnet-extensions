namespace Structurizr.Core.Tests
{
    public abstract class AbstractTestBase
    {

        protected Workspace Workspace;
        protected Model Model;
        protected ViewSet Views;

        public AbstractTestBase()
        {
            Workspace = new Workspace("Name", "Description");
            Model = Workspace.Model;
            Views = Workspace.Views;
        }

    }
}
