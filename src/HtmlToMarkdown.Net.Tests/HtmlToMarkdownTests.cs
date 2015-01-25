using Xunit;

namespace HtmlToMarkdown.Net.Tests
{
    public class HtmlToMarkdownTests
    {
        private HtmlToMarkdownConverter _converter;
        private HtmlToMarkdownConverter _inlineConverter;

        public HtmlToMarkdownTests()
        {
            _converter = new HtmlToMarkdownConverter();
            _inlineConverter = new HtmlToMarkdownConverter(true);
        }

        [Fact]
        public void should_be_able_to_convert_h1()
        {
            Assert.Equal("# H1\n\n", _converter.Convert("<h1>H1</h1>"));
        }

        [Fact]
        public void should_be_able_to_convert_h2()
        {
            Assert.Equal("## H2\n\n", _converter.Convert("<h2>H2</h2>"));
        }

        [Fact]
        public void should_be_able_to_convert_h3()
        {
            Assert.Equal("### H3\n\n", _converter.Convert("<h3>H3</h3>"));
        }

        [Fact]
        public void should_be_able_to_convert_h4()
        {
            Assert.Equal("#### H4\n\n", _converter.Convert("<h4>H4</h4>"));
        }

        [Fact]
        public void should_be_able_to_convert_h5()
        {
            Assert.Equal("##### H5\n\n", _converter.Convert("<h5>H5</h5>"));
        }

