namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    
    /// <summary>
    /// Вложения ответа
    /// </summary>
    public class AppealCitsAnswerAttachment: BaseEntity
    {
        /// <summary>
        /// Ответ на обращение
        /// </summary>
        public virtual AppealCitsAnswer AppealCitsAnswer { get; set; }

        /// <summary>
        /// Описание файла
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Путь к файлу на ftp
        /// </summary>
        /// <remarks>
        /// Используется в Челябинске
        /// Результат наплевательского проектирования
        /// </remarks>
        public virtual string UniqueName { get; set; }
    }
}