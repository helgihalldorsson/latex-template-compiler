using latex.template.data.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace latex.template.data
{
    public class NodeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(INode);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.ReadFrom(reader);

            if (token.Type == JTokenType.String)
            {
                return new StringNode(token.ToString());
            }
            else if (token.Type == JTokenType.Object)
            {
                return ReadFromDictionary(token, serializer);
            }
            else if (token.Type == JTokenType.Array)
            {
                return ReadFromList(token, serializer);
            }
            return null;
        }

        private DictionaryNode ReadFromDictionary(JToken token, JsonSerializer serializer)
        {
            var dict = new Dictionary<string, INode>();
            foreach (JToken child in token.Children())
            {
                dict.Add(child.Path, child.First.ToObject<INode>(serializer));
            }
            return new DictionaryNode(dict);
        }

        private ListNode ReadFromList(JToken token, JsonSerializer serializer)
        {
            var list = new List<INode>();
            foreach (JToken child in token.Children())
            {
                list.Add(child.ToObject<INode>(serializer));
            }
            return new ListNode(list);
        }


        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
