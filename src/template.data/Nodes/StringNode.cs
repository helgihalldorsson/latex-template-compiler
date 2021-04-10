using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template.data.Nodes
{
    public class StringNode : INode
    {
        private string value;
        public StringNode(string value)
        {
            this.value = value;
        }

        public void InputData(List<string> latexTemplates, string dataPath)
        {
            string dataTag = $"<:{dataPath}:>";
            for(int i = 0; i < latexTemplates.Count; i++)
            {
                latexTemplates[i] = InputData(latexTemplates[i], dataTag);
            }
        }

        private string InputData(string latexTemplate, string dataTag) {
            return latexTemplate.Replace(dataTag, value);
        }
    }
}
