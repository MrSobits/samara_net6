namespace Bars.GkhGji.Utils.StampStrategy
{
    using Bars.GkhGji.Properties;
    using System;
    using System.IO;
    // TODO: Найти замену iTextSharp
/*
    
    /// <summary>
    /// Базовая стратегия установки штампа
    /// </summary>
    public class BaseStampStrategy
    {
        /// <summary>
        /// Серийный номер сертификата
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Причина подписания документа
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Имя сотрудника
        /// </summary>
        public string PersonName { get; set; }

        /// <summary>
        /// Дата с
        /// </summary>
        public DateTime ValidFromDate { get; set; }

        /// <summary>
        /// Дата по
        /// </summary>
        public DateTime ValidToDate { get; set; }

        /// <summary>
        /// Ключ
        /// </summary>
        public string ContextGuid { get; set; }

        /// <summary>
        /// Размер одного миллиметра
        /// </summary>
        protected const float oneMillimeter = 2.95f;

        /// <summary>
        /// Размер контура
        /// </summary>
        protected const float contourThickness = 0.6f * BaseStampStrategy.oneMillimeter;

        protected const float defaultMargin = 3.5f * BaseStampStrategy.oneMillimeter;

        /// <summary>
        /// Ширина печати
        /// </summary>
        protected float Width { get; set; }

        /// <summary>
        /// Высота печати
        /// </summary>
        protected float Height { get; set; }

        /// <summary>
        /// Отсутп от левого края
        /// </summary>
        protected virtual float Left { get; set; }

        /// <summary>
        /// Отступ от нижнего края
        /// </summary>
        protected virtual float Bottom { get; set; }

        /// <summary>
        /// Ширина прямоугольника
        /// </summary>
        protected float CentralRectWidth { get; set; }

        /// <summary>
        /// Высота прямоугольника
        /// </summary>
        protected float CentralRectHeight { get; set; }

        /// <summary>
        /// Отступ прямоугольника
        /// </summary>
        protected float CentralRectVerticalOffset { get; set; }

        /// <summary>
        /// Скругление контура
        /// </summary>
        protected float СontourRoundRadius { get; set; }

        /// <summary>
        /// Цвет подписи
        /// </summary>
        protected BaseColor SignatureColor { get; set; }

        /// <summary>
        /// Цвет шрифта в прямоугольнике
        /// </summary>
        protected BaseColor CentralFontColor { get; set; }

        /// <summary>
        /// Цвет шрифта в подписи
        /// </summary>
        protected BaseColor FontColor { get; set; }

        /// <summary>
        /// Путь до шрифтов
        /// </summary>
        public string FontPath { get; set; }

        /// <summary>
        /// Путь до герба РТН
        /// </summary>
        public string BlazonePath { get; set; }


        /// <summary>
        /// Ширина герба
        /// </summary>
        protected float BlazonWidth { get; set; }

        /// <summary>
        /// Высота герба
        /// </summary>
        protected float BlazonHeight { get; set; }

        /// <summary>
        /// Страница штампа
        /// </summary>
        public int? StampPage { get; set; }

        /// <summary>
        /// Герб
        /// </summary>
        public Image Blazon { get; set; }

        /// <summary>
        /// Отступ штампа по X
        /// </summary>
        public int? StampOffsetX { get; set; }

        /// <summary>
        /// Отступ штампа по Y
        /// </summary>
        public int? StampOffsetY { get; set; }

        /// <summary>
        /// Прямоугольник с маркером для подписи
        /// </summary>
        public Rectangle Mark { get; set; }

        /// <summary>
        /// Минимальное расстояние от нижнего края страницы до нижнего края штампа
        /// </summary>
        protected const float bottomStampLimit = 25 * BaseStampStrategy.oneMillimeter;

        /// <summary>
        /// Расстояние от нижнего края контекста до верхнего края штампа
        /// </summary>
        protected const float topStampDistance = 2.5f * BaseStampStrategy.oneMillimeter;

        public BaseStampStrategy(Rectangle mark, int? page, Image blazon = null)
        {
            this.Width = 78 * BaseStampStrategy.oneMillimeter;
            this.Height = 34 * BaseStampStrategy.oneMillimeter;
            this.Left = 0;
            this.Bottom = 0;
            this.Mark = mark;
            this.StampPage = page;
            this.Blazon = blazon;

            this.CentralRectWidth = this.Width - (2 * BaseStampStrategy.contourThickness) - BaseStampStrategy.defaultMargin;
            this.CentralRectHeight = BaseStampStrategy.defaultMargin;
            this.CentralRectVerticalOffset = 16f * BaseStampStrategy.oneMillimeter;
            this.СontourRoundRadius = 3f * BaseStampStrategy.oneMillimeter;
            this.SignatureColor = BaseColor.BLACK;
            this.CentralFontColor = BaseColor.WHITE;
            this.FontColor = BaseColor.BLACK;

            this.BlazonWidth = 10f * BaseStampStrategy.oneMillimeter;
            this.BlazonHeight = 10f * BaseStampStrategy.oneMillimeter;
        }

        /// <summary>
        /// Установить штамп
        /// </summary>
        public PdfSignatureAppearance Stamp(PdfReader reader, Stream os)
        {
            var stamper = PdfStamper.CreateSignature(reader, os, '\0', null, true);

            var baseFont = BaseFont.CreateFont("calibri.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, false, Resources.FontCalibri, null);
            var footerFont = BaseFont.CreateFont("arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, false, Resources.FontArial, null);

            var appearance = stamper.SignatureAppearance;
            appearance.Reason = this.Reason;

            this.Left = this.StampOffsetX ?? this.Left;
            this.Bottom = this.StampOffsetY ?? this.Bottom;

            this.BeforeDraw(appearance, reader);

            this.AddSignatureArea(appearance, reader);

            this.DrawSignature(appearance, baseFont, footerFont);

            IExternalSignatureContainer external = new ExternalBlankSignatureContainer(PdfName.ADOBE_PPKLITE, PdfName.ADBE_PKCS7_DETACHED);
            MakeSignature.SignExternalContainer(appearance, external, 65536);

            return appearance;
        }

        /// <summary>
        /// Действия перед отрисовкой печати
        /// </summary>
        protected virtual void BeforeDraw(PdfSignatureAppearance appearance, PdfReader reader)
        {
            if (Mark != null)
            {
                Left = Mark.Right - Width;
                Bottom = Mark.Top - Height;

                if (Bottom < bottomStampLimit)
                {
                    const float additionalSpace = 5 * oneMillimeter;
                    Bottom = bottomStampLimit + additionalSpace;
                }
            }
        }

        /// <summary>
        /// Нарисовать видимую подпись
        /// </summary>
        protected virtual void DrawSignature(PdfSignatureAppearance appearance, BaseFont baseFont, BaseFont footerFont)
        {
            var n0 = appearance.GetLayer(0);
            
            if (this.Blazon != null)
            {
                this.DrawBlazon(n0, new Rectangle(this.GetBlazonRectangle(n0)));
            }

            var headerLines = new[] {
                "ДОКУМЕНТ ПОДПИСАН",
                "ЭЛЕКТРОННОЙ ПОДПИСЬЮ"
            };
            var headerFont = new Font(baseFont, 10, Font.NORMAL, this.FontColor);

            this.DrawText(n0, this.GetHeaderRectangle(n0), headerFont, headerLines, headerFont.Size * 1.3f, Element.ALIGN_CENTER);

            this.DrawContour(n0);

            this.DrawCentralEDS(n0, new Rectangle(this.GetCentralRectangle(n0)), baseFont);

            var n2 = appearance.GetLayer(2);
            var detailsFont = new Font(footerFont, 6, Font.NORMAL, this.FontColor);
            var certificateLines = new[] {
                $"Сертификат:\u00A0{this.SerialNumber}",
                $"Владелец:\u00A0{this.PersonName}",
                $"Действителен:\u00A0c\u00A0{this.ValidFromDate:dd.MM.yyyy}\u00A0по\u00A0{this.ValidToDate:dd.MM.yyyy}"
            };

            this.DrawText(n2, this.GetFooterRectangle(n2), detailsFont, certificateLines, detailsFont.Size * 1.8f);
        }

        /// <summary>
        /// Добавить контейнер под подпись
        /// </summary>
        protected virtual void AddSignatureArea(PdfSignatureAppearance appearance, PdfReader reader)
        {
            var rectangle = new Rectangle(this.Left, this.Bottom, this.Left + this.Width, this.Bottom + this.Height);
            appearance.SetVisibleSignature(rectangle, this.StampPage ?? reader.NumberOfPages, this.ContextGuid);
        }

        /// <summary>
        /// Отрисовка центральной части
        /// </summary>
        protected void DrawCentralEDS(PdfTemplate layout, Rectangle rectangle, BaseFont baseFont)
        {
            layout.SetColorFill(this.SignatureColor);
            layout.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
            layout.Fill();

            var textVerticalOffset = 0.6f * BaseStampStrategy.oneMillimeter;
            var font = new Font(baseFont, 10, Font.NORMAL, this.CentralFontColor);
            var textRectangle = new Rectangle(rectangle.Left, rectangle.Bottom + textVerticalOffset, rectangle.Right, rectangle.Top + textVerticalOffset);
            this.DrawText(layout, textRectangle, font, "СВЕДЕНИЯ О СЕРТИФИКАТЕ ЭП", font.Size, Element.ALIGN_CENTER);
        }

        /// <summary>
        /// Отрисовка контура
        /// </summary>
        protected void DrawContour(PdfTemplate layout)
        {
            var x = layout.BoundingBox.Left;
            var y = layout.BoundingBox.Bottom;
            var width = layout.BoundingBox.Width;
            var height = layout.BoundingBox.Height;
            var halfThickness = BaseStampStrategy.contourThickness / 2;

            layout.SetColorStroke(this.SignatureColor);
            layout.SetLineWidth(BaseStampStrategy.contourThickness);
            layout.RoundRectangle(x + halfThickness, y + halfThickness, width - (2 * halfThickness), height - (2 * halfThickness), this.СontourRoundRadius);
            layout.Stroke();
        }

        /// <summary>
        /// Получить ограничивающий прямоугольник центра
        /// </summary>
        protected Rectangle GetCentralRectangle(PdfTemplate layout)
        {
            var x = layout.BoundingBox.Left;
            var y = layout.BoundingBox.Bottom;

            var left = x + BaseStampStrategy.contourThickness + BaseStampStrategy.defaultMargin;
            var bottom = y + BaseStampStrategy.contourThickness + this.CentralRectVerticalOffset;
            return new Rectangle(left, bottom, left + this.CentralRectWidth - BaseStampStrategy.defaultMargin, bottom + this.CentralRectHeight);
        }

        /// <summary>
        /// Отрисовка текста
        /// </summary>
        protected void DrawText(PdfTemplate layout, Rectangle rectangle, Font font, string text, float leading, int alignment = 0)
        {
            this.DrawText(layout, rectangle, font, new[] { text }, leading, alignment);
        }

        /// <summary>
        /// Отрисовка текста
        /// </summary>
        protected void DrawText(PdfTemplate layout, Rectangle rectangle, Font font, string[] lines, float leading, int alignment = 0)
        {
            var ct = new ColumnText(layout);
            ct.SetSimpleColumn(rectangle);
            foreach (var line in lines)
            {
                var p = new Paragraph(leading, line, font) { Alignment = alignment };
                ct.AddElement(p);
            }
            ct.Go();
        }

        /// <summary>
        /// Получить ограничивающий прямоугольник нижнего колонтитула
        /// </summary>
        protected Rectangle GetFooterRectangle(PdfTemplate layout)
        {
            var x = layout.BoundingBox.Left;
            var y = layout.BoundingBox.Bottom;
            var width = layout.BoundingBox.Width;

            var footerHeight = 10.5f * BaseStampStrategy.oneMillimeter;
            var left = x + BaseStampStrategy.contourThickness + BaseStampStrategy.defaultMargin;
            var bottom = y + BaseStampStrategy.contourThickness + BaseStampStrategy.defaultMargin;
            return new Rectangle(left, bottom, width - (BaseStampStrategy.contourThickness * 2) - BaseStampStrategy.defaultMargin, BaseStampStrategy.contourThickness + footerHeight + BaseStampStrategy.oneMillimeter + BaseStampStrategy.defaultMargin);
        }

        /// <summary>
        /// Получить ограничивающий прямоугольник для герба
        /// </summary>
        protected Rectangle GetBlazonRectangle(PdfTemplate layout)
        {
            var x = layout.BoundingBox.Left;
            var y = layout.BoundingBox.Bottom;
            var height = layout.BoundingBox.Height;

            var left = x + BaseStampStrategy.contourThickness + BaseStampStrategy.defaultMargin;
            var bottom = height - BaseStampStrategy.contourThickness - this.BlazonHeight - BaseStampStrategy.defaultMargin + 2 * BaseStampStrategy.oneMillimeter;
            return new Rectangle(left, bottom, left + this.BlazonWidth, bottom + this.BlazonHeight);
        }

        /// <summary>
        /// Отрисовка герба
        /// </summary>
        protected void DrawBlazon(PdfTemplate layout, Rectangle rectangle)
        {
            layout.AddImage(this.Blazon, rectangle.Width, 0, 0, rectangle.Height, rectangle.Left, rectangle.Bottom);
        }

        /// <summary>
        /// Получить ограничивающий прямоугольник верхней надписи
        /// </summary>
        protected Rectangle GetHeaderRectangle(PdfTemplate layout)
        {
            var x = layout.BoundingBox.Left;
            var y = layout.BoundingBox.Bottom;
            var width = layout.BoundingBox.Width;
            var height = layout.BoundingBox.Height;

            var left = x + BaseStampStrategy.contourThickness + this.BlazonWidth + BaseStampStrategy.defaultMargin;
            var bottom = height - BaseStampStrategy.contourThickness - this.BlazonHeight - BaseStampStrategy.defaultMargin + 2 * BaseStampStrategy.oneMillimeter;
            return new Rectangle(left, bottom, width - BaseStampStrategy.contourThickness - BaseStampStrategy.defaultMargin, bottom + this.BlazonHeight);
        }
    }*/
}