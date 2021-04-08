using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace latex.template.Model
{
    public class Node
    {
        private string value;
        private List<Node> list;
        private Dictionary<string, Node> dict;

        #region Constructors
        public Node(string value)
        {
            this.value = value;
        }

        public Node(List<Node> list)
        {
            this.list = list;
        }

        public Node(Dictionary<string, Node> dict)
        {
            this.dict = dict;
        }
        #endregion
    }

    public class NodeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Node);
        }
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.ReadFrom(reader);
            
            if(token.Type == JTokenType.String)
            {
                return new Node(token.ToString());
            }
            else if(token.Type == JTokenType.Object)
            {   
                Console.WriteLine("");
                return ReadFromDictionary(token, serializer);
            }
            else if(token.Type == JTokenType.Array)
            {
                Console.WriteLine("");
                return ReadFromList(token, serializer);
            }
            return null;
        }
        
        private Node ReadFromDictionary(JToken token, JsonSerializer serializer)
        {
            var dict = new Dictionary<string, Node>();
            foreach(JToken child in token.Children())
            {
                dict.Add(child.Path, child.First.ToObject<Node>(serializer));
            }
            return new Node(dict);
        }

        private Node ReadFromList(JToken token, JsonSerializer serializer)
        {
            var list = new List<Node>();
            foreach (JToken child in token.Children())
            {
                list.Add(child.ToObject<Node>(serializer));
            }
            return new Node(list);
        }


        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
