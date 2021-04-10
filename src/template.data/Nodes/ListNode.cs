using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template.data.Nodes
{
    public class ListNode : INode
    {
        private List<INode> value;
        public ListNode(List<INode> value)
        {
            this.value = value;
        }

        public void InputData(List<string> latexTemplates, string dataPath)
        {
            string dataTagBegin = $"<:{dataPath}::>";

            int i = 0; 
            while(i < latexTemplates.Count)
            {
                if (latexTemplates[i].Contains(dataTagBegin))
                {
                    List<string> templateWithData = InputData(latexTemplates[i], dataPath);
                    if(templateWithData == null || templateWithData.Count == 0)
                    {
                        i++;
                    }
                    else
                    {
                        latexTemplates.RemoveAt(i);
                        latexTemplates.InsertRange(i, templateWithData);
                        i += templateWithData.Count;
                    }
                }
                else
                {
                    i++;
                }
            }
        }

        private List<string> InputData(string latexTemplate, string dataPath)
        {
            string dataTagBegin = $"<:{dataPath}::>";

            if (!latexTemplate.Contains(dataTagBegin))
            {
                return null;
            }

            string dataTagEnd = $"<::{dataPath}:>";


            var parts = SplitTemplateIntoParts(latexTemplate, dataTagBegin, dataTagEnd);
            if(parts == null)
            {
                return null;
            }

            string frontPart = parts.Item1;
            latexTemplate = parts.Item2;
            string backPart = parts.Item3;
            string divider = parts.Item4;

            List<string> latexTemplateParts = new List<string> { frontPart };
            foreach (INode node in value)
            {
                List<string> itemTemplateParts = new List<string> { latexTemplate };
                node.InputData(itemTemplateParts, dataPath);
                latexTemplateParts.AddRange(itemTemplateParts);
                latexTemplateParts.Add(divider);
            }
            latexTemplateParts.RemoveAt(latexTemplateParts.Count - 1);

            if(backPart.Contains(dataTagBegin))
            {
                List<string> backPartTemplateParts = InputData(backPart, dataPath);
                latexTemplateParts.AddRange(backPartTemplateParts);
            }
            else
            {
                latexTemplateParts.Add(backPart);
            }

            return latexTemplateParts;
        }

        private Tuple<string, string, string, string> SplitTemplateIntoParts(string latexTemplate, string dataTagBegin, string dataTagEnd)
        { 
            int begin = latexTemplate.IndexOf(dataTagBegin);
            int end = latexTemplate.IndexOf(dataTagEnd, begin);

            string frontPart = latexTemplate.Substring(0, begin);
            string backPart = latexTemplate.Substring(end + dataTagEnd.Length);
            latexTemplate = latexTemplate.Substring(begin + dataTagBegin.Length, end - (begin + dataTagBegin.Length));
            string divider = null;

            if(latexTemplate.StartsWith("<>"))
            {
                latexTemplate = latexTemplate.Substring(2);
                int dividerEnd = latexTemplate.IndexOf("<>");
                divider = latexTemplate.Substring(0, dividerEnd);
                latexTemplate = latexTemplate.Substring(divider.Length + 2);
            }

            return new Tuple<string, string, string, string>(frontPart, latexTemplate, backPart, divider);
        }

        private Tuple<string, string, string> SplitStringIntoParts(string template, string tagStart, string tagEnd)
        {
            int begin = template.IndexOf(tagStart);
            int end = template.IndexOf(tagEnd);

            string frontPart = template.Substring(0, begin);
            string backPart = template.Substring(end + tagEnd.Length);
            template = template.Substring(begin + tagStart.Length, end - (begin + tagStart.Length));

            return new Tuple<string, string, string>(frontPart, template, backPart);
        }
    }
}
