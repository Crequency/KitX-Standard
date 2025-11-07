# KitX.CSharp.Parser 类图与调用关系

## 📊 整体架构类图

```mermaid
classDiagram
    %% 外部依赖
    class KitXDashboard {
        +PluginsServer.Instance
        +WorkflowScriptService
    }

    class KitXShared {
        <<external>>
        +PluginInfo
        +Function
        +Parameter
        +Connector
        +Request
    }

    %% 主入口类
    class Parser {
        <<static>>
        -_defaultPluginManager: IPluginManager
        -_pluginManagerFactory: Func~IPluginManager~
        +Generate(plugins, assemblyName, pluginManager, useCache): Assembly
        +GenerateFromJson(jsonString, assemblyName, pluginManager, useCache): Assembly
        +GenerateFromFileAsync(jsonFilePath, assemblyName, pluginManager, useCache): Task~Assembly~
        +Regenerate(plugins, assemblyName, pluginManager): Assembly
        +SetDefaultPluginManager(pluginManager): void
        +SetPluginManagerFactory(factory): void
        +RegisterCustomType(typeName, type): void
        +ClearCache(): void
        +GetCacheStatistics(): CacheStatistics
        +HasCache(plugins, assemblyName): bool
        +GetPluginTypes(assembly): List~Type~
        +GetPluginMethods(pluginType): List~MethodInfo~
    }

    %% 核心接口
    class IPluginManager {
        <<interface>>
        +Call~T~(callInfo): T
        +Call(callInfo): void
        +IsPluginExists(pluginName): bool
        +IsMethodExists(pluginName, methodName): bool
    }

    %% 插件管理器实现
    class MockPluginManager {
        -_mockData: Dictionary~string, Dictionary~
        +Call~T~(callInfo): T
        +Call(callInfo): void
        +IsPluginExists(pluginName): bool
        +IsMethodExists(pluginName, methodName): bool
        -InvokeMockMethod(callInfo, method): object
        -GetDefaultValue(type): object
    }

    class RealPluginManager {
        -_pluginsServer: object
        -_infoLogger: Action~string~
        -_errorLogger: Action~string~
        -_pendingRequests: ConcurrentDictionary~string, TaskCompletionSource~
        +Call~T~(callInfo): T
        +Call(callInfo): void
        +IsPluginExists(pluginName): bool
        +IsMethodExists(pluginName, methodName): bool
        -FindPluginInfo(pluginName): PluginInfo
        -FindPluginConnector(pluginInfo): object
        -SendPluginRequest~T~(connector, callInfo): Task~T~
        -SendRequestToConnector(connector, request): Task
        +HandlePluginResponse(requestId, responseJson): void
        -GetDefaultResult~T~(): T
    }

    %% 代码生成组件
    class MethodEmitter {
        <<static>>
        -_staticPluginManager: IPluginManager
        +SetStaticPluginManager(pluginManager): void
        +GenerateAssembly(plugins, assemblyName, pluginManager): Assembly
        -GeneratePluginClass(moduleBuilder, plugin, pluginManager): void
        -GenerateStaticConstructor(typeBuilder, pluginManagerField, pluginManager): void
        -GeneratePluginMethod(typeBuilder, pluginName, function, pluginManagerField): void
        -GenerateMethodBody(methodBuilder, pluginName, function, parameterTypes, returnType, pluginManagerField): void
        -ConvertDefaultValue(value, targetType): object
    }

    class TypeMapper {
        <<static>>
        -_basicTypeMap: Dictionary~string, Type~
        -_customTypeMap: ConcurrentDictionary~string, Type~
        -_typeCache: ConcurrentDictionary~string, Type~
        -_genericTypeRegex: Regex
        +MapType(typeName): Type
        -ResolveGenericType(genericMatch): Type
        -ParseGenericArguments(argsString): Type[]
        -SplitGenericArguments(argsString): string[]
        +RegisterCustomType(typeName, type): void
        +ClearCache(): void
        +GetCustomTypes(): IReadOnlyDictionary~string, Type~
    }

    class AssemblyCache {
        <<static>>
        -_assemblyCache: ConcurrentDictionary~string, Assembly~
        -_referenceCount: ConcurrentDictionary~string, int~
        -_cacheLock: object
        +GetOrCreateAssembly(plugins, assemblyName, pluginManager): Assembly
        -GenerateCacheKey(plugins, assemblyName): string
        -IncrementReference(cacheKey): void
        +DecrementReference(cacheKey): void
        +ClearCache(): void
        +GetStatistics(): CacheStatistics
        +HasCache(plugins, assemblyName): bool
        +ForceRegenerate(plugins, assemblyName, pluginManager): Assembly
    }

    %% 数据模型
    class PluginCallInfo {
        +PluginName: string
        +MethodName: string
        +Parameters: object[]
        +ParameterTypes: Type[]
        +ToString(): string
    }

    class CacheStatistics {
        +CachedAssemblyCount: int
        +TotalReferences: int
        +CacheKeys: List~string~
        +ToString(): string
    }

    %% 异常类
    class ParserException {
        +ParserException()
        +ParserException(message)
        +ParserException(message, innerException)
        +TypeMappingError(typeName, innerException): ParserException
        +ILGenerationError(methodName, innerException): ParserException
        +AssemblyGenerationError(assemblyName, innerException): ParserException
    }

    %% 示例类
    class BasicUsageExample {
        <<static>>
        +RunExample(): Task
        -RunWithBuiltInData(): void
        -DynamicInvokeExample(assembly): void
        -GenerateTestArguments(methodName, parameters): object[]
        -GetDefaultValue(type): object
        +RunAdvancedExample(): void
    }

    class RealPluginManagerExample {
        <<static>>
        +RunExample(): void
        +DashboardIntegrationExample(): void
    }

    %% 关系定义
    Parser --> IPluginManager : 使用
    Parser --> TypeMapper : 使用
    Parser --> AssemblyCache : 使用
    Parser --> MethodEmitter : 调用
    Parser --> CacheStatistics : 返回

    IPluginManager <|.. MockPluginManager : 实现
    IPluginManager <|.. RealPluginManager : 实现

    MockPluginManager --> PluginCallInfo : 使用
    RealPluginManager --> PluginCallInfo : 使用
    RealPluginManager --> KitXDashboard : 依赖
    RealPluginManager --> KitXShared : 使用

    MethodEmitter --> IPluginManager : 使用
    MethodEmitter --> TypeMapper : 使用
    MethodEmitter --> PluginCallInfo : 使用
    MethodEmitter --> ParserException : 抛出

    TypeMapper --> ParserException : 抛出

    AssemblyCache --> MethodEmitter : 调用
    AssemblyCache --> CacheStatistics : 返回

    BasicUsageExample --> Parser : 使用
    RealPluginManagerExample --> Parser : 使用
    RealPluginManagerExample --> RealPluginManager : 使用

    KitXDashboard --> RealPluginManager : 创建
```

