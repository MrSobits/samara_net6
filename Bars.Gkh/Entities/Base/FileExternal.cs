namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Таблица для конвертации связь страого external_id  с файлом в новой
    /// </summary>
    public class FileExternal : BaseGkhEntity
    {
        /// <summary>
        /// Id файла в новой 
        /// </summary>
        public virtual long FileInfoId { get; set; }
    }
}
