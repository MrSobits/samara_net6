namespace Bars.Gkh.Map.Contragent
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    public class ActivityStageMap : GkhBaseEntityMap<ActivityStage>
    {
        public ActivityStageMap() : base("GKH_ACTIVITY_STAGE")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.EntityId, "Id сущности").Column("ENTITY_ID").NotNull();
            this.Property(x => x.EntityType, "Тип сущности").Column("ENYITY_TYPE").NotNull();
            this.Property(x => x.DateStart, "Дата начала").Column("DATE_START").NotNull();
            this.Property(x => x.DateEnd, "Дата окончания").Column("DATE_END");
            this.Property(x => x.ActivityStageType, "Стадия").Column("ACTIVITY_STAGE_TYPE").NotNull();
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION");

            this.Reference(x => x.Document, "Документ").Column("FILE_ID");
        }
    }
}