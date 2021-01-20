using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DeadLock
{
    class Program
    {
        static void FileCreationDeadlockAsync(string file1, string file2, int sleepTimeMs)
        {
            while (!File.Exists(file1))
            {
                Thread.Sleep(sleepTimeMs);
                Console.WriteLine($"file{file1} not exist! waiting creation");
            }

            using (var f = File.Create(file2))
            {
            } ;
        }
        static async Task Main(string[] args)
        {
            
            var t1= Task.Run(() =>{
                FileCreationDeadlockAsync("a.txt", "b.txt", 100);
            });
            var t2 = Task.Run(() => {
                FileCreationDeadlockAsync("a.txt", "b.txt", 100);
            });
            var ch = Console.ReadKey();
            await using (var f = File.Create("a.txt"))
            {
            };
           
            var ch2 = Console.ReadKey();
            await t1;
            await t2;




        }
    }
}
