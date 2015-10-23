//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

using System;
using System.Diagnostics;
using System.IO;

namespace screenium.Reports
{
    class WindowsSupport
    {
        internal static void OpenFileInExplorer(string filePath)
        {
            //Run as separate process, let Windows handle it by file extension:
            Process proc = new Process();
            proc.StartInfo.FileName = Path.Combine(
                Environment.GetEnvironmentVariable("windir"),
                "explorer.exe"
            );
            proc.StartInfo.Arguments = filePath;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = false;
            proc.Start();
        }
    }
}
