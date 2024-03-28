namespace Bars.GkhCr.LogMap
{
    using Bars.B4.Utils;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.GkhCr.Entities;

    class DefectListLogMap : AuditLogMap<DefectList>
    {
        public DefectListLogMap()
        {
            Name("Дефектная ведомость");

            Description(x => x.ReturnSafe(y => y.ObjectCr.RealityObject.Address));

            MapProperty(x => x.Work, "Work", "Вид работы", x => x.Return(y => y.Name));
            MapProperty(x => x.Volume, "Volume", "Объем по ведомости");
            MapProperty(x => x.CostPerUnitVolume, "CostPerUnitVolume", "Стоимость на единицу объема по ведомости");
            MapProperty(x => x.Sum, "Sum", "Общая стоимость работы по ведомости, руб");
        }
    }

}
