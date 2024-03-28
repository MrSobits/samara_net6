namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Привязка документа ДПКР к версии программы
    /// </summary>
    public class DpkrDocumentProgramVersion : BaseEntity
    {
        /// <summary>
        /// Документ ДПКР
        /// </summary>
        public virtual DpkrDocument DpkrDocument { get; set; }

        /// <summary>
        /// Версия программы
        /// </summary>
        public virtual ProgramVersion ProgramVersion { get; set; }
    }
}