using System.Collections.Generic;

namespace Structurizr.IO.C4PlantUML.ModelExtensions
{
    /// <summary>
    /// WORKAROUND: RelationshipView supports no properties anymore, therefore the direction is stored in Position
    /// </summary>
    public static class RelationshipViewExtensions
    {
        /// <summary>
        /// Get a direction of the relation which should be used in a specific C4PlantUML views
        /// (direction is stored in Position)
        /// </summary>
        /// <param name="relationshipView"></param>
        /// <param name="viewSpecific">returns true if it is defined via the view specific RelationshipView and false if it is defined via the underlying Relationship</param>
        /// <returns></returns>
        public static string GetDirection(this RelationshipView relationshipView, out bool viewSpecific)
        {
            string value = DirectionValues.NotSet;
            if (relationshipView.Position.HasValue && Position2Direction.TryGetValue(relationshipView.Position.Value, out value) == true)
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
        /// (direction is internal stored in Position)
        /// </summary>
        /// <param name="relationshipView"></param>
        /// <param name="direction">one of <see cref="DirectionValues"/></param>
        public static void SetDirection(this RelationshipView relationshipView, string direction)
        {
            if (string.IsNullOrWhiteSpace(direction)) // direction DirectionValues.NotSet
                relationshipView.Position = null;
            else
                relationshipView.Position = Direction2Position[direction];
        }

        private static Dictionary<string, int> Direction2Position = new Dictionary<string, int>
        {
            [DirectionValues.Up] = 1,
            [DirectionValues.Down] = 2,
            [DirectionValues.Left] = 3,
            [DirectionValues.Right] = 4
        };

        private static Dictionary<int, string> Position2Direction = new Dictionary<int, string>
        {
            [1] = DirectionValues.Up,
            [2] = DirectionValues.Down,
            [3] = DirectionValues.Left,
            [4] = DirectionValues.Right
        };
    }
}