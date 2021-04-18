using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using template.compiler;
using template.data;

namespace latex.compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            string templateDir = @"D:\Code\latex-template-compiler\data\template";
            string rootFile = @"template.tex";
            string outputDir = @"D:\Code\latex-template-compiler\data\output";
            string dataPath = @"D:\Code\latex-template-compiler\data\data.json";

            TemplateCompiler compiler = new TemplateCompiler(templateDir, rootFile, dataPath);
            compiler.CleanUpAuxiliary = true;
            compiler.CleanUpTemplate = true;
            compiler.CleanUpSyncTex = true;
            compiler.Compile(outputDir);
        }
    }
}