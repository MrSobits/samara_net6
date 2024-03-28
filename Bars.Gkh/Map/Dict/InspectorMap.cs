/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Инспекторы"
///     /// </summary>
///     public class InspectorMap : BaseGkhEntityMap<Inspector>
///     {
///         public InspectorMap()
///             : base("GKH_DICT_INSPECTOR")
///         {
///             Map(x => x.Code, "CODE").Length(300).Not.Nullable();
///             Map(x => x.Position, "POSITION").Length(300);
///             Map(x => x.Fio, "FIO").Length(300).Not.Nullable();
///             Map(x => x.ShortFio, "SHORTFIO").Length(100);
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///             Map(x => x.IsHead, "IS_HEAD").Not.Nullable();
///             Map(x => x.FioGenitive, "FIO_GENITIVE").Length(300);
///             Map(x => x.FioDative, "FIO_DATIVE").Length(300);
///             Map(x => x.FioAccusative, "FIO_ACCUSATIVE").Length(300);
///             Map(x => x.FioAblative, "FIO_ABLATIVE").Length(300);
///             Map(x => x.FioPrepositional, "FIO_PREPOSITIONAL").Length(300);
///             Map(x => x.PositionGenitive, "POSITION_GENITIVE").Length(300);
///             Map(x => x.PositionDative, "POSITION_DATIVE").Length(300);
///             Map(x => x.PositionAccusative, "POSITION_ACCUSATIVE").Length(300);
///             Map(x => x.PositionAblative, "POSITION_ABLATIVE").Length(300);
///             Map(x => x.PositionPrepositional, "POSITION_PREPOSITIONAL").Length(300);
///             Map(x => x.Phone, "PHONE").Length(300);
///             Map(x => x.Email, "EMAIL").Length(100);
///             //References(x => x.ZonalInspection, "ZON_INSP_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Инспектор"</summary>
    public class InspectorMap : BaseImportableEntityMap<Inspector>
    {
        
        public InspectorMap() : 
                base("Инспектор", "GKH_DICT_INSPECTOR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Code, "Код").Column("CODE").Length(300).NotNull();
            Property(x => x.Position, "Должность. Наименование").Column("POSITION").Length(300);
            Property(x => x.Fio, "ФИО").Column("FIO").Length(300).NotNull();
            Property(x => x.ShortFio, "Фамилия И.О.").Column("SHORTFIO").Length(100);
            Property(x => x.Description, "Description").Column("DESCRIPTION").Length(500);
            Property(x => x.IsHead, "Начальник").Column("IS_HEAD").NotNull();
            Property(x => x.FioGenitive, "ФИО Родительный падеж").Column("FIO_GENITIVE").Length(300);
            Property(x => x.FioDative, "ФИО Дательный падеж").Column("FIO_DATIVE").Length(300);
            Property(x => x.FioAccusative, "ФИО Винительный падеж").Column("FIO_ACCUSATIVE").Length(300);
            Property(x => x.FioAblative, "ФИО Творительный падеж").Column("FIO_ABLATIVE").Length(300);
            Property(x => x.FioPrepositional, "ФИО Предложный падеж").Column("FIO_PREPOSITIONAL").Length(300);
            Property(x => x.PositionGenitive, "Должность Родительный падеж").Column("POSITION_GENITIVE").Length(300);
            Property(x => x.PositionDative, "Должность Дательный падеж").Column("POSITION_DATIVE").Length(300);
            Property(x => x.PositionAccusative, "Должность Винительный падеж").Column("POSITION_ACCUSATIVE").Length(300);
            Property(x => x.PositionAblative, "Должность Творительный падеж").Column("POSITION_ABLATIVE").Length(300);
            Property(x => x.PositionPrepositional, "Должность Предложный падеж").Column("POSITION_PREPOSITIONAL").Length(300);
            Property(x => x.Phone, "Телефон").Column("PHONE").Length(300);
            Property(x => x.Email, "Электронный адрес").Column("EMAIL").Length(100);
            Property(x => x.ERKNMPositionGuid, "ЕРКНМ_GUID").Column("ERKNM_POSITION_GUID").Length(36);
            Property(x => x.ERKNMTitleSignerGuid, "TITLE_GUID").Column("ERKNM_TITLE_GUID").Length(36);
            Property(x => x.GisGkhGuid, "ГИС ЖКХ GUID").Column("GIS_GKH_GUID").Length(36);
            Property(x => x.IsActive, "Инспектор активен").Column("IS_ACTIVE");
            Reference(x => x.InspectorPosition, "Должность. Справочное значение").Column("INSPECTOR_POSITION");
        }
    }
}
