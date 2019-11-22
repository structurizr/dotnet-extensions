using System;

namespace Structurizr.IO.C4PlantUML.ModelExtensions
{
    public static class ContainerExtensions
    {
        public static bool GetIsDatabase(this Container container)
        {
            string value = string.Empty;
            if (container.Properties?.TryGetValue(Properties.IsDatabase, out value) == true)
                return string.Compare(BooleanValues.True, value, StringComparison.OrdinalIgnoreCase) == 0;

            return false;
        }

        public static void SetIsDatabase(this Container container, bool isDatabase)
        {
            if (isDatabase)
                container.Properties[Properties.IsDatabase] = BooleanValues.True;
            else
                container.Properties.Remove(Properties.IsDatabase);
        }
    }
}