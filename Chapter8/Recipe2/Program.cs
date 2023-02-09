using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Recipe2;

internal class Program
{
  private static void Main(string[] args)
  {
    var observer = new CustomObserver();
    var goodObservable = new CustomSequuence(new[] { 1, 2, 3, 4, 5 });
    var badObservable = new CustomSequuence(null);

    using (IDisposable subscription = goodObservable.Subscribe(observer))
    {
    }

    using (IDisposable subscription = goodObservable.SubscribeOn(TaskPoolScheduler.Default).Subscribe(observer))
    {
      Thread.Sleep(100);
    }

    using (IDisposable subscription = badObservable.SubscribeOn(TaskPoolScheduler.Default).Subscribe(observer))
    {
      Console.ReadLine();
    }
  }

  private class CustomObserver : IObserver<int>
  {
    public void OnCompleted()
    {
      Console.WriteLine("Completed");
    }

    public void OnError(Exception error)
    {
      Console.WriteLine("Error: {0}", error.Message);
    }

    public void OnNext(int value)
    {
      Console.WriteLine("Next value: {0}; Thread Id: {1}", value, Thread.CurrentThread.ManagedThreadId);
    }
  }

  private class CustomSequuence : IObservable<int>
  {
    private readonly IEnumerable<int> _numbers;

    public CustomSequuence(IEnumerable<int> numbers)
    {
      _numbers = numbers;
    }

    public IDisposable Subscribe(IObserver<int> observer)
    {
      foreach (var number in _numbers)
      {
        observer.OnNext(number);
      }
      observer.OnCompleted();

      return Disposable.Empty;
    }
  }
}