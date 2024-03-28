namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;


    /// <summary>Маппинг для "Предложение капитального ремонта"</summary>
    public class OverhaulProposalMap : BaseEntityMap<OverhaulProposal>
    {

        public OverhaulProposalMap() :
                base("Предложение капитального ремонта", "OVRHL_PROPOSE")
        {
        }

        protected override void Map()
        {
            Property(x => x.ProgramNum, "Номер по программе").Column("PROGRAM_NUM").Length(300);
            Property(x => x.DateEndBuilder, "Дата завершения работ подрядчиком").Column("DATE_END_BUILDER");
            Property(x => x.DateStartWork, "Дата начала работ").Column("DATE_START_WORK");
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);
            Reference(x => x.ProgramCr, "Программа").Column("PROGRAM_ID").Fetch();
            Reference(x => x.ObjectCr, "ObjectCr").Column("OBJECT_CR_ID").NotNull().Fetch();
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
