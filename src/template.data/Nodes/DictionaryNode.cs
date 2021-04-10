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

        public void InputData(List<string> latexTemplates, string dataPath)
        {
            int i = 0;
            while(i < latexTemplates.Count)
            {
                if (latexTemplates[i].Contains($"<:{dataPath}:"))
                {
                    List<string> templateWithData = InputData(latexTemplates[i], dataPath);
                    latexTemplates.RemoveAt(i);
                    latexTemplates.InsertRange(i, templateWithData);
                    i += templateWithData.Count;
                }
                else
                {
                    i++;
                }
            }
        }

        private List<string> InputData(string latexTemplate, string dataPath)
        {
            var latexTemplateParts = new List<string> { latexTemplate };
            foreach(string key in value.Keys)
            {
                string innerDataPath;
                if(string.IsNullOrWhiteSpace(dataPath))
                {
                    innerDataPath = key;
                }
                else
                {
                    innerDataPath = $"{dataPath}:{key}";
                }
                value[key].InputData(latexTemplateParts, innerDataPath);
            }

            return latexTemplateParts;
        }
    }
}
