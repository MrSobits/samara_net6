using Bars.Gkh.Entities;

namespace Bars.Gkh.Overhaul.Hmao.Entities
{
   /// <summary>
   /// Соответствие записирезультата и работ, но основании которых делался рассчет
   /// </summary>
    public class EconFeasibilitiWork : BaseGkhEntity
    {
        /// <summary>
        /// Запись результата
        /// </summary>
        public virtual EconFeasibilityCalcResult ResultId { get; set; }
        /// <summary>
        /// Запись работы в ДПКР
        /// </summary>
        public virtual VersionRecord RecorWorkdId { get; set; }
    }
}
