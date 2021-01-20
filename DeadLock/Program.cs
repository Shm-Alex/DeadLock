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
                Console.WriteLine($"file {file1} not exist! waiting creation");
            }

            using (var f = File.Create(file2))
            {
            } ;
        }

        static void DeleteFileIfExist(string[]  fileNames)
        {
            foreach (string fileName in fileNames)
            {
                if (File.Exists(fileName)) File.Delete(fileName);
            }
            
        }

        static async Task Main(string[] args)
        {
            var aTxt = "a.txt";
            var bTxt = "b.txt";
            string[] ab = new string[] { aTxt, bTxt };
            DeleteFileIfExist(ab);
            if (File.Exists(bTxt))File.Delete(bTxt);
            var t1= Task.Run(() =>{
                FileCreationDeadlockAsync(aTxt, bTxt, 100);
            });
            var t2 = Task.Run(() => {
                FileCreationDeadlockAsync(bTxt, aTxt, 100);
            });
            // здесь потоки взаимо блокируют себя и немогут закончится
            // пока один из них не создаст файл а.тхт либо 
            var ch = Console.ReadKey();
            await using (var f = File.Create(aTxt))
            {
            };
           await t1;
            await t2;
            var ch2 = Console.ReadKey();
            DeleteFileIfExist(ab);



        }
    }
}
