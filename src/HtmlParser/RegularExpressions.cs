using System.Text.RegularExpressions;

namespace HtmlParser
{
    public class RegularExpressions
    {
        public static Regex StartTag = new Regex("^<(\\w+)((?:\\s+\\w+(?:\\s*=\\s*(?:(?:\"[^\"]*\")|(?:'[^']*')|[^>\\s]+))?)*)\\s*(\\/?)>");
        public static Regex IsEndTag = new Regex("^<\\/(\\w+)[^>]*>");

        public static Regex Attribute = new Regex("(\\w+)(?:\\s*=\\s*(?:(?:\"((?:\\.|[^\"])*)\")|(?:'((?:\\.|[^'])*)')|([^>\\s]+)))?", RegexOptions.Multiline);
    }
}