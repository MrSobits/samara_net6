/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class EntityLogLightMap : BaseImportableEntityMap<EntityLogLight>
///     {
///         public EntityLogLightMap()
///             : base("GKH_ENTITY_LOG_LIGHT")
///         {
///             Map(x => x.ClassName, "CCLASS_NAME", true, 100);
///             Map(x => x.ClassDescription, "CCLASS_DESC", false, 200);
///             Map(x => x.PropertyName, "CPROP_NAME", true, 100);
///             Map(x => x.PropertyDescription, "CPROP_DESCR", false, 200);
///             Map(x => x.PropertyValue, "CPROP_VALUE", false, 1000);
///             Map(x => x.DateApplied, "CDATE_APPLIED", false);
///             Map(x => x.DateEnd, "CDATE_END", false);
///             Map(x => x.UsedInRecalculation, "USED_IN_RECALC", false);
///             Map(x => x.DateActualChange, "DATE_ACTUAL", true);
///             Map(x => x.EntityId, "ENTITY_ID", true);
///             Map(x => x.ParameterName, "PARAM_NAME", true);
///             Map(x => x.User, "CUSER_NAME", true);
///             Map(x => x.Reason, "REASON", false);
/// 
///             References(x => x.Document, "FILE_ID", ReferenceMapConfig.CascadeDelete);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.Gkh.Entities;

    /// <summary>Маппинг для "Легковесное сущность для хранения изменения сущности"</summary>
    public class EntityLogLightMap : BaseImportableEntityMap<EntityLogLight>
    {
        
        public EntityLogLightMap() : 
                base("Легковесное сущность для хранения изменения сущности", "GKH_ENTITY_LOG_LIGHT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.EntityId, "Идентификатор сущности").Column("ENTITY_ID").NotNull();
            Property(x => x.ClassName, "Класс сущности").Column("CCLASS_NAME").Length(100).NotNull();
            Property(x => x.ClassDescription, "Описание сущности").Column("CCLASS_DESC").Length(200);
            Property(x => x.PropertyName, "Имя свойства").Column("CPROP_NAME").Length(100).NotNull();
            Property(x => x.PropertyDescription, "Описание измененного атрибута").Column("CPROP_DESCR").Length(2000);
            Property(x => x.PropertyValue, "Значение свойства").Column("CPROP_VALUE").Length(1000);
            Property(x => x.DateApplied, "Дата поступления сведений об изменении значения").Column("CDATE_APPLIED");
            Property(x => x.DateActualChange, "Дата начала действия значения").Column("DATE_ACTUAL").NotNull();
            Property(x => x.DateEnd, "Дата окончания действия нового значения").Column("CDATE_END");
            Reference(x => x.Document, "Документ - основание").Column("FILE_ID");
            Property(x => x.ParameterName, "Наименование параметра").Column("PARAM_NAME").Length(250).NotNull();
            Property(x => x.User, "Пользователь").Column("CUSER_NAME").Length(250).NotNull();
            Property(x => x.UsedInRecalculation, "Использовалось при перерасчете").Column("USED_IN_RECALC");
            Property(x => x.Reason, "Причина").Column("REASON").Length(250);
        }
    }
}
