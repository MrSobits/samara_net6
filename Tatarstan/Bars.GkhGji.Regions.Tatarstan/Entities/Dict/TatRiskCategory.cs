using Bars.Gkh.Entities.Dicts;

namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict
{
    /// <summary>
    /// Расширение справочника "Категории риска" для региона Татарстан
    /// </summary>
    public class TatRiskCategory : RiskCategory
    {
        /// <summary>
        /// Идентификатор в ЕРВК
        /// </summary>
        public virtual string ErvkGuid { get; set; }
    }
}
