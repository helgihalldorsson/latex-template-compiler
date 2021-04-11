using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template.data.Nodes
{
    public class IntNode : PrimitiveNode<int>
    {
        public IntNode(int value) : base(value) { }
    }
}
