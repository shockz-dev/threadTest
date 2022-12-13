namespace Recipe2;

internal class Program
{
  private static void Main(string[] args)
  {
    const string MutexName = "CSharpThreadingCookbook";

    using (var m = new Mutex(false, MutexName))
    {
      if(!m.WaitOne(TimeSpan.FromSeconds(5), false))
      {
        Console.WriteLine("Second instance is running!");
      }
      else
      {
        Console.WriteLine("Running!");
        Console.ReadLine();
        m.ReleaseMutex();
      }
    }
  }
}