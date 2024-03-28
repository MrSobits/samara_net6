/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Suggestion
/// {
///     using Bars.Gkh.Entities.Suggestion;
/// 
///     public class CitizenSuggestionMap : BaseGkhEntityMap<CitizenSuggestion>
///     {
///         public CitizenSuggestionMap() : base("GKH_CIT_SUG")
///         {
///             Map(x => x.Number, "SUG_NUM");
///             Map(x => x.CreationDate, "CREATION_DATE");
///             References(x => x.RealityObject, "RO_ID").Fetch.Join();
///             References(x => x.Rubric, "RUBRIC_ID").Not.Nullable().Fetch.Join();
///             Map(x => x.ApplicantFio, "FIO");
///             Map(x => x.ApplicantAddress, "ADDRESS");
///             Map(x => x.ApplicantPhone, "PHONE");
///             Map(x => x.ApplicantEmail, "EMAIL");
///             Map(x => x.Description, "DESCRIPTION");
///             Map(x => x.HasAnswer, "HAS_ANSWER");
///             Map(x => x.AnswerText, "ANSWER_TEXT");
///             Map(x => x.AnswerDate, "ANSWER_DATE");
/// 
///             Map(x => x.Address, "SUGGESTION_ADDRESS");
///             Map(x => x.Year, "SUGGESTION_YEAR");
///             Map(x => x.Num, "SUGGESTION_NUMBER");
///             References(x => x.ExecutorManagingOrganization, "EXECUTOR_MANORG_ID").Fetch.Join();
///             References(x => x.ExecutorMunicipality, "EXECUTOR_MUNICIPALITY_ID").Fetch.Join();
///             References(x => x.ExecutorZonalInspection, "EXECUTOR_ZONAL_INSP_ID").Fetch.Join();
///             References(x => x.ExecutorCrFund, "EXECUTOR_CR_FUND_ID").Fetch.Join();
///             References(x => x.ProblemPlace, "PROBLEM_PLACE_ID").Fetch.Join();
///             References(x => x.State, "STATE_ID").Fetch.Join();
/// 
///             Map(x => x.Body, "BODY");
///             Map(x => x.Room, "ROOM");
///             Map(x => x.Apartment, "APARTMENT");
///             Map(x => x.MunicipalityCodeKladr, "MUNICIPALITY_CODE_KLADR");
///             Map(x => x.CityCodeKladr, "CITY_CODE_KLADR");
///             Map(x => x.StreetCodeKladr, "STREET_CODE_KLADR");
///             Map(x => x.House, "HOUSE");
///             Map(x => x.AddressFullCode, "ADDRESS_FULL_CODE");
///             Map(x => x.Deadline, "DEADLINE");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Suggestion
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Suggestion;
    
    
    /// <summary>Маппинг для "Обращения граждан"</summary>
    public class CitizenSuggestionMap : BaseImportableEntityMap<CitizenSuggestion>
    {
        
        public CitizenSuggestionMap() : 
                base("Обращения граждан", "GKH_CIT_SUG")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Number, "Number").Column("SUG_NUM");
            Property(x => x.CreationDate, "CreationDate").Column("CREATION_DATE");
            Property(x => x.ApplicantFio, "ApplicantFio").Column("FIO");
            Property(x => x.ApplicantAddress, "ApplicantAddress").Column("ADDRESS");
            Property(x => x.ApplicantPhone, "ApplicantPhone").Column("PHONE");
            Property(x => x.ApplicantEmail, "ApplicantEmail").Column("EMAIL");
            Property(x => x.Description, "Description").Column("DESCRIPTION");
            Property(x => x.HasAnswer, "HasAnswer").Column("HAS_ANSWER");
            Property(x => x.AnswerText, "AnswerText").Column("ANSWER_TEXT");
            Property(x => x.AnswerDate, "AnswerDate").Column("ANSWER_DATE");
            Property(x => x.Address, "Address").Column("SUGGESTION_ADDRESS");
            Property(x => x.Year, "Year").Column("SUGGESTION_YEAR");
            Property(x => x.Num, "Num").Column("SUGGESTION_NUMBER");
            Property(x => x.Body, "Body").Column("BODY");
            Property(x => x.Room, "Room").Column("ROOM");
            Property(x => x.Apartment, "Apartment").Column("APARTMENT");
            Property(x => x.MunicipalityCodeKladr, "MunicipalityCodeKladr").Column("MUNICIPALITY_CODE_KLADR");
            Property(x => x.CityCodeKladr, "CityCodeKladr").Column("CITY_CODE_KLADR");
            Property(x => x.StreetCodeKladr, "StreetCodeKladr").Column("STREET_CODE_KLADR");
            Property(x => x.House, "House").Column("HOUSE");
            Property(x => x.AddressFullCode, "AddressFullCode").Column("ADDRESS_FULL_CODE");
            Property(x => x.Deadline, "Deadline").Column("DEADLINE");
            Property(x => x.TestSuggestion, "TestSuggestion").Column("TEST_SUGG");
            Reference(x => x.RealityObject, "RealityObject").Column("RO_ID").Fetch();
            Reference(x => x.Rubric, "Rubric").Column("RUBRIC_ID").NotNull().Fetch();
            Reference(x => x.ExecutorManagingOrganization, "ExecutorManagingOrganization").Column("EXECUTOR_MANORG_ID").Fetch();
            Reference(x => x.ExecutorMunicipality, "ExecutorMunicipality").Column("EXECUTOR_MUNICIPALITY_ID").Fetch();
            Reference(x => x.ExecutorZonalInspection, "ExecutorZonalInspection").Column("EXECUTOR_ZONAL_INSP_ID").Fetch();
            Reference(x => x.ExecutorCrFund, "ExecutorCrFund").Column("EXECUTOR_CR_FUND_ID").Fetch();
            Reference(x => x.ProblemPlace, "ProblemPlace").Column("PROBLEM_PLACE_ID").Fetch();
            Reference(x => x.SugTypeProblem, "SugTypeProblem").Column("TYPE_PROBLEM").Fetch();
            Reference(x => x.State, "State").Column("STATE_ID").Fetch();
            Reference(x => x.MessageSubject, "MessageSubject").Column("MESSAGE_SUBJECT_ID").Fetch();
            Reference(x => x.Flat, "ROOM").Column("ROOM_ID").Fetch();

            Property(x => x.GisGkhGuid, "ГИС ЖКХ GUID").Column("GIS_GKH_GUID").Length(36);
            Property(x => x.GisGkhTransportGuid, "ГИС ЖКХ Transport GUID").Column("GIS_GKH_TRANSPORT_GUID").Length(36);
            Property(x => x.GisGkhAnswerGuid, "ГИС ЖКХ Answer GUID").Column("GIS_GKH_ANSWER_GUID").Length(36);
            Property(x => x.GisGkhAnswerTransportGuid, "ГИС ЖКХ Answer Transport GUID").Column("GIS_GKH_ANSWER_TRANSPORT_GUID").Length(36);
            Property(x => x.GisGkhParentGuid, "ГИС ЖКХ GUID родительского обращения").Column("GIS_GKH_PARENT_GUID").Length(36);
            Property(x => x.GisGkhContragentGuid, "ГИС ЖКХ GUID контрагента, от которого обращение").Column("GIS_GKH_CONTRAGENT_GUID").Length(36);
            Property(x => x.GisWork, "Признак работы с обращением в ГИС ЖКХ").Column("GIS_WORK");
            Reference(x => x.ContragentCorrespondent, "Контрагент как корреспондент в обращении").Column("CORRESPONDENT_CONTRAGENT_ID").Fetch();
        }
    }
}
