namespace Bars.GkhGji.Regions.Tatarstan.Map.TatarstanProtocolGji
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;

    public class TatarstanDocumentWitnessMap : BaseEntityMap<TatarstanDocumentWitness>
    {
        /// <inheritdoc />
        public TatarstanDocumentWitnessMap()
            : base(typeof(TatarstanDocumentWitness).FullName, "GJI_DOCUMENT_WITNESS") 
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.DocumentGji, "DocumentGji").Column("DOCUMENT_ID").Fetch();
            this.Property(x => x.WitnessType, "WitnessType").Column("WITNESS_TYPE");
            this.Property(x => x.Fio, "Fio").Column("FIO");
            this.Property(x => x.FactAddress, "FactAddress").Column("FACT_ADDRESS");
            this.Property(x => x.Phone, "Phone").Column("PHONE");
        }
    }
}
