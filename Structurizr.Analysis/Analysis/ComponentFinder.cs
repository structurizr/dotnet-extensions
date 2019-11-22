using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Structurizr.Analysis
{

    /// <summary>
    /// This class allows you to find components in a .NET codebase, when used in conjunction
    /// with a number of pluggable component finder strategies.
    /// </summary>
    public class ComponentFinder
    {

        /// <summary>
        /// The Container that components will be added to.
        /// </summary>
        public Container Container { get; }

        /// <summary>
        /// The name of the namespace to be scanned. 
        /// </summary>
        public string Namespace { get; }

        /// <summary>
        /// The set of regexes that define which types should be excluded during the component finding process.
        /// </summary>
        public HashSet<Regex> Exclusions { get; }

        private readonly List<ComponentFinderStrategy> _componentFinderStrategies = new List<ComponentFinderStrategy>();

        /// <summary>
        /// Create a new component finder.
        /// </summary>
        /// <param name="container">The Container that components will be added to</param>
        /// <param name="namespaceToScan">The .NET namespace name to be scanned (e.g. "MyCompany.MyApp")</param>
        /// <param name="componentFinderStrategies">One or more ComponentFinderStrategy objects, describing how to find components</param>
        public ComponentFinder(Container container, string namespaceToScan, params ComponentFinderStrategy[] componentFinderStrategies)
        {
            if (container == null)
            {
                throw new ArgumentException("A container must be specified.");
            }

            if (namespaceToScan == null || namespaceToScan.Trim().Length == 0)
            {
                throw new ArgumentException("A package name must be specified.");
            }

            if (componentFinderStrategies.Length == 0)
            {
                throw new ArgumentException("One or more ComponentFinderStrategy objects must be specified.");
            }

            Container = container;
            Namespace = namespaceToScan;

            Exclusions = new HashSet<Regex>();
            Exclusions.Add(new Regex(@"System\."));

            foreach (ComponentFinderStrategy componentFinderStrategy in componentFinderStrategies)
            {
                _componentFinderStrategies.Add(componentFinderStrategy);
                componentFinderStrategy.ComponentFinder = this;
            }
        }

        /// <summary>
        /// Find components, using all of the configured component finder strategies
        /// in the order they were added.
        /// </summary>
        /// <returns>The set of components that were found.</returns>
        public ICollection<Component> FindComponents()
        {
            List<Component> componentsFound = new List<Component>();

            foreach (ComponentFinderStrategy componentFinderStrategy in _componentFinderStrategies)
            {
                componentFinderStrategy.BeforeFindComponents();
            }

            foreach (ComponentFinderStrategy componentFinderStrategy in _componentFinderStrategies) {
                componentsFound.AddRange(componentFinderStrategy.FindComponents());
            }

            foreach (ComponentFinderStrategy componentFinderStrategy in _componentFinderStrategies)
            {
                componentFinderStrategy.AfterFindComponents();
            }

            return componentsFound;
        }

        /// <summary>
        /// Adds one or more regexes to the set of regexes that define which types should be excluded during the component finding process.
        /// </summary>
        /// <param name="regexes">One or more regular expressions, as strings.</param>
        public void Exclude(params string[] regexes)
        {
            foreach (string regex in regexes)
            {
                Exclusions.Add(new Regex(regex));
            }
        }

    }
}