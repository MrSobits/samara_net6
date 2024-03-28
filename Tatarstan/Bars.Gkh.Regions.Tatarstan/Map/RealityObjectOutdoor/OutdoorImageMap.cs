namespace Bars.Gkh.Regions.Tatarstan.Map.RealityObjectOutdoor
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.RealityObjectOutdoor;

    public class OutdoorImageMap : BaseEntityMap<OutdoorImage>
    {
        /// <inheritdoc />
        public OutdoorImageMap()
            : base(typeof(OutdoorImage).FullName, "GKH_OUTDOOR_IMAGE")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.DateImage, "Дата изображения").Column("DATE_IMAGE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(100);
            this.Property(x => x.ImagesGroup, "Группа").Column("IMAGES_GROUP");
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(300);
            this.Reference(x => x.Outdoor, "Двор").Column("OUTDOOR_ID").Fetch();
            this.Reference(x => x.Period, "Период").Column("PERIOD_ID").Fetch();
            this.Reference(x => x.WorkCr, "Вид работы").Column("WORK_ID").Fetch();
            this.Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
        }
    }
}