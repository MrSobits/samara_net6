namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Расторжение договора подряда КР"</summary>
    public class BuildContractTerminationMap : BaseEntityMap<BuildContractTermination>
    {

        public BuildContractTerminationMap() :
                base("Расторжение договора подряда КР", "CR_OBJ_BUILD_CONTRACT_TERMINATION")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.BuildContract, "Договор подряда КР").Column("BUILD_CONTRACT_ID").Fetch();
            this.Property(x => x.TerminationDate, "Дата расторжения").Column("TERMINATION_DATE");
            this.Reference(x => x.DocumentFile, "Документ-основание").Column("DOCUMENT_FILE_ID");
            this.Property(x => x.Reason, "Основание расторжения").Column("REASON");
            this.Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER");
            this.Reference(x => x.DictReason, "Причина расторжения").Column("REASON_ID");
        }
    }
}
