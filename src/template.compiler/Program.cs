using latex.compiler;
using System;
using System.IO;
using template.data;

namespace template.compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            string json = File.ReadAllText(@"D:\Code\latex-template-compiler\data\data.json");
            string template = File.ReadAllText(@"D:\Code\latex-template-compiler\data\template.tex");

            TemplateData data = TemplateData.Deserialize(json);
            CompileLatex(template, data, @"D:\Code\latex-template-compiler\data\", "is");
            CompileLatex(template, data, @"D:\Code\latex-template-compiler\data\", "en");
        }

        private static void CompileLatex(string template, TemplateData data, string path, string language)
        {
            string result = data.FillTemplate(template, language);
            File.WriteAllText(@$"{path}\latex_{language}.tex", result);
            LatexCompiler.Compile(path, $"latex_{language}.tex");
        }
    }
}
