namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;

    public class TerminationReasonMap : BaseImportableEntityMap<TerminationReason>
    {
        /// <inheritdoc />
        public TerminationReasonMap()
            : base("CR_DICT_TERMINATION_REASON")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME").NotNull();
            this.Property(x => x.Code, "Код").Column("CODE").NotNull();
        }
    }
}
