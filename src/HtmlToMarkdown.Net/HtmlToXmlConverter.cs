using HtmlParser;
using System.Collections.Generic;
using System.Text;

namespace HtmlToMarkdown.Net
{
    public class HtmlToXmlConverter : BaseParser, IHtmlConverter
    {
        private StringBuilder Results = new StringBuilder();

        private readonly List<string> _excludedTags = new List<string>
            {
                "script",
                "style"
            };

        public override void comment(string text)
        {
            Results.AppendFormat("<!--{0}-->", text);
        }

        public override void chars(string text)
        {
            Results.Append(text);
        }

        public override void start(string tag, Dictionary<string, HtmlAttribute> attributes, bool unary)
        {
            Results.AppendFormat("<{0}", tag);

            foreach (HtmlAttribute attribute in attributes.Values)
            {
                Results.AppendFormat(" {0}=\"{1}\"", attribute.Name, attribute.Escaped);
            }

            Results.Append(unary ? "/>" : ">");
        }

        public override void end(string tag)
        {
            Results.AppendFormat("</{0}>", tag);

        }

        protected override List<string> ExcludedTags
        {
            get { return _excludedTags; }
        }

        public string Convert(string html)
        {
            Parse(html);

            return Results.ToString();
        }
    }
}