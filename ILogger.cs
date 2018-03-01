using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemcachedTryout
{
   public interface ILogger
   {
      void Debug(string message, params string[] items);
   }

   public class Logger : ILogger
   {
      public void Debug(string message, params string[] items)
      {
         Console.WriteLine(message, items);
      }
   }
}
