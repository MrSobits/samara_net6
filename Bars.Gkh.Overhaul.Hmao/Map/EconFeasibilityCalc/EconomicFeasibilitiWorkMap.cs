namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;


    /// <summary>
    /// Маппинг для "Контрагент"
    /// </summary>
    public class EconomicFeasibilitiWorkMap : BaseEntityMap<EconFeasibilitiWork>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public EconomicFeasibilitiWorkMap() :
                base("Работы, на основании которых выполнен рассчет", "OVRHL_ECON_FEASIBILITY_WORK")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.ResultId, "Запись рассчета").Column("RECORD_ID").NotNull().Fetch();
            this.Reference(x => x.RecorWorkdId, "Запись работы ы ДПКР").Column("REC_WORK_ID").NotNull().Fetch();            
        }
    }
}
