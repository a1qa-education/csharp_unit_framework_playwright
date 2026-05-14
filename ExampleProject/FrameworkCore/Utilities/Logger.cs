using NUnit.Framework;
using System;

namespace FrameworkCore.Utilities
{
    public static class Logger
    {
        public static void Info(string message)
        {
            TestContext.Progress.WriteLine($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}");
        }

        public static void Error(string message, Exception? ex = null)
        {
            TestContext.Progress.WriteLine($"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}");
            if (ex != null)
            {
                TestContext.Progress.WriteLine(ex.ToString());
            }
        }
    }
}