﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template.data.Nodes
{
    public class StringNode : PrimitiveNode<string>
    {
        public StringNode(string value) : base(value) { }
    }
}
