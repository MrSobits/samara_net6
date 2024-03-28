namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;

    /// <summary>
    /// Сущность, содержащая данные, необходимые при учете корректировки ДПКР
    /// Лимит займа, Дефицит ...
    /// </summary>
    public class DpkrCorrectionStage2 : BaseEntity
    {
        /// <summary>
        /// Объект недвижимости
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Скорректированный год
        /// </summary>
        public virtual int PlanYear { get; set; }

        /// <summary>
        /// Тип результата корректировки
        /// </summary>
        public virtual TypeResultCorrectionDpkr TypeResult { get; set; }

        /// <summary>
        /// Этап 2 формирования ДПКР
        /// </summary>
        public virtual VersionRecordStage2 Stage2 { get; set; }
   }
}