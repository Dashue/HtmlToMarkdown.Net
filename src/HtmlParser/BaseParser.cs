/*
* Ported from HTML Parser By John Resig (ejohn.org)
* Original code by Erik Arvidsson, Mozilla Public License
* http://erik.eae.net/simplehtmlparser/simplehtmlparser.js
*/

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HtmlParser
{
    public abstract class BaseParser
    {
        int _index;
        public readonly Stack<string> stack = new Stack<string>();
        private string last;
        private readonly List<string> _htmlBlocks = new List<string>();
        private bool isChars;
        private int index;

        private List<string> special = new List<string> { "script", "style" };

        protected void Parse(string html)
        {
            last = html;

            while (false == string.IsNullOrWhiteSpace(html))
            {
                isChars = true;

                // Make sure we're not in a script or style element
                if ((false == string.IsNullOrWhiteSpace(stack.SafePeek()) || false == special.Contains(stack.SafePeek())))
                {

                    // Comment
                    if (html.IndexOf("<!--") == 0)
                    {
                        index = html.IndexOf("-->");

                        if (index >= 0)
                        {
                            comment(html.Substring(4, index));
                            html = html.Substring(index + 3);
                            isChars = false;
                        }

                        // end tag
                    }
                    else if (html.IndexOf("</") == 0)
                    {
                        Match match = RegularExpressions.IsEndTag.Match(html);

                        if (match.Success)
                        {
                            var tagName = match.Groups[1].Value;

                            html = html.Substring(match.Groups[0].Value.Length);

                            //match.Groups[0].Value.Replace( endTag, parseEndTag );
                            parseEndTag(match.Groups[0].Value, tagName);
                            isChars = false;
                        }

                        // start tag
                    }
                    else if (html.IndexOf("<") == 0)
                    {
                        var match = RegularExpressions.StartTag.Match(html);

                        if (match.Success)
                        {
                            html = html.Substring(match.Groups[0].Length);

                            var tagName = match.Groups[1].Value;

                            //match[0].Replace(startTag, parseStartTag);
                            parseStartTag(match.Groups[0].Value, tagName, match.Groups[2].Value, match.Groups[3].Value);
                            isChars = false;
                        }
                    }

                    if (isChars)
                    {
                        index = html.IndexOf("<");

                        var text = index < 0 ? html : html.Substring(0, index);
                        html = index < 0 ? "" : html.Substring(index);

                        chars(text);
                    }

                }
                else
                {
                    throw new NotImplementedException();
                    //html = html.Replace(new RegExp("(.*)<\/" + stack.last() + "[^>]*>"), function(all, text){
                    //    text = text.Replace(/<!--(.*?)-->/g, "$1")
                    //        .Replace(/<!\[CDATA\[(.*?)]]>/g, "$1");

                    //    if ( chars )
                    //        chars( text );

                    //    return "";
                }

                //parseEndTag( "", stack.last() );
            }

            if (html == last)
                throw new Exception("Parse Error: " + html);
            last = html;

            parseEndTag("", "");
        }

        public void parseStartTag(string tag, string tagName, string rest, string unaryStr)
        {
            if (HtmlTags.Block.Contains(tagName))
            {
                while (false == string.IsNullOrWhiteSpace(stack.SafePeek()) && HtmlTags.Inline.Contains(stack.SafePeek()))
                {
                    parseEndTag("", stack.Peek());
                }
            }

            if (HtmlTags.SelfClosing.Contains(tagName) && stack.SafePeek() == tagName)
            {
                parseEndTag("", tagName);
            }

            bool parsedUnary;
            bool.TryParse(unaryStr, out parsedUnary);
            var unary = HtmlTags.Empty.Contains(tagName) || parsedUnary;

            if (!unary)
            {
                stack.Push(tagName);
            }

            var attrs = new Dictionary<string, HtmlAttribute>();

            var matches = RegularExpressions.Attribute.Matches(rest);

            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];

                if (match.Success)
                {
                    var attributeName = match.Groups[1].Value;
                    string value;

                    if (false == string.IsNullOrWhiteSpace(match.Groups[2].Value))
                    {
                        value = match.Groups[2].Value;
                    }
                    else if (false == string.IsNullOrWhiteSpace(match.Groups[3].Value))
                    {
                        value = match.Groups[3].Value;
                    }
                    else if (false == string.IsNullOrWhiteSpace(match.Groups[4].Value))
                    {
                        value = match.Groups[4].Value;
                    }
                    else if (HtmlTags.FillAttrs.Contains(tagName))
                    {
                        value = tagName;
                    }
                    else
                    {
                        value = string.Empty;
                    }

                    var htmlAttribute = new HtmlAttribute
                    {
                        Name = attributeName,
                        Value = value,
                    };

                    attrs.Add(attributeName, htmlAttribute);
                }
            }

            start(tagName, attrs, unary);
        }

        public void parseEndTag(string tag, string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                // If no tag name is provided, clean shop
                while (stack.Count > 0)
                {
                    end(stack.Pop());
                }
            }
            else
            {
                // Find the closest opened tag of the same type
                while (stack.Peek() != tagName)
                {
                    end(stack.Pop());
                }

                end(stack.Pop());
            }
        }

        public abstract void comment(string text);
        public abstract void chars(string text);
        public abstract void start(string tag, Dictionary<string, HtmlAttribute> attributes, bool unary);
        public abstract void end(string tag);

        // Special Elements (can contain anything)
        protected abstract List<string> ExcludedTags { get; }
    }
}