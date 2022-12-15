using System.ComponentModel;

namespace Recipe5;

internal class Program
{
  private static void Main(string[] args)
  {
    var tcs = new TaskCompletionSource<int>();
    var worker = new BackgroundWorker();
    worker.DoWork += (sender, eventArgs) =>
    {
      eventArgs.Result = TaskMethod("Background worker", 5);
    };

    worker.RunWorkerCompleted += (sender, e) =>
    {
      if (e.Error != null)
      {
        tcs.SetException(e.Error);
      }
      else if (e.Cancelled)
      {
        tcs.SetCanceled();
      }
      else
      {
        tcs.SetResult((int)e.Result);
      }
    };

    worker.RunWorkerAsync();
    int result = tcs.Task.Result;
    Console.WriteLine("Result is: {0}", result);
  }

  private static int TaskMethod(string name, int seconds)
  {
    Console.WriteLine("Task {0} is running on a thread id {1}. Is thread pool thread: {2}", name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
    Thread.Sleep(TimeSpan.FromSeconds(seconds));
    return 42 * seconds;
  }
}