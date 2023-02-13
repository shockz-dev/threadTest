using System.Diagnostics;

namespace ConcurrencyTest01;

internal class Program
{
  private static async Task Main(string[] args)
  {
    try
    {
      await new ch01_console().Execute();
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
    }
  }
}