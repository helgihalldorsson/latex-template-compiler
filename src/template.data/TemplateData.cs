using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace template.data
{
    public class TemplateData
    {
        public Settings settings { get; set; }

        public INode data { get; set; }

        public static TemplateData Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<TemplateData>(json, new NodeConverter());
        }

        public string FillTemplate(string template, string selectedLanguage = null)
        {
            if(settings == null)
            {
                settings = new Settings();
            }

            var parameters = new Parameters { 
                settings = settings, 
                selectedLanguage = selectedLanguage 
            };
            List<string> templateParts = new List<string> { template };
            data.InputData(templateParts, "", parameters);
            return string.Join("", templateParts);
        }
    }
}
