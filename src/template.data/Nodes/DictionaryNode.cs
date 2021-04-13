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
        public DictionaryNode(Dictionary<string, INode> value)
        {
            this.value = value;
        }

        public void InputData(List<string> latexTemplates, string dataPath, Settings settings)
        {
            int i = 0;
            while(i < latexTemplates.Count)
            {
                if (latexTemplates[i].Contains($"<:{dataPath}:"))
                {
                    List<string> templateWithData = InputData(latexTemplates[i], dataPath, settings);
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
        public bool IgnoreNode(Settings settings)
        {
            if (settings.ignoreEnabled)
            {
                if (value.ContainsKey("ignore") && value["ignore"].GetType() == typeof(BoolNode))
                {
                    BoolNode ignoreNode = (BoolNode)value["ignore"];
                    return ignoreNode.Value;
                }
            }
            return false;
        }


        private List<string> InputData(string latexTemplate, string dataPath, Settings settings)
        {
            var latexTemplateParts = new List<string> { latexTemplate };

            foreach(string key in value.Keys)
            {
                if(value[key].IgnoreNode(settings))
                {
                    continue;
                }

                string innerDataPath;
                if(string.IsNullOrWhiteSpace(dataPath))
                {
                    innerDataPath = key;
                }
                else
                {
                    innerDataPath = $"{dataPath}:{key}";
                }
                value[key].InputData(latexTemplateParts, innerDataPath, settings);
            }

            return latexTemplateParts;
        }
    }
}
