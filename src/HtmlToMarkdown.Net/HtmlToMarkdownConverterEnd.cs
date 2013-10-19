using HtmlConverters;
using System.Text.RegularExpressions;

namespace HtmlToMarkdown.Net
{
    public partial class HtmlToMarkdownConverter
    {
        public override void end(string tag)
        {
            tag = tag.ToLower();

            switch (tag)
            {
                case "title":
                case "h1":
                case "h2":
                case "h3":
                case "h4":
                case "h5":
                case "h6":
                    if (!removeIfEmptyTag(Markdown.Tags[tag]))
                    {
                        block(true);
                    }
                    break;
                case "p":
                case "div":
                case "table":
                case "tbody":
                case "tr":
                case "td":
                    while (nodeStack.Count > 0 && nodeStack.Peek().Trim() == "")
                    {
                        nodeStack.Pop();
                    }
                    block(true);
                    break;
                case "b":
                case "strong":
                case "i":
                case "em":
                case "dfn":
                case "var":
                case "cite":
                    if (!removeIfEmptyTag(Markdown.Tags[tag]))
                    {
                        nodeStack.Push(sliceText(Markdown.Tags[tag]).Trim());
                        nodeStack.Push(Markdown.Tags[tag]);
                    }
                    break;
                case "a":
                    var text = sliceText("[");
                    text = Regex.Replace(text, @"\s+", " ");
                    text = text.Trim();

                    if (text == "")
                    {
                        nodeStack.Pop();
                        break;
                    }

                    var attrs = linkAttrStack.Pop();
                    var url = attrs.ContainsKey("href") && attrs["href"].Value != "" ? attrs["href"].Value : "";

                    if (url == "")
                    {
                        nodeStack.Pop();
                        nodeStack.Push(text);
                        break;
                    }

                    nodeStack.Push(text);

                    if (!inlineStyle && !HtmlToMarkdownConverterHelper.startsWith(nodeStack.Peek(), "!"))
                    {
                        var l = links.IndexOf(url);
                        if (l == -1)
                        {
                            links.Add(url);
                            l = links.Count - 1;
                        }
                        nodeStack.Push("][" + l + "]");
                    }
                    else
                    {
                        if (HtmlToMarkdownConverterHelper.startsWith(nodeStack.Peek(), "!"))
                        {
                            var localText = nodeStack.Pop();
                            localText = nodeStack.Pop() + localText;
                            block(false);
                            nodeStack.Push(localText);
                        }

                        var title = attrs["title"];
                        var trimmedTitle = title.Value.Trim();
                        nodeStack.Push("](" + url + (false == string.IsNullOrWhiteSpace(title.Value) ? " \"" + Regex.Replace(trimmedTitle, @"\s+", " ") + "\"" : "") + ")");

                        if (HtmlToMarkdownConverterHelper.startsWith(nodeStack.Peek(), "!"))
                        {
                            block(true);
                        }
                    }
                    break;
                case "ul":
                case "ol":
                case "dl":
                    listBlock();
                    listTagStack.Pop();
                    break;
                case "li":
                case "dt":
                    var li = getListMarkdownTag();
                    if (!removeIfEmptyTag(li))
                    {
                        var liContent = sliceText(li).Trim();

                        if (liContent.StartsWith("[!["))
                        {
                            nodeStack.Pop();
                            block(false);
                            nodeStack.Push(liContent);
                            block(true);
                        }
                        else
                        {
                            nodeStack.Push(liContent);
                            listBlock();
                        }
                    }
                    break;
                case "blockquote":
                    blockquoteStack.Pop();
                    break;
                case "pre":
                    //        //uncomment following experimental code to discard line numbers when syntax highlighters are used
                    //        //notes this code thorough testing before production user
                    //        /*
                    //        var p=[];
                    //        var flag = true;
                    //        var count = 0, whiteSpace = 0, line = 0;
                    //        console.log(">> " + peek(nodeList));
                    //        while(peek(nodeList).startsWith("    ") || flag == true)
                    //        {
                    //            //console.log('inside');
                    //            var text = nodeList.pop();
                    //            p.push(text);

                    //            if(flag == true && !text.startsWith("    ")) {
                    //                continue;
                    //            } else {
                    //                flag = false;
                    //            }

                    //            //var result = parseInt(text.trim());
                    //            if(!isNaN(text.trim())) {
                    //                count++;
                    //            } else if(text.trim() == ""){
                    //                whiteSpace++;
                    //            } else {
                    //                line++;
                    //            }
                    //            flag = false;
                    //        }

                    //        console.log(line);
                    //        if(line != 0)
                    //        {
                    //            while(p.length != 0) {
                    //                nodeList.push(p.pop());
                    //            }
                    //        }
                    //        */
                    //        block(true);
                    //        preStack.pop();
                    break;
                case "code":
                case "span":
                    if (preStack.Count > 0)
                    {
                        break;
                    }
                    else if (string.IsNullOrWhiteSpace(nodeStack.Peek()))
                    {
                        nodeStack.Pop();
                        nodeStack.Push(Markdown.Tags[tag]);
                    }
                    else
                    {
                        var item = nodeStack.Pop();
                        nodeStack.Push(item.Trim());
                        nodeStack.Push(Markdown.Tags[tag]);
                    }
                    break;
                //case "table":
                //    nodeList.push("</table>");
                //    break;
                //case "thead":
                //    nodeList.push("</thead>");
                //    break;
                //case "tbody":
                //    nodeList.push("</tbody>");
                //    break;
                //case "tr":
                //    nodeList.push("</tr>");
                //    break;
                //case "td":
                //    nodeList.push("</td>");
                //    break;
                case "br":
                case "hr":
                case "img":
                    break;
            }

        }
    }
}
