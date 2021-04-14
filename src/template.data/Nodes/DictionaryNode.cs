using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template.data.Nodes
{
    public class DictionaryNode : INode
    {
        private Dictionary<string, INode> value;
        private bool isMultiLanguageNode = false;
        public DictionaryNode(Dictionary<string, INode> value)
        {
            this.value = value;
        }

        public void InputData(List<string> latexTemplates, string dataPath, Parameters parameters)
        {
            isMultiLanguageNode = CheckIfMultiLanguageNode(parameters);
            int i = 0;
            while(i < latexTemplates.Count)
            {
                if (latexTemplates[i].Contains($"<:{dataPath}:"))
                {
                    List<string> templateWithData = InputData(latexTemplates[i], dataPath, parameters);
                    if(templateWithData != null && templateWithData.Count > 0)
                    {
                        latexTemplates.RemoveAt(i);
                        latexTemplates.InsertRange(i, templateWithData);
                        i += templateWithData.Count;
                    }
                    else
                    {
                        i++;
                    }
                }
                else
                {
                    i++;
                }
            }
        }

        private bool CheckIfMultiLanguageNode(Parameters parameters)
        {
            // For a node to be a MultiLanguageNode, it must contain all supported languages as keys, and nothing else.

            List<string> supportedLanguages = parameters.settings.supportedLanguages;
            if (supportedLanguages == null || supportedLanguages.Count == 0)
            {
                return false;
            }

            if(!value.Keys.All(x => supportedLanguages.Contains(x)))
            {
                return false;
            }

            if(!supportedLanguages.All(x => value.Keys.Contains(x)))
            {
                return false;
            }

            return true;
        }

        public bool IgnoreNode(Parameters parameters)
        {
            if (parameters.settings.ignoreEnabled)
            {
                if (value.ContainsKey("ignore") && value["ignore"].GetType() == typeof(BoolNode))
                {
                    BoolNode ignoreNode = (BoolNode)value["ignore"];
                    return ignoreNode.Value;
                }
            }
            return false;
        }


        private List<string> InputData(string latexTemplate, string dataPath, Parameters parameters)
        {
            if(isMultiLanguageNode)
            {
                return InputDataFromSelectedLanguage(latexTemplate, dataPath, parameters);
            }
            else
            {
                return InputDataFromAllNodes(latexTemplate, dataPath, parameters);
            }
        }

        private List<string> InputDataFromAllNodes(string latexTemplate, string dataPath, Parameters parameters)
        {
            var latexTemplateParts = new List<string> { latexTemplate };

            foreach (string key in value.Keys)
            {
                if (value[key].IgnoreNode(parameters))
                {
                    continue;
                }

                string innerDataPath;
                if (string.IsNullOrWhiteSpace(dataPath))
                {
                    innerDataPath = key;
                }
                else
                {
                    innerDataPath = $"{dataPath}:{key}";
                }
                value[key].InputData(latexTemplateParts, innerDataPath, parameters);
            }

            return latexTemplateParts;
        }

        private List<string> InputDataFromSelectedLanguage(string latexTemplate, string dataPath, Parameters parameters)
        {
            INode node = value[parameters.selectedLanguage];
            var latexTemplateParts = new List<string> { latexTemplate };

            if (node.IgnoreNode(parameters))
            {
                return latexTemplateParts;
            }

            node.InputData(latexTemplateParts, dataPath, parameters);

            return latexTemplateParts;
        }
    }
}
