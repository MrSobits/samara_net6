namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для "Основание обращение граждан ГЖИ"</summary>
    public class BaseStatementMap : JoinedSubClassMap<BaseStatement>
    {
        public BaseStatementMap()
            : base("Основание обращение граждан ГЖИ", "GJI_INSPECTION_STATEMENT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.TypeForm, "Форма проверки").Column("FORM_CHECK").NotNull();
            this.Property(x => x.RequestType, "Тип запроса").Column("REQUEST_TYPE");
            this.Property(x => x.Inn, "ИНН физ./долж. лица").Column("INN");
        }
    }
}