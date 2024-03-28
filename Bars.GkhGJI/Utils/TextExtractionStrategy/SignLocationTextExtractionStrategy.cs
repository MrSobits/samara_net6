using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Bars.GkhGji.Utils.TextExtractionStrategy
{
    // TODO: Заменить itextSharp
   /* public class SignLocationTextExtractionStrategy : LocationTextExtractionStrategy
    {
        public List<Rectangle> TextPoints = new List<Rectangle>();

        private string TextToSearchFor { get; set; }

        private CompareOptions CompareOptions { get; set; }

        public SignLocationTextExtractionStrategy(string textToSearchFor, CompareOptions compareOptions = CompareOptions.None)
        {
            TextToSearchFor = textToSearchFor;
            CompareOptions = compareOptions;
        }

        public override void RenderText(TextRenderInfo renderInfo)
        {
            base.RenderText(renderInfo);

            var startPosition = CultureInfo.CurrentCulture.CompareInfo.IndexOf(renderInfo.GetText(), TextToSearchFor, CompareOptions);

            if (startPosition < 0)
            {
                return;
            }

            var chars = renderInfo.GetCharacterRenderInfos().Skip(startPosition).Take(TextToSearchFor.Length).ToList();

            var firstChar = chars.First();
            var lastChar = chars.Last();

            var bottomLeft = firstChar.GetDescentLine().GetStartPoint();
            var topRight = lastChar.GetAscentLine().GetEndPoint();

            var rect = new Rectangle(bottomLeft[Vector.I1], bottomLeft[Vector.I2], topRight[Vector.I1], topRight[Vector.I2]);

            TextPoints.Add(rect);
        }
    }*/
}
