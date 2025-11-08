using Kscript.CSharp.Compiler.Examples;

namespace Kscript.CSharp.Compiler;

/// <summary>
/// Kscript.CSharp.Compiler 主程序入口
/// </summary>
public static class Program
{
    /// <summary>
    /// 主入口方法
    /// </summary>
    /// <param name="args">命令行参数</param>
    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("🚀 欢迎使用 KitX.CSharp.Compiler!");
        Console.WriteLine("==================================================\n");

        try
        {
            // 运行脚本执行示例
            await ScriptExecutionExample.RunExample();

            Console.WriteLine("\n" + new string('=', 60));

            // 运行端到端测试
            await EndToEndTest.RunTest();

            Console.WriteLine("\n==================================================");
            Console.WriteLine("✅ 所有示例运行完成!");
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ 程序运行失败: {ex.Message}");
            Console.WriteLine($"详细错误信息:\n{ex}");

            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
