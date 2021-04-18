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
        #region Properties
        public TemplateData Data { get; set; }

        public bool CleanUpAuxiliary { get; set; } = false;
        public bool CleanUpTemplate { get; set; } = false;
        public bool CleanUpSyncTex { get; set; } = false;

        private readonly DirectoryInfo templateDir;
        private readonly string rootFile;

        
        private readonly string templateDirectory = "template";
        // Auxiliary and output moved up a level, to make all 3 directories appear in the same directory.
        private readonly string auxiliaryDirectory = @"..\auxiliary";
        private readonly string outputDirectory = @"..";
        #endregion

        #region Constructors
        public TemplateCompiler(string templateDir, string rootFile, string dataFilePath, bool cleanUpAuxiliary = false, bool cleanUpTemplate = false, bool cleanUpSyncTex = false)
        {
            this.templateDir = GetDirectoryInfo(templateDir);
            this.rootFile = rootFile;
            if (!File.Exists(dataFilePath))
            {
                throw new FileNotFoundException($"File {dataFilePath} not found.");
            }

            string templateData = File.ReadAllText(dataFilePath);            
            Data = TemplateData.Deserialize(templateData);

            CleanUpAuxiliary = cleanUpAuxiliary;
            CleanUpTemplate = cleanUpTemplate;
            CleanUpSyncTex = cleanUpSyncTex;
        }

        public TemplateCompiler(string templateDir, string rootFile, TemplateData templateData, bool cleanUpAuxiliary = false, bool cleanUpTemplate = false, bool cleanUpSyncTex = false)
        {
            this.templateDir = GetDirectoryInfo(templateDir);
            this.rootFile = rootFile;

            Data = templateData;

            CleanUpAuxiliary = cleanUpAuxiliary;
            CleanUpTemplate = cleanUpTemplate;
            CleanUpSyncTex = cleanUpSyncTex;
        }
        #endregion

        private static DirectoryInfo GetDirectoryInfo(string dir)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            if (!dirInfo.Exists)
            {
                throw new DirectoryNotFoundException($"Directory {dirInfo.FullName} not found.");
            }
            return dirInfo;
        }

        public void Compile(string outputDir)
        {
            DirectoryInfo outputDirInfo = new DirectoryInfo(outputDir);
            if(!outputDirInfo.Exists)
            {
                outputDirInfo.Create();
            }

            List<string> supportedLanguages = Data.settings.supportedLanguages;
            if (supportedLanguages != null && supportedLanguages.Count > 0)
            {
                foreach(string lang in supportedLanguages)
                {
                    DirectoryInfo languageDir = outputDirInfo.CreateSubdirectory(lang);
                    CompileSingleLanguage(languageDir, lang);
                }
            }
            else
            {
                CompileSingleLanguage(outputDirInfo);
            }
        }

        private void CompileSingleLanguage(DirectoryInfo outputDirInfo, string selectedLanguage = null)
        {
            DirectoryInfo outputTemplateDirInfo = outputDirInfo.CreateSubdirectory(templateDirectory);
            PrepareTemplateDirectory(templateDir, outputTemplateDirInfo, Data, selectedLanguage);
            LatexCompiler.Compile(outputTemplateDirInfo.FullName, rootFile, auxDir: auxiliaryDirectory, outputDir: outputDirectory);
            CleanUp(outputTemplateDirInfo);
        }

        private void CleanUp(DirectoryInfo outputTemplateDirInfo)
        {
            if(CleanUpAuxiliary)
            {
                DirectoryInfo auxDir = new DirectoryInfo(@$"{outputTemplateDirInfo.FullName}\{auxiliaryDirectory}");
                auxDir.Delete(true);
            }
            if(CleanUpSyncTex)
            {
                DirectoryInfo outputDir = new DirectoryInfo(@$"{outputTemplateDirInfo.FullName}\{outputDirectory}");
                string fileName = rootFile.Replace(".tex", ".synctex.gz");
                FileInfo synctexfile = new FileInfo(@$"{outputDir.FullName}\{fileName}");
                synctexfile.Delete();
            }
            if(CleanUpTemplate)
            {
                outputTemplateDirInfo.Delete(true);
            }
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
