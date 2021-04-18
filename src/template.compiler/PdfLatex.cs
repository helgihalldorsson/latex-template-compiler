using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using template.compiler.Exceptions;

namespace template.compiler
{
    public class PdfLatex
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

            Cmd cmd = new Cmd();
            string output = cmd.Run(workingDirectory, argument);
            ParseOutput(output, fileInfo);
        }

        private static void ParseOutput(string output, FileInfo fileInfo)
        {
            if(output.Contains("no output PDF file produced!"))
            {
                if(output.Contains("Transcript written on "))
                {
                    var regexMessage = Regex.Match(output, @"(Transcript written on .*\.log\.)", RegexOptions.Singleline);
                    if(regexMessage.Success)
                    {
                        string transcriptLocation = regexMessage.Groups[0].Value;
                        transcriptLocation = transcriptLocation.Replace("\r\n", "").Replace("\n", "");
                        throw new LatexException($"Error when compiling {fileInfo.FullName}. {transcriptLocation}");
                    }
                }
            }
        }
    }
}
