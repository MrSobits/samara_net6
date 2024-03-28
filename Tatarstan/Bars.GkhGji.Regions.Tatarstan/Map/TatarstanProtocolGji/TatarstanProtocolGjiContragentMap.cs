namespace Bars.GkhGji.Regions.Tatarstan.Map.TatarstanProtocolGji
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;

    public class TatarstanProtocolGjiContragentMap : JoinedSubClassMap<TatarstanProtocolGjiContragent>
    {
        /// <inheritdoc />
        public TatarstanProtocolGjiContragentMap()
            : base(typeof(TatarstanProtocolGjiContragent).FullName, "GJI_TATARSTAN_PROTOCOL_GJI_CONTRAGENT")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.Contragent, "Contragent").Column("CONTRAGENT_ID").Fetch();
            this.Property(x => x.DelegateFio, "DelegateFio").Column("DELEGATE_FIO");
            this.Property(x => x.DelegateCompany, "DelegateCompany").Column("DELEGATE_COMPANY");
            this.Property(x => x.ProcurationNumber, "ProcurationNumber").Column("PROCURATION_NUMBER");
            this.Property(x => x.ProcurationDate, "ProcurationDate").Column("PROCURATION_DATE");
            this.Property(x => x.DelegateResponsibilityPunishment, "DelegateResponsibilityPunishment").Column("DELEGATE_RESPONSIBILITY_PUNISHMENT");
        }
    }
}
