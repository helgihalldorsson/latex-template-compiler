using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template.data.Nodes
{
    public class BoolNode: PrimitiveNode<bool>
    {
        public BoolNode(bool value) : base(value) { }
    }
}
