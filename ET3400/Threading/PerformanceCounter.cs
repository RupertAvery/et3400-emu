using System.Runtime.InteropServices;

namespace ET3400.Threading
{
    public static class PerformanceCounter
    {
        [DllImport("Kernel32.dll")]
        public static extern bool QueryPerformanceCounter(
            out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        public static extern bool QueryPerformanceFrequency(
            out long lpFrequency);

        public static double GetTime()
        {
            QueryPerformanceCounter(out var lpPerformanceCount);
            QueryPerformanceFrequency(out var lpFrequency);
            return (double)lpPerformanceCount / (double)lpFrequency;
        }
    }
}