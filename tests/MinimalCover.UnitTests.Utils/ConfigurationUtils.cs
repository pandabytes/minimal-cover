using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace MinimalCover.UnitTests.Utils
{
  /// <summary>
  /// Utilities for working with <see cref="IConfiguration"/>
  /// </summary>
  public static class ConfigurationUtils
  {
    /// <summary>
    /// Check if given type is enumerable, aka can
    /// we iterate the object with the given type
    /// </summary>
    /// <remarks>
    /// This method checks if type implements <see cref="IEnumerable{T}"/>
    /// </remarks>
    /// <param name="type">Type</param>
    /// <returns>True if type is enumerable. False otherwise</returns>
    private static bool IsEnumerable(Type type)
    {
      var interfaces = type.GetInterfaces();
      return interfaces.Any(interf => interf == typeof(IEnumerable));
    }

    /// <summary>
    /// Check if given type is a primitive type
    /// </summary>
    /// <remarks>
    /// This method considers <see cref="string"/> to be primitive as well
    /// </remarks>
    /// <param name="type">Type</param>
    /// <returns>True if type is primitive. False otherwise</returns>
    private static bool IsPrimitive(Type type)
      => type.IsPrimitive || type == typeof(string);

    /// <summary>
    /// Create an in-memory configuration that is stored
    /// in <see cref="Dictionary{string, string}"/>
    /// </summary>
    /// <remarks>
    /// This method recursively adds all public properties in <paramref name="obj"/>
    /// to <paramref name="dict"/>. The key will be something like
    /// "Root:PropertyName1:NestedProperty1"
    /// "Root:PropertyName2"
    /// 
    /// Note that properties that are null or have their ToString() return null will be skipped
    /// </remarks>
    /// <param name="obj">Object to build configuration from</param>
    /// <param name="levelKey">The key at a particular level in the configuration</param>
    /// <param name="dict">The dictionary that stores the configuration. Should be empty when first provided</param>
    private static void CreateInMemoryConfig<T>(T obj, string levelKey, Dictionary<string, string> dict) where T : class
    {
      var properties = obj.GetType().GetProperties();
      foreach (var property in properties)
      {
        var propName = property.Name;
        var propValue = property.GetValue(obj);
        var propStringValue = propValue?.ToString();

        if (propValue != null && propStringValue != null)
        {
          if (IsPrimitive(property.PropertyType))
          {
            dict.Add($"{levelKey}:{propName}", propStringValue);
          }
          else if (IsEnumerable(property.PropertyType))
          {
            // Add each item that is primtive in the collection to the dictionary and
            // recursively do the same for non-primitive items
            int index = 0;
            foreach (var item in (IEnumerable)propValue)
            {
              if (IsPrimitive(item.GetType()))
              {
                // Use null forgiving here because primitive always has ToString implemented
                // If null reference exception occurs here, it means IsPrimitive is incorrectly implemented
                dict.Add($"{levelKey}:{propName}:{index}", item.ToString()!);
              }
              else
              {
                // Recursive call
                CreateInMemoryConfig(item, $"{levelKey}:{propName}:{index}", dict);
              }
              index++;
            }
          }
          else
          {
            // These properties are classes so recursively add the properties
            // in these classes
            CreateInMemoryConfig(propValue, $"{levelKey}:{propName}", dict);
          }
        }
      }
    }

    /// <summary>
    /// Create a configuration object given an object and a root key where the 
    /// provider is a in-memory <see cref="Dictionary{string, string}"/>
    /// </summary>
    /// <remarks>
    /// This method recursively uses all public properties in <paramref name="obj"/>
    /// to create a configuration object. The key will be something like
    /// "Root:PropertyName1:NestedProperty1"
    /// "Root:PropertyName2"
    /// </remarks>
    /// <typeparam name="T">Type that is a class with public get properties</typeparam>
    /// <param name="obj">Object that stores values that will be stored in a configuration object</param>
    /// <param name="rootKey">The root key that tells where to store the configuration</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="rootKey"/> is null or empty</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="obj"/> is null</exception>
    /// <returns><see cref="IConfiguration"/> object</returns>
    public static IConfiguration CreateConfig<T>(T obj, string rootKey) where T : class
    {
      if (string.IsNullOrWhiteSpace(rootKey))
      {
        throw new ArgumentException($"{nameof(rootKey)} {rootKey} cannot be null or empty");
      }

      var dict = new Dictionary<string, string>();
      CreateInMemoryConfig(obj, rootKey, dict);

      var config = new ConfigurationBuilder()
                    .AddInMemoryCollection(dict)
                    .Build();
      return config;
    }

    /// <summary>
    /// Update <paramref name="configuration"/> with additional configuration
    /// take from <paramref name="obj"/>
    /// </summary>
    /// <remarks>
    /// The provider for the new configuration is a in-memory <see cref="Dictionary{string, String}"/>
    /// </remarks>
    /// <typeparam name="T">Type that is a class with public get properties</typeparam>
    /// <param name="configuration">Old configuration object</param>
    /// <param name="obj">Object that stores values that will be stored in a configuration object</param>
    /// <param name="rootKey">The root key that tells where to store the configuration</param>
    /// <returns><see cref="IConfiguration"/> object</returns>
    public static IConfiguration UpdateConfig<T>(this IConfiguration configuration, T obj, string rootKey) where T : class
    {
      var dict = new Dictionary<string, string>();
      CreateInMemoryConfig(obj, rootKey, dict);

      var updatedConfig = new ConfigurationBuilder()
                            .AddConfiguration(configuration)
                            .AddInMemoryCollection(dict)
                            .Build();
      return updatedConfig;
    }

    /// <summary>
    /// Represent an empty configuration
    /// </summary>
    public static readonly IConfiguration EmptyConfiguration = new ConfigurationBuilder().Build();
  }
}
