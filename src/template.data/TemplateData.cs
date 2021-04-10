using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace template.data
{
    public class TemplateData
    {
        public INode data { get; set; }

        public static TemplateData Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<TemplateData>(json, new NodeConverter());
        }

        public string FillTemplate(string template)
        {
            List<string> templateParts = new List<string> { template };
            data.InputData(templateParts, "");
            return string.Join("", templateParts);
        }
    }
}
