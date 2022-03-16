using System.Collections.Generic;

namespace template.data.Nodes
{
    public class PrimitiveNode<T> : INode
    {
        protected T value;

        public PrimitiveNode(T value)
        {
            this.value = value;
        }

        public bool IgnoreNode(Parameters parameters)
        {
            return false;
        }

        public void InputData(List<string> latexTemplates, string dataPath, Parameters parameters)
        {
            string dataTag = $"<:{dataPath}:>";
            for (int i = 0; i < latexTemplates.Count; i++)
            {
                latexTemplates[i] = InputData(latexTemplates[i], dataTag, parameters);
            }
        }

        private string InputData(string latexTemplate, string dataTag, Parameters parameters)
        {
            return latexTemplate.Replace(dataTag, GetValueAsString());
        }

        protected virtual string GetValueAsString()
        {
            return value.ToString();
        }
    }
}
