using System.Collections.Generic;

namespace HtmlParser
{
    public static class StackExtensions
    {
        public static string SafePeek(this Stack<string> stack)
        {
            if (stack.Count == 0)
            {
                return string.Empty;
            }

            return stack.Peek();
        }
    }
}