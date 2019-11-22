using System.Runtime.Serialization;
using Structurizr.Documentation;

namespace Structurizr
{

    /// <summary>
    /// Represents a Structurizr workspace, which is a wrapper for a software architecture model and the associated views.
    /// </summary>
    [DataContract]
    public class Workspace : AbstractWorkspace
    {

        /// <summary>
        /// The software architecture model.
        /// </summary>
        /// <value>The software architecture model.</value>
        [DataMember(Name = "model", EmitDefaultValue = false)]
        public Model Model { get; set; }

        /// <summary>
        /// The set of views onto a software architecture model.
        /// </summary>
        /// <value>The set of views onto a software architecture model.</value>
        [DataMember(Name = "views", EmitDefaultValue = false)]
        public ViewSet Views { get; set; }

        /// <summary>
        /// The documentation associated with this workspace.
        /// </summary>
        [DataMember(Name = "documentation", EmitDefaultValue = false)]
        public Documentation.Documentation Documentation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Workspace" />class.
        /// </summary>
        /// <param name="Name">The name of the workspace..</param>
        /// <param name="Description">A short description of the workspace..</param>
        public Workspace(string name, string description) : base(name, description)
        {
            this.Model = new Model();
            this.Views = new ViewSet(Model);
            this.Documentation = new Documentation.Documentation(Model);
        }

        public void Hydrate()
        {
            this.Views.Model = Model;
            this.Documentation.Model = Model;

            this.Model.Hydrate();
            this.Views.Hydrate();
            this.Documentation.Hydrate();
        }

    }
}
