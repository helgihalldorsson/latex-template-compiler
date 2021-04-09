using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace latex.template.data
{
    public interface INode
    {
        public void InputData(List<string> latexTemplates, string dataPath);
    }
}
