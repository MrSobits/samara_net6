namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Адресат ответа на обращение
    /// </summary>
    public class AppealCitsAnswerAddressee : BaseEntity
    {
        /// <summary>
        /// Ответ
        /// </summary>
        public virtual AppealCitsAnswer Answer { get; set; }

        /// <summary>
        /// Адресат
        /// </summary>
        public virtual RevenueSourceGji Addressee { get; set; }

    }
}