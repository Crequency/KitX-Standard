using System.IO;
using System.ComponentModel;

namespace KitX.Core.Contract.FileWatcher;

/// <summary>
/// File watcher service interface
/// </summary>
public interface IFileWatcherService
{
    /// <summary>
    /// Registers a file watcher
    /// </summary>
    /// <param name="filePath">The file path to watch</param>
    /// <param name="onChanged">The callback when file changes</param>
    void RegisterWatcher(string filePath, FileSystemEventHandler onChanged);

    /// <summary>
    /// Unregisters a file watcher
    /// </summary>
    /// <param name="filePath">The file path to stop watching</param>
    void UnregisterWatcher(string filePath);

    /// <summary>
    /// Clears all file watchers
    /// </summary>
    void Clear();
}
