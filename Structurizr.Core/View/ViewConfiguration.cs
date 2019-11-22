using System.Runtime.Serialization;
using Structurizr.Core.View;

namespace Structurizr
{

    /// <summary>
    /// The configuration associated with a set of views.
    /// </summary>
    [DataContract]
    public sealed class ViewConfiguration
    {

        internal ViewConfiguration()
        {
            this.Styles = new Styles();
            this.Branding = new Branding();
            this.Terminology = new Terminology();
        }

        [DataMember(Name = "styles", EmitDefaultValue = false)]
        public Styles Styles { get; internal set; }

        [DataMember(Name = "branding", EmitDefaultValue = false)]
        public Branding Branding { get; internal set; }

        [DataMember(Name = "terminology", EmitDefaultValue = false)]
        public Terminology Terminology { get; internal set; }

        [DataMember(Name = "defaultView", EmitDefaultValue = false)]
        public string DefaultView { get; private set; }

        /// <summary>
        /// Sets the view that should be shown by default.
        /// </summary>
        /// <param name="view">A View object</param>
        public void SetDefaultView(View view)
        {
            if (view != null)
            {
                this.DefaultView = view.Key;
            }
        }

        [DataMember(Name = "lastSavedView", EmitDefaultValue = false)]
        internal string LastSavedView { get; set; }

        public void CopyConfigurationFrom(ViewConfiguration configuration)
        {
            LastSavedView = configuration.LastSavedView;
        }

        [DataMember(Name = "viewSortOrder", EmitDefaultValue = true)]
        public ViewSortOrder ViewSortOrder { get; set; }

    }
}
