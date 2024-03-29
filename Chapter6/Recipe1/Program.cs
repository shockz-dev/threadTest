﻿using System.Collections.Concurrent;
using System.Diagnostics;

namespace Recipe1;

internal class Program
{
  const string Item = "Dictionary item";
  public static string CurrentItem;

  private static void Main(string[] args)
  {
    var concurrentDictionary = new ConcurrentDictionary<int, string>();
    var dictionary = new Dictionary<int, string>();

    var sw = new Stopwatch();

    sw.Start();
    for (int i = 0; i < 1_000_000; i++)
    {
      lock (dictionary)
      {
        dictionary[i] = Item;
      }
    }
    sw.Stop();
    Console.WriteLine("Writing to dictionary with a lock: {0}", sw.Elapsed);

    sw.Restart();
    for (int i = 0; i < 1_000_000; i++)
    {
      concurrentDictionary[i] = Item;
    }
    sw.Stop();
    Console.WriteLine("Writing to a concurrent dictionary: {0}", sw.Elapsed);

    sw.Restart();
    for (int i = 0; i < 1_000_000; i++)
    {
      lock (dictionary)
      {
        CurrentItem = dictionary[i];
      }
    }
    sw.Stop();
    Console.WriteLine("Reading from dictionary with a lock: {0}", sw.Elapsed);

    sw.Restart();
    for (int i = 0; i < 1_000_000; i++)
    {
      CurrentItem = concurrentDictionary[i];
    }
    sw.Stop();
    Console.WriteLine("Reading from a concurrent dictionary: {0}", sw.Elapsed);
  }
}