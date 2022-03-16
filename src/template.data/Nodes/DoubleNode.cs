using System.Globalization;

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
