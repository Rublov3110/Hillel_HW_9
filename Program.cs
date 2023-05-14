using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace PRO_HW_9
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            try
            {
                var task1 = Task.Run(() => ReadFileAsync("file1.txt", cts.Token));
                var task2 = Task.Run(() => ReadFileAsync("file2.txt", cts.Token));
                var task3 = Task.Run(() => ReadFileAsync("file3.txt", cts.Token));
                var task4 = Task.Run(() => ReadFileAsync("file4.txt", cts.Token));
                var task5 = Task.Run(() => ReadFileAsync("file5.txt", cts.Token));

                await Task.WhenAny(task1, task2, task3, task4, task5);
                cts.Cancel();
                await Task.WhenAll(task1, task2, task3, task4, task5);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Зчитування файлу було відмінено.");
            }

        }

        static async Task ReadFileAsync(string filePath, CancellationToken token)
        {

            using var stream = new StreamReader(filePath);

            while (!stream.EndOfStream)
            {

                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                    
                }

                var line = await stream.ReadLineAsync();
                Console.WriteLine(line);
            }
        }
    }
}
