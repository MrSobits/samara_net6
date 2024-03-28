namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    public class ConstructionObjectTypeWorkMap : BaseEntityMap<ConstructionObjectTypeWork>
    {
        public ConstructionObjectTypeWorkMap()
            : base("Объект строительства - Вид работы", "GKH_CONSTRUCT_OBJ_TYPEWORK")
        {
        }

        protected override void Map()
        {
            Reference(x => x.ConstructionObject, "Объект строительства").Column("OBJECT_ID").NotNull().Fetch();
            Reference(x => x.Work, "Вид работы").Column("WORK_ID").Fetch();
            Property(x => x.YearBuilding, "Год строительства").Column("YEAR_BUILDING").NotNull();
            Property(x => x.HasPsd, "Наличие ПСД").Column("HAS_PSD").NotNull();
            Property(x => x.HasExpertise, "Наличие ПСД").Column("HAS_EXPERTISE").NotNull();
            Property(x => x.Volume, "Объем (плановый)").Column("VOLUME");
            Property(x => x.Sum, "Сумма (плановая)").Column("SUM");
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);
            Property(x => x.DateStartWork, "Дата начала работ").Column("DATE_START_WORK");
            Property(x => x.DateEndWork, "Дата окончания работ").Column("DATE_END_WORK");
            Property(x => x.VolumeOfCompletion, "Объем выполнения").Column("VOLUME_COMPLETION");
            Property(x => x.PercentOfCompletion, "Процент выполнения").Column("PERCENT_COMPLETION");
            Property(x => x.CostSum, "Сумма расходов").Column("COST_SUM");
            Property(x => x.CountWorker, "Чиленность рабочих(дробное так как м.б. смысл поля как пол ставки)").Column("COUNT_WORKER");
            Property(x => x.Deadline, "Контрольный срок").Column("DEADLINE");
        }
    }
}