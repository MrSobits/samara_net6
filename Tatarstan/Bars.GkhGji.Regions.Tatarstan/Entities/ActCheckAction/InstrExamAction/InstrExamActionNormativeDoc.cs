namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.InstrExamAction
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Нормативно-правовой акт действия "Инструментальное обследование"
    /// </summary>
    public class InstrExamActionNormativeDoc : BaseEntity
    {
        /// <summary>
        /// Действие "Инструментальное обследование"
        /// </summary>
        public virtual InstrExamAction InstrExamAction { get; set; }

        /// <summary>
        /// Нормативно-правовой акт
        /// </summary>
        public virtual NormativeDoc NormativeDoc { get; set; }
    }
}