using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Structurizr
{

    /// <summary>
    /// The set of views onto a software architecture model.
    /// </summary>
    [DataContract]
    public sealed class ViewSet
    {

        public Model Model { get; set; }

        /// <summary>
        /// The set of enterprise context views (this is for backwards compatibility).
        /// </summary>
        [DataMember(Name = "enterpriseContextViews", EmitDefaultValue = false)]
        internal ISet<SystemLandscapeView> EnterpriseContextViews
        {
            set
            {
                foreach (SystemLandscapeView systemLandscapeView in value)
                {
                    _systemLandscapeViews.Add(systemLandscapeView);
                }
            }
        }

        private HashSet<SystemLandscapeView> _systemLandscapeViews;

        /// <summary>
        /// The set of system landscape views.
        /// </summary>
        [DataMember(Name = "systemLandscapeViews", EmitDefaultValue = false)]
        public ISet<SystemLandscapeView> SystemLandscapeViews
        {
            get
            {
                return new HashSet<SystemLandscapeView>(_systemLandscapeViews);
            }

            internal set
            {
                _systemLandscapeViews = new HashSet<SystemLandscapeView>(value);
            }
        }

        private HashSet<SystemContextView> _systemContextViews;

        /// <summary>
        /// The set of system context views.
        /// </summary>
        [DataMember(Name = "systemContextViews", EmitDefaultValue = false)]
        public ISet<SystemContextView> SystemContextViews
        {
            get
            {
                return new HashSet<SystemContextView>(_systemContextViews);
            }

            internal set
            {
                _systemContextViews = new HashSet<SystemContextView>(value);
            }
        }

        private HashSet<ContainerView> _containerViews;

        /// <summary>
        /// The set of container views.
        /// </summary>
        [DataMember(Name = "containerViews", EmitDefaultValue = false)]
        public ISet<ContainerView> ContainerViews
        {
            get
            {
                return new HashSet<ContainerView>(_containerViews);
            }

            internal set
            {
                _containerViews = new HashSet<ContainerView>(value);
            }
        }

        private HashSet<ComponentView> _componentViews;

        /// <summary>
        /// The set of component views.
        /// </summary>
        [DataMember(Name = "componentViews", EmitDefaultValue = false)]
        public ISet<ComponentView> ComponentViews
        {
            get
            {
                return new HashSet<ComponentView>(_componentViews);
            }

            internal set
            {
                _componentViews = new HashSet<ComponentView>(value);
            }
        }

        private HashSet<DynamicView> _dynamicViews;

        /// <summary>
        /// The set of dynamic views.
        /// </summary>
        [DataMember(Name = "dynamicViews", EmitDefaultValue = false)]
        public ISet<DynamicView> DynamicViews
        {
            get
            {
                return new HashSet<DynamicView>(_dynamicViews);
            }

            internal set
            {
                _dynamicViews = new HashSet<DynamicView>(value);
            }
        }

        private HashSet<DeploymentView> _deploymentViews;

        /// <summary>
        /// The set of deployment views.
        /// </summary>
        [DataMember(Name = "deploymentViews", EmitDefaultValue = false)]
        public ISet<DeploymentView> DeploymentViews
        {
            get
            {
                return new HashSet<DeploymentView>(_deploymentViews);
            }

            internal set
            {
                _deploymentViews = new HashSet<DeploymentView>(value);
            }
        }

        private HashSet<FilteredView> _filteredViews;

        /// <summary>
        /// The set of filtered views.
        /// </summary>
        [DataMember(Name = "filteredViews", EmitDefaultValue = false)]
        public ISet<FilteredView> FilteredViews
        {
            get
            {
                return new HashSet<FilteredView>(_filteredViews);
            }

            internal set
            {
                _filteredViews = new HashSet<FilteredView>(value);
            }
        }

        /// <summary>
        /// The configuration object associated with this set of views.
        /// </summary>
        [DataMember(Name = "configuration", EmitDefaultValue = false)]
        public ViewConfiguration Configuration { get; internal set; }

        internal ViewSet()
        {
            _systemLandscapeViews = new HashSet<SystemLandscapeView>();
            _systemContextViews = new HashSet<SystemContextView>();
            _containerViews = new HashSet<ContainerView>();
            _componentViews = new HashSet<ComponentView>();
            _dynamicViews = new HashSet<DynamicView>();
            _deploymentViews = new HashSet<DeploymentView>();
            _filteredViews = new HashSet<FilteredView>();

            Configuration = new ViewConfiguration();
        }

        internal ViewSet(Model model) : this()
        {
            Model = model;
        }

        public SystemLandscapeView CreateSystemLandscapeView(string key, string description)
        {
            AssertThatTheViewKeyIsUnique(key);

            SystemLandscapeView view = new SystemLandscapeView(Model, key, description);
            _systemLandscapeViews.Add(view);
            return view;
        }

        public SystemContextView CreateSystemContextView(SoftwareSystem softwareSystem, string key, string description)
        {
            AssertThatTheViewKeyIsUnique(key);

            SystemContextView view = new SystemContextView(softwareSystem, key, description);
            _systemContextViews.Add(view);

            return view;
        }

        public ContainerView CreateContainerView(SoftwareSystem softwareSystem, string key, string description)
        {
            AssertThatTheViewKeyIsUnique(key);

            ContainerView view = new ContainerView(softwareSystem, key, description);
            _containerViews.Add(view);

            return view;
        }

        public ComponentView CreateComponentView(Container container, string key, string description)
        {
            AssertThatTheViewKeyIsUnique(key);

            ComponentView view = new ComponentView(container, key, description);
            _componentViews.Add(view);

            return view;
        }

        public DynamicView CreateDynamicView(string key, string description)
        {
            AssertThatTheViewKeyIsUnique(key);

            DynamicView view = new DynamicView(Model, key, description);
            _dynamicViews.Add(view);
            return view;
        }

        public DynamicView CreateDynamicView(SoftwareSystem softwareSystem, string key, string description)
        {
            AssertThatTheSoftwareSystemIsNotNull(softwareSystem);
            AssertThatTheViewKeyIsUnique(key);

            DynamicView view = new DynamicView(softwareSystem, key, description);
            _dynamicViews.Add(view);
            return view;
        }

        public DynamicView CreateDynamicView(Container container, string key, string description)
        {
            AssertThatTheContainerIsNotNull(container);
            AssertThatTheViewKeyIsUnique(key);

            DynamicView view = new DynamicView(container, key, description);
            _dynamicViews.Add(view);
            return view;
        }
        
        /// <summary>
        /// Creates a deployment view.
        /// </summary>
        /// <returns>a DeploymentView object</returns>
        public DeploymentView CreateDeploymentView(String key, String description) {
            AssertThatTheViewKeyIsUnique(key);

            DeploymentView view = new DeploymentView(Model, key, description);
            _deploymentViews.Add(view);
            return view;
        }

        /// <summary>
        /// Creates a deployment view, where the scope of the view is the specified software system.
        /// </summary>
        /// <param name="softwareSystem">the SoftwareSystem object representing the scope of the view</param>
        /// <param name="key">the key for the deployment view (must be unique)</param>
        /// <param name="description">a description of the  view</param>
        /// <returns>a DeploymentView object</returns>
        public DeploymentView CreateDeploymentView(SoftwareSystem softwareSystem, String key, String description) {
            AssertThatTheSoftwareSystemIsNotNull(softwareSystem);
            AssertThatTheViewKeyIsUnique(key);

            DeploymentView view = new DeploymentView(softwareSystem, key, description);
            _deploymentViews.Add(view);
            return view;
        }


        /// <summary>
        /// Creates a FilteredView on top of an existing static view. 
        /// </summary>
        /// <param name="view">the static view to base the FilteredView upon</param>
        /// <param name="key">the key for the filtered view (must be unique)</param>
        /// <param name="description">a description of the filtered view</param>
        /// <param name="mode">whether to Include or Exclude elements/relationships based upon their tag</param>
        /// <param name="tags">the tags to include or exclude</param>
        /// <returns></returns>
        public FilteredView CreateFilteredView(StaticView view, string key, string description, FilterMode mode, params string[] tags)
        {
            AssertThatTheViewKeyIsUnique(key);

            FilteredView filteredView = new FilteredView(view, key, description, mode, tags);
            _filteredViews.Add(filteredView);
            
            return filteredView;
        }

        private void AssertThatTheViewKeyIsUnique(string key)
        {
            if (GetViewWithKey(key) != null || GetFilteredViewWithKey(key) != null)
            {
                throw new ArgumentException("A view with the key " + key + " already exists.");
            }
        }
        
        private void AssertThatTheSoftwareSystemIsNotNull(SoftwareSystem softwareSystem)
        {
            if (softwareSystem == null)
            {
                throw new ArgumentException("Software system must not be null.");
            }
        }

        private void AssertThatTheContainerIsNotNull(Container container)
        {
            if (container == null)
            {
                throw new ArgumentException("Container must not be null.");
            }
        }

        public void Hydrate()
        {
            foreach (SystemLandscapeView view in _systemLandscapeViews)
            {
                view.Model = Model;
                HydrateView(view);
            }

            foreach (SystemContextView view in _systemContextViews)
            {
                view.SoftwareSystem = Model.GetSoftwareSystemWithId(view.SoftwareSystemId);
                HydrateView(view);
            }

            foreach (ContainerView view in _containerViews)
            {
                view.SoftwareSystem = Model.GetSoftwareSystemWithId(view.SoftwareSystemId);
                HydrateView(view);
            }

            foreach (ComponentView view in _componentViews)
            {
                view.SoftwareSystem = Model.GetSoftwareSystemWithId(view.SoftwareSystemId);
                view.Container = view.SoftwareSystem.GetContainerWithId(view.ContainerId);
                HydrateView(view);
            }

            foreach (DynamicView view in _dynamicViews)
            {
                view.Model = Model;
                HydrateView(view);
            }
            
            foreach (DeploymentView view in _deploymentViews)
            {
                if (!String.IsNullOrEmpty(view.SoftwareSystemId))
                {
                    view.SoftwareSystem = Model.GetSoftwareSystemWithId(view.SoftwareSystemId);
                }
                view.Model = Model;
                HydrateView(view);
            }
            
            foreach (FilteredView filteredView in _filteredViews)
            {
                filteredView.View = GetViewWithKey(filteredView.BaseViewKey);
            }
        }

        private void HydrateView(View view)
        {
            foreach (ElementView elementView in view.Elements)
            {
                elementView.Element = Model.GetElement(elementView.Id);
            }
            foreach (RelationshipView relationshipView in view.Relationships)
            {
                relationshipView.Relationship = Model.GetRelationship(relationshipView.Id);
            }
        }

        public void CopyLayoutInformationFrom(ViewSet source)
        {
            foreach (SystemLandscapeView sourceView in source.SystemLandscapeViews)
            {
                SystemLandscapeView destinationView = FindSystemLandscapeView(sourceView);
                if (destinationView != null)
                {
                    destinationView.CopyLayoutInformationFrom(sourceView);
                }
            }

            foreach (SystemContextView sourceView in source.SystemContextViews)
            {
                SystemContextView destinationView = FindSystemContextView(sourceView);
                if (destinationView != null)
                {
                    destinationView.CopyLayoutInformationFrom(sourceView);
                }
            }

            foreach (ContainerView sourceView in source.ContainerViews)
            {
                ContainerView destinationView = FindContainerView(sourceView);
                if (destinationView != null)
                {
                    destinationView.CopyLayoutInformationFrom(sourceView);
                }
            }

            foreach (ComponentView sourceView in source.ComponentViews)
            {
                ComponentView destinationView = FindComponentView(sourceView);
                if (destinationView != null)
                {
                    destinationView.CopyLayoutInformationFrom(sourceView);
                }
            }

            foreach (DynamicView sourceView in source.DynamicViews)
            {
                DynamicView destinationView = FindDynamicView(sourceView);
                if (destinationView != null)
                {
                    destinationView.CopyLayoutInformationFrom(sourceView);
                }
            }
            
            foreach (DeploymentView sourceView in source.DeploymentViews)
            {
                DeploymentView destinationView = FindDeploymentView(sourceView);
                if (destinationView != null)
                {
                    destinationView.CopyLayoutInformationFrom(sourceView);
                }
            }
        }

        private SystemLandscapeView FindSystemLandscapeView(SystemLandscapeView systemLandscapeView)
        {
            return _systemLandscapeViews.FirstOrDefault(view => view.Key == systemLandscapeView.Key);
        }

        private SystemContextView FindSystemContextView(SystemContextView systemContextView)
        {
            return _systemContextViews.FirstOrDefault(view => view.Key == systemContextView.Key);
        }

        private ContainerView FindContainerView(ContainerView containerView)
        {
            return _containerViews.FirstOrDefault(view => view.Key == containerView.Key);
        }

        private ComponentView FindComponentView(ComponentView componentView)
        {
            return _componentViews.FirstOrDefault(view => view.Key == componentView.Key);
        }

        private DynamicView FindDynamicView(DynamicView dynamicView)
        {
            return _dynamicViews.FirstOrDefault(view => view.Key == dynamicView.Key);
        }

        private DeploymentView FindDeploymentView(DeploymentView deploymentView)
        {
            return _deploymentViews.FirstOrDefault(view => view.Key == deploymentView.Key);
        }

        /// <summary>
        /// Finds the view with the specified key, or null if the view does not exist.
        /// </summary>
        public View GetViewWithKey(string key)
        {
            if (key == null)
            {
                throw new ArgumentException("A key must be specified.");
            }
            
            foreach (SystemLandscapeView view in SystemLandscapeViews)
            {
                if (view.Key.Equals(key))
                {
                    return view;
                }
            }

            foreach (SystemContextView view in _systemContextViews)
            {
                if (view.Key.Equals(key))
                {
                    return view;
                }
            }

            foreach (ContainerView view in _containerViews)
            {
                if (view.Key.Equals(key))
                {
                    return view;
                }
            }

            foreach (ComponentView view in _componentViews)
            {
                if (view.Key.Equals(key))
                {
                    return view;
                }
            }

            foreach (DynamicView view in _dynamicViews)
            {
                if (view.Key.Equals(key))
                {
                    return view;
                }
            }

            foreach (DeploymentView view in _deploymentViews)
            {
                if (view.Key.Equals(key))
                {
                    return view;
                }
            }

            return null;
        }

        public FilteredView GetFilteredViewWithKey(string key)
        {
            if (key == null)
            {
                throw new ArgumentException("A key must be specified.");
            }

            foreach (FilteredView view in _filteredViews)
            {
                if (view.Key.Equals(key))
                {
                    return view;
                }
            }

            return null;
        }

    }
}