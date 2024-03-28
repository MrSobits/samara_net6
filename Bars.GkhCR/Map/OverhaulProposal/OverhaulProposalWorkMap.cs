namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    using Bars.B4.Modules.Mapping.Mappers;
    /// <summary>Маппинг для "Вид работы предложения"</summary>
    public class OverhaulProposalWorkMap : BaseEntityMap<OverhaulProposalWork>
    {

        public OverhaulProposalWorkMap()
            : base("Вид работы предложения", "OVRHL_PROPOSE_TYPE_WORK")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.OverhaulProposal, "Объект капитального ремонта").Column("PROPOSAL_ID").NotNull().Fetch();          
            this.Reference(x => x.Work, "Вид работы").Column("WORK_ID").Fetch();          
            this.Property(x => x.Volume, "Объем (плановый)").Column("VOLUME");           
            this.Property(x => x.Sum, "Сумма (плановая)").Column("SUM");
            this.Property(x => x.ByAreaMkd, "Расчет по площади").Column("BY_AREA");
            this.Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);
            this.Property(x => x.DateStartWork, "Дата начала работ").Column("DATE_START_WORK");
            this.Property(x => x.DateEndWork, "Дата окончания работ").Column("DATE_END_WORK");         
          
        }
    }
}