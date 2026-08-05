# KitX.Core.Contract.CSharp

Core service contracts for KitX Dashboard written in C#.

## Overview

This project defines the service interfaces that separate the UI layer (Dashboard) from the core business logic layer. These interfaces enable:

- **Dependency Injection**: Core services can be injected into ViewModels
- **Testability**: Core logic can be tested independently of the UI
- **Flexibility**: Multiple frontends (Dashboard, CLI, etc.) can use the same core services
- **Maintainability**: Clear boundaries between UI and business logic

## Architecture

```
UI Layer (Dashboard, CLI, etc.)
    ↓ depends on
Core Service Contracts (interfaces)
    ↓ implemented by
Core Service Implementations (Managers)
```

## Service Interfaces

### Configuration
- `IConfigService` - Configuration management service
- `IAppConfig` - Application configuration
- `IPluginsConfig` - Plugins configuration
- `ISecurityConfig` - Security configuration

### Plugin Management
- `IPluginService` - Plugin lifecycle management
- `IPluginServer` - WebSocket server for plugin connections
- `IPluginConnector` - Individual plugin connection handler

### Device Management
- `IDeviceService` - Device discovery and management
- `IDeviceDiscoveryService` - UDP broadcast device discovery
- `IDeviceServer` - HTTP API server for device communication
- `IDevicesOrganizer` - Device organization and tracking

### Security
- `ISecurityService` - Encryption, decryption, and device key management

### Activity Logging
- `IActivityService` - Activity recording and statistics

### Statistics
- `IStatisticsService` - Application usage statistics

### Workflow
- `IWorkflowService` - Workflow script execution
- `IPluginServiceProvider` - Plugin integration for workflow scripts

### Event System
- `IEventService` - Global event bus for component communication

### Task Management
- `ITasksService` - Background task execution

### File Watching
- `IFileWatcherService` - File system monitoring for hot reload

### Hotkeys
- `IKeyHookService` - Global hotkey registration and handling

### Announcements
- `IAnnouncementService` - Announcement fetching and display

## Usage Example

```csharp
using KitX.Core.Contract.Configuration;
using KitX.Core.Contract.Plugin;

public class MyViewModel
{
    private readonly IConfigService _configService;
    private readonly IPluginService _pluginService;

    public MyViewModel(
        IConfigService configService,
        IPluginService pluginService)
    {
        _configService = configService;
        _pluginService = pluginService;

        // Subscribe to events
        _pluginService.PluginStatusChanged += OnPluginStatusChanged;
    }

    private void OnPluginStatusChanged(object? sender, PluginStatusChangedEventArgs e)
    {
        // Handle plugin status change
    }

    public async Task ImportPlugin(string filePath)
    {
        var success = await _pluginService.ImportPluginAsync(filePath);
        if (success)
        {
            _configService.SaveAll();
        }
    }
}
```

## Dependencies

- .NET Standard 2.0/2.1
- KitX.Shared.CSharp - Shared data models

## License

AGPL-3.0-only

## Links

- [KitX Repository](https://github.com/Crequency/KitX/)
- [KitX Standard Repository](https://github.com/Crequency/KitX-Standard/)
