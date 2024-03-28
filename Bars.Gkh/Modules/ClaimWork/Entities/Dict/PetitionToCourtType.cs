namespace Bars.Gkh.Modules.ClaimWork.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Справочник заявлений в суд
    /// </summary>
    public class PetitionToCourtType : BaseEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Короткое наименование
        /// </summary>
        public virtual string ShortName { get; set; }

        /// <summary>
        /// Полное наименование
        /// </summary>
        public virtual string FullName { get; set; }
    }
}