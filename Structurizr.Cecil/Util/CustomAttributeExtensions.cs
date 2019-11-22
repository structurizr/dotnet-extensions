using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;

namespace Structurizr.Cecil.Util
{
    /// <summary>
    /// Provides extension methods for working with custom attributes.
    /// </summary>
    static class CustomAttributeExtensions
    {
        /// <summary>
        /// Retrieves custom attributes of a specified type that are applied to a specified type.
        /// </summary>
        /// <param name="klassType">The type to inspect.</param>
        /// <typeparam name="TAttribute">The type of attribute to search for.</typeparam>
        /// <returns>
        /// A collection of custom attributes which match <typeparamref name="TAttribute"/>, or an empty enumeration if
        /// no such attribute is found.
        /// </returns>
        public static IEnumerable<TAttribute> ResolvableAttributes<TAttribute>(this TypeDefinition klassType)
            where TAttribute : Attribute
        {
            var attributeType = typeof(TAttribute).GetTypeInfo();

            foreach (var customAttribute in klassType.CustomAttributes)
            {
                if (!customAttribute.Is<TAttribute>()) continue;

                var arguments = customAttribute.ConstructorArguments.Select(a => a.Value).ToArray();

                var createdAttribute = (TAttribute)Activator.CreateInstance(typeof(TAttribute), arguments);
                foreach (var attributeProperty in customAttribute.Properties)
                {
                    var propertyInfo = attributeType.GetDeclaredProperty(attributeProperty.Name);
                    propertyInfo.SetValue(createdAttribute, attributeProperty.Argument.Value);
                }

                yield return createdAttribute;
            }
        }

        /// <summary>
        /// Retrieves custom attributes of a specified type that are applied to a specified field.
        /// </summary>
        /// <param name="field">The field to inspect.</param>
        /// <typeparam name="TAttribute">The type of attribute to search for.</typeparam>
        /// <returns>
        /// A collection of custom attributes which match <typeparamref name="TAttribute"/>, or an empty enumeration if
        /// no such attribute is found.
        /// </returns>
        public static IEnumerable<TAttribute> ResolvableAttributes<TAttribute>(this FieldDefinition field)
            where TAttribute : Attribute
        {
            var attributeType = typeof(TAttribute).GetTypeInfo();

            foreach (var customAttribute in field.CustomAttributes)
            {
                if (!customAttribute.Is<TAttribute>()) continue;

                var arguments = customAttribute.ConstructorArguments.Select(a => a.Value).ToArray();

                var createdAttribute = (TAttribute)Activator.CreateInstance(typeof(TAttribute), arguments);
                foreach (var attributeProperty in customAttribute.Properties)
                {
                    var propertyInfo = attributeType.GetDeclaredProperty(attributeProperty.Name);
                    propertyInfo.SetValue(createdAttribute, attributeProperty.Argument.Value);
                }

                yield return createdAttribute;
            }
        }

        /// <summary>
        /// Retrieves custom attributes of a specified type that are applied to a specified property.
        /// </summary>
        /// <param name="field">The property to inspect.</param>
        /// <typeparam name="TAttribute">The type of attribute to search for.</typeparam>
        /// <returns>
        /// A collection of custom attributes which match <typeparamref name="TAttribute"/>, or an empty enumeration if
        /// no such attribute is found.
        /// </returns>
        public static IEnumerable<TAttribute> ResolvableAttributes<TAttribute>(this PropertyDefinition property)
            where TAttribute : Attribute
        {
            var attributeType = typeof(TAttribute).GetTypeInfo();

            foreach (var customAttribute in property.CustomAttributes)
            {
                if (!customAttribute.Is<TAttribute>()) continue;

                var arguments = customAttribute.ConstructorArguments.Select(a => a.Value).ToArray();

                var createdAttribute = (TAttribute)Activator.CreateInstance(typeof(TAttribute), arguments);
                foreach (var attributeProperty in customAttribute.Properties)
                {
                    var propertyInfo = attributeType.GetDeclaredProperty(attributeProperty.Name);
                    propertyInfo.SetValue(createdAttribute, attributeProperty.Argument.Value);
                }

                yield return createdAttribute;
            }
        }

        /// <summary>
        /// Retrieves custom attributes of a specified type that are applied to a specified parameter.
        /// </summary>
        /// <param name="field">The property to inspect.</param>
        /// <typeparam name="TAttribute">The type of attribute to search for.</typeparam>
        /// <returns>
        /// A collection of custom attributes which match <typeparamref name="TAttribute"/>, or an empty enumeration if
        /// no such attribute is found.
        /// </returns>
        public static IEnumerable<TAttribute> ResolvableAttributes<TAttribute>(this ParameterDefinition parameter)
            where TAttribute : Attribute
        {
            var attributeType = typeof(TAttribute).GetTypeInfo();

            foreach (var customAttribute in parameter.CustomAttributes)
            {
                if (!customAttribute.Is<TAttribute>()) continue;

                var arguments = customAttribute.ConstructorArguments.Select(a => a.Value).ToArray();

                var createdAttribute = (TAttribute)Activator.CreateInstance(typeof(TAttribute), arguments);
                foreach (var attributeProperty in customAttribute.Properties)
                {
                    var propertyInfo = attributeType.GetDeclaredProperty(attributeProperty.Name);
                    propertyInfo.SetValue(createdAttribute, attributeProperty.Argument.Value);
                }

                yield return createdAttribute;
            }
        }

        private static bool Is<TAttribute>(this CustomAttribute attribute)
        {
            return attribute.AttributeType.FullName == typeof(TAttribute).FullName;
        }
    }
}
