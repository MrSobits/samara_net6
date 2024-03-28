namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    public class ProdCalendarMap : BaseEntityMap<ProdCalendar>
    {
        public ProdCalendarMap()
            : base("Справочник Производственный календарь", "GJI_DICT_PROD_CALENDAR")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME");
            this.Property(x => x.ProdDate, "Дата").Column("PROD_DATE");
        }
    }
}