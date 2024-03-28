namespace Bars.Gkh.Formulas.Impl
{
    using System.Collections.Generic;
    using System.Text;
    using Bars.Gkh.Formulas;

    public class RussianFormulaTranslator : IFormulaTranslator
    {
        private readonly Dictionary<string, string> matches = new Dictionary<string, string> { { "если", "if" } };

        public string Translate(string formulaText)
        {
            formulaText = this.DecodeFromUtf8(formulaText);

            foreach (var match in this.matches)
            {
                formulaText = formulaText.Replace(match.Key, match.Value);
            }

            return formulaText;
        }

        public string DecodeFromUtf8(string utf8String)
        {
            byte[] bytUTF8;
            byte[] bytUnicode;
            var strUnicode = string.Empty;
            bytUTF8 = Encoding.UTF8.GetBytes(utf8String);
            bytUnicode = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, bytUTF8);
            strUnicode = Encoding.Unicode.GetString(bytUnicode);

            return strUnicode;
        }
    }
}