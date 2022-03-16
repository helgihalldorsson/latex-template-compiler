using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template.data.Nodes
{
    public class DoubleNode : PrimitiveNode<double>
    {
        public DoubleNode(double value) : base(value) { }

        protected override string GetValueAsString()
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
