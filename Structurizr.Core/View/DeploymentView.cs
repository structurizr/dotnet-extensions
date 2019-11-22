using System.Linq;
using System.Runtime.Serialization;

namespace Structurizr
{

    /// <summary>
    /// A deployment view, used to show the mapping of container instances to deployment nodes.
    /// </summary>
    public sealed class DeploymentView : View
    {

        public override Model Model { get; set; }

        /// <summary>
        /// The name of the environment that this deployment view is for (e.g. "Development", "Live", etc).
        /// </summary>
        [DataMember(Name = "environment", EmitDefaultValue = false)]
        public string Environment { get; set; }

        DeploymentView()
        {
        }

        internal DeploymentView(Model model, string key, string description) : base(null, key, description)
        {
            Model = model;
        }

        internal DeploymentView(SoftwareSystem softwareSystem, string key, string description) : base(softwareSystem,
            key, description)
        {
            Model = softwareSystem.Model;
        }

        /// <summary>
        /// Adds all of the top-level deployment nodes to this view. 
        /// </summary>
        public void AddAllDeploymentNodes()
        {
            Model.DeploymentNodes.ToList().ForEach(Add);
        }

        /// <summary>
        /// Adds a deployment node to this view.
        /// </summary>
        /// <param name="deploymentNode">the DeploymentNode to add</param>
        public void Add(DeploymentNode deploymentNode)
        {
            if (deploymentNode != null)
            {
                if (AddContainerInstancesAndDeploymentNodes(deploymentNode))
                {
                    Element parent = deploymentNode.Parent;
                    while (parent != null)
                    {
                        AddElement(parent, false);
                        parent = parent.Parent;
                    }
                }
            }
        }

        private bool AddContainerInstancesAndDeploymentNodes(DeploymentNode deploymentNode)
        {
            bool hasContainers = false;
            foreach (ContainerInstance containerInstance in deploymentNode.ContainerInstances) {
                Container container = containerInstance.Container;
                if (SoftwareSystem == null || container.Parent.Equals(SoftwareSystem))
                {
                    AddElement(containerInstance, true);
                    hasContainers = true;
                }
            }

            foreach (DeploymentNode child in deploymentNode.Children)
            {
                hasContainers = hasContainers | AddContainerInstancesAndDeploymentNodes(child);
            }

            if (hasContainers)
            {
                AddElement(deploymentNode, false);
            }

            return hasContainers;
        }

        /// <summary>
        /// Removes a deployment node from this view.
        /// </summary>
        /// <param name="deploymentNode">the DpeloymentNode to remove</param>
        public void Remove(DeploymentNode deploymentNode)
        {
            RemoveElement(deploymentNode);
        }

        public override string Name
        {
            get
            {
                if (SoftwareSystem != null)
                {
                    return SoftwareSystem.Name + " - Deployment";
                }
                else
                {
                    return "Deployment";
                }
            }
        }
        
    }
}