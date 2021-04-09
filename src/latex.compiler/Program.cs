using latex.template;
using latex.template.data;
using System;
using System.IO;

namespace latex.compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            string json = File.ReadAllText(@"D:\Code\latex-template-compiler\data\data.json");
            string template = File.ReadAllText(@"D:\Code\latex-template-compiler\data\template.txt");

            TemplateData data = TemplateData.Deserialize(json);
            string result = data.FillTemplate(template);
            Console.WriteLine("");
        }
    }
}
