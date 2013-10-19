using System.Collections.Generic;

namespace Snow.HtmlParser
{
}

public class HtmlTags
{
    //HTML 4.01
    public static List<string> Empty = new List<string>
            {

                "area",
                "base",
                "basefont",
                "br",
                "col",
                "frame",
                "hr",
                "img",
                "input",
                "isindex",
                "link",
                "meta",
                "param",
                "embed"
            };

    public static List<string> Block = new List<string>
            {
                "address",
                "applet",
                "blockquote",
                "button",
                "center",
                "dd",
                "del",
                "dir",
                "div",
                "dl",
                "dt",
                "fieldset",
                "form",
                "frameset",
                "iframe",
                "ins",
                "isindex",
                "li",
                "map",
                "menu",
                "noframes",
                "noscript",
                "object",
                "ol",
                "p",
                "pre",
                "script",
                "table",
                "tbody",
                "td",
                "tfoot",
                "th",
                "thead",
                "tr",
                "ul"
            };

    public static List<string> Inline = new List<string>
            {
                "a",
                "abbr",
                "acronym",
                "applet",
                "b",
                "basefont",
                "bdo",
                "big",
                "br",
                "button",
                "cite",
                "code",
                "del",
                "dfn",
                "em",
                "font",
                "hr",
                "i",
                "iframe",
                "img",
                "input",
                "ins",
                "kbd",
                "label",
                "map",
                "object",
                "q",
                "s",
                "samp",
                "script",
                "select",
                "small",
                "span",
                "strike",
                "strong",
                "sub",
                "sup",
                "textarea",
                "tt",
                "u",
                "var"
            };

    // Elements that you can"," intentionally"," leave open (and which close themselves)
    public static List<string> SelfClosing = new List<string>
            {
                "colgroup",
                "dd",
                "dt",
                "li",
                "options",
                "p",
                "td",
                "tfoot",
                "th",
                "thead",
                "tr"
            };

    // Attributes that have their values filled in disabled= new List"disabled"
    public static List<string> FillAttrs = new List<string>
            {
                "checked",
                "compact",
                "declare",
                "defer",
                "disabled",
                "ismap",
                "multiple",
                "nohref",
                "noresize",
                "noshade",
                "nowrap",
                "readonly",
                "selected"
            };
}
