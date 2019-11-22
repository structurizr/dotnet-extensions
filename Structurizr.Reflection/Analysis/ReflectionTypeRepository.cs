using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Structurizr.Analysis
{

    /// <summary>
    /// Implements a type repository based on .NET reflection.
    /// </summary>
    public class ReflectionTypeRepository : ITypeRepository
    {

        private readonly string _namespace;
        private readonly HashSet<Regex> _exclusions = new HashSet<Regex>();
        private readonly Dictionary<string, Type> _types = new Dictionary<string, Type>();
        private readonly Dictionary<string, IEnumerable<string>> _referencedTypesCache = new Dictionary<string, IEnumerable<string>>();

        /// <inheritdoc />
        public string Namespace
        {
            get { return _namespace; }
        }

        /// <summary>
        /// Creates a new instance of <see cref="ReflectionTypeRepository"/>.
        /// </summary>
        /// <param name="namespaceName">The root namespace to use for limiting which types to analyze.</param>
        /// <param name="exclusions">A set of regular expressions to further exclude types by name from analysis.</param>
        public ReflectionTypeRepository(string namespaceName, HashSet<Regex> exclusions)
        {
            _namespace = namespaceName;

            if (exclusions != null)
            { 
                _exclusions.UnionWith(exclusions);
            }

            IEnumerable<Type> types = from a in AppDomain.CurrentDomain.GetAssemblies()
                                    from t in a.GetTypes()
                                    where InNamespace(t)
                                    select t;

            foreach (Type type in types)
            {
                if (type.AssemblyQualifiedName != null)
                {
                    _types.Add(type.AssemblyQualifiedName, type);
                }
            }
        }

        private bool InNamespace(Type type)
        {
            return type.Namespace != null && type.Namespace.StartsWith(_namespace);
        }

        /// <inheritdoc />
        public IEnumerable<Type> GetAllTypes()
        {
            return _types.Values;
        }

        /// <inheritdoc />
        public Type GetType(string typeName)
        {
            Func<Type, bool> predicate;
            var split = typeName.Split(new[] { ", " }, 2, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length == 2)
                predicate = t => t.FullName == split[0] && t.Module.Assembly.FullName == split[1];
            else
                predicate = t => t.FullName == typeName;

            return _types.Values.SingleOrDefault(predicate);
        }

        /// <inheritdoc />
        public IEnumerable<string> GetReferencedTypes(string typeName)
        {
            // use the cached version if possible
            if (_referencedTypesCache.ContainsKey(typeName))
            {
                return _referencedTypesCache[typeName];
            }

            HashSet<string> referencedTypes = new HashSet<string>();
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                                        BindingFlags.Static | BindingFlags.FlattenHierarchy;

            if (_types.TryGetValue(typeName, out Type type))
            {
                foreach (PropertyInfo propertyInfo in type.GetProperties(bindingFlags))
                {
                    AddReferencedTypeIfNotExcluded(propertyInfo.PropertyType, referencedTypes);
                }

                foreach (FieldInfo fieldInfo in type.GetFields(bindingFlags))
                {
                    AddReferencedTypeIfNotExcluded(fieldInfo.FieldType, referencedTypes);
                }

                foreach (MethodInfo methodInfo in type.GetMethods(bindingFlags))
                {
                    AddReferencedTypeIfNotExcluded(methodInfo.ReturnType, referencedTypes);

                    foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
                    {
                        AddReferencedTypeIfNotExcluded(parameterInfo.ParameterType, referencedTypes);
                    }

                    MethodBody methodBody = methodInfo.GetMethodBody();
                    if (methodBody != null)
                    {
                        foreach (LocalVariableInfo localVariableInfo in methodBody.LocalVariables)
                        {
                            var attr = Attribute.GetCustomAttribute(localVariableInfo.LocalType, typeof(CompilerGeneratedAttribute));
                            if (attr == null)
                            {
                                AddReferencedTypeIfNotExcluded(localVariableInfo.LocalType, referencedTypes);
                            }
                        }
                    }
                }
            }

            // cache for next time
            _referencedTypesCache[typeName] = referencedTypes;

            return referencedTypes;
        }

        private void AddReferencedTypeIfNotExcluded(Type type, HashSet<string> referencedTypes)
        {
            if (type.AssemblyQualifiedName != null)
            {
                if (!IsExcluded(type.AssemblyQualifiedName))
                {
                    referencedTypes.Add(type.AssemblyQualifiedName);
                }

                if (type.IsGenericType)
                {
                    foreach (Type genericArgumentType in type.GetGenericArguments())
                    {
                        AddReferencedTypeIfNotExcluded(genericArgumentType, referencedTypes);
                    }
                }
            }
        }

        /// <inheritdoc />
        public string FindVisibility(string typeName)
        {
            Type type = _types[typeName];
            if (type != null)
            {
                if (type.IsPublic)
                {
                    return "public";
                }
                else if (type.IsNestedAssembly)
                {
                    return "internal";
                }
            }

            // todo
            return null;
        }

        /// <inheritdoc />
        public string FindCategory(string typeName)
        {
            Type type = _types[typeName];
            if (type != null)
            {
                if (type.IsInterface)
                {
                    // IsAbstract=true for interfaces so we need to check this first!
                    return "interface";
                }
                else if (type.IsAbstract)
                {
                    if (type.IsSealed)
                    {
                        return "static class";
                    }
                    return "abstract class";
                }
                else if (type.IsEnum)
                {
                    return "enum";
                }
            }

            // todo
            return null;
        }

        private bool IsExcluded(string typeName)
        {
            foreach (Regex exclude in _exclusions)
            {
                if (exclude.IsMatch(typeName))
                {
                    return true;
                }
            }

            return false;
        }
    }

}
