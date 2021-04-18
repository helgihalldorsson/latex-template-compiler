using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template.compiler
{
    public class LatexCompiler
    {
        public static void Compile(string workingDirectory, string rootFile, string auxDir = "auxiliary", string outputDir = "output")
        {
            if (!workingDirectory.EndsWith("\\"))
            {
                workingDirectory += "\\";
            }
            var fileInfo = new FileInfo(workingDirectory + rootFile);

            if (!fileInfo.Exists)
            {
                throw new Exception($"File {rootFile} could not be found.");
            }
            if (!rootFile.EndsWith(".tex"))
            {
                throw new Exception($"File {rootFile} must be a .tex file.");
            }
            string argument = $"pdflatex.exe -synctex=1 -interaction=nonstopmode -aux-directory=\"{auxDir}\" -output-directory=\"{outputDir}\" \"{rootFile}\"";
            string output = RunCmd(workingDirectory, argument);
            Console.WriteLine(output);
        }

        private static string RunCmd(string workingDirectory, string argument)
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
            string output = null;// cmd.StandardOutput.ReadToEnd();
            cmd.StandardInput.WriteLine(argument);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            cmd.Close();
            return output;
        }
    }
}