        [Fact]
        public void should_be_able_to_convert_h6()
        {
            var html = "<h6>H6</h6>";
            var expected = "###### H6\n\n";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_convert_strong()
        {
            var html = "<strong>Bold</strong>";
            var expected = "**Bold**";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_convert_b()
        {
            var html = "<b>Bold</b>";
            var expected = "**Bold**";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_convert_em()
        {
            var html = "<em>Italic</em>";
            var expected = "_Italic_";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_convert_i()
        {
            var html = "<i>Italic</i>";
            var expected = "_Italic_";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_convert_title()
        {
            var html = "<title>This is document Title</title>";
            var expected = "# This is document Title\n\n";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_trim_text_inside_elements()
        {
            var html = "<strong> String </strong>";
            var expected = "**String**";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_convert_strong_inside_text()
        {
            var html = "This has a <strong>block</strong> word";
            var expected = "This has a **block** word";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_convert_hr()
        {
            var html = "<hr />";
            var expected = "- - -\n\n";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_convert_br()
        {
            var html = "<br/>";
            var expected = "  \n";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_convert_strong_and_em_inside_text()
        {
            var html = "This has <strong>blocked and <em>italicized</em></strong> texts.";
            var expected = "This has **blocked and _italicized_** texts.";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_convert_hr_inside_text()
        {
            var html = "this is text before hr<hr/>this is text after hr";
            var expected = "this is text before hr\n\n- - -\n\nthis is text after hr";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_convert_br_inside_text()
        {
            var html = "this is text before break<br/>this is text after break";
            var expected = "this is text before break  \nthis is text after break";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_convert_p()
        {
            var html = "<p>This is a paragraph. This is the second sentence.</p>";
            var expected = "This is a paragraph. This is the second sentence.\n\n";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_convert_p_inside_text()
        {
            var html = "this is text before paragraph<p>This is a paragraph</p>this is text after paragraph";
            var expected = "this is text before paragraph\n\nThis is a paragraph\n\nthis is text after paragraph";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_convert_blockquote()
        {
            var html = "<blockquote>This is blockquoted</blockquote>";
            var expected = "> This is blockquoted";

            Assert.Equal(expected, _converter.Convert(html));
        }
        [Fact]
        public void Should_convert_blockquote_inside_blockquote()
        {
            var html = "<blockquote>This is blockquoted<blockquote>This is nested blockquoted</blockquote></blockquote>";
            var expected = "> This is blockquoted\n\n> > This is nested blockquoted";

            Assert.Equal(expected, _converter.Convert(html));
        }


        [Fact]
        public void Should_convert_span()
        {
            var html = "<span>this is span element</span>";
            var expected = " this is span element ";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_convert_span_inside_text_without_spaces()
        {
            var html = "before<span>this is span element</span>after";
            var expected = "before this is span element after";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_convert_span_inside_text_with_spaces()
        {
            var html = "before <span>this is span element</span> after";
            var expected = "before this is span element after";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_be_able_to_convert_a_block_of_html()
        {
            using (var runner = new HtmlToMarkdownRunner())
            {
                runner.Input = "<p>This is a paragraph. Followed by a blockquote.</p><blockquote><p>This is a blockquote which will be truncated at 75 characters width. It will be somewhere around here.</p></blockquote>" + "<p>Some list for you:</p><ul><li>item a</li><li>item b</li></ul><p>So which one do you choose?</p>";

                runner.Expected = "This is a paragraph. Followed by a blockquote.\n\n> \nThis is a blockquote which will be truncated at 75 characters width. It \nwill be somewhere around here.\n\nSome list for you:\n\n* item a\n* item b\n\nSo which one do you choose?\n\n";
            }
        }

        [Fact]
        public void Should_convert_ul()
        {
            var html = "<ul><li>item a</li><li>item b</li></ul>";
            var expected = "* item a\n* item b\n";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_convert_ol()
        {
            var html = "<ol><li>item 1</li><li>item 2</li></ol>";
            var expected = "1. item 1\n1. item 2\n";

            Assert.Equal(expected, _converter.Convert(html));
        }
        [Fact]
        public void Should_convert_ul_nested()
        {
            var html = "<ul><li>item a<ul><li>item aa</li><li>item bb</li></ul></li><li>item b</li></ul>";
            var expected = "* item a\n  * item aa\n  * item bb\n* item b\n";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_not_convert_empty_list_ol()
        {
            var html = "<ol><li>item 1</li><li/></ol>";
            var expected = "1. item 1\n";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_not_convert_empty_list_ul()
        {
            var html = "<ul><li>item 1</li><li></li></ul>";
            var expected = "* item 1\n";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_convert_image_inline()
        {
            var html = "<img alt=\"Example Image\" title=\"Free example image\" src=\"/img/62838.jpg\"/>";
            var expected = "![Example Image](/img/62838.jpg \"Free example image\")\n\n";
            Assert.Equal(expected, _inlineConverter.Convert(html));
        }

        [Fact]
        public void Should_convert_image()
        {
            var html = "<img alt=\"Example Image\" title=\"Free example image\" src=\"/img/62838.jpg\"/>";
            var expected = "![Example Image][0]\n\n[0]: /img/62838.jpg";
            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_convert_image_if_alt_is_empty_use_title()
        {
            var html = "<img title=\"Free example image title\" src=\"/img/62838.jpg\">";
            var expected = "![Free example image title][0]\n\n[0]: /img/62838.jpg";
            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_convert_image_in_text()
        {
            var html = "before<img alt=\"Example Image\" title=\"Free example image\" src=\"/img/62838.jpg\"/>after";
            var expected = "before\n\n![Example Image][0]\n\nafter\n\n[0]: /img/62838.jpg";
            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_convert_image_in_text_inline()
        {
            var html = "before<img alt=\"Example Image\" title=\"Free example image\" src=\"/img/62838.jpg\"/>after";
            var expected = "before\n\n![Example Image](/img/62838.jpg \"Free example image\")\n\nafter";
            Assert.Equal(expected, _inlineConverter.Convert(html));
        }
        [Fact]
        public void Should_not_convert_image_if_url_is_empty()
        {
            var html = "<img alt=\"Example Image\" title=\"Free example image\">";
            var expected = string.Empty;
            Assert.Equal(expected, _inlineConverter.Convert(html));
        }

        [Fact]
        public void Should_convert_links()
        {
            var html = "<a href=\"http://www.example.com\" title=\"Example\">Visit Example</a>";
            html += "text1";
            html += "<a href=\"http://www.example1.com\" title=\"Example\">Visit Example1</a>";
            html += "text2";
            html += "<a href=\"http://www.example.com\" title=\"Example\">Visit Example</a>";

            //urls should not be duplicated in reference style
            var expected = "[Visit Example][0]text1[Visit Example1][1]text2[Visit Example][0]";
            expected += "\n\n";
            expected += "[0]: http://www.example.com\n";
            expected += "[1]: http://www.example1.com";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_convert_links_inline()
        {
            var html = "<a href=\"http://www.example.com\" title=\"Example\">Visit Example</a>";
            var expected = "[Visit Example](http://www.example.com \"Example\")";

            Assert.Equal(expected, _inlineConverter.Convert(html));
        }

				[Fact]
				public void Should_convert_links_that_have_no_title_attribute_inline()
				{
					var html = "<a href=\"http://www.example.com\">Visit Example</a>";
					var expected = "[Visit Example](http://www.example.com)";

					Assert.Equal(expected, _inlineConverter.Convert(html));
				}

        [Fact]
        public void Should_not_convert_links_if_no_text_to_display()
        {
            var html = "<a href=\"/\"/>";
            var expected = "";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void Should_not_convert_links_if_no_text_to_display_in_block()
        {
            var html = "<div class=\"logo\">\n";
            html += "	<a href=\"/\"/>\n";
            html += "</div>";
            var expected = "";

            Assert.Equal(expected, _converter.Convert(html));
        }


        [Fact]
        public void Should_convert_elements_with_child_elements_surrounded_by_whitespace()
        {
            var html = "<div>\n\t<h2>\n\t\t<a href=\"http://finance.yahoo.com\">Yahoo! Finance</a>\n\t</h2>\n</div>";
            var expected = "## [Yahoo! Finance][0]\n\n[0]: http://finance.yahoo.com";
            Assert.Equal(expected, _converter.Convert(html));


            html = "<span>\n\t<b>Hello</b>\n\t</span>";
            expected = " **Hello** ";
            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void should_convert_image_wrapped_in_anchor_to_markdown_that_can_be_rendered_using_showdown_inline()
        {
            using (var runner = new HtmlToMarkdownRunner())
            {
                runner.Input = "<a href=\"/exec/j/4/?pid=62838&lno=1&afsrc=1\"><img alt=\"Example Image\" title=\"Free example image\" src=\"/img/62838.jpg\"></a>";
                runner.Expected = "[![Example Image](/img/62838.jpg \"Free example image\")](/exec/j/4/?pid=62838&lno=1&afsrc=1)";
            }
        }

        [Fact]
        public void should_convert_image_wrapped_in_anchor_to_markdown_that_can_be_rendered_using_showdown()
        {
            using (var runner = new HtmlToMarkdownRunner())
            {
                runner.Input = "<a href=\"/exec/j/4/?pid=62838&lno=1&afsrc=1\"><img alt=\"Example Image\" title=\"Free example image\" src=\"/img/62838.jpg\"></a>";
                runner.Expected = "[![Example Image](/img/62838.jpg \"Free example image\")](/exec/j/4/?pid=62838&lno=1&afsrc=1)";

                //var html = "<a href=\"/exec/j/4/?pid=62838&lno=1&afsrc=1\">\n\t<img alt=\"Example Image\" title=\"Free example image\" src=\"/img/62838.jpg\">\n\t</a>";
            }
        }

        [Fact]
        public void Should_output_only_text_of_empty_links_inline()
        {
            using (var runner = new HtmlToMarkdownRunner())
            {
                runner.Input = "<a href=\"\">Empty Link Text</a>";
                runner.Expected = "Empty Link Text";
            }
        }

        [Fact]
        public void Should_not_convert_any_elements_that_have_no_parsing_rules()
        {
            //tags that have no parsing rules e.g. form elements "head", "style", script", "link" "option", "noscript", "noframes", "input", "button", "select", "textarea", and "label"
            var html = "<head><link rel=\"openid.delegate\" href=\"http://jeresig.livejournal.com/\"/>";
            html += "<script src=\"http://ejohn.org/files/retweet.js\"></script></head>";

            Assert.Equal(string.Empty, _converter.Convert(html));
        }

        [Fact]
        public void should_be_able_to_convert_tables_to_table_syntax()
        {
            using (var runner = new HtmlToMarkdownRunner())
            {
                runner.Input = "<table border=\"1\">";
                runner.Input += "<tr><td>Row 1 Cell 1</td><td>Row 1 Cell 2</td></tr>";
                runner.Input += "<tr><td>Row 2 Cell 1</td><td>Row 2 Cell 2</td></tr>";
                runner.Input += "</table>";

                runner.Expected = "Row 1 Cell 1\n\n";
                runner.Expected += "Row 1 Cell 2\n\n";
                runner.Expected += "Row 2 Cell 1\n\n";
                runner.Expected += "Row 2 Cell 2\n\n";
            }
        }

        [Fact]
        public void should_be_able_to_convert_tables_to_row_syntax()
        {
            using (var runner = new HtmlToMarkdownRunner())
            {
                runner.Input = "<table border=\"1\">";
                runner.Input += "<tr><td>Row 1 Cell 1</td><td>Row 1 Cell 2</td></tr>";
                runner.Input += "<tr><td>Row 2 Cell 1</td><td>Row 2 Cell 2</td></tr>";
                runner.Input += "</table>";

                runner.Expected = "Row 1 Cell 1\n\n";
                runner.Expected += "Row 1 Cell 2\n\n";
                runner.Expected += "Row 2 Cell 1\n\n";
                runner.Expected += "Row 2 Cell 2\n\n";
            }
        }

        [Fact]
        public void Should_be_able_to_convert_tables_with_lists()
        {
            using (var runner = new HtmlToMarkdownRunner())
            {
                runner.Input = "<table border=\"1\">";
                runner.Input += "<tr><td width=\"50%\"><ul><li>List Item 1</li><li>List Item 2</li></ul></td>";
                runner.Input += "<td><ul><li>List Item 3</li><li>List Item 4</li></ul></td></tr>";
                runner.Input += "</table>";

                runner.Expected = "* List Item 1\n* List Item 2\n\n* List Item 3\n* List Item 4\n\n";
            }
        }

        [Fact]
        public void Should_not_convert_empty_div()
        {
            using (var runner = new HtmlToMarkdownRunner())
            {
                runner.Input = "<div>        </div>";
                runner.Expected = string.Empty;
            }
        }
        [Fact]
        public void Should_not_convert_empty_h1()
        {
            using (var runner = new HtmlToMarkdownRunner())
            {
                runner.Input = "<h1>        </h1>";
                runner.Expected = string.Empty;
            }
        }
        [Fact]
        public void Should_not_convert_empty_b()
        {
            using (var runner = new HtmlToMarkdownRunner())
            {
                runner.Input = "<b>        </b>";
                runner.Expected = string.Empty;
            }
        }

        [Fact]
        public void Should_collape_whitespace_to_single_space_for_text_nodes()
        {
            Assert.Equal(" a b c d \n\n", "<div>     a     b     c\n     d    </div>");

            Assert.Equal(" a b c d \n\n", "<div></div><div>     a     b     c\n     d    </div>");

            Assert.Equal("1\n\na b c d \n\n", "<div>1</div><div>     a     b     c\n     d    </div>");

            Assert.Equal("# a b c d \n\n", "<h1>     a     b     c\n     d </h1>");
        }

        [Fact]
        public void should_trim_anchor_title_and_text_inline()
        {
            using (var runner = new HtmlToMarkdownInlineRunner())
            {
                runner.Input = "<a href=\"http://www.example.com\" title=\"   Example   \">   Visit Example    </a>";
                runner.Expected = "[Visit Example](http://www.example.com \"Example\")";
            }
        }
        [Fact]
        public void should_trim_anchor_title_and_text()
        {
            using (var runner = new HtmlToMarkdownRunner())
            {
                runner.Input = "<a href=\"http://www.example.com\" title=\"   Example   \">   Visit Example    </a>";
                runner.Expected = "[Visit Example][0]\n\n[0]: http://www.example.com";
            }
        }

        [Fact]
        public void should_trim_anchor_title_and_text_relative_path()
        {
            using (var runner = new HtmlToMarkdownRunner())
            {
                runner.Input = "<a href=\"/blog/line-length-readability#comments\">\n" +
                               "<span itemprop=\"interactionCount\">32</span>\n" + "comments\n</a>";
                runner.Expected = "[32 comments][0]\n\n[0]: /blog/line-length-readability#comments";
            }
        }

        [Fact]
        public void should_trim_image_alt_and_title()
        {
            var html = "<img alt=\"  Example Image   \" title=\"   Free example image   \" src=\"/img/62838.jpg\">";

            var expected = "![Example Image](/img/62838.jpg \"Free example image\")\n\n";
            Assert.Equal(expected, _inlineConverter.Convert(html));

            expected = "![Example Image][0]\n\n[0]: /img/62838.jpg";
            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void should_be_able_to_convert_image_followed_by_link_to_markdown_that_can_be_renderd_using_showdown()
        {
            var html = "<p>\n";
            html += "	<img alt=\"Feed\" class=\"icon\" src=\"http://mementodb.com/image/logo.png\"/>\n";
            html += "	<a href=\"http://mementodb.com\">Memento</a>\n";
            html += "</p>";

            var expected = "![Feed][0]\n\n[Memento][1]\n\n";
            expected += "[0]: http://mementodb.com/image/logo.png\n";
            expected += "[1]: http://mementodb.com";

            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void should_be_able_to_convert_list_items_with_linked_image_as_only_linked_image()
        {
            using (var runner = new HtmlToMarkdownRunner())
            {
                runner.Input = "before list";
                runner.Input += "<ul>\n";
                runner.Input += "	<li><div class=\"curve-down\"><a href=\"/ipad/#video\"><img src=\"http://image.apple.com/home/image/promo_video_ipad_launch.png\" alt=\"Watch the new iPad video\" width=\"237\" height=\"155\" /><span class=\"play\"></span></a></div></li>";
                runner.Input += "	<li><div class=\"curve-down\"><a href=\"/iphone/videos/#tv-ads-datenight\"><img src=\"http://image.apple.com/home/image/promo_video_iphone4s_ad.png\" alt=\"Watch the new iPhone TV Ad\" width=\"237\" height=\"155\" /><span class=\"play\"></span></a></div></li>";
                runner.Input += "</ul>\n";

                runner.Expected = "before list\n\n";
                runner.Expected += "[![Watch the new iPad video](http://image.apple.com/home/image/promo_video_ipad_launch.png)](/ipad/#video)\n\n";
                runner.Expected += "[![Watch the new iPhone TV Ad](http://image.apple.com/home/image/promo_video_iphone4s_ad.png)](/iphone/videos/#tv-ads-datenight)\n\n";
            }
        }

        [Fact]
        public void should_be_able_to_convert_title()
        {
            var html = "<hgroup>\n";
            html += "\t<h1><a href=\"http://www.google.com\">Nathen Harvey</a></h1>\n";
            html += "\t<h2>a blog</h2>\n";
            html += "</hgroup>";

            var expected = "# [Nathen Harvey][0]\n\n## a blog\n\n\n\n[0]: http://www.google.com";
            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void should_be_able_to_convert_paragrphs_in_blocquotes()
        {
            var html = "<blockquote>\n";
            html += "\t<p>Lorem ipsum</p>\n";
            html += "\t<p>Lorem ipsum</p>\n";
            html += "</blockquote>";

            var expected = "> Lorem ipsum\n\n> Lorem ipsum\n\n";
            Assert.Equal(expected, _converter.Convert(html));

            html = "<blockquote>\n";
            html += "\t<p>Lorem ipsum</p>\n";
            html += "</blockquote>\n";
            html += "<blockquote>\n";
            html += "\t<p>Lorem ipsum</p>\n";
            html += "</blockquote>";
            Assert.Equal(expected, _converter.Convert(html));
        }

        [Fact]
        public void should_be_able_to_convert_pre_block()
        {
            using (var runner = new HtmlToMarkdownRunner())
            {
                runner.Input = "<pre>";
                runner.Input += "	void main(String[] args) {\n";
                runner.Input += "		System.out.println(\"Hello Markdown\");\n";
                runner.Input += "	}";
                runner.Input += "</pre>";

                runner.Expected = "    " + "	void main(String[] args) {\n";
                runner.Expected += "    " + "		System.out.println(\"Hello Markdown\");\n";
                runner.Expected += "    " + "	}";
                runner.Expected += "\n\n";
            }
        }

        [Fact]
        public void should_be_able_to_convert_pre_block_with_html_tags()
        {
            using (var runner = new HtmlToMarkdownRunner())
            {
                runner.Input = "<pre>\n";
                runner.Input += "<div a=\"b\">\n";
                runner.Input += "	<span>this is span inside pre block</span>\n";
                runner.Input += "	this is paragraph inside pre block\n";
                runner.Input += "</div>";
                runner.Input += "</pre>";

                runner.Expected = "    " + "\n\n\n";
                runner.Expected += "    " + "	this is span inside pre block\n";
                runner.Expected += "    " + "	this is paragraph inside pre block\n";
                runner.Expected += "    " + "\n";
                runner.Expected += "\n";
            }
        }

        [Fact]
        public void should_be_able_to_convert_code_inside_pre_blocks()
        {
            using (var runner = new HtmlToMarkdownRunner())
            {
                runner.Input = "<pre><code>{% blockquote [author[, source]] [link] [source_link_title] %}" +
                               "\nQuote string"
                               + "\n{% endblockquote %}"
                               + "\n</code></pre>";

                runner.Expected = "    {% blockquote [author[, source]] [link] [source_link_title] %}"
                                  + "\n    Quote string"
                                  + "\n    {% endblockquote %}"
                                  + "\n    "
                                  + "\n\n";
            }
        }
    }
}