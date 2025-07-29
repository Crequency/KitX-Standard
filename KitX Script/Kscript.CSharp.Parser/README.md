# KitX.CSharp.Parser
> 将插件清单 JSON 即时编译成可直接调用的 C# 静态 API

---

## 🌱 项目定位
KitX.CSharp.Parser 是 **KitX 工作流子系统** 的第一环。
它只做一件事：
**把插件清单（JSON）→ 运行时静态类/方法**，
让后续脚本引擎（Roslyn、CSharpScript、Eval-etc.）可以像调用普通静态方法一样使用插件功能。

```
  JSON 清单  ─→  Parser  ─→  内存程序集(DynamicAssembly)
                                         │
                                         └─ 脚本侧: SamplePlugin.Run(…)
```

---

## 🎯 边界与职责
| 范围 | 属于本库 | 不属于本库 |
|---|---|---|
| 读取 JSON | ✅ 反序列化为 `PluginInfo` | ❌ 网络/磁盘 IO |
| 类型映射 | ✅ `"int"` → `System.Int32` | ❌ 复杂自定义类型序列化 |
| 生成 IL | ✅ 用 `Reflection.Emit` 生成静态类/方法 | ❌ 脚本执行、调试 |
| 生命周期 | ✅ 生成后缓存 `Assembly` | ❌ 热重载（由上层调用 `Regenerate()`） |
| 错误处理 | ✅ 语法/类型错误抛出 `ParserException` | ❌ 插件运行时异常 |

---

## 🧩 整体流程
1. **输入**
   `List<PluginInfo>`（已由外部从 JSON 反序列化好）。

2. **映射**
   把 `Function.ReturnValueType` / `Parameter.Type` 字符串映射到 `System.Type`。

3. **构建**
   - 每个插件 → 一个 `static class`（`TypeBuilder`）
   - 每个功能 → 一个 `public static` 方法（`MethodBuilder`）

4. **Emit**
   方法体内构造 `PluginCallInfo`，调用 `PluginManager.Call<T>()`，
   返回值拆箱后 `ret`。

5. **输出**
   程序集保存到内存（或可选磁盘 DLL），
   脚本侧通过 `PluginNamespace.PluginName.Func(...)` 直接调用。

---

## 🗂️ 目录结构（初始）
```
KitX.CSharp.Parser/
 ├─ src/
 │   ├─ Parser.cs            // 对外静态入口
 │   ├─ CodeGen/             // IL 生成实现
 │   │   ├─ TypeMapper.cs    // 字符串 → Type
 │   │   ├─ MethodEmitter.cs // IL 指令
 │   │   └─ AssemblyCache.cs // 已生成 Assembly 缓存
 │   └─ Models/
 │       └─ PluginInfo.cs    // 复用现有结构
 ├─ tests/
 │   └─ Parser.Tests/
 └─ README.md
```

---

## 🚀 快速开始

### 基本用法

```csharp
using Kscript.CSharp.Parser;

// 1. 从 JSON 文件生成程序集
var assembly = await Parser.GenerateFromFileAsync("example.json");

// 2. 或从插件信息列表生成
var plugins = new List<PluginInfo> { /* ... */ };
var assembly = Parser.Generate(plugins);

// 3. 脚本中直接调用生成的方法
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

## 📁 项目结构

```
KitX.CSharp.Parser/
├─ src/
│   ├─ Parser.cs                    # 主入口静态类
│   ├─ Models/
│   │   └─ PluginCallInfo.cs       # 插件调用信息模型
│   ├─ CodeGen/
│   │   ├─ TypeMapper.cs           # 字符串 → Type 映射
│   │   ├─ MethodEmitter.cs        # IL 方法生成器
│   │   └─ AssemblyCache.cs        # 程序集缓存管理
│   ├─ Core/
│   │   ├─ IPluginManager.cs       # 插件管理器接口
│   │   └─ MockPluginManager.cs    # 模拟实现
│   └─ Exceptions/
│       └─ ParserException.cs      # 自定义异常
├─ Examples/
│   ├─ BasicUsageExample.cs        # 基础用法示例
│   └─ Program.cs                  # 演示程序入口
├─ example.json                    # 插件清单示例
├─ README.md                       # 项目说明
└─ USAGE.md                        # 详细使用指南
```

---

## ⚙️ 主要特性

- ✅ **动态 IL 生成** - 使用 `Reflection.Emit` 生成高性能静态方法
- ✅ **智能类型映射** - 支持基础类型、泛型类型和自定义类型
- ✅ **程序集缓存** - 避免重复生成，提升性能
- ✅ **扩展性设计** - 支持自定义插件管理器和类型映射
- ✅ **异常处理** - 完善的错误处理机制
- ✅ **调试友好** - 详细的日志输出和统计信息

---

## 📐 类型映射支持

| JSON 字符串 | CLR 类型 | 示例 |
|---|---|---|
| `"int"` | `System.Int32` | 整数参数 |
| `"double"` | `System.Double` | 浮点数参数 |
| `"string"` | `System.String` | 字符串参数 |
| `"bool"` | `System.Boolean` | 布尔参数 |
| `"void"` | `System.Void` | 无返回值 |
| `"List<int>"` | `List<int>` | 泛型集合 |
| `"Dictionary<string,int>"` | `Dictionary<string,int>` | 字典类型 |
| 自定义 | 通过 `RegisterCustomType()` 注册 | 扩展类型 |

---

## 🎯 运行结果示例

```
🚀 欢迎使用 KitX.CSharp.Parser 演示程序!

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
   缓存统计: 1 个程序集, 1 个引用
```

---

## 📋 TODO / 未来计划

### 🔄 项目重构
- [ ] **转换为纯库项目** - 移除命令行功能，专注于提供API
- [ ] **移除演示程序** - 将 `Examples/` 目录移至独立的演示项目
- [ ] **调整项目配置** - 移除 `OutputType=Exe` 配置

### 🔌 插件管理器集成
- [ ] **替换 Mock 实现** - 当 KitX 主项目完成后，集成真实的 `PluginManager`
- [ ] **接口适配** - 根据实际需求调整 `IPluginManager` 接口定义
- [ ] **调用机制优化** - 优化插件调用的性能和稳定性

### 🛠️ 功能增强（暂无计划）
### 📈 性能优化（暂无计划）
### 🧪 测试覆盖（暂无计划）

> **注意**: 当前版本使用 `MockPluginManager` 进行功能验证。在 KitX 生态系统的其他组件完成后，需要将其替换为真实的插件调用实现。

