namespace Bars.GkhGji.Regions.Tatarstan.Map.Protocol
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Protocol;

    public class TatProtocolMap : JoinedSubClassMap<TatProtocol>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public TatProtocolMap() :
                base(nameof(TatProtocol), "GJI_TAT_PROTOCOL")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.DocumentPlace, nameof(TatProtocol.DocumentPlace)).Column("DOCUMENT_PLACE_ID");
            this.Property(x => x.DateWriteOut, nameof(TatProtocol.DateWriteOut)).Column("DATE_WRITE_OUT");
            this.Property(x => x.Surname, nameof(TatProtocol.Surname)).Column("SURNAME");
            this.Property(x => x.Name, nameof(TatProtocol.Name)).Column("NAME");
            this.Property(x => x.Patronymic, nameof(TatProtocol.Patronymic)).Column("PATRONYMIC");
            this.Property(x => x.BirthDate, nameof(TatProtocol.BirthDate)).Column("BIRTH_DATE");
            this.Property(x => x.BirthPlace, nameof(TatProtocol.BirthPlace)).Column("BIRTH_PLACE");
            this.Property(x => x.FactAddress, nameof(TatProtocol.FactAddress)).Column("FACT_ADDRESS");
            this.Reference(x => x.Citizenship, nameof(TatProtocol.Citizenship)).Column("GJI_DICT_CITIZENSHIP_ID");
            this.Property(x => x.CitizenshipType, nameof(TatProtocol.CitizenshipType)).Column("CITIZENSHIP_TYPE");
            this.Property(x => x.SerialAndNumber, nameof(TatProtocol.SerialAndNumber)).Column("SERIAL_AND_NUMBER");
            this.Property(x => x.IssueDate, nameof(TatProtocol.IssueDate)).Column("ISSUE_DATE");
            this.Property(x => x.IssuingAuthority, nameof(TatProtocol.IssuingAuthority)).Column("ISSUING_AUTHORITY");
            this.Property(x => x.Company, nameof(TatProtocol.Company)).Column("COMPANY");
            this.Property(x => x.Snils, nameof(TatProtocol.Snils)).Column("SNILS");
        }
    }
}
