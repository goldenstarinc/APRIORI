using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessor
{
    public class TimerUtility
    {
        public static TimeSpan MeasureTime(Action action)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Выполнение переданной функции
            action();

            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed;
            
            return elapsedTime;
        }
    }
}
