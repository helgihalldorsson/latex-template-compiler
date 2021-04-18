﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace latex.compiler
{
    public class LatexCompiler
    {
        public static void Compile(string workingDirectory, string filePath, string auxDir = "auxiliary", string outputDir = "output")
        {
            if(!workingDirectory.EndsWith("\\"))
            {
                workingDirectory += "\\";
            }
            var fileInfo = new FileInfo(workingDirectory + filePath);

            if (!fileInfo.Exists)
            {
                throw new Exception($"File {filePath} could not be found.");
            }
            if (!filePath.EndsWith(".tex"))
            {
                throw new Exception($"File {filePath} must be a .tex file.");
            }
            string argument = $"pdflatex.exe -synctex=1 -interaction=nonstopmode -aux-directory=\"{auxDir}\" -output-directory=\"{outputDir}\" \"{filePath}\"";
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
            string output = cmd.StandardOutput.ReadToEnd();
            cmd.StandardInput.WriteLine(argument);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            cmd.Close();
            return output;
        }
    }
}