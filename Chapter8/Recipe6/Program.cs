using System.Globalization;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Timers;

namespace Recipe6;

internal class Program
{
  private static void Main(string[] args)
  {
    IObservable<string> o = LongRunningOperationAsync("Task1");
    using (var sub = OutputToConsole(o))
    {
      Thread.Sleep(TimeSpan.FromSeconds(2));
    }
    Console.WriteLine("-------------------");

    Task<string> t = LongRunningOperationTaskAsync("Task2");
    using (var sub = OutputToConsole(t.ToObservable()))
    {
      Thread.Sleep(TimeSpan.FromSeconds(2));
    }
    Console.WriteLine("-------------------");

    var asyncMethod = LongRunningOperation;

    Func<string, IObservable<string>> observableFactory = Observable.FromAsyncPattern<string, string>(asyncMethod.BeginInvoke, asyncMethod.EndInvoke);
    o = observableFactory("Task3");
    using (var sub = OutputToConsole(o))
    {
      Thread.Sleep(TimeSpan.FromSeconds(2));
    }
    Console.WriteLine("-------------------");

    o = observableFactory("Task4");
    AwaitOnObservable(o).Wait();
    Console.WriteLine("-------------------");

    using (System.Timers.Timer timer = new System.Timers.Timer(1000))
    {
      var ot = Observable.FromEventPattern<ElapsedEventHandler, ElapsedEventArgs>(h => timer.Elapsed += h, h => timer.Elapsed -= h);
      timer.Start();

      using (var sub = OutputToConsole(ot))
      {
        Thread.Sleep(TimeSpan.FromSeconds(5));
      }
      Console.WriteLine("-------------------");

      timer.Stop();
    }
  }

  private static async Task<T> AwaitOnObservable<T>(IObservable<T> observable)
  {
    T obj = await observable;
    Console.WriteLine("{0}", obj);
    return obj;
  }

  private static Task<string> LongRunningOperationTaskAsync(string name)
  {
    return Task.Run(() => LongRunningOperation(name));
  }

  private static IObservable<string> LongRunningOperationAsync(string name)
  {
    return Observable.Start(() => LongRunningOperation(name));
  }

  private static string LongRunningOperation(string name)
  {
    Thread.Sleep(TimeSpan.FromSeconds(1));
    return string.Format("Task {0} is completed. Thread Id {1}", name, Thread.CurrentThread.ManagedThreadId);
  }

  private static IDisposable OutputToConsole(IObservable<EventPattern<ElapsedEventArgs>> sequence)
  {
    return sequence.Subscribe(obj => Console.WriteLine("{0}", obj.EventArgs.SignalTime), ex => Console.WriteLine("Error: {0}", ex.Message), () => Console.WriteLine("Completed"));
  }

  private static IDisposable OutputToConsole<T>(IObservable<T> sequence)
  {
    return sequence.Subscribe(obj => Console.WriteLine("{0}", obj), ex => Console.WriteLine("Error: {0}", ex.Message), () => Console.WriteLine("Completed"));
  }
}