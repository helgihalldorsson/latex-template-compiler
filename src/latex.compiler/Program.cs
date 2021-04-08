using latex.template;
using latex.template.Model;
using System;
using System.IO;

namespace latex.compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            Root data = TemplateData.ReadFile(@"D:\Code\latex-template-compiler\data\data.json");
            Console.WriteLine();
        }
    }
}
