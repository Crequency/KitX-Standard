using Kscript.CSharp.Parser.Examples;

namespace Kscript.CSharp.Parser.Examples;

/// <summary>
/// 演示程序入口
/// </summary>
public static class Program
{
    /// <summary>
    /// 主入口方法
    /// </summary>
    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("🚀 欢迎使用 KitX.CSharp.Parser 演示程序!");
        Console.WriteLine("==================================================\n");

        try
        {
            // 运行基础用法示例
            await BasicUsageExample.RunExample();

            // 运行高级功能示例
            BasicUsageExample.RunAdvancedExample();

            Console.WriteLine("\n==================================================");
            Console.WriteLine("✅ 所有示例运行完成!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ 程序运行失败: {ex.Message}");
            Console.WriteLine($"详细错误信息:\n{ex}");
        }

        Console.WriteLine("\n按任意键退出...");
        Console.ReadKey();
    }
}
