using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template.data.Nodes
{
    public class PrimitiveNode<T> : INode
    {
        protected T value;
        public PrimitiveNode(T value)
        {
            this.value = value;
        }

        public bool IgnoreNode(Settings settings)
        {
            return false;
        }

        public void InputData(List<string> latexTemplates, string dataPath, Settings settings)
        {
            string dataTag = $"<:{dataPath}:>";
            for (int i = 0; i < latexTemplates.Count; i++)
            {
                latexTemplates[i] = InputData(latexTemplates[i], dataTag, settings);
            }
        }

        private string InputData(string latexTemplate, string dataTag, Settings settings)
        {
            return latexTemplate.Replace(dataTag, value.ToString());
        }
    }
}
