namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Обращениям граждан - Запрос"</summary>
    public class PreventiveVisitMap : JoinedSubClassMap<PreventiveVisit>
    {
        
        public PreventiveVisitMap() : 
                base("Обращениям граждан - Акт профилактического визита", "GJI_PREVENTIVE_VISIT")
        {
        }
        
        protected override void Map()
        {    
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID");        
            Property(x => x.PhysicalPerson, "Физическое лицо").Column("PHYSICAL_PERSON").Length(300);
            Property(x => x.PhysicalPersonINN, "Физическое лицо").Column("PHYSICAL_PERSON_INN").Length(15);
            Property(x => x.AccessGuid, "GUID в ЕРКНМ").Column("ACCESSGUID").Length(100);
            Property(x => x.ActAddress, "Физическое лицо").Column("ACT_ADDRESS").Length(300);
            Property(x => x.TypePreventiveAct, "Тип основания акта").Column("TYPE_ACT").NotNull();
            Property(x => x.PhysicalPersonInfo, "Реквизиты физ. лица").Column("PHYSICAL_PERSON_INFO").Length(1500);
            Property(x => x.PhysicalPersonAddress, "Адрес физ. лица").Column("PHYSICAL_PERSON_ADDRESS").Length(1500);
            Property(x => x.PersonInspection, "Объект проверки").Column("PERSON_INSPECTION").NotNull();
            Property(x => x.UsedDistanceTech, "Дистационное мероприятие").Column("IS_DISTANCE");
            Property(x => x.DistanceDescription, "Описание дистационного мероприятия").Column("DISTANCE_DESCRIPTION");
            Property(x => x.DistanceCheckDate, "Дата дистационного мероприятия").Column("DISTANCE_DATE");
            Property(x => x.VideoLink, "Дистационное мероприятие").Column("VIDEOLINK");
            Property(x => x.ERKNMID, "Номер в ЕРКНМ").Column("ERKNMID").Length(100);
            Property(x => x.KindKND, "Вид контроля/надзора").Column("KIND_KND");
            Property(x => x.ERKNMGUID, "GUID в ЕРКНМ").Column("ERKNMGUID").Length(100);
            Property(x => x.SentToERKNM, "SentToERKNM").Column("IS_SENT").NotNull();
        }
    }
    
}
