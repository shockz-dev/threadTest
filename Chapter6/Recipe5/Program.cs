using System.Collections.Concurrent;

namespace Recipe5;

internal class Program
{
  private static void Main(string[] args)
  {
    Console.WriteLine("Using a queue inside of BlockingCollection");
    Console.WriteLine();
    Task t = RunProgram();
    t.Wait();

    Console.WriteLine();
    Console.WriteLine("Using a stack inside of BlokingCollection");
    Console.WriteLine();
    t = RunProgram(new ConcurrentStack<CustomTask>());
    t.Wait();
  }

  static async Task RunProgram(IProducerConsumerCollection<CustomTask> collection = null)
  {
    var taskCollection = new BlockingCollection<CustomTask>();
    if(null != collection)
    {
      taskCollection = new BlockingCollection<CustomTask>(collection);
    }

    var taskSource = Task.Run(() => TaskProducer(taskCollection));

    Task[] processors = new Task[4];
    for (int i = 1; i <= 4; i++)
    {
      string processorId = "Processor " + i.ToString();
      processors[i - 1] = Task.Run(() => TaskProcessor(taskCollection, processorId));
    }

    await taskSource;
    await Task.WhenAll(processors);
  }

  private static async Task TaskProducer(BlockingCollection<CustomTask> taskCollection)
  {
    for (int i = 1; i <= 20; i++)
    {
      await Task.Delay(20);
      var workItem = new CustomTask { Id = i };
      taskCollection.Add(workItem);
      Console.WriteLine("Task {0} have been posted", workItem.Id);
    }

    taskCollection.CompleteAdding();
  }

  private static async Task TaskProcessor(BlockingCollection<CustomTask> taskCollection, string processorId)
  {
    await GetRandomDelay();
    foreach (CustomTask item in taskCollection.GetConsumingEnumerable())
    {
      Console.WriteLine("Task {0} have been processed by {1}", item.Id, processorId);
      await GetRandomDelay();
    }
  }

  static Task GetRandomDelay()
  {
    int delay = new Random(DateTime.Now.Millisecond).Next(1, 500);
    return Task.Delay(delay);
  }
}