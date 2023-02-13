using ConcurrencyTestLib;

namespace ConcurrencyTest01
{
  internal class ch01_console
  {
    public async Task Execute()
    {
      await new ch01_01().DoSomethingAsync();
    }
  }
}