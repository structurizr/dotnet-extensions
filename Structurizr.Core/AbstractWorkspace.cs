using System;
using System.Runtime.Serialization;
using Structurizr.Config;

namespace Structurizr
{

    [DataContract]
    public abstract class AbstractWorkspace
    {

        /// <summary>
        /// The ID of the workspace. 
        /// </summary>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public long Id { get; set; }

        /// <summary>
        /// The name of the workspace.
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// A short description of the workspace.
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// The last modified date of the workspace.
        /// </summary>
        [DataMember(Name = "lastModifiedDate", EmitDefaultValue = false)]
        public DateTime LastModifiedDate { get; set; }

        /// <summary>
        /// The name of the user who last modified this workspace (e.g. a username).
        /// </summary>
        [DataMember(Name = "lastModifiedUser", EmitDefaultValue = false)]
        public string LastModifiedUser { get; set; }

        /// <summary>
        /// The name of the agent that was used to last modify this workspace (e.g. "Structurizr for .NET").
        /// </summary>
        [DataMember(Name = "lastModifiedAgent", EmitDefaultValue = false)]
        public string LastModifiedAgent { get; set; }

        /// <summary>
        /// The version of the workspace.
        /// </summary>
        [DataMember(Name = "version", EmitDefaultValue = false)]
        public string Version { get; set; }

        /// <summary>
        /// The revision number of the workspace.
        /// </summary>
        [DataMember(Name = "revision", EmitDefaultValue = false)]
        public long Revision { get; set; }

        /// <summary>
        /// The thumbnail associated with the workspace; a Base64 encoded PNG file as a Data URI (data:image/png;base64).
        /// </summary>
        /// <value>The thumbnail associated with the workspace; a Base64 encoded PNG file as a Data URI (data:image/png;base64).</value>
        [DataMember(Name = "thumbnail", EmitDefaultValue = false)]
        public string Thumbnail { get; set; }

        [DataMember(Name = "configuration", EmitDefaultValue = false)]
        public WorkspaceConfiguration Configuration { get; set; }

        public AbstractWorkspace() { }

        public AbstractWorkspace(string name, string description)
        {
            this.Name = name;
            this.Description = description;
            
            this.Configuration = new WorkspaceConfiguration();
        }

        public void ClearConfiguration()
        {
            Configuration = null;
        }

    }
}