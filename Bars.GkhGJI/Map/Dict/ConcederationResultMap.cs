namespace Bars.GkhGji.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    public class ConcederationResultMap : BaseEntityMap<ConcederationResult>
    {
        public ConcederationResultMap()
            : base("Результат рассмотрения", "GJI_DICT_CONCEDERATION_RESULT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Code, "Код записи").Column("CODE").Length(3).NotNull();
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(100).NotNull();
        }
    }
}