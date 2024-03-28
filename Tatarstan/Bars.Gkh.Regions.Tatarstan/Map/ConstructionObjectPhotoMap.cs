namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    public class ConstructionObjectPhotoMap : BaseEntityMap<ConstructionObjectPhoto>
    {
        public ConstructionObjectPhotoMap() :
            base("Фото-архив объекта строительства", "GKH_CONSTRUCT_OBJ_PHOTO")
        {      
        }

        protected override void Map()
        {
            this.Property(x => x.Date, "Дата изображения").Column("DATE");
			this.Property(x => x.Name, "Наименование").Column("NAME");
			this.Property(x => x.Group, "Группа").Column("PHOTO_GROUP");
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION");

			this.Reference(x => x.ConstructionObject, "Объект строительства").Column("OBJECT_ID").NotNull().Fetch();
			this.Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
		}
    }
}