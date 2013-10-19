using Xunit;

namespace HtmlToMarkdown.Net.Tests
{
    public class ReplaceSpecialCharsTests
    {
        [Fact]
        public void Should_handle_AND_sign()
        {
            using (var runner = new HtmlToMarkdownRunner())
            {
                runner.Input = "&copy;";
                runner.Expected = "&copy;";
            }
        }

        [Fact]
        public void Should_handle_AND_sign_in_text()
        {
            using (var runner = new HtmlToMarkdownRunner())
            {
                runner.Input = "AT&amp;T";
                runner.Expected = "AT&T";
            }
        }

        [Fact]
        public void Should_handle_LT_sign()
        {
            using (var runner = new HtmlToMarkdownRunner())
            {
                runner.Input = "4 &lt; 5";
                runner.Expected = "4 < 5";
            }
        }
    }
}