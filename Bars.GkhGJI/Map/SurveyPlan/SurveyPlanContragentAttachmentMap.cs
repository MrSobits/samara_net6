/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map.SurveyPlan
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Entities.SurveyPlan;
/// 
///     public class SurveyPlanContragentAttachmentMap : BaseEntityMap<SurveyPlanContragentAttachment>
///     {
///         public SurveyPlanContragentAttachmentMap()
///             : base("GJI_SURV_PLAN_CONTR_ATT")
///         {
///             Map(x => x.Name, "NAME", true, 250);
///             Map(x => x.Num, "NUM", true, 250);
///             Map(x => x.Date, "DOC_DATE", true);
///             Map(x => x.Description, "DESCRIPTION", false, 2000);
/// 
///             References(x => x.File, "FILE_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.SurveyPlanContragent, "SURV_PLAN_CONTR_ID", ReferenceMapConfig.NotNull);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map.SurveyPlan
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.SurveyPlan;
    
    
    /// <summary>Маппинг для "Приложение контрагента плана проверки"</summary>
    public class SurveyPlanContragentAttachmentMap : BaseEntityMap<SurveyPlanContragentAttachment>
    {
        
        public SurveyPlanContragentAttachmentMap() : 
                base("Приложение контрагента плана проверки", "GJI_SURV_PLAN_CONTR_ATT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Date, "Дата документа").Column("DOC_DATE").NotNull();
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            Reference(x => x.File, "Файл").Column("FILE_ID").NotNull().Fetch();
            Property(x => x.Name, "Наименование").Column("NAME").Length(250).NotNull();
            Property(x => x.Num, "Номер документа").Column("NUM").Length(250).NotNull();
            Reference(x => x.SurveyPlanContragent, "Контрагент плана проверки").Column("SURV_PLAN_CONTR_ID").NotNull();
        }
    }
}
