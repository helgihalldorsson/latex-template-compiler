using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template.data
{
    public interface INode
    {
        public void InputData(List<string> latexTemplates, string dataPath, Settings settings);

        public bool IgnoreNode(Settings settings);
    }
}
