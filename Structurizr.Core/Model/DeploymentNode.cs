using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Structurizr
{
    
    /// <summary>
    /// Represents a deployment node, which is something like:
    ///  - Physical infrastructure (e.g. a physical server or device)
    ///  - Virtualised infrastructure (e.g. IaaS, PaaS, a virtual machine)
    ///  - Containerised infrastructure (e.g. a Docker container)
    ///  - Database server
    ///  - Java EE web/application server
    ///  - Microsoft IIS
    ///  - etc
    /// </summary>
    [DataContract]
    public sealed class DeploymentNode : DeploymentElement
    {

        private DeploymentNode _parent;

        /// <summary>
        /// The parent DeploymentNode, or null if there is no parent.
        /// </summary>
        public override Element Parent
        {
            get { return _parent; }
            set { _parent = value as DeploymentNode; }
        }
            
        [DataMember(Name = "technology", EmitDefaultValue = false)]
        public string Technology { get; set; }

        [DataMember(Name = "instances", EmitDefaultValue = false)]
        public int Instances { get; set; }

        private HashSet<DeploymentNode> _children;

        /// <summary>
        /// The set of child deployment nodes.
        /// </summary>
        [DataMember(Name = "children", EmitDefaultValue = false)]
        public ISet<DeploymentNode> Children
        {
            get
            {
                return new HashSet<DeploymentNode>(_children);
            }

            internal set
            {
                _children = new HashSet<DeploymentNode>(value);
            }
        }

        private HashSet<ContainerInstance> _containerInstances;


        /// <summary>
        /// The set of container instances associated with this deployment node.
        /// </summary>
        [DataMember(Name = "containerInstances", EmitDefaultValue = false)]
        public ISet<ContainerInstance> ContainerInstances
        {
            get
            {
                return new HashSet<ContainerInstance>(_containerInstances);
            }

            internal set
            {
                _containerInstances = new HashSet<ContainerInstance>(value);
            }
        }

        internal DeploymentNode()
        {
            Instances = 1;
            _children = new HashSet<DeploymentNode>();
            _containerInstances = new HashSet<ContainerInstance>();
            Environment = DefaultDeploymentEnvironment;
        }

        public override string Tags {
            get
            {
                return "";
            }
            set
            {
                // no-op
            }
        }
        
        public override List<string> GetRequiredTags()
        {
            return new List<string>();
        }

        public override string CanonicalName
        {
            get
            {
                if (_parent != null)
                {
                    return _parent.CanonicalName + CanonicalNameSeparator + FormatForCanonicalName(Name);
                }
                else
                {
                    return CanonicalNameSeparator + "Deployment" + CanonicalNameSeparator + FormatForCanonicalName(Environment) + CanonicalNameSeparator + FormatForCanonicalName(Name);
                }
            }
        }

        /// <summary>
        /// Adds a container instance to this deployment node.
        /// </summary>
        /// <param name="container">the Container to add an instance of</param>
        /// <returns>a ContainerInstance object</returns>
        public ContainerInstance Add(Container container) {
            if (container == null) {
                throw new ArgumentException("A container must be specified.");
            }

            ContainerInstance containerInstance = Model.AddContainerInstance(this, container);
            _containerInstances.Add(containerInstance);
    
            return containerInstance;
        }

        /// <summary>
        /// Adds a child deployment node.
        /// </summary>
        /// <param name="name">the name of the deployment node</param>
        /// <param name="description">a short description</param>
        /// <param name="technology">the technology</param>
        /// <returns></returns>
        public DeploymentNode AddDeploymentNode(string name, string description, string technology) {
            return AddDeploymentNode(name, description, technology, 1);
        }
    
        /// <summary>
        /// Adds a child deployment node.
        /// </summary>
        /// <param name="name">the name of the deployment node</param>
        /// <param name="description">a short description</param>
        /// <param name="technology">the technology</param>
        /// <param name="instances">the number of  instances</param>
        /// <returns></returns>
        public DeploymentNode AddDeploymentNode(string name, string description, string technology, int instances) {
            return AddDeploymentNode(name, description, technology, instances, null);
        }
    
        /// <summary>
        /// Adds a child deployment node.
        /// </summary>
        /// <param name="name">the name of the deployment node</param>
        /// <param name="description">a short description</param>
        /// <param name="technology">the technology</param>
        /// <param name="instances">the number of  instances</param>
        /// <param name="properties">a Dictionary (string,string) describing name=value properties</param>
        /// <returns></returns>
        public DeploymentNode AddDeploymentNode(string name, string description, string technology, int instances, Dictionary<string,string> properties) {
            DeploymentNode deploymentNode = Model.AddDeploymentNode(this, this.Environment, name, description, technology, instances, properties);
            if (deploymentNode != null) {
                _children.Add(deploymentNode);
            }
            
            return deploymentNode;
        }

        /// <summary>
        /// Gets the DeploymentNode with the specified name.
        /// </summary>
        /// <param name="name">the name of the deployment node</param>
        /// <returns>the DeploymentNode instance with the specified name (or null if it doesn't exist)</returns>
        public DeploymentNode GetDeploymentNodeWithName(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException("A name must be specified.");
            }

            return _children.FirstOrDefault(dn => dn.Name.Equals(name));
        }

        /// <summary>
        /// Adds a relationship between this and another deployment node.
        /// </summary>
        /// <param name="destination">the destination DeploymentNode</param>
        /// <param name="description">a short description of the relationship</param>
        /// <param name="technology">the technology</param>
        /// <returns>a Relationship object</returns>
        public Relationship Uses(DeploymentNode destination, string description, string technology)
        {
            return Model.AddRelationship(this, destination, description, technology);
        }

    }
    
}