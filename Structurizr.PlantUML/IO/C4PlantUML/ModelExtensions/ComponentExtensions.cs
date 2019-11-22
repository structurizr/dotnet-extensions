using System;

namespace Structurizr.IO.C4PlantUML.ModelExtensions
{
    public static class ComponentExtensions
    {
        public static bool GetIsDatabase(this Component container)
        {
            string value = string.Empty;
            if (container.Properties?.TryGetValue(Properties.IsDatabase, out value) == true)
                return string.Compare(BooleanValues.True, value, StringComparison.OrdinalIgnoreCase) == 0;

            return false;
        }

        public static void SetIsDatabase(this Component container, bool isDatabase)
        {
            if (isDatabase)
                container.Properties[Properties.IsDatabase] = BooleanValues.True;
            else
                container.Properties.Remove(Properties.IsDatabase);
        }
    }
}