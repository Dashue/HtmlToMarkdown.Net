using System.Collections.Generic;

namespace HtmlToMarkdown.Net
{
    public class Markdown
    {
        public static Dictionary<string, string> Tags = new Dictionary<string, string>
            {
                {"hr", "- - -\n\n"},
                {"br", "  \n"},
                {"title", "# "},
                {"h1", "# "},
                {"h2", "## "},
                {"h3", "### "},
                {"h4", "#### "},
                {"h5", "##### "},
                {"h6", "###### "},
                {"b", "**"},
                {"strong", "**"},
                {"i", "_"},
                {"em", "_"},
                {"dfn", "_"},
                {"var", "_"},
                {"cite", "_"},
                {"span", " "},
                {"ul", "* "},
                {"ol", "1. "},
                {"dl", "- "},
                {"blockquote", "> "}
            };

        public static readonly List<string> ExcludedTags = new List<string>
            {
                "button", "frame", "head", "input", "iframe", "label", "link", "noframes", "noscript", "object", "option", "script", "select", "style", "textarea"
            };
    }
}