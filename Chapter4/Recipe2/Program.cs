namespace Recipe2;

internal class Program
{
  private static void Main(string[] args)
  {
    TaskMethod("Main Thread Task");

    Task<int> task = CreateTask("Task 1");
    task.Start();
    int result = task.Result;
    Console.WriteLine("Result is: {0}", result);

    task = CreateTask("Task 2");
    task.RunSynchronously();
    result = task.Result;
    Console.WriteLine("Result is: {0}", result);

    task = CreateTask("Task 3");
    task.Start();

    while (!task.IsCompleted)
    {
      Console.WriteLine(task.Status);
      Thread.Sleep(TimeSpan.FromSeconds(0.5));
    }

    Console.WriteLine(task.Status);
    result = task.Result;
    Console.WriteLine("Result is: {0}", result);
  }

  static Task<int> CreateTask(string name)
  {
    return new Task<int>(() => TaskMethod(name));
  }

  private static int TaskMethod(string name)
  {
    Console.WriteLine("Task {0} is running on a thread id {1}. Is thread pool thread: {2}", name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
    Thread.Sleep(TimeSpan.FromSeconds(2));
    return 42;
  }
}