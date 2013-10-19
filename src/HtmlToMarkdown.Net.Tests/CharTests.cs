using Xunit;

namespace HtmlToMarkdown.Net.Tests
{
    public class CharTests
    {
        [Fact]
        public void Should_replace_pre_content_new_lines_with_newlines_and_spaces()
        {
            var converter = new HtmlToMarkdownConverter();

            converter.preStack.Push(true);

            var input = "\n\n";
            var expected = "\n    \n    ";

            converter.chars(input);

            Assert.Equal(expected, converter.ReplaceForPre(input));
        }

        [Fact]
        public void Should_replace_multiple_whitespaces_with_one()
        {
            var converter = new HtmlToMarkdownConverter();

            converter.preStack.Push(true);

            var input = "  ";
            var expected = " ";

            converter.chars(input);

            Assert.Equal(expected, converter.ReplaceMultipleWhitespaveWithOne(input));
        }
    }
}