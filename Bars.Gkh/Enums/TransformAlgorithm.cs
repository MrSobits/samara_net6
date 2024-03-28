namespace Bars.Gkh.Enums
{
    /// <summary>
    /// Алгоритмы преобразования
    /// </summary>
    public enum TransformAlgorithm
    {
        /// <summary>
        /// Порядок шифрования и подписания
        /// </summary>
        Decryption = 0,

        /// <summary>
        /// Преобразование из base64 в соответствии с определением раздела 6.6.2 спецификации XMLDSIG
        /// </summary>
        DsigBase64 = 1,

        /// <summary>
        /// XML-канонизация C14N
        /// </summary>
        DsigC14N = 2,

        /// <summary>
        /// XML-канонизация C14N с комментариями
        /// </summary>
        DsigC14NWithComments = 3,

        /// <summary>
        /// Формат подписи
        /// </summary>
        DsigEnvelopedSignature = 4,

        /// <summary>
        /// Исключающая XML-канонизация C14N
        /// </summary>
        DsigExcC14N = 5,

        /// <summary>
        /// Исключающая XML-канонизация C14N с комментариями
        /// </summary>
        DsigExcC14NWithComments = 6,

        /// <summary>
        /// XPAth
        /// </summary>
        DsigXPath = 7,

        /// <summary>
        /// XSLT-преобразование
        /// </summary>
        DsigXslt = 8,

        /// <summary>
        /// Преобразование лицензий
        /// </summary>
        License = 9,

        /// <summary>
        /// СМЭВ
        /// <para>http://forum.minsvyaz.ru/assets/files/app_5.docx</para>
        /// </summary>
        DsigSmev = 10
    }
}