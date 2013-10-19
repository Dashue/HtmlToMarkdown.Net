using HtmlConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HtmlToMarkdown.Net
{
    public partial class HtmlToMarkdownConverter
    {
        private Dictionary<string, string> _replaceValues = new Dictionary<string, string>
            {
                { "&amp;", "&" },
                { "&lt;", "<" },
            };

        public override void chars(string text)
        {
            if (preStack.Count > 0)
            {
                text = ReplaceForPre(text);
            }
            else if (text.Trim() != "")
            {
                text = ReplaceMultipleWhitespaveWithOne(text);

                var prevText = HtmlToMarkdownConverterHelper.peekTillNotEmpty(nodeStack.ToList());

                if (prevText.EndsWith(" "))
                {
                    text = text.TrimStart();
                }
            }
            else
            {
                nodeStack.Push("");
                return;
            }

            //if(blockquoteStack.length > 0 && peekTillNotEmpty(nodeList).endsWith("\n")) {
            if (blockquoteStack.Count > 0)
            {
                var array = blockquoteStack.ToArray();
                Array.Reverse(array);

                nodeStack.Push(string.Join(string.Empty, array));
            }

            nodeStack.Push(ReplaceSpecialChars(text));
        }

        internal string ReplaceMultipleWhitespaveWithOne(string text)
        {
            return Regex.Replace(text, @"\s+", " ");
        }

        internal string ReplaceForPre(string text)
        {
            return Regex.Replace(text, "\n", "\n    ");
        }

        private string ReplaceSpecialChars(string text)
        {
            return _replaceValues.Aggregate(text, (current, pair) => current.Replace(pair.Key, pair.Value));
        }
    }
}
