/// <mapping-converter-backup>
/// namespace Bars.B4.Modules.Analytics.Reports.Maps
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.B4.Modules.Analytics.Reports.Entities;
/// 
///     public class ReportParamMap : BaseEntityMap<ReportParam>
///     {
///         public ReportParamMap()
///             : base("AL_REPORT_PARAM")
///         {
///             Map(x => x.ParamType, "PARAM_TYPE");
///             Map(x => x.OwnerType, "OWNER_TYPE");
///             Map(x => x.Required, "REQUIRED");
///             Map(x => x.Multiselect, "MULTISELECT");
///             Map(x => x.Label, "LABEL_TEXT");
///             Map(x => x.Additional, "ADDITIONAL");
///             Map(x => x.Name, "NAME");
///             Map(x => x.SqlQuery, "SQL_QUERY");
///             Property("DefaultValueBytes", mapper => mapper.Column("DEFAULT_VAL_BYTES"));
///             References(x => x.StoredReport, "REPORT_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.B4.Modules.Analytics.Reports.Map
{
    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.B4.Modules.Analytics.Reports.Entities.ReportParam"</summary>
    public class ReportParamGkhMap : BaseEntityMap<ReportParamGkh>
    {
        
        public ReportParamGkhMap() : 
                base("Bars.B4.Modules.Analytics.Reports.Entities.ReportParamGkh", "AL_REPORT_PARAM")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.StoredReport, "StoredReport").Column("REPORT_ID");
            Property(x => x.OwnerType, "OwnerType").Column("OWNER_TYPE");
            Property(x => x.ParamType, "ParamType").Column("PARAM_TYPE");
            Property(x => x.Required, "Required").Column("REQUIRED");
            Property(x => x.Multiselect, "Multiselect").Column("MULTISELECT");
            Property(x => x.Label, "Label").Column("LABEL_TEXT").Length(250);
            Property(x => x.Name, "Name").Column("NAME").Length(250);
            Property(x => x.SqlQuery, "SqlQuery").Column("SQL_QUERY").Length(2000);
            Property(x => x.Additional, "Поле, содержащее доп. информацию для формирование киентского field При ReportPara" +
                    "mType = Catalog, хранит идентификатор зарегистрированного справочника.").Column("ADDITIONAL").Length(250);
        }
    }

    public class ReportParamGkhNHibernateMapping : ClassMapping<ReportParamGkh>
    {
        public ReportParamGkhNHibernateMapping()
        {
            Property("DefaultValueBytes", mapper => mapper.Column("DEFAULT_VAL_BYTES"));
        }
    }
}
