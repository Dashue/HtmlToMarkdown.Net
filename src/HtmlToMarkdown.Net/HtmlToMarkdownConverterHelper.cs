using HtmlParser;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HtmlConverters
{
    public static class HtmlToMarkdownConverterHelper
    {
        public static bool endsWith(string value, string suffix)
        {
            var match = Regex.Match(value, suffix + "$");

            if (match.Success)
            {
                return match.Groups[0].Value == suffix;
            }

            return false;
        }

        public static bool startsWith(string value, string str)
        {
            return value.IndexOf(str) == 0;
        }

        public static Dictionary<string, HtmlAttribute> convertAttrs(Dictionary<string, HtmlAttribute> attrs)
        {
            return attrs;

            //var attributes = {};
            //for (var k in attrs) {
            //    var attr = attrs[k];
            //    attributes[attr.name] = attr;
            //}
            //return attributes;
        }

        public static string peek(List<string> list)
        {
            throw new NotImplementedException();
            //if (list && list.length > 0) {
            //    return list.slice(-1)[0];
            //}
            //return "";
        }

        public static string peekTillNotEmpty(List<string> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i] != "")
                {
                    return list[i];
                }
            }
            return "";
        }
    }
}