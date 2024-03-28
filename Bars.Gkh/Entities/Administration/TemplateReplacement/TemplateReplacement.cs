namespace Bars.Gkh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Замена шаблона
    /// </summary>
    public class TemplateReplacement : BaseEntity
    {
        /// <summary>
        /// Код шаблона
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Файл шаблона
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}