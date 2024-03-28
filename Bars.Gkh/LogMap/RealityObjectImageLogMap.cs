namespace Bars.Gkh.LogMap
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class RealityObjectImageLogMap : AuditLogMap<RealityObjectImage>
    {
        public RealityObjectImageLogMap()
        {
            Name("Фото-архив жилого дома");

            Description(x => x.Return(y => y.RealityObject).Return(y => y.Address));

            MapProperty(x => x.Name, "Name", "Наименование");
            MapProperty(x => x.Period, "Period", "Дата начала", x => x.Return(y => y.DateStart.ToShortDateString()));
            MapProperty(x => x.Period, "Period", "Дата окончания", x => x.Return(y => y.DateEnd.GetValueOrDefault().ToShortDateString()));
            MapProperty(x => x.WorkCr, "WorkCr", "Наименование работы", x => x.Return(y => y.Name));
            MapProperty(x => x.DateImage, "DateImage", "Дата изображения");
        }
    }
}
