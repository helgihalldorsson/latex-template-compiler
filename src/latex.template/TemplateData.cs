using latex.template.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace latex.template
{
    public class TemplateData
    {
        public static Root ReadFile(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<Root>(json, new NodeConverter());
        }
    }
}
