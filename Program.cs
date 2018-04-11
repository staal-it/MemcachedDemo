using System;
using System.Threading.Tasks;

namespace MemcachedTryout
{
   internal class Program
   {
      static void Main(string[] args)
      {
         MainAsync(args).GetAwaiter().GetResult();
      }

      static async Task MainAsync(string[] args)
      {
         try
         {
            var mc = new MemcacedCache();

                while (true)
                {
                    Console.WriteLine("Storing values...");
                    for (var i = 0; i < 10; i += 2)
                       await mc.AddAsync("Hello" + i, "World" + i);

                    Console.WriteLine("Reading values...");
                    for (var i = 0; i < 10; i++)
                    {
                       var item = await mc.GetWithSetAsync("Hello" + i, () => Task.FromResult("World" + i));
                       Console.WriteLine(item);
                    }

                    Console.WriteLine("Press key to exit");
                    Console.ReadLine();
                }
            }
         catch (Exception e)
         {
            Console.WriteLine(e);
            Console.WriteLine("Press key to exit");
            Console.ReadLine();
         }
      }
   }
}