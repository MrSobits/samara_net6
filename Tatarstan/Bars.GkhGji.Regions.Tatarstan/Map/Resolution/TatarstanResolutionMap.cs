namespace Bars.GkhGji.Regions.Tatarstan.Map.Resolution
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Resolution;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanResolution"</summary>
    public class TatarstanResolutionMap : JoinedSubClassMap<TatarstanResolution>
    {
        public TatarstanResolutionMap() :
            base("Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanResolution", "GJI_TATARSTAN_RESOLUTION")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.PatternDict, "PatternDict").Column("PATTERN_DICT_ID");
            this.Property(x => x.SurName, "SurName").Column("SUR_NAME").Length(255);
            this.Property(x => x.Name, "Name").Column("FIRST_NAME").Length(255);
            this.Property(x => x.Patronymic, "Patronymic").Column("PATRONYMIC");
            this.Property(x => x.BirthDate, "BirthDate").Column("BIRTH_DATE").Length(255);
            this.Property(x => x.BirthPlace, "BirthPlace").Column("BIRTH_PLACE").Length(255);
            this.Property(x => x.Address, "Address").Column("FACT_ADDRESS").Length(255);
            this.Reference(x => x.Citizenship, "Citizenship").Column("GJI_DICT_CITIZENSHIP_ID");
            this.Property(x => x.CitizenshipType, "CitizenshipType").Column("CITIZENSHIP_TYPE");
            this.Property(x => x.SerialAndNumber, "SerialAndNumber").Column("SERIAL_AND_NUMBER").Length(255);
            this.Property(x => x.IssueDate, "IssueDate").Column("ISSUE_DATE").Length(255);
            this.Property(x => x.IssuingAuthority, "IssuingAuthority").Column("ISSUING_AUTHORITY").Length(255);
            this.Property(x => x.Company, "Company").Column("COMPANY").Length(255);
            this.Property(x => x.ChangeReason, "ChangeReason").Column("CHANGE_REASON").Length(255);
            this.Property(x => x.Snils, "СНИЛС").Column("SNILS");
            this.Property(x => x.SanctionsDuration, "Срок накладываемых санкций").Column("SANCTIONS_DURATION").Length(512);
        }
    }
}