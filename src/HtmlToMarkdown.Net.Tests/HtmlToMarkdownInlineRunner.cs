using System;
using Xunit;

namespace HtmlToMarkdown.Net.Tests
{
    public class HtmlToMarkdownInlineRunner : IDisposable
    {
        public string Input { get; set; }
        public string Expected { get; set; }

        public void Dispose()
        {
            var converter = new HtmlToMarkdownConverter(true);

            Assert.Equal(Expected, converter.Convert(Input));
        }
    }
}