namespace template.data.Nodes
{
    public class BoolNode : PrimitiveNode<bool>
    {
        public BoolNode(bool value) : base(value) { }

        public bool Value { get { return value; } }
    }
}