## 🔄 调用时序图

### 基本生成流程

```mermaid
sequenceDiagram
    participant Client
    participant Parser
    participant AssemblyCache
    participant MethodEmitter
    participant TypeMapper
    participant IPluginManager

    Client->>Parser: Generate(plugins)
    Parser->>AssemblyCache: GetOrCreateAssembly()

    alt 缓存命中
        AssemblyCache-->>Parser: 返回缓存的Assembly
    else 缓存未命中
        AssemblyCache->>MethodEmitter: GenerateAssembly()

        loop 对每个插件
            MethodEmitter->>TypeMapper: MapType(typeName)
            TypeMapper-->>MethodEmitter: 返回Type

            MethodEmitter->>MethodEmitter: GeneratePluginClass()
            MethodEmitter->>MethodEmitter: GenerateStaticConstructor()

            loop 对每个功能
                MethodEmitter->>MethodEmitter: GeneratePluginMethod()
                MethodEmitter->>MethodEmitter: GenerateMethodBody()
                MethodEmitter->>IPluginManager: SetStaticPluginManager()
            end
        end

        MethodEmitter-->>AssemblyCache: 返回新Assembly
        AssemblyCache-->>Parser: 返回Assembly
    end

    Parser-->>Client: 返回Assembly
```

### 真实插件调用流程

