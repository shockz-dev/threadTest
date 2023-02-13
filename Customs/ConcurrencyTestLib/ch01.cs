using System.Diagnostics;

namespace ConcurrencyTestLib;

public class ch01_01
{
  public async Task DoSomethingAsync()
  {
    int value = 13;

    await Task.Delay(TimeSpan.FromSeconds(1));

    value *= 2;

    await Task.Delay(TimeSpan.FromSeconds(1));

    Trace.WriteLine(value);
  }
}