namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhCr.Entities;


    /// <summary>Маппинг для "Акт выполненных работ"</summary>
    public class PerformedWorkActPhotoMap : BaseEntityMap<PerformedWorkActPhoto>
    {

        public PerformedWorkActPhotoMap() :
                base("Фото в акте выполненных работ", "CR_OBJ_PERFOMED_WORK_ACT_PHOTO")
        {
        }

        protected override void Map()
        {
            Reference(x => x.PerformedWorkAct, "Акт выполненных работ").Column("PERF_WORK_ACT_ID").NotNull();
            Reference(x => x.Photo, "Фото").Column("PHOTO").NotNull();
            Property(x => x.Name, "Название").Column("NAME").Length(100);
            Property(x => x.Discription, "Описание").Column("DISCRIPTION").Length(2000);
            Property(x => x.PhotoType, "Тип фото").Column("PHOTO_TYPE");
        }
    }
}
