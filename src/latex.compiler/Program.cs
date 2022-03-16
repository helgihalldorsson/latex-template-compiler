using template.compiler;

namespace latex.compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            string templateDir = @"D:\Code\latex-template-compiler\example\template";
            string rootFile = @"template.tex";
            string outputDir = @"D:\Code\latex-template-compiler\example\output";
            string dataPath = @"D:\Code\latex-template-compiler\example\data.json";

            TemplateCompiler compiler = new TemplateCompiler(templateDir, rootFile, dataPath, cleanUpAuxiliary: true, cleanUpTemplate: true, cleanUpSyncTex: true);
            compiler.Compile(outputDir);
        }
    }
}