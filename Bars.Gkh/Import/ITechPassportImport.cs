namespace Bars.Gkh.Import
{
    using Bars.B4;

    public interface ITechPassportImport
    {
        /// <param name="files">
        /// Файл (zip-архив)
        /// </param>
        /// <param name="logImport">
        /// Лог
        /// </param>
        /// <param name="replaceData">
        /// Заменять текущие данные
        /// </param>
        void Import(FileData zipFile, ILogImport logImport, bool replaceData);
    }
}