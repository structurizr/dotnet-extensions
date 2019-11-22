using System.Runtime.Serialization;

namespace Structurizr
{
    
    /// <summary>
    /// This is the superclass for model elements that describe deployment nodes and container instances.
    /// </summary>
    [DataContract]
    public abstract class DeploymentElement : Element
    {
        
        internal static string DefaultDeploymentEnvironment = "Default";

        [DataMember(Name = "environment", EmitDefaultValue = false)]
        public string Environment { get; internal set; }

    }

}