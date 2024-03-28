namespace Bars.GkhGji.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Dict;

    public class FactCheckingTypeMap : BaseEntityMap<FactCheckingType>
    {
        public FactCheckingTypeMap()
            : base("Вид проверки факта", "GJI_DICT_FACT_CHECKING_TYPE")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Code, "Код записи").Column("CODE").Length(3).NotNull();
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(100).NotNull();
        }
    }
}