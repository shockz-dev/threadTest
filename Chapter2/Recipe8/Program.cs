namespace Recipe8;

internal class Program
{
  private static void Main(string[] args)
  {
    new Thread(Read) { IsBackground = true }.Start();
    new Thread(Read) { IsBackground = true }.Start();
    new Thread(Read) { IsBackground = true }.Start();

    new Thread(() => Write("Thread 1")) { IsBackground = true }.Start();
    new Thread(() => Write("Thread 2")) { IsBackground = true }.Start();

    Thread.Sleep(TimeSpan.FromSeconds(30));
  }

  private static ReaderWriterLockSlim _rw = new ReaderWriterLockSlim();
  private static Dictionary<int, int> _items = new Dictionary<int, int>();

  private static void Read()
  {
    Console.WriteLine("Reading contents of a dictionary");

    while (true)
    {
      try
      {
        _rw.EnterReadLock();
        foreach (var item in _items.Keys)
        {
          Thread.Sleep(TimeSpan.FromSeconds(0.1));
        }
      }
      finally
      {
        _rw.ExitReadLock();
      }
    }
  }

  private static void Write(string threadName)
  {
    while (true)
    {
      try
      {
        int newKey = new Random().Next(250);
        _rw.EnterUpgradeableReadLock();

        if (!_items.ContainsKey(newKey))
        {
          try
          {
            _rw.EnterWriteLock();
            _items[newKey] = 1;
            Console.WriteLine("New key {0} is added to a dictionary by a {1}", newKey, threadName);
          }
          finally
          {
            _rw.ExitWriteLock();
          }
        }

        Thread.Sleep(TimeSpan.FromSeconds(0.1));
      }
      finally
      {
        _rw.ExitUpgradeableReadLock();
      }
    }
  }
}