```mermaid
sequenceDiagram
    participant Script
    participant GeneratedMethod
    participant RealPluginManager
    participant PluginConnector
    participant PluginProcess
    participant KitXDashboard

    Script->>GeneratedMethod: SampleCalculator.Add(10, 20)
    GeneratedMethod->>RealPluginManager: Call(PluginCallInfo)

    RealPluginManager->>KitXDashboard: FindPluginInfo()
    KitXDashboard-->>RealPluginManager: PluginInfo

    RealPluginManager->>KitXDashboard: FindPluginConnector()
    KitXDashboard-->>RealPluginManager: PluginConnector

    RealPluginManager->>PluginConnector: SendRequest(Request)
    PluginConnector->>PluginProcess: WebSocket 通信
    PluginProcess-->>PluginConnector: Response
    PluginConnector-->>RealPluginManager: HandleResponse()

    RealPluginManager-->>GeneratedMethod: 返回结果
    GeneratedMethod-->>Script: 返回结果
```

## 📋 类职责说明

### 核心类

| 类名 | 职责 | 关键方法 |
|------|------|----------|
| **Parser** | 主入口，统一API | `Generate()`, `GenerateFromJson()`, `GenerateFromFileAsync()` |
| **IPluginManager** | 插件管理器接口 | `Call<T>()`, `IsPluginExists()` |
| **MockPluginManager** | 模拟插件管理器 | `Call<T>()`, `InvokeMockMethod()` |
| **RealPluginManager** | 真实插件管理器 | `Call<T>()`, `SendPluginRequest<T>()` |

### 代码生成类

| 类名 | 职责 | 关键方法 |
|------|------|----------|
| **MethodEmitter** | IL代码生成 | `GenerateAssembly()`, `GeneratePluginClass()` |
| **TypeMapper** | 类型映射 | `MapType()`, `RegisterCustomType()` |
| **AssemblyCache** | 程序集缓存 | `GetOrCreateAssembly()`, `ForceRegenerate()` |

### 数据模型类

| 类名 | 职责 | 关键属性 |
|------|------|----------|
| **PluginCallInfo** | 插件调用信息 | `PluginName`, `MethodName`, `Parameters` |
| **CacheStatistics** | 缓存统计信息 | `CachedAssemblyCount`, `TotalReferences` |

## 🔧 设计模式应用

### 1. 工厂模式
- `Parser.SetPluginManagerFactory()` - 创建插件管理器实例
- `MethodEmitter.GenerateAssembly()` - 创建动态程序集

### 2. 策略模式
- `IPluginManager` 接口 - 不同的插件管理器实现策略
- `MockPluginManager` vs `RealPluginManager` - 测试vs生产策略

### 3. 单例模式
- `AssemblyCache` - 静态缓存管理
- `TypeMapper` - 静态类型映射

### 4. 建造者模式
- `MethodEmitter` 中的 IL 生成过程
- `Connector` 的请求构建过程

### 5. 模板方法模式
- `Parser.Generate()` 方法的通用流程
- 插件方法生成的标准流程

## 🎯 关键调用路径

### 1. 程序集生成路径
```
Parser.Generate()
→ AssemblyCache.GetOrCreateAssembly()
→ MethodEmitter.GenerateAssembly()
→ TypeMapper.MapType()
→ IL生成
```

### 2. 插件调用路径
```
生成的静态方法
→ IPluginManager.Call()
→ RealPluginManager.SendPluginRequest()
→ WebSocket通信
→ 插件进程
```

### 3. 缓存管理路径
```
AssemblyCache.GenerateCacheKey()
→ SHA256哈希计算
→ 缓存查找/存储
→ 引用计数管理
```

这个类图展示了 KitX.CSharp.Parser 项目的完整架构，包括各个类之间的依赖关系、调用顺序和设计模式的应用。
