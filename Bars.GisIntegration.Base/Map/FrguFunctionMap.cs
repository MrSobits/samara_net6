namespace Bars.GisIntegration.Base.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Base.Entities;

    public class FrguFunctionMap : BaseEntityMap<FrguFunction>
    {
        public FrguFunctionMap() : 
            base("Bars.GisIntegration.Base.Entities.FrguFunction", "GI_FRGU_FUNCTION")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование контрольно-надзорной функции из ФРГУ").Column("NAME");
            this.Property(x => x.FrguId, "Идентификатор контрольно-надзорной функции из ФРГУ").Column("FRGU_ID");
            this.Property(x => x.Guid, "Идентификатор контрольно-надзорной функции формата GUID").Column("GUID");
        }
    }
}