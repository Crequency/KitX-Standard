# KitX.CSharp.Parser
> 将插件清单 JSON 即时编译成可直接调用的 C# 静态 API

---

## 🌱 项目定位

KitX.CSharp.Parser 是 **KitX 工作流子系统** 的核心组件。它专注于一个核心功能：

**把插件清单（JSON）→ 运行时静态类/方法**，让脚本引擎可以像调用普通静态方法一样使用插件功能。

```
JSON 清单 ─→ Parser ─→ 内存程序集 ─→ 脚本侧: SamplePlugin.Method(...)
```

---

## 🎯 核心特性

- ✅ **动态 IL 生成** - 使用 `Reflection.Emit` 生成高性能静态方法
- ✅ **智能类型映射** - 支持基础类型和自定义类型
- ✅ **程序集缓存** - 避免重复生成，提升性能
- ✅ **简化设计** - 直接依赖注入，无复杂抽象层
- ✅ **异常处理** - 完善的错误处理机制
- ✅ **调试友好** - 详细的日志输出和统计信息

---

## 🚀 快速开始

### 基本用法

```csharp
using Kscript.CSharp.Parser;

// 1. 设置插件管理器（必须）
Parser.SetPluginManager(new MockPluginManager());

// 2. 从 JSON 文件生成程序集
var assembly = await Parser.GenerateFromFileAsync("plugins.json");

// 3. 从插件信息列表生成程序集
var plugins = new List<PluginInfo> { /* ... */ };
var assembly = Parser.Generate(plugins);

// 4. 脚本中直接调用生成的方法
int sum = SampleCalculator.Add(10, 20);
string reversed = StringToolkit.Reverse("Hello");
```

### 运行演示

```bash
# 构建项目
dotnet build

# 运行演示程序
dotnet run
```

---

## 🔌 API 参考

### Parser 静态类

#### 生成方法

| 方法 | 描述 | 参数 |
|------|------|------|
| `Generate()` | 从插件信息列表生成程序集 | `plugins`, `assemblyName`, `useCache` |
| `GenerateFromJson()` | 从 JSON 字符串生成程序集 | `jsonString`, `assemblyName`, `useCache` |
| `GenerateFromFileAsync()` | 从 JSON 文件异步生成程序集 | `jsonFilePath`, `assemblyName`, `useCache` |

#### 配置方法

| 方法 | 描述 |
|------|------|
| `SetPluginManager()` | 设置插件管理器实例 | `IPluginManager pluginManager` |
| `ClearCache()` | 清除所有缓存 |
| `GetCacheStatistics()` | 获取缓存统计信息 |

#### 分析方法

| 方法 | 描述 |
|------|------|
| `GetPluginTypes()` | 获取程序集中的所有插件类型 |
| `GetPluginMethods()` | 获取插件类型的所有方法信息 |

---

## 📊 类型映射支持

| JSON 字符串 | CLR 类型 | 示例 |
|---|---|---|
| `"void"` | `System.Void` | 无返回值 |
| `"bool"` | `System.Boolean` | 布尔参数 |
| `"int"` | `System.Int32` | 整数参数 |
| `"double"` | `System.Double` | 浮点数参数 |
| `"string"` | `System.String` | 字符串参数 |
| `"object"` | `System.Object` | 对象参数 |

---

## 🏗️ 项目结构

```
KitX.CSharp.Parser/
├─ Parser.cs                    # 主入口静态类
├─ Models/
│   └─ PluginCallInfo.cs       # 插件调用信息模型
├─ CodeGen/
│   ├─ TypeMapper.cs           # 字符串 → Type 映射
│   ├─ MethodEmitter.cs        # IL 方法生成器
│   └─ AssemblyCache.cs        # 程序集缓存管理
├─ Core/
│   ├─ IPluginManager.cs       # 插件管理器接口
│   └─ MockPluginManager.cs    # 模拟实现
├─ Exceptions/
│   └─ ParserException.cs      # 自定义异常
├─ Examples/
│   ├─ BasicUsageExample.cs        # 基础用法示例
│   └─ Program.cs                  # 演示程序入口
├─ example.json                    # 插件清单示例
├─ README.md                       # 项目说明
└─ USAGE.md                        # 详细使用指南
```

---

## 🎯 运行结果示例

```
🚀 欢迎使用 KitX.CSharp.Parser 演示程序!

=== 测试简化后的 Parser 功能 ===
1. 测试默认插件管理器设置...
   ✓ 默认 MockPluginManager 工作正常

2. 测试自定义插件管理器设置...
   ✓ 自定义插件管理器设置工作正常

3. 测试空参数处理...
   ✓ 空参数正确抛出 ArgumentNullException

4. 测试插件存在性检测...
   ✓ SampleCalculator 存在: True
   ✓ SampleCalculator.Add 方法存在: True
   ✓ NonExistentPlugin 存在: False
   ✓ SampleCalculator.NonExistentMethod 方法存在: False
   ✓ 插件存在性检测功能工作正常

=== KitX.CSharp.Parser 基础用法示例 ===
1. 加载插件清单...
   ✓ 成功生成程序集: ExamplePluginAssembly

2. 分析生成的插件类型...
   发现 2 个插件类:
   - SampleCalculator
     └── Int32 Add(Int32 a, Int32 b)
     └── Double Divide(Double numerator, Double denominator, Int32 decimals)
   - StringToolkit
     └── String Reverse(String text)
     └── String ToUpper(String text)

3. 动态调用插件方法...
   调用 Add(10, 20) → 结果: 30
   调用 Divide(10, 3, 2) → 结果: 3.33
   调用 Reverse(Hello) → 结果: olleH
   调用 ToUpper(world) → 结果: WORLD

4. 缓存统计信息...
   缓存统计: 1 个程序集

5. 测试缓存效果...
   第二次生成是否使用缓存: True
   强制重新生成是否使用新程序集: True
   重新生成后缓存存在: True

✅ 所有示例运行完成!
```

---

## 🔧 实际插件管理器集成

### 设置真实插件管理器

```csharp
// 在 KitX Dashboard 启动时设置
Parser.SetPluginManager(new RealPluginManager(
    KitX.Dashboard.Network.PluginsNetwork.PluginsServer.Instance,
    message => Log.Information(message)
));
```

### 在脚本中使用

```csharp
// 现在可以直接调用，将通过真实的 KitX Dashboard 插件系统执行
var result = SampleCalculator.Add(10, 20);
```

---

## 📝 注意事项

- ✅ **简化设计**：采用直接依赖注入，移除了复杂的工厂函数模式
- ✅ **必须初始化**：使用前必须调用 `SetPluginManager()` 设置插件管理器
- ✅ **插件验证**：生成程序集前会检查所需插件是否存在
- ✅ **缓存优化**：相同插件清单只生成一次，提升性能
- ✅ **异常安全**：完善的参数验证和错误处理机制

---

## 🎉 总结

KitX.CSharp.Parser 提供了一个简洁、高效、易维护的解决方案，用于将插件清单转换为可直接调用的 C# API。通过简化的设计和完善的错误处理，确保了在生产环境中的稳定性和可靠性。
