using System.Collections.Concurrent;

namespace Recipe3;

internal class Program
{
  private static void Main(string[] args)
  {
    Task t = RunProgram();
    t.Wait();
  }

  private static async Task RunProgram()
  {
    var taskStack = new ConcurrentStack<CustomTask>();
    var cts = new CancellationTokenSource();

    var taskSource = Task.Run(() => TaskProducer(taskStack));

    Task[] processors = new Task[4];
    for (int i = 1; i <= 4; i++)
    {
      string processorId = i.ToString();
      processors[i - 1] = Task.Run(() => TaskProcessor(taskStack, "Processor " + processorId, cts.Token));
    }

    await taskSource;
    cts.CancelAfter(TimeSpan.FromSeconds(2));

    await Task.WhenAll(processors);
  }

  private static async Task TaskProducer(ConcurrentStack<CustomTask> taskStack)
  {
    for (int i = 1; i <= 20; i++)
    {
      await Task.Delay(50);
      var workItem = new CustomTask { Id = i };
      taskStack.Push(workItem);
      Console.WriteLine("Task {0} has been posted", workItem.Id); ;
    }
  }

  private static async void TaskProcessor(ConcurrentStack<CustomTask> taskStack, string name, CancellationToken token)
  {
    await GetRandomDelay();

    do
    {
      CustomTask workItem;
      bool popSuccessful = taskStack.TryPop(out workItem);
      if (popSuccessful)
      {
        Console.WriteLine("Task {0} has been processed by {1}", workItem.Id, name);
      }

      await GetRandomDelay();
    } while (!token.IsCancellationRequested);
  }

  private static Task GetRandomDelay()
  {
    int delay = new Random(DateTime.Now.Millisecond).Next(1, 500);
    return Task.Delay(delay);
  }
}

internal class CustomTask
{
  public int Id { get; set; }
}