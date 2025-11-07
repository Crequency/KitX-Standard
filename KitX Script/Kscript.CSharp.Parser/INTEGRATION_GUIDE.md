# KitX.CSharp.Parser 与 KitX Dashboard 集成指南

## 🎯 集成概述

本文档说明如何将 KitX.CSharp.Parser 的实际插件管理器集成到 KitX Dashboard 中，实现从 MockPluginManager 到真实插件调用的升级。

## 📋 完成的集成工作

### 1. 创建 RealPluginManager
- ✅ **文件**: [`Core/RealPluginManager.cs`](../Core/RealPluginManager.cs:1-225)
- ✅ **功能**: 实现了 `IPluginManager` 接口，支持真实的 KitX Dashboard 插件调用
- ✅ **设计**: 采用依赖注入模式，接受外部传入的 `PluginsServer` 实例和日志记录器
- ✅ **兼容性**: 完全兼容现有的 MockPluginManager 接口

### 2. 扩展 MethodEmitter
- ✅ **静态支持**: 在 [`CodeGen/MethodEmitter.cs`](../CodeGen/MethodEmitter.cs:13-21) 中添加了静态插件管理器支持
- ✅ **工厂模式**: 实现了 `SetStaticPluginManager()` 方法
- ✅ **灵活配置**: 支持多种插件管理器实现方式

### 3. 更新 Parser 主入口
- ✅ **工厂函数**: 在 [`Parser.cs`](../Parser.cs:23-32) 中添加了 `SetPluginManagerFactory()` 方法
- ✅ **向后兼容**: 保持与现有 `SetDefaultPluginManager()` 的完全兼容
- ✅ **自动设置**: 在生成程序集时自动设置静态插件管理器

### 4. 集成到 Dashboard
- ✅ **WorkflowScriptService**: 修改了 [`Services/WorkflowScriptService.cs`](../../KitX%20Clients/KitX%20Dashboard/KitX%20Dashboard/Services/WorkflowScriptService.cs:17-52)
- ✅ **静态构造**: 在静态构造函数中设置 RealPluginManager 工厂
- ✅ **实例传递**: 将PluginManager实例传递给 Parser
- ✅ **错误处理**: 添加了完善的异常处理和日志记录

## 🔧 集成架构

```
┌─────────────────────────────────────────────────┐
│              KitX Dashboard                 │
│  ┌─────────────────────────────────────┐    │
│  │    WorkflowScriptService        │    │
│  │  ┌─ SetPluginManagerFactory  │    │
│  │  │ │                         │    │
│  │  │ ▼                        │    │
│  │  │ RealPluginManager          │    │
│  │  │ ┌─ PluginsServer         │    │
│  │  │ │ ┌─ FindConnector      │    │
│  │  │ │ │                   │    │
│  │  │ │ ▼                   │    │
│  │  │ │ PluginConnector       │    │
│  │  │ │ ┌─ Request            │    │
│  │  │ │ │                   │    │
│  │  │ │ ▼                   │    │
│  │  │ │ WebSocket           │    │
│  │  │ └─ Plugin Process     │    │
│  │  └─────────────────────────────┘    │
│  │                               │    │
│  │ ▼ Generated Assembly              │    │
│  │ ┌─ MethodEmitter              │    │
│  │ │ │ ┌─ Static Constructor   │    │
│  │ │ │ │                   │    │
│  │ │ │ ▼                   │    │
│  │ │ │ │ SetStaticPluginManager │    │
│  │ │ │ └─────────────────────┘    │
│  │ │                           │    │
│  │ ▼ Script Engine              │    │
│  │ └─ CSharpScriptEngine       │    │
│  │                               │    │
│  │                               │    │
│  │                               │    │
│  │                               │    │
│  └─────────────────────────────────┘    │
│                                     │
│          Workflow Script Execution      │
│          ────────────────────────┘
└─────────────────────────────────────────┘
```

## 🚀 使用方法

### 在 KitX Dashboard 中启用 RealPluginManager

在 KitX Dashboard 启动时，`WorkflowScriptService` 的静态构造函数会自动执行以下操作：

1. **获取 PluginsServer 实例**: 获取 `KitX.Dashboard.Network.PluginsNetwork.PluginsServer.Instance`
2. **设置工厂函数**: 调用 `Parser.SetPluginManagerFactory()`
3. **创建 RealPluginManager**: 传入 `PluginsServer` 实例和日志记录器
4. **自动集成**: 后续的所有 Workflow Script 执行都将使用真实的插件系统

### 验证集成

集成成功后，您可以通过以下方式验证：

1. **编译 Dashboard**:
   ```bash
   cd "KitX Clients/KitX Dashboard"
   dotnet build
   ```

2. **运行 Dashboard**: 启动后检查Logger输出
   ```
   [WorkflowScriptService] RealPluginManager 工厂函数已设置
   ```

3. **测试 Workflow Script**: 创建并执行包含插件调用的脚本

## 📝 代码示例

### 基本集成
```csharp
// 在 Dashboard 的初始化代码中（通常在 App.xaml.cs 或 Program.cs）
// WorkflowScriptService 的静态构造函数会自动执行此代码

// 手动设置（可选）
Parser.SetPluginManagerFactory(() => new RealPluginManager(
    KitX.Dashboard.Network.PluginsNetwork.PluginsServer.Instance,
    message => Log.Information(message) // 使用 Serilog
));
```

### 脚本中使用
```csharp
// 现在脚本中可以直接调用，将通过真实的 KitX Dashboard 插件系统执行
var result = SampleCalculator.Add(10, 20);
var text = StringToolkit.Reverse("Hello World");
```

## ⚙️ 配置选项

### 工厂函数 vs 默认管理器
- **工厂函数** (`SetPluginManagerFactory`): 推荐用于生产环境
- **默认管理器** (`SetDefaultPluginManager`): 适用于简单场景或测试

### 日志记录器
- **开发阶段**: 使用 `Console.WriteLine` 或 `Debug.WriteLine`
- **生产阶段**: 使用 `Serilog.Log.Information` 或其他日志框架

## 🔍 故障排除

### 常见问题

1. **编译错误**: 确保已正确引用 `Kscript.CSharp.Parser` 项目
2. **运行时错误**: 检查 `PluginsServer.Instance` 是否可用
3. **反射异常**: 确保 Dashboard 程序集已正确加载

### 调试技巧

1. **启用详细日志**: 在 `RealPluginManager` 构造函数中添加更多日志
2. **检查插件状态**: 验证插件是否正确连接到 Dashboard
3. **测试单独组件**: 分别测试 `RealPluginManager` 和 `PluginsServer` 连接

## 📈 性能考虑

- ✅ **延迟初始化**: RealPluginManager 只在首次使用时创建
- ✅ **缓存机制**: 保持原有的程序集缓存功能
- ✅ **内存效率**: 静态插件管理器实例，避免重复创建

## 🎉 总结

通过这次集成，KitX.CSharp.Parser 现在完全支持：

1. **真实的插件调用**: 通过 KitX Dashboard 的插件系统
2. **灵活的配置**: 支持多种插件管理器实现
3. **向后兼容性**: 不影响现有代码
4. **生产就绪**: 具备完整的错误处理和日志记录

现在 Workflow Script 中的插件调用将直接通过 KitX Dashboard 的真实插件系统执行，实现了从模拟到真实的完整升级！
