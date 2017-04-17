using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace OFrameLibrary.Helpers
{
    public static class StringHelper
    {
        public static ListItem GetKey(Match match)
        {
            var key = match.Value.Replace("{", string.Empty).Replace("}", string.Empty);

            var keyParts = key.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

            return new ListItem(keyParts[0], keyParts[1]);
        }

        public static MatchCollection GetMatchCollection(string content)
        {
            const string regExp = "(\\{)((?:[a-z][a-z0-9_]*))(:)(\\d+)(\\})";

            var r = new Regex(regExp, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            var mc = r.Matches(content);

            if (mc.Count > 0)
            {
                return mc;
            }
            else
            {
                return null;
            }
        }

        public static bool HasSpecialChar(string text)
        {
            var chars = new string[] { ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "-", "_", "(", ")", ":", "|", "[", "]" };

            for (var i = 0; i < chars.Length; i++)
            {
                if (text.Contains(chars[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsEmail(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                const string emailRegX = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                              @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                              @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

                return Regex.IsMatch(email, emailRegX, RegexOptions.Compiled);
            }
            else
            {
                return false;
            }
        }

        public static bool IsWebURL(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                const string urlRegEx = @"(http|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?";

                return Regex.IsMatch(url, urlRegEx, RegexOptions.Compiled);
            }
            else
            {
                return false;
            }
        }

        public static string RemoveKeys(string content)
        {
            const string regExp = "(\\{)((?:[a-z][a-z0-9_]*))(:)(\\d+)(\\})";

            var r = new Regex(regExp, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            return r.Replace(content, string.Empty);
        }

        public static string RemoveSpecialChars(string text)
        {
            var chars = new string[] { ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "-", "_", "(", ")", ":", "|", "[", "]", " " };

            for (var i = 0; i < chars.Length; i++)
            {
                if (text.Contains(chars[i]))
                {
                    text = text.Replace(chars[i], string.Empty);
                }
            }
            return text;
        }

        public static string RemoveTruncator(string content)
        {
            return content.Replace("{TRUNCATE-HERE}", string.Empty);
        }

        /// <summary>
        /// Strips all HTML tags from a string
        /// </summary>
        /// <param name="s"></param>
        /// <param name="html">todo: describe html parameter on StripHtml</param>
        /// <returns></returns>
        public static string StripHtml(this string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return html;
            }

            return Regex.Replace(html, @"<(.|\n)*?>", string.Empty);
        }

        /// <summary>
        /// Truncates text to a number of characters
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxCharacters"></param>
        /// <param name="trailingText"></param>
        /// <returns></returns>
        public static string Truncate(this string text, int maxCharacters)
        {
            return text.Truncate(maxCharacters, null);
        }

        /// <summary>
        /// Truncates text to a number of characters and adds trailing text, i.e. elipses, to the end
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxCharacters"></param>
        /// <param name="trailingText"></param>
        /// <returns></returns>
        public static string Truncate(this string text, int maxCharacters, string trailingText)
        {
            if (string.IsNullOrEmpty(text) || maxCharacters <= 0 || text.Length <= maxCharacters)
            {
                return text;
            }
            else
            {
                return text.Substring(0, maxCharacters) + trailingText;
            }
        }

        public static string TruncateFromHere(string content)
        {
            var position = content.IndexOf("{TRUNCATE-HERE}", 0);

            if (position > 0)
            {
                return content.Remove(position);
            }
            else
            {
                return content;
            }
        }

        /// <summary>
        /// Truncates a string containing HTML to a number of text characters, keeping whole words.
        /// The result contains HTML and any tags left open are closed.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="html">todo: describe html parameter on TruncateHtml</param>
        /// <param name="maxCharacters">todo: describe maxCharacters parameter on TruncateHtml</param>
        /// <param name="trailingText">todo: describe trailingText parameter on TruncateHtml</param>
        /// <returns></returns>
        public static string TruncateHtml(this string html, int maxCharacters, string trailingText)
        {
            if (string.IsNullOrEmpty(html))
            {
                return html;
            }

            // find the spot to truncate
            // count the text characters and ignore tags
            var textCount = 0;
            var charCount = 0;
            var ignore = false;
            foreach (char c in html)
            {
                charCount++;
                if (c == '<')
                {
                    ignore = true;
                }
                else if (!ignore)
                {
                    textCount++;
                }

                ignore &= c != '>';

                // stop once we hit the limit
                if (textCount >= maxCharacters)
                {
                    break;
                }
            }

            // Truncate the html and keep whole words only
            var trunc = new StringBuilder(html.TruncateWords(charCount));

            // keep track of open tags and close any tags left open
            var tags = new Stack<string>();
            foreach (Match match in Regex.Matches(trunc.ToString(),
                @"<((?<tag>[^\s/>]+)|/(?<closeTag>[^\s>]+)).*?(?<selfClose>/)?\s*>",
                RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline))
            {
                if (match.Success)
                {
                    var tag = match.Groups["tag"].Value;
                    var closeTag = match.Groups["closeTag"].Value;

                    // push to stack if open tag and ignore it if it is self-closing, i.e. <br />
                    if (!string.IsNullOrEmpty(tag) && string.IsNullOrEmpty(match.Groups["selfClose"].Value))
                    {
                        tags.Push(tag);
                    }

                    // pop from stack if close tag
                    else if (!string.IsNullOrEmpty(closeTag))
                    {
                        // pop the tag to close it.. find the matching opening tag
                        // ignore any unclosed tags
                        while (tags.Pop() != closeTag && tags.Count > 0)
                        {
                        }
                    }
                }
            }

            if (html.Length > charCount)
            {
                // add the trailing text
                trunc.Append(trailingText);
            }

            // pop the rest off the stack to close remainder of tags
            while (tags.Count > 0)
            {
                trunc.Append("</");
                trunc.Append(tags.Pop());
                trunc.Append('>');
            }

            return trunc.ToString();
        }

        /// <summary>
        /// Truncates a string containing HTML to a number of text characters, keeping whole words.
        /// The result contains HTML and any tags left open are closed.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="html">todo: describe html parameter on TruncateHtml</param>
        /// <param name="maxCharacters">todo: describe maxCharacters parameter on TruncateHtml</param>
        /// <returns></returns>
        public static string TruncateHtml(this string html, int maxCharacters)
        {
            return html.TruncateHtml(maxCharacters, null);
        }

        /// <summary>
        /// Truncates text and discars any partial words left at the end
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxCharacters"></param>
        /// <param name="trailingText"></param>
        /// <returns></returns>
        public static string TruncateWords(this string text, int maxCharacters)
        {
            return text.TruncateWords(maxCharacters, null);
        }

        /// <summary>
        /// Truncates text and discars any partial words left at the end
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxCharacters"></param>
        /// <param name="trailingText"></param>
        /// <returns></returns>
        public static string TruncateWords(this string text, int maxCharacters, string trailingText)
        {
            if (string.IsNullOrEmpty(text) || maxCharacters <= 0 || text.Length <= maxCharacters)
            {
                return text;
            }

            // trunctate the text, then remove the partial word at the end
            return Regex.Replace(text.Truncate(maxCharacters),
                @"\s+[^\s]+$", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled) + trailingText;
        }
    }
}
