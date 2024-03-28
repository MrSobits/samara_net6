namespace Bars.B4.Modules.Analytics.Reports.Enums;

/// <summary>Modes for formats the report to be exported to.</summary>
/// <remarks>Взято из пакета Stimulsoft.Report версии 2021.2.2.0</remarks>
public enum StiExportFormat
{
    /// <summary>Export will not be done.</summary>
    None = 0,

    /// <summary>Adobe PDF format for export.</summary>
    Pdf = 1,

    /// <summary>Microsoft Xps format for export.</summary>
    Xps = 2,

    /// <summary>HTML Table format for export.</summary>
    HtmlTable = 3,

    /// <summary>HTML Span format for export.</summary>
    HtmlSpan = 4,

    /// <summary>HTML Div format for export.</summary>
    HtmlDiv = 5,

    /// <summary>RTF format for export.</summary>
    Rtf = 6,

    /// <summary>Table in Rtf format for export.</summary>
    RtfTable = 7,

    /// <summary>
    /// Components of the report will be placed into RTF frames for export.
    /// </summary>
    RtfFrame = 8,

    /// <summary>
    /// Components of the report will be placed into RTF frames with borders in Microsoft Word graphic format for export.
    /// </summary>
    RtfWinWord = 9,

    /// <summary>
    /// Mode for export to the RTF format with Tab symbol as delimiter of the text.
    /// </summary>
    RtfTabbedText = 10,

    /// <summary>Text format for export.</summary>
    Text = 11,

    /// <summary>
    /// Excel BIFF (Binary Interchange File Format) format for export.
    /// </summary>
    Excel = 12,

    /// <summary>Excel Xml format for export.</summary>
    ExcelXml = 13,

    /// <summary>Excel 2007 format for export.</summary>
    Excel2007 = 14,

    /// <summary>Word 2007 format for export.</summary>
    Word2007 = 15,

    /// <summary>Xml format for export.</summary>
    Xml = 16,

    /// <summary>CSV (Comma Separated Value) file format for export.</summary>
    Csv = 17,

    /// <summary>Dif file format for export.</summary>
    Dif = 18,

    /// <summary>Sylk file format for export.</summary>
    Sylk = 19,

    /// <summary>Image format for export.</summary>
    Image = 20,

    /// <summary>Image in GIF format for export.</summary>
    ImageGif = 21,

    /// <summary>Image in BMP format for export.</summary>
    ImageBmp = 22,

    /// <summary>Image in PNG format for export.</summary>
    ImagePng = 23,

    /// <summary>Image in TIFF format for export.</summary>
    ImageTiff = 24,

    /// <summary>Image in JPEG format for export.</summary>
    ImageJpeg = 25,

    /// <summary>Image in PCX format for export.</summary>
    ImagePcx = 26,

    /// <summary>Image in EMF format for export.</summary>
    ImageEmf = 27,

    /// <summary>Image in SVG format for export.</summary>
    ImageSvg = 28,

    /// <summary>Image in SVGZ format for export.</summary>
    ImageSvgz = 29,

    /// <summary>WebArchive format for export.</summary>
    Mht = 30,

    /// <summary>dBase/FoxPro format for export.</summary>
    Dbf = 31,

    /// <summary>HTML format for export.</summary>
    Html = 32,

    /// <summary>OpenDocument Calc file</summary>
    Ods = 33,

    /// <summary>OpenDocument Writer file</summary>
    Odt = 34,

    /// <summary>PowerPoint 2007 format for export</summary>
    Ppt2007 = 35,

    /// <summary>HTML5 format for export.</summary>
    Html5 = 36,

    /// <summary>Universal format for all data type of exports.</summary>
    Data = 37,

    /// <summary>JSON format for export.</summary>
    Json = 38,

    /// <summary>Document MDC file.</summary>
    Document = 1000,
}