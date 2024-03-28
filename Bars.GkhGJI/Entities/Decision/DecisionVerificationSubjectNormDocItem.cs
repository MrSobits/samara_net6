namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Требования НПА проверки
    /// </summary>
    public class DecisionVerificationSubjectNormDocItem : BaseEntity
    {
        /// <summary>
        /// НПА проверки приказа ГЖИ 
        /// </summary>
        public virtual DecisionVerificationSubject DecisionVerificationSubject { get; set; }

        /// <summary>
        /// Пункт нормативного документа
        /// </summary>
        public virtual NormativeDocItem NormativeDocItem { get; set; }

    }
}