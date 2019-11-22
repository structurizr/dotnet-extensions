

// Source base version copied from https://gist.github.com/coldacid/465fa8f3a4cd3fdd7b640a65ad5b86f4 (https://github.com/structurizr/dotnet/issues/47) 
// kirchsth: Extended with dynamic and deployment view
namespace Structurizr.IO.C4PlantUML.ModelExtensions
{
    public class Properties
    {
        /// <summary>
        /// This property enables to mark Components and Container as database with value "true"
        /// </summary>
        public const string IsDatabase = "IsDatabase";

        /// <summary>
        /// This property enables to define a specific (default) direction of Relationships or RelationshipViews
        /// </summary>
        public const string Direction = "Direction";
    }
}