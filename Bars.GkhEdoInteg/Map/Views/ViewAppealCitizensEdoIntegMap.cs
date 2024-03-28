namespace Bars.GkhEdoInteg.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhEdoInteg.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhEdoInteg.Entities.ViewAppealCitizensEdoInteg"</summary>
    public class ViewAppealCitizensEdoIntegMap : PersistentObjectMap<ViewAppealCitizensEdoInteg>
    {
        
        public ViewAppealCitizensEdoIntegMap() : 
                base("Bars.GkhEdoInteg.Entities.ViewAppealCitizensEdoInteg", "VIEW_GJI_APPEAL_CITS_EDO")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExecuteDate, "ExecuteDate").Column("EXECUTE_DATE");
            this.Property(x => x.Municipality, "Municipality").Column("MUNICIPALITY");
            this.Property(x => x.MunicipalityId, "MunicipalityId").Column("MUNICIPALITY_ID");
            this.Property(x => x.Number, "Number").Column("DOCUMENT_NUMBER");
            this.Property(x => x.NumberGji, "NumberGji").Column("GJI_NUMBER");
            this.Property(x => x.DateFrom, "DateFrom").Column("DATE_FROM");
            this.Property(x => x.CheckTime, "CheckTime").Column("CHECK_TIME");
            this.Property(x => x.IsEdo, "IsEdo").Column("IS_EDO");
            this.Property(x => x.Correspondent, "Correspondent").Column("CORRESPONDENT");
            this.Property(x => x.RealObjAddresses, "RealObjAddresses").Column("RO_ADR");
            this.Property(x => x.AddressEdo, "AddressEdo").Column("ADDRESS_EDO");
            this.Property(x => x.CountSubject, "CountSubject").Column("COUNT_SUBJECT");
            this.Property(x => x.QuestionsCount, "QuestionsCount").Column("QUESTIONS_COUNT");
            this.Property(x => x.CountRealtyObj, "CountRealtyObj").Column("COUNT_RO");
            this.Reference(x => x.SuretyResolve, "SuretyResolve").Column("SURETY_RESOLVE_ID").Fetch();
            this.Reference(x => x.Executant, "Executant").Column("EXECUTANT_ID").Fetch();
            this.Reference(x => x.Tester, "Tester").Column("TESTER_ID").Fetch();
            this.Reference(x => x.ZonalInspection, "ZonalInspection").Column("ZONAINSP_ID").Fetch();
            this.Reference(x => x.State, "State").Column("STATE_ID").Fetch();
            this.Reference(x => x.AppealCits, "AppealCits").Column("ID").NotNull();
            this.Reference(x => x.Contragent, "Contragent").Column("CONTRAGENT_ID").NotNull();
        }
    }
}
