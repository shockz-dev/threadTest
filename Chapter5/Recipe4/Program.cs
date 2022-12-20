namespace Recipe4;

internal class Program
{
  private static void Main(string[] args)
  {
    Task t = AsynchronousProcessing();
    t.Wait();
  }

  static async Task AsynchronousProcessing()
  {
    Task<string> t1 = GetInfoAsync("Task 1", 3);
    Task<string> t2 = GetInfoAsync("Task 2", 5);

    string[] results = await Task.WhenAll(t1, t2);
    foreach (string result in results)
    {
      Console.WriteLine(result);
    }
  }

  private static async Task<string> GetInfoAsync(string name, int seconds)
  {
    await Task.Delay(TimeSpan.FromSeconds(seconds));
    //await Task.Run(() => Thread.Sleep(TimeSpan.FromSeconds(seconds)));

    return string.Format("Task {0} is running on thread id: {1}. Is thread pool thread: {2}", name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
  }
}