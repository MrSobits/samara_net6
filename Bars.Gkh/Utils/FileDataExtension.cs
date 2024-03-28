namespace Bars.Gkh.Utils
{
    using Bars.B4;

    /// <summary>
    /// Расширение <see cref="FileData"/>
    /// </summary>
    public static class FileDataExtension
    {
        /// <summary>
        /// Получить название файла и его расширение
        /// </summary>
        public static string GetFullName(this FileData fileData )
        {
            return fileData == null ? string.Empty : $"{fileData.FileName}.{fileData.Extention}";
        }
    }
}