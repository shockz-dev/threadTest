internal class Program
{
  private static void Main(string[] args)
  {
    Task t = AsynchronyWithTPL();
    t.Wait();

    t = AsynchronyWithAwait();
    t.Wait();
  }

  private static Task AsynchronyWithTPL()
  {
    Task<string> t = GetInfoAsync("Task 1");
    Task t2 = t.ContinueWith(task => Console.WriteLine(t.Result), TaskContinuationOptions.NotOnFaulted);
    Task t3 = t.ContinueWith(task => Console.WriteLine(t.Exception.InnerException), TaskContinuationOptions.OnlyOnFaulted);

    return Task.WhenAny(t2, t3);
  }

  private static async Task AsynchronyWithAwait()
  {
    try
    {
      string result = await GetInfoAsync("Task 2");
      Console.WriteLine(result);
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
    }
  }

  private static async Task<string> GetInfoAsync(string name)
  {
    await Task.Delay(TimeSpan.FromSeconds(2));
    throw new Exception("Boom!");

    return string.Format("Task {0} is running on a thread id {1}. Is thread pool thread: {2}", name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
  }
}