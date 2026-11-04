namespace KitX.Core.Contract.Configuration;

/// <summary>
/// Loads configuration from files
/// </summary>
public interface IConfigLoader
{
    /// <summary>
    /// Loads a config file from the specified location
    /// </summary>
    /// <typeparam name="T">Config type</typeparam>
    /// <param name="location">Directory path</param>
    /// <param name="fileName">File name</param>
    /// <returns>The loaded config or default</returns>
    T Load<T>(string location, string fileName) where T : class, new();

    /// <summary>
    /// Loads SecurityConfig with special handling
    /// </summary>
    ISecurityConf LoadSecurityConfig(string location);
}
