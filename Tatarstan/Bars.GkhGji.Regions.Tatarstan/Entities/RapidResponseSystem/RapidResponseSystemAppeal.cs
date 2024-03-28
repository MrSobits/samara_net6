namespace Bars.GkhGji.Regions.Tatarstan.Entities.RapidResponseSystem
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Обращение в СОПР
    /// </summary>
    public class RapidResponseSystemAppeal : BaseEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }
        
        /// <summary>
        /// Обращение гражданина
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }
    }
}