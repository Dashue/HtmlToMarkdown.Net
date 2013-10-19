using System.Text.RegularExpressions;

namespace HtmlParser
{
    public class HtmlAttribute
    {
        private string _value;
        private string _escaped;

        public string Name { get; set; }
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;

                _escaped = Regex.Replace(value, @"(^|[^\\])""/g", "$1\\\"");
            }
        }

        public string Escaped { get { return _escaped; } }
    }
}