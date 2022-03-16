using System.Collections.Generic;

namespace template.data
{
    public interface INode
    {
        public void InputData(List<string> latexTemplates, string dataPath, Parameters parameters);

        public bool IgnoreNode(Parameters parameters);
    }
}
