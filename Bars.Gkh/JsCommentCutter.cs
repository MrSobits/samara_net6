namespace Bars.Gkh
{
    using System.Text.RegularExpressions;

    using Bars.B4.JsCompression;

    /// <summary>
    /// Вырезает комментарии из js
    /// <para>
    /// Не использовать с параметром ResourceBundle.CompressCombinedJs
    /// </para>
    /// </summary>
    public class JsCommentCutter : IJavascriptCompressor
    {
        private static readonly Regex CommentRegex = new Regex(@"((?:\/\*(?:[^*]|(?:\*+[^*\/]))*\*+\/)|(?:\/\/.*))", RegexOptions.Compiled | RegexOptions.Multiline);
        /// <inheritdoc />
        public string Compress(string jsToCompress)
        {
            return JsCommentCutter.CommentRegex.Replace(jsToCompress, string.Empty);
        }
        
    }
}
