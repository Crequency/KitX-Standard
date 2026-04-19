namespace KitX.Core.Contract.Configuration;

/// <summary>
/// Saves configuration to files
/// </summary>
public interface IConfigSaver
{
    /// <summary>
    /// Saves a config to the specified location
    /// </summary>
    /// <typeparam name="T">Config type</typeparam>
    /// <param name="config">Config to save</param>
    /// <param name="location">Directory path</param>
    /// <param name="fileName">File name</param>
    void Save<T>(T config, string location, string fileName) where T : class;
}
