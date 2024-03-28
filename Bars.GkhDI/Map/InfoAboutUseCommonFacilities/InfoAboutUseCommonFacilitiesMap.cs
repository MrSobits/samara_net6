namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.InfoAboutUseCommonFacilities"</summary>
    public class InfoAboutUseCommonFacilitiesMap : BaseImportableEntityMap<InfoAboutUseCommonFacilities>
    {
        
        public InfoAboutUseCommonFacilitiesMap() : 
                base("Bars.GkhDi.Entities.InfoAboutUseCommonFacilities", "DI_DISINFO_COM_FACILS")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.KindCommomFacilities, "KindCommomFacilities").Column("KIND_COMMON_FACILITIES").Length(300);
            this.Property(x => x.Number, "Number").Column("NUM").Length(300);
            this.Property(x => x.From, "From").Column("DATE_FROM");
            this.Property(x => x.LesseeType, "LesseeType").Column("LESSEE_TYPE").NotNull();
            this.Property(x => x.Surname, "Surname").Column("SURNAME").Length(300);
            this.Property(x => x.Name, "Name").Column("NAME").Length(300);
            this.Property(x => x.Patronymic, "Patronymic").Column("PATRONYMIC").Length(300);
            this.Property(x => x.Gender, "Gender").Column("GENDER").NotNull();
            this.Property(x => x.BirthDate, "BirthDate").Column("BIRTH_DATE");
            this.Property(x => x.BirthPlace, "BirthPlace").Column("BIRTH_PLACE").Length(300);
            this.Property(x => x.Snils, "Snils").Column("SNILS").Length(50);
            this.Property(x => x.Ogrn, "OGRN").Column("OGRN").Length(50);
            this.Property(x => x.Inn, "INN").Column("INN").Length(50);
            this.Property(x => x.Lessee, "Lessee").Column("LESSEE").Length(300);
            this.Property(x => x.DateStart, "DateStart").Column("DATE_START");
            this.Property(x => x.DateEnd, "DateEnd").Column("DATE_END");
            this.Property(x => x.TypeContract, "TypeContract").Column("TYPE_CONTRACT").NotNull();
            this.Property(x => x.CostContract, "CostContract").Column("COST_CONTRACT");
            this.Property(x => x.AppointmentCommonFacilities, "AppointmentCommonFacilities").Column("APPOINTMENT_COMMON_FACILITIES");
            this.Property(x => x.AreaOfCommonFacilities, "AreaOfCommonFacilities").Column("AREA_OF_COMMON_FACILITIES");
            this.Property(x => x.ContractNumber, "ContractNumber").Column("CONTRACT_NUMBER");
            this.Property(x => x.ContractSubject, "ContractSubject").Column("CONTRACT_SUBJECT").Length(300);
            this.Property(x => x.ContractDate, "ContractDate").Column("CONTRACT_DATE");
            this.Property(x => x.CostByContractInMonth, "CostByContractInMonth").Column("COST_BY_CONTRACT_IN_MONTH");
            this.Property(x => x.Comment, "Comment").Column("FACILS_COMMENT").Length(300);
            this.Property(x => x.SigningContractDate, "SigningContractDate").Column("SIGNING_CONTRACT_DATE");
            this.Property(x => x.DayMonthPeriodIn, "DayMonthPeriodIn").Column("DAY_MONTH_PERIOD_IN");
            this.Property(x => x.DayMonthPeriodOut, "DayMonthPeriodOut").Column("DAY_MONTH_PERIOD_OUT");
            this.Property(x => x.IsLastDayMonthPeriodIn, "IsLastDayMonthPeriodIn").Column("IS_LAST_DAY_MONTH_PERIOD_IN");
            this.Property(x => x.IsLastDayMonthPeriodOut, "IsLastDayMonthPeriodOut").Column("IS_LAST_DAY_MONTH_PERIOD_OUT");
            this.Property(x => x.IsNextMonthPeriodIn, "IsNextMonthPeriodIn").Column("IS_NEXT_MONTH_PERIOD_IN");
            this.Property(x => x.IsNextMonthPeriodOut, "IsNextMonthPeriodOut").Column("IS_NEXT_MONTH_PERIOD_OUT");
            this.Reference(x => x.DisclosureInfoRealityObj, "DisclosureInfoRealityObj").Column("DISINFO_RO_ID").NotNull().Fetch();
            this.Reference(x => x.ProtocolFile, "ProtocolFile").Column("PROTOCOL_FILE_ID").Fetch();
            this.Reference(x => x.ContractFile, "ContractFile").Column("CONTRACT_FILE_ID").Fetch();
        }
    }
}
