namespace Bars.Gkh.PrintForm.Utils
{
    using iTextSharp.text.pdf;
    using iTextSharp.text.pdf.parser;

    /// <summary>
    /// Стратегия установки штампа. Штамп посередине, +2.5мм от нижней границы контента
    /// </summary>
    public class AfterTextCentrStampStrategy : BaseStampStrategy
    {
        protected override void BeforeDraw(PdfSignatureAppearance appearance, PdfReader reader)
        {
            var lastPage = reader.NumberOfPages;
            var size = reader.GetPageSize(lastPage);

            var parser = new PdfReaderContentParser(reader);
            var finder = parser.ProcessContent(lastPage, new TextMarginFinder());
            var ly = finder.GetLly();

            Left = Left == 0f ? (size.Width / 2) - (Width / 2) : Left;
            Bottom = Bottom == 0f ? ly - Height - topStampDistance : Bottom;

            if (Bottom < bottomStampLimit)
            {
                const float additionalSpace  = 5 * oneMillimeter;
                Bottom = bottomStampLimit + additionalSpace;
            }
        }
    }
}