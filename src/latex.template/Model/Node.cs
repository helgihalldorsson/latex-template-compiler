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
            if (objectType == typeof(Node))
            {
                return true;
            }
            if (objectType == typeof(string))
            {
                return true;
            }
            if (objectType == typeof(Dictionary<string, Node>))
            {
                return true;
            }
            if (objectType == typeof(List<Node>))
            {
                return true;
            }
            return false;
        }
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if(objectType == typeof(string))
            {
                JToken token = JToken.ReadFrom(reader);
                return ReadAsString(token);
            }
            else if (objectType == typeof(Dictionary<string, Node>))
            {                
                JToken token = JToken.ReadFrom(reader);
                return ReadAsDictionary(token, serializer);
            }
            else if(objectType == typeof(List<Node>))
            {
                JToken token = JToken.ReadFrom(reader);
                return ReadAsList(token, serializer);
            }
            else if(objectType == typeof(Node))
            {
                JToken token = JToken.ReadFrom(reader);
                return ReadAsNode(token, serializer);

            }

            throw new JsonSerializationException("Unexpected JSON format encountered in NodeConverter");            
        }
        private Node ReadAsNode(JToken token, JsonSerializer serializer)
        {
            if(token.Type == JTokenType.String)
            {
                return new Node(token.ToString());
            }
            else
            {
                Console.WriteLine("");
            }
            return new Node("");
        }
        private Node ReadAsString(JToken token)
        {
            Console.WriteLine("");
            return new Node("");
        }

        private Node ReadAsDictionary(JToken token, JsonSerializer serializer)
        {
            var dict = new Dictionary<string, Node>();
            foreach(JToken child in token.Children())
            {
                string path = child.Path;
                Node node = child.First.ToObject<Node>(serializer);
                dict.Add(path, node);
                Console.WriteLine();
            }
            return new Node(dict);
        }

        private Node ReadAsList(JToken token, JsonSerializer serializer)
        {
            var list = new List<Node>();
            foreach (JToken child in token.Children())
            {
                Console.WriteLine();
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
