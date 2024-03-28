namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Маппинг для "Мероприятия в доме для акта без взаимодействия"
    /// </summary>
    public class ActIsolatedRealObjEventMap : BaseEntityMap<ActIsolatedRealObjEvent>
    {
        public ActIsolatedRealObjEventMap() : 
                base("Мероприятия в доме для акта без взаимодействия", "GJI_ACTISOLATED_ROBJECT_EVENT")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование мероприятия").Column("NAME").Length(300).NotNull();
            this.Property(x => x.Term, "Срок проведения мероприятия (в днях)").Column("TERM").NotNull();
            this.Property(x => x.DateStart, "Дата начала").Column("DATE_START").NotNull();
            this.Property(x => x.DateEnd, "Дата окончания").Column("DATE_END").NotNull();
            this.Reference(x => x.ActIsolatedRealObj, "Дом акта без взаимодействия").Column("ACTISOLATED_RO_ID").NotNull().Fetch();
        }
    }
}