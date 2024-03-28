namespace Bars.GkhGji.Entities
{
    using Gkh.Entities;

    /// <summary>
    /// Обращение граждан проверки по обращениям граджан
    /// Это просто таблица связи обращения и Проверки по обращению
    /// Непутать с InspectionGji
    /// </summary>
    public class InspectionAppealCits : BaseGkhEntity
    {
        /// <summary>
        /// Основание обращение граждан ГЖИ
        /// </summary>
        public virtual InspectionGji Inspection { get; set; }

        /// <summary>
        /// обращение граждан
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }
    }
}