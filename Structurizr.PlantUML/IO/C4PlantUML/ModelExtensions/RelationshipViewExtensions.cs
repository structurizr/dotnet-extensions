namespace Structurizr.IO.C4PlantUML.ModelExtensions
{
    public static class RelationshipViewExtensions
    {
        /// <summary>
        /// Get a direction of the relation which should be used in a specific C4PlantUML views
        /// </summary>
        /// <param name="relationshipView"></param>
        /// <param name="viewSpecific">returns true if it is defined via the view specific RelationshipView and false if it is defined via the underlying Relationship</param>
        /// <returns></returns>
        public static string GetDirection(this RelationshipView relationshipView, out bool viewSpecific)
        {
            string value = DirectionValues.NotSet;
            if (relationshipView.Properties?.TryGetValue(Properties.Direction, out value) == true)
            {
                viewSpecific = true;
                return value;
            }

            viewSpecific = false;
            value = relationshipView.Relationship.GetDirection();
            return value;
        }

        /// <summary>
        /// Set a direction of the relation which should be used in a specific C4PlantUML views
        /// </summary>
        /// <param name="relationshipView"></param>
        /// <param name="direction">one of <see cref="DirectionValues"/></param>
        public static void SetDirection(this RelationshipView relationshipView, string direction)
        {
            if (string.IsNullOrWhiteSpace(direction)) // direction DirectionValues.NotSet
                relationshipView.Properties.Remove(Properties.Direction);
            else
                relationshipView.Properties[Properties.Direction] = direction;
        }
    }
}