using System.Collections.Generic;

namespace Structurizr.Analysis
{

    /// <summary>
    /// The interface that all component finder strategies must implement.
    /// </summary>
    public interface ComponentFinderStrategy
    {

        /// <summary>
        /// A reference to the parent ComponentFinder.
        /// </summary>
        ComponentFinder ComponentFinder {set; }

        /// <summary>
        /// Called before all component finder strategies belonging to the
        /// same component finder are asked to find components.
        /// </summary>
        void BeforeFindComponents();

        /// <summary>
        /// Finds components.
        /// </summary>
        IEnumerable<Component> FindComponents();

        /// <summary>
        /// Called after all component finder strategies belonging to the
        /// same component finder have found components. This can be used
        /// to supplement the component with more information, such as
        /// dependencies.
        /// </summary>
        void AfterFindComponents();

    }

}