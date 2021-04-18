using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using template.data;

namespace template.compiler
{
    public class TemplateCompiler
    {
        public static void Compile(string templateDir, string rootFile, string outputDir, string dataFilePath)
        {
            if(!File.Exists(dataFilePath))
            {
                throw new FileNotFoundException($"File {dataFilePath} not found.");
            }
            string templateData = File.ReadAllText(dataFilePath);
            TemplateData data = TemplateData.Deserialize(templateData);
            Compile(templateDir, rootFile, outputDir, data);
        }

        public static void Compile(string templateDir, string rootFile, string outputDir, TemplateData templateData)
        {
            DirectoryInfo templateDirInfo = GetDirectoryInfo(templateDir);
            DirectoryInfo outputDirInfo = GetDirectoryInfo(outputDir);

            List<string> supportedLanguages = templateData.settings.supportedLanguages;
            if (supportedLanguages != null && supportedLanguages.Count > 0)
            {
                foreach(string lang in supportedLanguages)
                {
                    DirectoryInfo languageDir = outputDirInfo.CreateSubdirectory(lang);
                    CompileSingleLanguage(templateDirInfo, rootFile, languageDir, templateData, lang);
                }
            }
            else
            {
                CompileSingleLanguage(templateDirInfo, rootFile, outputDirInfo, templateData);
            }
        }

        private static void CompileSingleLanguage(DirectoryInfo templateDirInfo, string rootFile, DirectoryInfo outputDirInfo, TemplateData templateData, string selectedLanguage = null)
        {
            DirectoryInfo outputTemplateDirInfo = outputDirInfo.CreateSubdirectory("template");
            PrepareTemplateDirectory(templateDirInfo, outputTemplateDirInfo, templateData, selectedLanguage);
            LatexCompiler.Compile(outputTemplateDirInfo.FullName, rootFile, auxDir: @"..\auxiliary", outputDir: @"..");
        }

        private static DirectoryInfo GetDirectoryInfo(string dir)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            if (!dirInfo.Exists)
            {
                throw new DirectoryNotFoundException($"Directory {dir} not found.");
            }
            return dirInfo;
        }

        private static void PrepareTemplateDirectory(DirectoryInfo templateDirInfo, DirectoryInfo outputDirInfo, TemplateData templateData, string selectedLanguage = null)
        {
            foreach(FileInfo templateFile in templateDirInfo.EnumerateFiles())
            {
                PrepareTemplateFile(templateFile, outputDirInfo, templateData, selectedLanguage);
            }

            // Recursively go through each subdirectory in the template, and recreate in the output location.
            foreach(DirectoryInfo subTemplateDirInfo in templateDirInfo.EnumerateDirectories())
            {
                DirectoryInfo subOutputDirInfo = outputDirInfo.CreateSubdirectory(subTemplateDirInfo.Name);
                PrepareTemplateDirectory(subTemplateDirInfo, subOutputDirInfo, templateData, selectedLanguage);
            }
        }

        private static void PrepareTemplateFile(FileInfo fileInfo, DirectoryInfo outputDirInfo, TemplateData templateData, string selectedLanguage = null)
        {
            string outputFilePath = @$"{outputDirInfo.FullName}\{fileInfo.Name}";
            if (fileInfo.Extension != ".tex")
            {                
                fileInfo.CopyTo(outputFilePath);
            }
            else
            {
                string template = File.ReadAllText(fileInfo.FullName);
                string templateWithData = templateData.FillTemplate(template, selectedLanguage);
                File.WriteAllText(outputFilePath, templateWithData);
            }
        }
    }
}
