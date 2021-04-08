using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace latex.compiler
{
    class PdfCompiler
    {
        public static void CompileLaTeX(string workingDirectory, string filePath, string auxDir = "auxiliary", string outputDir = "output")
        {
            var fileInfo = new FileInfo(filePath);

            if (!fileInfo.Exists)
            {
                throw new Exception($"File {filePath} could not be found.");
            }
            if (!filePath.EndsWith(".tex"))
            {
                throw new Exception($"File {filePath} must be a .tex file.");
            }
            string argument = $"pdflatex.exe -synctex=1 -interaction=nonstopmode -aux-directory=\"{auxDir}\" -output-directory=\"{outputDir}\" \"{filePath}\"";
            RunCmd(workingDirectory, argument);
        }

        private static void RunCmd(string workingDirectory, string argument)
        {
            Process cmd = new Process();
            cmd.StartInfo = new ProcessStartInfo
            {
                WorkingDirectory = workingDirectory,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                UseShellExecute = false
            };
            cmd.Start();
            cmd.StandardInput.WriteLine(argument);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            cmd.Close();
        }
    }
}
