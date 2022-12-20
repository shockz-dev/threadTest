namespace Recipe2;

internal class Program
{
  private static void Main(string[] args)
  {
    Task t = AsynchronousProcessing();
    t.Wait();
  }

  static async Task AsynchronousProcessing()
  {
    Func<string, Task<string>> asyncLambda = async name =>
    {
      await Task.Delay(TimeSpan.FromSeconds(2));
      return string.Format("Task {0} is running on a thread id {1}. Is thread pool thread: {2}", name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
    };

    string result = await asyncLambda("async Lambda");

    Console.WriteLine(result);
  }
}