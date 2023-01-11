void ShowThreadAndTask(string title)
{
  Console.WriteLine(title);
  string taskMessage = Task.CurrentId == null ? "no task" : $"in the task {Task.CurrentId}";
  Console.WriteLine($"running in thread {Environment.CurrentManagedThreadId} and {taskMessage}");
}

ShowThreadAndTask("Started application");
await SampleAsync();

ShowThreadAndTask("Top level statements after calling SampleAsync");

SynchronizationContext? context = SynchronizationContext.Current;
if (context == null)
{
  Console.WriteLine("No synchronization context with console applicatinos");
}

Task SampleAsync()
{
  return Task.Run(() =>
  {
    ShowThreadAndTask("Started AsyncMethod");
  });
}