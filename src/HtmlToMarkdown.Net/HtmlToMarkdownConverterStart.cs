using HtmlConverters;
using HtmlParser;
using System.Collections.Generic;
using System.Linq;

namespace HtmlToMarkdown.Net
{
    public partial class HtmlToMarkdownConverter
    {
        public override void start(string tag, Dictionary<string, HtmlAttribute> attrs, bool unary)
        {
            tag = tag.ToLower();

            if (unary && (tag != "br" && tag != "hr" && tag != "img"))
            {
                return;
            }
            switch (tag)
            {
                case "p":
                case "div":
                case "table":
                case "tbody":
                case "tr":
                case "td":
                    block(false);
                    break;
            }

            switch (tag)
            {
                case "br":
                    nodeStack.Push(Markdown.Tags[tag]);
                    break;
                case "hr":
                    block(false);
                    nodeStack.Push(Markdown.Tags[tag]);
                    break;
                case "title":
                case "h1":
                case "h2":
                case "h3":
                case "h4":
                case "h5":
                case "h6":
                    block(false);
                    nodeStack.Push(Markdown.Tags[tag]);
                    break;
                case "b":
                case "strong":
                case "i":
                case "em":
                case "dfn":
                case "var":
                case "cite":
                    nodeStack.Push(Markdown.Tags[tag]);
                    break;
                case "code":
                case "span":
                    if (preStack.Count > 0)
                    {
                        break;
                    }
                    else if (false == nodeStack.SafePeek().EndsWith(" "))
                    {
                        nodeStack.Push(Markdown.Tags[tag]);
                    }
                    break;
                case "ul":
                case "ol":
                case "dl":
                    listTagStack.Push(Markdown.Tags[tag]);
                    // lists are block elements
                    if (listTagStack.Count > 1)
                    {
                        listBlock();
                    }
                    else
                    {
                        block(false);
                    }
                    break;
                case "li":
                case "dt":
                    var li = getListMarkdownTag();
                    nodeStack.Push(li);
                    break;
                case "a":
                    linkAttrStack.Push(HtmlToMarkdownConverterHelper.convertAttrs(attrs));
                    nodeStack.Push("[");
                    break;
                case "img":
                    var attribs = HtmlToMarkdownConverterHelper.convertAttrs(attrs);
                    string alt, title, url;

                    url = attribs.ContainsKey("src") ? attribs["src"].Value : "";
                    if (string.IsNullOrWhiteSpace(url))
                    {
                        break;
                    }

                    alt = attribs.ContainsKey("alt") ? attribs["alt"].Value.Trim() : "";
                    title = attribs.ContainsKey("title") ? attribs["title"].Value.Trim() : "";

                    // if parent of image tag is nested in anchor tag use inline style
                    if (!inlineStyle && false == HtmlToMarkdownConverterHelper.startsWith(HtmlToMarkdownConverterHelper.peekTillNotEmpty(nodeStack.ToList()), "["))
                    {
                        var l = links.IndexOf(url);
                        if (l == -1)
                        {
                            links.Add(url);
                            l = links.Count - 1;
                        }

                        block(false);
                        nodeStack.Push("![");
                        if (alt != "")
                        {
                            nodeStack.Push(alt);
                        }
                        else if (title != null)
                        {
                            nodeStack.Push(title);
                        }

                        nodeStack.Push("][" + l + "]");
                        block(false);
                    }
                    else
                    {
                        //if image is not a link image then treat images as block elements
                        if (false == HtmlToMarkdownConverterHelper.startsWith(HtmlToMarkdownConverterHelper.peekTillNotEmpty(nodeStack.ToList()), "["))
                        {
                            block(false);
                        }

                        nodeStack.Push("![" + alt + "](" + url + (false == string.IsNullOrWhiteSpace(title) ? " \"" + title + "\"" : "") + ")");

                        if (false == HtmlToMarkdownConverterHelper.startsWith(HtmlToMarkdownConverterHelper.peekTillNotEmpty(nodeStack.ToList()), "["))
                        {
                            block(true);
                        }
                    }
                    break;
                case "blockquote":
                    //listBlock();
                    block(false);
                    blockquoteStack.Push(Markdown.Tags[tag]);
                    break;
                case "pre":
                    block(false);
                    preStack.Push(true);
                    nodeStack.Push("    ");
                    break;
                //case "table":
                //    nodeStack.Push("<table>");
                //    break;
                //case "thead":
                //    nodeStack.Push("<thead>");
                //    break;
                //case "tbody":
                //    nodeStack.Push("<tbody>");
                //    break;
                //case "tr":
                //    nodeStack.Push("<tr>");
                //    break;
                //case "td":
                //    nodeStack.Push("<td>");
                //    break;
            }
        }
    }
}
